using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    public class Registados_Salas_JogoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Registados_Salas_JogoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registados_Salas_Jogo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Registados_Salas_Jogo.Include(r => r.Sala).Include(r => r.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Registados_Salas_Jogo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Registados_Salas_Jogo == null)
            {
                return NotFound();
            }

            var registados_Salas_Jogo = await _context.Registados_Salas_Jogo
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registados_Salas_Jogo == null)
            {
                return NotFound();
            }

            return View(registados_Salas_Jogo);
        }

        // GET: Registados_Salas_Jogo/Create
        public IActionResult Create()
        {
            ViewData["SalaFK"] = new SelectList(_context.Sala_Jogo_1_Contra_1, "IDSala", "IDSala");
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "Email");
            return View();
        }

        // POST: Registados_Salas_Jogo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDRegisto,IsFundador,UtilizadorFK,SalaFK")] Registados_Salas_Jogo registados_Salas_Jogo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registados_Salas_Jogo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaFK"] = new SelectList(_context.Sala_Jogo_1_Contra_1, "IDSala", "IDSala", registados_Salas_Jogo.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "Email", registados_Salas_Jogo.UtilizadorFK);
            return View(registados_Salas_Jogo);
        }

        // GET: Registados_Salas_Jogo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registados_Salas_Jogo == null)
            {
                return NotFound();
            }

            var registados_Salas_Jogo = await _context.Registados_Salas_Jogo.FindAsync(id);
            if (registados_Salas_Jogo == null)
            {
                return NotFound();
            }
            ViewData["SalaFK"] = new SelectList(_context.Sala_Jogo_1_Contra_1, "IDSala", "IDSala", registados_Salas_Jogo.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "Email", registados_Salas_Jogo.UtilizadorFK);
            return View(registados_Salas_Jogo);
        }

        // POST: Registados_Salas_Jogo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDRegisto,IsFundador,UtilizadorFK,SalaFK")] Registados_Salas_Jogo registados_Salas_Jogo)
        {
            if (id != registados_Salas_Jogo.IDRegisto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registados_Salas_Jogo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Registados_Salas_JogoExists(registados_Salas_Jogo.IDRegisto))
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
            ViewData["SalaFK"] = new SelectList(_context.Sala_Jogo_1_Contra_1, "IDSala", "IDSala", registados_Salas_Jogo.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "Email", registados_Salas_Jogo.UtilizadorFK);
            return View(registados_Salas_Jogo);
        }

        // GET: Registados_Salas_Jogo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registados_Salas_Jogo == null)
            {
                return NotFound();
            }

            var registados_Salas_Jogo = await _context.Registados_Salas_Jogo
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registados_Salas_Jogo == null)
            {
                return NotFound();
            }

            return View(registados_Salas_Jogo);
        }

        // POST: Registados_Salas_Jogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registados_Salas_Jogo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Registados_Salas_Jogo'  is null.");
            }
            var registados_Salas_Jogo = await _context.Registados_Salas_Jogo.FindAsync(id);
            if (registados_Salas_Jogo != null)
            {
                _context.Registados_Salas_Jogo.Remove(registados_Salas_Jogo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Registados_Salas_JogoExists(int id)
        {
            return _context.Registados_Salas_Jogo.Any(e => e.IDRegisto == id);
        }
    }
}
