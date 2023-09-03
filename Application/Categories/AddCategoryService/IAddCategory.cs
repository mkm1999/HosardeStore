using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.AddCategoryService
{
    public interface IAddCategory
    {
        ResultDto Execute(RequestAddCategoryDto request);
    }
}
