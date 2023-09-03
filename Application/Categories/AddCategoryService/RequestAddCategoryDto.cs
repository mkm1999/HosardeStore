namespace Application.Categories.AddCategoryService
{
    public class RequestAddCategoryDto
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
