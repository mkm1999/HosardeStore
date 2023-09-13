using Application.Categories.GetFullCategroiesService;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents
{
    public class SmallGetMenuViewComponent : ViewComponent
    {
        private readonly IGetFullCategories getFullCategories;

        public SmallGetMenuViewComponent(IGetFullCategories getFullCategories)
        {
            this.getFullCategories = getFullCategories;
        }

        public IViewComponentResult Invoke()
        {
            var categories = getFullCategories.Execute();
            return View("View", categories);
        }
    }
}
