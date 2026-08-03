using Application.Interfaces.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Application.Interfaces.Services;
using Application.Services.Implementation;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Presentation_Layer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SmartRecipeContext>(options =>
                options.UseMySQL(
                    builder.Configuration.GetConnectionString("MyConnectionString")!));
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.LogoutPath = "/Account/Logout";

        options.Cookie.Name = "SmartRecipeCookie";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });
            builder.Services.AddSingleton(Mapster.TypeAdapterConfig.GlobalSettings);
              
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
            builder.Services.AddScoped<IInstructionRepository, InstructionRepository>();
            builder.Services.AddScoped<IFavouriteRecipeRepository, FavouriteRecipeRepository>();
            builder.Services.AddScoped<ICommentRepository, RecipeCommentRepository>();
            builder.Services.AddScoped<IRatingRepository, RecipeRatingRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IIngredientService, IngredientService>();
            builder.Services.AddScoped<IInstructionService, InstructionService>();
            builder.Services.AddScoped<IFavouriteService, FavouriteRecipeService>();
            builder.Services.AddScoped<ICommentService, RecipeCommentService>();
            builder.Services.AddScoped<IRatingService, RecipeRatingService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.Run();

        }
    }
}
