type JSONValue =
  | string
  | number
  | boolean
  | null
  | JSONValue[]
  | { [key: string]: JSONValue }

type Brand<T, B> = T & { __brand: B }
type UserId = Brand<number, 'UserId'>
type Email = Brand<string, 'Email'>

function createUser(id: UserId, email: Email) {
  console.log(`User created: ${id} with ${email}`)
}

const userId = 1 as UserId
const userEmail = 'alice@test.com' as Email
createUser(userId, userEmail)

type Color = 'red' | 'green' | 'blue'
type Shape2 = 'circle' | 'square'

const config = {
  color: 'red',
  shape: 'circle',
  size: 10,
} satisfies { color: Color; shape: Shape2; size: number }

console.log(config.color.toUpperCase())
