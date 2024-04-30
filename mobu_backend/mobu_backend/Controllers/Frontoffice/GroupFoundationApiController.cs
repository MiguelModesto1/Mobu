using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;

namespace mobu.Controllers.Frontend;

[ApiController]
public class GroupFoundationApiController : ControllerBase
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

    public GroupFoundationApiController(
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
    [Route("api/group-foundation")]
    public async Task<IActionResult> RegisterUser([FromBody] string registerDataJson)
    {
        try
        {
            IActionResult status;
            var valid = false;
            var nomeFoto = "";
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            JObject registerData = JObject.Parse(registerDataJson);

            var groupName = registerData.Value<string>("groupName");
            var avatar = registerData.Value<string>("avatar");
            var adminId = registerData.Value<int>("adminId");

            // Criacao de grupo com nome e avatar

            if (avatar != "")
            {
                // Conversao de imagem
                byte[] imageBytes = Convert.FromBase64String(avatar[(avatar.IndexOf(",") + 1)..]);

                //extensao da foto
                var extensaoFoto = "." + avatar.Split(",").GetValue(0).ToString().Split(";").GetValue(0).ToString().Split("/").GetValue(1).ToString();

                // nome da imagem
                Guid g = Guid.NewGuid();
                nomeFoto = g.ToString();
                nomeFoto += extensaoFoto;

                // guardar foto

                // local p/guardar foto
                // perguntar ao servidor pela pasta
                // wwwroot/imagens
                string nomeLocalImagem = _webHostEnvironment.WebRootPath;

                // nome ficheiro no disco
                nomeLocalImagem = Path.Combine(nomeLocalImagem, "imagens");

                // garantir existencia da pasta
                if (!Directory.Exists(nomeLocalImagem))
                {
                    Directory.CreateDirectory(nomeLocalImagem);
                }

                // e possivel efetivamente guardar imagem

                // definir nome da imagem
                string nomeFotoImagem = Path.Combine(nomeLocalImagem, nomeFoto);

                System.IO.File.WriteAllBytes(nomeFotoImagem, imageBytes);

            }
            else
            {
                nomeFoto = "default_avatar.png";
            }


            SalasChat salas_Chat = new()
            {
                NomeFotografia = nomeFoto,
                DataFotografia = DateTime.Now,
                NomeSala = groupName,
                SeGrupo = true
            };

            _context.Attach(salas_Chat);
            await _context.SaveChangesAsync();

            // associacao com o fundador do grupo
            RegistadosSalasChat registados_Salas_Chat = new()
            {
                IsAdmin = true,
                UtilizadorFK = adminId,
                SalaFK = salas_Chat.IDSala
            };


            _context.Attach(registados_Salas_Chat);
            await _context.SaveChangesAsync();

            status = valid ? CreatedAtAction(nameof(groupName), new { id = salas_Chat.IDSala }, salas_Chat) : NotFound();

            return status;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error no registode grupo: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }
        finally
        {
            _logger.LogWarning("Saiu do método Post");
        }

    }
}
