# 27: GraphQL — Schema and Resolver Concepts

REST의 대안인 GraphQL의 스키마와 리졸버 개념을 학습합니다.

## GraphQL vs REST

| REST | GraphQL |
|------|---------|
| URL + 메서드로 리소스 정의 | 쿼리로 원하는 필드만 요청 |
| 여러 엔드포인트 | 단일 엔드포인트 |
| 오버페칭/언더페칭 발생 가능 | 필요한 데이터만 정확히 응답 |

## 스키마 (Schema)

어떤 데이터가 조회 가능한지 타입으로 정의합니다.

```graphql
type User {
  id: ID!
  name: String!
  email: String
  posts: [Post]
}

type Query {
  user(id: ID!): User
  users: [User]
}
```

## 리졸버 (Resolver)

스키마의 각 필드가 실제 데이터를 반환하는 함수입니다.

```js
const resolvers = {
  user: ({ id }) => users.find((u) => u.id === Number(id)),
};
```

## 쿼리 실행

클라이언트는 필요한 필드만 골라 요청합니다.

```graphql
{
  user(id: 1) {
    name
    email
    posts { title }
  }
}
```

## 예제 실행

예제는 apollo-server 없이 미니 GraphQL 엔진으로 스키마/리졸버/쿼리를 시뮬레이션합니다.

```bash
node index.js
```
