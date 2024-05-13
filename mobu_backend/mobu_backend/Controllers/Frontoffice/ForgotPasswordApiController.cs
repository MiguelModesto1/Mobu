using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Api_models;
using mobu_backend.ApiModels;
using mobu_backend.Data;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;

namespace mobu.Controllers.Frontend;

[ApiController]
public class ForgotPasswordApiController : ControllerBase
{

    /// <summary>
    /// objeto que referencia a Base de Dados do projeto
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// ferramenta com acesso a gestao de users
    /// </summary>
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Este recurso (tecnicamente, um atributo) mostra os 
    /// dados do servidor. 
    /// E necessário inicializar este atributo no construtor da classe
    /// </summary>
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Interface para a funcao de logging do Destinatario de emails
    /// </summary>
    private readonly ILogger<EmailSender> _loggerEmail;

    /// <summary>
    /// Interface que permite o acesso ao contexto HTTP
    /// </summary>
    private readonly IHttpContextAccessor _http;

    /// <summary>
    /// opcoes para ter acesso à chave da API do SendGrid
    /// </summary>
    private readonly IOptions<AuthMessageSenderOptions> _optionsAccessor;

    /// <summary>
    /// Interface para a funcao de logging no controller
    /// </summary>
    private readonly ILogger<LoginApiController> _logger;

    public ForgotPasswordApiController(
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        UserManager<IdentityUser> userManager,
        ILogger<EmailSender> loggerEmail,
        IHttpContextAccessor http,
        IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<LoginApiController> logger
    )
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
        _logger = logger;
        _loggerEmail = loggerEmail;
        _http = http;
        _optionsAccessor = optionsAccessor;
    }

    [HttpPost]
    [Route("api/forgot-password/send-email")]
    public async Task<IActionResult> SendEmail([FromBody] ForgotPassworEmail emailJson)
    {
        try
        {
            StatusCodeResult status;
            var request = _http.HttpContext.Request;
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            //JObject registerData = JObject.Parse(emailJson);
            var email = emailJson.Email;

            // verificacao e envio de email
            var identityUser = _context.Users.FirstOrDefault(u => u.Email == email);
            var user = _context.UtilizadorRegistado.FirstOrDefault(u => u.Email == email);

            if (identityUser != null && user != null)
            {

                valid = true;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var href = Environment.GetEnvironmentVariable("FRONTEND_APP_URL") + "/password-reset?email=" + user.Email;

                var htmlElement = "Para mudar a sua palavra-passe <a href='" + href + "'>clique aqui</a>.";

                EmailSender emailSender = new(_optionsAccessor, _loggerEmail);

                await emailSender.SendEmailAsync(identityUser.Email, "Mude a sua palavra passe", htmlElement);
            }


            status = valid ? Ok() : NotFound();

            return status;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro no envio de email: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error

        }
        finally
        {
            _logger.LogWarning("Saiu do método Post");
        }

    }

    [HttpPost]
    [Route("api/forgot-password/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPassword passwordResetJson)
    {
        try
        {
            StatusCodeResult status;
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            var newPassword = passwordResetJson.NewPassword;
            var currPassword = passwordResetJson.CurrentPassword;
            var email = passwordResetJson.Email;

            // mudanca de password
            var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (identityUser != null && 
                newPassword != "" && newPassword != null && 
                currPassword != "" && currPassword != null)
            {
                valid = true;
                await _userManager.ChangePasswordAsync(identityUser, currPassword, newPassword);
            }

            status = valid ? Ok() : BadRequest();

            return status;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro na mudança de password: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }
        finally
        {
            _logger.LogWarning("Saiu do método Post");
        }
    }
}
