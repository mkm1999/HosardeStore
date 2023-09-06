using Application.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Products.GetProductsForAdminService
{
    public class GetProductsForAdmin : IGetProductsForAdmin
    {
        private readonly IDataBaseContext _context;

        public GetProductsForAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public List<GetProductsForAdminDto> Execute(int? CategoryId = null)
        {
            var ProductsQuery = _context.products.Include(p => p.Category).ThenInclude(c => c.ParentCategory).ThenInclude(c => c.ParentCategory).AsQueryable();
            if (CategoryId != null)
            {
                ProductsQuery = ProductsQuery.Where(p => p.CategoryId == CategoryId || p.Category.ParentCategoryId == CategoryId || p.Category.ParentCategory.ParentCategoryId == CategoryId);
            }
            var Products = ProductsQuery.Select(p => new GetProductsForAdminDto
            {
                CategoryName = $"{p.Category.Name} - {p.Category.ParentCategory.Name} - {p.Category.ParentCategory.ParentCategory.Name}",
                Brand = p.Barnd,
                Id = p.Id,
                Inventory = p.Inventory,
                IsEnable = p.IsEnable,
                Name = p.Name,
                Price = p.Price,
            }).ToList();
            return Products;
        }
    }
}
