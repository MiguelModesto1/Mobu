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

/// <summary>
/// Controller API para os pedidos de amizade
/// </summary>
[ApiController]
public class TestRequestsApiController : ControllerBase
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

    /// <summary>
    /// Construtor do controller API para os pedidos de amizade
    /// </summary>
    /// <param name="context"></param>
    /// <param name="webHostEnvironment"></param>
    /// <param name="userManager"></param>
    /// <param name="loggerEmail"></param>
    /// <param name="http"></param>
    /// <param name="optionsAccessor"></param>
    /// <param name="logger"></param>
    public TestRequestsApiController(
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

    /// <summary>
    /// Método para obter pedidos de amizade pendentes
    /// </summary>
    /// <param name="id">ID do utilizador</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("api/test-pending-requests")]
    public async Task<IActionResult> GetPendingRequests([FromQuery(Name = "id")] int id)
    {
        JObject resp = [];

        // user da BD
        UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
        var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.AuthenticationID);

        // verificar se o utilizador associado ao ID existe
        if (user == null || identityUser == null)
        {
            return Unauthorized();
        }

        //validar cookie de sessao
        if (!Request.Cookies.TryGetValue("Session-Id-" + id, out var sessionId))
        {
            return Unauthorized();
        }

        var sessionClaim = await _context.UserClaims
            .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && user.AuthenticationID == u.UserId);

        if (sessionClaim == null)
        {
            return Unauthorized();
        }

        // variavel com o array dos pedidos pendentes
        var pendingRequests = _context.Amizade
            .Where(a => a.DestinatarioFK == id && a.DataResposta == null)
            .Select(a => new PendingRequests()
            {
                DestID = a.DestinatarioFK,
                RemID = a.RemetenteFK,
                ImageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + a.Remetente.NomeFotografia,
                RemName = a.Remetente.NomeUtilizador
            })
            .ToArray();

        resp.Add("pendingRequests", pendingRequests.ToJToken());

        var notFound = pendingRequests.Length == 0;

        return !notFound ? Ok(resp.ToJson()) : NotFound();

    }
}