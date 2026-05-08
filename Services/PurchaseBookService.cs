using Library_Management_System.DTOs.Common;
using Library_Management_System.DTOs.PurchaseBooks;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class PurchaseBookService(IRepository<PurchaseBook> purchaseBooks) : IPurchaseBookService
    {
        #region Query

        public async Task<PagedResultDto<PurchaseBookViewDto>> GetAllAsync(PurchaseBookFilterDto filter)
        {
            var query = purchaseBooks.Query()
                .AsNoTracking()
                .Include(purchaseBook => purchaseBook.Book)
                .AsQueryable();

            if (filter.BookId.HasValue)
            {
                query = query.Where(purchaseBook => purchaseBook.BookId == filter.BookId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.PurchaseFrom))
            {
                query = query.Where(purchaseBook => purchaseBook.PurchaseFrom.Contains(filter.PurchaseFrom));
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
            {
                query = query.Where(purchaseBook => purchaseBook.PaymentMethod == filter.PaymentMethod);
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
            {
                query = query.Where(purchaseBook => purchaseBook.PaymentStatus == filter.PaymentStatus);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(purchaseBook => purchaseBook.PurchaseDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(purchaseBook => purchaseBook.PurchaseDate <= filter.ToDate.Value);
            }

            return await query
                .OrderByDescending(purchaseBook => purchaseBook.PurchaseDate)
                .Select(purchaseBook => ToViewDto(purchaseBook))
                .ToPagedResultAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PurchaseBookViewDto?> GetByIdAsync(int id)
        {
            return await purchaseBooks.Query()
                .AsNoTracking()
                .Include(purchaseBook => purchaseBook.Book)
                .Where(purchaseBook => purchaseBook.PurchaseId == id)
                .Select(purchaseBook => ToViewDto(purchaseBook))
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseBookUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            return await purchaseBooks.Query()
                .AsNoTracking()
                .Where(purchaseBook => purchaseBook.PurchaseId == id)
                .Select(purchaseBook => new PurchaseBookUpdateDto
                {
                    BookId = purchaseBook.BookId,
                    PurchaseAmountPerUnit = purchaseBook.PurchaseAmountPerUnit,
                    PurchaseQuantity = purchaseBook.PurchaseQuantity,
                    PurchaseDate = purchaseBook.PurchaseDate,
                    PurchaseFrom = purchaseBook.PurchaseFrom,
                    PaymentMethod = purchaseBook.PaymentMethod,
                    PaymentStatus = purchaseBook.PaymentStatus
                })
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Commands

        public async Task<PurchaseBookViewDto> CreateAsync(PurchaseBookCreateDto dto)
        {
            var purchaseBook = new PurchaseBook
            {
                BookId = dto.BookId,
                PurchaseAmountPerUnit = dto.PurchaseAmountPerUnit,
                PurchaseQuantity = dto.PurchaseQuantity,
                PurchaseDate = dto.PurchaseDate,
                PurchaseFrom = dto.PurchaseFrom,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentStatus
            };

            await purchaseBooks.AddAsync(purchaseBook);
            await purchaseBooks.SaveChangesAsync();

            return await GetByIdAsync(purchaseBook.PurchaseId) ?? ToViewDto(purchaseBook);
        }

        public async Task<bool> UpdateAsync(int id, PurchaseBookUpdateDto dto)
        {
            var purchaseBook = await purchaseBooks.GetByIdAsync(id);

            if (purchaseBook is null)
            {
                return false;
            }

            purchaseBook.BookId = dto.BookId;
            purchaseBook.PurchaseAmountPerUnit = dto.PurchaseAmountPerUnit;
            purchaseBook.PurchaseQuantity = dto.PurchaseQuantity;
            purchaseBook.PurchaseDate = dto.PurchaseDate;
            purchaseBook.PurchaseFrom = dto.PurchaseFrom;
            purchaseBook.PaymentMethod = dto.PaymentMethod;
            purchaseBook.PaymentStatus = dto.PaymentStatus;

            purchaseBooks.Update(purchaseBook);
            await purchaseBooks.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var purchaseBook = await purchaseBooks.GetByIdAsync(id);

            if (purchaseBook is null)
            {
                return false;
            }

            purchaseBooks.Delete(purchaseBook);
            await purchaseBooks.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Mapping

        private static PurchaseBookViewDto ToViewDto(PurchaseBook purchaseBook)
        {
            return new PurchaseBookViewDto
            {
                PurchaseId = purchaseBook.PurchaseId,
                BookId = purchaseBook.BookId,
                BookTitle = purchaseBook.Book?.Title,
                PurchaseAmountPerUnit = purchaseBook.PurchaseAmountPerUnit,
                PurchaseQuantity = purchaseBook.PurchaseQuantity,
                PurchaseDate = purchaseBook.PurchaseDate,
                PurchaseFrom = purchaseBook.PurchaseFrom,
                PaymentMethod = purchaseBook.PaymentMethod,
                PaymentStatus = purchaseBook.PaymentStatus
            };
        }

        #endregion
    }
}
