using Microsoft.AspNetCore.Mvc;
using VeggieShop.Interfaces;
using VeggieShop.Models;

namespace VeggieShop.Controllers
{
    [ApiController]
    [Route("vegetable")]
    public class VegetableController : ControllerBase
    {
        private readonly IVeggieService _veggieService;
        private readonly IProcessingService _processingService;

        public VegetableController(IVeggieService veggieService, IProcessingService processingService)
        {
            _veggieService = veggieService;
            _processingService = processingService;
        }

        [HttpGet]
        public IActionResult GetAllVegetables()
        {
            var veggieData = _veggieService.GetAll();
            return Ok(veggieData);
        }

        [HttpGet("{id}")]
        public IActionResult GetVegetableById(Guid id, [FromQuery] bool sliced)
        {
            var vegie = _veggieService.GetById(id);

            if (vegie is null)
            {
                return NotFound();
            }
                 
            return sliced ? Ok(_processingService.GetSlisedVegetable(vegie)) : Ok(vegie);
        }

        [HttpPost]
        public IActionResult AddVegetable(VegetableDetailed vegetable)
        {
            if (_veggieService.NameExists(vegetable.Name, null))
            {
                return Problem("Vegetable with same name already exists.", "Vegetable", StatusCodes.Status403Forbidden);
            }

            _veggieService.Insert(vegetable);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVegetable(Guid id, VegetableDetailed vegetable)
        { 
            if (_veggieService.IsLocked(id)) 
            {
                return Problem("Vegetable is already locked.", "Vegetable", StatusCodes.Status423Locked, "Locked");
            }

            if (_veggieService.NameExists(vegetable.Name, id))
            {
                return Problem("Vegetable with same name already exists.", "Vegetable", StatusCodes.Status403Forbidden);
            }

            _veggieService.Update(id, vegetable);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVegetable(Guid id)
        {
            return _veggieService.Delete(id) ? NoContent() : NotFound();
        }

        [HttpPost("lock/{id}")]
        public IActionResult LockVegetable(Guid id)
        {
            if (_veggieService.IsLocked(id))
            {
                return Problem("Vegetable is already locked.", "Vegetable", StatusCodes.Status423Locked, "Locked");
            }

            _veggieService.SetLock(id, true);
            return Ok();
        }

        [HttpPost("unlock/{id}")]
        public IActionResult UnlockVegetable(Guid id)
        {
            _veggieService.SetLock(id, false);
            return Ok();
        }

    }
}
