using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace mobu_backend.Controllers
{
    public class MensagemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MensagemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mensagem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.mensagem.Include(m => m.Remetente).Include(m => m.Sala);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Mensagem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.mensagem
                .Include(m => m.Remetente)
                .Include(m => m.Sala)
                .FirstOrDefaultAsync(m => m.IDMensagem == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        // GET: Mensagem/Create
        public IActionResult Create()
        {
            ViewBag.RemetenteFK = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador");
            ViewData["salaFK"] = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala");
            return View();
        }

        // POST: Mensagem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDMensagem,ConteudoMsg,EstadoMensagem,RemetenteFK,SalaFK")] Mensagem mensagem)
        {

            if (ModelState.IsValid)
            {
                _context.Add(mensagem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                AddErrorsFromModel(ModelState.Values);
                return View(mensagem);
            }
                ViewBag.RemetenteFK = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
                ViewBag.SalaFK = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala", mensagem.SalaFK);
                return View(mensagem);
        }

        private void AddErrorsFromModel(ModelStateDictionary.ValueEnumerable values)
        {
            foreach (ModelStateEntry modelState in values)
                foreach (Microsoft.AspNetCore.Mvc.ModelBinding.ModelError error in modelState.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage.ToString());
                }
        }

        // GET: Mensagem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.mensagem.FindAsync(id);
            if (mensagem == null)
            {
                return NotFound();
            }
            ViewBag.RemetenteFK = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewBag.SalaFK = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        // POST: Mensagem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDMensagem,ConteudoMsg,EstadoMensagem,RemetenteFK,SalaFK")] Mensagem mensagem)
        {
            if (id != mensagem.IDMensagem)
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
                    if (!MensagemExists(mensagem.IDMensagem))
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
            ViewBag.RemetenteFK = new SelectList(_context.Utilizador_Registado, "IDUtilizador", "NomeUtilizador", mensagem.RemetenteFK);
            ViewBag.SalaFK = new SelectList(_context.Salas_Chat, "IDSala", "NomeSala", mensagem.SalaFK);
            return View(mensagem);
        }

        // GET: Mensagem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.mensagem == null)
            {
                return NotFound();
            }

            var mensagem = await _context.mensagem
                .Include(m => m.Remetente)
                .Include(m => m.Sala)
                .FirstOrDefaultAsync(m => m.IDMensagem == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        // POST: Mensagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.mensagem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.mensagem'  is null.");
            }
            var mensagem = await _context.mensagem.FindAsync(id);
            if (mensagem != null)
            {
                _context.mensagem.Remove(mensagem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MensagemExists(int id)
        {
          return (_context.mensagem?.Any(e => e.IDMensagem == id)).GetValueOrDefault();
        }
    }
}
