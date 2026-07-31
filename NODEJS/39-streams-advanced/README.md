# 39: 스트림 고급 — Pipeline, Backpressure, Transform

스트림의 고급 개념인 파이프라인, backpressure, transform을 학습합니다.

## 파이프라인 (Pipeline)

`pipeline`은 스트림 간 연결과 에러 처리를 한 번에 해결합니다. 중간에 실패하면 이미 연결된 스트림도 자동 정리됩니다.

```js
const { pipeline } = require('stream/promises');

await pipeline(
  fs.createReadStream('input.txt'),
  zlib.createGzip(),
  fs.createWriteStream('input.txt.gz')
);
```

## Backpressure

생산자(Readable)가 소비자(Writable)보다 빠르면 메모리에 쌓입니다. `write()`가 `false`를 반환하면 **drain** 이벤트를 기다렸다가 다시 씁니다.

```js
if (!writable.write(chunk)) {
  await once(writable, 'drain');
}
```

`highWaterMark`는 내부 버퍼 크기 기준입니다.

## Transform 스트림

읽고 쓰는 중간에 데이터를 변환합니다. (예: 압축, 로그 정규화)

```js
class UppercaseStream extends Transform {
  _transform(chunk, encoding, cb) {
    cb(null, chunk.toString().toUpperCase());
  }
}
```

## async 이터레이션

Readable은 `for await...of`로 순회할 수 있습니다.

```js
for await (const chunk of fs.createReadStream('file.txt')) {
  // ...
}
```

## 예제 실행

```bash
node index.js
```
