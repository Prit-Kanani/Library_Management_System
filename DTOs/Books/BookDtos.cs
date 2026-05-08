namespace Library_Management_System.DTOs.Books
{
    public class BookFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public string? Author { get; set; }

        public bool? IsActive { get; set; }

        public bool IncludeRemoved { get; set; }
    }

    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Author { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public int TotalQuantity { get; set; }

        public decimal PenaltyAmount { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class BookUpdateDto : BookCreateDto
    {
    }

    public class BookViewDto
    {
        public int BookId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Author { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int TotalQuantity { get; set; }

        public decimal PenaltyAmount { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsActive { get; set; }

        public bool IsRemoved { get; set; }
    }
}
