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
        private readonly IUnitOfWork _unitOfWork;

        public FavouriteRecipeService(
            IFavouriteRecipeRepository favouriteRecipeRepository,
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _favouriteRecipeRepository = favouriteRecipeRepository;
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateFavouriteRecipeResponseModel>> AddFavouriteRecipeAsync(CreateFavouriteRecipeRequestModel request, Guid customerId)              
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<CreateFavouriteRecipeResponseModel>
                    .Failure("Customer not found.");
            }

            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<CreateFavouriteRecipeResponseModel>
                    .Failure("Recipe not found.");
            }

            var exist = await _favouriteRecipeRepository.IsExist(customer.Id, request.RecipeId);

            if (exist == true)
            {
                return BaseResponse<CreateFavouriteRecipeResponseModel>
                    .Failure("Recipe already added to favourites.");
            }

            var favourite = new Favourite
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                RecipeId = request.RecipeId
            };

            await _favouriteRecipeRepository.AddFavouriteRecipeAsync(favourite);

            await _unitOfWork.SaveChangesAsync();

            var response = favourite.Adapt<CreateFavouriteRecipeResponseModel>();

            return BaseResponse<CreateFavouriteRecipeResponseModel>
                .Success("Recipe added to favourites.", response);
        }

        public async Task<BaseResponse<CreateFavouriteRecipeResponseModel>> GetFavouriteRecipeByIdAsync(Guid id)
        {
            var favourite = await _favouriteRecipeRepository.GetFavouriteRecipeByIdAsync(id);

            if (favourite == null)
            {
                return BaseResponse<CreateFavouriteRecipeResponseModel>
                    .Failure("Favourite recipe not found.");
            }

            var response = favourite.Adapt<CreateFavouriteRecipeResponseModel>();

            return BaseResponse<CreateFavouriteRecipeResponseModel>
                .Success("Favourite recipe retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateFavouriteRecipeResponseModel>>> GetAllFavouriteRecipeAsync()
        {
            var favourites = await _favouriteRecipeRepository.GetAllFavouriteRecipeAsync();

            var response = favourites.Adapt<ICollection<CreateFavouriteRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateFavouriteRecipeResponseModel>>
                .Success("Favourite recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<bool>> RemoveFavouriteRecipeAsync(Guid id)
        {
            var favourite = await _favouriteRecipeRepository.GetFavouriteRecipeByIdAsync(id);

            if (favourite == null)
            {
                return BaseResponse<bool>
                    .Failure("Favourite recipe not found.");
            }

            _favouriteRecipeRepository.DeleteFavouriteRecipe(favourite);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success("Favourite recipe deleted successfully.", true);
        }
    }
}
