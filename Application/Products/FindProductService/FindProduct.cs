using Application.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Products.FindProductService
{
    public class FindProduct : IFindProduct
    {
        private readonly IDataBaseContext _context;

        public FindProduct(IDataBaseContext context)
        {
            _context = context;
        }

        public ProductDto FindWithId(int ProductId)
        {
            var productQuery = _context.products.Where(p => p.Id == ProductId).Include(p => p.Category).ThenInclude(c => c.ParentCategory)
                .ThenInclude(c => c.ParentCategory).Include(p => p.Images).Include(p => p.Properties).FirstOrDefault();
            var product = new ProductDto
            {
                Brand = productQuery.Barnd,
                Name = productQuery.Name,
                Description = productQuery.Description,
                Id = productQuery.Id,
                Inventory = productQuery.Inventory,
                Price = productQuery.Price,
                IsEnable = productQuery.IsEnable,
                Images = new List<string> { },
                Properties = new List<propertyDto> { },
            };
            product.CategoryName = productQuery.Category.Name;
            if(productQuery.Category.ParentCategory != null)
            {
                product.CategoryName = $"{productQuery.Category.ParentCategory.Name}/{product.CategoryName}";
                if (productQuery.Category.ParentCategory.ParentCategory != null)
                {
                    product.CategoryName = $"{productQuery.Category.ParentCategory.ParentCategory.Name}/{productQuery.Category.ParentCategory.Name}/{product.CategoryName}";
                }
            }

            foreach (var item in productQuery.Images)
            {
                product.Images.Add(item.Url);
            }
            foreach(var item in productQuery.Properties)
            {
                product.Properties.Add(new propertyDto
                {
                    Key = item.DisplayName,
                    Value = item.Value
                });
            }
            return product;
        }
    }
}
