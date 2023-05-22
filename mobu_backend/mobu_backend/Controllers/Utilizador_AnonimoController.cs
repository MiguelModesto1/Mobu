using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Controllers
{
    public class Utilizador_AnonimoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Utilizador_AnonimoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utilizador_Anonimo
        public async Task<IActionResult> Index()
        {
              return _context.Utilizador_Anonimo != null ? 
                          View(await _context.Utilizador_Anonimo.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Utilizador_Anonimo'  is null.");
        }

        // GET: Utilizador_Anonimo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Utilizador_Anonimo == null)
            {
                return NotFound();
            }

            var utilizador_Anonimo = await _context.Utilizador_Anonimo
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizador_Anonimo == null)
            {
                return NotFound();
            }

            return View(utilizador_Anonimo);
        }

        // GET: Utilizador_Anonimo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizador_Anonimo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDUtilizador,NomeUtilizador,EnderecoIPv4,EnderecoIPv6")] Utilizador_Anonimo utilizador_Anonimo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador_Anonimo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador_Anonimo);
        }

        // GET: Utilizador_Anonimo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utilizador_Anonimo == null)
            {
                return NotFound();
            }

            var utilizador_Anonimo = await _context.Utilizador_Anonimo.FindAsync(id);
            if (utilizador_Anonimo == null)
            {
                return NotFound();
            }
            return View(utilizador_Anonimo);
        }

        // POST: Utilizador_Anonimo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDUtilizador,NomeUtilizador,EnderecoIPv4,EnderecoIPv6")] Utilizador_Anonimo utilizador_Anonimo)
        {
            if (id != utilizador_Anonimo.IDUtilizador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador_Anonimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Utilizador_AnonimoExists(utilizador_Anonimo.IDUtilizador))
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
            return View(utilizador_Anonimo);
        }

        // GET: Utilizador_Anonimo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utilizador_Anonimo == null)
            {
                return NotFound();
            }

            var utilizador_Anonimo = await _context.Utilizador_Anonimo
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizador_Anonimo == null)
            {
                return NotFound();
            }

            return View(utilizador_Anonimo);
        }

        // POST: Utilizador_Anonimo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utilizador_Anonimo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Utilizador_Anonimo'  is null.");
            }
            var utilizador_Anonimo = await _context.Utilizador_Anonimo.FindAsync(id);
            if (utilizador_Anonimo != null)
            {
                _context.Utilizador_Anonimo.Remove(utilizador_Anonimo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Utilizador_AnonimoExists(int id)
        {
          return (_context.Utilizador_Anonimo?.Any(e => e.IDUtilizador == id)).GetValueOrDefault();
        }
    }
}
