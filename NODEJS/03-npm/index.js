// npm 패키지 사용 예제
const _ = require('lodash');

const numbers = [5, 3, 8, 1, 9, 2];
console.log('원본:', numbers);
console.log('정렬:', _.sortBy(numbers));
console.log('합계:', _.sum(numbers));
console.log('평균:', _.mean(numbers));
