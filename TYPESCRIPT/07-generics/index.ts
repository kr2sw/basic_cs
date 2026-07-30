function identity<T>(arg: T): T {
  return arg
}

function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key]
}

interface Repository<T> {
  getAll(): T[]
  getById(id: number): T | undefined
  add(item: T): void
}

class TodoRepository implements Repository<{ id: number; title: string }> {
  private items: { id: number; title: string }[] = []

  getAll() { return this.items }
  getById(id: number) { return this.items.find(i => i.id === id) }
  add(item: { id: number; title: string }) { this.items.push(item) }
}

console.log(identity('hello'))
console.log(identity(42))

const repo = new TodoRepository()
repo.add({ id: 1, title: 'Learn TypeScript' })
console.log(repo.getAll())

const user = { name: 'Alice', age: 25 }
console.log(getProperty(user, 'name'))
