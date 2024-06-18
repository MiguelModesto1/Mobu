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

namespace mobu_backend.Controllers.Backend
{
    /// <summary>
    /// Controlador das amizades
    /// </summary>
    [Authorize(Roles = "Moderador")]
    public class AmizadeController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controlador de Amizades
        /// </summary>
        /// <param name="context">O contexto da base de dados.</param>
        public AmizadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter vista de Índice
        /// </summary>
        /// <returns></returns>
        // GET: Amizade
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Amizade
                .Include(a => a.Destinatario)
                .Include(a => a.Remetente);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Obter vista de detalhes
        /// </summary>
        /// <param name="remetenteId">O ID do remetente</param>
        /// <param name="destinatarioId">O ID do destinatário</param>
        /// <returns></returns>
        // GET: Amizade/Details/5
        public async Task<IActionResult> Details(int? remetenteId, int? destinatarioId)
        {
            if (remetenteId == null || destinatarioId == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade
                .Include(a => a.Destinatario)
                .Include(a => a.Remetente)
                .FirstOrDefaultAsync(m => m.DestinatarioFK == destinatarioId && m.RemetenteFK == remetenteId);
            if (amizade == null)
            {
                return NotFound();
            }

            return View(amizade);
        }

        /// <summary>
        /// Obter vista de criação
        /// </summary>
        /// <returns></returns>
        // GET: Amizade/Create
        public IActionResult Create()
        {
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        /// <summary>
        /// Publicar criação
        /// </summary>
        /// <param name="amizade">O modelo de amizade a ser criado.</param>
        /// <returns></returns>
        // POST: Amizade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataPedido,DataResposta,Desbloqueado,DestinatarioFK,RemetenteFK")] Amizade amizade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(amizade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DestinatarioFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.RemetenteFK);
            return View(amizade);
        }

        /// <summary>
        /// Obter vista de edição
        /// </summary>
        /// <param name="remetenteId">O ID do remetente</param>
        /// <param name="destinatarioId">O ID do destinatário</param>
        /// <returns></returns>
        // GET: Amizade/Edit/5
        public async Task<IActionResult> Edit(int? remetenteId, int? destinatarioId)
        {
            if (remetenteId == null || destinatarioId == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade.FindAsync(remetenteId, destinatarioId);
            if (amizade == null)
            {
                return NotFound();
            }
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DestinatarioFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.RemetenteFK);
            return View(amizade);
        }

        /// <summary>
        /// Publicar edição
        /// </summary>
        /// <param name="remetenteId">O ID do remetente</param>
        /// <param name="destinatarioId">O ID do destinatário</param>
        /// <param name="amizade">O modelo de amizade a ser editado</param>
        /// <returns></returns>
        // POST: Amizade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int remetenteId, int destinatarioId, [Bind("DataPedido,DataResposta,Desbloqueado,DestinatarioFK,RemetenteFK")] Amizade amizade)
        {
            if (remetenteId != amizade.RemetenteFK || destinatarioId != amizade.DestinatarioFK)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amizade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmizadeExists(amizade.RemetenteFK, amizade.DestinatarioFK))
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
            ViewData["DestinatarioFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.DestinatarioFK);
            ViewData["RemetenteFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", amizade.RemetenteFK);
            return View(amizade);
        }

        /// <summary>
        /// Obter vista de remoção
        /// </summary>
        /// <param name="remetenteId">O ID do remetente</param>
        /// <param name="destinatarioId">O ID do destinatário</param>
        /// <returns>Uma vista com o formulário de exclusão de amizade</returns>
        // GET: Amizade/Delete/5
        public async Task<IActionResult> Delete(int? remetenteId, int? destinatarioId)
        {
            if (remetenteId == null || destinatarioId == null)
            {
                return NotFound();
            }

            var amizade = await _context.Amizade
                .Include(a => a.Destinatario)
                .Include(a => a.Remetente)
                .FirstOrDefaultAsync(m => m.RemetenteFK == remetenteId && m.DestinatarioFK == destinatarioId);
            if (amizade == null)
            {
                return NotFound();
            }

            return View(amizade);
        }

        /// <summary>
        /// Publicar remoção
        /// </summary>
        /// <param name="remetenteId">O ID do remetente</param>
        /// <param name="destinatarioId">O ID do destinatário</param>
        /// <returns>Uma tarefa que representa a operação assíncrona</returns>
        // POST: Amizade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int remetenteId, int destinatarioId)
        {
            var amizade = await _context.Amizade.FindAsync(remetenteId, destinatarioId);
            if (amizade != null)
            {
                _context.Amizade.Remove(amizade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se uma amizade existe
        /// </summary>
        /// <param name="remetenteId">O ID do remetente</param>
        /// <param name="destinatarioId">O ID do destinatário</param>
        /// <returns>True se a amizade existir, caso contrário, False</returns>
        private bool AmizadeExists(int remetenteId, int destinatarioId)
        {
            return _context.Amizade.Any(e => e.RemetenteFK == remetenteId && e.DestinatarioFK == destinatarioId);
        }
    }
}
