# 05. path 모듈

`path` 모듈은 파일 경로를 다루는 유틸리티를 제공합니다.

## 불러오기

```javascript
const path = require('path');
```

## 주요 메서드

### path.join()
여러 경로 세그먼트를 결합합니다. OS에 맞는 구분자(`\` 또는 `/`)를 자동 사용합니다.

```javascript
path.join('/users', 'john', 'docs', 'file.txt');
// Windows: \users\john\docs\file.txt
// Linux:   /users/john/docs/file.txt
```

### path.resolve()
절대 경로로 변환합니다. 오른쪽에서 왼쪽으로 처리됩니다.

```javascript
path.resolve('docs', 'file.txt');
// C:\work\basic_cs\docs\file.txt (현재 디렉토리 기준)

path.resolve('/data', 'docs', 'file.txt');
// C:\data\docs\file.txt
```

### path.basename(), path.dirname(), path.extname()

```javascript
const file = '/users/john/docs/file.txt';

path.basename(file);      // 'file.txt'
path.basename(file, '.txt'); // 'file'
path.dirname(file);       // '/users/john/docs'
path.extname(file);       // '.txt'
```

### path.parse()
경로를 구성 요소로 분해합니다.

```javascript
path.parse('/users/john/docs/file.txt');
// {
//   root: '/',
//   dir: '/users/john/docs',
//   base: 'file.txt',
//   name: 'file',
//   ext: '.txt'
// }
```
