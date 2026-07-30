type Person = { name: string; age: number; email: string }
type PersonKeys = keyof Person

function getValue<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key]
}

type ArrayElement<T> = T extends (infer U)[] ? U : never
type NumberArrayElement = ArrayElement<number[]>

type Nullable<T> = T | null
type StringOrNull = Nullable<string>

type ReadonlyMapped<T> = { readonly [K in keyof T]: T[K] }
type MutablePerson = ReadonlyMapped<Person>

const p: Person = { name: 'Alice', age: 25, email: 'a@test.com' }
console.log(getValue(p, 'name'))
console.log(getValue(p, 'age'))

type ReturnOf<T> = T extends (...args: any[]) => infer R ? R : never
type FnReturnType = ReturnOf<() => string[]>
