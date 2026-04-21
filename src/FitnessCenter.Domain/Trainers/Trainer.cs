using System;

namespace FitnessCenter.Domain.Trainers
{
    public sealed class Trainer
    {
        private Trainer(Guid id, string firstName, string lastName, string patronymic, string specialization, string phone)
        {
            Id = id; FirstName = firstName; LastName = lastName;
            Patronymic = patronymic; Specialization = specialization; Phone = phone;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Patronymic { get; private set; }
        public string Specialization { get; private set; }
        public string Phone { get; private set; }

        public static Trainer Create(string firstName, string lastName, string patronymic, string specialization, string phone)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new Exception("Имя тренера не может быть пустым");
            if (string.IsNullOrWhiteSpace(lastName)) throw new Exception("Фамилия тренера не может быть пустой");
            if (string.IsNullOrWhiteSpace(specialization)) throw new Exception("Специализация тренера не может быть пустой");
            return new Trainer(Guid.NewGuid(), firstName, lastName, patronymic, specialization, phone);
        }
    }
}