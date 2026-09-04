using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class RecipeLikeRequest
    {
        public Guid RecipeId { get; set; }
    }

    public class RecipeLikeResponse
    {
        public Guid RecipeId { get; set; }

        public bool IsLiked { get; set; }

        public int LikeCount { get; set; }
    }
}
