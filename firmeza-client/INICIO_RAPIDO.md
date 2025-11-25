# 🚀 Inicio Rápido - Firmeza Client

## ⚡ Solución Rápida al Error de Autoprefixer

Si ves el error: **"Cannot find module 'autoprefixer'"**

### Solución en 2 pasos:

```bash
# 1. Instalar dependencias faltantes
npm install autoprefixer postcss

# 2. Iniciar el servidor
npm run dev
```

✅ **¡Listo!** El servidor debería iniciar en http://localhost:3000

---

## 🎯 Inicio Completo (Primera vez)

### 1. Instalar todas las dependencias
```bash
cd C:\Users\luisc\RiderProjects\Firmeza\firmeza-client
npm install
```

### 2. Verificar que las dependencias estén correctas
```bash
npm list autoprefixer postcss tailwindcss
```

Deberías ver:
```
firmeza-client@0.1.0
├── autoprefixer@10.4.22
├── postcss@8.5.6
└── tailwindcss@3.4.18
```

### 3. Iniciar el servidor de desarrollo
```bash
npm run dev
```

### 4. Abrir el navegador
Abre: **http://localhost:3000**

---

## 🐛 Problemas Comunes

### Error: Cannot find module 'X'
```bash
npm install
```

### El servidor no inicia
```bash
# Limpiar caché y reinstalar
rmdir /s /q node_modules
del package-lock.json
npm install
npm run dev
```

### Puerto 3000 ocupado
```bash
# Cambiar puerto
npm run dev -- -p 3001
```

---

## 📝 Credenciales de Prueba

```
Email: admin@firmeza.com
Password: Admin123$
```

⚠️ **Nota:** La contraseña termina con `$` (símbolo de dólar)

---

## ✅ Checklist

- [ ] Node.js 18+ instalado
- [ ] API corriendo en http://localhost:5090
- [ ] Dependencias instaladas (`npm install`)
- [ ] Autoprefixer instalado
- [ ] Servidor iniciado (`npm run dev`)
- [ ] Navegador abierto en http://localhost:3000

---

## 🎬 Script de Inicio Automático

**Opción 1: Doble click en el archivo**
```
iniciar-cliente.bat
```

**Opción 2: Desde la terminal**
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\firmeza-client
npm run dev
```

---

## 📚 Más Información

- Ver `README.md` para documentación completa
- Ver archivos en `app/` para código de páginas
- Ver `services/api.ts` para llamadas a la API

---

**¡Disfruta desarrollando!** 🎉

