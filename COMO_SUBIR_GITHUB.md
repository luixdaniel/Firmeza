# 🚀 Cómo Subir a GitHub - Guía Rápida

## Opción 1: Script Automático (Recomendado) ⚡

```bash
cd /home/Coder/Escritorio/Firmeza
./subir_a_github.sh
```

El script hará:
1. ✅ Verificar que no haya credenciales expuestas
2. ✅ Agregar todos los archivos con `git add .`
3. ✅ Hacer commit con mensaje descriptivo
4. ✅ Preguntar si quieres hacer push
5. ✅ Subir a GitHub

---

## Opción 2: Manual (Paso a Paso)

### Si es un NUEVO repositorio:

```bash
cd /home/Coder/Escritorio/Firmeza

# 1. Inicializar Git (si no está)
git init

# 2. Agregar archivos
git add .

# 3. Commit
git commit -m "Initial commit: Sistema Firmeza completo"

# 4. Crear repo en GitHub (en el navegador)
# Ve a: https://github.com/new
# Nombre: firmeza
# NO inicialices con README (ya tienes uno)

# 5. Conectar con GitHub
git remote add origin https://github.com/TU_USUARIO/firmeza.git

# 6. Subir
git branch -M main
git push -u origin main
```

### Si ACTUALIZAS un repositorio existente:

```bash
cd /home/Coder/Escritorio/Firmeza

# 1. Ver cambios
git status

# 2. Agregar cambios
git add .

# 3. Commit
git commit -m "fix: Corregir email SMTP y documentación"

# 4. Subir
git push origin main
```

---

## ✅ Verificación antes de Subir

```bash
# Ver qué se va a subir
git status

# Verificar que no haya credenciales
grep -r "Password=" . --include="*.json" --exclude-dir={bin,obj,node_modules}

# Ver el .gitignore
cat .gitignore | grep -E "secrets|.env|bin"
```

---

## 🔒 Seguridad Garantizada

Tu repositorio está configurado para **NO subir**:
- ❌ `secrets.json` - Credenciales locales
- ❌ `.env` - Variables de entorno
- ❌ `bin/`, `obj/` - Binarios compilados
- ❌ `node_modules/` - Dependencias
- ❌ Archivos temporales de prueba

---

## 📝 Después de Subir

1. **Verifica en GitHub** que todo se subió:
   ```
   https://github.com/TU_USUARIO/firmeza
   ```

2. **Clona en otro lugar** para probar:
   ```bash
   cd /tmp
   git clone https://github.com/TU_USUARIO/firmeza.git
   cd firmeza
   # Configurar secrets...
   docker-compose up
   ```

3. **Comparte el repositorio** con tu equipo

---

## 🆘 Solución de Problemas

### "No tienes permisos para hacer push"
```bash
# Configura tu token de GitHub
git remote set-url origin https://TU_TOKEN@github.com/TU_USUARIO/firmeza.git
```

### "El remote 'origin' ya existe"
```bash
# Ver remotes actuales
git remote -v

# Cambiar URL
git remote set-url origin https://github.com/TU_USUARIO/firmeza.git
```

### "Conflictos al hacer push"
```bash
# Traer cambios del remoto primero
git pull origin main --rebase

# Luego subir
git push origin main
```

---

## 🎯 TODO ESTÁ LISTO

Tu proyecto Firmeza:
- 🔒 **NO expone credenciales** - secrets.json está excluido
- 📚 **Tiene documentación completa** - READMEs en cada proyecto
- 🐳 **Es reproducible** - Docker Compose configurado
- 🧪 **Tiene tests** - Suite de pruebas incluida
- 🚀 **Está production-ready** - Configuración para despliegue

**¡Solo ejecuta el script o los comandos de arriba!** 🎉

---

## ⚡ Quick Start para otros

Cuando alguien clone tu repo:

```bash
# 1. Clonar
git clone https://github.com/TU_USUARIO/firmeza.git
cd firmeza

# 2. Configurar secrets
cd ApiFirmeza.Web
dotnet user-secrets set "EmailSettings:SenderPassword" "su_password"
# ... más secrets

# 3. Ejecutar con Docker
cd ..
docker-compose up --build
```

**Documentación completa en**: `ApiFirmeza.Web/SECRETS_SETUP.md`

