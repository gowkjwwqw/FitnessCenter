using NUnit.Framework;
using FitnessCenter.Domain.Trainers;

namespace FitnessCenter.Tests.Domain;

[TestFixture]
public class TrainerTests
{
    [Test]
    public void Create_ValidData_ReturnsTrainer()
    {
        var trainer = Trainer.Create("Иван", "Иванов", "Иванович", "Йога", "+79001234567");

        Assert.That(trainer, Is.Not.Null);
    }

    [TestCase("Иван", "Иванов", "Иванович", "Йога", "+79001234567")]
    [TestCase("Анна", "Петрова", "", "Пилатес", "")]
    [TestCase("Олег", "Смирнов", null, "Кроссфит", null)]
    public void Create_ValidData_SetsPropertiesCorrectly(string firstName, string lastName, string? patronymic, string specialization, string? phone)
    {
        var trainer = Trainer.Create(firstName, lastName, patronymic!, specialization, phone!);

        Assert.That(trainer.FirstName,      Is.EqualTo(firstName));
        Assert.That(trainer.LastName,       Is.EqualTo(lastName));
        Assert.That(trainer.Specialization, Is.EqualTo(specialization));
        Assert.That(trainer.Id,             Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void Create_TwoTrainers_HaveDifferentIds()
    {
        var first  = Trainer.Create("Иван", "Иванов", "", "Йога", "");
        var second = Trainer.Create("Анна", "Петрова", "", "Пилатес", "");

        Assert.That(first.Id, Is.Not.EqualTo(second.Id));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Create_InvalidFirstName_ThrowsException(string? invalidFirstName)
    {
        var ex = Assert.Throws<Exception>(() =>
            Trainer.Create(invalidFirstName!, "Иванов", "", "Йога", ""));

        Assert.That(ex!.Message, Is.EqualTo("Имя тренера не может быть пустым"));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Create_InvalidLastName_ThrowsException(string? invalidLastName)
    {
        var ex = Assert.Throws<Exception>(() =>
            Trainer.Create("Иван", invalidLastName!, "", "Йога", ""));

        Assert.That(ex!.Message, Is.EqualTo("Фамилия тренера не может быть пустой"));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Create_InvalidSpecialization_ThrowsException(string? invalidSpecialization)
    {
        var ex = Assert.Throws<Exception>(() =>
            Trainer.Create("Иван", "Иванов", "", invalidSpecialization!, ""));

        Assert.That(ex!.Message, Is.EqualTo("Специализация тренера не может быть пустой"));
    }
}
