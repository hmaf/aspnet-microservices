using Discount.Api.Entities;
using Discount.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.Api.Controllers
{
    [Route("api/v1/Discount")]
    [ApiController]
    public class DiscountController : ControllerBase
    {

        #region Constractore
        private readonly IDiscountRepository _Repo;

        public DiscountController(IDiscountRepository repo)
        {
            _Repo = repo;
        }
        #endregion

        #region get coupon
        [HttpGet("GetDisocunt/{productName}", Name = "GetDisocunt")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDisocunt(string productName)
        {
            
                var coupon = await _Repo.GetDiscount(productName);

                return Ok(coupon);

        }
        #endregion

        #region create coupon
        [HttpPost("CreateDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody]Coupon model)
        {
            await _Repo.CreateDiscount(model);

            return CreatedAtRoute("GetDisocunt", new { productName = model.ProductName }, model);
        }
        #endregion

        #region update coupon
        [HttpPut("UpdateDiscount")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody]Coupon model)
        {
            return Ok(await _Repo.UpdateDiscount(model));
        }
        #endregion

        #region delete coupon
        [HttpDelete("DeleteDiscount")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            return Ok(await _Repo.DeleteDiscount(productName));
        }
        #endregion

    }
}
