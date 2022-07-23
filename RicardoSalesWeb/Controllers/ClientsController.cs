using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RicardoSalesWeb.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IConfiguration _configuration;
        public ClientsController(IConfiguration config)
        {
            _configuration = config;
        }
        // GET: ClientsController
        public async Task<ActionResult> Index()
        {
            IEnumerable<ClientModel> model = await new BLL.UserBusiness(_configuration).GetClients(null).ConfigureAwait(false);
            return View(model);
        }

        // GET: ClientsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                List<ClientModel> modelList = await new BLL.UserBusiness(_configuration).GetClients(id).ConfigureAwait(false);
                ClientModel model = modelList.FirstOrDefault();
                if (model!=null)
                {
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClientModel model)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    bool res= await new BLL.UserBusiness(_configuration).Update(model, false).ConfigureAwait(false);
                    if (res)
                    {
                        ViewData["Message"] = "Registro actualizado correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Message"] = "No se logro actualizar el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return RedirectToAction("Error","Home");
            }
            return RedirectToAction("Index");
        }
               
        
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                List<ClientModel> modelList = await new BLL.UserBusiness(_configuration).GetClients(id).ConfigureAwait(false);
                ClientModel model = modelList.FirstOrDefault();
                if (model == null)
                {
                    ViewData["Message"] = "Registro incorrecto";
                    return RedirectToAction("Index");
                }
                bool res = await new BLL.UserBusiness(_configuration).Update(model, true).ConfigureAwait(false);
                if (res)
                {
                    ViewData["Message"] = "Registro eliminado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Message"] = "Registro incorrecto";
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
