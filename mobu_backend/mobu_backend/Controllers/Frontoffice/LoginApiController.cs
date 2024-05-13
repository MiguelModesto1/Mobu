using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.ApiModels;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
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


    private readonly IConfiguration _config;

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
        ILogger<LoginApiController> logger,
        IConfiguration config
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
        _config = config;
    }


    [HttpPost]
    [Route("api/login")]
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

            // Validacao de id e password
            var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            //user da BD
            var user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.NomeUtilizador == username);

            var userId = 0;

            if (identityUser != null && user != null)
            {
                userId = user.IDUtilizador;
                
                var confirmPassword = await _userManager.CheckPasswordAsync(identityUser, password);
                if (confirmPassword)
                {
                    valid = true;

                    await _signInManager.SignInAsync(identityUser, true);

                    // criar cookie e adicionar nova claim a BD

                    var guid = Guid.NewGuid();
                    var claimValue = guid.ToString();
                    var claim = new IdentityUserClaim<string>
                    {
                        UserId = identityUser.Id,
                        ClaimType = "Session-Id",
                        ClaimValue = claimValue
                    };

                    _context.UserClaims.Attach(claim);
                    await _context.SaveChangesAsync();

                    Response.Cookies.Append("Session-Id", claimValue, new CookieOptions{HttpOnly=true, Secure=true, Expires=DateTimeOffset.Now.AddMinutes(15)});

                }
            }

            info.Add("userId", userId);
            resp = valid ? Ok(info.ToJson()) : NotFound();

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

    [HttpPost]
    [Authorize]
    [Route("api/logout")]
    public async Task<IActionResult> Logout([FromBody] Logout logoutDataJson)
    {
        try
        {

            IActionResult resp;
            JObject info = new();
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados

            var id = logoutDataJson.Id;

            //user da BD
            var user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);

            // Validacao de id
            var identityUser = await _context.Users.FirstOrDefaultAsync(u => user.AuthenticationID == u.Id);

            if (identityUser != null && user != null)
            {
                //validar cookie de sessao
                if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
                {
                    return Unauthorized();
                }

                var sessionClaim = await _context.UserClaims
                    .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && identityUser.Id == u.UserId);

                if(sessionClaim == null)
                {
                    return Unauthorized();
                }

                valid = true;

                await _signInManager.SignOutAsync();

                // apagar cookie de sessao e claim
                Response.Cookies.Delete("Session-Id");

                _context.Remove(sessionClaim);
                await _context.SaveChangesAsync();
            }

            resp = valid ? Ok() : NotFound();

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
