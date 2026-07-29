const fs = require('fs');
const path = require('path');

const sampleFile = path.join(__dirname, 'sample.txt');

// 1. 파일 쓰기 (동기)
fs.writeFileSync(sampleFile, '안녕하세요, Node.js!\n두 번째 줄입니다.');
console.log('파일이 생성되었습니다.');

// 2. 파일 읽기 (동기)
const data = fs.readFileSync(sampleFile, 'utf-8');
console.log('--- 파일 내용 ---');
console.log(data);

// 3. 파일 존재 확인
console.log('파일 존재?:', fs.existsSync(sampleFile));
console.log('없는 파일?:', fs.existsSync('./no-file.txt'));

// 4. 디렉토리 읽기
console.log('\n--- 현재 디렉토리 목록 ---');
const files = fs.readdirSync('.');
files.forEach((f) => console.log(' ', f));

// 5. 비동기 (콜백) 방식
console.log('\n비동기 읽기 시작...');
fs.readFile(sampleFile, 'utf-8', (err, content) => {
  if (err) {
    console.error('에러:', err);
    return;
  }
  console.log('비동기 읽기 완료:', content);
});

// 6. 프로미스 API (fs/promises)
async function runWithPromises() {
  const fsp = require('fs/promises');
  const content = await fsp.readFile(sampleFile, 'utf-8');
  console.log('\n프로미스 API로 읽기:', content);
}
runWithPromises();
