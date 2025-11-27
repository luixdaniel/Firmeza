# 🚀 INICIO RÁPIDO - Sistema Firmeza

## ⚠️ IMPORTANTE: Tres Portales Separados

Este sistema tiene **3 componentes independientes** que **NO deben mezclarse**:

```
┌─────────────────────────────────────────────────────┐
│  1. API REST         → Puerto 5000 (Backend)       │
│  2. Portal Admin     → Puerto 5002 (Admins)        │
│  3. Portal Clientes  → Puerto 3000 (Clientes)      │
└─────────────────────────────────────────────────────┘
```

---

## 🎯 ¿Qué Portal Usar?

### 👨‍💼 Si eres ADMINISTRADOR:
✅ Usa: **Firmeza.Web** (Puerto 5002)
- URL: `http://localhost:5002`
- Login: `http://localhost:5002/Identity/Account/Login`
- Usuario: `admin@firmeza.com`
- Password: `Admin123$`

### 👥 Si eres CLIENTE:
✅ Usa: **firmeza-client** (Puerto 3000)
- URL: `http://localhost:3000`
- Login: `http://localhost:3000/login`
- Usuario: `cliente@firmeza.com`
- Password: `Cliente123$`

---

## 🚀 Inicio Rápido

### Opción 1: Script Automático (Recomendado)
```bash
./iniciar-portales.sh
```

### Opción 2: Manual

#### Terminal 1 - API REST
```bash
cd ApiFirmeza.Web
dotnet run
# Se inicia en: http://localhost:5000
```

#### Terminal 2 - Portal Admin (Razor)
```bash
cd Firmeza.Web
dotnet run
# Se inicia en: http://localhost:5002
```

#### Terminal 3 - Portal Cliente (Next.js)
```bash
cd firmeza-client
npm install  # Solo la primera vez
npm run dev
# Se inicia en: http://localhost:3000
```

---

## 📋 Puertos Configurados

| Componente | Puerto HTTP | Puerto HTTPS | Propósito |
|-----------|------------|--------------|-----------|
| **API REST** | 5000 | 5001 | Backend para clientes |
| **Portal Admin** | 5002 | 5003 | Gestión administrativa |
| **Portal Cliente** | 3000 | - | Interfaz de clientes |

---

## 🔐 Credenciales de Prueba

### Portal Admin (Puerto 5002)
```
Email: admin@firmeza.com
Password: Admin123$
Rol: Admin
```

### Portal Cliente (Puerto 3000)
```
Email: cliente@firmeza.com
Password: Cliente123$
Rol: Cliente
```

---

## 🚫 Errores Comunes

### ❌ Error: "No sale el portal de admin"
**Problema:** Estás accediendo al puerto equivocado.

**Solución:**
- Portal Admin: `http://localhost:5002` (NO 5000)
- Portal Cliente: `http://localhost:3000`

### ❌ Error: "Intento hacer login de admin en el portal de clientes"
**Problema:** Los portales están separados.

**Solución:**
- Admins: Usen `http://localhost:5002`
- Clientes: Usen `http://localhost:3000`
- **NO mezclar los portales**

### ❌ Error: "El puerto está en uso"
**Problema:** Ya hay un servicio corriendo.

**Solución:**
```bash
# Linux/Mac
lsof -ti:5000 | xargs kill -9
lsof -ti:5002 | xargs kill -9
lsof -ti:3000 | xargs kill -9
```

---

## 📁 Estructura del Proyecto

```
Firmeza/
├── ApiFirmeza.Web/          # API REST (Puerto 5000)
│   └── Controllers/         # Endpoints de la API
├── Firmeza.Web/             # Portal Admin (Puerto 5002)
│   ├── Areas/Admin/         # Área administrativa
│   └── Areas/Identity/      # Login de Identity
└── firmeza-client/          # Portal Cliente (Puerto 3000)
    └── app/                 # Páginas de Next.js
```

---

## 📚 Documentación Completa

- **Arquitectura:** Lee `ARQUITECTURA_PORTALES.md`
- **Configuración JWT:** Lee `CONFIGURAR_SECRETS_JWT.md`
- **Probar API:** Lee `GUIA_PROBAR_SWAGGER.md`

---

## ✅ Checklist de Verificación

Después de iniciar, verifica:

- [ ] API REST responde en `http://localhost:5000/swagger`
- [ ] Portal Admin carga en `http://localhost:5002`
- [ ] Portal Cliente carga en `http://localhost:3000`
- [ ] Login de admin funciona en puerto 5002
- [ ] Login de cliente funciona en puerto 3000
- [ ] Los portales están completamente separados

---

## 🆘 Ayuda

Si tienes problemas:

1. Verifica que los 3 servicios estén corriendo
2. Revisa los puertos en `launchSettings.json`
3. Lee `ARQUITECTURA_PORTALES.md` para entender la separación
4. Asegúrate de no mezclar los portales

---

**¡Importante!** No intentes hacer login de administrador en el portal de clientes (puerto 3000). Cada portal tiene su propósito específico.

