using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class RecipeDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CategoryId { get; set; }

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;

        public int PreparationTimeMin { get; set; }
        public int CookingTimeMin { get; set; }
        public int Serving { get; set; }

        public Difficulty Difficulty { get; set; }
        public RecipeStatus RecipeStatus { get; set; }

        public string CategoryName { get; set; } = default!;
        public string CustomerName { get; set; } = default!;

        public double AverageRating { get; set; }

        public int LikeCount { get; set; }

        public bool IsLiked { get; set; }

        public ICollection<CreateInstructionResponseModel> Instructions { get; set; }
            = new List<CreateInstructionResponseModel>();

        public ICollection<CreateIngredientResponseModel> Ingredients { get; set; }
            = new List<CreateIngredientResponseModel>();

        public ICollection<CreateRecipeCommentResponseModel> Comments { get; set; }
            = new List<CreateRecipeCommentResponseModel>();
    }
}
