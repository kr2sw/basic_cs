import { Config, fetchData, VERSION } from './types'

const config: Config = {
  apiUrl: 'https://api.example.com',
  timeout: 5000,
}

console.log(`Version: ${VERSION}`)
console.log(`Config: ${JSON.stringify(config)}`)

async function loadUser(id: number) {
  const data = await fetchData<{ id: number; name: string }>(`${config.apiUrl}/users/${id}`)
  console.log(data)
}
