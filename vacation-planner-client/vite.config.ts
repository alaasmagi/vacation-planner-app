import { defineConfig, loadEnv} from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  const url = new URL(env.VITE_FRONTEND_URL ?? "http://localhost:8080")

  return {
    plugins: [react()],
    server: {
      host: url.hostname,
      port: Number(url.port),
    },
  }
})