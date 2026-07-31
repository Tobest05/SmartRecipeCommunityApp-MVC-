using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using BCrypt.Net;
using Mapster;

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
            {
                return BaseResponse<LoginResponseModel>.Failure("Invalid email or password.");
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (!isValidPassword)
            {
                return BaseResponse<LoginResponseModel>.Failure("Invalid email or password.");
            }

            var userRole = await _userRoleRepository.GetByIdAsync(user.Id);

            if (userRole == null)
            {
                return BaseResponse<LoginResponseModel>.Failure("User role not found.");
            }

            var role = await _roleRepository.GetByIdAsync(userRole.RoleId);

            if (role == null)
            {
                return BaseResponse<LoginResponseModel>.Failure("Role not found.");
            }
            var response = user.Adapt<LoginResponseModel>();

            return BaseResponse<LoginResponseModel>.Success(
                "Login successful.",
                response);
        }

    }

}
