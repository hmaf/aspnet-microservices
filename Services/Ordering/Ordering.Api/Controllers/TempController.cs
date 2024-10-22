using EventBus.Message.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Controllers
{
    [Route("api/Temp")]
    [ApiController]
    public class TempController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TempController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(string name)
        {
            await _publishEndpoint.Publish(new HadiTestEvent { Name = name});

            return Accepted();
        }

        [HttpPost("add2")]
        public async Task<IActionResult> Add2(string name, string family)
        {
            await _publishEndpoint.Publish(new HadiTestEvent2 { Name = name, Family = family});

            return Accepted();
        }
    }
}
