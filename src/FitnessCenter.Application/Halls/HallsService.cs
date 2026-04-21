using System;
using System.Collections.Generic;
using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Domain.Halls;

namespace FitnessCenter.Application.Halls
{
    public sealed class HallsService
    {
        private readonly IHallsRepository _repository;

        public HallsService(IHallsRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateHall(string name, string location, int capacity, string description)
        {
            var hall = Hall.Create(name, location, capacity, description);
            _repository.Add(hall);
            return hall.Id;
        }

        public void DeleteHall(Guid id)
        {
            var hall = _repository.GetById(id);
            if (hall == null) throw new Exception($"Зал с id={id} не найден");
            _repository.Delete(id);
        }

        public Hall GetHall(Guid id)
        {
            var hall = _repository.GetById(id);
            if (hall == null) throw new Exception($"Зал с id={id} не найден");
            return hall;
        }

        public IReadOnlyList<Hall> GetAllHalls() => _repository.GetAll();
    }
}