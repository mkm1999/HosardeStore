using System.Collections.Generic;

namespace Application.Categories.GetFullCategroiesService
{
    public class FullCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FullCategoryDto> Children { get; set; }
    }
}
