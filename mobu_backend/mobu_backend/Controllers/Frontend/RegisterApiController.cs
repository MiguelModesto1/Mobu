using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using mobu_backend.Data;
using mobu_backend.Migrations;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

[ApiController]
public class RegisterApiController : ControllerBase
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
    /// Interface para aceder ao ambiente do host
    /// </summary>
    private readonly IHostEnvironment _hostEnvironment;

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

    public RegisterApiController(
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        IHostEnvironment hostEnvironment,
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
        _hostEnvironment = hostEnvironment;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _loggerEmail = loggerEmail;
        _http = http;
        _optionsAccessor = optionsAccessor;
    }

    [HttpPost]
    [Route("api/register")]
    public async Task<IActionResult> RegisterUser([FromBody] string registerDataJson)
    {
        try{
            StatusCodeResult status;
            var valid = false;
            var nomeFoto = "";
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            JObject registerData = JObject.Parse(registerDataJson);

            var avatar = registerData.Value<string>("avatar");
            var username = registerData.Value<string>("username");
            var email = registerData.Value<string>("email");
            var password = registerData.Value<string>("password");
            
            // Criacao de utilizador com username, email e password

            // avatar

            if(avatar != ""){
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

            }else{
                nomeFoto = "default_avatar.png";
            }

            UtilizadorRegistado user = new(){
                NomeUtilizador = username,
                Email = email,
                NomeFotografia = nomeFoto,
                DataFotografia = DateTime.Now
            };

            _context.Attach(user);
            await _context.SaveChangesAsync();

            // guardar dados no identitiy
            
            await _userManager.CreateAsync(new IdentityUser(){
                UserName = username,
                Email = email
            });

            var identityUser = await _userManager.FindByEmailAsync(email);

            await _userManager.AddPasswordAsync(identityUser, password);

            // adicionar a role se esta existir

            if(!await _roleManager.RoleExistsAsync("Registered")){
                await _roleManager.CreateAsync(new IdentityRole("Registered"));
            }

            await _userManager.AddToRoleAsync(identityUser,"Registered");

            // enviar email de confirmacao

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var request = _http.HttpContext.Request;

            var href = "https://" + request.Host.ToString() + "/api/confirm-new-account?email=" + identityUser.Email + "&code=" + code; // IMPLEMENTAR ROTA PARA A PAGINA DE AGRADECIMENTO

            var htmlElement = "Para confirmar o seu email <a href='" + href + "' target='_blank'>clique aqui</a>.";

            EmailSender emailSender = new(_optionsAccessor, _loggerEmail);

            await emailSender.SendEmailAsync(identityUser.Email, "Confirme o seu email", htmlElement);

            status = valid ? NoContent() : NotFound();
            
            return status;
        }catch(Exception ex){
            _logger.LogError($"Error na edição de um perfil: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
        
    }

    [HttpGet]
    [Route("api/confirm-new-account")]
    public async Task<IActionResult> ConfirmNewAccount (string code, string email){
        try{

            IActionResult confirmedResp;
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            // confirmar nova conta
            var identityProfileList = await _userManager.GetUsersInRoleAsync("Registered");
            var identityProfile = identityProfileList.FirstOrDefault(u => u.Email == email);

            if(identityProfile != null){
                await _userManager.ConfirmEmailAsync(identityProfile, code);

                valid = true;
            }

            confirmedResp = valid ? Redirect(Environment.GetEnvironmentVariable("FRONTEND_APP_URL") + "") : NotFound(); //IMPLEMENTAR ROTA PARA PAGINA DE LOGIN

            return confirmedResp;

        }catch(Exception ex){
            _logger.LogError($"Error na edição de um perfil: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
    }

    [HttpPost]
    [Route("api/guest")]
    public async Task<IActionResult> Guest([FromBody] string create){
        try{
            IActionResult status;
            var valid = false;
            UtilizadorAnonimo anon = null;
            JObject resp = new();
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados
            JObject registerData = JObject.Parse(create);
            
            // Criacao de anonimo novo

            if(registerData.Value<bool>("create")){
                valid = true;
                anon = new();

                _context.Attach(anon);
                await _context.SaveChangesAsync();
            }

            int  anonId = anon.IDUtilizador;

            resp.Add("anonymous", anonId.ToJToken());

            status = valid ? Ok(resp.ToJson()) : NotFound();

            return status;

        }catch(Exception ex){
            _logger.LogError($"Error na edição de um perfil: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
    }
}
