# 18. 스트림 (Streams)

Node.js 스트림을 사용하여 대용량 데이터를 효율적으로 처리하는 방법을 학습합니다.

## 스트림이 필요한 이유

파일 전체를 메모리에 로드하지 않고 청크 단위로 처리하여 메모리 사용량을 최소화합니다.

## 스트림의 종류

| 종류 | 설명 |
|------|------|
| **Readable** | 데이터를 읽을 수 있는 스트림 (fs.createReadStream) |
| **Writable** | 데이터를 쓸 수 있는 스트림 (fs.createWriteStream) |
| **Transform** | 읽고 쓰면서 데이터를 변환하는 스트림 (zlib) |
| **Duplex** | 읽기와 쓰기가 모두 가능한 스트림 |

## fs.createReadStream

```js
const readStream = fs.createReadStream('input.txt', { encoding: 'utf8' });
readStream.on('data', chunk => console.log('Chunk:', chunk.length));
```

## pipeline

`pipeline`은 스트림 간 에러 처리를 자동으로 해줍니다.

```js
const { pipeline } = require('stream/promises');
await pipeline(
  fs.createReadStream('input.txt'),
  fs.createWriteStream('output.txt')
);
```

## zlib 압축

```js
const zlib = require('zlib');
await pipeline(
  fs.createReadStream('input.txt'),
  zlib.createGzip(),
  fs.createWriteStream('input.txt.gz')
);
```

## 예제 실행

```bash
node index.js
```
