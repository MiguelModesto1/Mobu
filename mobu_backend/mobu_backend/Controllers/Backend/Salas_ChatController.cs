using System.Collections.Immutable;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using static System.Net.WebRequestMethods;

namespace mobu.Controllers.Backend
{
    [Authorize(Roles = "Administrator")]
    public class Salas_ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<Salas_ChatController> _logger;

        public Salas_ChatController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ILogger<Salas_ChatController> logger
            )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Salas_Chat
        public async Task<IActionResult> Index()
        {
            return _context.Salas_Chat != null ?
                        View(await _context.Salas_Chat.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Salas_Chat'  is null.");
        }

        // GET: Salas_Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Salas_Chat == null)
            {
                return NotFound();
            }

            var salas_Chat = await _context.Salas_Chat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salas_Chat == null)
            {
                return NotFound();
            }

            return View(salas_Chat);
        }

        // GET: Salas_Chat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salas_Chat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDSala,NomeSala,SeGrupo,NomeFotografia,DataFotografia")] Salas_Chat salas_Chat, IFormFile fotografia)
        {
            // variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                salas_Chat.DataFotografia = DateTime.Now;
                salas_Chat.NomeFotografia = "default_avatar.png";
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
                    salas_Chat.DataFotografia = DateTime.Now;
                    salas_Chat.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    salas_Chat.DataFotografia = DateTime.Now;
                    salas_Chat.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {

                    // adicionar dados da sala de chat
                    // a BD
                    _context.Attach(salas_Chat);

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
                    _logger.LogInformation("$Ocorreu um erro com a adição da sala" + salas_Chat.NomeSala + "\nA apagar sala...");
                    if (Salas_ChatExists(salas_Chat.IDSala))
                    {
                        _context.Remove(salas_Chat);
                    }
                    
                    _logger.LogInformation("Administrador apagado!");

                    ModelState.AddModelError("", "Ocorreu um erro com a adição da sala" + salas_Chat.NomeSala);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(salas_Chat);
        }

        // GET: Salas_Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Salas_Chat == null)
            {
                return NotFound();
            }

            var salas_Chat = await _context.Salas_Chat.FindAsync(id);
            if (salas_Chat == null)
            {
                return NotFound();
            }
            return View(salas_Chat);
        }

        // POST: Salas_Chat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDSala,NomeSala,SeGrupo,NomeFotografia,DataFotografia")] Salas_Chat salas_Chat, IFormFile fotografia)
        {
            //variaveis auxiliares
            string nomeFoto = _context.Salas_Chat
                        .Where(sc => sc.IDSala == id)
                        .Select(sc => sc.NomeFotografia)
                        .ToImmutableArray()[0];
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                salas_Chat.DataFotografia = DateTime.Now;
                salas_Chat.NomeFotografia = "default_avatar.png";
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
                    nomeFoto = _context.Salas_Chat
                        .Where(sc => sc.IDSala == id)
                        .Select(sc => sc.NomeFotografia)
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
                    salas_Chat.DataFotografia = DateTime.Now;
                    salas_Chat.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    salas_Chat.DataFotografia = DateTime.Now;
                    salas_Chat.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // editar dados da sala de chat
                    // na BD
                    _context.Update(salas_Chat);

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
                    //voltar a lista
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados da sala " + salas_Chat.NomeSala);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(salas_Chat);
        }

        // GET: Salas_Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Salas_Chat == null)
            {
                return NotFound();
            }

            var salas_Chat = await _context.Salas_Chat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salas_Chat == null)
            {
                return NotFound();
            }

            return View(salas_Chat);
        }

        // POST: Salas_Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Salas_Chat == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Salas_Chat'  is null.");
            }
            // Retorna a entidade encontrada de forma assincrona
            var salas_Chat = await _context.Salas_Chat
                    .FindAsync(id);

            // se user existir, remover da BD
            if (salas_Chat != null)
            {
                try
                {
                    // eliminar fotografia de user do disco

                    // buscar nome na base de dados
                    var nomeFoto = salas_Chat.NomeFotografia;

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

                    _context.Remove(salas_Chat);
                    await _context.SaveChangesAsync();

                    //voltar a lista
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a remoção dos dados da sala " + salas_Chat.NomeSala);
                }
            }

            return View(salas_Chat);
        }

        private bool Salas_ChatExists(int id)
        {
            return (_context.Salas_Chat?.Any(e => e.IDSala == id)).GetValueOrDefault();
        }
    }
}
