using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    [Authorize(Roles = "Administrator")]
    public class UtilizadorAnonimoController : Controller
    {
        /// <summary>
        /// objeto que referencia a Base de Dados do projeto
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// ferramenta com acesso aos dados da pessoa autenticada
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Este recurso (tecnicamente, um atributo) mostra os 
        /// dados do servidor. 
        /// E necessário inicializar este atributo no construtor da classe
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UtilizadorAnonimoController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: UtilizadorAnonimo
        public async Task<IActionResult> Index()
        {
            // Consulta que inclui dados sobre a fotografia
            // do user
            var utilizadores = _context.UtilizadorAnonimo;

            //voltar a lista
            return View(await utilizadores.ToListAsync());
        }

        // GET: UtilizadorAnonimo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Retorna o codigo de erro 404 se o id ou o user
            // nao existir ou for nulo
            if (id == null || _context.UtilizadorAnonimo == null)
            {
                return NotFound();
            }

            // Consulta que retorna todos os detalhes do
            // administrador com IDUtilizador = id
            var utilizadorAnonimo = await _context.UtilizadorAnonimo
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizadorAnonimo == null)
            {
                return NotFound();
            }

            return View(utilizadorAnonimo);
        }

        // GET: UtilizadorAnonimo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UtilizadorAnonimo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //,EnderecoIPv4,EnderecoIPv6
        public async Task<IActionResult> Create([Bind("IDUtilizador")] UtilizadorAnonimo utilizadorAnonimo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // adicionar dados do utilizador anonimo
                    // a BD
                    _context.Attach(utilizadorAnonimo);

                    // realizar commit
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do utilizador anónimo" + utilizadorAnonimo.IDUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizadorAnonimo);
        }

        // GET: UtilizadorAnonimo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UtilizadorAnonimo == null)
            {
                return NotFound();
            }

            var utilizadorAnonimo = await _context.UtilizadorAnonimo
                .FirstOrDefaultAsync(a => a.IDUtilizador == id);
            if (utilizadorAnonimo == null)
            {
                return NotFound();
            }
            return View(utilizadorAnonimo);
        }

        // POST: UtilizadorAnonimo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //,EnderecoIPv4,EnderecoIPv6
        public async Task<IActionResult> Edit(int id, [Bind("IDUtilizador")] UtilizadorAnonimo utilizadorAnonimo)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    // editar dados do utilizador anonimo
                    // na BD
                    _context.Update(utilizadorAnonimo);

                    // realizar commit
                    await _context.SaveChangesAsync();

                    // voltar a lista
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados do utilizador anónimo " + utilizadorAnonimo.IDUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizadorAnonimo);
        }

        // GET: UtilizadorAnonimo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UtilizadorAnonimo == null)
            {
                return NotFound();
            }

            var utilizadorAnonimo = await _context.UtilizadorAnonimo
                .FindAsync(id);
            if (utilizadorAnonimo == null)
            {
                return NotFound();
            }

            return View(utilizadorAnonimo);
        }

        // POST: UtilizadorAnonimo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UtilizadorAnonimo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UtilizadorAnonimo'  is null.");
            }
            var utilizadorAnonimo = await _context.UtilizadorAnonimo.FindAsync(id);
            if (utilizadorAnonimo != null)
            {
                _context.UtilizadorAnonimo.Remove(utilizadorAnonimo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(utilizadorAnonimo);
        }

        private bool UtilizadorAnonimoExists(int id)
        {
            return (_context.UtilizadorAnonimo?.Any(e => e.IDUtilizador == id)).GetValueOrDefault();
        }
    }
}
