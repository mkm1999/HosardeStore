using Application.Interfaces.Context;
using Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.AddCategoryService
{
    public class AddCategory : IAddCategory
    {
        private readonly IDataBaseContext _context;

        public AddCategory(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestAddCategoryDto request)
        {
            if(string.IsNullOrEmpty(request.Name))
            {
                return new ResultDto()
                {
                    message = "لطفا نام دسته بندی را وارد کنید.",
                    isSuccess = false
                };
            }
            Category Parent = null;
            if (request.ParentId != null)
            {
                Parent = _context.categories.Find(request.ParentId);
            }
            Category newCategory = new Category
            {
                Name = request.Name,
                ParentCategory = Parent
            };
            _context.categories.Add(newCategory);
            _context.SaveChanges();
            return new ResultDto()
            {
                message = "دسته بندی با موفقیت افزوده شد",
                isSuccess = true
            };

        }
    }
}
