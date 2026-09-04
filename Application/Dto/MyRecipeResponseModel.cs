using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class MyRecipeResponseModel
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public int PreparationTimeMinutes { get; set; }

        public int CookingTimeMinutes { get; set; }

        public int TotalTimeMinutes => PreparationTimeMinutes + CookingTimeMinutes;

        public int Servings { get; set; }

        public Difficulty Difficulty { get; set; }

        public RecipeStatus Status { get; set; }

        public int LikeCount { get; set; }

        public double AverageRating { get; set; }
    }
}
