using EllipticCurve.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
public class ProfileApiController : ControllerBase
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

    public ProfileApiController(
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
    [Route("api/profile/get-profile")]
    public async Task<IActionResult> GetProfile(int id, bool isGroup)
    {
        try{
            IActionResult profileResp;
            JObject profileObj = JObject.FromObject(new object());
            var valid = false;

            _logger.LogWarning("Entrou no método Get");

            // aquisição de perfil
            if(isGroup){
            Salas_Chat profile = await _context.Salas_Chat.FirstOrDefaultAsync(s => s.IDSala == id);
                if(profile != null){
                    valid = true;
                    profileObj.Add("avatar", profile.NomeFotografia);
                    profileObj.Add("groupName", profile.NomeSala);
                }
            }else{
                Utilizador_Registado profile = await _context.Utilizador_Registado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
                if(profile != null){
                    valid = true;
                    profileObj.Add("avatar", profile.NomeFotografia);
                    profileObj.Add("username", profile.NomeUtilizador);
                    profileObj.Add("email", profile.Email);
                    profileObj.Add("birthDate", profile.DataNasc.Date);
                }
            }

            profileResp = valid ? Ok(profileObj.ToJson()) : NotFound();
            
            return profileResp;

        }catch(Exception ex){
             _logger.LogError($"Error na aqusição de um perfil: {ex.Message}");
            return StatusCode(500);
        }finally{
            _logger.LogWarning("Saiu do método Get");
        }
    }

    [HttpPost]
    [Route("api/profile/edit-person-profile")]
    public async Task<IActionResult> EditPersonProfile([FromBody] string profileDataJson)
    {
        try{
            StatusCodeResult status;
            var valid = false;

            _logger.LogWarning("Entrou no método post");

            // organizar os dados
            JObject profileData = JObject.Parse(profileDataJson);

            var id = profileData.Value<int>("id");
            var avatar = profileData.Value<string>("avatar");
            var username = profileData.Value<string>("username");
            var email = profileData.Value<string>("email");
            var newPassword = profileData.Value<string>("newPassword");
            var currPassword = profileData.Value<string>("currPassword");

            // editar perfil
            Utilizador_Registado profile = await _context.Utilizador_Registado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
            var identityProfileList = await _userManager.GetUsersInRoleAsync("Registered");
            var identityProfile = identityProfileList.FirstOrDefault(u => u.Email == email);

            if(identityProfile != null && profile != null){
                // avatar
                if(avatar == ""){
                    
                    // caminho completo da foto
                    var nomeFoto = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", profile.NomeFotografia);

                    //fileInfo da foto
                    FileInfo fif = new(nomeFoto);

                    // garantir que foto existe
                    if (fif.Exists && fif.Name != "default_avatar.png")
                    {
                        //apagar foto
                        fif.Delete();
                    }

                    profile.NomeFotografia = "default_avatar.png";
                    profile.DataFotografia = DateTime.Now;
                }else{

                    // descodificar foto
                    byte[] imageBytes = Convert.FromBase64String(avatar[(avatar.IndexOf(",") + 1)..]);

                    //extenstao da foto
                    var extensaoFoto = "." + avatar.Split(",").GetValue(0).ToString().Split(";").GetValue(0).ToString().Split("/").GetValue(1).ToString();

                    // caminho completo da foto
                    var nomeFoto = profile.NomeFotografia.Split(".")[0] + extensaoFoto;

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

                    // definir nome da imagem
                    string nomeFotoImagem = Path.Combine(nomeLocalImagem, nomeFoto);

                    System.IO.File.WriteAllBytes(nomeFotoImagem, imageBytes);

                    profile.NomeFotografia = nomeFoto;
                    profile.DataFotografia = DateTime.Now;
                }

                // username
                if(username != ""){
                    profile.NomeUtilizador = username;
                    await _userManager.SetUserNameAsync(identityProfile, username);
                }

                // email
                if(email != ""){
                    profile.Email = email;
                    await _userManager.SetEmailAsync(identityProfile, email);
                }

                // password
                if(newPassword != ""){
                    await _userManager.ChangePasswordAsync(identityProfile, currPassword, newPassword);
                }

                _context.Update(profile);
                await _context.SaveChangesAsync();
                valid = true;
            }

            status = valid ? NoContent() : NotFound();

            return status;

        }catch(Exception ex){
             _logger.LogError($"Error na edição de um perfil: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método post");
        }
        
    }

    [HttpPost]
    [Route("api/profile/edit-group-profile")]
    public async Task<IActionResult> EditGroupProfile([FromBody] string profileDataJson)
    {
        try{
            StatusCodeResult status;
            var valid = false;

            _logger.LogWarning("Entrou no método post");

            // organizar os dados
            JObject profileData = JObject.Parse(profileDataJson);

            var id = profileData.Value<int>("id");
            var avatar = profileData.Value<string>("avatar");
            var groupName = profileData.Value<string>("groupName");

            Salas_Chat profile = await _context.Salas_Chat.FirstOrDefaultAsync(s => s.IDSala == id);

            // editar perfil
            if(profile != null){
                // avatar
                if(avatar == ""){
                    
                    // caminho completo da foto
                    var nomeFoto = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", profile.NomeFotografia);

                    //fileInfo da foto
                    FileInfo fif = new(nomeFoto);

                    // garantir que foto existe
                    if (fif.Exists && fif.Name != "default_avatar.png")
                    {
                        //apagar foto
                        fif.Delete();
                    }

                    profile.NomeFotografia = "default_avatar.png";
                    profile.DataFotografia = DateTime.Now;
                }else{

                    // descodificar foto
                    byte[] imageBytes = Convert.FromBase64String(avatar[(avatar.IndexOf(",") + 1)..]);

                    //extenstao da foto
                    var extensaoFoto = "." + avatar.Split(",").GetValue(0).ToString().Split(";").GetValue(0).ToString().Split("/").GetValue(1).ToString();

                    // caminho completo da foto
                    var nomeFoto = profile.NomeFotografia.Split(".")[0] + extensaoFoto;

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

                    // definir nome da imagem
                    string nomeFotoImagem = Path.Combine(nomeLocalImagem, nomeFoto);

                    System.IO.File.WriteAllBytes(nomeFotoImagem, imageBytes);

                    profile.NomeFotografia = nomeFoto;
                    profile.DataFotografia = DateTime.Now;
                }

                // username
                if(groupName != ""){
                    profile.NomeSala = groupName;
                }

                _context.Update(profile);
                await _context.SaveChangesAsync();
                valid = true;
            }
            

            status = valid ? NoContent() : NotFound();

            return status;
        }catch(Exception ex){
            _logger.LogError($"Error na edição de um perfil: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método post");
        }
        
    }
}
