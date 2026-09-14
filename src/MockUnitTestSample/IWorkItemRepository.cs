namespace MockUnitTestSample;

public interface IWorkItemRepository
{
    IReadOnlyList<WorkItem> FindDueBetween(DateOnly startDate, DateOnly endDate);
}
