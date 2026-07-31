using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Recipe : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public int PreparationTimeMinutes { get; set; }
        public int CookingTimeMinutes { get; set; }
        public int Servings { get; set; }
        public Difficulty Difficulty { get; set; }
        public RecipeStatus Status { get; set; }
        public int TotalTimeMinutes => PreparationTimeMinutes + CookingTimeMinutes;
        public Customer Customer { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<Instruction> Instruction { get; set; } = new List<Instruction>();
        public ICollection<RecipeComment> RecipeComment { get; set; } = new List<RecipeComment>();
        public ICollection<RecipeRating> RecipeRating { get; set; } = new List<RecipeRating>();
        public ICollection<Favourite> FavouriteRecipe { get; set; } = new List<Favourite>();


    }
}
