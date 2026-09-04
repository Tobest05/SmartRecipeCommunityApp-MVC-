using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Implementation
{
    public class RecipeLikeService : IRecipeLikeService
    {
        private readonly IRecipeLikeRepository _recipeLikeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeLikeService(
            IRecipeLikeRepository recipeLikeRepository,
            ICustomerRepository customerRepository,
            IRecipeRepository recipeRepository,
            IUnitOfWork unitOfWork)
        {
            _recipeLikeRepository = recipeLikeRepository;
            _customerRepository = customerRepository;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<RecipeLikeResponse>> LikeAsync( Guid userId, RecipeLikeRequest request)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return BaseResponse<RecipeLikeResponse>.Failure("Customer not found.");
            }

            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<RecipeLikeResponse>.Failure("Recipe not found.");
            }

            var existingLike = await _recipeLikeRepository.GetAsync(customer.Id,request.RecipeId);

            if (existingLike != null)
            {
                return BaseResponse<RecipeLikeResponse>.Failure("You already liked this recipe.");
            }

            var like = new RecipeLike
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                RecipeId = request.RecipeId,
                CreatedAt = DateTime.UtcNow
            };

            await _recipeLikeRepository.AddAsync(like);

            await _unitOfWork.SaveChangesAsync();

            var likeCount = await _recipeLikeRepository.GetLikeCountAsync(request.RecipeId);

            var response = new RecipeLikeResponse
            {
                RecipeId = request.RecipeId,
                IsLiked = true,
                LikeCount = likeCount
            };

            return BaseResponse<RecipeLikeResponse>.Success("Recipe liked successfully.",response);
        }


        public async Task<BaseResponse<RecipeLikeResponse>> UnlikeAsync(
            Guid userId,
            RecipeLikeRequest request)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return BaseResponse<RecipeLikeResponse>.Failure("Customer not found.");
            }

            var existingLike = await _recipeLikeRepository.GetAsync( customer.Id,request.RecipeId);

            if (existingLike == null)
            {
                return BaseResponse<RecipeLikeResponse>.Failure("You have not liked this recipe.");
            }

            _recipeLikeRepository.Remove(existingLike);

            await _unitOfWork.SaveChangesAsync();

            var likeCount = await _recipeLikeRepository.GetLikeCountAsync(request.RecipeId);

            var response = new RecipeLikeResponse
            {
                RecipeId = request.RecipeId,
                IsLiked = false,
                LikeCount = likeCount
            };

            return BaseResponse<RecipeLikeResponse>.Success("Recipe unliked successfully.",response);
        }


        public async Task<BaseResponse<RecipeLikeResponse>> GetLikeAsync(Guid userId,Guid recipeId)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return BaseResponse<RecipeLikeResponse>.Failure("Customer not found.");
            }

            var like = await _recipeLikeRepository.GetAsync(customer.Id,recipeId);

            if (like == null)
            {

                return BaseResponse<RecipeLikeResponse>.Failure("Like not found.");
            }

            var response = like.Adapt<RecipeLikeResponse>();

            return BaseResponse<RecipeLikeResponse>.Success("Like retrieved successfully.",response);
        }
    }
}
