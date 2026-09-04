using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IFavouriteRecipeRepository _favouriteRecipeRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(
            ICustomerRepository customerRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IRecipeRepository recipeRepository,
            IFavouriteRecipeRepository favouriteRecipeRepository,
            ICommentRepository commentRepository,
            IRatingRepository ratingRepository,
            IImageService imageService, 
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _recipeRepository = recipeRepository;
            _favouriteRecipeRepository = favouriteRecipeRepository;
            _commentRepository = commentRepository;
            _ratingRepository = ratingRepository;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<RegisterCustomerResponse>> RegisterAsync(RegisterCustomerRequest request)
        {
            var exist = await _userRepository.IsExistAsync(request.Email);

            if (exist)
            {
                return BaseResponse<RegisterCustomerResponse>
                    .Failure("Email already exists.");
            }

            var role = await _roleRepository.GetByNameAsync("Customer");

            if (role == null)
            {
                return BaseResponse<RegisterCustomerResponse>
                    .Failure("Customer role not found.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddUserAsync(user);

            string imagePath = "";
            if (request.ProfileImage !=null)
            {
                imagePath = await _imageService.UploadImageAsync(request.ProfileImage, "customers");
            }

            var customer = request.Adapt<Customer>();

            customer.Id = Guid.NewGuid();
            customer.UserId = user.Id;
            customer.Email = user.Email;
            customer.ProfileImageUrl = imagePath;

            await _customerRepository.AddCustomerAsync(customer);

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userRoleRepository.AddUserRoleAsync(userRole);

            await _unitOfWork.SaveChangesAsync();

            var response = customer.Adapt<RegisterCustomerResponse>();

            return BaseResponse<RegisterCustomerResponse>
                .Success("Registration Successful.", response);
        }
        public async Task<BaseResponse<RegisterCustomerResponse>> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return BaseResponse<RegisterCustomerResponse>
                    .Failure("Customer not found.");
            }

            var response = customer.Adapt<RegisterCustomerResponse>();

            return BaseResponse<RegisterCustomerResponse>
                .Success("Customer retrieved successfully.", response);
        }

        public async Task<BaseResponse<ICollection<RegisterCustomerResponse>>> GetAllCustomerAsync()
        {
            var customers = await _customerRepository.GetAllCustomerAsync();

            var response = customers.Adapt<ICollection<RegisterCustomerResponse>>();

            return BaseResponse<ICollection<RegisterCustomerResponse>>
                .Success("Customers retrieved successfully.", response);
        }

        public async Task<BaseResponse<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
            {
                return BaseResponse<UpdateCustomerResponse>
                    .Failure("Customer not found.");
            }

            request.Adapt(customer);

            
            if (request.ProfileImage != null)
            {
                customer.ProfileImageUrl = await _imageService.UploadImageAsync(
                    request.ProfileImage,
                    "customers");
            }

            _customerRepository.UpdateCustomer(customer);

            await _unitOfWork.SaveChangesAsync();

            var response = customer.Adapt<UpdateCustomerResponse>();

            return BaseResponse<UpdateCustomerResponse>
                .Success("Customer updated successfully.", response);
        }
        public async Task<BaseResponse<bool>> DeleteCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return BaseResponse<bool>
                    .Failure("Customer not found.");
            }

            _customerRepository.DeleteCustomer(customer);

            await _unitOfWork.SaveChangesAsync();

            return BaseResponse<bool>
                .Success("Customer deleted successfully.", true);
        }
        public async Task<BaseResponse<CustomerDashboardViewModel>> GetDashboardAsync(Guid userId)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return BaseResponse<CustomerDashboardViewModel>
                    .Failure("Customer not found.");
            }

            var recipes = await _recipeRepository
                .GetRecipeByCustomerIdAsync(customer.Id);

            var favourites = await _favouriteRecipeRepository
                .GetFavouriteRecipeByCustomerIdAsync(customer.Id);

            var comments = await _commentRepository
                .GetRecipeCommentByCustomerIdAsync(customer.Id);

            var averageRating = await _ratingRepository
                .GetAverageCustomerRatingByIdAsync(customer.Id);

            var dashboard = new CustomerDashboardViewModel
            {
                FullName = $"{customer.FirstName} {customer.LastName}",

                ProfileImageUrl = customer.ProfileImageUrl,

                TotalRecipes = recipes.Count,

                TotalFavourites = favourites.Count,

                TotalComments = comments.Count,

                AverageRating = averageRating ?? 0,

                RecentRecipes = recipes
                    .OrderByDescending(x => x.CreatedBy)
                    .Take(5)
                    .Adapt<ICollection<CreateRecipeResponseModel>>()
            };

            return BaseResponse<CustomerDashboardViewModel>
                .Success("Dashboard loaded successfully.", dashboard);
        }

        public async Task<BaseResponse<RegisterCustomerResponse>> GetCustomerByUserIdAsync(Guid userId)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return BaseResponse<RegisterCustomerResponse>
                    .Failure("Customer not found.");
            }

            var response = customer.Adapt<RegisterCustomerResponse>();

            return BaseResponse<RegisterCustomerResponse>
                .Success("Customer retrieved successfully.", response);
        }
    }
}