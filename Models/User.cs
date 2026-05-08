namespace Library_Management_System.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public ICollection<IssuedBook> IssuedBooks { get; set; } = [];
    }
}
