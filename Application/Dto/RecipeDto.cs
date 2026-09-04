using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateRecipeRequestModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public IFormFile ImageUrl { get; set; } = default!;
        public int PreparationTimeMinutes { get; set; }
        public int CookingTimeMinutes { get; set; }
        public int Servings { get; set; }
        public Difficulty Difficulty { get; set; }
        public RecipeStatus Status { get; set; }
    }

    public class CreateRecipeResponseModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public int PreparationTimeMinutes { get; set; }
        public int CookingTimeMinutes { get; set; }
        public int Servings { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public Difficulty Difficulty { get; set; }
        public RecipeStatus Status { get; set; }
    }
    

    public class UpdateRecipeRequestModel
    {
        public Guid CategoryId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public IFormFile ImageUrl { get; set; } = default!;
        public string ExistingImageUrl { get; set; } = default!;
        public int PreparationTimeMinutes { get; set; }
        public int CookingTimeMinutes { get; set; }
        public int Servings { get; set; }
        public Difficulty Difficulty { get; set; }
        public RecipeStatus Status { get; set; }
    }
    public class UpdateRecipeResponseModel
    {
        public string Name { get; set; } = default!;
        public IFormFile ImageUrl { get; set; } = default!;
    }
}
