using Common;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.AddNewProductService
{
    public interface IAddNewProduct
    {
        ResultDto Execute(RequestAddNewProductDto request);
    }
}
