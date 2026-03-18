using Base.Domain;

namespace Domain;

public class VacationRequest : BaseEntity
{ 
    public Guid EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Comment { get; set; }
    public EVacationStatus Status { get; set; }
    
    public bool IsValid => EndDate > StartDate 
                           && StartDate >= DateOnly.FromDateTime(DateTime.Today);

    public int DurationDays => EndDate.DayNumber - StartDate.DayNumber;
    public bool IsOverTime => DurationDays > 28;
}