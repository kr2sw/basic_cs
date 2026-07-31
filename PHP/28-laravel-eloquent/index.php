<?php
// --- Eloquent ORM 개념: 모델, 관계 (1:N, N:M) 인메모리 시뮬레이션 ---

// 엘로퀀트 스타일 베이스 모델 (인메모리 버전)
abstract class Model {
    protected static array $tableData = [];
    protected static array $fillable = [];
    protected static int $autoIncrement = 0;

    protected array $attributes = [];

    public function __construct(array $attributes = []) {
        $this->fill($attributes);
    }

    // $fillable에 있는 키만 대량 할당 (Mass Assignment 보호)
    public function fill(array $attributes): void {
        foreach ($attributes as $key => $value) {
            if (in_array($key, static::$fillable, true)) {
                $this->setAttribute($key, $value);
            }
        }
    }

    public function setAttribute(string $key, mixed $value): void {
        // 뮤테이터: setPasswordAttribute 등
        $mutator = 'set' . str_replace('_', '', ucwords($key, '_')) . 'Attribute';
        $this->attributes[$key] = method_exists($this, $mutator)
            ? $this->$mutator($value)
            : $value;
    }

    public function getAttribute(string $key): mixed {
        // 접근자: getFullNameAttribute 등
        $accessor = 'get' . str_replace('_', '', ucwords($key, '_')) . 'Attribute';
        if (method_exists($this, $accessor)) {
            return $this->$accessor();
        }
        return $this->attributes[$key] ?? null;
    }

    public function __get(string $key): mixed {
        return $this->getAttribute($key);
    }

    public function __set(string $key, mixed $value): void {
        $this->setAttribute($key, $value);
    }

    // 쿼리 빌더 시뮬레이션 (실제로는 DB 조회)
    public static function all(): array {
        return static::$tableData;
    }

    public static function find(int $id): ?static {
        return static::$tableData[$id] ?? null;
    }

    public static function create(array $attributes): static {
        $model = new static($attributes);
        static::$autoIncrement++;
        $model->attributes['id'] = static::$autoIncrement;
        static::$tableData[static::$autoIncrement] = $model;
        return $model;
    }

    public function update(array $attributes): static {
        $this->fill($attributes);
        return $this;
    }

    public function delete(): bool {
        unset(static::$tableData[$this->attributes['id']]);
        return true;
    }

    public function toArray(): array {
        return $this->attributes;
    }

    // --- 관계 헬퍼 ---

    // 1:N — $this가 1, $related가 N
    protected function hasMany(string $related, string $foreignKey): array {
        $id = $this->attributes['id'];
        return array_values(array_filter(
            $related::all(),
            fn(Model $m) => $m->getAttribute($foreignKey) === $id
        ));
    }

    // 1:N의 역방향 — $this가 N
    protected function belongsTo(string $related, string $foreignKey): ?Model {
        $fk = $this->attributes[$foreignKey] ?? null;
        return $fk !== null ? $related::find($fk) : null;
    }
}

// --- 모델 정의 ---

class Company extends Model {
    protected static array $tableData = [];
    protected static array $fillable = ['name'];
}

class User extends Model {
    protected static array $tableData = [];
    protected static array $fillable = ['name', 'email', 'password', 'company_id'];

    // 1:N — 사용자는 여러 글을 가짐
    public function posts(): array {
        return $this->hasMany(Post::class, 'user_id');
    }

    // belongsTo — 사용자는 한 회사에 속함
    public function company(): ?Model {
        return $this->belongsTo(Company::class, 'company_id');
    }

    // N:M — 사용자는 여러 역할을 가짐 (피벗: role_user)
    public function roles(): array {
        $ids = RoleUser::roleIdsFor($this->attributes['id']);
        return array_map(fn(int $id) => Role::find($id), $ids);
    }

    // 접근자: $user->full_name
    public function getFullNameAttribute(): string {
        return $this->attributes['name'] . ' 님';
    }

    // 뮤테이터: 저장 시 비밀번호 해시
    public function setPasswordAttribute(string $value): string {
        return password_hash($value, PASSWORD_DEFAULT);
    }

    // 지역 스코프: 이름으로 필터
    public static function scopeWhereName(string $name): array {
        return array_values(array_filter(
            static::$tableData,
            fn(Model $m) => $m->getAttribute('name') === $name
        ));
    }
}

class Post extends Model {
    protected static array $tableData = [];
    protected static array $fillable = ['title', 'body', 'user_id'];

    public function user(): ?Model {
        return $this->belongsTo(User::class, 'user_id');
    }
}

class Role extends Model {
    protected static array $tableData = [];
    protected static array $fillable = ['name'];
}

// 피벗 테이블 (role_user) 시뮬레이션
class RoleUser {
    private static array $data = [];

    public static function attach(int $userId, int $roleId): void {
        self::$data[] = ['user_id' => $userId, 'role_id' => $roleId];
    }

    public static function roleIdsFor(int $userId): array {
        return array_values(array_map(
            fn(array $row) => $row['role_id'],
            array_filter(self::$data, fn(array $row) => $row['user_id'] === $userId)
        ));
    }
}

// --- 데모 ---

echo "=== 1. 모델 생성과 fillable 보호 ===\n";

$company = Company::create(['name' => '네이버', 'secret' => '이 값은 무시됨']);
echo "회사: {$company->name}\n";
echo "secret 속성은? " . var_export($company->secret, true) . " (fillable 보호)\n";

$alice = User::create([
    'name' => 'Alice',
    'email' => 'alice@example.com',
    'company_id' => $company->id,
]);
$alice->password = 'secret123';   // __set → 뮤테이터

$bob = User::create([
    'name' => 'Bob',
    'email' => 'bob@example.com',
    'company_id' => $company->id,
]);

echo "접근자 full_name: {$alice->full_name}\n";
$storedHash = $alice->getAttribute('password');
echo "저장된 비밀번호(해시): " . substr($storedHash, 0, 18) . "...\n";
echo "비밀번호 검증: " . (password_verify('secret123', $storedHash) ? '성공' : '실패') . "\n\n";

echo "=== 2. 1:N 관계 (User hasMany Post) ===\n";

Post::create(['title' => '첫 번째 글', 'body' => '안녕하세요', 'user_id' => $alice->id]);
Post::create(['title' => '두 번째 글', 'body' => 'PHP 중급 과정', 'user_id' => $alice->id]);
Post::create(['title' => 'Bob의 글', 'body' => '반갑습니다', 'user_id' => $bob->id]);

foreach ($alice->posts() as $post) {
    echo "  - [{$post->id}] {$post->title} (작성자: {$post->user()->name})\n";
}

echo "  Alice의 글 수: " . count($alice->posts()) . "\n\n";

echo "=== 3. belongsTo 역방향 ===\n";
echo "  Alice의 회사: " . $alice->company()->name . "\n";
echo "  첫 글의 작성자: " . $alice->posts()[0]->user()->name . "\n\n";

echo "=== 4. N:M 관계 (belongsToMany, 피벗 role_user) ===\n";

$admin = Role::create(['name' => 'admin']);
$editor = Role::create(['name' => 'editor']);
$viewer = Role::create(['name' => 'viewer']);

RoleUser::attach($alice->id, $admin->id);
RoleUser::attach($alice->id, $editor->id);
RoleUser::attach($bob->id, $viewer->id);

$aliceRoles = array_map(fn(Role $r) => $r->name, $alice->roles());
$bobRoles = array_map(fn(Role $r) => $r->name, $bob->roles());

echo "  Alice의 역할: " . implode(', ', $aliceRoles) . "\n";
echo "  Bob의 역할: " . implode(', ', $bobRoles) . "\n\n";

echo "=== 5. 지역 스코프 ===\n";
$found = User::scopeWhereName('Bob');
echo "  scopeWhereName('Bob') 결과: " . count($found) . "명 ("
    . $found[0]->email . ")\n\n";

echo "=== 6. update / delete ===\n";
$alice->update(['name' => 'Alicia']);
echo "  update 후 이름: {$alice->name}, full_name: {$alice->full_name}\n";
$bob->delete();
echo "  Bob 삭제 후 사용자 수: " . count(User::all()) . "\n";
