using System;
using System.Collections.Generic;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Domain.Trainers;

namespace FitnessCenter.Application.Trainers
{
    public sealed class TrainersService
    {
        private readonly ITrainersRepository _repository;

        public TrainersService(ITrainersRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateTrainer(string firstName, string lastName, string patronymic, string specialization, string phone)
        {
            var trainer = Trainer.Create(firstName, lastName, patronymic, specialization, phone);
            _repository.Add(trainer);
            return trainer.Id;
        }

        public void DeleteTrainer(Guid id)
        {
            var trainer = _repository.GetById(id);
            if (trainer == null) throw new Exception($"Тренер с id={id} не найден");
            _repository.Delete(id);
        }

        public Trainer GetTrainer(Guid id)
        {
            var trainer = _repository.GetById(id);
            if (trainer == null) throw new Exception($"Тренер с id={id} не найден");
            return trainer;
        }

        public IReadOnlyList<Trainer> GetAllTrainers() => _repository.GetAll();
        public IReadOnlyList<Trainer> SearchByLastName(string lastName) => _repository.SearchByLastName(lastName);
    }
}