using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Application.Sessions;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Domain.Sessions;
using FitnessCenter.Presentation.Contracts;

namespace FitnessCenter.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingSessionsController : ControllerBase
    {
        private readonly TrainingSessionsService _service;
        private readonly ITrainersRepository _trainersRepository;
        private readonly IHallsRepository _hallsRepository;

        public TrainingSessionsController(TrainingSessionsService service, ITrainersRepository trainersRepository, IHallsRepository hallsRepository)
        {
            _service = service;
            _trainersRepository = trainersRepository;
            _hallsRepository = hallsRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTrainingSessionRequest request)
        {
            try { var id = _service.CreateSession(request.TrainerId, request.HallId, request.StartTime); return Ok(new { id }); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try { _service.DeleteSession(id); return NoContent(); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllSessions().Select(MapToResponse));

        [HttpGet("trainer/{trainerId:guid}")]
        public IActionResult GetTrainerSchedule(Guid trainerId, [FromQuery] DateTime date) =>
            Ok(_service.GetTrainerScheduleByDate(trainerId, date).Select(MapToResponse));

        [HttpGet("hall/{hallId:guid}")]
        public IActionResult GetHallOccupancy(Guid hallId, [FromQuery] DateTime date) =>
            Ok(_service.GetHallOccupancyByDate(hallId, date).Select(MapToResponse));

        private TrainingSessionResponse MapToResponse(TrainingSession s)
        {
            var trainer = _trainersRepository.GetById(s.TrainerId);
            var hall = _hallsRepository.GetById(s.HallId);
            return new TrainingSessionResponse(
                s.Id, s.TrainerId,
                trainer == null ? "—" : $"{trainer.LastName} {trainer.FirstName} {trainer.Patronymic}",
                s.HallId,
                hall == null ? "—" : hall.Name,
                s.StartTime, s.EndTime);
        }
    }
}