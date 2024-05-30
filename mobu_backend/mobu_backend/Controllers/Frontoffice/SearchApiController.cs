using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using mobu_backend.ApiModels;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
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
    [Authorize]
    [Route("api/get-search-page")]
    public async Task<IActionResult> GetSearchPage([FromQuery(Name = "id")] int id)
    {
        try
        {
            var profile = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
            var identityProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == profile.AuthenticationID);

            if (profile == null || identityProfile == null)
            {
                return Unauthorized();
            }

            //validar cookie de sessao
            if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
            {
                return Unauthorized();
            }

            var sessionClaim = await _context.UserClaims
                .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && profile.AuthenticationID == u.UserId);

            if (sessionClaim == null)
            {
                return Unauthorized();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error na edição de um perfil: {ex.Message}");
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("api/search")]
    public async Task<IActionResult> GetUnknownPeopleAndGroups([FromQuery(Name = "id")] int id, [FromQuery(Name = "searchString")] string searchString)
    {
        try
        {
            JObject unknownPeopleAndGroupsObj = [];
            List<object> unknownPeople = [];
            List<object> unknownGroups = [];

            _logger.LogWarning("Entrou no método Get");

            // user da BD
            UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
            var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.AuthenticationID);

            if (searchString == null || searchString == "")
            {
                return BadRequest();
            }

            if (user == null || identityUser == null)
            {
                return Unauthorized();
            }

            //validar cookie de sessao
            if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
            {
                return Unauthorized();
            }

            var sessionClaim = await _context.UserClaims
                .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && user.AuthenticationID == u.UserId);

            if (sessionClaim == null)
            {
                return Unauthorized();
            }

            // fabricar lista de todos os desconhecidos do utilizador com ID=id

            // listas de amigos e grupos para exclusao no 
            // conjunto de pessoas e grupos descnhecidos

            var friendsList = _context.Amizade
                .Where(a => a.RemetenteFK == id)
                .Select(a => a.Destinatario);

            var groupsList = _context.RegistadosSalasChat
                .Where(a => a.UtilizadorFK == id && a.Sala.SeGrupo)
                .Select(a => a.Sala);

            // variaveis para guardar consultas
            IQueryable<UtilizadorRegistado> peopleQuery;
            IQueryable<SalasChat> groupsQuery;

            // verificar se parametro de pesquisa e numero ou string
            var isNumber = int.TryParse(searchString, out int searchId);

            if (isNumber)
            {

                peopleQuery = _context.UtilizadorRegistado
                .Where(u => u.IDUtilizador == searchId && u.IDUtilizador != id);

                unknownPeople = peopleQuery.Except(friendsList)
                    .Select(u => new UnkownPerson()
                    {
                        Id = u.IDUtilizador,
                        Nome = u.NomeUtilizador,
                        Email = u.Email
                    })
                    .ToList<object>();

                groupsQuery = _context.RegistadosSalasChat
                .Where(rs =>
                rs.Sala.IDSala == searchId &&
                rs.UtilizadorFK != id &&
                rs.Sala.SeGrupo)
                .Select(rs => rs.Sala)
                .Distinct();

                unknownGroups = groupsQuery.Except(groupsList)
                   .Select(u => new UnkownGroup()
                   {
                       Id = u.IDSala,
                       Nome = u.NomeSala
                   })
                    .ToList<object>();
            }
            else
            {
                peopleQuery = _context.UtilizadorRegistado
                .Where(u => 
                (u.NomeUtilizador.ToString().Contains(searchString) ||
                u.Email.Contains(searchString)) &&
                u.IDUtilizador != id);

                unknownPeople = peopleQuery.Except(friendsList)
                .Select(u => new UnkownPerson()
                {
                    Id = u.IDUtilizador,
                    Nome = u.NomeUtilizador,
                    Email = u.Email
                })
                .ToList<object>();

                groupsQuery = _context.RegistadosSalasChat
                .Where(rs =>
                rs.Sala.NomeSala.ToString().Contains(searchString) &&
                rs.UtilizadorFK != id &&
                rs.Sala.SeGrupo)
                .Select(rs => rs.Sala)
                .Distinct();

                unknownGroups = groupsQuery.Except(groupsList)
                .Select(u => new UnkownGroup()
                {
                    Id = u.IDSala,
                    Nome = u.NomeSala
                })
                .ToList<object>();
            }

            // adicionar array ao objeto JSON
            unknownPeopleAndGroupsObj.Add("unknownPeople", unknownPeople.ToJToken());
            unknownPeopleAndGroupsObj.Add("unknownGroups", unknownGroups.ToJToken());

            var notFound = unknownPeople.IsNullOrEmpty() && unknownGroups.IsNullOrEmpty();

            return notFound ? NotFound() : Ok(unknownPeopleAndGroupsObj.ToJson());

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
