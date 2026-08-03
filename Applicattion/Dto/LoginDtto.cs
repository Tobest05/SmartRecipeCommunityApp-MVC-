using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class LoginRequestModel
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginResponseModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public ICollection<string> Roles { get; set; } = new List<string>();
    }


}
