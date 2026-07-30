let num: number = 42
let str: string = 'hello'
let bool: boolean = true
let arr: number[] = [1, 2, 3]
let arr2: Array<string> = ['a', 'b']

let tuple: [string, number] = ['Alice', 25]

enum Color { Red, Green, Blue }
let c: Color = Color.Green

let unknownVal: unknown = 'could be anything'
if (typeof unknownVal === 'string') {
  console.log(unknownVal.toUpperCase())
}

function fail(): never {
  throw new Error('Always throws')
}

function logMsg(msg: string): void {
  console.log(msg)
}

console.log({ num, str, bool, arr, arr2, tuple, color: c })
logMsg('void function works')
