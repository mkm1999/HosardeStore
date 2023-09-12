
using System.Collections.Generic;


namespace Application.Products.GetProductsForSiteService
{
    public interface IGetProductsForSite
    {
        List<GetProductsForSiteDto> Execute(string SearchKey, int? CategoryId = null);
    }
}
