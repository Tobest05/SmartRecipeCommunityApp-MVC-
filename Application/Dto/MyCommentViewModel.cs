using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class MyCommentViewModel
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }

        public string RecipeName { get; set; } = default!;

        public string Comment { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }
}
