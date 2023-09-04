using Application.Interfaces.Context;
using Common;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace Application.Products.DeleteProductService
{
    public class DeleteProduct : IDeleteProduct
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public DeleteProduct(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public ResultDto Execute(int ProductId)
        {
            var productImages = _context.productsImages.Where(p => p.ProductId == ProductId).ToList();
            if(productImages.Count > 0)
            {
                foreach (var image in productImages)
                {
                    string path = _environment.WebRootPath + image.Url;
                    File.Delete(path);
                }

            }
            var product = _context.products.Find(ProductId);
            if (product == null)
            {
                return new ResultDto
                {
                    isSuccess = false,
                    message = "محصول یافت نشد",
                };
            }
            _context.products.Remove(product);
            _context.SaveChanges();
            return new ResultDto
            {
                isSuccess = true,
                message = "کالا با موفقیت حذف شد"
            };
        }
    }
}
