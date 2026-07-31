// 권한 관리: RBAC(역할 기반)와 ACL(접근 제어 목록) 구현 예제

// ---------- 1. RBAC: 역할별 권한 매트릭스 ----------
const ROLES = {
  viewer: ['post:read'],
  editor: ['post:read', 'post:create', 'post:update', 'post:delete'],
  admin: [
    'post:read', 'post:create', 'post:update', 'post:delete',
    'user:read', 'user:create', 'user:delete',
  ],
};

// 역할에 권한이 있는지 검사
function can(role, permission) {
  return (ROLES[role] || []).includes(permission);
}

// ---------- 2. 인가 미들웨어 (Express 유사) ----------
function authorize(permission) {
  return (req, res, next) => {
    if (!can(req.user.role, permission)) {
      return res.status(403).json({ error: '권한이 없습니다 (403)' });
    }
    next();
  };
}

// 실제 요청을 흉내 내는 데코레이터
function simulate(req, middleware, onEnd) {
  const res = {
    status: null,
    body: null,
    status(code) { this.status = code; return this; },
    json(body) { this.body = body; onEnd(this); },
  };
  middleware(req, res, () => onEnd(res));
}

console.log('=== 1. RBAC 검증 ===');
console.log('admin이 user:delete 가능?', can('admin', 'user:delete'));
console.log('viewer가 user:delete 가능?', can('viewer', 'user:delete'));
console.log('editor가 post:update 가능?', can('editor', 'post:update'));

console.log('\n--- 인가 미들웨어 동작 ---');
simulate({ user: { id: 1, role: 'viewer' } }, authorize('post:create'), (res) => {
  console.log(`viewer가 post:create -> ${res.status || 200} ${JSON.stringify(res.body || {})}`);
});

simulate({ user: { id: 2, role: 'editor' } }, authorize('post:create'), (res) => {
  console.log(`editor가 post:create -> ${res.status || 200} ${JSON.stringify(res.body || {})}`);
});

// ---------- 3. 역할 계층 (Role Hierarchy) ----------
const LEVEL = { viewer: 1, editor: 2, admin: 3 };

function canHierarchy(role, permission) {
  // 상위 역할은 하위 역할의 권한을 포함
  const roleLevel = LEVEL[role];
  if (!roleLevel) return false;
  const roleWithPermission = Object.entries(ROLES).find(
    ([name, perms]) => LEVEL[name] <= roleLevel && perms.includes(permission)
  );
  return Boolean(roleWithPermission);
}

console.log('\n=== 2. 역할 계층 ===');
console.log('admin이 post:update (editor 권한) 가능?', canHierarchy('admin', 'post:update'));
console.log('admin이 user:create (admin 전용) 가능?', canHierarchy('admin', 'user:create'));
console.log('editor가 user:create 가능?', canHierarchy('editor', 'user:create'));

// ---------- 4. ACL: 리소스 단위 접근 제어 ----------
class Acl {
  constructor() {
    this.rules = new Map(); // resourceId -> { userId: Set<perm> }
  }

  // 리소스에 사용자 권한 부여
  grant(userId, resourceId, permissions) {
    if (!this.rules.has(resourceId)) this.rules.set(resourceId, new Map());
    const users = this.rules.get(resourceId);
    if (!users.has(userId)) users.set(userId, new Set());
    const perms = users.get(userId);
    (Array.isArray(permissions) ? permissions : [permissions]).forEach((p) => perms.add(p));
  }

  revoke(userId, resourceId, permission) {
    const users = this.rules.get(resourceId);
    users?.get(userId)?.delete(permission);
  }

  check(userId, resourceId, permission) {
    return this.rules.get(resourceId)?.get(userId)?.has(permission) || false;
  }

  canAccess(userId, resourceId, permission) {
    // 소유자는 모든 권한 보유
    return this.check(userId, resourceId, 'owner') || this.check(userId, resourceId, permission);
  }
}

console.log('\n=== 3. ACL 데모 ===');
const acl = new Acl();

// 문서 소유권과 공유 권한
acl.grant('user-1', 'doc-42', 'owner');
acl.grant('user-2', 'doc-42', ['read', 'write']); // 편집자로 공유
acl.grant('user-3', 'doc-42', ['read']);          // 열람만 가능

console.log('user-1은 owner 권한 있음:', acl.check('user-1', 'doc-42', 'owner'));
console.log('user-2는 write 가능:', acl.check('user-2', 'doc-42', 'write'));
console.log('user-3는 write 가능:', acl.check('user-3', 'doc-42', 'write'));
console.log('user-3는 read 가능:', acl.check('user-3', 'doc-42', 'read'));

// 소유자 권한은 삭제해도 완전히 제거 불가 (데모)
acl.revoke('user-3', 'doc-42', 'read');
console.log('\nuser-3의 read 권한 제거 후:', acl.check('user-3', 'doc-42', 'read'));

console.log('\n--- ACL 기반 요청 처리 ---');
function checkAccess(userId, resourceId, permission) {
  if (!acl.canAccess(userId, resourceId, permission)) {
    return `[403] ${userId}는 ${resourceId}에 대한 ${permission} 권한이 없습니다`;
  }
  return `[200] ${userId}가 ${resourceId}를 ${permission} 합니다`;
}
console.log(checkAccess('user-2', 'doc-42', 'write'));
console.log(checkAccess('user-3', 'doc-42', 'write'));
console.log(checkAccess('user-1', 'doc-42', 'delete'));
