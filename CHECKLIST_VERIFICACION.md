# ✅ CHECKLIST DE VERIFICACIÓN

## Después de aplicar los cambios, verifica lo siguiente:

### 🖥️ Compilación
- [ ] El proyecto compila sin errores: `dotnet build`
- [ ] No hay errores en los archivos .cshtml
- [ ] No hay errores en los controladores

### 🌐 Navegación
- [ ] La página principal carga correctamente
- [ ] El área Admin es accesible
- [ ] Los enlaces del menú funcionan (Dashboard, Productos, Clientes, Ventas)
- [ ] El logout funciona correctamente

### 🎨 Estilos (IMPORTANTE)
- [ ] Bootstrap se carga (verifica en DevTools Network)
- [ ] Los botones tienen colores (azul, rojo, verde, etc.)
- [ ] El sidebar de Admin tiene el gradiente azul
- [ ] Las tablas tienen el diseño de Bootstrap
- [ ] Los formularios están bien estilizados
- [ ] Los campos de entrada tienen bordes y padding

### 📱 Responsive
- [ ] El menú colapsa en móvil
- [ ] Las tablas son responsive
- [ ] El layout se adapta a diferentes tamaños de pantalla

### ✍️ Formularios
- [ ] Los formularios de crear/editar productos funcionan
- [ ] La validación del lado del cliente funciona
- [ ] Los mensajes de error se muestran correctamente
- [ ] Los select/dropdown funcionan

### 🔍 DevTools (F12)
- [ ] No hay errores 404 en la consola
- [ ] Los archivos CSS se cargan desde cdn.jsdelivr.net
- [ ] jQuery se carga desde code.jquery.com
- [ ] No hay errores de JavaScript

### 🧪 Funcionalidades Core
- [ ] Crear producto funciona
- [ ] Editar producto funciona
- [ ] Eliminar producto funciona
- [ ] Ver detalles funciona
- [ ] La lista de productos se muestra correctamente
- [ ] La lista de clientes se muestra
- [ ] La lista de ventas se muestra

---

## 🐛 Si algo no funciona:

### Bootstrap no se carga
1. Abre DevTools (F12)
2. Ve a Network/Red
3. Busca bootstrap.min.css
4. Si hay error, verifica tu conexión a Internet
5. Los CDN requieren conexión a Internet

### Los estilos se ven diferentes
- Es normal, ahora usas Bootstrap 5.3.2
- Si algo se ve roto, puede ser CSS personalizado en site.css

### Los formularios no validan
- Verifica que jquery-validation se cargue
- Mira la consola del navegador por errores
- Asegúrate de que _ValidationScriptsPartial esté incluido

---

## 📝 Comandos Útiles

```bash
# Limpiar y compilar
dotnet clean
dotnet build

# Ejecutar la aplicación
dotnet run --project Firmeza.Web/Firmeza.Web.csproj

# Ver logs detallados
dotnet run --project Firmeza.Web/Firmeza.Web.csproj --verbosity detailed
```

---

## 🆘 Problemas Comunes

### "No se puede cargar Bootstrap"
**Solución**: Verifica tu conexión a Internet. Los CDN requieren conexión.

### "Los estilos no se aplican"
**Solución**: 
1. Limpia el caché del navegador (Ctrl+Shift+Del)
2. Recarga la página con Ctrl+F5
3. Verifica DevTools que los CSS se carguen

### "Error 404 en algún archivo CSS/JS"
**Solución**: Revisa que NO haya referencias a `~/lib/` en los layouts

---

## ✅ Todo Funciona

Si todos los checks están marcados, ¡felicidades! Tu aplicación está funcionando correctamente en Windows.

Los cambios también funcionarán en Linux sin problemas.

