using System;
using System.Collections.Generic;
using System.Linq;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Domain.Trainers;

namespace FitnessCenter.Infrastructure.Trainers
{
    public sealed class TrainersRepository : ITrainersRepository
    {
        private readonly Dictionary<Guid, Trainer> _trainers = new();

        public void Add(Trainer trainer) => _trainers[trainer.Id] = trainer;
        public void Delete(Guid id) => _trainers.Remove(id);
        public Trainer? GetById(Guid id) => _trainers.GetValueOrDefault(id);
        public IReadOnlyList<Trainer> GetAll() => _trainers.Values.ToList();
        public IReadOnlyList<Trainer> SearchByLastName(string lastName) =>
            _trainers.Values.Where(x => x.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}