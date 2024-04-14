using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Data;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
[Route("api/requests")]
public class RequestsApiController : ControllerBase
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

    public RequestsApiController(
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

   /* [HttpGet]
    public IActionResult GetRequests(int id)
    {

        try
        {
            JObject json = new();

            var requests =
           _context.DestinatarioPedidosAmizade
           .Where(dp => dp.IDDestinatarioPedido == id)
           .Select(dp => new
           {
               dp.RemetenteFK,
               dp.RemetentePedido.NomeUtilizador
           })
           .ToArray();

            json.Add("requests", requests.ToJToken());

            return Ok(json.ToJson());

        }
        catch (Exception ex)
        {
            _logger.LogError("Error: ", ex.Message);
            return StatusCode(500);
        }
        finally
        {

        }
    }*/
}