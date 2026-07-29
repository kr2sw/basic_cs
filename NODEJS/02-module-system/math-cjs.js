// CommonJS 방식으로 내보내기
const add = (a, b) => a + b;
const sub = (a, b) => a - b;
const mul = (a, b) => a * b;
const div = (a, b) => a / b;

module.exports = { add, sub, mul, div };

// 참고: exports = { ... }는 동작하지 않음 (참조가 끊어짐)
// exports.add = add;  // 개별 할당은 가능
