type ID = number | string
type Status = 'active' | 'inactive' | 'pending'

type Person = {
  name: string
  age: number
}

type Address = {
  city: string
  country: string
}

type Contact = Person & Address & { email: string }

function printContact(c: Contact): void {
  console.log(`${c.name}, ${c.age} - ${c.city}, ${c.country} (${c.email})`)
}

const contact: Contact = {
  name: 'Bob',
  age: 30,
  city: 'Seoul',
  country: 'Korea',
  email: 'bob@test.com',
}

printContact(contact)

function getStatus(s: Status): string {
  return `Status: ${s}`
}

console.log(getStatus('active'))
