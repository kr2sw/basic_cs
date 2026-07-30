abstract class Animal {
  constructor(protected name: string) {}
  abstract makeSound(): void
  move(): void {
    console.log(`${this.name} moves`)
  }
}

class Dog extends Animal {
  constructor(name: string, private breed: string) {
    super(name)
  }
  makeSound(): void {
    console.log(`${this.name} barks! Woof!`)
  }
  getBreed(): string {
    return this.breed
  }
}

interface Runnable {
  run(speed: number): void
}

class Athlete implements Runnable {
  constructor(private name: string) {}
  run(speed: number): void {
    console.log(`${this.name} runs at ${speed} km/h`)
  }
}

const dog = new Dog('Max', 'Golden Retriever')
dog.makeSound()
dog.move()
console.log(dog.getBreed())

const athlete = new Athlete('Charlie')
athlete.run(15)
