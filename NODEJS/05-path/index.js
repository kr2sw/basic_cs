const path = require('path');

// 현재 파일의 디렉토리와 절대 경로
console.log('__dirname:', __dirname);
console.log('__filename:', __filename);
console.log('---');

// 1. path.join() - 경로 결합
const joined = path.join(__dirname, 'subdir', 'test.txt');
console.log('join:', joined);

const joined2 = path.join('users', 'john', '..', 'docs', 'file.txt');
console.log('join (.. 포함):', joined2); // users\docs\file.txt

// 2. path.resolve() - 절대 경로로 변환
const resolved = path.resolve('docs', 'file.txt');
console.log('resolve:', resolved);

const resolved2 = path.resolve('/absolute', 'docs', 'file.txt');
console.log('resolve (절대):', resolved2);

// 3. path.basename() - 파일명
const filePath = '/users/john/docs/file.txt';
console.log('\n파일 경로:', filePath);
console.log('basename:', path.basename(filePath));
console.log('basename (확장자 제거):', path.basename(filePath, '.txt'));

// 4. path.dirname() - 디렉토리 경로
console.log('dirname:', path.dirname(filePath));

// 5. path.extname() - 확장자
console.log('extname:', path.extname(filePath));

// 6. path.parse() - 전체 파싱
console.log('\nparse:');
const parsed = path.parse(filePath);
console.log(parsed);
console.log('  root:', parsed.root);
console.log('  dir:', parsed.dir);
console.log('  name:', parsed.name);
console.log('  ext:', parsed.ext);
console.log('  base:', parsed.base);

// 7. path.format() - parse의 역
const formatted = path.format(parsed);
console.log('\nformat:', formatted);
