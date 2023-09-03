using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ImageUploadService
{
    public interface IUploadImage
    {
         ResultDto<string> Execute(IFormFile file);
    }

    public class UploadImage : IUploadImage
    {
        private readonly IHostingEnvironment environment;

        public UploadImage(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public static bool IsImage(string Extention)
        {
            var ImageFormats = new List<string> { ".jpg", ".jpeg", ".jpe", ".bmp", ".gif", ".png" };
            return ImageFormats.Contains(Extention.ToLowerInvariant());
        }
        public  ResultDto<string> Execute(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                return new ResultDto<string>
                {
                    isSuccess = false,
                    data = null,
                    message = "فایلی دریافت نشد",
                };
            }
            
            string FileExtention = Path.GetExtension(file.FileName);
            if(!IsImage(FileExtention))
            {
                return new ResultDto<string>
                {
                    data = null,
                    message = "لطفا فقط فایل تصویر ارسال کنید.",
                    isSuccess = false,
                };
            }
            string FileName = Path.GetRandomFileName();
            string FullFileName = Path.ChangeExtension(FileName, FileExtention);

            string Folder = $@"Images\Products";
            string FilePath = Path.Combine(environment.WebRootPath, Folder);
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            string FullFilePath = Path.Combine(FilePath, FullFileName);
            using var Stream = new FileStream(FullFilePath , FileMode.Create);
            file.CopyTo(Stream);
            return new ResultDto<string>
            {
                message = "فایل با موفقیت ذخیره شد",
                data = $@"\{Folder}\{FullFileName}",
                isSuccess = true
            };

        }
    }
}
