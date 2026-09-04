using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IRecipeLikeService
    {
        Task<BaseResponse<RecipeLikeResponse>> LikeAsync(Guid userId, RecipeLikeRequest request);

        Task<BaseResponse<RecipeLikeResponse>> UnlikeAsync(Guid userId, RecipeLikeRequest request);

        Task<BaseResponse<RecipeLikeResponse>> GetLikeAsync(Guid userId,Guid recipeId);
    }
}
