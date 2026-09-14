namespace MockUnitTestSample;

public sealed class WorkItemReminderService
{
    private const int MaximumDaysAhead = 30;
    private readonly IWorkItemRepository _repository;
    private readonly INotificationService _notificationService;

    public WorkItemReminderService(
        IWorkItemRepository repository,
        INotificationService notificationService)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(notificationService);

        _repository = repository;
        _notificationService = notificationService;
    }

    public int SendDueSoonReminders(DateOnly today, int daysAhead)
    {
        if (daysAhead is < 0 or > MaximumDaysAhead)
        {
            throw new ArgumentOutOfRangeException(
                nameof(daysAhead),
                $"提醒天數必須介於 0 到 {MaximumDaysAhead} 天之間。");
        }

        DateOnly endDate = today.AddDays(daysAhead);
        IReadOnlyList<WorkItem> dueItems =
            _repository.FindDueBetween(today, endDate);

        foreach (WorkItem item in dueItems)
        {
            _notificationService.SendDueDateReminder(
                item.AssigneeEmail,
                item.Title,
                item.DueDate);
        }

        return dueItems.Count;
    }
}

