// 스트림 고급: 파이프라인, backpressure, transform 스트림 예제

const { Readable, Transform, Writable } = require('stream');
const { pipeline } = require('stream/promises');
const zlib = require('zlib');
const fs = require('fs');
const os = require('os');
const path = require('path');
const { once } = require('events');

function delay(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

// ---------- 1. Transform 스트림: 로그 정규화 ----------
class LogNormalizer extends Transform {
  constructor() {
    super({ objectMode: true });
    this.count = 0;
  }

  _transform(line, encoding, cb) {
    if (line) {
      this.count += 1;
      const normalized = `[${this.count}] ${line.trim()}`;
      this.push(normalized);
    }
    cb();
  }

  _flush(cb) {
    this.push(`-- 총 ${this.count}줄 정규화 완료 --`);
    cb();
  }
}

console.log('=== 1. Transform 스트림 (objectMode) ===');
const lines = Readable.from(['  첫 번째 로그  ', ' 두 번째 로그', '', '  세 번째 로그  ']);
const normalizer = new LogNormalizer();
normalizer.on('data', (line) => console.log('  ', line));
lines.pipe(normalizer);
lines.on('end', async () => {
  await delay(50);

  // ---------- 2. Backpressure 데모 ----------
  console.log('\n=== 2. Backpressure ===');
  const slowWriter = new Writable({
    highWaterMark: 8, // 내부 버퍼가 매우 작도록 설정
    write(chunk, encoding, cb) {
      process.stdout.write(`  [저장] ${chunk.length} bytes\n`);
      setTimeout(cb, 50); // 느린 저장소 시뮬레이션
    },
  });

  let backpressureCount = 0;
  const chunks = ['A'.repeat(100), 'B'.repeat(100), 'C'.repeat(100), 'D'.repeat(100)];
  for (const chunk of chunks) {
    const ok = slowWriter.write(chunk);
    if (!ok) {
      backpressureCount += 1;
      console.log(`  write()가 false 반환 -> backpressure! drain 기다림`);
      await once(slowWriter, 'drain');
    }
  }
  slowWriter.end();
  await once(slowWriter, 'finish');
  console.log(`  backpressure 발생 횟수: ${backpressureCount}`);

  // ---------- 3. pipeline + zlib 압축 ----------
  console.log('\n=== 3. pipeline + gzip 압축 ===');
  const srcFile = path.join(os.tmpdir(), 'streams-advanced-input.txt');
  const destFile = path.join(os.tmpdir(), 'streams-advanced-output.txt.gz');

  fs.writeFileSync(srcFile, 'Node.js 스트림 강좌에 오신 것을 환영합니다!\n'.repeat(5000));

  await pipeline(
    fs.createReadStream(srcFile),
    zlib.createGzip(),
    fs.createWriteStream(destFile)
  );

  const inSize = fs.statSync(srcFile).size;
  const outSize = fs.statSync(destFile).size;
  console.log(`  압축 완료: ${inSize} bytes -> ${outSize} bytes (${((outSize / inSize) * 100).toFixed(1)}%)`);

  // ---------- 4. async 이터레이션 ----------
  console.log('\n=== 4. async 이터레이션 (for await) ===');
  let wordCount = 0;
  for await (const chunk of fs.createReadStream(srcFile, { encoding: 'utf8' })) {
    wordCount += chunk.split(/\s+/).filter(Boolean).length;
  }
  console.log(`  원본 파일 단어 수: ${wordCount}`);

  // ---------- 5. 대용량 파일 파이프라인 (해시 계산) ----------
  console.log('\n=== 5. 파이프라인 + crypto 해시 ===');
  const { createHash } = require('crypto');
  const hash = createHash('sha256');
  await pipeline(fs.createReadStream(srcFile), new Transform({
    transform(chunk, enc, cb) {
      hash.update(chunk);
      cb();
    },
  }));
  console.log(`  SHA-256: ${hash.digest('hex')}`);

  // 임시 파일 정리
  fs.unlinkSync(srcFile);
  fs.unlinkSync(destFile);
  console.log('\n(스트림 고급 데모 완료)');
});
