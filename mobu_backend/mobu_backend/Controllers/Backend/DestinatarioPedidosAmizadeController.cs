using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    [Authorize]
    public class DestinatarioPedidosAmizadeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinatarioPedidosAmizadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DestinatarioPedidosAmizade
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DestinatarioPedidosAmizade.Include(d => d.RemetentePedido);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DestinatarioPedidosAmizade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DestinatarioPedidosAmizade == null)
            {
                return NotFound();
            }

            var destinatarioPedidosAmizade = await _context.DestinatarioPedidosAmizade
                .Include(d => d.RemetentePedido)
                .FirstOrDefaultAsync(m => m.IDPedido == id);
            if (destinatarioPedidosAmizade == null)
            {
                return NotFound();
            }

            return View(destinatarioPedidosAmizade);
        }

        // GET: DestinatarioPedidosAmizade/Create
        public IActionResult Create()
        {
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: DestinatarioPedidosAmizade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDPedido,IDDestinatarioPedido,DataHoraPedido,EstadoPedido,RemetenteFK")] DestinatarioPedidosAmizade destinatarioPedidosAmizade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(destinatarioPedidosAmizade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", destinatarioPedidosAmizade.RemetenteFK);
            return View(destinatarioPedidosAmizade);
        }

        // GET: DestinatarioPedidosAmizade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DestinatarioPedidosAmizade == null)
            {
                return NotFound();
            }

            var destinatarioPedidosAmizade = await _context.DestinatarioPedidosAmizade.FindAsync(id);
            if (destinatarioPedidosAmizade == null)
            {
                return NotFound();
            }
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", destinatarioPedidosAmizade.RemetenteFK);
            return View(destinatarioPedidosAmizade);
        }

        // POST: DestinatarioPedidosAmizade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDPedido,IDDestinatarioPedido,DataHoraPedido,EstadoPedido,RemetenteFK")] DestinatarioPedidosAmizade destinatarioPedidosAmizade)
        {
            if (id != destinatarioPedidosAmizade.IDPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destinatarioPedidosAmizade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinatarioPedidosAmizadeExists(destinatarioPedidosAmizade.RemetenteFK))
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
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", destinatarioPedidosAmizade.RemetenteFK);
            return View(destinatarioPedidosAmizade);
        }

        // GET: DestinatarioPedidosAmizade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DestinatarioPedidosAmizade == null)
            {
                return NotFound();
            }

            var destinatarioPedidosAmizade = await _context.DestinatarioPedidosAmizade
                .Include(d => d.RemetentePedido)
                .FirstOrDefaultAsync(m => m.IDPedido == id);
            if (destinatarioPedidosAmizade == null)
            {
                return NotFound();
            }

            return View(destinatarioPedidosAmizade);
        }

        // POST: DestinatarioPedidosAmizade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DestinatarioPedidosAmizade == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DestinatarioPedidosAmizade'  is null.");
            }
            var destinatarioPedidosAmizade = await _context.DestinatarioPedidosAmizade.FindAsync(id);
            if (destinatarioPedidosAmizade != null)
            {
                _context.DestinatarioPedidosAmizade.Remove(destinatarioPedidosAmizade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinatarioPedidosAmizadeExists(int id)
        {
            return _context.DestinatarioPedidosAmizade.Any(e => e.IDPedido == id);
        }
    }
}
