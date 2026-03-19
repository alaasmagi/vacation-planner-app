using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Domain;

namespace DTO.DataAccess;

public class VacationRequestEntity : BaseEntityWithMeta
{
    public Guid EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    [MaxLength(512)]
    public string? Comment { get; set; }
    public EVacationStatus Status { get; set; }
}