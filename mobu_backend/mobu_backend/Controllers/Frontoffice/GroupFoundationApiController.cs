using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Api_models;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace mobu.Controllers.Frontend;

/// <summary>
/// Controller da API para a fundação de grupos
/// </summary>
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

    /// <summary>
    /// Construtor do controller da API para a fundação de grupos
    /// </summary>
    /// <param name="context"></param>
    /// <param name="webHostEnvironment"></param>
    /// <param name="userManager"></param>
    /// <param name="loggerEmail"></param>
    /// <param name="http"></param>
    /// <param name="optionsAccessor"></param>
    /// <param name="logger"></param>
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

    /// <summary>
    /// Autorizacao para aceder ao formulario de fundacao de grupos
    /// </summary>
    /// <param name="id">ID do novo administrador de grupo</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("api/get-group-foundation")]
    public async Task<IActionResult> GetGroupFoundation([FromQuery(Name = "id")] int id)
    {
        try
        {
            _logger.LogWarning("Entrou no método Get");

            // encontrar utilizador com a claim de valor igual ao cookie
            // (se este existir)

            // user da BD
            UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
            var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.AuthenticationID);

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
            return Ok();

        }catch (Exception ex)
        {
            _logger.LogError($"Error na aqusição de um perfil: {ex.Message}");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Registo de grupos
    /// </summary>
    /// <param name="registerData">Dados de registo</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("api/group-foundation")]
    public async Task<IActionResult> RegisterGroup([FromForm] RegisterGroup registerData)
    {
        try
        {
            
            _logger.LogWarning("Entrou no método Post");

            var haFoto = false;
            var nomeFoto = "";
            var groupName = registerData.NomeSala;
            var avatar = registerData.Avatar;
            var adminId = registerData.AdminId;

            var adminProfile = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == adminId);

            if (groupName == null)
            {
                return BadRequest();
            }

            if (adminProfile == null)
            {
                return Unauthorized();
            }

            //validar cookie de sessao
            if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
            {
                return Unauthorized();
            }

            var sessionClaim = await _context.UserClaims
                .FirstOrDefaultAsync(u => u.ClaimValue == sessionId && adminProfile.AuthenticationID == u.UserId);

            if (sessionClaim == null)
            {
                return Unauthorized();
            }

            // sala de grupo
            var salaChat = new SalasChat()
            {
                NomeSala = groupName,
                SeGrupo = true,
            };

            // avatar

            if (avatar == null)
            {
                // sem foto
                // foto por predefenicao
                salaChat.DataFotografia = DateTime.Now;
                salaChat.NomeFotografia = "default_avatar.png";
            }
            else
            {
                if (avatar.ContentType == "image/jpeg" ||
                    avatar.ContentType == "image/png")
                {

                    // nome da imagem
                    Guid g = Guid.NewGuid();
                    nomeFoto = g.ToString();
                    string extensaoFoto =
                        Path.GetExtension(avatar.FileName).ToLower();
                    nomeFoto += extensaoFoto;

                    // tornar foto do modelo na foto processada acima
                    salaChat.DataFotografia = DateTime.Now;
                    salaChat.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;

                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    salaChat.DataFotografia = DateTime.Now;
                    salaChat.NomeFotografia = "default_avatar.png";
                }
            }

            try
            {
                // adicionar dados do utilizador registado
                // a BD
                _context.Add(salaChat);

                // realizar commit
                await _context.SaveChangesAsync();

                // adicionar administrador
                var admin = new RegistadosSalasChat()
                {
                    IsAdmin = true,
                    Sala = salaChat,
                    SalaFK = salaChat.IDSala,
                    Utilizador = adminProfile,
                    UtilizadorFK = adminProfile.IDUtilizador
                };

                // adicionar relacionamento sala <-> administrador
                // a BD
                _context.Add(admin);

                // realizar commit
                await _context.SaveChangesAsync();


                // guardar foto

                if (haFoto)
                {
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

                    // criar objeto para manipular imagem
                    using var stream = new FileStream(nomeFotoImagem, FileMode.Create);

                    // efetivamente guardar ficheiro no disco
                    await avatar.CopyToAsync(stream);
                }/*else
                {

                    // caminho completo da foto
                    nomeFoto = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", nomeFoto);

                    //fileInfo da foto
                    FileInfo fif = new(nomeFoto);

                    // garantir que foto existe
                    if (fif.Exists && fif.Name != "default_avatar.png")
                    {
                        //apagar foto
                        fif.Delete();
                    }
                }*/
                return Ok();

            }
            catch (Exception ex)
            {
                //informar de erro de adicao
                _logger.LogInformation("$Ocorreu um erro com a adição do utilizador" + salaChat.NomeSala + "\nA apagar utilizador...");

                if (SalaChatExists(salaChat.IDSala))
                {
                    _context.Remove(salaChat);

                    // realizar commit
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Utilizador apagado!");
                }
            }

            _logger.LogWarning("Saiu do método Post");
            return BadRequest();

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

    /// <summary>
    /// Verifica se existe uma sala de chat na base de dados
    /// </summary>
    /// <param name="id">ID da sala</param>
    /// <returns></returns>
    private bool SalaChatExists(int id)
    {
        return _context.UtilizadorRegistado.Any(e => e.IDUtilizador == id);
    }
}
