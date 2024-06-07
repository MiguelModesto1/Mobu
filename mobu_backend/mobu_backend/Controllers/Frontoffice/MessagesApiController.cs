using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
    /// ferramenta com acesso a gestao de login
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

    /// <summary>
    /// Interface para a funcao de logging no controller
    /// </summary>
    private readonly ILogger<LoginApiController> _logger;

    public MessagesApiController(
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<EmailSender> loggerEmail,
        IHttpContextAccessor http,
        IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<LoginApiController> logger
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
    }

    [HttpGet]
    [Authorize]
    [Route("api/messages")]
    public async Task<IActionResult> GetFriendsInformation([FromQuery(Name = "id")] int id)
    {
        try
        {

            IActionResult resp;
            JObject info = [];
            var valid = false;

            _logger.LogWarning("Entrou no método Get");

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

            if(sessionClaim == null)
            {
                return Unauthorized();
            }

            // amigos do utilizador
            UtilizadorRegistado[] friends = _context.Amizade
                .Where(f => f.RemetenteFK == id)
                .Select(f => f.Destinatario)
                .ToArray();

            List<object> friendsList = [];
            List<object> groupsList = [];
            var arrayIncrement = 0;

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
                    bool? blockedThem = _context.Amizade
                        .Where(f => f.DestinatarioFK == friendId && f.RemetenteFK == id)
                        .Select(f => !f.Desbloqueado).ToArray()[0];

                    try
                    {
                        // Tentar aceder ao primeiro elemento do array
                        // para o guardar em 'blockedYou'
                        // Se nao conseguir, lanca excecao
                        // ArrayOutOfBoundsException, pois o array estara vazio.
                        bool? blockedYou = _context.Amizade
                            .Where(f => f.RemetenteFK == friendId && f.DestinatarioFK == id)
                            .Select(f => !f.Desbloqueado).ToArray()[0];

                        // avatar do amigo
                        string imageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + friend.NomeFotografia;

                        // salas
                        var userRoomsId = _context.RegistadosSalasChat
                        .Where(rs => rs.UtilizadorFK == user.IDUtilizador && !rs.Sala.SeGrupo)
                        .Select(rs => rs.SalaFK);

                        var friendRoomsId = _context.RegistadosSalasChat
                        .Where(rs => rs.UtilizadorFK == friend.IDUtilizador && !rs.Sala.SeGrupo)
                        .Select(rs => rs.SalaFK);

                        // tentar encotrar ID da sala em comum. Se nao conseguir, lanca excecao
                        // ArrayOutOfBoundsException, pois o array estara vazio.
                    
                        int commonRoomId = userRoomsId.Intersect(friendRoomsId).ToArray()[0];
                    

                        // mensagens

                        var msgs = _context.Mensagem
                            .Where(m => m.SalaFK == commonRoomId)
                            .Select(m => new Messages()
                            {
                                IDMensagem = m.IDMensagem,
                                IDRemetente = m.RemetenteFK,
                                URLImagemRemetente = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + m.Remetente.NomeFotografia,
                                NomeRemetente = m.Remetente.NomeUtilizador,
                                ConteudoMsg = m.ConteudoMsg
                            })
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

                        FriendObject friendObject = new()
                        { 
                            ItemId = arrayIncrement,
                            FriendId = friendId, 
                            FriendName = friendName, 
                            FriendEmail = friendEmail, 
                            CommonRoomId = commonRoomId, 
                            ImageURL = imageURL, 
                            Messages = msgs,
                            BlockedThem = blockedThem,
                            BlockedYou = blockedYou
                        };

                        friendsList.Add(friendObject);

                        arrayIncrement++;
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }

                }
            }

            // grupos
            SalasChat[] userGroups = _context.RegistadosSalasChat
            .Where(rs => rs.UtilizadorFK == user.IDUtilizador && rs.Sala.SeGrupo)
            .Select(rs => rs.Sala).ToArray();

            // administradores
            bool[] isOwnerAdmin = _context.RegistadosSalasChat
            .Where(rs => rs.UtilizadorFK == user.IDUtilizador && rs.Sala.SeGrupo)
            .Select (rs => rs.IsAdmin).ToArray();

            arrayIncrement = 0;

            for (int i = 0; i < userGroups.Length; i++)
            {

                valid = true;

                var msgs = _context.Mensagem.Where(m => m.SalaFK == userGroups[i].IDSala)
                .Select(m => new Messages()
                {
                    IDMensagem = m.IDMensagem,
                    IDRemetente = m.RemetenteFK,
                    URLImagemRemetente = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + m.Remetente.NomeFotografia,
                    NomeRemetente = m.Remetente.NomeUtilizador,
                    ConteudoMsg = m.ConteudoMsg
                })
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

                GroupObject groupObject = new()
                { 
                    ItemId = arrayIncrement,
                    IDSala = userGroups[i].IDSala,
                    NomeSala = userGroups[i].NomeSala,
                    ImageURL = imageURL,
                    Mensagens = msgs,
                    IsOwnerAdmin = isOwnerAdmin[i],
                    HasLeft = false,
                    WasExpelled = false
                };

                groupsList.Add(groupObject);

                arrayIncrement++;
            }
            info.Add("friends", friendsList.ToJToken());
            info.Add("groups", groupsList.ToJToken());
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
