namespace MockUnitTestSample;

public interface INotificationService
{
    void SendDueDateReminder(
        string recipientEmail,
        string workItemTitle,
        DateOnly dueDate);
}
