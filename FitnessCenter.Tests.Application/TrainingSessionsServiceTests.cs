using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Application.Sessions;
using FitnessCenter.Application.Sessions.Abstractions;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Domain.Halls;
using FitnessCenter.Domain.Sessions;
using FitnessCenter.Domain.Trainers;
using Moq;

namespace FitnessCenter.Tests.Application;

[TestFixture]
public class TrainingSessionsServiceTests
{
    private Mock<ITrainingSessionsRepository> _sessionsRepositoryMock = null!;
    private Mock<ITrainersRepository> _trainersRepositoryMock = null!;
    private Mock<IHallsRepository> _hallsRepositoryMock = null!;
    private TrainingSessionsService _service = null!;

    private Trainer _trainer = null!;
    private Hall _hall = null!;
    private static readonly DateTime ValidTime = new(2026, 4, 20, 10, 0, 0);

    [SetUp]
    public void SetUp()
    {
        _sessionsRepositoryMock = new Mock<ITrainingSessionsRepository>();
        _trainersRepositoryMock = new Mock<ITrainersRepository>();
        _hallsRepositoryMock = new Mock<IHallsRepository>();

        _service = new TrainingSessionsService(
            _sessionsRepositoryMock.Object,
            _trainersRepositoryMock.Object,
            _hallsRepositoryMock.Object);

        _trainer = Trainer.Create("Иван", "Иванов", "", "Йога", "");
        _hall = Hall.Create("Большой зал", "1 этаж", 20, "");

        _trainersRepositoryMock.Setup(r => r.GetById(_trainer.Id)).Returns(_trainer);
        _hallsRepositoryMock.Setup(r => r.GetById(_hall.Id)).Returns(_hall);
        _sessionsRepositoryMock.Setup(r => r.GetByTrainerId(_trainer.Id)).Returns(new List<TrainingSession>());
        _sessionsRepositoryMock.Setup(r => r.GetByHallId(_hall.Id)).Returns(new List<TrainingSession>());
    }

    [Test]
    public void CreateSession_ValidData_CallsAddOnce()
    {
        _service.CreateSession(_trainer.Id, _hall.Id, ValidTime);

        _sessionsRepositoryMock.Verify(r => r.Add(It.IsAny<TrainingSession>()), Times.Once);
    }

    [Test]
    public void CreateSession_ValidData_ReturnsNonEmptyGuid()
    {
        var id = _service.CreateSession(_trainer.Id, _hall.Id, ValidTime);

        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void CreateSession_TrainerNotFound_ThrowsException()
    {
        var trainerId = Guid.NewGuid();
        _trainersRepositoryMock.Setup(r => r.GetById(trainerId)).Returns((Trainer)null!);

        var ex = Assert.Throws<Exception>(() => _service.CreateSession(trainerId, _hall.Id, ValidTime));

        Assert.That(ex!.Message, Does.Contain(trainerId.ToString()));
    }

    [Test]
    public void CreateSession_HallNotFound_ThrowsException()
    {
        var hallId = Guid.NewGuid();
        _hallsRepositoryMock.Setup(r => r.GetById(hallId)).Returns((Hall)null!);

        var ex = Assert.Throws<Exception>(() => _service.CreateSession(_trainer.Id, hallId, ValidTime));

        Assert.That(ex!.Message, Does.Contain(hallId.ToString()));
    }

    [Test]
    public void CreateSession_TrainerBusy_ThrowsException()
    {
        var existing = TrainingSession.Create(_trainer.Id, _hall.Id, ValidTime);
        _sessionsRepositoryMock.Setup(r => r.GetByTrainerId(_trainer.Id)).Returns(new List<TrainingSession> { existing });

        var ex = Assert.Throws<Exception>(() => _service.CreateSession(_trainer.Id, _hall.Id, ValidTime));

        Assert.That(ex!.Message, Is.EqualTo("Тренер не может проводить несколько занятий одновременно"));
    }

    [Test]
    public void CreateSession_HallBusy_ThrowsException()
    {
        var existing = TrainingSession.Create(_trainer.Id, _hall.Id, ValidTime);
        _sessionsRepositoryMock.Setup(r => r.GetByHallId(_hall.Id)).Returns(new List<TrainingSession> { existing });

        var ex = Assert.Throws<Exception>(() => _service.CreateSession(_trainer.Id, _hall.Id, ValidTime));

        Assert.That(ex!.Message, Is.EqualTo("В одном зале в один час может проводиться только одно занятие"));
    }

    [Test]
    public void DeleteSession_ExistingId_CallsDeleteOnce()
    {
        var session = TrainingSession.Create(_trainer.Id, _hall.Id, ValidTime);
        _sessionsRepositoryMock.Setup(r => r.GetById(session.Id)).Returns(session);

        _service.DeleteSession(session.Id);

        _sessionsRepositoryMock.Verify(r => r.Delete(session.Id), Times.Once);
    }

    [Test]
    public void DeleteSession_NotFound_ThrowsException()
    {
        var id = Guid.NewGuid();
        _sessionsRepositoryMock.Setup(r => r.GetById(id)).Returns((TrainingSession)null!);

        Assert.Throws<Exception>(() => _service.DeleteSession(id));
    }

    [Test]
    public void GetTrainerScheduleByDate_ReturnsOnlySpecifiedDateOrdered()
    {
        var date = new DateTime(2026, 4, 20);
        var sessions = new List<TrainingSession>
        {
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 20, 12, 0, 0)),
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 19, 10, 0, 0)),
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 20, 9, 0, 0))
        };
        _sessionsRepositoryMock.Setup(r => r.GetByTrainerId(_trainer.Id)).Returns(sessions);

        var result = _service.GetTrainerScheduleByDate(_trainer.Id, date);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].StartTime, Is.EqualTo(new DateTime(2026, 4, 20, 9, 0, 0)));
        Assert.That(result[1].StartTime, Is.EqualTo(new DateTime(2026, 4, 20, 12, 0, 0)));
    }

    [Test]
    public void GetHallOccupancyByDate_ReturnsOnlySpecifiedDateOrdered()
    {
        var date = new DateTime(2026, 4, 20);
        var sessions = new List<TrainingSession>
        {
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 20, 18, 0, 0)),
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 18, 10, 0, 0)),
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 20, 8, 0, 0))
        };
        _sessionsRepositoryMock.Setup(r => r.GetByHallId(_hall.Id)).Returns(sessions);

        var result = _service.GetHallOccupancyByDate(_hall.Id, date);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].StartTime, Is.EqualTo(new DateTime(2026, 4, 20, 8, 0, 0)));
        Assert.That(result[1].StartTime, Is.EqualTo(new DateTime(2026, 4, 20, 18, 0, 0)));
    }

    [Test]
    public void GetAllSessions_ReturnsListFromRepository()
    {
        var sessions = new List<TrainingSession>
        {
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 20, 10, 0, 0)),
            TrainingSession.Create(_trainer.Id, _hall.Id, new DateTime(2026, 4, 20, 11, 0, 0))
        };
        _sessionsRepositoryMock.Setup(r => r.GetAll()).Returns(sessions);

        var result = _service.GetAllSessions();

        Assert.That(result.Count, Is.EqualTo(2));
        _sessionsRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }
}
