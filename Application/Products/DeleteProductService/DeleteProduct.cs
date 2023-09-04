using Application.Interfaces.Context;
using Common;

namespace Application.Products.DeleteProductService
{
    public class DeleteProduct : IDeleteProduct
    {
        private readonly IDataBaseContext _context;

        public DeleteProduct(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(int ProductId)
        {
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
