using Common;

namespace Application.Categories.DeleteCategoryService
{
    public interface IDeleteCategory
    {
        ResultDto<int?> Execute(int CategoryId);
    }
}
