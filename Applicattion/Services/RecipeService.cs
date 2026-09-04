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
        private readonly IImageService _imageService;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IInstructionRepository _instructionRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecipeLikeRepository _recipeLikeRepository;

        public RecipeService(
            IRecipeRepository recipeRepository,
            ICustomerRepository customerRepository,
            ICategoryRepository categoryRepository,
            IIngredientRepository ingredientRepository,
            IRecipeLikeRepository recipeLikeRepository,
            IInstructionRepository instructionRepository,
            IImageService imageService,
            IRatingRepository ratingRepository,
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _customerRepository = customerRepository;
            _categoryRepository = categoryRepository;
            _ingredientRepository = ingredientRepository;
            _instructionRepository = instructionRepository;
            _ratingRepository = ratingRepository;
            _recipeLikeRepository = recipeLikeRepository;
            _imageService = imageService;
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateRecipeResponseModel>> AddRecipeAsync(
     CreateRecipeRequestModel request,
     Guid userId)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return BaseResponse<CreateRecipeResponseModel>
                    .Failure("Customer not found.");
            }

            var category = await _categoryRepository
                .GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return BaseResponse<CreateRecipeResponseModel>
                    .Failure("Category not found.");
            }

            var imagePath = await _imageService.UploadImageAsync(
                request.ImageUrl,
                "recipes");

            var recipe = request.Adapt<Recipe>();

            recipe.Id = Guid.NewGuid();

            
            recipe.CustomerId = customer.Id;
            recipe.CategoryId = category.Id;
            recipe.ImageUrl = imagePath;

            await _recipeRepository.AddAsync(recipe);

            await _unitOfWork.SaveChangesAsync();

            var response = recipe.Adapt<CreateRecipeResponseModel>();

            return BaseResponse<CreateRecipeResponseModel>
                .Success(
                    "Recipe added successfully.",
                    response);
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

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetPublishedRecipeAsync(Guid? customerId)
        {
            var recipes = await _recipeRepository.GetPublishedRecipesAsync();

            var response = recipes.Adapt<ICollection<CreateRecipeResponseModel>>();
            Guid? loggedInCustomerId = null;
                if (customerId.HasValue)
                {
                    var customer = await _customerRepository.GetByUserIdAsync(customerId.Value);
                    if (customer != null)
                    {
                        loggedInCustomerId = customerId;
                    }
                }
                foreach (var recipe in response)
                {
                    recipe.LikeCount = await _recipeLikeRepository.GetLikeCountAsync(recipe.Id);
                recipe.IsLiked = false;
                if (loggedInCustomerId.HasValue)
                {
                    var like = await _recipeLikeRepository.GetAsync(loggedInCustomerId.Value, recipe.Id);
                    recipe.IsLiked = like != null;  
                }
            }
            return BaseResponse<ICollection<CreateRecipeResponseModel>>.Success("Published recipes retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateRecipeResponseModel>>> GetRecipeByCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return BaseResponse<ICollection<CreateRecipeResponseModel>>
                    .Failure("Customer not found.");
            }

            var recipes = await _recipeRepository.GetRecipeByCustomerIdAsync(customerId);

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

            
            if (request.ImageUrl != null)
            {
                recipe.ImageUrl = await _imageService.UploadImageAsync(
                    request.ImageUrl,
                    "recipes");
            }

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

        public async Task<BaseResponse<RecipeDetailsViewModel>> GetRecipeDetailsAsync(Guid id,Guid? customerId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);

            if (recipe == null)
            {
                return BaseResponse<RecipeDetailsViewModel>
                    .Failure("Recipe not found.");
            }

            var averageRating = recipe.RecipeRating.Any()
                ? recipe.RecipeRating.Average(x => x.Rating)
                : 0;

            var likeCount = await _recipeLikeRepository
                .GetLikeCountAsync(recipe.Id);

            var isLiked = false;

            if (customerId.HasValue)
            {
                var customer = await _customerRepository
                    .GetByUserIdAsync(customerId.Value);

                if (customer != null)
                {
                    var existingLike = await _recipeLikeRepository
                        .GetAsync(customer.Id, recipe.Id);

                    isLiked = existingLike != null;
                }
            }

            var model = new RecipeDetailsViewModel
            {
                Id = recipe.Id,
                CustomerId = recipe.CustomerId,
                CategoryId = recipe.CategoryId,

                Name = recipe.Name,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,

                PreparationTimeMin = recipe.PreparationTimeMinutes,
                CookingTimeMin = recipe.CookingTimeMinutes,
                Serving = recipe.Servings,

                Difficulty = recipe.Difficulty,
                RecipeStatus = recipe.Status,

                CategoryName = recipe.Category?.Name ?? "Unknown",

                CustomerName = recipe.Customer == null
                    ? "Unknown"
                    : $"{recipe.Customer.FirstName} {recipe.Customer.LastName}",

                AverageRating = averageRating,

                LikeCount = likeCount,

                IsLiked = isLiked,

                Instructions = recipe.Instruction
                    .OrderBy(x => x.StepNumber)
                    .Adapt<ICollection<CreateInstructionResponseModel>>(),

                Ingredients = recipe.Ingredients
                    .Adapt<ICollection<CreateIngredientResponseModel>>(),

                Comments = recipe.RecipeComment
                    .Adapt<ICollection<CreateRecipeCommentResponseModel>>()
            };

            return BaseResponse<RecipeDetailsViewModel>
                .Success(
                    "Recipe details retrieved successfully.",
                    model);
        }

        public async Task<BaseResponse<ICollection<MyRecipeResponseModel>>> GetMyRecipesAsync(Guid customerId)
        {
           var customer = await _customerRepository.GetByIdAsync(customerId);
            if(customer ==  null)
            {
                return BaseResponse<ICollection<MyRecipeResponseModel>>.Failure("Customer not found");
            }
            var recipes = await _recipeRepository.GetRecipeByCustomerIdAsync(customerId);
            var response = new List<MyRecipeResponseModel>();
            foreach (var recipe in recipes)
            {
                var likeCount = await _recipeLikeRepository.GetLikeCountAsync(recipe.Id);
                var averageRating = recipe.RecipeRating.Any() ? recipe.RecipeRating.Average(x => x.Rating) : 0;
                response.Add(new MyRecipeResponseModel
                {
                    Id = recipe.Id,
                    CustomerId = recipe.CustomerId,
                    CategoryId = recipe.CategoryId,
                    CategoryName = recipe.Category?.Name ?? "Unknown",
                    Name = recipe.Name,
                    Description = recipe.Description,
                    ImageUrl = recipe.ImageUrl,
                    PreparationTimeMinutes  = recipe.PreparationTimeMinutes,
                    CookingTimeMinutes = recipe.CookingTimeMinutes,
                    Servings = recipe.Servings,
                    Difficulty = recipe.Difficulty,
                    Status = recipe.Status,
                    LikeCount = likeCount,
                    AverageRating = averageRating,
                });
              
            }
            return BaseResponse<ICollection<MyRecipeResponseModel>>.Success("My Recipes Retrieve Successfully");
        }
    }
}
