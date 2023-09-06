using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.GetProductsForAdminService
{
    public interface IGetProductsForAdmin
    {
        public List<GetProductsForAdminDto> Execute(int? CategoryId = null);

    }
}
