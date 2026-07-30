function App() {
  const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:3000'
  const env = import.meta.env.MODE

  return (
    <div>
      <h1>Deployment</h1>
      <p>Environment: <strong>{env}</strong></p>
      <p>API URL: <code>{apiUrl}</code></p>

      <section>
        <h2>Build Commands</h2>
        <pre>{`npm run build      # builds to dist/
npm run preview    # preview production build`}</pre>
      </section>

      <section>
        <h2>Deploy to Netlify</h2>
        <pre>{`# netlify.toml
[build]
  command = "npm run build"
  publish = "dist"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200`}</pre>
      </section>

      <section>
        <h2>Deploy to Vercel</h2>
        <pre>{`# vercel.json
{
  "buildCommand": "npm run build",
  "outputDirectory": "dist",
  "framework": "vite",
  "rewrites": [{ "source": "/(.*)", "destination": "/index.html" }]
}`}</pre>
      </section>
    </div>
  )
}

export default App
