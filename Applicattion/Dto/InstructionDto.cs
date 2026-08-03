using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CreateInstructionRequestModel
    {
        public Guid RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
    }
    public class CreateInstructionResponseModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
    }
    public class UpdateInstructionRequestModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }

        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
    }
    public class UpdateInstructionResponseModel
    {
        public Guid RecipeId { get; set; }
    }
}

