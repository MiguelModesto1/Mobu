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
    public class AmizadeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AmizadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Amizade
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Amizade
                .Include(a => a.Destinatario)
                .Include(a => a.Remetente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Amizade/Details/5
        public async Task<IActionResult> Details(int? remetenteId, int? destinatarioId)
        {
            if (remetenteId == null || destinatarioId == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade
                .Include(a => a.Destinatario)
                .Include(a => a.Remetente)
                .FirstOrDefaultAsync(m => m.DestinatarioFK == destinatarioId && m.RemetenteFK == remetenteId);
            if (amizade == null)
            {
                return NotFound();
            }

            return View(amizade);
        }

        // GET: Amizade/Create
        public IActionResult Create()
        {
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: Amizade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataPedido,DataResposta,Desbloqueado,DestinatarioFK,RemetenteFK")] Amizade amizade)
        {
            if (ModelState.IsValid)
            {

                _context.Add(amizade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DestinatarioFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.RemetenteFK);
            return View(amizade);
        }

        // GET: Amizade/Edit/5
        public async Task<IActionResult> Edit(int? remetenteId, int? destinatarioId)
        {
            if (remetenteId == null || destinatarioId == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade.FindAsync(remetenteId, destinatarioId);
            if (amizade == null)
            {
                return NotFound();
            }
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DestinatarioFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.RemetenteFK);
            return View(amizade);
        }

        // POST: Amizade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int remetenteId, int destinatarioId, [Bind("DataPedido,DataResposta,Desbloqueado,DestinatarioFK,RemetenteFK")] Amizade amizade)
        {
            if (remetenteId != amizade.DestinatarioFK || destinatarioId != amizade.DestinatarioFK)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amizade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmizadeExists(amizade.RemetenteFK, amizade.DestinatarioFK))
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
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DestinatarioFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.RemetenteFK);
            return View(amizade);
        }

        // GET: Amizade/Delete/5
        public async Task<IActionResult> Delete(int? remetenteId, int? destinatarioId)
        {
            if (remetenteId == null || destinatarioId == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade
                .Include(a => a.Destinatario)
                .Include(a => a.Remetente)
                .FirstOrDefaultAsync(m => m.RemetenteFK == remetenteId && m.DestinatarioFK == destinatarioId);
            if (amizade == null)
            {
                return NotFound();
            }

            return View(amizade);
        }

        // POST: Amizade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int remetenteId, int destinatarioId)
        {
            var amizade = await _context.Amizade.FindAsync(remetenteId, destinatarioId);
            if (amizade != null)
            {
                _context.Amizade.Remove(amizade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmizadeExists(int remetenteId, int destinatarioId)
        {
            return _context.Amizade.Any(e => e.RemetenteFK == remetenteId && e.DestinatarioFK == destinatarioId);
        }
    }
}
