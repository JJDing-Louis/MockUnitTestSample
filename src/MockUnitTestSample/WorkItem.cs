namespace MockUnitTestSample;

public sealed class WorkItem
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string AssigneeEmail { get; init; } = string.Empty;

    public DateOnly DueDate { get; init; }

    public WorkItemStatus Status { get; init; }
}

