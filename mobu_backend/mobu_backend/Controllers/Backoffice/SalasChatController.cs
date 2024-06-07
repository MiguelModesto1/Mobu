using System.Collections.Immutable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    [Authorize]
    public class SalasChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<SalasChatController> _logger;

        public SalasChatController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ILogger<SalasChatController> logger
            )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: SalasChat
        public async Task<IActionResult> Index()
        {
            return _context.SalasChat != null ?
                        View(await _context.SalasChat.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.SalasChat'  is null.");
        }

        // GET: SalasChat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SalasChat == null)
            {
                return NotFound();
            }

            var salasChat = await _context.SalasChat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salasChat == null)
            {
                return NotFound();
            }

            return View(salasChat);
        }

        // GET: SalasChat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalasChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDSala,NomeSala,SeGrupo,NomeFotografia,DataFotografia")] SalasChat salasChat, IFormFile fotografia)
        {
            // variaveis auxiliares
            string nomeFoto = "";
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                salasChat.DataFotografia = DateTime.Now;
                salasChat.NomeFotografia = "default_avatar.png";
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
                    salasChat.DataFotografia = DateTime.Now;
                    salasChat.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    salasChat.DataFotografia = DateTime.Now;
                    salasChat.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {

                    // adicionar dados da sala de chat
                    // a BD
                    _context.Attach(salasChat);

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
                    _logger.LogInformation("$Ocorreu um erro com a adição da sala" + salasChat.NomeSala + "\nA apagar sala...");
                    if (SalasChatExists(salasChat.IDSala))
                    {
                        _context.Remove(salasChat);
                    }

                    _logger.LogInformation("Administrador apagado!");

                    ModelState.AddModelError("", "Ocorreu um erro com a adição da sala" + salasChat.NomeSala);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(salasChat);
        }

        // GET: SalasChat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SalasChat == null)
            {
                return NotFound();
            }

            var salasChat = await _context.SalasChat.FindAsync(id);
            if (salasChat == null)
            {
                return NotFound();
            }
            return View(salasChat);
        }

        // POST: SalasChat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDSala,NomeSala,SeGrupo,NomeFotografia,DataFotografia")] SalasChat salasChat, IFormFile fotografia)
        {
            //variaveis auxiliares
            string nomeFoto = _context.SalasChat
                        .Where(sc => sc.IDSala == id)
                        .Select(sc => sc.NomeFotografia)
                        .ToImmutableArray()[0];
            bool haFoto = false;

            if (fotografia == null)
            {
                // sem foto
                // foto por predefenicao
                salasChat.DataFotografia = DateTime.Now;
                salasChat.NomeFotografia = "default_avatar.png";
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
                    nomeFoto = _context.SalasChat
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
                    salasChat.DataFotografia = DateTime.Now;
                    salasChat.NomeFotografia = nomeFoto;

                    // preparar foto p/ser guardada no disco
                    // do servidor
                    haFoto = true;
                }
                else
                {
                    // ha ficheiro, mas e invalido
                    // foto predefinida adicionada
                    salasChat.DataFotografia = DateTime.Now;
                    salasChat.NomeFotografia = "default_avatar.png";
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // editar dados da sala de chat
                    // na BD
                    _context.Update(salasChat);

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
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados da sala " + salasChat.NomeSala);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(salasChat);
        }

        // GET: SalasChat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SalasChat == null)
            {
                return NotFound();
            }

            var salasChat = await _context.SalasChat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salasChat == null)
            {
                return NotFound();
            }

            return View(salasChat);
        }

        // POST: SalasChat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SalasChat == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SalasChat'  is null.");
            }
            // Retorna a entidade encontrada de forma assincrona
            var salasChat = await _context.SalasChat
                    .FindAsync(id);

            // se user existir, remover da BD
            if (salasChat != null)
            {
                try
                {
                    // eliminar fotografia de user do disco

                    // buscar nome na base de dados
                    var nomeFoto = salasChat.NomeFotografia;

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

                    //_context.UtilizadorRegistado.Attach(utilizador_Registado);

                    _context.Remove(salasChat);
                    await _context.SaveChangesAsync();

                    //voltar a lista
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a remoção dos dados da sala " + salasChat.NomeSala);
                }
            }

            return View(salasChat);
        }

        private bool SalasChatExists(int id)
        {
            return (_context.SalasChat?.Any(e => e.IDSala == id)).GetValueOrDefault();
        }
    }
}
