// 첫 번째 Node.js 스크립트
console.log('Hello Node.js!');

// process.argv로 명령줄 인수 확인
console.log('Arguments:', process.argv);

// 사용법: node index.js [name]
const name = process.argv[2];
if (name) {
  console.log(`안녕하세요, ${name}님!`);
} else {
  console.log('이름을 인수로 전달해보세요. 예: node index.js 홍길동');
}
