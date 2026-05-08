namespace Library_Management_System.DTOs.Categories
{
    public class CategoryFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
    }

    public class CategoryCreateDto
    {
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CategoryUpdateDto
    {
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CategoryViewDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
