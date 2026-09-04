using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Context
{
    public class SmartRecipeContext : DbContext
    {
        public SmartRecipeContext(DbContextOptions<SmartRecipeContext> options) : base(options)
        { }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Favourite> FavouriteRecipes { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Instruction> Instruction { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<RecipeLike> RecipeLikes     { get; set; }
        public DbSet<RecipeComment> RecipeComments { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var customerRoleId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var adminUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var userRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Role>().HasData(

                new Role
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    CreatedAt = DateTime.UtcNow,
                },

                new Role
                {
                    Id = customerRoleId,
                    Name = "Customer",
                    CreatedAt = DateTime.UtcNow,
                }

            );

            modelBuilder.Entity<User>().HasData(

                new User
                {
                    Id = adminUserId,
                    Email = "admin@smartrecipe.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedAt = DateTime.UtcNow,
                }

            );

            modelBuilder.Entity<UserRole>().HasData(

                new UserRole
                {
                    Id = userRoleId,
                    UserId = adminUserId,
                    RoleId = adminRoleId,
                    CreatedAt = DateTime.UtcNow,
                }

            );
        }
    }
}