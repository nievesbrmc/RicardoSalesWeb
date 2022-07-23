using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RicardoSalesWeb.Controllers
{
    public class ProductModelData
    {
        public Entity.ProductModel Product { get; set; }
        public Entity.StoreModel StoreModel { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }
    }
    public class ProductController : Controller
    {

        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration config)
        {
            _configuration = config;
        }
        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            IEnumerable<ProductModel> product = await new BLL.ProductBusiness(_configuration).GetProducts(null).ConfigureAwait(false);
            return View(product);
        }

        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            ProductModelData model = new ProductModelData();
            IEnumerable<Entity.StoreModel> storesList = await new BLL.StoreAction(_configuration).Get(null).ConfigureAwait(false);
            model.Stores = (from item in storesList
                            where item.Active == true
                            select new SelectListItem
                            {
                                Text = item.Address,
                                Value = item.StoreId.ToString()
                            }).ToList();
            return View(model);
        }
        
        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductModelData model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Entity.ProductModel pmodel = model.Product;
                    int productNew = await new BLL.ProductBusiness(_configuration).Create(pmodel).ConfigureAwait(false);
                    if (productNew>0)
                    {
                        bool result = await new BLL.ProductStore(_configuration).Create(new ProductStoreModel
                        {
                            ProductId = productNew,
                            StoreId = model.StoreModel.StoreId
                        }).ConfigureAwait(false);
                        if (result)
                        {
                            ViewData["Message"] = "El producto se agrego correctamente";
                        }
                        else
                        {
                            ViewData["Message"] = "No fue posible ligar el producto a la tienda";
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "No fue posible agregar el producto en este momento. Intente más tarde";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error","Home");
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            IEnumerable<Entity.ProductModel> products = await new BLL.ProductBusiness(_configuration).GetProducts(id).ConfigureAwait(false);
            ProductModel pModel = products.FirstOrDefault();
            if (pModel != null)
            {
                ProductModelData model = new ProductModelData();
                model.Product = pModel;
                IEnumerable<Entity.StoreModel> storesList = await new BLL.StoreAction(_configuration).Get(null).ConfigureAwait(false);
                model.Stores = (from item in storesList
                                where item.Active == true
                                select new SelectListItem
                                {
                                    Text = item.Address,
                                    Value = item.StoreId.ToString()
                                }).ToList();
                return View(model);
            }

            ViewData["Message"] = "No fue posible obtener el detalle del producto en este momento. Intente más tarde";
            return RedirectToAction(nameof(Index));
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductModelData model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Entity.ProductModel pmodel = model.Product;
                    bool update = await new BLL.ProductBusiness(_configuration).updateDelete(pmodel,false).ConfigureAwait(false);
                    if (update)
                    {
                        update = await new BLL.ProductStore(_configuration).Create(new ProductStoreModel
                        {
                            ProductId = model.Product.ProductId,
                            StoreId = model.StoreModel.StoreId
                        }).ConfigureAwait(false);
                        if (update)
                        {
                            ViewData["Message"] = "El producto se actualizo correctamente";
                        }
                        else
                        {
                            ViewData["Message"] = "No fue posible actualizar producto a la tienda correcta";
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "No fue posible actualizar la información del producto en este momento. Intente más tarde";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
