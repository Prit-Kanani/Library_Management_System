using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            #region Categories
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });
            #endregion

            #region Users
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "User"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.CheckConstraint("CK_Users_Role", "[Role] IN ('Admin', 'Librarian', 'User')");
                });

            #endregion

            #region Books
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.CheckConstraint("CK_Books_AvailableQuantity", "[AvailableQuantity] >= 0 AND [AvailableQuantity] <= [TotalQuantity]");
                    table.CheckConstraint("CK_Books_PenaltyAmount", "[PenaltyAmount] >= 0");
                    table.CheckConstraint("CK_Books_Price", "[Price] >= 0");
                    table.CheckConstraint("CK_Books_TotalQuantity", "[TotalQuantity] >= 0");
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.NoAction);
                });
            #endregion

            #region IssuedBooks
            migrationBuilder.CreateTable(
                name: "IssuedBooks",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IssuePurpose = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "DATEADD(day, 14, SYSUTCDATETIME())"),
                    CurrentPenaltyAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Issued")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuedBooks", x => x.IssueId);
                    table.CheckConstraint("CK_IssuedBooks_CurrentPenaltyAmount", "[CurrentPenaltyAmount] >= 0");
                    table.CheckConstraint("CK_IssuedBooks_ReturnDate", "[ReturnDate] >= [IssueDate]");
                    table.CheckConstraint("CK_IssuedBooks_Status", "[Status] IN ('Issued', 'Returned', 'Penalized')");
                    table.ForeignKey(
                        name: "FK_IssuedBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_IssuedBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });
            #endregion

            #region PurchaseBooks
            migrationBuilder.CreateTable(
                name: "PurchaseBooks",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    PurchaseAmountPerUnit = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PurchaseQuantity = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    PurchaseFrom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Cash"),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Unpaid")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseBooks", x => x.PurchaseId);
                    table.CheckConstraint("CK_PurchaseBooks_PaymentMethod", "[PaymentMethod] IN ('Cash', 'Card', 'Online')");
                    table.CheckConstraint("CK_PurchaseBooks_PaymentStatus", "[PaymentStatus] IN ('Paid', 'Unpaid')");
                    table.CheckConstraint("CK_PurchaseBooks_PurchaseAmountPerUnit", "[PurchaseAmountPerUnit] >= 0");
                    table.CheckConstraint("CK_PurchaseBooks_PurchaseQuantity", "[PurchaseQuantity] > 0");
                    table.ForeignKey(
                        name: "FK_PurchaseBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.NoAction);
                });
            #endregion

            #region Indexes
            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssuedBooks_BookId",
                table: "IssuedBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedBooks_UserId",
                table: "IssuedBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseBooks_BookId",
                table: "PurchaseBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuedBooks");

            migrationBuilder.DropTable(
                name: "PurchaseBooks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
