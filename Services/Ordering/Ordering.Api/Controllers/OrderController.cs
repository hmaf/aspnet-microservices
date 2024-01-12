﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Net;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region Constructor

        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        #endregion

        #region Get all order

        [HttpGet("{userName}", Name = "GetOrderByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderByUserName(GetOrdersListQuery userName)
        {
            var result = await _mediator.Send(userName);

            return Ok(result);
        }

        #endregion

        #region Checkout order

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        #endregion

        #region Update order

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)StatusCodes.Status204NoContent)]
        [ProducesResponseType((int)StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        #endregion

        #region Delete order

        [HttpPut("{id}", Name = "DeleteOrder")]
        [ProducesResponseType((int)StatusCodes.Status204NoContent)]
        [ProducesResponseType((int)StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteOrder(DeleteOrderCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        #endregion
    }
}