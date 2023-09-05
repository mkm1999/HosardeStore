using Common;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Products.EditProductService.EditProduct;

namespace Application.Products.EditProductService
{
    public interface IEditProduct
    {
        ResultDto Execute(RequestEditProductDto requestEditProduct);
    }
}
