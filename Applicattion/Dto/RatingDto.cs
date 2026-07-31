using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateRecipeRatingRequestModel
    {
        public Guid RecipeId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; } = default!;
    }
    public class UpdateRecipeRatingRequestModel
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; } = default!;
    }
    public class UpdateRecipeRatingResponseModel
    {
        public Guid RecipeId { get; set; }
        public int Rating { get; set; }
    }

    public class CreateRecipeRatingResponseModel
    {
        public int Rating { get; set; }
        public string Review { get; set; } = default!;
    }
}
