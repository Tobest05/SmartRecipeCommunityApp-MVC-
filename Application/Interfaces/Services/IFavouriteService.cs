using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IFavouriteService
    {
        Task<BaseResponse<FavouriteRecipeResponseModel>> AddFavouriteRecipeAsync(CreateFavouriteRecipeRequestModel request, Guid customerId);

        Task<BaseResponse<ICollection<FavouriteRecipeViewModel>>>GetFavouriteRecipesByCustomerAsync(Guid customerId);

        Task<BaseResponse<bool>> RemoveFavouriteRecipeAsync(Guid id, Guid customerId);
    }
}
