// types.d.ts
export interface Config {
  apiUrl: string
  timeout: number
  retries?: number
}

export function fetchData<T>(url: string): Promise<T>

export const VERSION: string
