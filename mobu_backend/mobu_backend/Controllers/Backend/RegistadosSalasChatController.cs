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
    public class RegistadosSalasChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistadosSalasChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RegistadosSalasChat
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RegistadosSalasChat.Include(r => r.Sala).Include(r => r.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RegistadosSalasChat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RegistadosSalasChat == null)
            {
                return NotFound();
            }

            var registadosSalasChat = await _context.RegistadosSalasChat
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registadosSalasChat == null)
            {
                return NotFound();
            }

            return View(registadosSalasChat);
        }

        // GET: RegistadosSalasChat/Create
        public IActionResult Create()
        {
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "IDSala");
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        // POST: RegistadosSalasChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDRegisto,IsAdmin,UtilizadorFK,SalaFK")] RegistadosSalasChat registadosSalasChat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registadosSalasChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "IDSala", registadosSalasChat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasChat.UtilizadorFK);
            return View(registadosSalasChat);
        }

        // GET: RegistadosSalasChat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RegistadosSalasChat == null)
            {
                return NotFound();
            }

            var registadosSalasChat = await _context.RegistadosSalasChat.FindAsync(id);
            if (registadosSalasChat == null)
            {
                return NotFound();
            }
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "IDSala", registadosSalasChat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasChat.UtilizadorFK);
            return View(registadosSalasChat);
        }

        // POST: RegistadosSalasChat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDRegisto,IsAdmin,UtilizadorFK,SalaFK")] RegistadosSalasChat registadosSalasChat)
        {
            if (id != registadosSalasChat.IDRegisto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registadosSalasChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistadosSalasChatExists(registadosSalasChat.IDRegisto))
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
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "IDSala", registadosSalasChat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasChat.UtilizadorFK);
            return View(registadosSalasChat);
        }

        // GET: RegistadosSalasChat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RegistadosSalasChat == null)
            {
                return NotFound();
            }

            var registadosSalasChat = await _context.RegistadosSalasChat
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.IDRegisto == id);
            if (registadosSalasChat == null)
            {
                return NotFound();
            }

            return View(registadosSalasChat);
        }

        // POST: RegistadosSalasChat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RegistadosSalasChat == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RegistadosSalasChat'  is null.");
            }
            var registadosSalasChat = await _context.RegistadosSalasChat.FindAsync(id);
            if (registadosSalasChat != null)
            {
                _context.RegistadosSalasChat.Remove(registadosSalasChat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistadosSalasChatExists(int id)
        {
            return _context.RegistadosSalasChat.Any(e => e.IDRegisto == id);
        }
    }
}
