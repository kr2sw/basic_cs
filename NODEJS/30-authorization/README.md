# 30: 권한 관리 — RBAC and ACL

RBAC(역할 기반)와 ACL(접근 제어 목록)을 학습합니다.

## RBAC (Role-Based Access Control)

사용자에게 직접 권한을 부여하지 않고 **역할(role)**에 권한을 묶습니다.

```js
const ROLES = {
  admin:  ['user:read', 'user:create', 'user:delete', 'post:delete'],
  editor: ['user:read', 'post:create', 'post:update'],
  viewer: ['user:read', 'post:read'],
};

function can(role, permission) {
  return (ROLES[role] || []).includes(permission);
}
```

## 인가 미들웨어

인증된 요청이 특정 권한을 가졌는지 검사합니다.

```js
function authorize(permission) {
  return (req, res, next) => {
    if (!can(req.user.role, permission)) {
      return res.status(403).json({ error: '권한이 없습니다' });
    }
    next();
  };
}

app.delete('/posts/:id', authorize('post:delete'), deletePost);
```

## 역할 계층

상위 역할이 하위 역할의 권한을 모두 가집니다.

```js
const LEVEL = { viewer: 1, editor: 2, admin: 3 };
canHierarchy(level >= LEVEL['editor'], 'post:update');
```

## ACL (Access Control List)

객체(리소스) 단위로 특정 사용자의 권한을 관리합니다. 문서 공유, 드라이브 폴더 권한에 적합합니다.

```js
acl.grant('user-1', 'doc-42', ['read', 'write']);
acl.check('user-1', 'doc-42', 'write'); // true
```

| 구분 | RBAC | ACL |
|------|------|-----|
| 기준 | 사용자의 역할 | 리소스 단위 접근 목록 |
| 적합 | 시스템 전체 권한 | 특정 데이터의 세밀한 권한 |

## 예제 실행

```bash
node index.js
```
