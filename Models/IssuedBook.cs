namespace Library_Management_System.Models
{
    public class IssuedBook
    {
        public int IssueId { get; set; }

        public int BookId { get; set; }

        public Book? Book { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public string? IssuePurpose { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public DateTime ReturnDate { get; set; }

        public decimal CurrentPenaltyAmount { get; set; }

        public string Status { get; set; } = "Issued";
    }
}
