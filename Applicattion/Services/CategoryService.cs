using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CategoryResponseModel>> AddCategoryAsync(CreateCategoryRequestModel request)
        {
            var exist = await _categoryRepository.IsExistAsync(request.Name);

            if (exist == true)
            {
                return BaseResponse<CategoryResponseModel>
                    .Failure("Category already exists.");
            }

            var category = request.Adapt<Category>();

            category.Id = Guid.NewGuid();

            await _categoryRepository.AddCategoryAsync(category);

            await _unitOfWork.SaveChangesAsync();

            var response = category.Adapt<CategoryResponseModel>();

            return BaseResponse<CategoryResponseModel>
                .Success("Category created successfully.", response);
        }

        public async Task<BaseResponse<CategoryResponseModel>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return BaseResponse<CategoryResponseModel>
                    .Failure("Category not found.");
            }

            var response = category.Adapt<CategoryResponseModel>();

            return BaseResponse<CategoryResponseModel>
                .Success("Category retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CategoryResponseModel>>> GetAllCategoryAsync()
        {
            var categories = await _categoryRepository.GetAllCategoryAsync();

            var response = categories.Adapt<ICollection<CategoryResponseModel>>();

            return BaseResponse<ICollection<CategoryResponseModel>>
                .Success("Categories retrieved successfully.", response);
        }

        public async Task<BaseResponse<UpdateCategoryResponseModel>> UpdateCategoryAsync(UpdateCategoryRequestModel request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                return BaseResponse<UpdateCategoryResponseModel>
                    .Failure("Category not found.");
            }

            request.Adapt(category);

            _categoryRepository.UpdateCategory(category);

            await _unitOfWork.SaveChangesAsync();

            var response = category.Adapt<UpdateCategoryResponseModel>();

            return BaseResponse<UpdateCategoryResponseModel>
                .Success("Category updated successfully.", response);
        }

        public async Task<BaseResponse<bool>> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return BaseResponse<bool>
                    .Failure("Category not found.");
            }

            _categoryRepository.DeleteCategory(category);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success("Category deleted successfully.", true);
        }
    }
}
