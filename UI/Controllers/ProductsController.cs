using Application.Products.GetProductsForSiteService;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IGetProductsForSite getProductsForSite;

        public ProductsController(IGetProductsForSite getProductsForSite)
        {
            this.getProductsForSite = getProductsForSite;
        }

        public IActionResult Index(string SearchKey)
        {
            var products = getProductsForSite.Execute(SearchKey);
            return View(products);
        }
    }
}
