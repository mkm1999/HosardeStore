using System;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.FindProductService
{
    public interface IFindProduct
    {
        ProductDto FindWithId(int ProductId);

    }
}
