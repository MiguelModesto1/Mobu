using System.Collections.Immutable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Areas.Identity.Pages.Account;
using mobu_backend.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace mobu.Controllers.Backend
{

    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
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
        /// Interface para a funcao de logging no modelo de registo
        /// </summary>
        private readonly ILogger<RegisterModel> _logger;

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

        public AdminController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            ILogger<EmailSender> loggerEmail,
            IHttpContextAccessor http,
            IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _logger = logger;
            _loggerEmail = loggerEmail;
            _http = http;
            _optionsAccessor = optionsAccessor;
            _roleManager = roleManager;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            // Consulta que inclui dados do adiminstrador
            var utilizadores = _context.Admin;
            // voltar a lista
            return View(await utilizadores.ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Retorna o codigo de erro 404 se o id ou o admin
            // nao existir ou for nulo
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            // Consulta que retorna todos os detalhes do
            // administrador com IDAdmin = id
            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.IDAdmin == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Boss")]
        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("IDAdmin,NomeAdmin,Password,DataJuncao,DataNasc,Email,NomeFotografia,DataFotografia,AuthenticationID")] Admin admin, IFormFile fotografia)
        {
            // data de juncao
            admin.DataJuncao = DateTime.Now;

            // variaveis auxiliares
            var request = _http.HttpContext.Request;
            string code = "";
            string nomeFoto = "";
            bool haFoto = false;
            var user = new IdentityUser();

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                admin.DataFotografia = DateTime.Now;
                admin.NomeFotografia = "default_avatar.png";
            }
            else
            {
                // ficheiro existe
                // sera valido?
                if (fotografia.ContentType == "image/jpeg" ||
                    fotografia.ContentType == "image/png")
                {
                    // imagem valida

                    // nome da imagem
                    Guid g = Guid.NewGuid();
                    nomeFoto = g.ToString();
                    string extensaoFoto =
                        Path.GetExtension(fotografia.FileName).ToLower();
                    nomeFoto += extensaoFoto;

                    // tornar foto do modelo na foto processada acima
                    admin.DataFotografia = DateTime.Now;
                    admin.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    admin.DataFotografia = DateTime.Now;
                    admin.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // colocar conteudos nas tabelas
                    // do identity

                    await _userManager.SetUserNameAsync(user, admin.NomeAdmin);
                    await _userManager.SetEmailAsync(user, admin.Email);

                    var result = await _userManager.CreateAsync(user, admin.Password);

                    if (result.Succeeded)
                    {

                        // criar role de administrador se esta nao existir
                        if (!await _roleManager.RoleExistsAsync("Administrator"))
                        {
                            var role = new IdentityRole
                            {
                                Name = "Administrator",
                                ConcurrencyStamp = Guid.NewGuid().ToString()
                            };
                            await _roleManager.CreateAsync(role);

                            role = new IdentityRole
                            {
                                Name = "Boss",
                                ConcurrencyStamp = Guid.NewGuid().ToString()
                            };
                            await _roleManager.CreateAsync(role);

                            // atribuir ao primeiro admin a role de chefe dos admins
                            await _userManager.AddToRoleAsync(user, _roleManager.Roles
                            .Where(r => r.Name == "Boss")
                            .Select(r => r.Name)
                            .ToImmutableArray()[0]);
                        }

                        // atribuir ao administrador em questao a role
                        await _userManager.AddToRoleAsync(user, _roleManager.Roles
                            .Where(r => r.Name == "Administrator")
                            .Select(r => r.Name)
                            .ToImmutableArray()[0]);



                        _logger.LogInformation("Administrador criou uma nova conta com palavra-passe.");

                        var userId = await _userManager.GetUserIdAsync(user);

                        // enviar email de confirmacao de email

                        code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        var href = "https://" + request.Host.ToString() + "/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code + "&returnUrl=%2F";

                        var htmlElement = "Para confirmar o seu email <a href='" + href + "' target='_blank'>clique aqui</a>.";

                        EmailSender emailSender = new(_optionsAccessor, _loggerEmail);

                        await emailSender.SendEmailAsync(admin.Email, "Confirme o seu email", htmlElement);
                    }
                    else
                    {
                        // se o resultado da adicao nao tiver exito
                        // lanca excecao para a execucao saltar para
                        // o bloco 'catch'
                        throw new Exception();
                    }

                    admin.AuthenticationID = user.Id;

                    // adicionar dados do admin
                    // a BD
                    _context.Attach(admin);

                    // realizar commit
                    await _context.SaveChangesAsync();

                    // e possivel guardar imagem em disco
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
                        await fotografia.CopyToAsync(stream);
                    }

                    // voltar a lista
                    return RedirectToAction(nameof(Index));

                }
                // apanhar excecao para escrever um erro de modelo personalizado
                catch (Exception)
                {
                    //informar de erro de adicao
                    _logger.LogInformation($"Ocorreu um erro com a adição do admin {admin.NomeAdmin}.\nA apagar administrador...");

                    if (AdminExists(admin.IDAdmin))
                    {
                        _context.Remove(admin);

                        // realizar commit
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Admin apagado!");
                    }

                    // se existir admin na base de dados do negocio
                    if (await _context.Admin.FirstOrDefaultAsync(a => a.IDAdmin == admin.IDAdmin) != null)
                    {
                        await _userManager.DeleteAsync(user);

                        _logger.LogInformation("Admin apagado do Identity!");

                        // se nao exisitrem administradores
                        if ((await _userManager.GetUsersInRoleAsync("Administrator")).ToImmutableArray().Length == 0)
                        {
                            // apagar role 'Administrator'
                            await _roleManager.DeleteAsync(_roleManager.FindByNameAsync("Administrator").Result);
                        }
                    }

                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do admin " + admin.NomeAdmin);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(admin);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Retorna o codigo de erro 404 se o id ou o admin
            // nao existir ou for nulo
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            // Retorna a entidade encontrada de forma assincrona
            var admin = await _context.Admin
                .FirstOrDefaultAsync(a => a.IDAdmin == id);

            // Retorna o codigo de erro 404 se  o admin
            // nao existir ou for nulo
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDAdmin,NomeAdmin,Password,DataJuncao,DataNasc,Email,NomeFotografia,DataFotografia,AuthenticationID")] Admin admin, IFormFile fotografia)
        {

            //variaveis auxiliares
            string nomeFoto = _context.Admin
                        .Where(ur => ur.IDAdmin == id)
                        .Select(ur => ur.NomeFotografia)
                        .ToImmutableArray()[0];
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                admin.DataFotografia = DateTime.Now;
                admin.NomeFotografia = "default_avatar.png";
            }
            else
            {
                // ficheiro existe
                // sera valido?
                if (fotografia.ContentType == "image/jpeg" ||
                    fotografia.ContentType == "image/png")
                {
                    // imagem valida

                    if (nomeFoto == "default_avatar.png")
                    {
                        Guid g = Guid.NewGuid();
                        nomeFoto = g.ToString();
                        string extensaoFoto =
                            Path.GetExtension(fotografia.FileName).ToLower();
                        nomeFoto += extensaoFoto;
                    }

                    // tornar foto do modelo na foto processada acima
                    admin.DataFotografia = DateTime.Now;
                    admin.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    admin.DataFotografia = DateTime.Now;
                    admin.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // colocar conteudos nas tabelas
                    // do identity
                    var user = _userManager.FindByIdAsync(admin.AuthenticationID).Result;

                    await _userManager.SetUserNameAsync(user, admin.NomeAdmin);
                    await _userManager.SetEmailAsync(user, admin.Email);
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, admin.Password);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Administrador editou uma conta.");

                        var userId = await _userManager.GetUserIdAsync(user);

                        // enviar email de confirmacao de email

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var request = _http.HttpContext.Request;

                        var href = "https://" + request.Host.ToString() + "/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code + "&returnUrl=%2F";

                        var htmlElement = "<a href='" + href + "' target='_blank'>clique aqui</a>";

                        EmailSender emailSender = new(_optionsAccessor, _loggerEmail);

                        await emailSender.SendEmailAsync(admin.Email, "Confirme o seu email", htmlElement);
                    }
                    else
                    {
                        // se o resultado da adicao nao tiver exito
                        // lanca excecao para a execucao saltar para
                        // o bloco 'catch'
                        throw new Exception();
                    }

                    // adicionar dados do admin
                    // a BD
                    _context.Update(admin);

                    // realizar commit
                    await _context.SaveChangesAsync();

                    // e possivel guardar imagem em disco
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
                        await fotografia.CopyToAsync(stream);

                    }
                    else
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
                    }

                    // voltar a lista
                    return RedirectToAction(nameof(Index));

                }
                // apanhar excecao para escrever um erro de modelo personalizado
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados do administrador " + admin.NomeAdmin);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(admin);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Retorna o codigo de erro 404 se o id ou o admin
            // nao existir ou for nulo
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            // Retorna a entidade encontrada de forma assincrona
            var admin = await _context.Admin
                .FindAsync(id);

            // Retorna o erro 404 se admin for nulo
            if (admin == null)
            {
                return NotFound();
            }

            // voltar a lista
            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retorna o problema que escreve:
            // Entity set 'ApplicationDbContext.Admin'  is null.
            // se o admin nao existir ou for nulo
            if (_context.Admin == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Admin'  is null.");
            }

            // Retorna a entidade encontrada de forma assincrona
            var admin = await _context.Admin.FindAsync(id);

            // se admin existir, remover da BD
            if (admin != null)
            {
                try
                {
                    // eliminar fotografia de user do disco

                    // buscar nome na base de dados
                    var nomeFoto = admin.NomeFotografia;

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

                    //_context.Utilizador_Registado.Attach(utilizador_Registado);

                    _context.Remove(admin);

                    // apagar os dados do Identity

                    var user = _userManager.FindByIdAsync(admin.AuthenticationID).Result;
                    var result = await _userManager.DeleteAsync(user);

                    // se nao exisitrem mais administradores, remover role
                    if ((await _userManager.GetUsersInRoleAsync("Administrator")).ToImmutableArray().Length == 0)
                    {
                        await _roleManager.DeleteAsync(_roleManager.FindByNameAsync("Administrator").Result);
                    }

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Um administrador foi removido.");

                    //voltar a lista
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a remoção dos dados do administrador " + admin.NomeAdmin);
                }
            }

            return View(admin);
        }

        //verificar se existe admin com ID = id (paramentro)
        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.IDAdmin == id);
        }
    }
}
