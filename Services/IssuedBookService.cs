using Library_Management_System.DTOs.IssuedBooks;
using Library_Management_System.DTOs.Common;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class IssuedBookService(IRepository<IssuedBook> issuedBooks) : IIssuedBookService
    {
        #region Query

        public async Task<PagedResultDto<IssuedBookViewDto>> GetAllAsync(IssuedBookFilterDto filter)
        {
            var query = issuedBooks.Query()
                .AsNoTracking()
                .Include(issuedBook => issuedBook.Book)
                .Include(issuedBook => issuedBook.User)
                .AsQueryable();

            if (filter.BookId.HasValue)
            {
                query = query.Where(issuedBook => issuedBook.BookId == filter.BookId.Value);
            }

            if (filter.UserId.HasValue)
            {
                query = query.Where(issuedBook => issuedBook.UserId == filter.UserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(issuedBook => issuedBook.Status == filter.Status);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(issuedBook => issuedBook.IssueDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(issuedBook => issuedBook.IssueDate <= filter.ToDate.Value);
            }

            return await query
                .OrderByDescending(issuedBook => issuedBook.IssueDate)
                .Select(issuedBook => ToViewDto(issuedBook))
                .ToPagedResultAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IssuedBookViewDto?> GetByIdAsync(int id)
        {
            return await issuedBooks.Query()
                .AsNoTracking()
                .Include(issuedBook => issuedBook.Book)
                .Include(issuedBook => issuedBook.User)
                .Where(issuedBook => issuedBook.IssueId == id)
                .Select(issuedBook => ToViewDto(issuedBook))
                .FirstOrDefaultAsync();
        }

        public async Task<IssuedBookUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            return await issuedBooks.Query()
                .AsNoTracking()
                .Where(issuedBook => issuedBook.IssueId == id)
                .Select(issuedBook => new IssuedBookUpdateDto
                {
                    BookId = issuedBook.BookId,
                    UserId = issuedBook.UserId,
                    IssuePurpose = issuedBook.IssuePurpose,
                    IssueDate = issuedBook.IssueDate,
                    ReturnDate = issuedBook.ReturnDate,
                    CurrentPenaltyAmount = issuedBook.CurrentPenaltyAmount,
                    Status = issuedBook.Status
                })
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Commands

        public async Task<IssuedBookViewDto> CreateAsync(IssuedBookCreateDto dto)
        {
            var issuedBook = new IssuedBook
            {
                BookId = dto.BookId,
                UserId = dto.UserId,
                IssuePurpose = dto.IssuePurpose,
                IssueDate = dto.IssueDate,
                ReturnDate = dto.ReturnDate,
                CurrentPenaltyAmount = dto.CurrentPenaltyAmount,
                Status = dto.Status
            };

            await issuedBooks.AddAsync(issuedBook);
            await issuedBooks.SaveChangesAsync();

            return await GetByIdAsync(issuedBook.IssueId) ?? ToViewDto(issuedBook);
        }

        public async Task<bool> UpdateAsync(int id, IssuedBookUpdateDto dto)
        {
            var issuedBook = await issuedBooks.GetByIdAsync(id);

            if (issuedBook is null)
            {
                return false;
            }

            issuedBook.BookId = dto.BookId;
            issuedBook.UserId = dto.UserId;
            issuedBook.IssuePurpose = dto.IssuePurpose;
            issuedBook.IssueDate = dto.IssueDate;
            issuedBook.ReturnDate = dto.ReturnDate;
            issuedBook.CurrentPenaltyAmount = dto.CurrentPenaltyAmount;
            issuedBook.Status = dto.Status;

            issuedBooks.Update(issuedBook);
            await issuedBooks.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var issuedBook = await issuedBooks.GetByIdAsync(id);

            if (issuedBook is null)
            {
                return false;
            }

            issuedBooks.Delete(issuedBook);
            await issuedBooks.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Mapping

        private static IssuedBookViewDto ToViewDto(IssuedBook issuedBook)
        {
            return new IssuedBookViewDto
            {
                IssueId = issuedBook.IssueId,
                BookId = issuedBook.BookId,
                BookTitle = issuedBook.Book?.Title,
                UserId = issuedBook.UserId,
                UserFullName = issuedBook.User?.FullName,
                IssuePurpose = issuedBook.IssuePurpose,
                IssueDate = issuedBook.IssueDate,
                ReturnDate = issuedBook.ReturnDate,
                CurrentPenaltyAmount = issuedBook.CurrentPenaltyAmount,
                Status = issuedBook.Status
            };
        }

        #endregion
    }
}
