using Domain;

namespace DTO.Presentation;

public class VacationRequestWebDto
{
    public Guid? Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Comment { get; set; }
    public EVacationStatus? Status { get; set; }
    public int? DurationDays {get; set;}
    public bool? IsOverTime {get; set;}
}