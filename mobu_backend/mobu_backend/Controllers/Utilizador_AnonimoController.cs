using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Controllers
{
    public class Utilizador_AnonimoController : Controller
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

        public Utilizador_AnonimoController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Utilizador_Anonimo
        public async Task<IActionResult> Index()
        {
            // Consulta que inclui dados sobre a fotografia
            // do user (FALTA AUTENTICACAO)
            var utilizadores = _context.Utilizador_Anonimo;

            //voltar a lista
            return View(await utilizadores.ToListAsync());
        }

        // GET: Utilizador_Anonimo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Retorna o codigo de erro 404 se o id ou o user
            // nao existir ou for nulo
            if (id == null || _context.Utilizador_Anonimo == null)
            {
                return NotFound();
            }

            // Consulta que retorna todos os detalhes do
            // administrador com IDUtilizador = id
            var utilizador_Anonimo = await _context.Utilizador_Anonimo
                .FirstOrDefaultAsync(m => m.IDUtilizador == id);
            if (utilizador_Anonimo == null)
            {
                return NotFound();
            }

            return View(utilizador_Anonimo);
        }

        // GET: Utilizador_Anonimo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizador_Anonimo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDUtilizador,NomeUtilizador,EnderecoIPv4,EnderecoIPv6")] Utilizador_Anonimo utilizador_Anonimo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // adicionar dados do utilizador anonimo
                    // a BD
                    _context.Attach(utilizador_Anonimo);

                    // realizar commit
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                    
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do utilizador " + utilizador_Anonimo.NomeUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizador_Anonimo);
        }

        // GET: Utilizador_Anonimo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utilizador_Anonimo == null)
            {
                return NotFound();
            }

            var utilizador_Anonimo = await _context.Utilizador_Anonimo
                .FirstOrDefaultAsync(a => a.IDUtilizador == id);
            if (utilizador_Anonimo == null)
            {
                return NotFound();
            }
            return View(utilizador_Anonimo);
        }

        // POST: Utilizador_Anonimo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDUtilizador,NomeUtilizador,EnderecoIPv4,EnderecoIPv6")] Utilizador_Anonimo utilizador_Anonimo)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    // editar dados do utilizador anonimo
                    // na BD
                    _context.Update(utilizador_Anonimo);

                    // realizar commit
                    await _context.SaveChangesAsync();
                    
                    // voltar a lista
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a edição dos dados do utilizador " + utilizador_Anonimo.NomeUtilizador);
                }
            }

            // dados invalidos
            // devolver controlo a view
            return View(utilizador_Anonimo);
        }

        // GET: Utilizador_Anonimo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utilizador_Anonimo == null)
            {
                return NotFound();
            }

            var utilizador_Anonimo = await _context.Utilizador_Anonimo
                .FindAsync(id);
            if (utilizador_Anonimo == null)
            {
                return NotFound();
            }

            return View(utilizador_Anonimo);
        }

        // POST: Utilizador_Anonimo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utilizador_Anonimo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Utilizador_Anonimo'  is null.");
            }
            var utilizador_Anonimo = await _context.Utilizador_Anonimo.FindAsync(id);
            if (utilizador_Anonimo != null)
            {
                _context.Utilizador_Anonimo.Remove(utilizador_Anonimo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(utilizador_Anonimo);
        }

        private bool Utilizador_AnonimoExists(int id)
        {
          return (_context.Utilizador_Anonimo?.Any(e => e.IDUtilizador == id)).GetValueOrDefault();
        }
    }
}
