using Basket.Api.Entities;
using Basket.Api.Repositories.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _context;

        public BasketRepository(IDistributedCache context)
        {
            _context = context;
        }
        public async Task DeleteBasket(string UserName)
        {
            await _context.RemoveAsync(UserName);
        }

        public async Task<ShoppingCart> GetBasket(string UserName)
        {
            var basket = await _context.GetStringAsync(UserName);

            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _context.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }
    }
}
