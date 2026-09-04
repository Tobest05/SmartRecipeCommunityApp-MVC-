using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class RecipeRatingService : IRatingService
    {
        private readonly IRatingRepository _recipeRatingRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeRatingService(
            IRatingRepository recipeRatingRepository,
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _recipeRatingRepository = recipeRatingRepository;
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateRecipeRatingResponseModel>> AddRatingAsync(CreateRecipeRatingRequestModel request , Guid customerId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<CreateRecipeRatingResponseModel>.Failure("Recipe not found.");
            }

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<CreateRecipeRatingResponseModel>.Failure("Customer not found.");
            }

            var exist = await _recipeRatingRepository.IsExist(customer.Id, request.RecipeId);

            if (exist == true)
            {
                return BaseResponse<CreateRecipeRatingResponseModel>.Failure("You have already rated this recipe.");
            }
            if(request.Rating < 1 || request.Rating > 5)
            {
                return BaseResponse<CreateRecipeRatingResponseModel>.Failure("Rating must be between 1 and 5.");
            }

            var recipeRating = request.Adapt<RecipeRating>();

            recipeRating.Id = Guid.NewGuid();
            recipeRating.CustomerId = customer.Id;

            await _recipeRatingRepository.AddRecipeRatingAsync(recipeRating);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<CreateRecipeRatingResponseModel>.Success("Recipe rated successfully.",recipeRating.Adapt<CreateRecipeRatingResponseModel>());
        }

        public async Task<BaseResponse<UpdateRecipeRatingResponseModel>> UpdateRatingAsync( Guid customerId, UpdateRecipeRatingRequestModel request)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<UpdateRecipeRatingResponseModel>.Failure("Customer not found.");
            }

            var recipeRating = await _recipeRatingRepository.GetByCustomerAndRecipeAsync(customer.Id, request.RecipeId);

            if (recipeRating == null)
            {
                return BaseResponse<UpdateRecipeRatingResponseModel>.Failure("Rating not found.");
            }
            if (request.Rating < 1 || request.Rating > 5)
            {
                return BaseResponse<UpdateRecipeRatingResponseModel>.Failure("Rating must be between 1 and 5.");
            }

            recipeRating.Rating = request.Rating;
            recipeRating.Review = request.Review;

            _recipeRatingRepository.UpdatRecipeRating(recipeRating);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<UpdateRecipeRatingResponseModel>.Success("Rating updated successfully.",recipeRating.Adapt<UpdateRecipeRatingResponseModel>());
        }

        public async Task<BaseResponse<ICollection<MyRatingResponseModel>>> GetMyRatingsAsync(Guid customerId)
        {
            var ratings = await _recipeRatingRepository.GetByCustomerIdAsync(customerId);

            var response = ratings.Select(x => new MyRatingResponseModel
            {
                Id = x.Id,
                RecipeId = x.RecipeId,
                RecipeName = x.Recipe?.Name ?? "Unknown Recipe",
                RecipeImageUrl = x.Recipe?.ImageUrl ?? "",
                Rating = x.Rating,
                Review = x.Review
            }).ToList();

            return BaseResponse<ICollection<MyRatingResponseModel>>.Success("Ratings retrieved successfully.",response);
        }
        public async Task<BaseResponse<double>> GetAverageRatingAsync(Guid recipeId)
        {
            var average = await _recipeRatingRepository.GetAverageRatingAsync(recipeId);

            return BaseResponse<double>.Success("Average rating retrieved successfully.",average ?? 0);
        }

        public async Task<BaseResponse<bool>> DeleteRatingAsync(Guid recipeId, Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<bool>.Failure("Customer not found.");
            }

            var recipeRating = await _recipeRatingRepository.GetByCustomerAndRecipeAsync(customer.Id, recipeId);

            if (recipeRating == null)
            {
                return BaseResponse<bool>.Failure("Rating not found.");
            }

            _recipeRatingRepository.DeleteRecipeRating(recipeRating);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>.Success("Rating deleted successfully.",true);
        }
    }
}

