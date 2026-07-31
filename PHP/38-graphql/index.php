<?php
// --- GraphQL: 스키마, 쿼리/리졸버 개념 (미니 실행 엔진) ---

echo "=== 1. 스키마 (SDL) ===\n\n";

$schema = <<<'SDL'
type Query {
    users: [User!]!
    user(id: ID!): User
    post(id: ID!): Post
}

type User {
    id: ID!
    name: String!
    email: String!
    posts: [Post!]!          # 관계 필드 — 리졸버로 해석
}

type Post {
    id: ID!
    title: String!
    user: User!
}
SDL;
echo $schema . "\n\n";

echo "=== 2. 데이터 + 리졸버 ===\n\n";

// 시뮬레이션 데이터베이스
$users = [
    1 => ['id' => 1, 'name' => 'Alice', 'email' => 'alice@example.com'],
    2 => ['id' => 2, 'name' => 'Bob', 'email' => 'bob@example.com'],
];
$posts = [
    1 => ['id' => 1, 'title' => 'GraphQL 첫걸음', 'user_id' => 1],
    2 => ['id' => 2, 'title' => '리졸버 이해하기', 'user_id' => 1],
    3 => ['id' => 3, 'title' => '스키마 설계', 'user_id' => 2],
];

// 필드별 리졸버 ("Type.field" → 함수). 인자: (부모 객체, args)
$resolvers = [
    'Query.users' => fn($parent, array $args) => array_values($users),
    'Query.user' => fn($parent, array $args) => $users[$args['id']] ?? null,
    'Query.post' => fn($parent, array $args) => $posts[$args['id']] ?? null,
    'User.posts' => fn($parent, array $args) => array_values(array_filter(
        $posts,
        fn($p) => $p['user_id'] === $parent['id']
    )),
    'Post.user' => fn($parent, array $args) => $users[$parent['user_id']] ?? null,
];

// 필드가 어떤 타입을 반환하는지 정의 (서브필드 재귀용)
$typeMap = [
    'Query.users' => 'User',
    'Query.user' => 'User',
    'Query.post' => 'Post',
    'User.posts' => 'Post',
    'Post.user' => 'User',
];

// --- 미니 GraphQL 실행 엔진 ---

// 쿼리 문자열 → 토큰
function tokenize(string $query): array {
    preg_match_all('/"[^"]*"|[a-zA-Z_][a-zA-Z0-9_]*|\d+|[{}():,]/', $query, $m);
    return $m[0];
}

// 토큰 → 선택 집합 트리
function parseSelectionSet(array $tokens, int &$i): array {
    $fields = [];
    if (($tokens[$i] ?? '') === '{') {
        $i++;
    }

    while (isset($tokens[$i]) && $tokens[$i] !== '}') {
        $name = $tokens[$i++];

        // 인자 파싱: user(id: 1)
        $args = [];
        if (($tokens[$i] ?? '') === '(') {
            $i++;
            while (isset($tokens[$i]) && $tokens[$i] !== ')') {
                $argName = $tokens[$i++];
                $i++;                          // ':'
                $raw = $tokens[$i++];
                $args[$argName] = str_starts_with($raw, '"') ? trim($raw, '"') : (int)$raw;
                if (($tokens[$i] ?? '') === ',') {
                    $i++;
                }
            }
            $i++;                              // ')'
        }

        // 중첩 선택: { name posts { title } }
        $sub = [];
        if (($tokens[$i] ?? '') === '{') {
            $sub = parseSelectionSet($tokens, $i);
        }

        $fields[] = ['name' => $name, 'args' => $args, 'fields' => $sub];
    }
    $i++;   // '}'
    return $fields;
}

function resolveFields(array $fields, array $resolvers, array $typeMap, ?array $parent, string $type): array {
    $result = [];
    foreach ($fields as $field) {
        $key = $type . '.' . $field['name'];
        $resolver = $resolvers[$key] ?? null;

        $value = $resolver !== null
            ? $resolver($parent, $field['args'])
            : ($parent[$field['name']] ?? null);   // 스칼라 필드

        // 서브필드가 있으면 재귀적으로 해석
        if ($field['fields']) {
            $subType = $typeMap[$key] ?? 'Scalar';
            if (is_array($value) && array_is_list($value)) {
                $value = array_map(
                    fn($row) => resolveFields($field['fields'], $resolvers, $typeMap, $row, $subType),
                    $value
                );
            } elseif (is_array($value)) {
                $value = resolveFields($field['fields'], $resolvers, $typeMap, $value, $subType);
            } else {
                $value = null;
            }
        }

        $result[$field['name']] = $value;
    }
    return $result;
}

function executeQuery(string $query, array $resolvers, array $typeMap): array {
    $tokens = tokenize($query);
    $i = 0;
    if (($tokens[$i] ?? '') === 'query') {
        $i++;
    }
    $fields = parseSelectionSet($tokens, $i);
    return resolveFields($fields, $resolvers, $typeMap, null, 'Query');
}

// --- 데모 ---
echo "=== 3. 쿼리 실행 ===\n\n";

$queries = [
    '{ users { id name email } }',
    '{ user(id: 1) { id name posts { title } } }',
    '{ post(id: 3) { id title user { name } } }',
    '{ users { name posts { title } } }',
];

foreach ($queries as $query) {
    echo "  쿼리: $query\n";
    echo "  결과:\n";
    echo json_encode(
        executeQuery($query, $resolvers, $typeMap),
        JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT
    ) . "\n";
}
