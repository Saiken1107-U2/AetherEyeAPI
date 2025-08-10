using System.Net;
using System.Net.Mail;

namespace AetherEyeAPI.Services
{
    public interface IEmailService
    {
        Task<bool> EnviarCredencialesUsuario(string destinatario, string nombreUsuario, string correo, string contrasena);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public EmailService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<bool> EnviarCredencialesUsuario(string destinatario, string nombreUsuario, string correo, string contrasena)
        {
            try
            {
                Console.WriteLine($"📧 Intentando enviar correo a: {destinatario}");

                // Obtener configuración de email
                var emailUsername = _configuration["Email:Username"];
                var emailPassword = _configuration["Email:Password"];
                
                if (string.IsNullOrEmpty(emailUsername) || string.IsNullOrEmpty(emailPassword))
                {
                    Console.WriteLine("⚠️ Credenciales de email no configuradas");
                    return false;
                }

                // Si la contraseña sigue siendo la de placeholder, mostrar en consola
                if (emailPassword.Contains("AQUI_VA_TU_CONTRASEÑA") || emailPassword == "M0c7ezum@")
                {
                    Console.WriteLine("🔧 MODO DESARROLLO - Contraseña de aplicación no configurada aún:");
                    Console.WriteLine($"📧 Para: {destinatario}");
                    Console.WriteLine($"📋 Asunto: 🔑 Bienvenido a AetherEye - Credenciales de Acceso");
                    Console.WriteLine($"👤 Usuario: {nombreUsuario}");
                    Console.WriteLine($"📧 Email: {correo}");
                    Console.WriteLine($"🔐 Contraseña: {contrasena}");
                    Console.WriteLine("💡 Para envío real: configura contraseña de aplicación de Gmail");
                    Console.WriteLine(new string('=', 50));
                    return true;
                }

                // Configurar cliente SMTP
                var smtpClient = new SmtpClient(_configuration["Email:SmtpHost"] ?? "smtp.gmail.com")
                {
                    Port = int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
                    Credentials = new NetworkCredential(emailUsername, emailPassword),
                    EnableSsl = true,
                };

                // Crear mensaje
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailUsername, "Sistema AetherEye"),
                    Subject = "🔑 Bienvenido a AetherEye - Credenciales de Acceso",
                    Body = GenerarPlantillaCorreo(nombreUsuario, correo, contrasena),
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(destinatario);

                // Enviar correo
                await smtpClient.SendMailAsync(mailMessage);
                
                Console.WriteLine($"✅ Correo enviado exitosamente a: {destinatario}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar correo: {ex.Message}");
                Console.WriteLine($"🔍 Detalles: {ex.InnerException?.Message}");
                
                // Mostrar información útil para depuración
                if (ex.Message.Contains("Authentication"))
                {
                    Console.WriteLine("💡 Error de autenticación: Verifica que hayas configurado una contraseña de aplicación de Gmail");
                }
                
                return false;
            }
        }

        private string GenerarPlantillaCorreo(string nombreUsuario, string correo, string contrasena)
        {
            return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Bienvenido a AetherEye</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
        .credentials {{ background: white; padding: 20px; border-radius: 8px; border-left: 4px solid #667eea; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 14px; }}
        .button {{ display: inline-block; background: #667eea; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 10px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🌟 ¡Bienvenido a AetherEye!</h1>
            <p>Tu cuenta ha sido creada exitosamente</p>
        </div>
        
        <div class='content'>
            <h2>Hola {nombreUsuario},</h2>
            
            <p>Te damos la bienvenida al sistema <strong>AetherEye</strong>. Tu cuenta de cliente ha sido creada por nuestro equipo administrativo.</p>
            
            <div class='credentials'>
                <h3>🔐 Tus credenciales de acceso:</h3>
                <p><strong>Correo electrónico:</strong> {correo}</p>
                <p><strong>Contraseña temporal:</strong> {contrasena}</p>
            </div>
            
            <div style='background: #fff3cd; border: 1px solid #ffeaa7; border-radius: 5px; padding: 15px; margin: 20px 0;'>
                <h4>⚠️ Importante:</h4>
                <ul>
                    <li>Te recomendamos cambiar tu contraseña después del primer inicio de sesión</li>
                    <li>Mantén tus credenciales seguras y no las compartas</li>
                    <li>Si tienes problemas para acceder, contacta a nuestro equipo</li>
                </ul>
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='http://localhost:51887/auth/login' class='button'>🚀 Iniciar Sesión Ahora</a>
            </div>
            
            <h3>📋 ¿Qué puedes hacer con tu cuenta?</h3>
            <ul>
                <li>✅ Solicitar cotizaciones de productos</li>
                <li>✅ Ver el historial de tus compras</li>
                <li>✅ Gestionar tu perfil personal</li>
                <li>✅ Dejar comentarios y calificaciones</li>
            </ul>
        </div>
        
        <div class='footer'>
            <p>Este correo fue generado automáticamente por el sistema AetherEye</p>
            <p>Si no solicitaste esta cuenta, por favor contacta a nuestro equipo</p>
            <p>&copy; 2025 AetherEye. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
