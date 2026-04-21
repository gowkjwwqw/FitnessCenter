using System;
using System.Collections.Generic;
using FitnessCenter.Domain.Sessions;

namespace FitnessCenter.Application.Sessions.Abstractions
{
    public interface ITrainingSessionsRepository
    {
        void Add(TrainingSession session);
        void Delete(Guid id);
        TrainingSession? GetById(Guid id);
        IReadOnlyList<TrainingSession> GetAll();
        IReadOnlyList<TrainingSession> GetByTrainerId(Guid trainerId);
        IReadOnlyList<TrainingSession> GetByHallId(Guid hallId);
        IReadOnlyList<TrainingSession> GetByDate(DateTime date);
    }
}