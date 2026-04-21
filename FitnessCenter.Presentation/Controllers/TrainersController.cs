using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitnessCenter.Application.Trainers;
using FitnessCenter.Domain.Trainers;
using FitnessCenter.Presentation.Contracts;

namespace FitnessCenter.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly TrainersService _service;

        public TrainersController(TrainersService service) { _service = service; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllTrainers().Select(MapToResponse));

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            try { return Ok(MapToResponse(_service.GetTrainer(id))); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string lastName) =>
            Ok(_service.SearchByLastName(lastName).Select(MapToResponse));

        [HttpPost]
        public IActionResult Create([FromBody] CreateTrainerRequest request)
        {
            try
            {
                var id = _service.CreateTrainer(request.FirstName, request.LastName, request.Patronymic, request.Specialization, request.Phone);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try { _service.DeleteTrainer(id); return NoContent(); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        private static TrainerResponse MapToResponse(Trainer t) =>
            new(t.Id, t.FirstName, t.LastName, t.Patronymic, t.Specialization, t.Phone);
    }
}