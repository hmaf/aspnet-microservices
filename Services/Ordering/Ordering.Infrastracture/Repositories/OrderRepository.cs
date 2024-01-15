using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastracture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastracture.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(Context db) : base(db) { }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
            => await _db.Orders
                        .Where(obj => obj.UserName.Equals(userName))
                        .ToListAsync();
    }
}
