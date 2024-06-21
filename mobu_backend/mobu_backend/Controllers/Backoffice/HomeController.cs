using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mobu_backend.Models;

namespace mobu.Controllers.Backend
{
    /// <summary>
    /// Controlador principal para gerir as vistas Home
    /// </summary>
    [Authorize(Roles = "Moderador")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Construtor do controlador Home
        /// </summary>
        /// <param name="logger">O logger para registar mensagens e eventos.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Apresenta a vista inicial
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Apresenta a vista de privacidade
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Apresenta a vista de erro
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Cria um modelo de erro com o ID da requisição atual ou o identificador de rastreamento do HTTP
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
