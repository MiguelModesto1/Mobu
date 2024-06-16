using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    /// <summary>
    /// Controlador de mensagens
    /// </summary>
    [Authorize]
    public class MensagemController : Controller
    {
        /// <summary>
        /// Contexto da base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controlador de mensagens
        /// </summary>
        /// <param name="context"></param>
        public MensagemController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter vista de Índice
        /// </summary>
        /// <returns></returns>
        // GET: Mensagem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mensagem.Include(m => m.Remetente).Include(m => m.Sala);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Obter vista de detalhes
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns></returns>
        // GET: Mensagem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagem
                .Include(m => m.Remetente)
                .Include(m => m.Sala)
                .FirstOrDefaultAsync(m => m.IDMensagem == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        /// <summary>
        /// Obter vista de criação
        /// </summary>
        /// <returns></returns>
        // GET: Mensagem/Create
        public IActionResult Create()
        {
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala");
            return View();
        }

        /// <summary>
        /// Publicar criação
        /// </summary>
        /// <param name="mensagem">Instância de modelo</param>
        /// <returns></returns>
        // POST: Mensagem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDMensagem,ConteudoMsg,DataHoraMsg,EstadoMensagem,RemetenteFK,SalaFK")] Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mensagem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        /// <summary>
        /// Obter vista de edição
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns></returns>
        // GET: Mensagem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagem.FindAsync(id);
            if (mensagem == null)
            {
                return NotFound();
            }
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        /// <summary>
        /// Publicar edição
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <param name="mensagem">Instância de modelo</param>
        /// <returns></returns>
        // POST: Mensagem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDMensagem,ConteudoMsg,DataHoraMsg,EstadoMensagem,RemetenteFK,SalaFK")] Mensagem mensagem)
        {
            if (id != mensagem.IDMensagem)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mensagem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MensagemExists(mensagem.IDMensagem))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        /// <summary>
        /// Obter vista de remoção
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns></returns>
        // GET: Mensagem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagem
                .Include(m => m.Remetente)
                .Include(m => m.Sala)
                .FirstOrDefaultAsync(m => m.IDMensagem == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        /// <summary>
        /// Publicar remoção
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns></returns>
        // POST: Mensagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mensagem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Mensagem'  is null.");
            }
            var mensagem = await _context.Mensagem.FindAsync(id);
            if (mensagem != null)
            {
                _context.Mensagem.Remove(mensagem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verificar se uma mensagem existe na base de dados
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns></returns>
        private bool MensagemExists(int id)
        {
            return _context.Mensagem.Any(e => e.IDMensagem == id);
        }
    }
}
