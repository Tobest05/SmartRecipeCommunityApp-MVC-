using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IRecipeLikeRepository
    {
        Task<RecipeLike?> GetAsync(Guid customerId, Guid recipeId);

        Task AddAsync(RecipeLike like);

        void Remove(RecipeLike like);

        Task<int> GetLikeCountAsync(Guid recipeId);
    }
}
