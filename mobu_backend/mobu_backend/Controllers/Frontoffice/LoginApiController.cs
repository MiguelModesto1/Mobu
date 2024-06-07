using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.ApiModels;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json;
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

    /// <summary>
    /// Metodo da API para obter um novo cookie de sessao
    /// depois da meia-vida do atual
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("api/get-new-cookie")]
    public async Task<IActionResult> GetNewCookie([FromQuery(Name = "id")] int id)
    {
        JObject info = new();
        var expiry = new DateTimeOffset();

        //user da BD
        var user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);

        // Validacao de id
        var identityUser = await _context.Users.FirstOrDefaultAsync(u => user.AuthenticationID == u.Id);

        if (identityUser == null || user == null)
        {
            return Unauthorized();
        }

        //validar cookie de sessao
        if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
        {
            return Unauthorized();
        }

        var sessionClaim = await _context.UserClaims
            .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && identityUser.Id == u.UserId);

        if (sessionClaim == null)
        {
            return Unauthorized();
        }

        // Reatribuir novo cookie
        Response.Cookies.Delete("Session-Id");

        expiry = DateTimeOffset.Now.AddMinutes(15);
        Response.Cookies.Append("Session-Id", sessionId, new CookieOptions { HttpOnly = true, Secure = true, Expires = expiry });

        info.Add("expiryDate", expiry.ToUniversalTime().ToJToken());
        info.Add("startDate", expiry.AddMinutes(-15).ToUniversalTime().ToJToken());

        return Ok(info.ToJson());
    }

    //[HttpGet]
    //[Route("api/get-login")]
    //public async Task<IActionResult> GetLogin()
    //{
    //    JObject info = new();

    //    //validar cookie de sessao
    //    Request.Cookies.TryGetValue("Session-Id", out var sessionId);

    //    var sessionClaim = await _context.UserClaims
    //        .FirstOrDefaultAsync(u => u.ClaimValue == sessionId);

    //    var user = await _context.UtilizadorRegistado
    //        .FirstOrDefaultAsync(u => u.AuthenticationID == sessionClaim.UserId);

    //    if (sessionClaim != null)
    //    {
    //        //HttpContext.Session.TryGetValue("expiry", out byte[] expiryDateBytes);

    //        //var expiryDateObj = JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(expiryDateBytes)).ToString();
    //        //var expiryDate = new DateTimeOffset();
            
    //        info.Add("userId", user.IDUtilizador);
    //        //info.Add("expiryDate", expiryDate.ToUniversalTime().ToJToken());
    //        //info.Add("startDate", expiryDate.AddMinutes(-15).ToUniversalTime().ToJToken());

    //        return Ok(info.ToJson());
    //    }

    //    return NotFound();
    //}

    /// <summary>
    /// Metodo de login
    /// </summary>
    /// <param name="loginDataJson"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/login")]
    public async Task<IActionResult> UserValidation([FromBody] Login loginDataJson)
    {
        try
        {
            DateTimeOffset expiry = new DateTimeOffset();
            IActionResult resp;
            JObject info = new();
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados

            var username = loginDataJson.NomeUtilizador;
            var password = loginDataJson.Password;

            // Validacao de id
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

                    await _signInManager.SignInAsync(identityUser, false);

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

                    expiry = DateTimeOffset.Now.AddMinutes(15);
                    Response.Cookies.Append("Session-Id", claimValue, new CookieOptions { HttpOnly = true, Secure = true, Expires = expiry });

                    info.Add("userId", userId);
                    info.Add("expiryDate", expiry.ToUniversalTime().ToJToken());
                    info.Add("startDate", expiry.AddMinutes(-15).ToUniversalTime().ToJToken());
                }

            }

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

    /// <summary>
    /// Metodo de logout
    /// </summary>
    /// <param name="logoutDataJson"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("api/logout")]
    public async Task<IActionResult> Logout([FromBody] Logout logoutDataJson)
    {
        try
        {

            IActionResult resp;
            JObject info = new();
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados

            var id = logoutDataJson.Id;

            //user da BD
            var user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);

            // Validacao de id
            var identityUser = await _context.Users.FirstOrDefaultAsync(u => user.AuthenticationID == u.Id);

            if (identityUser == null || user == null)
            {
                return Unauthorized();
            }

            //validar cookie de sessao
            if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
            {
                return Unauthorized();
            }

            var sessionClaim = await _context.UserClaims
                .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && identityUser.Id == u.UserId);

            if (sessionClaim == null)
            {
                return Unauthorized();
            }

            await _signInManager.SignOutAsync();

            // apagar cookie de sessao e claim
            Response.Cookies.Delete("Session-Id");

            _context.Remove(sessionClaim);
            await _context.SaveChangesAsync();

            resp = Ok();

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
