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
    public class Salas_ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Salas_ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salas_Chat
        public async Task<IActionResult> Index()
        {
              return _context.Salas_Chat != null ? 
                          View(await _context.Salas_Chat.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Salas_Chat'  is null.");
        }

        // GET: Salas_Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Salas_Chat == null)
            {
                return NotFound();
            }

            var salas_Chat = await _context.Salas_Chat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salas_Chat == null)
            {
                return NotFound();
            }

            return View(salas_Chat);
        }

        // GET: Salas_Chat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salas_Chat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDSala,NomeSala,SeGrupo")] Salas_Chat salas_Chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salas_Chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salas_Chat);
        }

        // GET: Salas_Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Salas_Chat == null)
            {
                return NotFound();
            }

            var salas_Chat = await _context.Salas_Chat.FindAsync(id);
            if (salas_Chat == null)
            {
                return NotFound();
            }
            return View(salas_Chat);
        }

        // POST: Salas_Chat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDSala,NomeSala,SeGrupo")] Salas_Chat salas_Chat)
        {
            if (id != salas_Chat.IDSala)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salas_Chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Salas_ChatExists(salas_Chat.IDSala))
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
            return View(salas_Chat);
        }

        // GET: Salas_Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Salas_Chat == null)
            {
                return NotFound();
            }

            var salas_Chat = await _context.Salas_Chat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salas_Chat == null)
            {
                return NotFound();
            }

            return View(salas_Chat);
        }

        // POST: Salas_Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Salas_Chat == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Salas_Chat'  is null.");
            }
            var salas_Chat = await _context.Salas_Chat
                .FirstOrDefaultAsync(m => m.IDSala == id);
            if (salas_Chat != null)
            {
                _context.Salas_Chat.Remove(salas_Chat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Salas_ChatExists(int id)
        {
          return (_context.Salas_Chat?.Any(e => e.IDSala == id)).GetValueOrDefault();
        }
    }
}
