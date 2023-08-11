using Basket.Api.Entities;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart cart);
        Task<ShoppingCart> GetUserBasketAsync(string userName);
        Task DeleteBasketAsync(string userName);
    }
}
