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
    public class AmigoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AmigoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Amigo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Amigo.Include(a => a.DonoListaAmigos);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Amigo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Amigo == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigo
                .Include(a => a.DonoListaAmigos)
                .FirstOrDefaultAsync(m => m.IDAmizade == id);
            if (amigo == null)
            {
                return NotFound();
            }

            return View(amigo);
        }

        // GET: Amigo/Create
        public IActionResult Create()
        {
            ViewData["DonoListaFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: Amigo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDAmizade,IDAmigo,DonoListaFK")] Amigo amigo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(amigo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonoListaFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", amigo.DonoListaFK);
            return View(amigo);
        }

        // GET: Amigo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Amigo == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigo.FindAsync(id);
            if (amigo == null)
            {
                return NotFound();
            }
            ViewData["DonoListaFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", amigo.DonoListaFK);
            return View(amigo);
        }

        // POST: Amigo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDAmizade,IDAmigo,DonoListaFK")] Amigo amigo)
        {
            if (id != amigo.IDAmizade)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amigo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmigoExists(amigo.IDAmizade))
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
            ViewData["DonoListaFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", amigo.DonoListaFK);
            return View(amigo);
        }

        // GET: Amigo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Amigo == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigo
                .Include(a => a.DonoListaAmigos)
                .FirstOrDefaultAsync(m => m.IDAmizade == id);
            if (amigo == null)
            {
                return NotFound();
            }

            return View(amigo);
        }

        // POST: Amigo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Amigo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Amigo'  is null.");
            }
            var amigo = await _context.Amigo.FindAsync(id);
            if (amigo != null)
            {
                _context.Amigo.Remove(amigo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmigoExists(int id)
        {
            return _context.Amigo.Any(e => e.IDAmizade == id);
        }
    }
}
