namespace Application.Categories.GetCategoriesService
{
    public class GetCategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public bool HasChild { get; set; }

    }
}
