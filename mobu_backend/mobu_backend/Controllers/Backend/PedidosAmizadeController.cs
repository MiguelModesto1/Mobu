using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Controllers.Backend
{
    public class PedidosAmizadeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosAmizadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PedidosAmizade
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PedidosAmizade.Include(p => p.DonoListaPedidos).Include(p => p.Remetente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PedidosAmizade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosAmizade = await _context.PedidosAmizade
                .Include(p => p.DonoListaPedidos)
                .Include(p => p.Remetente)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedidosAmizade == null)
            {
                return NotFound();
            }

            return View(pedidosAmizade);
        }

        // GET: PedidosAmizade/Create
        public IActionResult Create()
        {
            ViewData["DonoListaPedidosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: PedidosAmizade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,DonoListaPedidosFK,RemetenteFK")] PedidosAmizade pedidosAmizade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidosAmizade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonoListaPedidosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", pedidosAmizade.DonoListaPedidosFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", pedidosAmizade.RemetenteFK);
            return View(pedidosAmizade);
        }

        // GET: PedidosAmizade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosAmizade = await _context.PedidosAmizade.FindAsync(id);
            if (pedidosAmizade == null)
            {
                return NotFound();
            }
            ViewData["DonoListaPedidosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", pedidosAmizade.DonoListaPedidosFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", pedidosAmizade.RemetenteFK);
            return View(pedidosAmizade);
        }

        // POST: PedidosAmizade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,DonoListaPedidosFK,RemetenteFK")] PedidosAmizade pedidosAmizade)
        {
            if (id != pedidosAmizade.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidosAmizade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidosAmizadeExists(pedidosAmizade.IdPedido))
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
            ViewData["DonoListaPedidosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", pedidosAmizade.DonoListaPedidosFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", pedidosAmizade.RemetenteFK);
            return View(pedidosAmizade);
        }

        // GET: PedidosAmizade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosAmizade = await _context.PedidosAmizade
                .Include(p => p.DonoListaPedidos)
                .Include(p => p.Remetente)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedidosAmizade == null)
            {
                return NotFound();
            }

            return View(pedidosAmizade);
        }

        // POST: PedidosAmizade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidosAmizade = await _context.PedidosAmizade.FindAsync(id);
            if (pedidosAmizade != null)
            {
                _context.PedidosAmizade.Remove(pedidosAmizade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidosAmizadeExists(int id)
        {
            return _context.PedidosAmizade.Any(e => e.IdPedido == id);
        }
    }
}
