using Application.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Categories.GetFullCategroiesService
{
    public class GetFullCategories : IGetFullCategories
    {
        private readonly IDataBaseContext _context;

        public GetFullCategories(IDataBaseContext context)
        {
            _context = context;
        }

        public List<FullCategoryDto> Execute()
        {
            var categories = _context.categories.Where(c => c.ParentCategoryId == null)
                .Include(c => c.SubCategories).Include(c =>c.SubCategories).Select(c => new FullCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Children = c.SubCategories.Select(ch => new FullCategoryDto
                {
                    Id= ch.Id,
                    Name = ch.Name,
                    Children =  ch.SubCategories.Select(ch => new FullCategoryDto
                    {
                        Id = ch.Id,
                        Name = ch.Name,
                    }).ToList(),
                }).ToList(),
            }).ToList();

            return categories;
        }
    }
}
