using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IFavouriteService
    {
        Task<BaseResponse<CreateFavouriteRecipeResponseModel>> AddFavouriteRecipeAsync(CreateFavouriteRecipeRequestModel request, Guid customerId);

        Task<BaseResponse<ICollection<CreateFavouriteRecipeResponseModel>>> GetAllFavouriteRecipeAsync();

        Task<BaseResponse<bool>> RemoveFavouriteRecipeAsync(Guid id);
    }
}
