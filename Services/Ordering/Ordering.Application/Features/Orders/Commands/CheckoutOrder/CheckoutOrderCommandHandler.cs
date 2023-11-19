using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastracture;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        #region Constractor

        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }
        
        #endregion

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOder = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation($"order {orderEntity.Id} is successfully created");

            // send Mail
            await SendMail(newOder);

            return newOder.Id;
        }

        private async Task SendMail(Order order)
        {
            try
            {
                // send email
                await _emailService.SendEmail(new Email
                {
                    To = "test@test.com",
                    Subject = "new order has created",
                    Body = "this is a body email"
                });
            }
            catch (Exception)
            {
                _logger.LogInformation("email has not been send");
            }
        }
    }
}
