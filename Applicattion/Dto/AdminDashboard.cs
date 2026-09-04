using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class AdminDashboardViewModel
    {
        public int TotalCustomers { get; set; }

        public int TotalRecipes { get; set; }

        public int TotalCategories { get; set; }

        public int TotalComments { get; set; }

        public int TotalRatings { get; set; }

        public ICollection<CreateRecipeResponseModel> RecentRecipes { get; set; }
            = new List<CreateRecipeResponseModel>();
    }
}
