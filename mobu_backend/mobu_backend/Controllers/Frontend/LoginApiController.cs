using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using SendGrid.Helpers.Errors.Model;

namespace mobu.Controllers.Frontend;

[ApiController]
[Route("api/login")]
public class LoginApiController : ControllerBase
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

    public LoginApiController(
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


    [HttpPost]
    public async Task<IActionResult> UserValidation(Login loginDataJson)
    {
        try{
            IActionResult resp;
            var valid = false;
            _logger.LogWarning("Entrou no método Post");

            //organizar os dados

            var email = loginDataJson.Email;
            var password = loginDataJson.Password;
            
            // Validacao de email e password
            var identityUserList = await  _userManager.GetUsersInRoleAsync("Registered");
            var identityUser = identityUserList.FirstOrDefault(u => u.Email == email);

            //user da BD
            var user = await _context.Utilizador_Registado.FirstOrDefaultAsync(u => u.Email == email);

            

            if(identityUser != null){
                var confirmPassword = await _userManager.CheckPasswordAsync(identityUser, password);
                if(confirmPassword){
                    valid = true;
                    user.DataNasc = DateTime.Now;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
            }

            resp = valid ? Ok() : NotFound(); // Implementar rota pagina mensagens
            
            _logger.LogWarning("Saiu do método Post");

            return resp;
        }catch(Exception ex){
            _logger.LogError($"Error no login: {ex.Message}");
            return StatusCode(500); // 500 Internal Server Error
        }finally{
            _logger.LogWarning("Saiu do método Post");
        }
    }
}
