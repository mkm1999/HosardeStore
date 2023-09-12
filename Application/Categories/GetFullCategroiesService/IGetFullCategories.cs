using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.GetFullCategroiesService
{
    public interface IGetFullCategories
    {
        List<FullCategoryDto> Execute();
    }
}
