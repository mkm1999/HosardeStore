using Application.ImageUploadService;
using Application.Interfaces.Context;
using Common;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Products.EditProductService
{
    public partial class EditProduct : IEditProduct
    {
        private readonly IDataBaseContext _context;
        private readonly IUploadImage uploadImage;

        public EditProduct(IDataBaseContext context , IUploadImage uploadImage)
        {
            _context = context;
            this.uploadImage = uploadImage;
        }

        public ResultDto Execute(RequestEditProductDto requestEditProduct)
        {
            if (string.IsNullOrEmpty(requestEditProduct.Name))
            {
                return new ResultDto
                {
                    message = "لطفا نام کالا را وارد کنید",
                    isSuccess = false,
                };
            }
            var Category = _context.categories.Find(requestEditProduct.CategoryId);
            if (Category == null)
            {
                return new ResultDto
                {
                    message = "دسته بندی یافت نشد",
                    isSuccess = false,
                };
            }
            var Product = _context.products.Find(requestEditProduct.ProductId);
            if (Product == null)
            {
                return new ResultDto
                {
                    message = "محصول مورد نظر یافت نشد",
                    isSuccess = false,
                };
            }
            Product.Name = requestEditProduct.Name;
            Product.Description = requestEditProduct.Description;
            Product.Inventory = requestEditProduct.Inventory;
            Product.Price = requestEditProduct.Price;
            Product.Barnd = requestEditProduct.Brand;
            Product.Category = Category;
            Product.IsEnable = requestEditProduct.IsActive;

            List<ProductImages> images = new();
            foreach (var item in requestEditProduct.Images)
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
                    Product = Product,
                    Url = UploadResult.data
                });
            }
            _context.productsImages.AddRange(images);

            var oldprops = _context.ProductsProperties.Where(p => p.ProductId == requestEditProduct.ProductId).ToList();
            _context.ProductsProperties.RemoveRange(oldprops);
            List<ProductProperties> properties = new();
            foreach (var item in requestEditProduct.Properties)
            {
                if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
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
                    Product = Product
                });
            }
            _context.ProductsProperties.AddRange(properties);
            _context.SaveChanges();
            return new ResultDto
            {
                message = $"محصول {Product.Name} با موفقیت ویرایش شد.",
                isSuccess = true,
            };
        }
    }
}
