using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel model);
    }
}
