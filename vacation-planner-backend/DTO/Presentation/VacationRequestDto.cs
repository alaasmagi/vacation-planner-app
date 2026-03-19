using Base.Domain;
using Domain;

namespace DTO.Presentation;

public class VacationRequestDto : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Comment { get; set; }
    public EVacationStatus Status { get; set; }
    public int? DurationDays {get; set;}
    public bool? IsOverTime {get; set;}
}