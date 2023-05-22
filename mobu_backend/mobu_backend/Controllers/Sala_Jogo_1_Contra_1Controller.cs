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
    public class Sala_Jogo_1_Contra_1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Sala_Jogo_1_Contra_1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sala_Jogo_1_Contra_1
        public async Task<IActionResult> Index()
        {
              return _context.Sala_Jogo_1_Contra_1 != null ? 
                          View(await _context.Sala_Jogo_1_Contra_1.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Sala_Jogo_1_Contra_1'  is null.");
        }

        // GET: Sala_Jogo_1_Contra_1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sala_Jogo_1_Contra_1 == null)
            {
                return NotFound();
            }

            var sala_Jogo_1_Contra_1 = await _context.Sala_Jogo_1_Contra_1
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (sala_Jogo_1_Contra_1 == null)
            {
                return NotFound();
            }

            return View(sala_Jogo_1_Contra_1);
        }

        // GET: Sala_Jogo_1_Contra_1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sala_Jogo_1_Contra_1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDSala")] Sala_Jogo_1_Contra_1 sala_Jogo_1_Contra_1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sala_Jogo_1_Contra_1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sala_Jogo_1_Contra_1);
        }

        // GET: Sala_Jogo_1_Contra_1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sala_Jogo_1_Contra_1 == null)
            {
                return NotFound();
            }

            var sala_Jogo_1_Contra_1 = await _context.Sala_Jogo_1_Contra_1.FindAsync(id);
            if (sala_Jogo_1_Contra_1 == null)
            {
                return NotFound();
            }
            return View(sala_Jogo_1_Contra_1);
        }

        // POST: Sala_Jogo_1_Contra_1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDSala")] Sala_Jogo_1_Contra_1 sala_Jogo_1_Contra_1)
        {
            if (id != sala_Jogo_1_Contra_1.IDSala)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sala_Jogo_1_Contra_1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Sala_Jogo_1_Contra_1Exists(sala_Jogo_1_Contra_1.IDSala))
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
            return View(sala_Jogo_1_Contra_1);
        }

        // GET: Sala_Jogo_1_Contra_1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sala_Jogo_1_Contra_1 == null)
            {
                return NotFound();
            }

            var sala_Jogo_1_Contra_1 = await _context.Sala_Jogo_1_Contra_1
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (sala_Jogo_1_Contra_1 == null)
            {
                return NotFound();
            }

            return View(sala_Jogo_1_Contra_1);
        }

        // POST: Sala_Jogo_1_Contra_1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sala_Jogo_1_Contra_1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sala_Jogo_1_Contra_1'  is null.");
            }
            var sala_Jogo_1_Contra_1 = await _context.Sala_Jogo_1_Contra_1.FindAsync(id);
            if (sala_Jogo_1_Contra_1 != null)
            {
                _context.Sala_Jogo_1_Contra_1.Remove(sala_Jogo_1_Contra_1);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Sala_Jogo_1_Contra_1Exists(int id)
        {
          return (_context.Sala_Jogo_1_Contra_1?.Any(e => e.IDSala == id)).GetValueOrDefault();
        }
    }
}
