using FitnessCenter.Application.Halls;
using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Domain.Halls;
using Moq;

namespace FitnessCenter.Tests.Application;

[TestFixture]
public class HallsServiceTests
{
    private Mock<IHallsRepository> _repositoryMock = null!;
    private HallsService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IHallsRepository>();
        _service = new HallsService(_repositoryMock.Object);
    }

    [Test]
    public void CreateHall_ValidData_CallsRepositoryAddOnce()
    {
        _service.CreateHall("Большой зал", "1 этаж", 20, "Описание");

        _repositoryMock.Verify(r => r.Add(It.IsAny<Hall>()), Times.Once);
    }

    [Test]
    public void CreateHall_ValidData_ReturnsNonEmptyGuid()
    {
        var id = _service.CreateHall("Большой зал", "1 этаж", 20, "Описание");

        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void GetHall_ExistingId_ReturnsHall()
    {
        var hall = Hall.Create("Йога", "2 этаж", 15, "Описание");
        _repositoryMock.Setup(r => r.GetById(hall.Id)).Returns(hall);

        var result = _service.GetHall(hall.Id);

        Assert.That(result, Is.EqualTo(hall));
        _repositoryMock.Verify(r => r.GetById(hall.Id), Times.Once);
    }

    [Test]
    public void GetHall_NotFound_ThrowsException()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetById(id)).Returns((Hall)null!);

        var ex = Assert.Throws<Exception>(() => _service.GetHall(id));

        Assert.That(ex!.Message, Does.Contain(id.ToString()));
    }

    [Test]
    public void DeleteHall_ExistingId_CallsRepositoryDeleteOnce()
    {
        var hall = Hall.Create("Йога", "2 этаж", 15, "Описание");
        _repositoryMock.Setup(r => r.GetById(hall.Id)).Returns(hall);

        _service.DeleteHall(hall.Id);

        _repositoryMock.Verify(r => r.Delete(hall.Id), Times.Once);
    }

    [Test]
    public void DeleteHall_NotFound_ThrowsException()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetById(id)).Returns((Hall)null!);

        Assert.Throws<Exception>(() => _service.DeleteHall(id));
    }

    [Test]
    public void GetAllHalls_ReturnsListFromRepository()
    {
        var halls = new List<Hall>
        {
            Hall.Create("Зал 1", "1 этаж", 10, ""),
            Hall.Create("Зал 2", "2 этаж", 20, "")
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(halls);

        var result = _service.GetAllHalls();

        Assert.That(result.Count, Is.EqualTo(2));
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }
}
