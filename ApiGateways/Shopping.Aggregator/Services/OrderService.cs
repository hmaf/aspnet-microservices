﻿using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interface;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrderByUserName(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Order/{userName}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
