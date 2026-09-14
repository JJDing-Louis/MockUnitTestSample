using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace MockUnitTestSample.Tests;

[TestFixture]
public sealed class WorkItemReminderServiceTests
{
    private static readonly DateOnly Today = new(2026, 9, 14);

    [Test]
    public void SendDueSoonReminders_TwoDueItems_SendsOneReminderPerItemAndReturnsTwo()
    {
        // Arrange
        IReadOnlyList<WorkItem> dueItems = WorkItemFakerFactory.Create(Today).Generate(2);
        var repository = new Mock<IWorkItemRepository>();
        repository
            .Setup(instance => instance.FindDueBetween(Today, Today.AddDays(3)))
            .Returns(dueItems);
        var notificationService = new Mock<INotificationService>();
        var sut = new WorkItemReminderService(repository.Object, notificationService.Object);

        // Act
        int sentCount = sut.SendDueSoonReminders(Today, daysAhead: 3);

        // Assert
        sentCount.Should().Be(2);
        repository.Verify(
            instance => instance.FindDueBetween(Today, Today.AddDays(3)),
            Times.Once);
        repository.VerifyNoOtherCalls();

        foreach (WorkItem item in dueItems)
        {
            notificationService.Verify(
                instance => instance.SendDueDateReminder(
                    item.AssigneeEmail,
                    item.Title,
                    item.DueDate),
                Times.Once);
        }

        notificationService.VerifyNoOtherCalls();
    }

    [Test]
    public void SendDueSoonReminders_NoDueItems_DoesNotSendReminderAndReturnsZero()
    {
        // Arrange
        var repository = new Mock<IWorkItemRepository>();
        repository
            .Setup(instance => instance.FindDueBetween(Today, Today.AddDays(3)))
            .Returns(Array.Empty<WorkItem>());
        var notificationService = new Mock<INotificationService>();
        var sut = new WorkItemReminderService(repository.Object, notificationService.Object);

        // Act
        int sentCount = sut.SendDueSoonReminders(Today, daysAhead: 3);

        // Assert
        sentCount.Should().Be(0);
        repository.Verify(
            instance => instance.FindDueBetween(Today, Today.AddDays(3)),
            Times.Once);
        repository.VerifyNoOtherCalls();
        notificationService.VerifyNoOtherCalls();
    }

    [TestCase(-1, TestName = "SendDueSoonReminders_DaysAheadBelowMinimum_ThrowsBeforeCallingDependencies")]
    [TestCase(31, TestName = "SendDueSoonReminders_DaysAheadAboveMaximum_ThrowsBeforeCallingDependencies")]
    public void SendDueSoonReminders_InvalidDaysAhead_ThrowsBeforeCallingDependencies(
        int daysAhead)
    {
        // Arrange
        var repository = new Mock<IWorkItemRepository>();
        var notificationService = new Mock<INotificationService>();
        var sut = new WorkItemReminderService(repository.Object, notificationService.Object);

        // Act
        Action act = () => sut.SendDueSoonReminders(Today, daysAhead);

        // Assert
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        repository.VerifyNoOtherCalls();
        notificationService.VerifyNoOtherCalls();
    }

    [TestCase(0, TestName = "SendDueSoonReminders_ZeroDaysAhead_PassesSameStartAndEndDateToRepository")]
    [TestCase(30, TestName = "SendDueSoonReminders_ThirtyDaysAhead_PassesMaximumDateRangeToRepository")]
    public void SendDueSoonReminders_ValidBoundary_PassesExpectedDateRangeToRepository(
        int daysAhead)
    {
        // Arrange
        var repository = new Mock<IWorkItemRepository>();
        repository
            .Setup(instance => instance.FindDueBetween(Today, Today.AddDays(daysAhead)))
            .Returns(Array.Empty<WorkItem>());
        var notificationService = new Mock<INotificationService>();
        var sut = new WorkItemReminderService(repository.Object, notificationService.Object);

        // Act
        int sentCount = sut.SendDueSoonReminders(Today, daysAhead);

        // Assert
        sentCount.Should().Be(0);
        repository.Verify(
            instance => instance.FindDueBetween(Today, Today.AddDays(daysAhead)),
            Times.Once);
        repository.VerifyNoOtherCalls();
        notificationService.VerifyNoOtherCalls();
    }

    [Test]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Arrange
        var notificationService = new Mock<INotificationService>();

        // Act
        Action act = () => new WorkItemReminderService(null!, notificationService.Object);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
        notificationService.VerifyNoOtherCalls();
    }

    [Test]
    public void Constructor_NullNotificationService_ThrowsArgumentNullException()
    {
        // Arrange
        var repository = new Mock<IWorkItemRepository>();

        // Act
        Action act = () => new WorkItemReminderService(repository.Object, null!);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
        repository.VerifyNoOtherCalls();
    }
}
