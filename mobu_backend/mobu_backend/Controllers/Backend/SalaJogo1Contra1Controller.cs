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
    public class SalaJogo1Contra1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalaJogo1Contra1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalaJogo1Contra1
        public async Task<IActionResult> Index()
        {
            return _context.SalaJogo1Contra1 != null ?
                        View(await _context.SalaJogo1Contra1.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.SalaJogo1Contra1'  is null.");
        }

        // GET: SalaJogo1Contra1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SalaJogo1Contra1 == null)
            {
                return NotFound();
            }

            var salaJogo1Contra1 = await _context.SalaJogo1Contra1
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salaJogo1Contra1 == null)
            {
                return NotFound();
            }

            return View(salaJogo1Contra1);
        }

        // GET: SalaJogo1Contra1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalaJogo1Contra1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDSala")] SalaJogo1Contra1 salaJogo1Contra1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salaJogo1Contra1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salaJogo1Contra1);
        }

        // GET: SalaJogo1Contra1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SalaJogo1Contra1 == null)
            {
                return NotFound();
            }

            var salaJogo1Contra1 = await _context.SalaJogo1Contra1.FindAsync(id);
            if (salaJogo1Contra1 == null)
            {
                return NotFound();
            }
            return View(salaJogo1Contra1);
        }

        // POST: SalaJogo1Contra1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDSala")] SalaJogo1Contra1 salaJogo1Contra1)
        {
            if (id != salaJogo1Contra1.IDSala)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaJogo1Contra1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaJogo1Contra1Exists(salaJogo1Contra1.IDSala))
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
            return View(salaJogo1Contra1);
        }

        // GET: SalaJogo1Contra1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SalaJogo1Contra1 == null)
            {
                return NotFound();
            }

            var salaJogo1Contra1 = await _context.SalaJogo1Contra1
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salaJogo1Contra1 == null)
            {
                return NotFound();
            }

            return View(salaJogo1Contra1);
        }

        // POST: SalaJogo1Contra1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SalaJogo1Contra1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SalaJogo1Contra1'  is null.");
            }
            var salaJogo1Contra1 = await _context.SalaJogo1Contra1
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salaJogo1Contra1 != null)
            {
                _context.SalaJogo1Contra1.Remove(salaJogo1Contra1);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaJogo1Contra1Exists(int id)
        {
            return (_context.SalaJogo1Contra1?.Any(e => e.IDSala == id)).GetValueOrDefault();
        }
    }
}
