using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// objeto que referencia a Base de Dados do projeto
        /// </summary>
        private readonly ApplicationDbContext _context;

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

        public AdminController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            // Consulta que inclui dados sobre a fotografia
            // do adiminstrador (FALTA AUTENTICACAO)
            var utilizadores = _context.Admin
                            .Include(ur => ur.Fotografia);
            //voltar a lista
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
                .Include(m => m.Fotografia)
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
        public async Task<IActionResult> Create([Bind("IDAdmin,NomeAdmin,Password,Email,IDFotografia")] Admin admin, IFormFile fotografia)
        {
            //variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                admin.Fotografia.DataFotografia = DateTime.Now;
                admin.Fotografia.Local = "No foto";
                admin.Fotografia.NomeFicheiro = "default_avatar.png";
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
                    admin.Fotografia.DataFotografia = DateTime.Now;
                    admin.Fotografia.Local = "";
                    admin.Fotografia.NomeFicheiro = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    admin.Fotografia.DataFotografia = DateTime.Now;
                    admin.Fotografia.Local = "No foto";
                    admin.Fotografia.NomeFicheiro = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // adicionar dados do admin
                    // a BD
                    _context.Add(admin);

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
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do administrador " + admin.NomeAdmin);
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
                .Include(a => a.Fotografia)
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
        public async Task<IActionResult> Edit(int id, [Bind("IDAdmin,NomeAdmin,Password,Email,IDFotografia")] Admin admin, IFormFile fotografia)
        {

            admin.Fotografia.Id = _context.Admin
                .Include(ur => ur.Fotografia)
                .Where(ur => ur.IDAdmin == id)
                .Select(ur => ur.IDFotografia)
                .ToImmutableArray()[0];

            //variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                admin.Fotografia.DataFotografia = DateTime.Now;
                admin.Fotografia.Local = "No foto";
                admin.Fotografia.NomeFicheiro = "default_avatar.png";
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
                    nomeFoto = _context.Admin
                        .Include(ur => ur.Fotografia)
                        .Where(ur => ur.IDAdmin == id)
                        .Select(ur => ur.Fotografia.NomeFicheiro)
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
                    admin.Fotografia.DataFotografia = DateTime.Now;
                    admin.Fotografia.Local = "";
                    admin.Fotografia.NomeFicheiro = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    admin.Fotografia.DataFotografia = DateTime.Now;
                    admin.Fotografia.Local = "No foto";
                    admin.Fotografia.NomeFicheiro = "default_avatar.png";
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
                .Include(m => m.Fotografia)
                .FirstOrDefaultAsync(m => m.IDAdmin == id);

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

            // se admin existir ,remover da BD
            if (admin != null)
            {
                _context.Admin.Remove(admin);
            }
            
            // guardar alteracoes na BD (realizar COMMIT)
            await _context.SaveChangesAsync();

            // voltar a lista
            return RedirectToAction(nameof(Index));
        }

        //verificar se existe admin com ID = id (paramentro)
        private bool AdminExists(int id)
        {
          return _context.Admin.Any(e => e.IDAdmin == id);
        }
    }
}
