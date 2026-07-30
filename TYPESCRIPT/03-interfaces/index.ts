interface User {
  readonly id: number
  name: string
  email?: string
}

interface Admin extends User {
  role: 'admin' | 'superadmin'
  permissions: string[]
}

function createAdmin(user: Admin): void {
  console.log(`${user.name} (${user.role}) has: ${user.permissions.join(', ')}`)
}

const admin: Admin = {
  id: 1,
  name: 'Alice',
  role: 'admin',
  permissions: ['read', 'write', 'delete'],
}

createAdmin(admin)

interface StringMap {
  [key: string]: string
}

const env: StringMap = { NODE_ENV: 'development', PORT: '3000' }
console.log(env)
