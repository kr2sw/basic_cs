export function add(a: i32, b: i32): i32 {
  return a + b;
}

export function factorial(n: i32): i32 {
  if (n <= 1) return 1;
  return n * factorial(n - 1);
}

export function fibonacci(n: i32): i32 {
  if (n <= 1) return n;
  return fibonacci(n - 1) + fibonacci(n - 2);
}

export function isPrime(n: i32): bool {
  if (n < 2) return false;
  for (let i: i32 = 2; i * i <= n; i++) {
    if (n % i == 0) return false;
  }
  return true;
}

export function greet(name: string): string {
  return "Hello, " + name + "! From AssemblyScript!";
}

export function sumArray(arr: Int32Array): i32 {
  let sum: i32 = 0;
  for (let i: i32 = 0; i < arr.length; i++) {
    sum += arr[i];
  }
  return sum;
}

export function createCounter(initial: i32): i32 {
  return store<u32>(0, initial, 0);
}
