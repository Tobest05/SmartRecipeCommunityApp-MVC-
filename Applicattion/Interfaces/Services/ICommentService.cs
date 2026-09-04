using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{

    public interface ICommentService
    {
        Task<BaseResponse<CreateRecipeCommentResponseModel>>
            AddCommentAsync(
                CreateRecipeCommentRequestModel request,
                Guid customerId);


        Task<BaseResponse<CreateRecipeCommentResponseModel>>
            GetCommentByIdAsync(Guid id);


        Task<BaseResponse<ICollection<CreateRecipeCommentResponseModel>>>
            GetAllCommentAsync();


        Task<BaseResponse<ICollection<MyCommentViewModel>>>
            GetCommentsByCustomerAsync(Guid customerId);


        Task<BaseResponse<UpdateRecipeCommentResponseModel>>
            UpdateCommentAsync(
                Guid id,
                UpdateRecipeCommentRequestModel request,
                Guid customerId);


        Task<BaseResponse<bool>>
            DeleteCommentAsync(
                Guid id,
                Guid customerId);
    }
}
