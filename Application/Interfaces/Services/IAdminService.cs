using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<BaseResponse<AdminDashboardViewModel>> GetDashboardAsync();
    }
}
