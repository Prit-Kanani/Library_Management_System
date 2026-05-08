namespace Library_Management_System.Models
{
    public class PurchaseBook
    {
        public int PurchaseId { get; set; }

        public int BookId { get; set; }

        public Book? Book { get; set; }

        public decimal PurchaseAmountPerUnit { get; set; }

        public int PurchaseQuantity { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public string PurchaseFrom { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = "Cash";

        public string PaymentStatus { get; set; } = "Unpaid";
    }
}
