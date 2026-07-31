<?php
// --- REST API 설계: 엔드포인트, 상태 코드, 버저닝 (미니 라우터) ---

echo "=== 1. HTTP 요청/응답 객체 ===\n";

class ApiRequest {
    public function __construct(
        public string $method,
        public string $path,
        public array $query = [],
        public array $body = []
    ) {}

    public function describe(): string {
        return "{$this->method} {$this->path}";
    }
}

class ApiResponse {
    public function __construct(
        public int $status,
        public array $data = []
    ) {}

    public function toHttpText(): string {
        $reason = self::reasonPhrase($this->status);
        $body = json_encode($this->data, JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        return "HTTP/1.1 {$this->status} $reason\n"
            . "Content-Type: application/json; charset=utf-8\n\n"
            . $body;
    }

    public static function reasonPhrase(int $code): string {
        return match ($code) {
            200 => 'OK',
            201 => 'Created',
            204 => 'No Content',
            400 => 'Bad Request',
            401 => 'Unauthorized',
            403 => 'Forbidden',
            404 => 'Not Found',
            405 => 'Method Not Allowed',
            409 => 'Conflict',
            422 => 'Unprocessable Entity',
            500 => 'Internal Server Error',
            default => '',
        };
    }
}

echo "상태 코드 예시:\n";
foreach ([200, 201, 204, 400, 401, 403, 404, 405, 409, 422, 500] as $code) {
    echo "  $code " . ApiResponse::reasonPhrase($code) . "\n";
}

echo "\n=== 2. 인메모리 저장소 ===\n";

class TaskRepository {
    private static array $tasks = [
        ['id' => 1, 'title' => 'REST API 설계', 'completed' => true],
        ['id' => 2, 'title' => '미니 라우터 구현', 'completed' => false],
    ];
    private static int $nextId = 3;

    public static function all(): array {
        return self::$tasks;
    }

    public static function find(int $id): ?array {
        foreach (self::$tasks as $task) {
            if ($task['id'] === $id) {
                return $task;
            }
        }
        return null;
    }

    public static function create(string $title): array {
        $task = ['id' => self::$nextId++, 'title' => $title, 'completed' => false];
        self::$tasks[] = $task;
        return $task;
    }

    public static function update(int $id, array $data): ?array {
        foreach (self::$tasks as &$task) {
            if ($task['id'] === $id) {
                if (isset($data['title'])) {
                    $task['title'] = $data['title'];
                }
                if (array_key_exists('completed', $data)) {
                    $task['completed'] = (bool)$data['completed'];
                }
                return $task;
            }
        }
        return null;
    }

    public static function delete(int $id): bool {
        foreach (self::$tasks as $i => $task) {
            if ($task['id'] === $id) {
                array_splice(self::$tasks, $i, 1);
                return true;
            }
        }
        return false;
    }
}

echo "초기 데이터: " . count(TaskRepository::all()) . "개\n\n";

echo "=== 3. RESTful 라우터 (버저닝 포함) ===\n\n";

class ApiRouter {
    private array $routes = [];
    private array $supportedVersions = ['v1'];

    public function add(string $method, string $pattern, callable $handler): void {
        $this->routes[] = ['method' => $method, 'pattern' => $pattern, 'handler' => $handler];
    }

    public function dispatch(ApiRequest $request): ApiResponse {
        // URL 버저닝: /v1/tasks → version = v1
        $segments = explode('/', trim($request->path, '/'));
        $version = array_shift($segments) ?? '';
        $path = '/' . implode('/', $segments);

        if (!in_array($version, $this->supportedVersions, true)) {
            return new ApiResponse(404, ['error' => '지원하지 않는 API 버전', 'version' => $version]);
        }

        $pathMatched = false;
        foreach ($this->routes as $route) {
            $pattern = preg_replace('/\{([a-zA-Z_][a-zA-Z0-9_]*)\}/', '([^/]+)', $route['pattern']);
            if (!preg_match('#^' . $pattern . '$#', $path, $matches)) {
                continue;
            }

            $pathMatched = true;
            if ($route['method'] !== $request->method) {
                continue;
            }

            array_shift($matches);
            return call_user_func($route['handler'], $request, $version, $matches);
        }

        if ($pathMatched) {
            return new ApiResponse(405, ['error' => 'Method Not Allowed', 'method' => $request->method]);
        }

        return new ApiResponse(404, ['error' => 'Not Found', 'path' => $request->path]);
    }
}

$router = new ApiRouter();

// GET /v1/tasks — 목록
$router->add('GET', '/tasks', function (ApiRequest $req, string $version, array $params): ApiResponse {
    $tasks = TaskRepository::all();
    return new ApiResponse(200, ['data' => $tasks, 'meta' => ['count' => count($tasks)]]);
});

// GET /v1/tasks/{id} — 단건 조회
$router->add('GET', '/tasks/{id}', function (ApiRequest $req, string $version, array $params): ApiResponse {
    $task = TaskRepository::find((int)$params[0]);
    return $task
        ? new ApiResponse(200, ['data' => $task])
        : new ApiResponse(404, ['error' => 'Task not found', 'id' => (int)$params[0]]);
});

// POST /v1/tasks — 생성
$router->add('POST', '/tasks', function (ApiRequest $req, string $version, array $params): ApiResponse {
    $title = trim((string)($req->body['title'] ?? ''));
    if ($title === '') {
        return new ApiResponse(422, ['error' => 'title 필드는 비어 있을 수 없습니다.']);
    }
    return new ApiResponse(201, ['data' => TaskRepository::create($title)]);
});

// PATCH /v1/tasks/{id} — 부분 수정
$router->add('PATCH', '/tasks/{id}', function (ApiRequest $req, string $version, array $params): ApiResponse {
    $task = TaskRepository::update((int)$params[0], $req->body);
    return $task
        ? new ApiResponse(200, ['data' => $task])
        : new ApiResponse(404, ['error' => 'Task not found', 'id' => (int)$params[0]]);
});

// DELETE /v1/tasks/{id} — 삭제
$router->add('DELETE', '/tasks/{id}', function (ApiRequest $req, string $version, array $params): ApiResponse {
    return TaskRepository::delete((int)$params[0])
        ? new ApiResponse(204, ['data' => null])
        : new ApiResponse(404, ['error' => 'Task not found', 'id' => (int)$params[0]]);
});

// --- 데모 시나리오 ---
$requests = [
    new ApiRequest('GET', '/v1/tasks'),
    new ApiRequest('GET', '/v1/tasks/2'),
    new ApiRequest('GET', '/v1/tasks/999'),
    new ApiRequest('POST', '/v1/tasks', body: ['title' => '새 작업 추가']),
    new ApiRequest('POST', '/v1/tasks', body: ['title' => '   ']),
    new ApiRequest('PATCH', '/v1/tasks/1', body: ['completed' => false]),
    new ApiRequest('DELETE', '/v1/tasks/2'),
    new ApiRequest('DELETE', '/v1/tasks'),        // 405
    new ApiRequest('GET', '/v2/tasks'),           // 404 (버전 미지원)
    new ApiRequest('GET', '/v1/unknown'),         // 404
];

foreach ($requests as $request) {
    echo "▶ {$request->describe()}\n";
    echo $router->dispatch($request)->toHttpText() . "\n\n";
}
