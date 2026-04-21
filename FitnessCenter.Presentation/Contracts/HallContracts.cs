using System;

namespace FitnessCenter.Presentation.Contracts
{
    public record CreateHallRequest(string Name, string Location, int Capacity, string Description);
    public record HallResponse(Guid Id, string Name, string Location, int Capacity, string Description);
}