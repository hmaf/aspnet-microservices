using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCatche;

        public BasketRepository(IDistributedCache redisCatche)
        {
            _redisCatche = redisCatche;
        }


        public async Task<ShoppingCart?> GetUserBasketAsync(string userName)
        {
            var basket = await _redisCatche.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart cart)
        {
            await _redisCatche.SetStringAsync(cart.UserName, JsonConvert.SerializeObject(cart));

            return await GetUserBasketAsync(cart.UserName);
        }
        
        public async Task DeleteBasketAsync(string userName)
        {
            await _redisCatche.RemoveAsync(userName);
        }
    }
}
