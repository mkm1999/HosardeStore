using Application.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Categories.GetCategoriesService
{
    public class GetCategories : IGetCategories
    {
        private readonly IDataBaseContext _context;

        public GetCategories(IDataBaseContext context)
        {
            _context = context;
        }

        public List<GetCategoriesDto> Execute(int? ParentId)
        {
            var result = _context.categories.Include(c => c.ParentCategory).Include(c => c.SubCategories).Where(c => c.ParentCategoryId == ParentId).Select(c => new GetCategoriesDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentName = c.ParentCategory.Name != null ? c.ParentCategory.Name : "-",
                HasChild = c.SubCategories.Count() > 0,
            }).ToList();
            return result;
        }
    }
}
