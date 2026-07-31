using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Application.Dto
{
    public class CreateCategoryRequestModel
    {
        public string Name { get; set; } = default!;

    }

    public class UpdateCategoryRequestModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
    }
    public class UpdateCategoryResponseModel
    {

        public string Name { get; set; } = default!;
    }

    public class CategoryResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
    }
}
