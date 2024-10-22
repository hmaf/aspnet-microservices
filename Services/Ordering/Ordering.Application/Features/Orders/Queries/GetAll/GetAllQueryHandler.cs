
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.GetAll
{
    internal class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Order>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderRepository.Getall();
            return result.ToList();
        }
    }
}
