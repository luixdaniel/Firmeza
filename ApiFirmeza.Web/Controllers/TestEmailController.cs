using ApiFirmeza.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiFirmeza.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestEmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<TestEmailController> _logger;
    private readonly IConfiguration _configuration;

    public TestEmailController(IEmailService emailService, ILogger<TestEmailController> logger, IConfiguration configuration)
    {
        _emailService = emailService;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Endpoint para probar las credenciales SMTP sin enviar correo
    /// </summary>
    [HttpGet("test-credentials")]
    [AllowAnonymous]
    public async Task<IActionResult> TestCredentials()
    {
        try
        {
            var email = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:SenderPassword"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new 
                { 
                    success = false, 
                    message = "Credenciales no configuradas en secrets.json",
                    email = string.IsNullOrEmpty(email) ? "FALTA" : "OK",
                    password = string.IsNullOrEmpty(password) ? "FALTA" : "OK"
                });
            }

            _logger.LogInformation("🧪 Probando credenciales SMTP...");
            var resultado = await GmailConnectionTest.TestGmailConnection(email, password);

            if (resultado)
            {
                return Ok(new 
                { 
                    success = true, 
                    message = "✅ Credenciales verificadas correctamente",
                    email = email,
                    passwordLength = password.Length
                });
            }
            else
            {
                return BadRequest(new 
                { 
                    success = false, 
                    message = "❌ Error de autenticación. Revisa los logs para más detalles.",
                    email = email,
                    passwordLength = password.Length
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al probar credenciales");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Endpoint de prueba para verificar el envío de correos
    /// </summary>
    [HttpPost("send-test")]
    [AllowAnonymous]
    public async Task<IActionResult> SendTestEmail([FromBody] TestEmailRequest request)
    {
        try
        {
            _logger.LogInformation("🧪 Iniciando prueba de envío de correo a {Email}", request.Email);

            // Generar un PDF de prueba simple
            var pdfBytes = GenerarPdfPrueba();

            var resultado = await _emailService.EnviarComprobanteCompraAsync(
                destinatario: request.Email,
                nombreCliente: "Usuario de Prueba",
                ventaId: 9999,
                total: 100.00m,
                numeroFactura: "TEST-001",
                pdfBytes: pdfBytes
            );

            if (resultado)
            {
                _logger.LogInformation("✅ Correo de prueba enviado exitosamente");
                return Ok(new 
                { 
                    success = true, 
                    message = "Correo de prueba enviado exitosamente a " + request.Email,
                    instructions = "Revisa tu bandeja de entrada y carpeta de spam"
                });
            }
            else
            {
                _logger.LogError("❌ Fallo al enviar correo de prueba");
                return StatusCode(500, new 
                { 
                    success = false, 
                    message = "Error al enviar el correo. Revisa los logs de la aplicación para más detalles.",
                    tip = "Asegúrate de que la configuración EmailSettings esté correctamente configurada en appsettings.json"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Excepción al enviar correo de prueba");
            return StatusCode(500, new 
            { 
                success = false, 
                message = "Error al enviar correo: " + ex.Message,
                type = ex.GetType().Name
            });
        }
    }

    private byte[] GenerarPdfPrueba()
    {
        // Genera un PDF simple de prueba
        var contenido = @"%PDF-1.4
1 0 obj
<< /Type /Catalog /Pages 2 0 R >>
endobj
2 0 obj
<< /Type /Pages /Kids [3 0 R] /Count 1 >>
endobj
3 0 obj
<< /Type /Page /Parent 2 0 R /Resources 4 0 R /MediaBox [0 0 612 792] /Contents 5 0 R >>
endobj
4 0 obj
<< /Font << /F1 << /Type /Font /Subtype /Type1 /BaseFont /Helvetica >> >> >>
endobj
5 0 obj
<< /Length 44 >>
stream
BT
/F1 18 Tf
100 700 Td
(Comprobante de Prueba) Tj
ET
endstream
endobj
xref
0 6
0000000000 65535 f 
0000000009 00000 n 
0000000058 00000 n 
0000000115 00000 n 
0000000214 00000 n 
0000000309 00000 n 
trailer
<< /Size 6 /Root 1 0 R >>
startxref
402
%%EOF";
        return System.Text.Encoding.UTF8.GetBytes(contenido);
    }
}

public class TestEmailRequest
{
    public string Email { get; set; } = string.Empty;
}

