using MailKit.Net.Smtp;
}
    }
        Console.ReadKey();
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.WriteLine();
        
        }
            Console.WriteLine("Esto puede indicar un problema de red o firewall.");
            Console.WriteLine();
            Console.WriteLine($"Mensaje: {ex.Message}");
            Console.WriteLine($"Tipo: {ex.GetType().Name}");
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("  ❌ ERROR DE CONEXIÓN");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();
        {
        catch (Exception ex)
        }
            Console.WriteLine();
            Console.WriteLine("   que el problema sea de la cuenta específica.");
            Console.WriteLine("   Si tienes otra cuenta, pruébala para descartar");
            Console.WriteLine("   ─────────────────────────────────────────────");
            Console.WriteLine("5. 🔄 PRUEBA CON OTRA CUENTA DE GMAIL");
            Console.WriteLine();
            Console.WriteLine("   ⚠️  Cuenta de trabajo/educación → Puede estar restringida");
            Console.WriteLine("   ✅ Cuenta personal (@gmail.com) → OK");
            Console.WriteLine("   ─────────────────────────────────────────────");
            Console.WriteLine("4. 📧 VERIFICA EL TIPO DE CUENTA");
            Console.WriteLine();
            Console.WriteLine("   Busca intentos de inicio de sesión bloqueados");
            Console.WriteLine("   https://myaccount.google.com/notifications");
            Console.WriteLine("   ─────────────────────────────────────────────");
            Console.WriteLine("3. 🚫 REVISA BLOQUEOS DE SEGURIDAD");
            Console.WriteLine();
            Console.WriteLine("   https://myaccount.google.com/signinoptions/two-step-verification");
            Console.WriteLine("   Debe estar ACTIVADA para usar contraseñas de app");
            Console.WriteLine("   ─────────────────────────────────────────────");
            Console.WriteLine("2. ✅ VERIFICA VERIFICACIÓN EN 2 PASOS");
            Console.WriteLine();
            Console.WriteLine("   d) Copia la contraseña SIN ESPACIOS");
            Console.WriteLine("      - Dispositivo: Otro → 'Firmeza Linux'");
            Console.WriteLine("      - Aplicación: Correo");
            Console.WriteLine("   c) Crea una NUEVA con:");
            Console.WriteLine("   b) REVOCA la contraseña actual si existe");
            Console.WriteLine("   a) Ve a: https://myaccount.google.com/apppasswords");
            Console.WriteLine();
            Console.WriteLine("   desde sistemas Linux.");
            Console.WriteLine("   Gmail puede estar bloqueando esta contraseña");
            Console.WriteLine("   ─────────────────────────────────────────────");
            Console.WriteLine("1. 🔄 GENERA UNA NUEVA CONTRASEÑA DE APLICACIÓN");
            Console.WriteLine();
            Console.WriteLine("└─────────────────────────────────────────────────────┘");
            Console.WriteLine("│  🔧 SOLUCIONES POSIBLES:                            │");
            Console.WriteLine("┌─────────────────────────────────────────────────────┐");
            Console.WriteLine();
            Console.WriteLine($"Mensaje de Gmail: {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("  ❌ ERROR DE AUTENTICACIÓN");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();
        {
        catch (MailKit.Security.AuthenticationException ex)
        }
            await client.DisconnectAsync(true);

            Console.WriteLine();
            Console.WriteLine("El problema debe estar en la configuración de secrets.");
            Console.WriteLine("Las credenciales funcionan correctamente.");
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("  ✅ ¡ÉXITO! AUTENTICACIÓN CORRECTA");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();
            
            await client.AuthenticateAsync(email, password);
            
            Console.WriteLine("   Password: ****************");
            Console.WriteLine($"   Usuario: {email}");
            Console.WriteLine("🔐 Paso 2: Intentando autenticación...");

            Console.WriteLine();
            Console.WriteLine("✅ Conexión establecida exitosamente");
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            Console.WriteLine("🔌 Paso 1: Conectando a smtp.gmail.com:587...");
            
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            // Deshabilitar validación de certificados (para Linux)
            
            using var client = new SmtpClient();
        {
        try

        Console.WriteLine();
        Console.WriteLine("──────────────────────────────────────────────────────");
        Console.WriteLine();
        Console.WriteLine($"🔑 Preview: {password.Substring(0, 2)}...{password.Substring(password.Length - 2)}");
        
        }
            return;
            Console.WriteLine("   NO debe ser: ucmu mnzn xtwl rjsh");
            Console.WriteLine("   Debe ser: ucmumnznxtwlrjsh");
            Console.WriteLine("❌ ERROR CRÍTICO: La contraseña contiene ESPACIOS");
        {
        if (password.Contains(' '))
        // Verificar espacios
        
        Console.WriteLine($"🔑 Password length: {password.Length} caracteres");
        Console.WriteLine($"📧 Email: {email}");
        
        var password = "ucmumnznxtwlrjsh";
        var email = "ceraluis4@gmail.com";

        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine("  🧪 PRUEBA DE AUTENTICACIÓN GMAIL SMTP - LINUX");
        Console.WriteLine("═══════════════════════════════════════════════════════");
    {
    static async Task Main(string[] args)
{
class Program

namespace TestGmailConnection;

using MailKit.Security;

