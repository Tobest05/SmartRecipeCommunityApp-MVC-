using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateIngredientRequestModel
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = default!;
    }
    public class CreateIngredientResponseModel
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = default!;
    }

    public class UpdateIngredientRequestModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = default!;
    }
    public class UpdateIngredientResponseModel
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = default!;
    }
}
