using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Basket.Api.Entities;
using System.Net;
using Basket.Api.GrpcServices;
using AutoMapper;
using EventBus.Message.Events;
using MassTransit;

namespace Basket.Api.Controllers
{
    [Route("api/v1/Basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region Constractor
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _discountService = discountService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        #endregion

        #region get Basket
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasket(string userName)
        {
            try
            {
                var basket = await _basketRepository.GetUserBasketAsync(userName);
                return Ok(basket ?? new ShoppingCart());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region update Basket
        [HttpPost("UpdateBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart model)
        {
            try
             {
                foreach (var item in model.Items)
                {
                    var coupon = await _discountService.GetDiscount(item.ProductName);
                    item.Price -= coupon.Amount;
                }

                return Ok(await _basketRepository.UpdateBasketAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region delete Basket
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            try
            {
                await _basketRepository.DeleteBasketAsync(userName);
                return RedirectToAction("GetBasket");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Checkout

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest  )]
        public async Task<IActionResult> Checout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price
            var basket = await _basketRepository.GetUserBasketAsync(basketCheckout.UserName);
            if (basket is null) return BadRequest();

            // create basketCheckoutEvent -- set total price on basketCheckout event Message
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            // send checkout event to RabbitMQ
            await _publishEndpoint.Publish(eventMessage);

            // remove basket
            await _basketRepository.DeleteBasketAsync(basketCheckout.UserName);

            return Accepted();
        }

        #endregion
    }
}
