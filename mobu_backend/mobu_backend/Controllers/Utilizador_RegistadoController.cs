using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Controllers
{
    public class Utilizador_RegistadoController : Controller
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

        public Utilizador_RegistadoController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Utilizador_Registado
        public async Task<IActionResult> Index()
        {

            var utilizadores = _context.Utilizador_Registado;

            return View(await utilizadores.ToListAsync());
        }

        // GET: Utilizador_Registado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Utilizador_Registado == null)
            {
                return NotFound();
            }

            var utilizador_Registado = await _context.Utilizador_Registado
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizador_Registado == null)
            {
                return NotFound();
            }

            return View(utilizador_Registado);
        }

        // GET: Utilizador_Registado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizador_Registado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDUtilizador,NomeUtilizador,Email,Password,DataJuncao,NomeFotografia,DataFotografia")] Utilizador_Registado utilizador_Registado, IFormFile fotografia)
        {
            // data de juncao
            utilizador_Registado.DataJuncao = DateTime.Now;

            // variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;

            if(fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                utilizador_Registado.DataFotografia = DateTime.Now;
                utilizador_Registado.NomeFotografia = "default_avatar.png";
            }
            else
            {
                // ficheiro existe
                // sera valido?
                if(fotografia.ContentType == "image/jpeg" ||
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
                    utilizador_Registado.DataFotografia = DateTime.Now;
                    utilizador_Registado.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    utilizador_Registado.DataFotografia = DateTime.Now;
                    utilizador_Registado.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // adicionar dados do utilizador registado
                    // a BD
                    _context.Attach(utilizador_Registado);

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
                    
                }catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do utilizador " + utilizador_Registado.NomeUtilizador);
                }  
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizador_Registado);
        }

        // GET: Utilizador_Registado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utilizador_Registado == null)
            {
                return NotFound();
            }

            var utilizador_Registado = await _context.Utilizador_Registado
                .FirstOrDefaultAsync(a => a.IDUtilizador == id);
            if (utilizador_Registado == null)
            {
                return NotFound();
            }
            return View(utilizador_Registado);
        }

        // POST: Utilizador_Registado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDUtilizador,NomeUtilizador,Email,Password,DataJuncao,NomeFotografia,DataFotografia")] Utilizador_Registado utilizador_Registado, IFormFile fotografia)
        {

            //variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                utilizador_Registado.DataFotografia = DateTime.Now;
                utilizador_Registado.NomeFotografia = "default_avatar.png";

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
                    nomeFoto = _context.Utilizador_Registado
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
                    utilizador_Registado.DataFotografia = DateTime.Now;
                    utilizador_Registado.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    utilizador_Registado.DataFotografia = DateTime.Now;
                    utilizador_Registado.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // editar dados do utilizador registado
                    // na BD
                    _context.Update(utilizador_Registado);

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
                    // voltar a lista
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados do utilizador " + utilizador_Registado.NomeUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizador_Registado);
        }

        // GET: Utilizador_Registado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utilizador_Registado == null)
            {
                return NotFound();
            }

            var utilizador_Registado = await _context.Utilizador_Registado
                .FindAsync(id);

            if (utilizador_Registado == null)
            {
                return NotFound();
            }

            return View(utilizador_Registado);
        }

        // POST: Utilizador_Registado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utilizador_Registado == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Utilizador_Registado'  is null.");
            }

            var utilizador_Registado = await _context.Utilizador_Registado
                    .FindAsync(id);

            if (utilizador_Registado != null)
            {
                try
                {
                    // eliminar fotografia de user do disco

                    // buscar nome na base de dados
                    var nomeFoto = utilizador_Registado.NomeFotografia;

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

                    _context.Remove(utilizador_Registado);

                    await _context.SaveChangesAsync();

                    //voltar a lista
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a remoção dos dados do utilizador " + utilizador_Registado.NomeUtilizador);
                }
            }

            return View(utilizador_Registado);   
        }

        private bool Utilizador_RegistadoExists(int id)
        {
          return _context.Utilizador_Registado.Any(e => e.IDUtilizador == id);
        }
    }
}
