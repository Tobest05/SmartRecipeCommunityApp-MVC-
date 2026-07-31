using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IRatingService
    {
        Task<BaseResponse<CreateRecipeRatingResponseModel>> AddRatingAsync(CreateRecipeRatingRequestModel request, Guid customerId);

        Task<BaseResponse<UpdateRecipeRatingResponseModel>> UpdateRatingAsync(Guid customerId, UpdateRecipeRatingRequestModel request);

        Task<BaseResponse<double>> GetAverageRatingAsync(Guid recipeId);

        Task<BaseResponse<bool>> DeleteRatingAsync(Guid recipeId, Guid customerId);
    }
}
