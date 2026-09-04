using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<BaseResponse<RegisterCustomerResponse>> RegisterAsync(RegisterCustomerRequest request);

        Task<BaseResponse<RegisterCustomerResponse>> GetCustomerByIdAsync(Guid id);
        Task<BaseResponse<RegisterCustomerResponse>> GetCustomerByUserIdAsync(Guid userId);

        Task<BaseResponse<ICollection<RegisterCustomerResponse>>> GetAllCustomerAsync();

        Task<BaseResponse<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request);

        Task<BaseResponse<bool>> DeleteCustomerAsync(Guid id);

        Task<BaseResponse<CustomerDashboardViewModel>> GetDashboardAsync(Guid userId);
    }
}
