using System;
using System.Collections.Generic;
using System.Linq;
using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Domain.Halls;

namespace FitnessCenter.Infrastructure.Halls
{
    public sealed class HallsRepository : IHallsRepository
    {
        private readonly Dictionary<Guid, Hall> _halls = new();

        public void Add(Hall hall) => _halls[hall.Id] = hall;
        public void Delete(Guid id) => _halls.Remove(id);
        public Hall? GetById(Guid id) => _halls.GetValueOrDefault(id);
        public IReadOnlyList<Hall> GetAll() => _halls.Values.ToList();
    }
}