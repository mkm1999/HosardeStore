using Application.Products.AddNewProductService;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Products.EditProductService
{
    public class RequestEditProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public bool IsActive { get; set; }
        public string Brand { get; set; }
        public List<IFormFile> Images;
        public List<PropertiesDto> Properties;
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
    }
}
