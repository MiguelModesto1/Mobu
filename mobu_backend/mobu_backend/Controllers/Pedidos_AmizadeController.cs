using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mobu_backend.Controllers
{
    public class Pedidos_AmizadeController : Controller
    {
        // GET: Pedidos_AmizadeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pedidos_AmizadeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pedidos_AmizadeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedidos_AmizadeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pedidos_AmizadeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pedidos_AmizadeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pedidos_AmizadeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pedidos_AmizadeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
