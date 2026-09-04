using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class FavouriteRecipeViewModel
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }

        public string RecipeName { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public string CategoryName { get; set; } = default!;

        public Difficulty Difficulty { get; set; }

        public int PreparationTimeMinutes { get; set; }

        public int CookingTimeMinutes { get; set; }

        public int Servings { get; set; }
    }
}
