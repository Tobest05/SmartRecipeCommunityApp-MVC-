using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateFavouriteRecipeRequestModel
    {
        public Guid RecipeId { get; set; }
        public Guid CustomerId { get; set; }
    }
    public class FavouriteRecipeResponseModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }

        public string RecipeName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;

        public int PreparationTimeMinutes { get; set; }
        public int CookingTimeMinutes { get; set; }

        public int Servings { get; set; }

        public Difficulty Difficulty { get; set; }

        public int LikeCount { get; set; }
    }
}
