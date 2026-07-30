export interface Logger {
  log(message: string): void
}

export function createConsoleLogger(prefix: string): Logger {
  return {
    log(message: string) {
      console.log(`[${prefix}] ${message}`)
    },
  }
}

export const DEFAULT_PREFIX = 'App'
