using System;
using System.Collections.Generic;
using FitnessCenter.Domain.Trainers;

namespace FitnessCenter.Application.Trainers.Abstractions
{
    public interface ITrainersRepository
    {
        void Add(Trainer trainer);
        void Delete(Guid id);
        Trainer? GetById(Guid id);
        IReadOnlyList<Trainer> GetAll();
        IReadOnlyList<Trainer> SearchByLastName(string lastName);
    }
}