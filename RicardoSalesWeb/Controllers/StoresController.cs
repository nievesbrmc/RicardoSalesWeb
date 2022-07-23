using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RicardoSalesWeb.Controllers
{
    public class StoresController : Controller
    {
        private readonly IConfiguration _configuration;
        public StoresController(IConfiguration config)
        {
            _configuration = config;
        }
        // GET: StoresController
        public async Task<ActionResult> Index()
        {
            IEnumerable<Entity.StoreModel > model = await new BLL.StoreAction(_configuration).Get(null).ConfigureAwait(false);
            return View(model);
        }

        // GET: StoresController/Create
        public ActionResult Create()
        {
            return View(new Entity.StoreModel());
        }

        // POST: StoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Entity.StoreModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = await new BLL.StoreAction(_configuration).Create(model).ConfigureAwait(false);
                    if (result>0)
                    {

                        ViewData["Message"] = "Tienda agregada correctamente";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoresController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoresController/Edit/5
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

        // GET: StoresController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoresController/Delete/5
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
