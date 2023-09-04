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
    public class MensagemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MensagemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mensagems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mensagem.Include(m => m.Remetente).Include(m => m.Sala);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Mensagems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagem
                .Include(m => m.Remetente)
                .Include(m => m.Sala)
                .FirstOrDefaultAsync(m => m.IDMensagemSala == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        // GET: Mensagems/Create
        public IActionResult Create()
        {
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador");
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala");
            return View();
        }

        // POST: Mensagems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDMensagemSala,IDMensagem,ConteudoMsg,DataHoraMsg,EstadoMensagem,RemetenteFK,SalaFK")] Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mensagem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        // GET: Mensagems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagem.FindAsync(id);
            if (mensagem == null)
            {
                return NotFound();
            }
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        // POST: Mensagems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDMensagemSala,IDMensagem,ConteudoMsg,DataHoraMsg,EstadoMensagem,RemetenteFK,SalaFK")] Mensagem mensagem)
        {
            if (id != mensagem.IDMensagemSala)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mensagem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MensagemExists(mensagem.IDMensagemSala))
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
            ViewData["RemetenteFK"] = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewData["SalaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        // GET: Mensagems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagem
                .Include(m => m.Remetente)
                .Include(m => m.Sala)
                .FirstOrDefaultAsync(m => m.IDMensagemSala == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        // POST: Mensagems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mensagem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Mensagem'  is null.");
            }
            var mensagem = await _context.Mensagem.FindAsync(id);
            if (mensagem != null)
            {
                _context.Mensagem.Remove(mensagem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MensagemExists(int id)
        {
            return _context.Mensagem.Any(e => e.IDMensagemSala == id);
        }
    }
}
