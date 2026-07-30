function LogMethod(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const original = descriptor.value
  descriptor.value = function (...args: any[]) {
    console.log(`Calling ${propertyKey}(${args.map(a => JSON.stringify(a)).join(', ')})`)
    return original.apply(this, args)
  }
}

function ReadOnly(target: any, propertyKey: string) {
  Object.defineProperty(target, propertyKey, { writable: false })
}

class Calculator {
  @ReadOnly
  version: string = '1.0'

  @LogMethod
  add(a: number, b: number): number {
    return a + b
  }

  @LogMethod
  multiply(a: number, b: number): number {
    return a * b
  }
}

const calc = new Calculator()
console.log(calc.add(3, 4))
console.log(calc.multiply(5, 6))
console.log(`Version: ${calc.version}`)
