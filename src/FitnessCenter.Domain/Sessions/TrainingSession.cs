using System;

namespace FitnessCenter.Domain.Sessions
{
    public sealed class TrainingSession
    {
        private static readonly TimeSpan SlotDuration = TimeSpan.FromHours(1);
        private static readonly TimeSpan WorkdayStart = new TimeSpan(8, 0, 0);
        private static readonly TimeSpan WorkdayEnd = new TimeSpan(21, 0, 0);

        private TrainingSession(Guid id, Guid trainerId, Guid hallId, DateTime startTime)
        {
            Id = id; TrainerId = trainerId; HallId = hallId; StartTime = startTime;
        }

        public Guid Id { get; private set; }
        public Guid TrainerId { get; private set; }
        public Guid HallId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime => StartTime.Add(SlotDuration);

        public static TrainingSession Create(Guid trainerId, Guid hallId, DateTime startTime)
        {
            if (startTime.Minute != 0 || startTime.Second != 0)
                throw new Exception("Занятие может начинаться только ровно в начале часа");

            var time = startTime.TimeOfDay;
            if (time < WorkdayStart || time >= WorkdayEnd)
                throw new Exception("Занятие должно быть назначено в рабочее время с 08:00 до 21:00");

            return new TrainingSession(Guid.NewGuid(), trainerId, hallId, startTime);
        }
    }
}