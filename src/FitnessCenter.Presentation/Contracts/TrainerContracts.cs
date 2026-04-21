using System;

namespace FitnessCenter.Presentation.Contracts
{
    public record CreateTrainerRequest(string FirstName, string LastName, string Patronymic, string Specialization, string Phone);
    public record TrainerResponse(Guid Id, string FirstName, string LastName, string Patronymic, string Specialization, string Phone);
}