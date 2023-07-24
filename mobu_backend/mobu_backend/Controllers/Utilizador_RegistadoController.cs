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
    public class Utilizador_RegistadoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Utilizador_RegistadoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utilizador_Registado
        public async Task<IActionResult> Index()
        {
              return View(await _context.Utilizador_Registado.ToListAsync());
        }

        // GET: Utilizador_Registado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Utilizador_Registado == null)
            {
                return NotFound();
            }

            var utilizador_Registado = await _context.Utilizador_Registado
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizador_Registado == null)
            {
                return NotFound();
            }

            return View(utilizador_Registado);
        }

        // GET: Utilizador_Registado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizador_Registado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDUtilizador,NomeUtilizador,Email,Password,Fotografia")] Utilizador_Registado utilizador_Registado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador_Registado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador_Registado);
        }

        // GET: Utilizador_Registado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utilizador_Registado == null)
            {
                return NotFound();
            }

            var utilizador_Registado = await _context.Utilizador_Registado.FindAsync(id);
            if (utilizador_Registado == null)
            {
                return NotFound();
            }
            return View(utilizador_Registado);
        }

        // POST: Utilizador_Registado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDUtilizador,NomeUtilizador,Email,Password,Fotografia")] Utilizador_Registado utilizador_Registado)
        {
            if (id != utilizador_Registado.IDUtilizador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador_Registado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Utilizador_RegistadoExists(utilizador_Registado.IDUtilizador))
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
            return View(utilizador_Registado);
        }

        // GET: Utilizador_Registado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utilizador_Registado == null)
            {
                return NotFound();
            }

            var utilizador_Registado = await _context.Utilizador_Registado
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizador_Registado == null)
            {
                return NotFound();
            }

            return View(utilizador_Registado);
        }

        // POST: Utilizador_Registado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utilizador_Registado == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Utilizador_Registado'  is null.");
            }
            var utilizador_Registado = await _context.Utilizador_Registado.FindAsync(id);
            if (utilizador_Registado != null)
            {
                _context.Utilizador_Registado.Remove(utilizador_Registado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Utilizador_RegistadoExists(int id)
        {
          return _context.Utilizador_Registado.Any(e => e.IDUtilizador == id);
        }
    }
}
