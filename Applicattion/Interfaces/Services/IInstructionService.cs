using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IInstructionService
    {
        Task<BaseResponse<CreateInstructionResponseModel>> AddInstructionAsync(CreateInstructionRequestModel request);

        Task<BaseResponse<CreateInstructionResponseModel>> GetInstructionByIdAsync(Guid id);

        Task<BaseResponse<ICollection<CreateInstructionResponseModel>>> GetAllInstructionAsync();

        Task<BaseResponse<UpdateInstructionResponseModel>> UpdateInstructionAsync(UpdateInstructionRequestModel request);

        Task<BaseResponse<bool>> DeleteInstructionAsync(Guid id);
    }
}
