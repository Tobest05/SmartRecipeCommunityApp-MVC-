using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class InstructionService : IInstructionService
    {
        private readonly IInstructionRepository _instructionRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InstructionService(
            IInstructionRepository instructionRepository,
            IRecipeRepository recipeRepository,
            IUnitOfWork unitOfWork)
        {
            _instructionRepository = instructionRepository;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateInstructionResponseModel>> AddInstructionAsync(CreateInstructionRequestModel request)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                return BaseResponse<CreateInstructionResponseModel>
                    .Failure("Recipe not found.");
            }

            var instruction = request.Adapt<Instruction>();

            instruction.Id = Guid.NewGuid();

            await _instructionRepository.AddInstructionAsync(instruction);

            await _unitOfWork.SaveChangesAsync();

            var response = instruction.Adapt<CreateInstructionResponseModel>();

            return BaseResponse<CreateInstructionResponseModel>
                .Success("Instruction added successfully.", response);
        }

        public async Task<BaseResponse<CreateInstructionResponseModel>> GetInstructionByIdAsync(Guid id)
        {
            var instruction = await _instructionRepository.GetInstructionByIdAsync(id);

            if (instruction == null)
            {
                return BaseResponse<CreateInstructionResponseModel>
                    .Failure("Instruction not found.");
            }

            var response = instruction.Adapt<CreateInstructionResponseModel>();

            return BaseResponse<CreateInstructionResponseModel>
                .Success("Instruction retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<CreateInstructionResponseModel>>> GetAllInstructionAsync()
        {
            var instructions = await _instructionRepository.GetAllInstructionAsync();

            var response = instructions.Adapt<ICollection<CreateInstructionResponseModel>>();

            return BaseResponse<ICollection<CreateInstructionResponseModel>>
                .Success("Instructions retrieved successfully.", response);
        }

        public async Task<BaseResponse<UpdateInstructionResponseModel>> UpdateInstructionAsync(UpdateInstructionRequestModel request)
        {
            var instruction = await _instructionRepository.GetInstructionByIdAsync(request.Id);

            if (instruction == null)
            {
                return BaseResponse<UpdateInstructionResponseModel>
                    .Failure("Instruction not found.");
            }

            request.Adapt(instruction);

            _instructionRepository.UpdateInstruction(instruction);

            await _unitOfWork.SaveChangesAsync();

            var response = instruction.Adapt<UpdateInstructionResponseModel>();

            return BaseResponse<UpdateInstructionResponseModel>
                .Success("Instruction updated successfully.", response);
        }

        public async Task<BaseResponse<bool>> DeleteInstructionAsync(Guid id)
        {
            var instruction = await _instructionRepository.GetInstructionByIdAsync(id);

            if (instruction == null)
            {
                return BaseResponse<bool>
                    .Failure("Instruction not found.");
            }

            _instructionRepository.DeleteInstruction(instruction);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success("Instruction deleted successfully.", true);
        }
    }
}
