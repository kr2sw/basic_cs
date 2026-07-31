<?php
// --- 종합 프로젝트: CLI 기반 작업 관리 앱 ---
// 사용법: php index.php [list | add "할 일" | done ID | remove ID | clear | help]

// 1. 엔티티
class Task {
    public function __construct(
        public int $id,
        public string $title,
        public bool $done = false,
        public string $createdAt = ''
    ) {
        if ($this->createdAt === '') {
            $this->createdAt = date('Y-m-d H:i:s');
        }
    }
}

// 2. 저장소 (JSON 파일 영속성)
class TaskRepository {
    private string $file;
    private array $tasks = [];

    public function __construct(string $file) {
        $this->file = $file;
        $this->load();
    }

    private function load(): void {
        if (!is_file($this->file)) {
            return;
        }
        $data = json_decode((string)file_get_contents($this->file), true);
        foreach ($data ?? [] as $row) {
            $this->tasks[] = new Task(
                $row['id'],
                $row['title'],
                (bool)$row['done'],
                $row['created_at']
            );
        }
    }

    private function save(): void {
        $data = array_map(fn(Task $t) => [
            'id' => $t->id,
            'title' => $t->title,
            'done' => $t->done,
            'created_at' => $t->createdAt,
        ], $this->tasks);
        file_put_contents($this->file, json_encode($data, JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT));
    }

    public function all(): array {
        return $this->tasks;
    }

    public function find(int $id): ?Task {
        foreach ($this->tasks as $task) {
            if ($task->id === $id) {
                return $task;
            }
        }
        return null;
    }

    public function add(string $title): Task {
        $nextId = $this->tasks === []
            ? 1
            : max(array_map(fn(Task $t) => $t->id, $this->tasks)) + 1;

        $task = new Task($nextId, $title);
        $this->tasks[] = $task;
        $this->save();
        return $task;
    }

    public function toggle(int $id): ?Task {
        $task = $this->find($id);
        if ($task !== null) {
            $task->done = !$task->done;
            $this->save();
        }
        return $task;
    }

    public function remove(int $id): bool {
        foreach ($this->tasks as $i => $task) {
            if ($task->id === $id) {
                array_splice($this->tasks, $i, 1);
                $this->save();
                return true;
            }
        }
        return false;
    }

    public function clear(): void {
        $this->tasks = [];
        $this->save();
    }
}

// 3. 앱 (명령 처리 + 출력)
class TaskApp {
    public function __construct(private TaskRepository $repo) {}

    public function help(): void {
        echo "작업 관리 앱 (CLI)\n";
        echo str_repeat('-', 50) . "\n";
        echo "  php index.php                 작업 목록 (기본)\n";
        echo "  php index.php list            작업 목록\n";
        echo "  php index.php add \"할 일\"     작업 추가\n";
        echo "  php index.php done 1          완료/진행 토글\n";
        echo "  php index.php remove 1        작업 삭제\n";
        echo "  php index.php clear           전체 삭제\n";
        echo "  php index.php help            도움말\n";
    }

    public function list(): void {
        $tasks = $this->repo->all();

        if ($tasks === []) {
            echo "작업이 없습니다. add 명령으로 추가하세요.\n";
            return;
        }

        echo str_pad('ID', 4) . str_pad('상태', 8) . str_pad('생성일', 20) . "제목\n";
        echo str_repeat('-', 60) . "\n";

        foreach ($tasks as $task) {
            $status = $task->done ? '완료' : '진행중';
            printf("%-4d %-8s %-20s %s\n", $task->id, $status, $task->createdAt, $task->title);
        }

        $doneCount = count(array_filter($tasks, fn(Task $t) => $t->done));
        echo str_repeat('-', 60) . "\n";
        printf("총 %d개 중 %d개 완료\n", count($tasks), $doneCount);
    }

    public function add(string $title): void {
        if (trim($title) === '') {
            echo "제목이 비어 있습니다.\n";
            return;
        }
        $task = $this->repo->add(trim($title));
        echo "추가됨: [#{$task->id}] {$task->title}\n";
    }

    public function toggle(int $id): void {
        $task = $this->repo->toggle($id);
        if ($task === null) {
            echo "작업을 찾을 수 없습니다: #$id\n";
            return;
        }
        $status = $task->done ? '완료' : '진행중';
        echo "업데이트: [#{$task->id}] {$task->title} → $status\n";
    }

    public function remove(int $id): void {
        if ($this->repo->remove($id)) {
            echo "삭제됨: #$id\n";
        } else {
            echo "작업을 찾을 수 없습니다: #$id\n";
        }
    }

    public function clear(): void {
        $this->repo->clear();
        echo "모든 작업을 삭제했습니다.\n";
    }
}

// 4. 진입점
$dataFile = sys_get_temp_dir() . '/php-task-app/tasks.json';
if (!is_dir(dirname($dataFile))) {
    mkdir(dirname($dataFile), 0777, true);
}

$repo = new TaskRepository($dataFile);
$app = new TaskApp($repo);

$command = $argv[1] ?? 'list';
$argument = $argv[2] ?? null;

match ($command) {
    'list' => $app->list(),
    'add' => $app->add((string)$argument),
    'done' => $app->toggle((int)$argument),
    'remove' => $app->remove((int)$argument),
    'clear' => $app->clear(),
    'help', '--help', '-h' => $app->help(),
    default => $app->help(),
};
