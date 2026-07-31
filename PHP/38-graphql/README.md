# 38: GraphQL — 스키마, 쿼리/리졸버 개념

## GraphQL이란

REST가 엔드포인트별로 고정된 응답을 주는 반면, GraphQL은 **클라이언트가 필요한 필드만** 요청합니다.

```
POST /graphql
query { user(id: 1) { name posts { title } } }
```

- 과다 페치(필요 이상의 데이터) / 과소 페치(추가 요청) 문제 해결
- 단일 엔드포인트, PHP에서는 `webonyx/graphql-php`가 대표적

## 스키마 (SDL)

```graphql
type Query {
    users: [User!]!
    user(id: ID!): User
}

type User {
    id: ID!
    name: String!
    posts: [Post!]!
}
```

- `!` — null 허용 안 함 (NonNull)
- `[User!]!` — null 없는 요소들의 목록

## 쿼리와 인자

```graphql
query {
    user(id: 1) {
        name
        posts { title }
    }
}
```

## 리졸버 (Resolver)

필드마다 값을 해석하는 함수입니다. 부모 객체(`parent`)와 인자(`args`)를 받습니다.

```php
'User.posts' => fn($parent, $args) => filter posts by $parent['id'],
```

쿼리 파싱 → 트리 탐색 → 필드별 리졸버 호출로 실행됩니다.

## Mutation

데이터 변경은 `mutation` 키워드로 구분합니다.

```graphql
mutation { createUser(name: "Alice") { id name } }
```

## 실행

```bash
php index.php
```
