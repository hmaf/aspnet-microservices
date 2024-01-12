using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastracture.Persistence
{
    public class ContextSeed
    {
        public static async Task Seed(Context context, ILogger<Context> logger)
        {
             if(!context.Orders.Any())
            {
                await context.Orders.AddRangeAsync(GetPreconfiguredOrders());
                await context.Orders.AddRangeAsync();
                logger.LogInformation("data seed section cofigured");
            }
        }

        public static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    FirstName = "mohammad",
                    LastName = "ordookhani",
                    UserName = "mohammad",
                    EmailAddress = "test@test.com",
                    City = "tehran",
                    Country = "iran",
                    TotalPrice = 10000
                }
            };
        }
    }
}
