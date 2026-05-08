namespace Library_Management_System.DTOs.Users
{
    public class UserFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public string? Role { get; set; }

        public bool? IsActive { get; set; }

        public bool IncludeDeleted { get; set; }
    }

    public class UserCreateDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public bool IsActive { get; set; } = true;
    }

    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public bool IsActive { get; set; } = true;
    }

    public class UserViewDto
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
