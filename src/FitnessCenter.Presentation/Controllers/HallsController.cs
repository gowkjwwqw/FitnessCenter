using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitnessCenter.Application.Halls;
using FitnessCenter.Domain.Halls;
using FitnessCenter.Presentation.Contracts;

namespace FitnessCenter.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly HallsService _service;

        public HallsController(HallsService service) { _service = service; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllHalls().Select(MapToResponse));

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            try { return Ok(MapToResponse(_service.GetHall(id))); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateHallRequest request)
        {
            try
            {
                var id = _service.CreateHall(request.Name, request.Location, request.Capacity, request.Description);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try { _service.DeleteHall(id); return NoContent(); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        private static HallResponse MapToResponse(Hall h) =>
            new(h.Id, h.Name, h.Location, h.Capacity, h.Description);
    }
}