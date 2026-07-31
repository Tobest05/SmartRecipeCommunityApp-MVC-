using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateRecipeCommentRequestModel
    {
        public Guid RecipeId { get; set; }
        public Guid CustomerId { get; set; }

        public string Comment { get; set; } = default!;
    }
    public class UpdateRecipeCommentRequestModel
    {
        public Guid RecipeId { get; set; }
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Comment { get; set; } = default!;
    }
    public class UpdateRecipeCommentResponseModel
    {
        public Guid RecipeId { get; set; }
    }
    public class CreateRecipeCommentResponseModel
    {
        public string Comment { get; set; } = default!;
    }

}

