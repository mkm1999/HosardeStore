using Application.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.GetChildsCategories
{
    public interface IGetChildCategories
    {
        List<GetChildCategoriesDto> Execute();
    }

    public class GetChildCategories : IGetChildCategories
    {
        private readonly IDataBaseContext _context;

        public GetChildCategories(IDataBaseContext context)
        {
            _context = context;
        }

        public List<GetChildCategoriesDto> Execute()
        {
            var result = _context.categories.Where(c => c.SubCategories.Count == 0)
                .Include(c => c.ParentCategory)
                .ThenInclude(c =>c.ParentCategory)
                .Select(c => new GetChildCategoriesDto 
                {
                    Name = c.Name,
                    GrandParentName = c.ParentCategory.ParentCategory.Name,
                    ParentName = c.ParentCategory.Name,
                    Id = c.Id,
                }).ToList();
            return result;
        }
    }
    public class GetChildCategoriesDto
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string GrandParentName { get; set; }
        public int Id { get; set; }
    }
}
