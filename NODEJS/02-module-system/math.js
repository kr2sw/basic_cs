// ES Module 방식으로 내보내기 (package.json에 "type": "module" 필요)

export const add = (a, b) => a + b;
export const sub = (a, b) => a - b;
export const mul = (a, b) => a * b;
export const div = (a, b) => a / b;

export default { add, sub, mul, div };
