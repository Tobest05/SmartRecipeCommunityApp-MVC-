using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enum;
using Mapster;

namespace Application.Services.Implementation
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeService(
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateRecipeResponseModel>> AddRecipeAsync(CreateRecipeRequestModel request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer == null)
            {
                return BaseResponse<CreateRecipeResponseModel>.Failure("Customer not found.");
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return BaseResponse<CreateRecipeResponseModel>.Failure("Category not found.");
            }

            var recipe = request.Adapt<Recipe>();

            recipe.Id = Guid.NewGuid();

            await _recipeRepository.AddAsync(recipe);

            await _unitOfWork.SaveChangesAsync();

            var response = recipe.Adapt<CreateRecipeResponseModel>();

            return BaseResponse<CreateRecipeResponseModel>.Success("Recipe added successfully.", response);
        }

        public async Task<BaseResponse<CreateRecipeResponseModel>> GetRecipeByIdAsync(Guid id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);

            if (recipe == null)
            {
                return BaseResponse<CreateRecipeResponseModel>.Failure("Recipe not found.");
            }

            var response = recipe.Adapt<CreateRecipeResponseModel>();

            return BaseResponse<CreateRecipeResponseModel>.Success("Recipe retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetAllRecipeAsync()
        {
            var recipes = await _recipeRepository.GetAllAsync();

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetPublishedRecipeAsync()
        {
            var recipes = await _recipeRepository.GetPublishedRecipesAsync();

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Published recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<ICollection<CreateRecipeResponseModel>>
                    .Failure("Customer not found.");
            }

            var recipes = await _recipeRepository.GetByCustomerIdAsync(customerId);

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> SearchRecipeAsync(string name)
        {
            var recipes = await _recipeRepository.SearchByNameAsync(name);

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                return BaseResponse<ICollection<CreateRecipeResponseModel>>
                    .Failure("Category not found.");
            }

            var recipes = await _recipeRepository.GetByCategoryIdAsync(categoryId);

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByDifficultyAsync(Difficulty difficulty)
        {
            var recipes = await _recipeRepository.GetByDifficultyAsync(difficulty);

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }
        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByStatusAsync(RecipeStatus status)
        {
            var recipes = await _recipeRepository.GetByStatusAsync(status);

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCookingTimeAsync(int cookingTime)
        {
            var recipes = await _recipeRepository.GetByCookingTimeAsync(cookingTime);

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();

            return BaseResponse<ICollection<CreateRecipeResponseModel>>
                .Success("Recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<UpdateRecipeResponseModel>> UpdateRecipeAsync(UpdateRecipeRequestModel request)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.Id);

            if (recipe == null)
            {
                return BaseResponse<UpdateRecipeResponseModel>
                    .Failure("Recipe not found.");
            }

            request.Adapt(recipe);

            _recipeRepository.Update(recipe);

            await _unitOfWork.SaveChangesAsync();

            var response = recipe.Adapt<UpdateRecipeResponseModel>();

            return BaseResponse<UpdateRecipeResponseModel>
                .Success("Recipe updated successfully.", response);
        }

        public async Task<BaseResponse<bool>> DeleteRecipeAsync(Guid id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);

            if (recipe == null)
            {
                return BaseResponse<bool>
                    .Failure("Recipe not found.");
            }

            _recipeRepository.Delete(recipe);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success("Recipe deleted successfully.", true);
        }
    }
}
