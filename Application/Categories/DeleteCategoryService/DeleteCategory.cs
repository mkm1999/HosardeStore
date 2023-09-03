using Application.Interfaces.Context;
using Common;

namespace Application.Categories.DeleteCategoryService
{
    public class DeleteCategory : IDeleteCategory
    {
        private readonly IDataBaseContext _context;

        public DeleteCategory(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<int?> Execute(int CategoryId)
        {
            var category = _context.categories.Find(CategoryId);
            int? ParentId = category.ParentCategoryId;
            if (category == null)
            {
                return new ResultDto<int?>()
                {
                    isSuccess = false,
                    message = "دسته بندی یافت نشد",
                };
            }
            _context.categories.Remove(category);
            _context.SaveChanges();
            return new ResultDto<int?>()
            {
                isSuccess = true,
                message = "دسته بندی با موفقیت حذف شد",
                data = ParentId
            };
        }
    }
}
