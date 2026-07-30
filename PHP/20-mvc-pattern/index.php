<?php
// --- 간단한 MVC 프레임워크 ---

// Router
class Router {
    private array $routes = [];

    public function get(string $path, callable $handler): void {
        $this->routes['GET'][$path] = $handler;
    }

    public function post(string $path, callable $handler): void {
        $this->routes['POST'][$path] = $handler;
    }

    public function dispatch(string $method, string $uri): void {
        $path = parse_url($uri, PHP_URL_PATH);
        $path = rtrim($path, '/') ?: '/';

        $handler = $this->routes[$method][$path] ?? null;

        if ($handler) {
            call_user_func($handler);
        } else {
            http_response_code(404);
            View::render('errors/404', ['path' => $path]);
        }
    }
}

// Model
class TaskModel {
    private array $tasks;

    public function __construct() {
        $this->tasks = [
            ['id' => 1, 'title' => 'PHP 공부하기', 'done' => false],
            ['id' => 2, 'title' => 'MVC 이해하기', 'done' => true],
            ['id' => 3, 'title' => '예제 작성하기', 'done' => false],
        ];
    }

    public function getAll(): array {
        return $this->tasks;
    }

    public function getById(int $id): ?array {
        foreach ($this->tasks as $task) {
            if ($task['id'] === $id) return $task;
        }
        return null;
    }

    public function add(string $title): array {
        $task = [
            'id' => count($this->tasks) + 1,
            'title' => $title,
            'done' => false,
        ];
        $this->tasks[] = $task;
        return $task;
    }

    public function toggle(int $id): bool {
        foreach ($this->tasks as &$task) {
            if ($task['id'] === $id) {
                $task['done'] = !$task['done'];
                return true;
            }
        }
        return false;
    }
}

// View
class View {
    public static function render(string $template, array $data = []): void {
        extract($data);

        echo "<!DOCTYPE html><html lang='ko'><head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>PHP MVC</title>
            <style>
                * { margin: 0; padding: 0; box-sizing: border-box; }
                body { font-family: Arial; max-width: 800px; margin: 40px auto; padding: 20px; }
                h1 { color: #333; border-bottom: 2px solid #4CAF50; padding-bottom: 10px; }
                ul { list-style: none; margin-top: 20px; }
                li { padding: 12px; background: #f9f9f9; margin: 5px 0; border-radius: 4px; }
                li.done { text-decoration: line-through; color: #999; }
                .btn { display: inline-block; padding: 6px 12px; text-decoration: none;
                       background: #4CAF50; color: white; border-radius: 4px; font-size: 14px; }
                .btn-danger { background: #f44336; }
                form { margin-top: 20px; display: flex; gap: 8px; }
                input[type=text] { flex: 1; padding: 8px; border: 1px solid #ddd; border-radius: 4px; }
                input[type=submit] { padding: 8px 16px; background: #4CAF50; color: white;
                                    border: none; border-radius: 4px; cursor: pointer; }
                .error { color: #f44336; margin-top: 20px; }
            </style>
        </head><body>";

        switch ($template) {
            case 'tasks/index':
                echo "<h1>📋 할 일 목록</h1>";
                echo "<ul>";
                foreach ($tasks as $task) {
                    $class = $task['done'] ? "class='done'" : "";
                    echo "<li $class>";
                    echo "{$task['title']} ";
                    echo "<a href='?action=toggle&id={$task['id']}' class='btn'>"
                        . ($task['done'] ? '취소' : '완료') . "</a>";
                    echo "</li>";
                }
                echo "</ul>";
                echo "<form method='POST'>
                    <input type='text' name='title' placeholder='새 할 일 입력' required>
                    <input type='submit' value='추가'>
                </form>";
                break;

            case 'errors/404':
                echo "<h1>404 Not Found</h1>";
                echo "<p>요청한 경로 '<strong>{$path}</strong>'를 찾을 수 없습니다.</p>";
                echo "<a href='/' class='btn'>홈으로</a>";
                break;
        }

        echo "</body></html>";
    }
}

// Controller
class TaskController {
    private TaskModel $model;

    public function __construct() {
        $this->model = new TaskModel();
    }

    public function index(): void {
        $tasks = $this->model->getAll();
        View::render('tasks/index', ['tasks' => $tasks]);
    }

    public function add(): void {
        $title = $_POST['title'] ?? '';
        if (!empty(trim($title))) {
            $this->model->add(trim($title));
        }
        header('Location: /');
        exit;
    }

    public function toggle(): void {
        $id = (int)($_GET['id'] ?? 0);
        $this->model->toggle($id);
        header('Location: /');
        exit;
    }
}

// --- 애플리케이션 진입점 ---

$router = new Router();
$controller = new TaskController();

// 라우트 정의
$router->get('/', [$controller, 'index']);
$router->post('/', [$controller, 'add']);
$router->get('/toggle', [$controller, 'toggle']);

// 요청 처리
$method = $_SERVER['REQUEST_METHOD'] ?? 'GET';
$uri = $_SERVER['REQUEST_URI'] ?? '/';

echo "<!-- PHP MVC 패턴 예제 -->\n";

$router->dispatch($method, $uri);

echo "\n<!--\n";
echo "처리 흐름:\n";
echo "  Request ({$method} {$uri})\n";
echo "  → Router\n";
echo "  → Controller\n";
echo "  → Model (데이터)\n";
echo "  → View (렌더링)\n";
echo "  → Response (HTML)\n";
echo "-->\n";
