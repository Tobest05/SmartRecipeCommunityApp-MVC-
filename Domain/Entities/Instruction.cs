using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Instruction : BaseEntity
    {
        public Guid RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
        public Recipe Recipe { get; set; } = null!;

    }
}
