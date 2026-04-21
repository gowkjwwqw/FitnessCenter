using System;
using System.Collections.Generic;
using FitnessCenter.Domain.Halls;

namespace FitnessCenter.Application.Halls.Abstractions
{
    public interface IHallsRepository
    {
        void Add(Hall hall);
        void Delete(Guid id);
        Hall? GetById(Guid id);
        IReadOnlyList<Hall> GetAll();
    }
}