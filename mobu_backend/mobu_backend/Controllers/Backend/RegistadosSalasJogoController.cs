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
    public class RegistadosSalasJogoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistadosSalasJogoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RegistadosSalasJogo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RegistadosSalasJogo.Include(r => r.Sala).Include(r => r.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RegistadosSalasJogo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RegistadosSalasJogo == null)
            {
                return NotFound();
            }

            var registadosSalasJogo = await _context.RegistadosSalasJogo
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registadosSalasJogo == null)
            {
                return NotFound();
            }

            return View(registadosSalasJogo);
        }

        // GET: RegistadosSalasJogo/Create
        public IActionResult Create()
        {
            ViewData["SalaFK"] = new SelectList(_context.SalaJogo1Contra1, "IDSala", "IDSala");
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: RegistadosSalasJogo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDRegisto,IsFundador,Pontos,UtilizadorFK,SalaFK")] RegistadosSalasJogo registadosSalasJogo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registadosSalasJogo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaFK"] = new SelectList(_context.SalaJogo1Contra1, "IDSala", "IDSala", registadosSalasJogo.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasJogo.UtilizadorFK);
            return View(registadosSalasJogo);
        }

        // GET: RegistadosSalasJogo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RegistadosSalasJogo == null)
            {
                return NotFound();
            }

            var registadosSalasJogo = await _context.RegistadosSalasJogo.FindAsync(id);
            if (registadosSalasJogo == null)
            {
                return NotFound();
            }
            ViewData["SalaFK"] = new SelectList(_context.SalaJogo1Contra1, "IDSala", "IDSala", registadosSalasJogo.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasJogo.UtilizadorFK);
            return View(registadosSalasJogo);
        }

        // POST: RegistadosSalasJogo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDRegisto,IsFundador,Pontos,UtilizadorFK,SalaFK")] RegistadosSalasJogo registadosSalasJogo)
        {
            if (id != registadosSalasJogo.IDRegisto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registadosSalasJogo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistadosSalasJogoExists(registadosSalasJogo.IDRegisto))
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
            ViewData["SalaFK"] = new SelectList(_context.SalaJogo1Contra1, "IDSala", "IDSala", registadosSalasJogo.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasJogo.UtilizadorFK);
            return View(registadosSalasJogo);
        }

        // GET: RegistadosSalasJogo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RegistadosSalasJogo == null)
            {
                return NotFound();
            }

            var registadosSalasJogo = await _context.RegistadosSalasJogo
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registadosSalasJogo == null)
            {
                return NotFound();
            }

            return View(registadosSalasJogo);
        }

        // POST: RegistadosSalasJogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RegistadosSalasJogo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RegistadosSalasJogo'  is null.");
            }
            var registadosSalasJogo = await _context.RegistadosSalasJogo.FindAsync(id);
            if (registadosSalasJogo != null)
            {
                _context.RegistadosSalasJogo.Remove(registadosSalasJogo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistadosSalasJogoExists(int id)
        {
            return _context.RegistadosSalasJogo.Any(e => e.IDRegisto == id);
        }
    }
}
