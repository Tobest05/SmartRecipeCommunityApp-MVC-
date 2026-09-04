using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RecipeLike : BaseEntity
    {
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } = default!;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
