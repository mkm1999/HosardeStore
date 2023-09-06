namespace Application.Products.GetProductsForSiteService
{
    public class GetProductsForSiteDto
    {
        public string Name { get; set; }
        public string ImgSrc { get; set; }
        public string Price { get; set; }
        public int? StarsCount { get; set; }
        public int? OffPercent { get; set; }
        public string OffedPrice { get; set; }
        public int Id { get; set; }
    }
}
