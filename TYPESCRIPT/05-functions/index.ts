function add(a: number, b: number): number {
  return a + b
}

function greet(name: string, greeting?: string): string {
  return `${greeting ?? 'Hello'}, ${name}!`
}

function multiply(a: number, b: number = 1): number {
  return a * b
}

function sum(...nums: number[]): number {
  return nums.reduce((a, b) => a + b, 0)
}

function reverse(x: string): string
function reverse(x: number): number
function reverse(x: string | number): string | number {
  if (typeof x === 'string') return x.split('').reverse().join('')
  return Number(String(x).split('').reverse().join(''))
}

console.log(add(3, 4))
console.log(greet('Alice'))
console.log(greet('Bob', 'Hi'))
console.log(multiply(5))
console.log(sum(1, 2, 3, 4, 5))
console.log(reverse('hello'))
console.log(reverse(12345))
