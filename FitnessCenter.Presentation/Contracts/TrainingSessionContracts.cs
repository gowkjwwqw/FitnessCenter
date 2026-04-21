using System;

namespace FitnessCenter.Presentation.Contracts
{
    public record CreateTrainingSessionRequest(Guid TrainerId, Guid HallId, DateTime StartTime);
    public record TrainingSessionResponse(Guid Id, Guid TrainerId, string TrainerFullName, Guid HallId, string HallName, DateTime StartTime, DateTime EndTime);
}