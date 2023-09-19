using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Areas.Identity.Pages.Account;
using mobu_backend.Data;
using mobu_backend.Models;
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
    /// ferramenta com acesso aos papeis de privilegios de cada utilizador
    /// </summary>
    private readonly RoleManager<IdentityRole> _roleManager;
    
    /// <summary>
    /// Este recurso (tecnicamente, um atributo) mostra os 
    /// dados do servidor. 
    /// E necessário inicializar este atributo no construtor da classe
    /// </summary>
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Interface para a funcao de logging do Remetente de emails
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
        RoleManager<IdentityRole> roleManager,
        ILogger<EmailSender> loggerEmail,
        IHttpContextAccessor http,
        IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<LoginApiController> logger
    )
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _loggerEmail = loggerEmail;
        _http = http;
        _optionsAccessor = optionsAccessor;
    }

    [HttpPost]
    [Route("api/forgot-password/send-email")]
    public async Task<IActionResult> SendEmail([FromBody] string emailJson)
    {
        try{
            StatusCodeResult status;
            var request = _http.HttpContext.Request;
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            JObject registerData = JObject.Parse(emailJson);

            var email = registerData.Value<string>("email");
            
            // verificacao e envio de email
            var identityUserList = await  _userManager.GetUsersInRoleAsync("Registered");
            var identityUser = identityUserList.FirstOrDefault(u => u.Email == email);

            if(identityUser != null){

                valid = true;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var href = "https://" + request.Host.ToString() + "?email=" + identityUser.Email; //INSERIR ROTA PARA O FORMULARIO DE MUDANCA DE PASSWORD

                var htmlElement = "Para mudar a sua palavra-passe <a href='" + href + "' target='_blank'>clique aqui</a>.";

                EmailSender emailSender = new(_optionsAccessor, _loggerEmail);

                await emailSender.SendEmailAsync(identityUser.Email, "Mude a sua palavra passe", htmlElement);
            }
            

            status = valid ? NoContent() : NotFound();

            return status;

        }catch(Exception ex){
            _logger.LogError($"Erro no envio de email: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error

        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
        
    }

    [HttpPost]
    [Route("api/forgot-password/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] string passwordJson, string email)
    {
        try{
            StatusCodeResult status;
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            JObject registerData = JObject.Parse(passwordJson);
            var newPassword = registerData.Value<string>("newPassword");
            var currPassword = registerData.Value<string>("currentPassword");
            
            // mudanca de password
            var identityUserList = await  _userManager.GetUsersInRoleAsync("Registered");
            var identityUser = identityUserList.FirstOrDefault(u => u.Email == email);

            if(identityUser != null && newPassword != "" && currPassword != ""){
                valid = true;
                await _userManager.ChangePasswordAsync(identityUser, currPassword, newPassword);
            }

            valid = true;
            
            status = valid ? NoContent() : NotFound();

            return status;
        }catch(Exception ex){
             _logger.LogError($"Erro na mudança de password: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
    }
}
