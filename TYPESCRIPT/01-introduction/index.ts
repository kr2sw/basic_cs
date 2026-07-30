let message: string = 'Hello, TypeScript!'
let age: number = 25
let isReady: boolean = true

function greet(name: string): string {
  return `Hello, ${name}!`
}

console.log(message)
console.log(greet('Alice'))
console.log(`Age: ${age}, Ready: ${isReady}`)
