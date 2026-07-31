using Application.Dto;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IRecipeService
    {
        Task<BaseResponse<CreateRecipeResponseModel>> AddRecipeAsync(CreateRecipeRequestModel request);

        Task<BaseResponse<CreateRecipeResponseModel>> GetRecipeByIdAsync(Guid id);

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetAllRecipeAsync();

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetPublishedRecipeAsync();

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCustomerAsync(Guid customerId);

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> SearchRecipeAsync(string name);

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCategoryAsync(Guid categoryId);

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByDifficultyAsync(Difficulty difficulty);

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByStatusAsync(RecipeStatus status);

        Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCookingTimeAsync(int cookingTime);

        Task<BaseResponse<UpdateRecipeResponseModel>> UpdateRecipeAsync(UpdateRecipeRequestModel request);

        Task<BaseResponse<bool>> DeleteRecipeAsync(Guid id);
    }
}
