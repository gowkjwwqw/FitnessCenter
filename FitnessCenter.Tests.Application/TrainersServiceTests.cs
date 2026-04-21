using FitnessCenter.Application.Trainers;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Domain.Trainers;
using Moq;

namespace FitnessCenter.Tests.Application;

[TestFixture]
public class TrainersServiceTests
{
    private Mock<ITrainersRepository> _repositoryMock = null!;
    private TrainersService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITrainersRepository>();
        _service = new TrainersService(_repositoryMock.Object);
    }

    [Test]
    public void CreateTrainer_ValidData_CallsRepositoryAddOnce()
    {
        _service.CreateTrainer("Иван", "Иванов", "Иванович", "Йога", "+79001234567");

        _repositoryMock.Verify(r => r.Add(It.IsAny<Trainer>()), Times.Once);
    }

    [Test]
    public void CreateTrainer_ValidData_ReturnsNonEmptyGuid()
    {
        var id = _service.CreateTrainer("Иван", "Иванов", "Иванович", "Йога", "+79001234567");

        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void GetTrainer_ExistingId_ReturnsTrainer()
    {
        var trainer = Trainer.Create("Анна", "Петрова", "", "Пилатес", "");
        _repositoryMock.Setup(r => r.GetById(trainer.Id)).Returns(trainer);

        var result = _service.GetTrainer(trainer.Id);

        Assert.That(result, Is.EqualTo(trainer));
        _repositoryMock.Verify(r => r.GetById(trainer.Id), Times.Once);
    }

    [Test]
    public void GetTrainer_NotFound_ThrowsException()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetById(id)).Returns((Trainer)null!);

        var ex = Assert.Throws<Exception>(() => _service.GetTrainer(id));

        Assert.That(ex!.Message, Does.Contain(id.ToString()));
    }

    [Test]
    public void DeleteTrainer_ExistingId_CallsRepositoryDeleteOnce()
    {
        var trainer = Trainer.Create("Анна", "Петрова", "", "Пилатес", "");
        _repositoryMock.Setup(r => r.GetById(trainer.Id)).Returns(trainer);

        _service.DeleteTrainer(trainer.Id);

        _repositoryMock.Verify(r => r.Delete(trainer.Id), Times.Once);
    }

    [Test]
    public void DeleteTrainer_NotFound_ThrowsException()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetById(id)).Returns((Trainer)null!);

        Assert.Throws<Exception>(() => _service.DeleteTrainer(id));
    }

    [Test]
    public void GetAllTrainers_ReturnsListFromRepository()
    {
        var trainers = new List<Trainer>
        {
            Trainer.Create("Иван", "Иванов", "", "Йога", ""),
            Trainer.Create("Анна", "Петрова", "", "Пилатес", "")
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(trainers);

        var result = _service.GetAllTrainers();

        Assert.That(result.Count, Is.EqualTo(2));
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Test]
    public void SearchByLastName_CallsRepositoryWithCorrectLastName()
    {
        _repositoryMock.Setup(r => r.SearchByLastName("Иванов")).Returns(new List<Trainer>());

        _service.SearchByLastName("Иванов");

        _repositoryMock.Verify(r => r.SearchByLastName("Иванов"), Times.Once);
    }
}
