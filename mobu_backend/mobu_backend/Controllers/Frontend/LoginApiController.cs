using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
[Route("api/login")]
public class LoginApiController : ControllerBase
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
    /// ferramenta com acesso a gestao de acessos
    /// </summary>
    private readonly SignInManager<IdentityUser> _signInManager;

    /// <summary>
    /// Este recurso (tecnicamente, um atributo) mostra os 
    /// dados do servidor. 
    /// E necessário inicializar este atributo no construtor da classe
    /// </summary>
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Interface para a funcao de logging do DonoListaPedidos de emails
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

    public LoginApiController(
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<EmailSender> loggerEmail,
        IHttpContextAccessor http,
        IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<LoginApiController> logger
    )
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _loggerEmail = loggerEmail;
        _http = http;
        _optionsAccessor = optionsAccessor;
    }


    [HttpPost]
    public async Task<IActionResult> UserValidation([FromBody] Login loginDataJson)
    {
        try
        {
            IActionResult resp;
            JObject info = new();
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados

            var username = loginDataJson.NomeUtilizador;
            var password = loginDataJson.Password;

            // Validacao de username e password
            var identityUser = _context.Users.FirstOrDefault(u => u.UserName == username);

            //user da BD
            var user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.NomeUtilizador == username);

            var userId = 0;

            if (identityUser != null || user != null)
            {
                userId = user.IDUtilizador;
                var confirmPassword = await _userManager.CheckPasswordAsync(identityUser, password);
                if (confirmPassword)
                {
                    valid = true;
                    await _signInManager.SignInAsync(identityUser, true);
                }
            }

            info.Add("userId", userId);
            resp = valid ? Ok(info.ToJson()) : NotFound(); // Implementar rota pagina mensagens

            _logger.LogWarning("Saiu do método Post");

            return resp;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error no login: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }
        finally
        {
            _logger.LogWarning("Saiu do método Post");
        }
    }
}
