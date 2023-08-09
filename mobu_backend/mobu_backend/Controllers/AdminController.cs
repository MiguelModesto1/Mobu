using System.Collections.Immutable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Areas.Identity.Pages.Account;

namespace mobu_backend.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        /// <summary>
        /// objeto que referencia a Base de Dados do projeto
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Interface para guardar utilizadores
        /// </summary>
        private readonly IUserStore<IdentityUser> _userStore;

        /// <summary>
        /// Interface para guardar emails de utilizadores
        /// </summary>
        private readonly IUserEmailStore<IdentityUser> _userEmailStore;

        /// <summary>
        /// ferramenta com acesso aos dados da pessoa autenticada
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

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

        public AdminController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _userStore = userStore;
            _userEmailStore = (IUserEmailStore<IdentityUser>)_userStore;
            _logger = logger;
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

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDAdmin,NomeAdmin,Password,DataJuncao,Email,NomeFotografia,DataFotografia,AuthenticationID")] Admin admin, IFormFile fotografia)
        {
            // data de juncao
            admin.DataJuncao = DateTime.Now;

            // variaveis auxiliares
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

                    await _userStore.SetUserNameAsync(user, admin.NomeAdmin, CancellationToken.None);
                    await _userEmailStore.SetEmailAsync(user, admin.Email, CancellationToken.None);

                    var result = await _userManager.CreateAsync(user, admin.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Administrador criou uma nova conta com palavra-passe.");

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        /*IMPLEMENTAR CONFIRMACAO DE EMAIL*/

                        var confirmEmail = await _userManager.ConfirmEmailAsync(user, code);
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
                    _logger.LogInformation("$Ocorreu um erro com a adição do admin" + admin.NomeAdmin + "\nA apagar administrador...");
                    await _userManager.DeleteAsync(user);
                    _logger.LogInformation("Administrador apagado!");

                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do utilizador " + admin.NomeAdmin);
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
        public async Task<IActionResult> Edit(int id, [Bind("IDAdmin,NomeAdmin,Password,DataJuncao,Email,NomeFotografia,DataFotografia,AuthenticationID")] Admin admin, IFormFile fotografia)
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

                    // colocar conteudos nas tabelas
                    // do identity
                    var user = _userManager.FindByIdAsync(admin.AuthenticationID).Result;

                    await _userStore.SetUserNameAsync(user, admin.NomeAdmin, CancellationToken.None);
                    await _userEmailStore.SetEmailAsync(user, admin.Email, CancellationToken.None);
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, admin.Password);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Administrador editou uma conta.");

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        /*IMPLEMENTAR CONFIRMACAO DE EMAIL*/
                        var confirmEmail = await _userManager.ConfirmEmailAsync(user, code);
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
