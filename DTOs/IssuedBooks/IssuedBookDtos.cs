namespace Library_Management_System.DTOs.IssuedBooks
{
    public class IssuedBookFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int? BookId { get; set; }

        public int? UserId { get; set; }

        public string? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class IssuedBookCreateDto
    {
        public int BookId { get; set; }

        public int UserId { get; set; }

        public string? IssuePurpose { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public DateTime ReturnDate { get; set; } = DateTime.UtcNow.AddDays(14);

        public decimal CurrentPenaltyAmount { get; set; }

        public string Status { get; set; } = "Issued";
    }

    public class IssuedBookUpdateDto : IssuedBookCreateDto
    {
    }

    public class IssuedBookViewDto
    {
        public int IssueId { get; set; }

        public int BookId { get; set; }

        public string? BookTitle { get; set; }

        public int UserId { get; set; }

        public string? UserFullName { get; set; }

        public string? IssuePurpose { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal CurrentPenaltyAmount { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
