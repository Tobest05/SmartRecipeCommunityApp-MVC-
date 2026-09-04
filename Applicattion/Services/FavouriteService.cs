using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class FavouriteRecipeService : IFavouriteService
    {
        private readonly IFavouriteRecipeRepository _favouriteRecipeRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRecipeLikeRepository _recipeLikeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FavouriteRecipeService(
            IFavouriteRecipeRepository favouriteRecipeRepository,
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            IRecipeLikeRepository recipeLikeRepository,
            IUnitOfWork unitOfWork)
        {
            _favouriteRecipeRepository = favouriteRecipeRepository;
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _recipeLikeRepository = recipeLikeRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<BaseResponse<FavouriteRecipeResponseModel>>
            AddFavouriteRecipeAsync(
                CreateFavouriteRecipeRequestModel request,
                Guid customerId)
        {
            var customer =
                await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<FavouriteRecipeResponseModel>
                    .Failure("Customer not found.");
            }


            var recipe =
                await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<FavouriteRecipeResponseModel>
                    .Failure("Recipe not found.");
            }


            var exists =
                await _favouriteRecipeRepository.IsExist(
                    customerId,
                    request.RecipeId);

            if (exists)
            {
                return BaseResponse<FavouriteRecipeResponseModel>
                    .Failure(
                        "Recipe already added to favourites.");
            }


            var favourite = new Favourite
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                RecipeId = request.RecipeId
            };


            await _favouriteRecipeRepository
                .AddFavouriteRecipeAsync(favourite);

            await _unitOfWork.SaveChangesAsync();


            var response = new FavouriteRecipeResponseModel
            {
                Id = favourite.Id,
                RecipeId = recipe.Id,
                RecipeName = recipe.Name,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                PreparationTimeMinutes =
                    recipe.PreparationTimeMinutes,
                CookingTimeMinutes =
                    recipe.CookingTimeMinutes,
                Servings = recipe.Servings,
                Difficulty = recipe.Difficulty,
                LikeCount =
                    await _recipeLikeRepository
                        .GetLikeCountAsync(recipe.Id)
            };


            return BaseResponse<FavouriteRecipeResponseModel>
                .Success(
                    "Recipe added to favourites.",
                    response);
        }


        public async Task<BaseResponse<bool>>
        RemoveFavouriteRecipeAsync(Guid id, Guid customerId)
        {
            var favourite =
                await _favouriteRecipeRepository
                    .GetFavouriteRecipeByIdAsync(id);

            if (favourite == null)
            {
                return BaseResponse<bool>
                    .Failure("Favourite recipe not found.");
            }

            if (favourite.CustomerId != customerId)
            {
                return BaseResponse<bool>
                    .Failure("You can only remove your own favourite recipes.");
            }

            _favouriteRecipeRepository.DeleteFavouriteRecipe(favourite);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success(
                    "Favourite recipe removed successfully.",
                    true);
        }

        public async Task<BaseResponse<ICollection<FavouriteRecipeViewModel>>>
    GetFavouriteRecipesByCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<ICollection<FavouriteRecipeViewModel>>
                    .Failure("Customer not found.");
            }

            var favourites =
                await _favouriteRecipeRepository
                    .GetFavouriteRecipeByCustomerIdAsync(customerId);

            var response = favourites.Select(x => new FavouriteRecipeViewModel
            {
                Id = x.Id,
                RecipeId = x.RecipeId,

                RecipeName = x.Recipe.Name,
                Description = x.Recipe.Description,
                ImageUrl = x.Recipe.ImageUrl,

                CategoryName = x.Recipe.Category?.Name ?? "Unknown",

                Difficulty = x.Recipe.Difficulty,

                PreparationTimeMinutes =
                    x.Recipe.PreparationTimeMinutes,

                CookingTimeMinutes =
                    x.Recipe.CookingTimeMinutes,

                Servings = x.Recipe.Servings

            }).ToList();

            return BaseResponse<ICollection<FavouriteRecipeViewModel>>
                .Success(
                    "Favourite recipes retrieved successfully.",
                    response);
        }
    }
}
