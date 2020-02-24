using Microsoft.AspNetCore.Mvc;

namespace OZeroBlackjack.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        [HttpGet]
        public void Get([FromQuery] string cards, string money, string cardHistory)
        {
        }
    }
}