using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Areas.Identity.Pages.Account;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
public class GameApiController : ControllerBase
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

    public GameApiController(
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
    [Route("api/get-random-user-id")]
    public IActionResult GetRandomUserId()
    {
        try
        {
            IActionResult resp;
            JObject respObj = new();
            var valid = false;
            Random random = new();
            int randomUser = 0;

            _logger.LogWarning("Entrou no método Get");

            UtilizadorRegistado[] users = _context.UtilizadorRegistado
            .ToArray();

            if (users.Length > 0)
            {
                valid = true;

                randomUser = random.Next(1, users.Length);
                respObj.Add("randomUser", users[randomUser].ToJToken());
            }

            resp = valid ? Ok(respObj.ToJson()) : NotFound();

            return resp;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error ao obter user aleatório: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }
        finally
        {
            _logger.LogWarning("Saiu do método Post");
        }
    }

    [HttpGet]
    [Route("api/get-friends")]
    public async Task<IActionResult> GetFriends(int id){
        try{
            IActionResult resp;
            JObject info = new();
            var valid = false;

            _logger.LogWarning("Entrou no método Get");
            
            // user da BD
            UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);

            // amigos do utilizador
            Amigo [] friends = _context.Amigo.Where(a => a.DonoListaFK == user.IDUtilizador).ToArray();

            object [] friendsArray = {};

            if(friends.Length > 0){
                valid = true;
                for(int i=0; i < friends.Length; i++){

                    // amigo
                    var friend = await _context.UtilizadorRegistado
                    .FirstOrDefaultAsync(u => u.IDUtilizador == friends[i].IDAmigo);

                    int friendId = friend.IDUtilizador;
                    string friendName = friend.NomeUtilizador;

                    // avatar do amigo
                    string imageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + Path.Combine(_webHostEnvironment.WebRootPath, "images", friend.NomeFotografia);

                    object [] friendArray = {friendId, friendName, imageURL};

                    friendsArray.Append(friendArray);
                
                }
            }

            info.Add("friends", friendsArray.ToJToken());
            info.Add("lengthFriendsList", friends.Length);
            info.Add("ownerInfo", user.IDUtilizador);

            resp = valid ? Ok(info.ToJson()) : NotFound();

           return resp;          
        }catch(Exception ex){
            _logger.LogError($"Error ao obter amigos: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
    }

    [HttpPost]
    [Route("api/store-game-results")]
    public async Task<IActionResult> StoreGameResults([FromBody] string gameResult){
        try{

            JObject result = JObject.Parse(gameResult);

            // organizar dados
            int challenger = result.Value<int>("challenger");
            int opponent = result.Value<int>("opponent");
            string winner = result.Value<string>("winner");

            // guardar resultados

            if(winner != "none"){
                int [] chal = _context.RegistadosSalasJogo
                .Where(rs => rs.UtilizadorFK == challenger)
                .Select(rs => rs.UtilizadorFK)
                .ToArray();

                int [] opp = _context.RegistadosSalasJogo
                .Where(rs => rs.UtilizadorFK == opponent)
                .Select(rs => rs.UtilizadorFK)
                .ToArray();

                int commonGameRoomId = chal.Intersect(opp).ToArray()[0];

                RegistadosSalasJogo storeChal = (RegistadosSalasJogo)_context.RegistadosSalasJogo
                .Where(rs => rs.SalaFK == commonGameRoomId && rs.UtilizadorFK == challenger);

                RegistadosSalasJogo storeOpp = (RegistadosSalasJogo)_context.RegistadosSalasJogo
                .Where(rs => rs.SalaFK == commonGameRoomId && rs.UtilizadorFK == opponent);

                storeChal.Pontos = int.Parse(winner) == challenger ? storeChal.Pontos + 1 : storeChal.Pontos;
                storeOpp.Pontos = int.Parse(winner) == opponent ? storeOpp.Pontos + 1 : storeOpp.Pontos;

                _context.Update(storeChal);
                _context.Update(storeOpp);
                await _context.SaveChangesAsync();
            }

            return NoContent();

        }catch(Exception ex){
            _logger.LogError($"Error ao guardar resultados de jogo: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
    }

}