// fs/promises API를 사용한 예제
async function fileManager() {
  const fs = require('fs/promises');
  const filePath = './data.json';

  // 쓰기
  const data = { name: '홍길동', age: 30, job: '개발자' };
  await fs.writeFile(filePath, JSON.stringify(data, null, 2));
  console.log('JSON 파일 저장 완료');

  // 읽기
  const raw = await fs.readFile(filePath, 'utf-8');
  const obj = JSON.parse(raw);
  console.log('읽은 데이터:', obj);

  // 디렉토리 목록
  const dir = await fs.readdir('.');
  console.log('파일 목록:', dir);

  // 파일 정보
  const stat = await fs.stat(filePath);
  console.log('파일 크기:', stat.size, 'bytes');
  console.log('수정 시간:', stat.mtime);
}

fileManager().catch(console.error);
