using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class MyRatingResponseModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string RecipeName { get; set; } = default!;
        public string RecipeImageUrl { get; set; } = default!;
        public int Rating { get; set; }
        public string Review { get; set; } = default!;
    }
}
