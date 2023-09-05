using System.Collections.Generic;

namespace Application.Products.FindProductService
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public bool IsEnable { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<ImageDto> Images { get; set; }
        public List<propertyDto> Properties { get; set; }

    }

    public class ImageDto
    {
        public int Id { get; set; }
        public string Src { get; set; }
    }
}
