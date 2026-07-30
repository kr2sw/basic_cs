enum Direction {
  Up = 'UP',
  Down = 'DOWN',
  Left = 'LEFT',
  Right = 'RIGHT',
}

type Shape =
  | { kind: 'circle'; radius: number }
  | { kind: 'square'; side: number }
  | { kind: 'triangle'; base: number; height: number }

function area(shape: Shape): number {
  switch (shape.kind) {
    case 'circle': return Math.PI * shape.radius ** 2
    case 'square': return shape.side ** 2
    case 'triangle': return (shape.base * shape.height) / 2
  }
}

function isString(value: unknown): value is string {
  return typeof value === 'string'
}

console.log(Direction.Up)

console.log(area({ kind: 'circle', radius: 5 }))
console.log(area({ kind: 'square', side: 4 }))
console.log(area({ kind: 'triangle', base: 3, height: 6 }))

const val: unknown = 'hello'
if (isString(val)) {
  console.log(val.toUpperCase())
}
