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
[Route("api/search")]
public class SearchApiController : ControllerBase
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

    public SearchApiController(
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

    [HttpGet]
    public IActionResult GetUnknownPeopleAndGroups(string query)
    {
        try
        {
            JObject unknownPeopleAndGroupsObj = new();
            object[] unknownPeople = Array.Empty<object>();
            object[] unknownGroups = Array.Empty<object>();

            _logger.LogWarning("Entrou no método Get");

            // fabricar lista de todos os desconhecidos do utilizador com ID=id

            // pesquisar por id
            if (query != null)
            {
                unknownPeople = unknownPeople
                .Union(_context.UtilizadorRegistado
                .Where(u => u.IDUtilizador.ToString() == query)
                .Select(u => new { u.IDUtilizador, u.NomeUtilizador })
                .ToArray())
                .ToArray();
                unknownGroups = unknownGroups
                .Union(_context.SalasChat
                .Where(s => s.IDSala.ToString() == query && s.SeGrupo)
                .Select(s => new { s.IDSala, s.NomeSala, s.SeGrupo })
                .ToArray())
                .ToArray();

                unknownPeople = unknownPeople
                .Union(_context.UtilizadorRegistado
                .Where(u => u.NomeUtilizador == query)
                .Select(u => new { u.IDUtilizador, u.NomeUtilizador })
                .ToArray())
                .ToArray();
                unknownGroups = unknownGroups
                .Union(_context.SalasChat
                .Where(s => s.NomeSala == query && s.SeGrupo)
                .Select(s => new { s.IDSala, s.NomeSala, s.SeGrupo })
                .ToArray())
                .ToArray();

                unknownPeople = unknownPeople
                .Union(_context.UtilizadorRegistado
                .Where(u => u.Email == query)
                .Select(u => new { u.IDUtilizador, u.NomeUtilizador })
                .ToArray())
                .ToArray();
            }

            var unknownArray = unknownPeople.Union(unknownGroups).ToArray();

            // ordenar array
            Array.Sort(unknownArray);

            // adicionar array ao objeto JSON
            unknownPeopleAndGroupsObj.Add("unknown", unknownArray.ToJToken());

            var u = unknownPeopleAndGroupsObj.ToJson();

            return Ok(unknownPeopleAndGroupsObj.ToJson());

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error na pesquisa de um perfil: {ex.Message}");
            return StatusCode(500);
        }
        finally
        {
            _logger.LogWarning("Saiu do método Get");
        }

    }
}
