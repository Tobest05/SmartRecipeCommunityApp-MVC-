using Application.Dto;
namespace Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<BaseResponse<CategoryResponseModel>> AddCategoryAsync(CreateCategoryRequestModel request);

        Task<BaseResponse<CategoryResponseModel>> GetCategoryByIdAsync(Guid id);

        Task<BaseResponse<ICollection<CategoryResponseModel>>> GetAllCategoryAsync();

        Task<BaseResponse<UpdateCategoryResponseModel>> UpdateCategoryAsync(UpdateCategoryRequestModel request);

        Task<BaseResponse<bool>> DeleteCategoryAsync(Guid id);
    }
}
