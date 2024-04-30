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
public class MessagesApiController : ControllerBase
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

    public MessagesApiController(
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
    [Route("api/messages")]
    public async Task<IActionResult> GetFriendsInformation([FromQuery(Name = "id")]int id)
    {

        try
        {


            IActionResult resp;
            JObject info = new();
            var valid = false;

            _logger.LogWarning("Entrou no método Get");

            // user da BD
            UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);

            // amigos do utilizador
            UtilizadorRegistado[] friends = _context.Amizade
                .Where(f => f.RemetenteFK == id)
                .Select(f => f.Destinatario)
                .ToArray();

            object[] friendsArray = { };
            object[] groupsArray = { };

            if (friends.Length > 0)
            {
                valid = true;
                for (int i = 0; i < friends.Length; i++)
                {

                    // amigo
                    var friend = friends[i];

                    int friendId = friend.IDUtilizador;
                    string friendName = friend.NomeUtilizador;
                    string friendEmail = friend.Email;

                    // avatar do amigo
                    string imageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + friend.NomeFotografia;

                    // salas
                    var userRoomsId = _context.RegistadosSalasChat
                    .Where(rs => rs.UtilizadorFK == user.IDUtilizador && !rs.Sala.SeGrupo)
                    .Select(rs => rs.SalaFK);

                    var friendRoomsId = _context.RegistadosSalasChat
                    .Where(rs => rs.UtilizadorFK == friend.IDUtilizador && !rs.Sala.SeGrupo)
                    .Select(rs => rs.SalaFK);

                    int commonRoomId = userRoomsId.Intersect(friendRoomsId).ToArray()[0];

                    // mensagens

                    var msgs = _context.Mensagem
                    .Where(m => m.SalaFK == commonRoomId)
                    .Select(m => m.ConteudoMsg)
                    .ToArray();

                    // truncar mensagens
                    /*object[] msgTrunk = new object[25];

                    if (msgs.Length >= 25)
                    {
                        int decrement = 1;
                        for (int j = msgTrunk.Length - 1; j >= 0; j--)
                        {
                            msgTrunk[j] = msgs[^decrement];
                            decrement--;
                        }
                    }
                    else
                    {
                        msgTrunk = msgs;
                    }*/

                    object[] friendArray = { friendId, friendName, friendEmail, commonRoomId, imageURL, msgs };

                    friendsArray.Append(friendArray);

                }
            }

            // grupos
            SalasChat[] userGroups = _context.RegistadosSalasChat
            .Where(rs => rs.UtilizadorFK == user.IDUtilizador && rs.Sala.SeGrupo)
            .Select(rs => rs.Sala).ToArray();

            for (int i = 0; i < userGroups.Length; i++)
            {

                var msgs = _context.Mensagem.Where(m => m.SalaFK == userGroups[i].IDSala)
                .Select(m => m.ConteudoMsg)
                .ToArray();

                // truncar mensagens
                /*object[] msgTrunk = new object[25];

                if (msgs.Length >= 25)
                {
                    int decrement = 1;
                    for (int j = msgTrunk.Length - 1; j >= 0; j--)
                    {
                        msgTrunk[j] = msgs[^decrement];
                        decrement--;
                    }
                }
                else
                {
                    msgTrunk = msgs;
                }
                */
                // avatar do grupo
                string imageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + userGroups[i].NomeFotografia;

                object[] userGroup = { userGroups[i].IDSala, userGroups[i].NomeSala, imageURL, msgs };

                groupsArray.Append(userGroup);
            }
            info.Add("friends", friendsArray.ToJToken());
            info.Add("groups", groupsArray.ToJToken());
            info.Add("lengthFriendsList", friends.Length);
            info.Add("lengthGroupsList", userGroups.Length);
            info.Add("ownerInfo", user.IDUtilizador);

            resp = valid ? Ok(info.ToJson()) : NotFound();

            return resp;

        }
        catch (Exception ex)
        {
            _logger.LogError("Erro na aquisição de informação de um user", ex.Message);
            return StatusCode(500);
        }
        finally
        {
            _logger.LogWarning("Saiu do método Get");
        }
    }
}
