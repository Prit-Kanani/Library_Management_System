using Library_Management_System.DTOs.Books;
using Library_Management_System.DTOs.Categories;
using Library_Management_System.DTOs.Common;
using Library_Management_System.DTOs.IssuedBooks;
using Library_Management_System.DTOs.PurchaseBooks;
using Library_Management_System.DTOs.Users;

namespace Library_Management_System.Services.Interfaces
{
    public interface IUserService
    {
        #region Query

        Task<PagedResultDto<UserViewDto>> GetAllAsync(UserFilterDto filter);

        Task<UserViewDto?> GetByIdAsync(int id);

        Task<UserUpdateDto?> GetByIdForUpdateAsync(int id);

        Task<IReadOnlyList<DropdownDto>> GetDropdownAsync();

        #endregion

        #region Commands

        Task<UserViewDto> CreateAsync(UserCreateDto dto);

        Task<bool> UpdateAsync(int id, UserUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        #endregion
    }

    public interface ICategoryService
    {
        #region Query

        Task<PagedResultDto<CategoryViewDto>> GetAllAsync(CategoryFilterDto filter);

        Task<CategoryViewDto?> GetByIdAsync(int id);

        Task<CategoryUpdateDto?> GetByIdForUpdateAsync(int id);

        Task<IReadOnlyList<DropdownDto>> GetDropdownAsync();

        #endregion

        #region Commands

        Task<CategoryViewDto> CreateAsync(CategoryCreateDto dto);

        Task<bool> UpdateAsync(int id, CategoryUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        #endregion
    }

    public interface IBookService
    {
        #region Query

        Task<PagedResultDto<BookViewDto>> GetAllAsync(BookFilterDto filter);

        Task<BookViewDto?> GetByIdAsync(int id);

        Task<BookUpdateDto?> GetByIdForUpdateAsync(int id);

        Task<IReadOnlyList<DropdownDto>> GetDropdownAsync();

        #endregion

        #region Commands

        Task<BookViewDto> CreateAsync(BookCreateDto dto);

        Task<bool> UpdateAsync(int id, BookUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        #endregion
    }

    public interface IIssuedBookService
    {
        #region Query

        Task<PagedResultDto<IssuedBookViewDto>> GetAllAsync(IssuedBookFilterDto filter);

        Task<IssuedBookViewDto?> GetByIdAsync(int id);

        Task<IssuedBookUpdateDto?> GetByIdForUpdateAsync(int id);

        #endregion

        #region Commands

        Task<IssuedBookViewDto> CreateAsync(IssuedBookCreateDto dto);

        Task<bool> UpdateAsync(int id, IssuedBookUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        #endregion
    }

    public interface IPurchaseBookService
    {
        #region Query

        Task<PagedResultDto<PurchaseBookViewDto>> GetAllAsync(PurchaseBookFilterDto filter);

        Task<PurchaseBookViewDto?> GetByIdAsync(int id);

        Task<PurchaseBookUpdateDto?> GetByIdForUpdateAsync(int id);

        #endregion

        #region Commands

        Task<PurchaseBookViewDto> CreateAsync(PurchaseBookCreateDto dto);

        Task<bool> UpdateAsync(int id, PurchaseBookUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        #endregion
    }
}
