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
    public class Registados_Salas_ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Registados_Salas_ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registados_Salas_Chat
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Registados_Salas_Chat.Include(r => r.Sala).Include(r => r.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Registados_Salas_Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Registados_Salas_Chat == null)
            {
                return NotFound();
            }

            var registados_Salas_Chat = await _context.Registados_Salas_Chat
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registados_Salas_Chat == null)
            {
                return NotFound();
            }

            return View(registados_Salas_Chat);
        }

        // GET: Registados_Salas_Chat/Create
        public IActionResult Create()
        {
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "IDSala");
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: Registados_Salas_Chat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDRegisto,IsAdmin,UtilizadorFK,SalaFK")] Registados_Salas_Chat registados_Salas_Chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registados_Salas_Chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "IDSala", registados_Salas_Chat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", registados_Salas_Chat.UtilizadorFK);
            return View(registados_Salas_Chat);
        }

        // GET: Registados_Salas_Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registados_Salas_Chat == null)
            {
                return NotFound();
            }

            var registados_Salas_Chat = await _context.Registados_Salas_Chat.FindAsync(id);
            if (registados_Salas_Chat == null)
            {
                return NotFound();
            }
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "IDSala", registados_Salas_Chat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", registados_Salas_Chat.UtilizadorFK);
            return View(registados_Salas_Chat);
        }

        // POST: Registados_Salas_Chat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDRegisto,IsAdmin,UtilizadorFK,SalaFK")] Registados_Salas_Chat registados_Salas_Chat)
        {
            if (id != registados_Salas_Chat.IDRegisto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registados_Salas_Chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Registados_Salas_ChatExists(registados_Salas_Chat.IDRegisto))
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
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "IDSala", registados_Salas_Chat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", registados_Salas_Chat.UtilizadorFK);
            return View(registados_Salas_Chat);
        }

        // GET: Registados_Salas_Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registados_Salas_Chat == null)
            {
                return NotFound();
            }

            var registados_Salas_Chat = await _context.Registados_Salas_Chat
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registados_Salas_Chat == null)
            {
                return NotFound();
            }

            return View(registados_Salas_Chat);
        }

        // POST: Registados_Salas_Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registados_Salas_Chat == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Registados_Salas_Chat'  is null.");
            }
            var registados_Salas_Chat = await _context.Registados_Salas_Chat.FindAsync(id);
            if (registados_Salas_Chat != null)
            {
                _context.Registados_Salas_Chat.Remove(registados_Salas_Chat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Registados_Salas_ChatExists(int id)
        {
            return _context.Registados_Salas_Chat.Any(e => e.IDRegisto == id);
        }
    }
}
