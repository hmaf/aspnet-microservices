using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services.Interface
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);

    }
}
