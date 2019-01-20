using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WollisXExercise.Models;
using WollisXExercise.Proxies;

namespace WollisXExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrolleyController : ControllerBase
    {
        private readonly ITrolleyProxy _trolleyProxy;

        public TrolleyController(ITrolleyProxy trolleyProxy)
        {
            _trolleyProxy = trolleyProxy;
        }

        [HttpPost("trolleyTotal")]
        public async Task<ActionResult<decimal>> CalculateTrolley([FromBody]TrolleyCalculatorRequest request)
        {
            var result = await _trolleyProxy.CalculateTrolley(request);

            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Content);
        }
    }
}
