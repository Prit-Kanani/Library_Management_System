using Library_Management_System.DTOs.Books;
using Library_Management_System.DTOs.Common;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services;

public class BookService(IRepository<Book> books) : IBookService
{
    #region Query

    public async Task<PagedResultDto<BookViewDto>> GetAllAsync(BookFilterDto filter)
    {
        var query = books.Query()
            .AsNoTracking()
            .Include(book => book.Category)
            .AsQueryable();

        if (!filter.IncludeRemoved)
        {
            query = query.Where(book => !book.IsRemoved);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(book =>
                book.Title.Contains(filter.Search) ||
                (book.Description != null && book.Description.Contains(filter.Search)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Author))
        {
            query = query.Where(book => book.Author.Contains(filter.Author));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(book => book.CategoryId == filter.CategoryId.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(book => book.IsActive == filter.IsActive.Value);
        }

        return await query
            .OrderBy(book => book.Title)
            .Select(book => ToViewDto(book))
            .ToPagedResultAsync(filter.PageNumber, filter.PageSize);
    }

    public async Task<BookViewDto?> GetByIdAsync(int id)
    {
        return await books.Query()
            .AsNoTracking()
            .Include(book => book.Category)
            .Where(book => book.BookId == id)
            .Select(book => ToViewDto(book))
            .FirstOrDefaultAsync();
    }

    public async Task<BookUpdateDto?> GetByIdForUpdateAsync(int id)
    {
        return await books.Query()
            .AsNoTracking()
            .Where(book => book.BookId == id)
            .Select(book => new BookUpdateDto
            {
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Price = book.Price,
                CategoryId = book.CategoryId,
                TotalQuantity = book.TotalQuantity,
                PenaltyAmount = book.PenaltyAmount,
                AvailableQuantity = book.AvailableQuantity,
                IsActive = book.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<DropdownDto>> GetDropdownAsync()
    {
        return await books.Query()
            .AsNoTracking()
            .Where(book => book.IsActive && !book.IsRemoved)
            .OrderBy(book => book.Title)
            .Select(book => new DropdownDto
            {
                Id = book.BookId,
                Text = book.Title
            })
            .ToListAsync();
    }

    #endregion

    #region Commands

    public async Task<BookViewDto> CreateAsync(BookCreateDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Description = dto.Description,
            Author = dto.Author,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            TotalQuantity = dto.TotalQuantity,
            PenaltyAmount = dto.PenaltyAmount,
            AvailableQuantity = dto.AvailableQuantity,
            IsActive = dto.IsActive
        };

        await books.AddAsync(book);
        await books.SaveChangesAsync();

        return await GetByIdAsync(book.BookId) ?? ToViewDto(book);
    }

    public async Task<bool> UpdateAsync(int id, BookUpdateDto dto)
    {
        var book = await books.GetByIdAsync(id);

        if (book is null || book.IsRemoved)
        {
            return false;
        }

        book.Title = dto.Title;
        book.Description = dto.Description;
        book.Author = dto.Author;
        book.Price = dto.Price;
        book.CategoryId = dto.CategoryId;
        book.TotalQuantity = dto.TotalQuantity;
        book.PenaltyAmount = dto.PenaltyAmount;
        book.AvailableQuantity = dto.AvailableQuantity;
        book.IsActive = dto.IsActive;

        books.Update(book);
        await books.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await books.GetByIdAsync(id);

        if (book is null || book.IsRemoved)
        {
            return false;
        }

        book.IsRemoved = true;
        book.IsActive = false;
        books.Update(book);
        await books.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Mapping

    private static BookViewDto ToViewDto(Book book)
    {
        return new BookViewDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Description = book.Description,
            Author = book.Author,
            Price = book.Price,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.CategoryName,
            TotalQuantity = book.TotalQuantity,
            PenaltyAmount = book.PenaltyAmount,
            AvailableQuantity = book.AvailableQuantity,
            IsActive = book.IsActive,
            IsRemoved = book.IsRemoved
        };
    }

    #endregion
}
