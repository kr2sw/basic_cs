<?php
// --- Symfony 컴포넌트 개념: Console, Routing (미니 구현) ---

echo "=== 1. Console 컴포넌트: 미니 커맨드 앱 ===\n\n";

// Input / Output 객체
class Input {
    public function __construct(private array $arguments = []) {}

    // 인자는 위치 기반: 0번째=name, 1번째=value
    public function getArgument(string $name): ?string {
        $index = match ($name) {
            'name' => 0,
            'value' => 1,
            default => null,
        };
        return $index !== null ? ($this->arguments[$index] ?? null) : null;
    }

    public function hasOption(string $name): bool {
        return in_array("--$name", $this->arguments, true);
    }
}

class Output {
    public function writeln(string $text = ''): void {
        echo $text . "\n";
    }

    public function info(string $text): void {
        echo "\033[32m$text\033[0m\n";
    }

    public function error(string $text): void {
        echo "\033[31m$text\033[0m\n";
    }
}

// Command 추상 클래스 (Symfony Console의 Command와 동일 구조)
abstract class Command {
    protected string $name = '';
    protected string $description = '';

    abstract protected function execute(Input $input, Output $output): int;

    public function getName(): string {
        return $this->name;
    }

    public function getDescription(): string {
        return $this->description;
    }

    public function run(Input $input, Output $output): int {
        return $this->execute($input, $output);
    }
}

class HelloCommand extends Command {
    protected string $name = 'app:hello';
    protected string $description = '인사말을 출력합니다';

    protected function execute(Input $input, Output $output): int {
        $name = $input->getArgument('name') ?? 'World';
        $output->info("안녕하세요, $name님!");
        return 0;
    }
}

class DateCommand extends Command {
    protected string $name = 'app:date';
    protected string $description = '현재 날짜/시간을 출력합니다 (--timezone 옵션)';

    protected function execute(Input $input, Output $output): int {
        if ($input->hasOption('timezone')) {
            $tz = $input->getArgument('name') ?? 'UTC';
            if (!in_array($tz, timezone_identifiers_list(), true)) {
                $output->error("잘못된 타임존: $tz");
                return 1;
            }
            date_default_timezone_set($tz);
        }
        $output->writeln('현재 시각: ' . date('Y-m-d H:i:s'));
        return 0;
    }
}

class Application {
    private array $commands = [];

    public function add(Command $command): void {
        $this->commands[$command->getName()] = $command;
    }

    public function getCommands(): array {
        return $this->commands;
    }

    // bin/console <명령어> <인자...>
    public function run(array $argv = []): int {
        $output = new Output();
        $commandName = $argv[0] ?? null;

        if ($commandName === null || in_array($commandName, ['list', 'help'], true)) {
            $output->writeln('사용 가능한 명령어:');
            foreach ($this->commands as $name => $command) {
                $output->writeln("  $name");
                $output->writeln("    {$command->getDescription()}");
            }
            return 0;
        }

        if (!isset($this->commands[$commandName])) {
            $output->error("명령어를 찾을 수 없습니다: $commandName");
            return 1;
        }

        $input = new Input(array_slice($argv, 1));
        return $this->commands[$commandName]->run($input, $output);
    }
}

$app = new Application();
$app->add(new HelloCommand());
$app->add(new DateCommand());

// 명령어 실행 시뮬레이션
$app->run([]);
echo "\n";
$app->run(['app:hello', 'Alice']);
echo "\n";
$app->run(['app:date']);
echo "\n";
$app->run(['unknown:command']);
echo "\n";

echo "=== 2. Routing 컴포넌트: 라우트 매칭 ===\n\n";

class Route {
    public function __construct(
        public string $path,
        public string $controller,
        public array $requirements = [],
        public array $methods = ['GET']
    ) {}
}

class RouteCollection {
    private array $routes = [];

    public function add(string $name, Route $route): void {
        $this->routes[$name] = $route;
    }

    public function all(): array {
        return $this->routes;
    }
}

class UrlMatcher {
    public function __construct(private RouteCollection $routes) {}

    public function match(string $method, string $path): array {
        foreach ($this->routes->all() as $name => $route) {
            if (!in_array($method, $route->methods, true)) {
                continue;
            }

            // {id} 같은 플레이스홀더를 캡처 그룹으로 변환
            $pattern = preg_replace('/\{([a-zA-Z_][a-zA-Z0-9_]*)\}/', '([^/]+)', $route->path);
            $regex = '#^' . $pattern . '$#';

            if (preg_match($regex, $path, $matches)) {
                array_shift($matches);
                preg_match_all('/\{([a-zA-Z_][a-zA-Z0-9_]*)\}/', $route->path, $names);

                $parameters = [];
                foreach ($names[1] as $i => $name) {
                    // requirements 제약 검사 (예: id는 숫자만)
                    $value = $matches[$i];
                    if (isset($route->requirements[$name])) {
                        $req = '#^' . $route->requirements[$name] . '$#';
                        if (!preg_match($req, $value)) {
                            continue 2;
                        }
                    }
                    $parameters[$name] = $value;
                }

                return [
                    '_route' => $name,
                    '_controller' => $route->controller,
                    'parameters' => $parameters,
                ];
            }
        }

        throw new RuntimeException("라우트를 찾을 수 없습니다: $method $path");
    }
}

$routes = new RouteCollection();
$routes->add('home', new Route('/', 'App\Controller\HomeController::index'));
$routes->add('user_show', new Route('/users/{id}', 'App\Controller\UserController::show', ['id' => '\d+'], ['GET']));
$routes->add('user_posts', new Route('/users/{id}/posts', 'App\Controller\UserController::posts'));
$routes->add('blog_show', new Route('/blog/{slug}', 'App\Controller\BlogController::show'));

$matcher = new UrlMatcher($routes);

foreach (['GET /', 'GET /users/42', 'GET /users/abc', 'GET /blog/php-tips', 'POST /users/42'] as $line) {
    [$method, $path] = explode(' ', $line);
    try {
        $match = $matcher->match($method, $path);
        echo "  $line\n";
        echo "    → {$match['_controller']}  파라미터: "
            . json_encode($match['parameters'], JSON_UNESCAPED_UNICODE) . "\n";
    } catch (RuntimeException $e) {
        echo "  $line\n    → [404] " . $e->getMessage() . "\n";
    }
}
