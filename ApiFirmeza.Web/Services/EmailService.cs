using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiFirmeza.Web.Services;

public interface IEmailService
{
    Task<bool> EnviarComprobanteCompraAsync(string destinatario, string nombreCliente, 
        int ventaId, decimal total, string numeroFactura, byte[] pdfBytes);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> EnviarComprobanteCompraAsync(
        string destinatario, 
        string nombreCliente, 
        int ventaId, 
        decimal total, 
        string numeroFactura,
        byte[] pdfBytes)
    {
        try
        {
            _logger.LogInformation("📧 Iniciando envío de comprobante de compra a {Email}", destinatario);

            // Configuración SMTP desde appsettings
            var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];
            var senderName = _configuration["EmailSettings:SenderName"] ?? "Firmeza - Tienda";

            _logger.LogInformation("🔧 Configuración SMTP: Host={Host}, Port={Port}, From={From}", 
                smtpHost, smtpPort, senderEmail);

            if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
            {
                _logger.LogError("❌ Configuración de email incompleta. Verifica EmailSettings en appsettings.json");
                _logger.LogError("   SenderEmail: {HasEmail}", string.IsNullOrEmpty(senderEmail) ? "FALTA" : "OK");
                _logger.LogError("   SenderPassword: {HasPassword}", string.IsNullOrEmpty(senderPassword) ? "FALTA" : "OK");
                return false;
            }

            // Crear el mensaje
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress(nombreCliente, destinatario));
            message.Subject = $"Comprobante de Compra - Factura {numeroFactura}";

            // Crear el cuerpo del mensaje
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                                     color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                            .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                            .invoice-details {{ background: white; padding: 20px; border-radius: 8px; margin: 20px 0; }}
                            .invoice-row {{ display: flex; justify-content: space-between; padding: 10px 0; 
                                          border-bottom: 1px solid #eee; }}
                            .total {{ font-size: 1.3em; font-weight: bold; color: #667eea; }}
                            .footer {{ text-align: center; color: #666; margin-top: 30px; font-size: 0.9em; }}
                            .button {{ display: inline-block; padding: 12px 30px; background: #667eea; 
                                     color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>¡Gracias por tu compra!</h1>
                                <p>Tu pedido ha sido procesado exitosamente</p>
                            </div>
                            <div class='content'>
                                <p>Hola <strong>{nombreCliente}</strong>,</p>
                                <p>Tu compra ha sido confirmada. Adjuntamos el comprobante en formato PDF con todos los detalles.</p>
                                
                                <div class='invoice-details'>
                                    <h3 style='margin-top: 0; color: #667eea;'>Detalles de la Compra</h3>
                                    <div class='invoice-row'>
                                        <span>Número de Factura:</span>
                                        <strong>{numeroFactura}</strong>
                                    </div>
                                    <div class='invoice-row'>
                                        <span>ID de Venta:</span>
                                        <strong>#{ventaId}</strong>
                                    </div>
                                    <div class='invoice-row'>
                                        <span>Fecha:</span>
                                        <strong>{DateTime.Now:dd/MM/yyyy HH:mm}</strong>
                                    </div>
                                    <div class='invoice-row total'>
                                        <span>Total Pagado:</span>
                                        <span>${total:N2}</span>
                                    </div>
                                </div>

                                <p>El comprobante detallado está adjunto a este correo en formato PDF.</p>
                                
                                <div style='text-align: center;'>
                                    <p>¿Tienes alguna pregunta? Contáctanos</p>
                                </div>
                            </div>
                            <div class='footer'>
                                <p>Este es un correo automático, por favor no respondas a este mensaje.</p>
                                <p>&copy; {DateTime.Now.Year} Firmeza. Todos los derechos reservados.</p>
                            </div>
                        </div>
                    </body>
                    </html>"
            };

            // Adjuntar el PDF
            if (pdfBytes != null && pdfBytes.Length > 0)
            {
                builder.Attachments.Add($"Comprobante_{numeroFactura}.pdf", pdfBytes, 
                    new ContentType("application", "pdf"));
                _logger.LogInformation("📎 PDF adjunto: {Size} bytes", pdfBytes.Length);
            }

            message.Body = builder.ToMessageBody();

            // Enviar el correo usando SMTP
            using (var client = new SmtpClient())
            {
                _logger.LogInformation("🔌 Conectando al servidor SMTP {Host}:{Port}...", smtpHost, smtpPort);
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                _logger.LogInformation("✅ Conectado al servidor SMTP");

                _logger.LogInformation("🔐 Autenticando con {Email}...", senderEmail);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                _logger.LogInformation("✅ Autenticación exitosa");

                _logger.LogInformation("📤 Enviando mensaje...");
                await client.SendAsync(message);
                _logger.LogInformation("✅ Mensaje enviado");

                await client.DisconnectAsync(true);
                _logger.LogInformation("🔌 Desconectado del servidor SMTP");
            }

            _logger.LogInformation("✅ Correo enviado exitosamente a {Email}", destinatario);
            return true;
        }
        catch (MailKit.Security.AuthenticationException authEx)
        {
            _logger.LogError(authEx, "❌ Error de autenticación SMTP. Verifica el email y contraseña de aplicación.");
            _logger.LogError("   Asegúrate de usar una 'Contraseña de Aplicación' de Gmail, no la contraseña normal.");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al enviar correo a {Email}: {Message}", destinatario, ex.Message);
            _logger.LogError("   Tipo de error: {Type}", ex.GetType().Name);
            if (ex.InnerException != null)
            {
                _logger.LogError("   Error interno: {InnerMessage}", ex.InnerException.Message);
            }
            return false;
        }
    }
}

