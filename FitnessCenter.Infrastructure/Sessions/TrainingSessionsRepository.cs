using System;
using System.Collections.Generic;
using System.Linq;
using FitnessCenter.Application.Sessions.Abstractions;
using FitnessCenter.Domain.Sessions;

namespace FitnessCenter.Infrastructure.Sessions
{
    public sealed class TrainingSessionsRepository : ITrainingSessionsRepository
    {
        private readonly Dictionary<Guid, TrainingSession> _sessions = new();

        public void Add(TrainingSession session) => _sessions[session.Id] = session;
        public void Delete(Guid id) => _sessions.Remove(id);
        public TrainingSession? GetById(Guid id) => _sessions.GetValueOrDefault(id);
        public IReadOnlyList<TrainingSession> GetAll() => _sessions.Values.ToList();
        public IReadOnlyList<TrainingSession> GetByTrainerId(Guid trainerId) =>
            _sessions.Values.Where(x => x.TrainerId == trainerId).ToList();
        public IReadOnlyList<TrainingSession> GetByHallId(Guid hallId) =>
            _sessions.Values.Where(x => x.HallId == hallId).ToList();
        public IReadOnlyList<TrainingSession> GetByDate(DateTime date) =>
            _sessions.Values.Where(x => x.StartTime.Date == date.Date).ToList();
    }
}