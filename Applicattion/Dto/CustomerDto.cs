using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class RegisterCustomerRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Bio { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
    public class RegisterCustomerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Bio { get; set; } = default!;
    }

    public class UpdateCustomerRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Bio { get; set; } = default!;
    }
    public class UpdateCustomerResponse
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Bio { get; set; } = default!;
    }


}
