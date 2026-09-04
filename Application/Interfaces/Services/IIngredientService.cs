using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<BaseResponse<CreateIngredientResponseModel>> AddIngredientAsync(CreateIngredientRequestModel request);

        Task<BaseResponse<CreateIngredientResponseModel>> GetIngredientByIdAsync(Guid id);

        Task<BaseResponse<ICollection<CreateIngredientResponseModel>>> GetAllIngredientAsync();

        Task<BaseResponse<UpdateIngredientResponseModel>> UpdateIngredientAsync(UpdateIngredientRequestModel request);

        Task<BaseResponse<bool>> DeleteIngredientAsync(Guid id);
    }
}
