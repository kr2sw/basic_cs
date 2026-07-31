<?php
// --- Laravel 기초: 라우팅, 컨트롤러, 블레이드 (미니 시뮬레이션) ---

echo "=== 1. 프로젝트 생성 ===\n";
echo "  composer create-project laravel/laravel my-app \"^11.0\"\n";
echo "  cd my-app\n";
echo "  php artisan serve   → http://127.0.0.1:8000\n\n";

echo "=== 2. 블레이드 템플릿 엔진 (미니 구현) ===\n\n";

// 블레이드 문법 → PHP 변환 후 실행
function blade(string $template, array $data): string {
    extract($data, EXTR_SKIP);

    // {{ $var }} → 이스케이프 출력
    $template = preg_replace(
        '/\{\{\s*(.+?)\s*\}\}/',
        '<?php echo htmlspecialchars($1, ENT_QUOTES, "UTF-8"); ?>',
        $template
    );

    // 블레이드 디렉티브 → PHP 대체 문법 (if(...): endif; 등)
    $template = preg_replace('/@foreach\((.*?)\)/', '<?php foreach ($1): ?>', $template);
    $template = str_replace('@endforeach', '<?php endforeach; ?>', $template);
    $template = preg_replace('/@if\((.*?)\)/', '<?php if ($1): ?>', $template);
    $template = preg_replace('/@elseif\((.*?)\)/', '<?php elseif ($1): ?>', $template);
    $template = str_replace('@else', '<?php else: ?>', $template);
    $template = str_replace('@endif', '<?php endif; ?>', $template);

    ob_start();
    eval('?>' . $template);
    return ob_get_clean();
}

// 가상의 resources/views 파일들
$GLOBALS['__views'] = [
    'users/index' => <<<'BLADE'
<!DOCTYPE html>
<html lang="ko">
<head><meta charset="UTF-8"><title>사용자 목록</title></head>
<body>
<h1>사용자 목록</h1>
<ul>
@foreach($users as $id => $user)
    <li>#{{ $id }} {{ $user['name'] }} — {{ $user['email'] }}</li>
@endforeach
</ul>
</body>
</html>
BLADE,

    'users/show' => <<<'BLADE'
<h1>사용자 상세 (#{{ $id }})</h1>
@if($user)
    <p>이름: <strong>{{ $user['name'] }}</strong></p>
    <p>이메일: {{ $user['email'] }}</p>
@else
    <p>존재하지 않는 사용자입니다.</p>
@endif
BLADE,
];

function view(string $view, array $data): string {
    $template = $GLOBALS['__views'][$view] ?? "뷰를 찾을 수 없습니다: $view";
    return blade($template, $data);
}

echo "=== 3. 라우팅 + 컨트롤러 ===\n\n";

class Route {
    private static array $routes = [];

    public static function get(string $uri, callable|array $action): void {
        self::register('GET', $uri, $action);
    }

    public static function post(string $uri, callable|array $action): void {
        self::register('POST', $uri, $action);
    }

    private static function register(string $method, string $uri, callable|array $action): void {
        self::$routes[$method][$uri] = $action;
    }

    public static function dispatch(string $method, string $uri): mixed {
        $path = rtrim(parse_url($uri, PHP_URL_PATH), '/') ?: '/';

        foreach (self::$routes[$method] ?? [] as $route => $action) {
            // {id} 플레이스홀더 → 캡처 그룹
            $pattern = preg_replace('/\{([a-zA-Z_][a-zA-Z0-9_]*)\}/', '([^/]+)', $route);

            if (preg_match('#^' . $pattern . '$#', $path, $matches)) {
                array_shift($matches);
                preg_match_all('/\{([a-zA-Z_][a-zA-Z0-9_]*)\}/', $route, $names);

                $params = [];
                foreach ($names[1] as $i => $name) {
                    $params[$name] = $matches[$i];
                }

                if (is_array($action)) {
                    [$class, $methodName] = $action;
                    $controller = new $class();
                    return $controller->$methodName(...array_values($params));
                }

                return call_user_func($action, ...array_values($params));
            }
        }

        return "404 Not Found: $method $path";
    }
}

class UserController {
    private array $users = [
        1 => ['name' => 'Alice', 'email' => 'alice@example.com'],
        2 => ['name' => 'Bob', 'email' => 'bob@example.com'],
    ];

    public function index(): string {
        return view('users/index', ['users' => $this->users]);
    }

    public function show(int $id): string {
        $user = $this->users[$id] ?? null;
        return view('users/show', ['user' => $user, 'id' => $id]);
    }

    public function store(): string {
        // 실제로는 Request 객체에서 입력값을 받음
        return json_encode([
            'status' => 'created',
            'message' => '새 사용자 생성 요청 (POST)',
            'hint' => 'Request::validate(), User::create()',
        ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    }
}

// routes/web.php
Route::get('/', [UserController::class, 'index']);
Route::get('/users', [UserController::class, 'index']);
Route::get('/users/{id}', [UserController::class, 'show']);
Route::post('/users', [UserController::class, 'store']);

foreach ([['GET', '/users'], ['GET', '/users/1'], ['GET', '/users/99'], ['POST', '/users']] as [$method, $uri]) {
    echo "Request: $method $uri\n";
    echo str_repeat('-', 40) . "\n";
    echo Route::dispatch($method, $uri) . "\n\n";
}

echo "=== 4. 아티즌 명령어 (개념) ===\n";
echo "  php artisan list\n";
echo "  php artisan make:controller UserController\n";
echo "  php artisan make:model User -m\n";
echo "  php artisan migrate\n";
echo "  php artisan route:list\n";
echo "  php artisan tinker\n";
