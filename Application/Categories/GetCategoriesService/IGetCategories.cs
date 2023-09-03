
using System.Collections.Generic;


namespace Application.Categories.GetCategoriesService
{
    public interface IGetCategories
    {
        List<GetCategoriesDto> Execute(int? ParentId);
    }
}
