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

namespace mobu_backend.Controllers.Backend
{
    [Authorize]
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
            var applicationDbContext = _context.Amizade.Include(a => a.Amigo).Include(a => a.DonoListaAmigos);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Amizade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade
                .Include(a => a.Amigo)
                .Include(a => a.DonoListaAmigos)
                .FirstOrDefaultAsync(m => m.IdAmizade == id);
            if (amizade == null)
            {
                return NotFound();
            }

            return View(amizade);
        }

        // GET: Amizade/Create
        public IActionResult Create()
        {
            ViewData["AmigoFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            ViewData["DonoListaAmigosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: Amizade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAmizade,DonoListaAmigosFK,AmigoFK")] Amizade amizade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(amizade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AmigoFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.AmigoFK);
            ViewData["DonoListaAmigosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DonoListaAmigosFK);
            return View(amizade);
        }

        // GET: Amizade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade.FindAsync(id);
            if (amizade == null)
            {
                return NotFound();
            }
            ViewData["AmigoFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.AmigoFK);
            ViewData["DonoListaAmigosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DonoListaAmigosFK);
            return View(amizade);
        }

        // POST: Amizade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAmizade,DonoListaAmigosFK,AmigoFK")] Amizade amizade)
        {
            if (id != amizade.IdAmizade)
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
                    if (!AmizadeExists(amizade.IdAmizade))
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
            ViewData["AmigoFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.AmigoFK);
            ViewData["DonoListaAmigosFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DonoListaAmigosFK);
            return View(amizade);
        }

        // GET: Amizade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade
                .Include(a => a.Amigo)
                .Include(a => a.DonoListaAmigos)
                .FirstOrDefaultAsync(m => m.IdAmizade == id);
            if (amizade == null)
            {
                return NotFound();
            }

            return View(amizade);
        }

        // POST: Amizade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var amizade = await _context.Amizade.FindAsync(id);
            if (amizade != null)
            {
                _context.Amizade.Remove(amizade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmizadeExists(int id)
        {
            return _context.Amizade.Any(e => e.IdAmizade == id);
        }
    }
}
