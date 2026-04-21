using System;
using System.Collections.Generic;
using System.Linq;
using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Application.Sessions.Abstractions;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Domain.Sessions;

namespace FitnessCenter.Application.Sessions
{
    public sealed class TrainingSessionsService
    {
        private readonly ITrainingSessionsRepository _sessionsRepository;
        private readonly ITrainersRepository _trainersRepository;
        private readonly IHallsRepository _hallsRepository;

        public TrainingSessionsService(
            ITrainingSessionsRepository sessionsRepository,
            ITrainersRepository trainersRepository,
            IHallsRepository hallsRepository)
        {
            _sessionsRepository = sessionsRepository;
            _trainersRepository = trainersRepository;
            _hallsRepository = hallsRepository;
        }

        public Guid CreateSession(Guid trainerId, Guid hallId, DateTime startTime)
        {
            var trainer = _trainersRepository.GetById(trainerId);
            if (trainer == null) throw new Exception($"Тренер с id={trainerId} не найден");

            var hall = _hallsRepository.GetById(hallId);
            if (hall == null) throw new Exception($"Зал с id={hallId} не найден");

            var trainerBusy = _sessionsRepository.GetByTrainerId(trainerId).Any(x => x.StartTime == startTime);
            if (trainerBusy) throw new Exception("Тренер не может проводить несколько занятий одновременно");

            var hallBusy = _sessionsRepository.GetByHallId(hallId).Any(x => x.StartTime == startTime);
            if (hallBusy) throw new Exception("В одном зале в один час может проводиться только одно занятие");

            var session = TrainingSession.Create(trainerId, hallId, startTime);
            _sessionsRepository.Add(session);
            return session.Id;
        }

        public void DeleteSession(Guid id)
        {
            var session = _sessionsRepository.GetById(id);
            if (session == null) throw new Exception($"Занятие с id={id} не найдено");
            _sessionsRepository.Delete(id);
        }

        public IReadOnlyList<TrainingSession> GetTrainerScheduleByDate(Guid trainerId, DateTime date) =>
            _sessionsRepository.GetByTrainerId(trainerId).Where(x => x.StartTime.Date == date.Date).OrderBy(x => x.StartTime).ToList();

        public IReadOnlyList<TrainingSession> GetHallOccupancyByDate(Guid hallId, DateTime date) =>
            _sessionsRepository.GetByHallId(hallId).Where(x => x.StartTime.Date == date.Date).OrderBy(x => x.StartTime).ToList();

        public IReadOnlyList<TrainingSession> GetAllSessions() => _sessionsRepository.GetAll();
    }
}