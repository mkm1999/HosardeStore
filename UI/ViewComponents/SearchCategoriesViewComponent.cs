using Application.Categories.GetCategoriesService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace UI.ViewComponents
{
    public class SearchCategoriesViewComponent : ViewComponent
    {
        private readonly IGetCategories getCategories;

        public SearchCategoriesViewComponent(IGetCategories getCategories)
        {
            this.getCategories = getCategories;
        }

        public IViewComponentResult Invoke()
        {
            var categoriesList = getCategories.Execute(null);
            var categories = new SelectList(categoriesList, "Id", "Name");
            return View("View",categories);
        }
    }
}
