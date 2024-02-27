using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ObjectiveC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    public MessagesApiController(
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
    [Route("api/messages")]
    public async Task<IActionResult> GetFriendsInformation(string email, int id){

        try{
            

            IActionResult resp;
            JObject info = new();
            var valid = false;

            _logger.LogWarning("Entrou no método Get");

            // user da BD
            UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.Email == email || u.IDUtilizador == id);

            // amigos do utilizador
            Amigo [] friends = _context.Amigo.Where(a => a.DonoListaFK == user.IDUtilizador).ToArray();

            object [] friendsArray = {};
            object [] groupsArray = {};

            if(friends.Length > 0){
                valid = true;
                for(int i=0; i < friends.Length; i++){

                    // amigo
                    var friend = await _context.UtilizadorRegistado
                    .FirstOrDefaultAsync(u => u.IDUtilizador == friends[i].IDAmigo);

                    int friendId = friend.IDUtilizador;
                    string friendName = friend.NomeUtilizador;
                    string friendEmail = friend.Email;

                    // avatar do amigo
                    string imageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + Path.Combine(_webHostEnvironment.WebRootPath, "images", friend.NomeFotografia);

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
                    .Select(m => new{
                        m.ConteudoMsg,
                        m.RemetenteFK,
                        m.IDMensagem
                        })
                    .ToArray();

                    object [] msgTrunk = new object[25];

                    if(msgs.Length >= 25){
                        int decrement = 1;
                        for(int j=msgTrunk.Length-1; j>=0; j--){
                            msgTrunk[j] = msgs[^decrement];
                            decrement--;
                        }
                    }else{
                        msgTrunk = msgs;
                    }

                    object [] friendArray = {friendId, friendName, friendEmail, commonRoomId, imageURL, msgTrunk};

                    friendsArray.Append(friendArray);
                
                }
            }

            // grupos
            SalasChat [] userGroups = _context.RegistadosSalasChat
            .Where(rs => rs.UtilizadorFK == user.IDUtilizador && rs.Sala.SeGrupo)
            .Select(rs => rs.Sala).ToArray();

            for(int i=0; i<userGroups.Length; i++){

                var msgs = _context.Mensagem.Where(m => m.SalaFK == userGroups[i].IDSala)
                .Select(m => new{
                    m.ConteudoMsg,
                    m.RemetenteFK,
                    m.IDMensagem}
                    )
                .ToArray();

                object [] msgTrunk = new object[25];

                    if(msgs.Length >= 25){
                        int decrement = 1;
                        for(int j=msgTrunk.Length-1; j>=0; j--){
                            msgTrunk[j] = msgs[^decrement];
                            decrement--;
                        }
                    }else{
                        msgTrunk = msgs;
                    }

                // avatar do grupo
                string imageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + Path.Combine(_webHostEnvironment.WebRootPath, "images", userGroups[i].NomeFotografia);

                object [] userGroup = {userGroups[i].IDSala, userGroups[i].NomeSala, imageURL, msgTrunk};

                groupsArray.Append(userGroup);
            }
            info.Add("friends", friendsArray.ToJToken());
            info.Add("groups", groupsArray.ToJToken());
            info.Add("lengthFriendsList", friends.Length);
            info.Add("lengthGroupsList", userGroups.Length);
            info.Add("ownerInfo", user.IDUtilizador);

            resp = valid ? Ok(info.ToJson()) : NotFound();

           return resp;

        }catch(Exception ex){
            _logger.LogError("Erro na aquisição de informação de um user", ex.Message);
            return StatusCode(500);
        }finally{
            _logger.LogWarning("Saiu do método Get");
        }
    }

    [HttpPost]
    [Route("api/block")]
    public async Task<IActionResult> Block ([FromBody] string friendId){

        JObject obj = JObject.Parse(friendId);

        int id = obj.Value<int>("id");
        int owner = obj.Value<int>("owner");

        if(id != 0 && owner != 0){

            int [] ow = _context.RegistadosSalasChat
            .Where(rs => rs.UtilizadorFK == owner && !rs.Sala.SeGrupo)
            .Select(rs => rs.SalaFK)
            .ToArray();

            int [] fr = _context.RegistadosSalasChat
            .Where(rs => rs.UtilizadorFK == id && !rs.Sala.SeGrupo)
            .Select(rs => rs.SalaFK)
            .ToArray();

            int commonRoomId = ow.Intersect(fr).ToArray()[0];

            SalasChat room = await _context.SalasChat
            .FirstOrDefaultAsync(s => s.IDSala == commonRoomId);

            _context.Remove(room);
            await _context.SaveChangesAsync();

        }

        return NoContent();

    }
}
