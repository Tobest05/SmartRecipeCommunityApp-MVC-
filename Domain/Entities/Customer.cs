using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Bio { get; set; } = default!;
        public string Email { get; set; } = default!;
        public User? User { get; set; }
        public ICollection<Favourite> FavouriteRecipes { get; set; } = new List<Favourite>();
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<RecipeComment> RecipeComment { get; set; } = new List<RecipeComment>();
        public ICollection<RecipeRating> RecipeRating { get; set; } = new List<RecipeRating>();
    }
}
