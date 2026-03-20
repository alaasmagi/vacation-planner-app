using Domain;

namespace Test;

public class DomainTest
{
    [Test]
    public void IsValid_ShouldBeTrue_WhenDatesAreInFutureAndEndAfterStart()
    {
        var vacationRequest = new VacationRequest(defaultVacationLength: 10)
        {
            StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
        };

        Assert.That(vacationRequest.IsValid, Is.True);
    }

    [Test]
    public void IsValid_ShouldBeFalse_WhenEndDateIsSameAsStartDate()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(2));
        var vacationRequest = new VacationRequest(defaultVacationLength: 10)
        {
            StartDate = date,
            EndDate = date
        };

        Assert.That(vacationRequest.IsValid, Is.False);
    }

    [Test]
    public void IsValid_ShouldBeFalse_WhenStartDateIsInPast()
    {
        var vacationRequest = new VacationRequest(defaultVacationLength: 10)
        {
            StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
        };

        Assert.That(vacationRequest.IsValid, Is.False);
    }

    [Test]
    public void DurationDays_ShouldReturnDifferenceInDays()
    {
        var vacationRequest = new VacationRequest(defaultVacationLength: 10)
        {
            StartDate = new DateOnly(2026, 04, 10),
            EndDate = new DateOnly(2026, 04, 15)
        };

        Assert.That(vacationRequest.DurationDays, Is.EqualTo(5));
    }

    [Test]
    public void IsOverTime_ShouldBeTrue_WhenDurationExceedsDefaultVacationLength()
    {
        var vacationRequest = new VacationRequest(defaultVacationLength: 3)
        {
            StartDate = new DateOnly(2026, 05, 01),
            EndDate = new DateOnly(2026, 05, 06)
        };

        Assert.That(vacationRequest.IsOverTime, Is.True);
    }

    [Test]
    public void IsOverTime_ShouldBeFalse_WhenDurationEqualsDefaultVacationLength()
    {
        var vacationRequest = new VacationRequest(defaultVacationLength: 5)
        {
            StartDate = new DateOnly(2026, 06, 01),
            EndDate = new DateOnly(2026, 06, 06)
        };

        Assert.That(vacationRequest.IsOverTime, Is.False);
    }
}