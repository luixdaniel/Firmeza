/** @type {import('next').NextConfig} */
const nextConfig = {
  // Variables de entorno públicas
  env: {
    NEXT_PUBLIC_API_URL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5090',
  },
  
  // Output standalone para Docker
  // Esto crea un build optimizado para producción con servidor standalone
  output: 'standalone',
}

module.exports = nextConfig

