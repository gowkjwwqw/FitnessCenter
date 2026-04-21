using System;

namespace FitnessCenter.Domain.Halls
{
    public sealed class Hall
    {
        private Hall(Guid id, string name, string location, int capacity, string description)
        {
            Id = id; Name = name; Location = location; Capacity = capacity; Description = description;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }
        public int Capacity { get; private set; }
        public string Description { get; private set; }

        public static Hall Create(string name, string location, int capacity, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Название зала не может быть пустым");
            if (capacity <= 0) throw new Exception("Вместимость зала должна быть больше нуля");
            return new Hall(Guid.NewGuid(), name, location, capacity, description);
        }
    }
}