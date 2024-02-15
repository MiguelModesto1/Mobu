using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    [Authorize(Roles = "Administrator")]
    public class Destinatario_Pedidos_AmizadeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Destinatario_Pedidos_AmizadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Destinatario_Pedidos_Amizade
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Destinatario_Pedidos_Amizade.Include(d => d.RemetentePedido);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Destinatario_Pedidos_Amizade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Destinatario_Pedidos_Amizade == null)
            {
                return NotFound();
            }

            var destinatario_Pedidos_Amizade = await _context.Destinatario_Pedidos_Amizade
                .Include(d => d.RemetentePedido)
                .FirstOrDefaultAsync(m => m.IDPedido == id);
            if (destinatario_Pedidos_Amizade == null)
            {
                return NotFound();
            }

            return View(destinatario_Pedidos_Amizade);
        }

        // GET: Destinatario_Pedidos_Amizade/Create
        public IActionResult Create()
        {
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: Destinatario_Pedidos_Amizade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDPedido,IDDestinatarioPedido,DataHoraPedido,EstadoPedido,RemetenteFK")] Destinatario_Pedidos_Amizade destinatario_Pedidos_Amizade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(destinatario_Pedidos_Amizade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", destinatario_Pedidos_Amizade.RemetenteFK);
            return View(destinatario_Pedidos_Amizade);
        }

        // GET: Destinatario_Pedidos_Amizade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Destinatario_Pedidos_Amizade == null)
            {
                return NotFound();
            }

            var destinatario_Pedidos_Amizade = await _context.Destinatario_Pedidos_Amizade.FindAsync(id);
            if (destinatario_Pedidos_Amizade == null)
            {
                return NotFound();
            }
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", destinatario_Pedidos_Amizade.RemetenteFK);
            return View(destinatario_Pedidos_Amizade);
        }

        // POST: Destinatario_Pedidos_Amizade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDPedido,IDDestinatarioPedido,DataHoraPedido,EstadoPedido,RemetenteFK")] Destinatario_Pedidos_Amizade destinatario_Pedidos_Amizade)
        {
            if (id != destinatario_Pedidos_Amizade.IDPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destinatario_Pedidos_Amizade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Destinatario_Pedidos_AmizadeExists(destinatario_Pedidos_Amizade.RemetenteFK))
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
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", destinatario_Pedidos_Amizade.RemetenteFK);
            return View(destinatario_Pedidos_Amizade);
        }

        // GET: Destinatario_Pedidos_Amizade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Destinatario_Pedidos_Amizade == null)
            {
                return NotFound();
            }

            var destinatario_Pedidos_Amizade = await _context.Destinatario_Pedidos_Amizade
                .Include(d => d.RemetentePedido)
                .FirstOrDefaultAsync(m => m.IDPedido == id);
            if (destinatario_Pedidos_Amizade == null)
            {
                return NotFound();
            }

            return View(destinatario_Pedidos_Amizade);
        }

        // POST: Destinatario_Pedidos_Amizade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Destinatario_Pedidos_Amizade == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Destinatario_Pedidos_Amizade'  is null.");
            }
            var destinatario_Pedidos_Amizade = await _context.Destinatario_Pedidos_Amizade.FindAsync(id);
            if (destinatario_Pedidos_Amizade != null)
            {
                _context.Destinatario_Pedidos_Amizade.Remove(destinatario_Pedidos_Amizade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Destinatario_Pedidos_AmizadeExists(int id)
        {
            return _context.Destinatario_Pedidos_Amizade.Any(e => e.IDPedido == id);
        }
    }
}
