// ES Module 방식으로 가져오기

// 개별 내보내기 가져오기
import { add, sub, mul, div } from './math.js';

// 기본 내보내기 전체 가져오기
import math from './math.js';

console.log('add(10, 5):', add(10, 5));
console.log('sub(10, 5):', sub(10, 5));
console.log('mul(10, 5):', mul(10, 5));
console.log('div(10, 5):', div(10, 5));

console.log('---');
console.log('math.add(10, 5):', math.add(10, 5));
