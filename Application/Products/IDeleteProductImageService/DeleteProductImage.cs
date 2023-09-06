using Application.Interfaces.Context;
using Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace Application.Products.IDeleteProductImageService
{
    public class DeleteProductImage : IDeleteProductImage
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public DeleteProductImage(IDataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;
        }

        public ResultDto Execute(int ImageId)
        {
            var Image = _context.productsImages.Find(ImageId);
            if (Image == null)
            {
                return new ResultDto
                {
                    isSuccess = false,
                    message = "تصویر یافت نشد"
                };
            }
            var Path = _environment.WebRootPath + Image.Url;
            try
            {
                File.Delete(Path);
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException("exception happend when deleting", ex);
            }
            finally
            {
                _context.productsImages.Remove(Image);
                _context.SaveChanges();
            }
            return new ResultDto { isSuccess = true };
        }
    }
}
