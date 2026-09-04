using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class CustomerDashboardViewModel
    {
        public string FullName { get; set; } = default!;

        public string ProfileImageUrl { get; set; } = default!;

        public int TotalRecipes { get; set; }

        public int TotalFavourites { get; set; }

        public int TotalComments { get; set; }

        public double AverageRating { get; set; }

        public ICollection<CreateRecipeResponseModel> RecentRecipes { get; set; } = new List<CreateRecipeResponseModel>();
    }
}


