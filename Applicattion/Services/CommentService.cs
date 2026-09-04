using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;


        public CommentService(
            ICommentRepository commentRepository,
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }


        // =========================================================
        // CREATE COMMENT
        // =========================================================

        public async Task<BaseResponse<CreateRecipeCommentResponseModel>>
            AddCommentAsync(
                CreateRecipeCommentRequestModel request,
                Guid customerId)
        {
            var recipe =
                await _recipeRepository.GetByIdAsync(
                    request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<CreateRecipeCommentResponseModel>
                    .Failure("Recipe not found.");
            }


            var customer =
                await _customerRepository.GetByIdAsync(
                    customerId);

            if (customer == null)
            {
                return BaseResponse<CreateRecipeCommentResponseModel>
                    .Failure("Customer not found.");
            }


            if (string.IsNullOrWhiteSpace(request.Comment))
            {
                return BaseResponse<CreateRecipeCommentResponseModel>
                    .Failure("Comment cannot be empty.");
            }


            var comment = new RecipeComment
            {
                Id = Guid.NewGuid(),

                CustomerId = customerId,

                RecipeId = request.RecipeId,

                Comment = request.Comment
            };


            await _commentRepository
                .AddRecipeCommentAsync(comment);


            await _unitOfWork.SaveChangesAsync();


            var response = new CreateRecipeCommentResponseModel
            {
                Id = comment.Id,

                RecipeId = recipe.Id,

                RecipeName = recipe.Name,

                Comment = comment.Comment,

                CustomerName =
                    $"{customer.FirstName} {customer.LastName}",

                CreatedAt = comment.CreatedBy
            };


            return BaseResponse<CreateRecipeCommentResponseModel>
                .Success(
                    "Comment added successfully.",
                    response);
        }


        // =========================================================
        // GET COMMENT BY ID
        // =========================================================

        public async Task<BaseResponse<CreateRecipeCommentResponseModel>>
            GetCommentByIdAsync(Guid id)
        {
            var comment =
                await _commentRepository
                    .GetRecipeCommentByIdAsync(id);


            if (comment == null)
            {
                return BaseResponse<CreateRecipeCommentResponseModel>
                    .Failure("Comment not found.");
            }


            var response = new CreateRecipeCommentResponseModel
            {
                Id = comment.Id,

                RecipeId = comment.RecipeId,

                RecipeName =
                    comment.Recipe?.Name ?? "Unknown Recipe",

                Comment = comment.Comment,

                CustomerName =
                    comment.Customer == null
                        ? "Unknown"
                        : $"{comment.Customer.FirstName} {comment.Customer.LastName}",

                CreatedAt = comment.CreatedBy
            };


            return BaseResponse<CreateRecipeCommentResponseModel>
                .Success(
                    "Comment retrieved successfully.",
                    response);
        }


        // =========================================================
        // GET ALL COMMENTS
        // =========================================================

        public async Task<BaseResponse<
            ICollection<CreateRecipeCommentResponseModel>>>
            GetAllCommentAsync()
        {
            var comments =
                await _commentRepository
                    .GetAllRecipeCommentAsync();


            var response =
                comments.Select(comment =>
                    new CreateRecipeCommentResponseModel
                    {
                        Id = comment.Id,

                        RecipeId = comment.RecipeId,

                        RecipeName =
                            comment.Recipe?.Name
                            ?? "Unknown Recipe",

                        Comment = comment.Comment,

                        CustomerName =
                            comment.Customer == null
                                ? "Unknown"
                                : $"{comment.Customer.FirstName} {comment.Customer.LastName}",

                        CreatedAt = comment.CreatedBy

                    }).ToList();


            return BaseResponse<
                ICollection<CreateRecipeCommentResponseModel>>
                .Success(
                    "Comments retrieved successfully.",
                    response);
        }

        public async Task<BaseResponse<ICollection<MyCommentViewModel>>>GetCommentsByCustomerAsync(Guid customerId)
        {
            var customer =
                await _customerRepository
                    .GetByIdAsync(customerId);


            if (customer == null)
            {
                return BaseResponse<ICollection<MyCommentViewModel>>
                    .Failure("Customer not found.");
            }


            var comments =
                await _commentRepository
                    .GetRecipeCommentByCustomerIdAsync(
                        customerId);


            var response =
                comments.Select(comment =>
                    new MyCommentViewModel
                    {
                        Id = comment.Id,

                        RecipeId = comment.RecipeId,

                        RecipeName =
                            comment.Recipe?.Name
                            ?? "Unknown Recipe",

                        Comment = comment.Comment,

                        CreatedAt = comment.CreatedBy

                    }).ToList();


            return BaseResponse<ICollection<MyCommentViewModel>>
                .Success(
                    "Your comments retrieved successfully.",
                    response);
        }


        // =========================================================
        // UPDATE COMMENT
        // =========================================================

        public async Task<BaseResponse<UpdateRecipeCommentResponseModel>>
            UpdateCommentAsync(
                Guid id,
                UpdateRecipeCommentRequestModel request,
                Guid customerId)
        {
            var comment =
                await _commentRepository
                    .GetRecipeCommentByIdAsync(id);


            if (comment == null)
            {
                return BaseResponse<UpdateRecipeCommentResponseModel>
                    .Failure("Comment not found.");
            }


            // IMPORTANT:
            // Customer can only edit their own comment.

            if (comment.CustomerId != customerId)
            {
                return BaseResponse<UpdateRecipeCommentResponseModel>
                    .Failure(
                        "You can only edit your own comment.");
            }


            if (string.IsNullOrWhiteSpace(request.Comment))
            {
                return BaseResponse<UpdateRecipeCommentResponseModel>
                    .Failure("Comment cannot be empty.");
            }


            comment.Comment = request.Comment;


            _commentRepository
                .UpdateRecipeComment(comment);


            await _unitOfWork.SaveChangesAsync();


            var response = new UpdateRecipeCommentResponseModel
            {
                Id = comment.Id,

                RecipeId = comment.RecipeId,

                Comment = comment.Comment
            };


            return BaseResponse<UpdateRecipeCommentResponseModel>
                .Success(
                    "Comment updated successfully.",
                    response);
        }


        // =========================================================
        // DELETE COMMENT
        // =========================================================

        public async Task<BaseResponse<bool>>
            DeleteCommentAsync(
                Guid id,
                Guid customerId)
        {
            var comment =
                await _commentRepository
                    .GetRecipeCommentByIdAsync(id);


            if (comment == null)
            {
                return BaseResponse<bool>
                    .Failure("Comment not found.");
            }


            if (comment.CustomerId != customerId)
            {
                return BaseResponse<bool>
                    .Failure(
                        "You can only delete your own comment.");
            }


            _commentRepository
                .DeleteRecipeComment(comment);


            await _unitOfWork.SaveChangesAsync();


            return BaseResponse<bool>
                .Success(
                    "Comment deleted successfully.",
                    true);
        }
    }
}