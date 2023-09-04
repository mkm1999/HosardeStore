using Application.Categories.AddCategoryService;
using Application.Categories.DeleteCategoryService;
using Application.Categories.EditCategoryService;
using Application.Categories.GetCategoriesService;
using Application.Categories.GetChildsCategories;
using Application.Products.AddNewProductService;
using Application.Products.DeleteProductService;
using Application.Products.GetProductsForAdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Boss")]
    public class ProductsController : Controller
    {
        private readonly IGetCategories _getCategories;
        private readonly IEditCategory _editCategory;
        private readonly IDeleteCategory _deleteCategory;
        private readonly IAddCategory _addCategory;
        private readonly IGetChildCategories getChildCategories;
        private readonly IAddNewProduct addNewProduct;
        private readonly IGetProductsForAdmin getProductsForAdmin;
        private readonly IDeleteProduct deleteProduct;

        public ProductsController(IGetCategories getCategories,IEditCategory editCategory , IDeleteCategory deleteCategory,IAddCategory addCategory,
            IGetChildCategories getChildCategories,IAddNewProduct addNewProduct,IGetProductsForAdmin getProductsForAdmin, IDeleteProduct deleteProduct)
        {
            _getCategories = getCategories;
            _editCategory = editCategory;
            _deleteCategory = deleteCategory;
            _addCategory = addCategory;
            this.getChildCategories = getChildCategories;
            this.addNewProduct = addNewProduct;
            this.getProductsForAdmin = getProductsForAdmin;
            this.deleteProduct = deleteProduct;
        }

        public IActionResult Index(int? CategoryId = null)
        {
            var result = getProductsForAdmin.Execute(CategoryId);
            return View(result);
        }

        public IActionResult Categories(int? ParentCategoryId)
        {
            ViewBag.ParentCategoryId = ParentCategoryId;
            var Categories = _getCategories.Execute(ParentCategoryId);
            return View(Categories);
        }

        public IActionResult EditCategory(int CategoryId , string NewName)
        {
            var result = _editCategory.Execute(NewName, CategoryId);
            if(!result.isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction("Categories", new { ParentCategoryId = result.data });
        }

        public IActionResult DeleteCategory(int CategoryId)
        {
            var result = _deleteCategory.Execute(CategoryId);
            if(!result.isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction("Categories", new { ParentCategoryId = result.data});
        }

        public IActionResult AddCategory(RequestAddCategoryDto request)
        {
            var result = _addCategory.Execute(request);
            if(!result.isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction("Categories", new { ParentCategoryId = request.ParentId});
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            var Categories = getChildCategories.Execute().Select(r => new { Name = r.GrandParentName + "-" + r.ParentName + "-" + r.Name ,Id = r.Id}).ToList();
            ViewBag.Categories = new SelectList(Categories,"Id","Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(RequestAddNewProductDto request, List<PropertiesDto> properties)
        {
            request.Properties = properties;
            List<IFormFile> Images = new();
            foreach (var item in Request.Form.Files)
            {
                Images.Add(item);
            }
            request.Images = Images;
            var result = addNewProduct.Execute(request);
            return Json(result);
        }

        public IActionResult DeleteProduct(int ProductId)
        {
            var result = deleteProduct.Execute(ProductId);
            if(! result.isSuccess)  return BadRequest();
            return RedirectToAction("Index", "Products");
        }
    }
}
