namespace Library_Management_System.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Author { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public int TotalQuantity { get; set; }

        public decimal PenaltyAmount { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsRemoved { get; set; }

        public ICollection<IssuedBook> IssuedBooks { get; set; } = [];

        public ICollection<PurchaseBook> PurchaseBooks { get; set; } = [];
    }
}
