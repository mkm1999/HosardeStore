using Application.Interfaces.Context;
using Common;

namespace Application.Categories.EditCategoryService
{
    public class EditCategory : IEditCategory
    {
        private readonly IDataBaseContext _context;

        public EditCategory(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<int?> Execute(string NewName, int CategoryId)
        {
            var Category = _context.categories.Find(CategoryId);
            if (Category == null)
            {
                return new ResultDto<int?>
                {
                    isSuccess = false,
                    message = "دسته بندی یافت نشد",
                };
            }
            Category.Name = NewName;
            _context.SaveChanges();
            return new ResultDto<int?>
            {
                isSuccess = true,
                message = "دسته بندی با موفقیت ویرایش شد",
                data = Category.ParentCategoryId
            };
        }
    }
}
