namespace Application.Products.GetProductsForAdminService
{
    public class GetProductsForAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnable { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public string CategoryName { get; set; }
        public string Brand { get; set; }

    }
}
