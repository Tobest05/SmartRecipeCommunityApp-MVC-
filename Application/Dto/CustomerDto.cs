using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dto
{
    public class RegisterCustomerRequest
    {
        [Required(ErrorMessage ="First name is required")]
        public string FirstName { get; set; } = default!;
        [Required]
        public string LastName { get; set; } = default!;
        public IFormFile? ProfileImage { get; set; } = default!;
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; } = default!;
        public string Bio { get; set; } = default!;
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; } = default!;
    }
    public class RegisterCustomerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Bio { get; set; } = default!;
    }

    public class UpdateCustomerRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ExistingImageUrl { get; set; } = default!;
        public IFormFile? ProfileImage { get; set; } = default!;
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
