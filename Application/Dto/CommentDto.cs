using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateRecipeCommentRequestModel
    {
        public Guid RecipeId { get; set; }

        public string Comment { get; set; } = default!;
    }
    public class UpdateRecipeCommentRequestModel
    {
        public Guid RecipeId { get; set; }
        public string Comment { get; set; } = default!;
    }
    public class UpdateRecipeCommentResponseModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string Comment { get; set; } = default!;
    }
    public class CreateRecipeCommentResponseModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string Comment { get; set; } = default!;
        public string RecipeName { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }

}

