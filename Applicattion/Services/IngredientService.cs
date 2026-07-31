using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IngredientService(
            IIngredientRepository ingredientRepository,
            IRecipeRepository recipeRepository,
            IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateIngredientResponseModel>> AddIngredientAsync(CreateIngredientRequestModel request)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<CreateIngredientResponseModel>
                    .Failure("Recipe not found.");
            }

            var exist = await _ingredientRepository.IsExist(request.RecipeId, request.Name);

            if (exist == true)
            {
                return BaseResponse<CreateIngredientResponseModel>
                    .Failure("Ingredient already exists.");
            }

            var ingredient = request.Adapt<Ingredient>();

            ingredient.Id = Guid.NewGuid();

            await _ingredientRepository.AddIngredientAsync(ingredient);

            await _unitOfWork.SaveChangesAsync();

            var response = ingredient.Adapt<CreateIngredientResponseModel>();

            return BaseResponse<CreateIngredientResponseModel>
                .Success("Ingredient added successfully.", response);
        }

        public async Task<BaseResponse<CreateIngredientResponseModel>> GetIngredientByIdAsync(Guid id)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(id);

            if (ingredient == null)
            {
                return BaseResponse<CreateIngredientResponseModel>
                    .Failure("Ingredient not found.");
            }

            var response = ingredient.Adapt<CreateIngredientResponseModel>();

            return BaseResponse<CreateIngredientResponseModel>
                .Success("Ingredient retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateIngredientResponseModel>>> GetAllIngredientAsync()
        {
            var ingredients = await _ingredientRepository.GetAllIngredientAsync();

            var response = ingredients.Adapt<ICollection<CreateIngredientResponseModel>>();

            return BaseResponse<ICollection<CreateIngredientResponseModel>>
                .Success("Ingredients retrieved successfully.", response);
        }

        public async Task<BaseResponse<UpdateIngredientResponseModel>> UpdateIngredientAsync(UpdateIngredientRequestModel request)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.Id);

            if (ingredient == null)
            {
                return BaseResponse<UpdateIngredientResponseModel>
                    .Failure("Ingredient not found.");
            }

            request.Adapt(ingredient);

            _ingredientRepository.UpdateIngredient(ingredient);

            await _unitOfWork.SaveChangesAsync();

            var response = ingredient.Adapt<UpdateIngredientResponseModel>();

            return BaseResponse<UpdateIngredientResponseModel>
                .Success("Ingredient updated successfully.", response);
        }

        public async Task<BaseResponse<bool>> DeleteIngredientAsync(Guid id)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(id);

            if (ingredient == null)
            {
                return BaseResponse<bool>
                    .Failure("Ingredient not found.");
            }

            _ingredientRepository.DeleteIngredient(ingredient);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success("Ingredient deleted successfully.", true);
        }
    }
}

