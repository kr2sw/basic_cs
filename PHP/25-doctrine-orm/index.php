<?php
// --- ORM 개념: 엔티티 매핑, 영속성 (인메모리 시뮬레이션) ---

echo "=== 1. 엔티티 매핑 (Attribute) ===\n";

#[\Attribute]
class Entity {
    public function __construct(public string $table = '') {}
}

#[\Attribute]
class Column {
    public function __construct(public string $name = '', public string $type = 'string') {}
}

#[\Attribute]
class Id {}

#[Entity(table: 'users')]
class User {
    #[Id, Column('id', 'integer')]
    public int $id = 0;

    #[Column('name', 'string')]
    public string $name = '';

    #[Column('email', 'string')]
    public string $email = '';

    public function __toString(): string {
        return "User#{$this->id} {$this->name} <{$this->email}>";
    }
}

#[Entity(table: 'posts')]
class Post {
    #[Id, Column('id', 'integer')]
    public int $id = 0;

    #[Column('title', 'string')]
    public string $title = '';

    #[Column('user_id', 'integer')]
    public int $userId = 0;
}

// 리플렉션으로 매핑 정보(메타데이터) 읽기
function metadataFor(object|string $class): array {
    $rc = new ReflectionClass($class);
    $meta = ['table' => '', 'fields' => [], 'id' => null];

    $entityAttrs = $rc->getAttributes(Entity::class);
    $meta['table'] = $entityAttrs
        ? $entityAttrs[0]->newInstance()->table
        : strtolower($rc->getShortName()) . 's';

    foreach ($rc->getProperties() as $prop) {
        $column = $prop->getAttributes(Column::class);
        if (!$column) {
            continue;
        }
        $col = $column[0]->newInstance();
        $meta['fields'][$prop->getName()] = [
            'name' => $col->name ?: $prop->getName(),
            'type' => $col->type,
        ];
        if ($prop->getAttributes(Id::class)) {
            $meta['id'] = $prop->getName();
        }
    }
    return $meta;
}

echo "테이블: " . metadataFor(User::class)['table'] . "\n";
echo "매핑 정보:\n";
echo json_encode(metadataFor(User::class)['fields'], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT) . "\n\n";

echo "=== 2. UnitOfWork: 엔티티 상태 추적 ===\n";

class UnitOfWork {
    public const STATE_NEW = 'new';
    public const STATE_MANAGED = 'managed';
    public const STATE_DETACHED = 'detached';
    public const STATE_REMOVED = 'removed';

    private array $states = [];
    private array $originalData = [];

    public function registerNew(object $entity): void {
        $this->states[spl_object_id($entity)] = self::STATE_NEW;
    }

    public function markManaged(object $entity): void {
        $this->states[spl_object_id($entity)] = self::STATE_MANAGED;
    }

    public function markRemoved(object $entity): void {
        $this->states[spl_object_id($entity)] = self::STATE_REMOVED;
    }

    public function getState(object $entity): string {
        return $this->states[spl_object_id($entity)] ?? self::STATE_DETACHED;
    }

    public function snapshot(object $entity): void {
        $this->originalData[spl_object_id($entity)] = get_object_vars($entity);
    }

    // 관리 중인 엔티티가 persist 시점 대비 변경됐는지 (dirty check)
    public function isDirty(object $entity): bool {
        if ($this->getState($entity) !== self::STATE_MANAGED) {
            return false;
        }
        $oid = spl_object_id($entity);
        return ($this->originalData[$oid] ?? []) !== get_object_vars($entity);
    }
}

echo "\n=== 3. EntityManager: 영속성 컨텍스트 ===\n";

class EntityManager {
    // 시뮬레이션 DB (테이블명 => [id => 행])
    private static array $database = [];
    private static array $sequences = [];

    private array $identityMap = [];
    private array $tracked = [];
    private array $repositories = [];

    private UnitOfWork $uow;

    public function __construct(?UnitOfWork $uow = null) {
        $this->uow = $uow ?? new UnitOfWork();
    }

    public function getUnitOfWork(): UnitOfWork {
        return $this->uow;
    }

    public function persist(object $entity): void {
        $this->uow->registerNew($entity);
        $this->tracked[] = $entity;
    }

    public function remove(object $entity): void {
        $this->uow->markRemoved($entity);
        $this->tracked[] = $entity;
    }

    // 저장 보류 중이던 모든 변경을 DB에 반영 (flush)
    public function flush(): void {
        $remaining = [];
        foreach ($this->tracked as $entity) {
            $state = $this->uow->getState($entity);
            if ($state === UnitOfWork::STATE_NEW) {
                $this->insert($entity);
                $this->uow->markManaged($entity);
                $this->uow->snapshot($entity);
                $remaining[] = $entity;
            } elseif ($state === UnitOfWork::STATE_REMOVED) {
                $this->delete($entity);
            } else {
                $this->update($entity);
                $this->uow->snapshot($entity);
                $remaining[] = $entity;
            }
        }
        $this->tracked = $remaining;
        echo "  flush() 실행됨\n";
    }

    private function insert(object $entity): void {
        $meta = metadataFor($entity);
        $table = $meta['table'];
        $next = (self::$sequences[$table] ?? 0) + 1;
        self::$sequences[$table] = $next;

        $idProp = $meta['id'];
        $entity->$idProp = $next;

        self::$database[$table][$next] = $this->toRow($entity);
        $this->identityMap["{$entity::class}:$next"] = $entity;
    }

    private function update(object $entity): void {
        $meta = metadataFor($entity);
        $id = $entity->{$meta['id']};
        self::$database[$meta['table']][$id] = $this->toRow($entity);
    }

    private function delete(object $entity): void {
        $meta = metadataFor($entity);
        $id = $entity->{$meta['id']};
        unset(self::$database[$meta['table']][$id]);
        unset($this->identityMap["{$entity::class}:$id"]);
    }

    private function toRow(object $entity): array {
        $row = [];
        foreach (metadataFor($entity)['fields'] as $prop => $field) {
            $row[$field['name']] = $entity->$prop;
        }
        return $row;
    }

    // Identity Map: 같은 id는 같은 인스턴스
    public function find(string $class, int $id): ?object {
        $key = "$class:$id";
        if (isset($this->identityMap[$key])) {
            return $this->identityMap[$key];
        }

        $meta = metadataFor($class);
        $row = self::$database[$meta['table']][$id] ?? null;
        if (!$row) {
            return null;
        }

        $entity = $this->hydrate($class, $row);
        $this->identityMap[$key] = $entity;
        $this->uow->markManaged($entity);
        $this->uow->snapshot($entity);
        return $entity;
    }

    public function findAll(string $class): array {
        $meta = metadataFor($class);
        $idCol = $meta['fields'][$meta['id']]['name'];
        $result = [];
        foreach (self::$database[$meta['table']] ?? [] as $row) {
            $result[] = $this->find($class, $row[$idCol]);
        }
        return $result;
    }

    public function getRepository(string $class): Repository {
        return $this->repositories[$class] ??= new Repository($this, $class);
    }

    private function hydrate(string $class, array $row): object {
        $entity = new $class();
        foreach (metadataFor($class)['fields'] as $prop => $field) {
            $value = $row[$field['name']] ?? null;
            $entity->$prop = $field['type'] === 'integer' ? (int)$value : $value;
        }
        return $entity;
    }
}

class Repository {
    public function __construct(
        private EntityManager $em,
        private string $class
    ) {}

    public function find(int $id): ?object {
        return $this->em->find($this->class, $id);
    }

    public function findAll(): array {
        return $this->em->findAll($this->class);
    }

    public function persist(object $entity): void {
        $this->em->persist($entity);
    }
}

// --- 데모 ---
$em = new EntityManager();
$uow = $em->getUnitOfWork();

echo "\n=== 4. persist + flush (INSERT) ===\n";

$user = new User();
$user->name = 'Alice';
$user->email = 'alice@example.com';
echo "  persist 전 상태: {$uow->getState($user)}\n";

$em->persist($user);
echo "  persist 후 상태: {$uow->getState($user)}\n";

$em->flush();
echo "  flush 후: id={$user->id}, 상태: {$uow->getState($user)}\n";

$bob = new User();
$bob->name = 'Bob';
$bob->email = 'bob@example.com';
$em->persist($bob);

$post = new Post();
$post->title = 'ORM 첫걸음';
$post->userId = 1;
$em->persist($post);
$em->flush();

echo "\n=== 5. Identity Map ===\n";
$a = $em->find(User::class, 1);
$b = $em->find(User::class, 1);
echo "  두 번 find해도 같은 인스턴스? " . ($a === $b ? 'true' : 'false') . "\n";

echo "\n=== 6. 변경 감지 (Dirty Checking) ===\n";
$a->name = 'Alicia';
echo "  이름 변경 후 isDirty? " . ($uow->isDirty($a) ? 'true' : 'false') . "\n";
$em->flush();
echo "  flush 후 다시 조회: " . $em->find(User::class, 1)->name . "\n";

echo "\n=== 7. Repository 패턴 ===\n";
$users = $em->getRepository(User::class)->findAll();
foreach ($users as $u) {
    echo "  - $u\n";
}

echo "\n=== 8. remove + flush (DELETE) ===\n";
$em->remove($em->find(User::class, 2));
$em->flush();
echo "  남은 사용자 수: " . count($em->findAll(User::class)) . "\n";
echo "  남은 게시글: " . $em->findAll(Post::class)[0]->title . " (작성자 user_id={$post->userId})\n";
