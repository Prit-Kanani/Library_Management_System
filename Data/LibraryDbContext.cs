using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Data
{
    public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Book> Books => Set<Book>();

        public DbSet<IssuedBook> IssuedBooks => Set<IssuedBook>();

        public DbSet<PurchaseBook> PurchaseBooks => Set<PurchaseBook>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_Users_Role",
                        "[Role] IN ('Admin', 'Librarian', 'User')");
                });

                entity.HasKey(user => user.UserId);

                entity.Property(user => user.FullName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(user => user.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(user => user.Password)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(user => user.Role)
                    .HasMaxLength(10)
                    .HasDefaultValue("User")
                    .IsRequired();

                entity.HasIndex(user => user.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(category => category.CategoryId);

                entity.Property(category => category.CategoryName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(category => category.CategoryName)
                    .IsUnique();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_Books_Price",
                        "[Price] >= 0");

                    table.HasCheckConstraint(
                        "CK_Books_TotalQuantity",
                        "[TotalQuantity] >= 0");

                    table.HasCheckConstraint(
                        "CK_Books_PenaltyAmount",
                        "[PenaltyAmount] >= 0");

                    table.HasCheckConstraint(
                        "CK_Books_AvailableQuantity",
                        "[AvailableQuantity] >= 0 AND [AvailableQuantity] <= [TotalQuantity]");
                });

                entity.HasKey(book => book.BookId);

                entity.Property(book => book.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(book => book.Description)
                    .HasMaxLength(500);

                entity.Property(book => book.Author)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(book => book.Price)
                    .HasPrecision(10, 2);

                entity.Property(book => book.PenaltyAmount)
                    .HasPrecision(10, 2);

                entity.HasOne(book => book.Category)
                    .WithMany(category => category.Books)
                    .HasForeignKey(book => book.CategoryId);
            });

            modelBuilder.Entity<IssuedBook>(entity =>
            {
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_IssuedBooks_CurrentPenaltyAmount",
                        "[CurrentPenaltyAmount] >= 0");

                    table.HasCheckConstraint(
                        "CK_IssuedBooks_Status",
                        "[Status] IN ('Issued', 'Returned', 'Penalized')");

                    table.HasCheckConstraint(
                        "CK_IssuedBooks_ReturnDate",
                        "[ReturnDate] >= [IssueDate]");
                });

                entity.HasKey(issuedBook => issuedBook.IssueId);

                entity.Property(issuedBook => issuedBook.IssuePurpose)
                    .HasMaxLength(200);

                entity.Property(issuedBook => issuedBook.IssueDate)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.Property(issuedBook => issuedBook.ReturnDate)
                    .HasDefaultValueSql("DATEADD(day, 14, SYSUTCDATETIME())");

                entity.Property(issuedBook => issuedBook.CurrentPenaltyAmount)
                    .HasPrecision(10, 2)
                    .HasDefaultValue(0);

                entity.Property(issuedBook => issuedBook.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Issued")
                    .IsRequired();

                entity.HasOne(issuedBook => issuedBook.Book)
                    .WithMany(book => book.IssuedBooks)
                    .HasForeignKey(issuedBook => issuedBook.BookId);

                entity.HasOne(issuedBook => issuedBook.User)
                    .WithMany(user => user.IssuedBooks)
                    .HasForeignKey(issuedBook => issuedBook.UserId);
            });

            modelBuilder.Entity<PurchaseBook>(entity =>
            {
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_PurchaseBooks_PurchaseAmountPerUnit",
                        "[PurchaseAmountPerUnit] >= 0");

                    table.HasCheckConstraint(
                        "CK_PurchaseBooks_PurchaseQuantity",
                        "[PurchaseQuantity] > 0");

                    table.HasCheckConstraint(
                        "CK_PurchaseBooks_PaymentMethod",
                        "[PaymentMethod] IN ('Cash', 'Card', 'Online')");

                    table.HasCheckConstraint(
                        "CK_PurchaseBooks_PaymentStatus",
                        "[PaymentStatus] IN ('Paid', 'Unpaid')");
                });

                entity.HasKey(purchaseBook => purchaseBook.PurchaseId);

                entity.Property(purchaseBook => purchaseBook.PurchaseAmountPerUnit)
                    .HasPrecision(10, 2);

                entity.Property(purchaseBook => purchaseBook.PurchaseDate)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.Property(purchaseBook => purchaseBook.PurchaseFrom)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(purchaseBook => purchaseBook.PaymentMethod)
                    .HasMaxLength(20)
                    .HasDefaultValue("Cash")
                    .IsRequired();

                entity.Property(purchaseBook => purchaseBook.PaymentStatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("Unpaid")
                    .IsRequired();

                entity.HasOne(purchaseBook => purchaseBook.Book)
                    .WithMany(book => book.PurchaseBooks)
                    .HasForeignKey(purchaseBook => purchaseBook.BookId);
            });
        }
    }
}
