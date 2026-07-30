interface User {
  id: number
  name: string
  email: string
  age: number
}

type PartialUser = Partial<User>
type RequiredUser = Required<PartialUser>
type ReadonlyUser = Readonly<User>
type NameOnly = Pick<User, 'name' | 'email'>
type WithoutEmail = Omit<User, 'email'>
type RoleMap = Record<string, 'admin' | 'user' | 'guest'>
type Primitive = Exclude<string | number | boolean, boolean>
type Fn = (x: number, y: string) => boolean
type FnReturn = ReturnType<Fn>
type FnParams = Parameters<Fn>

const partial: PartialUser = { name: 'Alice' }
const picked: NameOnly = { name: 'Bob', email: 'bob@test.com' }
const roles: RoleMap = { alice: 'admin', bob: 'user', charlie: 'guest' }
const fnReturn: FnReturn = true

console.log({ partial, picked, roles, fnReturn })
