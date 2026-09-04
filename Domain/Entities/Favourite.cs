using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Favourite : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid RecipeId { get; set; }
        public Customer Customer { get; set; } = null!;
        public Recipe Recipe { get; set; } = null!;
    }
}
