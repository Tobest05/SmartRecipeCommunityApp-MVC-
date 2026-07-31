using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Ingredient : BaseEntity
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = default!;
        public Recipe Recipe { get; set; } = null!;

    }
}
