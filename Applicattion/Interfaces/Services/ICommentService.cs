using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
   
    public interface ICommentService
    {
        Task<BaseResponse<CreateRecipeCommentResponseModel>> AddCommentAsync(CreateRecipeCommentRequestModel request, Guid customerId);

        Task<BaseResponse<ICollection<CreateRecipeCommentResponseModel>>> GetAllCommentAsync();
        Task<BaseResponse<CreateRecipeCommentResponseModel>> GetCommentByIdAsync(Guid id);
        Task<BaseResponse<UpdateRecipeCommentResponseModel>> UpdateCommentAsync(Guid id, UpdateRecipeCommentRequestModel request);
        Task<BaseResponse<bool>> DeleteCommentAsync(Guid id);
    }
}
