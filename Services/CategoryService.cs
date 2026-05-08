using Library_Management_System.DTOs.Categories;
using Library_Management_System.DTOs.Common;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class CategoryService(IRepository<Category> categories) : ICategoryService
    {
        #region Query

        public async Task<PagedResultDto<CategoryViewDto>> GetAllAsync(CategoryFilterDto filter)
        {
            var query = categories.Query().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(category => category.CategoryName.Contains(filter.Search));
            }

            return await query
                .OrderBy(category => category.CategoryName)
                .Select(category => ToViewDto(category))
                .ToPagedResultAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<CategoryViewDto?> GetByIdAsync(int id)
        {
            return await categories.Query()
                .AsNoTracking()
                .Where(category => category.CategoryId == id)
                .Select(category => ToViewDto(category))
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            return await categories.Query()
                .AsNoTracking()
                .Where(category => category.CategoryId == id)
                .Select(category => new CategoryUpdateDto
                {
                    CategoryName = category.CategoryName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<DropdownDto>> GetDropdownAsync()
        {
            return await categories.Query()
                .AsNoTracking()
                .OrderBy(category => category.CategoryName)
                .Select(category => new DropdownDto
                {
                    Id = category.CategoryId,
                    Text = category.CategoryName
                })
                .ToListAsync();
        }

        #endregion

        #region Commands

        public async Task<CategoryViewDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName
            };

            await categories.AddAsync(category);
            await categories.SaveChangesAsync();

            return ToViewDto(category);
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var category = await categories.GetByIdAsync(id);

            if (category is null)
            {
                return false;
            }

            category.CategoryName = dto.CategoryName;
            categories.Update(category);
            await categories.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await categories.GetByIdAsync(id);

            if (category is null)
            {
                return false;
            }

            categories.Delete(category);
            await categories.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Mapping

        private static CategoryViewDto ToViewDto(Category category)
        {
            return new CategoryViewDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        #endregion
    }
}
