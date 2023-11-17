using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Basket.Api.Entities;
using System.Net;
using Basket.Api.GrpcServices;

namespace Basket.Api.Controllers
{
    [Route("api/v1/Basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region Constractor
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountService;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountService)
        {
            _basketRepository = basketRepository;
            _discountService = discountService;
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
    }
}
