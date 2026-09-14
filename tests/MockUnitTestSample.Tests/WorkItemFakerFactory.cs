using Bogus;

namespace MockUnitTestSample.Tests;

public static class WorkItemFakerFactory
{
    public static Faker<WorkItem> Create(DateOnly today)
    {
        return new Faker<WorkItem>("zh_TW")
            .StrictMode(true)
            .UseSeed(2026)
            .RuleFor(item => item.Id, faker => faker.Random.Int(1, 10_000))
            .RuleFor(item => item.Title, faker => faker.Lorem.Sentence(5))
            .RuleFor(item => item.AssigneeEmail, faker => faker.Internet.Email())
            .RuleFor(item => item.DueDate, faker => today.AddDays(faker.Random.Int(0, 3)))
            .RuleFor(item => item.Status, WorkItemStatus.InProgress);
    }
}

