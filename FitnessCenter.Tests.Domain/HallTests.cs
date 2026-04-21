using FitnessCenter.Domain.Halls;

namespace FitnessCenter.Tests.Domain;

[TestFixture]
public class HallTests
{
    [Test]
    public void Create_ValidData_ReturnsHall()
    {
        var hall = Hall.Create("Большой зал", "1 этаж", 20, "Для групповых тренировок");

        Assert.That(hall, Is.Not.Null);
    }

    [TestCase("Большой зал", "1 этаж", 20, "Описание")]
    [TestCase("Йога", "2 этаж", 10, "")]
    [TestCase("Кардио", "Подвал", 50, null)]
    public void Create_ValidData_SetsPropertiesCorrectly(string name, string location, int capacity, string description)
    {
        var hall = Hall.Create(name, location, capacity, description);

        Assert.That(hall.Name, Is.EqualTo(name));
        Assert.That(hall.Location, Is.EqualTo(location));
        Assert.That(hall.Capacity, Is.EqualTo(capacity));
        Assert.That(hall.Description, Is.EqualTo(description));
        Assert.That(hall.Id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void Create_TwoHalls_HaveDifferentIds()
    {
        var first = Hall.Create("Зал 1", "1 этаж", 10, "");
        var second = Hall.Create("Зал 2", "2 этаж", 15, "");

        Assert.That(first.Id, Is.Not.EqualTo(second.Id));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Create_InvalidName_ThrowsException(string invalidName)
    {
        var ex = Assert.Throws<Exception>(() =>
            Hall.Create(invalidName, "1 этаж", 20, "Описание"));

        Assert.That(ex!.Message, Is.EqualTo("Название зала не может быть пустым"));
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public void Create_InvalidCapacity_ThrowsException(int invalidCapacity)
    {
        var ex = Assert.Throws<Exception>(() =>
            Hall.Create("Большой зал", "1 этаж", invalidCapacity, "Описание"));

        Assert.That(ex!.Message, Is.EqualTo("Вместимость зала должна быть больше нуля"));
    }
}
