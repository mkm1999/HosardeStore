using Application.Products.FindProductService;
using Application.Products.GetProductsForSiteService;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IGetProductsForSite getProductsForSite;
        private readonly IFindProduct findProduct;

        public ProductsController(IGetProductsForSite getProductsForSite, IFindProduct findProduct)
        {
            this.getProductsForSite = getProductsForSite;
            this.findProduct = findProduct;
        }

        public IActionResult Index(string SearchKey)
        {
            var products = getProductsForSite.Execute(SearchKey);
            return View(products);
        }

        public IActionResult Details(int Id)
        {
            var product = findProduct.FindWithId(Id);
            return View(product);
        }
    }
}
