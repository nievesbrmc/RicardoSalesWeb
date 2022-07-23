using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RicardoSalesWeb.Controllers
{
    public class SalesClientModel
    {   
        public int SaledIdentifier { get; set; }
        public Entity.ProductModel product { get; set; }
        public Entity.StoreModel store { get; set; }
        public DateTime? SaleDate { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
    }
    public class SalesController : Controller
    {
        private readonly IConfiguration _configuration;
        public SalesController(IConfiguration config)
        {
            _configuration = config;
        }
        // GET: SalesController
        public async Task<ActionResult> Index()
        {
            IEnumerable<SalesClientModel> modelResponse = new List<SalesClientModel>();
            try
            {
                SalesClientModel model = new SalesClientModel();
                IEnumerable<Entity.StoreModel> storesData = await new BLL.StoreAction(_configuration).Get(null).ConfigureAwait(false);
                storesData.ToList().RemoveAll(x => x.Active == false);

                IEnumerable<Entity.ProductModel> productsData = await new BLL.ProductBusiness(_configuration).GetProducts(null).ConfigureAwait(false);
                productsData.ToList().RemoveAll(x => x.Active == false);

                IEnumerable<Entity.ClientProductModel> clProd = await new BLL.ClientProduct(_configuration).Get(null).ConfigureAwait(false);

                IEnumerable<Entity.ProductStoreModel> prodStore = await new BLL.ProductStore(_configuration).Get(null).ConfigureAwait(false);

                Request.Cookies.TryGetValue("usrData", out string? clientId);
                if (!string.IsNullOrEmpty(clientId) && int.TryParse(clientId, out int IdC))
                {
                    IEnumerable<Entity.ClientModel> cliList = await new BLL.UserBusiness(_configuration).GetClients(IdC).ConfigureAwait(false);
                    if (clientId != null)
                    {
                        Entity.ClientModel clientModel = cliList.FirstOrDefault();
                        IEnumerable<Entity.ClientProductModel> listByClient = (from item in clProd
                                                                               where item.ClientId == clientModel.ClientId
                                                                               select new Entity.ClientProductModel
                                                                               {
                                                                                   ClientProductId = item.ClientProductId,
                                                                                   ClientId = item.ClientId,
                                                                                   ProductId = item.ClientId,
                                                                                   DateInserted = item.DateInserted
                                                                               });

                        modelResponse = (from Lc in listByClient
                                         join products in productsData on Lc.ProductId equals products.ProductId
                                         join proStore in prodStore on Lc.ProductId equals proStore.ProductId
                                         join stores in storesData on proStore.StoreId equals stores.StoreId
                                         select new SalesClientModel
                                         {
                                             product = new Entity.ProductModel
                                             {
                                                 ProductId = products.ProductId,
                                                 Description = products.Description,
                                                 Code = products.Code,
                                                 Price = products.Price,
                                             },
                                             store = new Entity.StoreModel
                                             {
                                                 StoreId = stores.StoreId,
                                                 Branch = stores.Branch
                                             },
                                             SaledIdentifier = Lc.ClientProductId,
                                             SaleDate = Lc.DateInserted
                                         }).ToList();
                        return View(modelResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(modelResponse);
            }
            return View(modelResponse);
        }

        // GET: SalesController/Create
        public async Task<ActionResult> Create()
        {
            SalesClientModel model = new SalesClientModel();
            IEnumerable<Entity.StoreModel> storesData = await new BLL.StoreAction(_configuration).Get(null).ConfigureAwait(false);
            storesData.ToList().RemoveAll(x => x.Active == false);

            IEnumerable<Entity.ProductModel> productsData = await new BLL.ProductBusiness(_configuration).GetProducts(null).ConfigureAwait(false);
            productsData.ToList().RemoveAll(x => x.Active == false);

            model.Stores = (from item in storesData
                            where item.Active == true
                            select new SelectListItem
                            {
                                Text = item.Branch,
                                Value = item.StoreId.ToString()
                            }).ToList();

            model.Products = (from item in productsData
                              where item.Active == true
                              select new SelectListItem
                              {
                                  Text = item.Description,
                                  Value = item.ProductId.ToString()
                              }).ToList();
            return View(model);
        }

        // POST: SalesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SalesClientModel collection)
        {
            try
            {
                Request.Cookies.TryGetValue("usrData", out string? clientId);
                if (ModelState.IsValid && int.TryParse(clientId, out int Id))
                {
                  bool response =  await new BLL.ClientProduct(_configuration).Create(new Entity.ClientProductModel
                    {
                        ClientId = Id,
                        ProductId = collection.product.ProductId
                    }).ConfigureAwait(false);
                    if (response)
                    {
                        ViewData["Message"] = "El producto se agrego al carrito correctamente";
                    }
                    else
                    {
                        ViewData["Message"] = "No es posible agregar el producto. Intente más tarde";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
