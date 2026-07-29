# 01. Node.js 소개

## Node.js란?

Node.js는 Chrome V8 JavaScript 엔진으로 빌드된 **서버 사이드 JavaScript 런타임**입니다. 브라우저 밖에서도 JavaScript를 실행할 수 있게 해줍니다.

### 주요 특징

- **비동기 I/O**: 논블로킹 방식으로 파일, 네트워크 요청 처리
- **이벤트 기반**: 이벤트 루프를 통해 작업 처리
- **싱글 스레드**: 하나의 스레드로 여러 요청 처리 (비동기 방식)
- **npm**: 방대한 패키지 생태계

## REPL (Read-Eval-Print Loop)

터미널에서 `node` 명령어만 입력하면 대화형 환경 실행:

```bash
node
> console.log('Hello Node.js!')
Hello Node.js!
undefined
> 1 + 2
3
> .exit
```

## 첫 번째 스크립트

`index.js`:

```javascript
console.log('Hello Node.js!');
console.log('Arguments:', process.argv);
```

실행:

```bash
node index.js
# 출력: Hello Node.js!
# 출력: Arguments: [ 'node.exe', 'index.js' ]

node index.js foo bar
# 출력: Arguments: [ 'node.exe', 'index.js', 'foo', 'bar' ]
```

## process.argv

`process.argv`는 명령줄 인수를 담은 배열입니다:
- `argv[0]`: Node.js 실행 파일 경로
- `argv[1]`: 실행된 스크립트 경로
- `argv[2]`부터: 사용자가 전달한 인수
