using Microsoft.AspNetCore.Mvc;
using OZeroBlackjack.Business;
using OZeroBlackjack.Business.RequestModel;
using System.Linq;

namespace OZeroBlackjack.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IDecisionProcessor _processor;

        public PlayerController()
        {
            _processor = new DecisionProcessor();
        }

        [HttpGet]
        public IActionResult PlayTurn([FromQuery] string cards, [FromQuery]string money, [FromQuery]string cardHistory)
        {
            var parameters = new TurnParams
            {
                CurrentCards = cards.Split(',').Select(x => x.ToUpperInvariant()).ToArray(),
                CardHistory = cardHistory.Split(',').Select(x => x.ToUpperInvariant()).ToArray(),
                MoneyInHand = int.Parse(money)
            };

            return Ok(_processor.Process(parameters));
        }
    }
}