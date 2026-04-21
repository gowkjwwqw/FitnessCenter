using FitnessCenter.Domain.Sessions;

namespace FitnessCenter.Tests.Domain;

[TestFixture]
public class TrainingSessionTests
{
    private static readonly Guid TrainerId = Guid.NewGuid();
    private static readonly Guid HallId = Guid.NewGuid();

    [TestCase(8)]
    [TestCase(9)]
    [TestCase(14)]
    [TestCase(20)]
    public void Create_ValidStartTime_ReturnsSession(int hour)
    {
        var startTime = new DateTime(2026, 4, 20, hour, 0, 0);

        var session = TrainingSession.Create(TrainerId, HallId, startTime);

        Assert.That(session, Is.Not.Null);
        Assert.That(session.StartTime, Is.EqualTo(startTime));
    }

    [Test]
    public void Create_ValidSession_EndTimeIsOneHourLater()
    {
        var startTime = new DateTime(2026, 4, 20, 10, 0, 0);

        var session = TrainingSession.Create(TrainerId, HallId, startTime);

        Assert.That(session.EndTime, Is.EqualTo(startTime.AddHours(1)));
    }

    [Test]
    public void Create_TwoSessions_HaveDifferentIds()
    {
        var first = TrainingSession.Create(TrainerId, HallId, new DateTime(2026, 4, 20, 10, 0, 0));
        var second = TrainingSession.Create(TrainerId, HallId, new DateTime(2026, 4, 20, 11, 0, 0));

        Assert.That(first.Id, Is.Not.EqualTo(second.Id));
    }

    [TestCase(8, 30)]
    [TestCase(10, 15)]
    [TestCase(12, 45)]
    public void Create_StartTimeNotAtBeginningOfHour_ThrowsException(int hour, int minute)
    {
        var startTime = new DateTime(2026, 4, 20, hour, minute, 0);

        var ex = Assert.Throws<Exception>(() =>
            TrainingSession.Create(TrainerId, HallId, startTime));

        Assert.That(ex!.Message, Is.EqualTo("Занятие может начинаться только ровно в начале часа"));
    }

    [TestCase(7)]
    [TestCase(21)]
    [TestCase(22)]
    public void Create_StartTimeOutsideWorkingHours_ThrowsException(int hour)
    {
        var startTime = new DateTime(2026, 4, 20, hour, 0, 0);

        var ex = Assert.Throws<Exception>(() =>
            TrainingSession.Create(TrainerId, HallId, startTime));

        Assert.That(ex!.Message, Is.EqualTo("Занятие должно быть назначено в рабочее время с 08:00 до 21:00"));
    }
}
