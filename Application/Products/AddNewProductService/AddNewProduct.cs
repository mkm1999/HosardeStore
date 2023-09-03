using Application.ImageUploadService;
using Application.Interfaces.Context;
using Common;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Products.AddNewProductService
{
    public class AddNewProduct : IAddNewProduct
    {
        private readonly IDataBaseContext _context;
        private readonly IUploadImage uploadImage;

        public AddNewProduct(IDataBaseContext context , IUploadImage uploadImage)
        {
            _context = context;
            this.uploadImage = uploadImage;
        }

        public ResultDto Execute(RequestAddNewProductDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return new ResultDto
                {
                    message = "لطفا نام کالا را وارد کنید",
                    isSuccess = false,
                };
            }
            var Category = _context.categories.Find(request.CategoryId);
            if(Category == null)
            {
                return new ResultDto
                {
                    message = "دسته بندی یافت نشد",
                    isSuccess = false,
                };
            }
            Product product = new Product
            {
                Barnd = request.Brand,
                Category = Category,
                Description = request.Description,
                Name = request.Name,
                Price = request.Price,
                Inventory = request.Inventory,
                IsEnable = request.IsActive
            };
            _context.products.Add(product);

            List<ProductImages> images = new();
            foreach (var item in request.Images)
            {
                var UploadResult = uploadImage.Execute(item);
                if (!UploadResult.isSuccess)
                {
                    return new ResultDto
                    {
                        message = UploadResult.message,
                        isSuccess = false
                    };
                }
                images.Add(new ProductImages
                {
                    Product = product,
                    Url = UploadResult.data
                });
            }
            _context.productsImages.AddRange(images);

            List<ProductProperties> properties = new();
            foreach (var item in request.Properties)
            {
                if(string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
                {
                    return new ResultDto
                    {
                        message = "نام یا مقدار خصوصیت را وارد کنید",
                        isSuccess = false,
                    };
                }
                properties.Add(new ProductProperties
                {
                    DisplayName = item.Name,
                    Value = item.Value,
                    Product = product
                });
            }
            _context.ProductsProperties.AddRange(properties);
            _context.SaveChanges();
            return new ResultDto
            {
                message = $"محصول {product.Name} با موفقیت اضافه شد.",
                isSuccess = true,
            };
        }
    }
}
