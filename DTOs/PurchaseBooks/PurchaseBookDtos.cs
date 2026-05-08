namespace Library_Management_System.DTOs.PurchaseBooks
{
    public class PurchaseBookFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int? BookId { get; set; }

        public string? PurchaseFrom { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class PurchaseBookCreateDto
    {
        public int BookId { get; set; }

        public decimal PurchaseAmountPerUnit { get; set; }

        public int PurchaseQuantity { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public string PurchaseFrom { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = "Cash";

        public string PaymentStatus { get; set; } = "Unpaid";
    }

    public class PurchaseBookUpdateDto : PurchaseBookCreateDto
    {
    }

    public class PurchaseBookViewDto
    {
        public int PurchaseId { get; set; }

        public int BookId { get; set; }

        public string? BookTitle { get; set; }

        public decimal PurchaseAmountPerUnit { get; set; }

        public int PurchaseQuantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string PurchaseFrom { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;
    }
}
