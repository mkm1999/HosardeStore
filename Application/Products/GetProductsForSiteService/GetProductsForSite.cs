using Application.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Products.GetProductsForSiteService
{
    public class GetProductsForSite : IGetProductsForSite
    {
        private readonly IDataBaseContext _context;

        public GetProductsForSite(IDataBaseContext context)
        {
            _context = context;
        }

        public List<GetProductsForSiteDto> Execute(string SearchKey , int? CategoryId = null)
        {
            var ProductsQuery = _context.products.Include(p => p.Images).Include(p => p.Category).ThenInclude(c => c.ParentCategory).AsQueryable();

            if (CategoryId != null)
            {
                ProductsQuery = ProductsQuery.Where(
                    p => p.CategoryId == CategoryId ||
                    p.Category.ParentCategoryId == CategoryId ||
                    p.Category.ParentCategory.ParentCategoryId == CategoryId
                    ).AsQueryable();
            }

            if (! string.IsNullOrEmpty(SearchKey))
            {
                ProductsQuery = ProductsQuery.Where(p => p.Name.Contains(SearchKey) || p.Barnd.Contains(SearchKey) || p.Category.Name.Contains(SearchKey)).AsQueryable();
            }
            
            var Products = ProductsQuery.Select(p => new GetProductsForSiteDto
            {
                ImgSrc = p.Images.FirstOrDefault().Url,
                Name = p.Name,
                Price = p.Price.ToString("N0"),
                Id = p.Id,
            }).ToList();
            return Products;
        }
    }
}
