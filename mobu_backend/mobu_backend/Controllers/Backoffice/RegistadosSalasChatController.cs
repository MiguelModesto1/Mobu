using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    /// <summary>
    /// Controlador da tabela de relacionamento registado <-> sala de chat
    /// </summary>
    [Authorize(Roles = "Moderador")]
    public class RegistadosSalasChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controlador da tabela de relacionamento registado <-> sala de chat
        /// </summary>
        /// <param name="context">O contexto da base de dados.</param>
        public RegistadosSalasChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter vista do Índice
        /// </summary>
        /// <returns></returns>
        // GET: RegistadosSalasChat
        public async Task<IActionResult> Index()
        {
            // Obtém todos os registos de RegistadosSalasChat, incluindo as entidades relacionadas Sala e Utilizador
            var applicationDbContext = _context.RegistadosSalasChat.Include(r => r.Sala).Include(r => r.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Obter vista de detalhes
        /// </summary>
        /// <param name="utilizadorId">ID do utilizador</param>
        /// <param name="salaId">ID da sala </param>
        /// <returns></returns>
        // GET: RegistadosSalasChat/Details/5
        public async Task<IActionResult> Details(int? utilizadorId, int? salaId)
        {
            // Verifica se os IDs do utilizador e da sala são nulos ou se o conjunto de dados RegistadosSalasChat é nulo
            if (utilizadorId == null || salaId == null || _context.RegistadosSalasChat == null)
            {
                return NotFound();
            }

            // Obtém o registo de RegistadosSalasChat com base nos IDs do utilizador e da sala
            var registadosSalasChat = await _context.RegistadosSalasChat
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.UtilizadorFK == utilizadorId && m.SalaFK == salaId);
            if (registadosSalasChat == null)
            {
                return NotFound();
            }

            return View(registadosSalasChat);
        }

        /// <summary>
        /// Obter vista de criação
        /// </summary>
        /// <returns></returns>
        // GET: RegistadosSalasChat/Create
        public IActionResult Create()
        {
            // Cria uma lista suspensa com as salas de chat disponíveis e os utilizadores registados
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala");
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador");
            return View();
        }

        /// <summary>
        /// Publicar criação
        /// </summary>
        /// <param name="registadosSalasChat">Instância do modelo</param>
        /// <returns></returns>
        // POST: RegistadosSalasChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAdmin,UtilizadorFK,SalaFK")] RegistadosSalasChat registadosSalasChat)
        {
            // Verifica se o modelo é válido antes de salvar os dados
            if (ModelState.IsValid)
            {
                _context.Add(registadosSalasChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala", registadosSalasChat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasChat.UtilizadorFK);
            return View(registadosSalasChat);
        }

        /// <summary>
        /// Obter vista de edição
        /// </summary>
        /// <param name="utilizadorId">ID do utilizador</param>
        /// <param name="salaId">ID da sala</param>
        /// <returns></returns>
        // GET: RegistadosSalasChat/Edit/5
        public async Task<IActionResult> Edit(int? utilizadorId, int? salaId)
        {
            // Verifica se os IDs do utilizador e da sala são nulos ou se o conjunto de dados RegistadosSalasChat é nulo
            if (utilizadorId == null || salaId == null || _context.RegistadosSalasChat == null)
            {
                return NotFound();
            }

            // Obtém o registo de RegistadosSalasChat com base nos IDs do utilizador e da sala
            var registadosSalasChat = await _context.RegistadosSalasChat.FindAsync(utilizadorId, salaId);
            if (registadosSalasChat == null)
            {
                return NotFound();
            }
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala", registadosSalasChat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasChat.UtilizadorFK);
            return View(registadosSalasChat);
        }

        /// <summary>
        /// Publicar edição
        /// </summary>
        /// <param name="utilizadorId">ID do utilizador</param>
        /// <param name="salaId">ID da sala</param>
        /// <param name="registadosSalasChat">Instância do modelo</param>
        /// <returns></returns>
        // POST: RegistadosSalasChat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int utilizadorId, int salaId, [Bind("IsAdmin,UtilizadorFK,SalaFK")] RegistadosSalasChat registadosSalasChat)
        {
            // Verifica se os IDs do utilizador e da sala correspondem aos IDs do objeto RegistadosSalasChat
            if (utilizadorId != registadosSalasChat.UtilizadorFK || salaId != registadosSalasChat.SalaFK)
            {
                return NotFound();
            }

            // Verifica se o modelo é válido antes de atualizar os dados
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registadosSalasChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistadosSalasChatExists(registadosSalasChat.UtilizadorFK, registadosSalasChat.SalaFK))
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
            ViewData["SalaFK"] = new SelectList(_context.SalasChat, "IDSala", "NomeSala", registadosSalasChat.SalaFK);
            ViewData["UtilizadorFK"] = new SelectList(_context.UtilizadorRegistado, "IDUtilizador", "NomeUtilizador", registadosSalasChat.UtilizadorFK);
            return View(registadosSalasChat);
        }

        /// <summary>
        /// Obter vista de remoção
        /// </summary>
        /// <param name="utilizadorId">ID do utilziador</param>
        /// <param name="salaId">ID da sala</param>
        /// <returns></returns>
        // GET: RegistadosSalasChat/Delete/5
        public async Task<IActionResult> Delete(int? utilizadorId, int? salaId)
        {
            // Verifica se os IDs do utilizador e da sala são nulos ou se o conjunto de dados RegistadosSalasChat é nulo
            if (utilizadorId == null || salaId == null || _context.RegistadosSalasChat == null)
            {
                return NotFound();
            }

            // Obtém o registo de RegistadosSalasChat com base nos IDs do utilizador e da sala
            var registadosSalasChat = await _context.RegistadosSalasChat
                .Include(r => r.Sala)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.UtilizadorFK == utilizadorId && m.SalaFK == salaId);
            if (registadosSalasChat == null)
            {
                return NotFound();
            }

            return View(registadosSalasChat);
        }

        /// <summary>
        /// Publicar remoção
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <returns></returns>
        // POST: RegistadosSalasChat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Verifica se o conjunto de dados RegistadosSalasChat é nulo
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

        /// <summary>
        /// Verifica se um registado numa sala existe na base de dados
        /// </summary>
        /// <param name="utilizadorId">ID do utilizador</param>
        /// <param name="salaId">ID da sala</param>
        /// <returns></returns>
        private bool RegistadosSalasChatExists(int utilizadorId, int salaId)
        {
            return _context.RegistadosSalasChat.Any(e => e.UtilizadorFK == utilizadorId && e.SalaFK == salaId);
        }
    }
}
