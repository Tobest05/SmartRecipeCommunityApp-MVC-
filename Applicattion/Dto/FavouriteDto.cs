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
    public class CreateFavouriteRecipeResponseModel
    {
        public Guid RecipeId { get; set; }
    }
}
