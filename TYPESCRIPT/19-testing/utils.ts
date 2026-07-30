export function add(a: number, b: number): number {
  return a + b
}

export function greet(name: string, prefix = 'Hello'): string {
  return `${prefix}, ${name}!`
}

export function parseJSON<T>(json: string): T | null {
  try {
    return JSON.parse(json) as T
  } catch {
    return null
  }
}

export async function fetchWithTimeout<T>(url: string, ms = 5000): Promise<T> {
  const controller = new AbortController()
  const id = setTimeout(() => controller.abort(), ms)

  try {
    const res = await fetch(url, { signal: controller.signal })
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    return (await res.json()) as T
  } finally {
    clearTimeout(id)
  }
}
