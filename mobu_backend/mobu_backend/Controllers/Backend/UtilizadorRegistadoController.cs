﻿using System.Collections.Immutable;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mobu_backend.Areas.Identity.Pages.Account;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;

namespace mobu.Controllers.Backend
{

    [Authorize(Roles = "Administrator")]
    public class UtilizadorRegistadoController : Controller
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
        /// Interface para a funcao de logging
        /// </summary>
        private readonly ILogger<RegisterModel> _logger;

        /// <summary>
        /// Interface para a funcao de logging do Remetente de emails
        /// </summary>
        private readonly ILogger<EmailSender> _emailLogger;

        /// <summary>
        /// Interface que permite o acesso ao contexto HTTP
        /// </summary>
        private readonly IHttpContextAccessor _http;

        /// <summary>
        /// opcoes para ter acesso à chave da API do SendGrid
        /// </summary>
        private readonly IOptions<AuthMessageSenderOptions> _optionsAccessor;


        public UtilizadorRegistadoController(
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
            _roleManager = roleManager;
            _logger = logger;
            _emailLogger = loggerEmail;
            _http = http;
            _optionsAccessor = optionsAccessor;
        }

        // GET: UtilizadorRegistado
        public async Task<IActionResult> Index()
        {
            // Consulta que inclui dados do utilizador
            var utilizadores = _context.UtilizadorRegistado;
            //voltar a lista
            return View(await utilizadores.ToListAsync());
        }

        // GET: UtilizadorRegistado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Retorna o codigo de erro 404 se o id ou o user
            // nao existir ou for nulo
            if (id == null || _context.UtilizadorRegistado == null)
            {
                return NotFound();
            }

            // Consulta que retorna todos os detalhes do
            // utilizador com IDAdmin = id
            var utilizadorRegistado = await _context.UtilizadorRegistado
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizadorRegistado == null)
            {
                return NotFound();
            }

            return View(utilizadorRegistado);
        }

        // GET: UtilizadorRegistado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UtilizadorRegistado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("IDUtilizador,NomeUtilizador,Email,Password,DataJuncao,DataNasc,NomeFotografia,DataFotografia,AuthenticationID")] UtilizadorRegistado utilizadorRegistado, IFormFile fotografia)
        {
            // data de juncao
            utilizadorRegistado.DataJuncao = DateTime.Now;

            // variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;
            var user = new IdentityUser();

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                utilizadorRegistado.DataFotografia = DateTime.Now;
                utilizadorRegistado.NomeFotografia = "default_avatar.png";
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
                    utilizadorRegistado.DataFotografia = DateTime.Now;
                    utilizadorRegistado.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    utilizadorRegistado.DataFotografia = DateTime.Now;
                    utilizadorRegistado.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // colocar conteudos nas tabelas
                    // do identity

                    await _userManager.SetUserNameAsync(user, utilizadorRegistado.NomeUtilizador);
                    await _userManager.SetEmailAsync(user, utilizadorRegistado.Email);

                    var result = await _userManager.CreateAsync(user, utilizadorRegistado.Password);

                    if (result.Succeeded)
                    {
                        // criar role de registado se esta nao existir
                        if (!await _roleManager.RoleExistsAsync("Registered"))
                        {
                            var role = new IdentityRole
                            {
                                Name = "Registered",
                                ConcurrencyStamp = Guid.NewGuid().ToString()
                            };
                            await _roleManager.CreateAsync(role);
                        }

                        // atribuir ao utilizador em questao a role
                        await _userManager.AddToRoleAsync(user, _roleManager.Roles
                            .Where(r => r.Name == "Registered")
                            .Select(r => r.Name)
                            .ToImmutableArray()[0]);

                        _logger.LogInformation("Administrador criou uma nova conta com palavra-passe.");

                        var userId = await _userManager.GetUserIdAsync(user);

                        // enviar email de confirmacao de email

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var request = _http.HttpContext.Request;

                        var href = "https://" + request.Host.ToString() + "/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code + "&returnUrl=%2F";

                        var htmlElement = "Para confirmar o seu email <a href='" + href + "' target='_blank'>clique aqui</a>.";

                        EmailSender emailSender = new(_optionsAccessor, _emailLogger);

                        await emailSender.SendEmailAsync(utilizadorRegistado.Email, "Confirme o seu email", htmlElement);
                    }
                    else
                    {
                        // se o resultado da adicao nao tiver exito
                        // lanca excecao para a execucao saltar para
                        // o bloco 'catch'
                        throw new Exception();
                    }

                    utilizadorRegistado.AuthenticationID = user.Id;

                    // adicionar dados do utilizador registado
                    // a BD
                    _context.Attach(utilizadorRegistado);

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
                catch (Exception)
                {
                    //informar de erro de adicao
                    _logger.LogInformation("$Ocorreu um erro com a adição do utilizador" + utilizadorRegistado.NomeUtilizador + "\nA apagar utilizador...");
                    
                    if (UtilizadorRegistadoExists(utilizadorRegistado.IDUtilizador))
                    {
                        _context.Remove(utilizadorRegistado);

                        // realizar commit
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Utilizador apagado!");
                    }

                    // se exisitr user na base de dados do negocio 
                    if(await _context.UtilizadorRegistado.FirstOrDefaultAsync(ur => ur.IDUtilizador == utilizadorRegistado.IDUtilizador) != null)
                    {
                        await _userManager.DeleteAsync(user);

                        _logger.LogInformation("Utilizador apagado do Identity!");

                        // se nao existirem utilizadores
                        if ((await _userManager.GetUsersInRoleAsync("Registered")).ToImmutableArray().Length == 0)
                        {
                            // apagar role 'Registered'
                            await _roleManager.DeleteAsync(_roleManager.FindByNameAsync("Registered").Result);
                        }
                    }
                    

                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do utilizador " + utilizadorRegistado.NomeUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizadorRegistado);
        }

        // GET: UtilizadorRegistado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UtilizadorRegistado == null)
            {
                return NotFound();
            }

            var utilizadorRegistado = await _context.UtilizadorRegistado
                .FirstOrDefaultAsync(a => a.IDUtilizador == id);
            if (utilizadorRegistado == null)
            {
                return NotFound();
            }
            return View(utilizadorRegistado);
        }

        // POST: UtilizadorRegistado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDUtilizador,NomeUtilizador,Email,Password,DataJuncao,DataNasc,NomeFotografia,DataFotografia,AuthenticationID")] UtilizadorRegistado utilizadorRegistado, IFormFile fotografia)
        {

            //variaveis auxiliares
            string nomeFoto = _context.UtilizadorRegistado
                        .Where(ur => ur.IDUtilizador == id)
                        .Select(ur => ur.NomeFotografia)
                        .ToImmutableArray()[0];
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                utilizadorRegistado.DataFotografia = DateTime.Now;
                utilizadorRegistado.NomeFotografia = "default_avatar.png";
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
                    nomeFoto = _context.UtilizadorRegistado
                        .Where(ur => ur.IDUtilizador == id)
                        .Select(ur => ur.NomeFotografia)
                        .ToImmutableArray()[0];

                    if (nomeFoto == "default_avatar.png")
                    {
                        Guid g = Guid.NewGuid();
                        nomeFoto = g.ToString();
                        string extensaoFoto =
                            Path.GetExtension(fotografia.FileName).ToLower();
                        nomeFoto += extensaoFoto;
                    }

                    // tornar foto do modelo na foto processada acima
                    utilizadorRegistado.DataFotografia = DateTime.Now;
                    utilizadorRegistado.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    utilizadorRegistado.DataFotografia = DateTime.Now;
                    utilizadorRegistado.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {

                    // colocar conteudos nas tabelas
                    // do identity
                    var user = _userManager.FindByIdAsync(utilizadorRegistado.AuthenticationID).Result;

                    await _userManager.SetUserNameAsync(user, utilizadorRegistado.NomeUtilizador);
                    await _userManager.SetEmailAsync(user, utilizadorRegistado.Email);
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, utilizadorRegistado.Password);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Utilizador editou uma conta.");

                        var userId = await _userManager.GetUserIdAsync(user);

                        // enviar email de confirmacao de email

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var request = _http.HttpContext.Request;

                        var href = "https://" + request.Host.ToString() + "/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code + "&returnUrl=%2F";

                        var htmlElement = "Para confirmar o seu email <a href='" + href + "' target='_blank'>clique aqui</a>";

                        EmailSender emailSender = new(_optionsAccessor, _emailLogger);

                        await emailSender.SendEmailAsync(utilizadorRegistado.Email, "Confirme o seu email", htmlElement);

                    }
                    else
                    {
                        // se o resultado da adicao nao tiver exito
                        // lanca excecao para a execucao saltar para
                        // o bloco 'catch'
                        throw new Exception();
                    }

                    // editar dados do utilizador registado
                    // na BD
                    _context.Update(utilizadorRegistado);

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

                        // abrir objeto para manipular imagem
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
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados do utilizador " + utilizadorRegistado.NomeUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizadorRegistado);
        }

        // GET: UtilizadorRegistado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UtilizadorRegistado == null)
            {
                return NotFound();
            }

            var utilizadorRegistado = await _context.UtilizadorRegistado
                .FindAsync(id);

            if (utilizadorRegistado == null)
            {
                return NotFound();
            }

            return View(utilizadorRegistado);
        }

        // POST: UtilizadorRegistado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UtilizadorRegistado == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UtilizadorRegistado'  is null.");
            }

            // Retorna a entidade encontrada de forma assincrona
            var utilizadorRegistado = await _context.UtilizadorRegistado
                    .FindAsync(id);

            // se user existir, remover da BD
            if (utilizadorRegistado != null)
            {
                try
                {
                    // eliminar fotografia de user do disco

                    // buscar nome na base de dados
                    var nomeFoto = utilizadorRegistado.NomeFotografia;

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

                    //_context.UtilizadorRegistado.Attach(utilizadorRegistado);

                    _context.Remove(utilizadorRegistado);

                    // apagar os dados do Identity

                    var user = _userManager.FindByIdAsync(utilizadorRegistado.AuthenticationID).Result;
                    var result = await _userManager.DeleteAsync(user);

                    // se nao exisitrem mais utilizadores registados, remover role
                    if ((await _userManager.GetUsersInRoleAsync("Registered")).ToImmutableArray().Length == 0)
                    {
                        await _roleManager.DeleteAsync(_roleManager.FindByNameAsync("Registered").Result);
                    }

                    await _context.SaveChangesAsync();

                    //voltar a lista
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a remoção dos dados do utilizador " + utilizadorRegistado.NomeUtilizador);
                }
            }

            return View(utilizadorRegistado);
        }

        //verificar se existe user com ID = id (parametro)
        private bool UtilizadorRegistadoExists(int id)
        {
            return _context.UtilizadorRegistado.Any(e => e.IDUtilizador == id);
        }
    }
}
