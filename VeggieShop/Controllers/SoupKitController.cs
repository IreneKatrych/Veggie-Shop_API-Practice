using Microsoft.AspNetCore.Mvc;
using VeggieShop.Interfaces;
using VeggieShop.Models;

namespace VeggieShop.Controllers
{
    [ApiController]
    public class SoupKitController : ControllerBase
    {
        private readonly IProcessingService _processingService;

        public SoupKitController(IProcessingService processingService)
        {
            _processingService = processingService;
        }

        [HttpGet("compose")]
        public IActionResult GetSoupKit([FromQuery] double weight)
        {
            var soupKit = _processingService.GetSoupKit(weight);

            if (soupKit is null)
            {
                return Problem("There are no available vegetables for the soupkit.", "Vegetable", StatusCodes.Status406NotAcceptable);
            }

            return Ok(soupKit);
        }
    }
}
