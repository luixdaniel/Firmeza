using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ApiFirmeza.Web.Services;

/// <summary>
/// Clase de prueba para verificar conexión SMTP de Gmail
/// Ejecutar desde Program.cs temporalmente o desde tests
/// </summary>
public class GmailConnectionTest
{
    public static async Task<bool> TestGmailConnection(string email, string password)
    {
        Console.WriteLine("🔍 Iniciando prueba de conexión SMTP Gmail...");
        Console.WriteLine($"📧 Email: {email}");
        Console.WriteLine($"🔑 Password length: {password.Length} caracteres");
        
        // Verificar espacios en la contraseña
        if (password.Contains(" "))
        {
            Console.WriteLine("❌ ERROR: La contraseña contiene ESPACIOS");
            Console.WriteLine("   Las contraseñas de aplicación de Gmail NO deben tener espacios");
            Console.WriteLine("   Ejemplo INCORRECTO: 'xxxx xxxx xxxx xxxx'");
            Console.WriteLine("   Ejemplo CORRECTO: 'xxxxxxxxxxxxxxxx'");
            return false;
        }
        
        Console.WriteLine($"🔑 Password preview: {password.Substring(0, 2)}...{password.Substring(password.Length - 2)}");
        Console.WriteLine();

        try
        {
            using var client = new SmtpClient();
            
            // Habilitar logging detallado
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            
            Console.WriteLine("🔌 Conectando a smtp.gmail.com:587...");
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            Console.WriteLine("✅ Conexión establecida");
            
            Console.WriteLine("🔐 Intentando autenticación...");
            await client.AuthenticateAsync(email, password);
            Console.WriteLine("✅ Autenticación exitosa!");
            
            await client.DisconnectAsync(true);
            Console.WriteLine("✅ Desconectado correctamente");
            Console.WriteLine();
            Console.WriteLine("🎉 ¡PRUEBA EXITOSA! Las credenciales funcionan correctamente");
            
            return true;
        }
        catch (MailKit.Security.AuthenticationException ex)
        {
            Console.WriteLine();
            Console.WriteLine("❌ ERROR DE AUTENTICACIÓN:");
            Console.WriteLine($"   {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("📝 SOLUCIONES POSIBLES:");
            Console.WriteLine();
            Console.WriteLine("1. Verifica que la verificación en 2 pasos esté activada:");
            Console.WriteLine("   https://myaccount.google.com/signinoptions/two-step-verification");
            Console.WriteLine();
            Console.WriteLine("2. Genera una NUEVA contraseña de aplicación:");
            Console.WriteLine("   https://myaccount.google.com/apppasswords");
            Console.WriteLine("   - Selecciona 'Correo' como aplicación");
            Console.WriteLine("   - Selecciona 'Otro' y escribe 'Firmeza Linux'");
            Console.WriteLine();
            Console.WriteLine("3. Copia la contraseña SIN ESPACIOS:");
            Console.WriteLine("   - Google muestra: 'abcd efgh ijkl mnop'");
            Console.WriteLine("   - Debes usar: 'abcdefghijklmnop'");
            Console.WriteLine();
            Console.WriteLine("4. Actualiza en secrets con:");
            Console.WriteLine("   dotnet user-secrets set 'EmailSettings:SenderPassword' 'tu_contraseña_sin_espacios'");
            Console.WriteLine();
            Console.WriteLine("5. Si usas una cuenta de trabajo/educación, puede estar restringida");
            Console.WriteLine();
            Console.WriteLine("6. Revisa notificaciones de seguridad en:");
            Console.WriteLine("   https://myaccount.google.com/notifications");
            
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"❌ ERROR: {ex.GetType().Name}");
            Console.WriteLine($"   {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
            }
            return false;
        }
    }
}

