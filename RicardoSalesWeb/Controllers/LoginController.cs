
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Entity;

namespace RicardoSalesWeb.Controllers
{
    public class UserModel
    {
        [Display(Name ="Correo electrónico")]
        [Required(ErrorMessage = "Ingrese el correo electrónico")]
        [EmailAddress(ErrorMessage ="Favor de ingresar un correo válido")]
        public string? UserName { get; set; }
        [Required(ErrorMessage ="Ingrese la contraseña")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password, ErrorMessage ="Favor ingrese su contraseña")]
        public string? Password { get; set; }
        
    }
         
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration config)
        {
            _configuration = config;
        }
        // GET: LoginController
        public ActionResult Login()
        {
            UserModel model = new UserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserModel request)
        {
            UserModel model = new UserModel();
            if (ModelState.IsValid)
            {
                BLL.UserBusiness data = new BLL.UserBusiness(_configuration);
                Entity.ClientModel cl = await data.Login(request.UserName, request.Password).ConfigureAwait(false);
                if (cl !=null && cl.ClientId > 0)
                {
                    Response.Cookies.Append("usrData", cl.ClientId.Value.ToString());
                    Response.Cookies.Append("usrName", cl.Name + " " + cl.LastName);
                    ViewData["usrName"] = cl.Name + " " + cl.LastName;
                    TempData["usrLogin"] = request.UserName;
                    return Redirect("Home/Index");
                }
                ViewData["Message"] = "Usuario o contraseña incorrecto";
            }
            return View(model);
        }

        // GET: LoginController/Details/5
        public ActionResult Register()
        {
            ClientModel model = new ClientModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(ClientModel model)
        {
            BLL.UserBusiness data = new BLL.UserBusiness(_configuration);
            if (ModelState.IsValid)
            {
                bool cl = await data.Register(model).ConfigureAwait(false);
                if (cl)
                {
                    ViewData["Message"] = "Usuario registrado correctamente";
                    return RedirectToAction("Login");
                }
            }                
            return View(model);
        }

        // GET: LoginController/Create
        public ActionResult LogOut()
        {
            TempData["usrLogin"] = null;
            return RedirectToAction("Login");
        }
    }
}
