# 02. 모듈 시스템

Node.js는 **CommonJS**와 **ES Modules** 두 가지 모듈 시스템을 지원합니다.

## CommonJS (require / exports)

Node.js 기본 모듈 시스템입니다.

### 모듈 내보내기 (exports)

```javascript
// math.js
const add = (a, b) => a + b;
const sub = (a, b) => a - b;

module.exports = { add, sub };
// 또는 개별 내보내기:
// exports.add = add;
// exports.sub = sub;
```

### 모듈 가져오기 (require)

```javascript
// app.js
const math = require('./math.js');
console.log(math.add(2, 3)); // 5
```

### module.exports vs exports

- `exports`는 `module.exports`의 별칭(참조)입니다.
- `exports = { ... }`처럼 재할당하면 참조가 끊어져 **동작하지 않습니다**.
- `module.exports = { ... }`로 사용하는 것이 안전합니다.

```javascript
// ❌ 잘못된 방식
exports = { add, sub };

// ✅ 올바른 방식
module.exports = { add, sub };
// 또는
exports.add = add;
exports.sub = sub;
```

## ES Modules (import / export)

ESM을 사용하려면 `package.json`에 `"type": "module"`을 추가하거나 파일 확장자를 `.mjs`로 사용합니다.

### package.json
```json
{
  "type": "module"
}
```

### 모듈 내보내기 (export)

```javascript
// math.mjs 또는 type:module 설정 후 math.js
export const add = (a, b) => a + b;
export const sub = (a, b) => a - b;

// 기본 내보내기
export default { add, sub };
```

### 모듈 가져오기 (import)

```javascript
// app.mjs
import { add, sub } from './math.mjs';
// 또는 기본 내보내기
import math from './math.mjs';
```
