using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class RecipeCommentService : ICommentService
    {
        private readonly ICommentRepository _recipeCommentRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeCommentService(
           ICommentRepository recipeCommentRepository,
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _recipeCommentRepository = recipeCommentRepository;
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateRecipeCommentResponseModel>> AddCommentAsync(CreateRecipeCommentRequestModel request, Guid customerId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<CreateRecipeCommentResponseModel>.Failure("Recipe not found.");
            }

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<CreateRecipeCommentResponseModel>.Failure("Customer not found.");
            }

            var recipeComment = request.Adapt<RecipeComment>();
            recipeComment.CustomerId = customer.Id;

            await _recipeCommentRepository.AddRecipeCommentAsync(recipeComment);
            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<CreateRecipeCommentResponseModel>.Success(
                "Comment added successfully.",
                recipeComment.Adapt<CreateRecipeCommentResponseModel>());
        }

        public async Task<BaseResponse<CreateRecipeCommentResponseModel>> GetCommentByIdAsync(Guid id)
        {
            var recipeComment = await _recipeCommentRepository.GetRecipeCommentByIdAsync(id);

            if (recipeComment == null)
            {
                return BaseResponse<CreateRecipeCommentResponseModel>.Failure("Comment not found.");
            }

            return BaseResponse<CreateRecipeCommentResponseModel>.Success(
                "Comment retrieved successfully.",
                recipeComment.Adapt<CreateRecipeCommentResponseModel>());
        }

        public async Task<BaseResponse<ICollection<CreateRecipeCommentResponseModel>>> GetAllCommentAsync()
        {
            var comments = await _recipeCommentRepository.GetAllRecipeCommentAsync();

            return BaseResponse<ICollection<CreateRecipeCommentResponseModel>>.Success(
                "Comments retrieved successfully.",
                comments.Adapt<ICollection<CreateRecipeCommentResponseModel>>());
        }

        public async Task<BaseResponse<UpdateRecipeCommentResponseModel>> UpdateCommentAsync(Guid id, UpdateRecipeCommentRequestModel request)
        {
            var recipeComment = await _recipeCommentRepository.GetRecipeCommentByIdAsync(id);

            if (recipeComment == null)
            {
                return BaseResponse<UpdateRecipeCommentResponseModel>.Failure("Comment not found.");
            }

            recipeComment.Comment = request.Comment;

            _recipeCommentRepository.UpdateRecipeComment(recipeComment);
            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<UpdateRecipeCommentResponseModel>.Success(
                "Comment updated successfully.",
                recipeComment.Adapt<UpdateRecipeCommentResponseModel>());
        }

        public async Task<BaseResponse<bool>> DeleteCommentAsync(Guid id)
        {
            var recipeComment = await _recipeCommentRepository.GetRecipeCommentByIdAsync(id);

            if (recipeComment == null)
            {
                return BaseResponse<bool>.Failure("Comment not found.");
            }

            _recipeCommentRepository.DeleteRecipeComment(recipeComment);
            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>.Success("Comment deleted successfully.", true);
        }
    }
}