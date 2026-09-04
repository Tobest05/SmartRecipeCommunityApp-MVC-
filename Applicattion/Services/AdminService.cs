using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    using global::Application.Dto;
    using global::Application.Interfaces.Repository;
    using global::Application.Interfaces.Services;
    using Mapster;

    namespace Application.Services.Implementation
    {
        public class AdminService : IAdminService
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IRecipeRepository _recipeRepository;
            private readonly ICategoryRepository _categoryRepository;
            private readonly ICommentRepository _commentRepository;
            private readonly IRatingRepository _ratingRepository;

            public AdminService(
                ICustomerRepository customerRepository,
                IRecipeRepository recipeRepository,
                ICategoryRepository categoryRepository,
                ICommentRepository commentRepository,
                IRatingRepository ratingRepository)
            {
                _customerRepository = customerRepository;
                _recipeRepository = recipeRepository;
                _categoryRepository = categoryRepository;
                _commentRepository = commentRepository;
                _ratingRepository = ratingRepository;
            }

            public async Task<BaseResponse<AdminDashboardViewModel>> GetDashboardAsync()
            {
                var customers = await _customerRepository.GetAllCustomerAsync();

                var recipes = await _recipeRepository.GetAllAsync();

                var categories = await _categoryRepository.GetAllCategoryAsync();

                var comments = await _commentRepository.GetAllRecipeCommentAsync();

                var ratings = await _ratingRepository.GetAllRecipeRatingAsync();

                var dashboard = new AdminDashboardViewModel
                {
                    TotalCustomers = customers.Count,

                    TotalRecipes = recipes.Count,

                    TotalCategories = categories.Count,

                    TotalComments = comments.Count,

                    TotalRatings = ratings.Count,

                    RecentRecipes = recipes
                        .OrderByDescending(x => x.CreatedBy)
                        .Take(5)
                        .Adapt<ICollection<CreateRecipeResponseModel>>()
                };

                return BaseResponse<AdminDashboardViewModel>
                    .Success("Dashboard loaded successfully.", dashboard);
            }
        }
    }
}
