using System.Collections.Immutable;
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

    public SearchApiController(
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

    [HttpGet]
    public IActionResult GetUnknownPeopleAndGroups(int id, string name, string email)
    {
        try{
            IActionResult unknownPeopleAndGroupsResp;
            JObject unknownPeopleAndGroupsObj = JObject.FromObject(new object());
            object [] unknownPeople = {};
            object [] unknownGroups = {};
            var valid = false;

            _logger.LogWarning("Entrou no método Get");

            // fabricar lista de todos os desconhecidos do utilizador com ID=id

            // pesquisar por id
            if(id != 0){
                valid = true;
                unknownPeople = unknownPeople.Union(_context.Utilizador_Registado.Where(u => u.IDUtilizador == id).ToArray()).ToArray();
                unknownGroups = unknownGroups.Union(_context.Salas_Chat.Where(s => s.IDSala == id).ToArray()).ToArray();
            }

            // pesuqisar por nome
            if(name != ""){
                valid = true;
                unknownPeople = unknownPeople.Union(_context.Utilizador_Registado.Where(u => u.NomeUtilizador == name).ToArray()).ToArray();
                unknownGroups = unknownGroups.Union(_context.Salas_Chat.Where(u => u.NomeSala == name).ToArray()).ToArray();
            }

            // pesqusiar par email
            if(email != ""){
                valid = true;
                unknownPeople = unknownPeople.Union(_context.Utilizador_Registado.Where(u => u.Email == email).ToArray()).ToArray();
            }

            var unknownArray = unknownPeople.Union(unknownGroups).ToArray();

            // ordenar array
            Array.Sort(unknownArray);

            // adicionar array ao objeto JSON
            unknownPeopleAndGroupsObj.Add(unknownArray);

            unknownPeopleAndGroupsResp = valid ? Ok(unknownPeopleAndGroupsObj.ToJson()) : NotFound();

            return unknownPeopleAndGroupsResp;

        }catch(Exception ex){
            _logger.LogError($"Error na pesquisa de um perfil: {ex.Message}");
            return StatusCode(500);
        }finally{
            _logger.LogWarning("Saiu do método Get");
        }
        
    }
}
