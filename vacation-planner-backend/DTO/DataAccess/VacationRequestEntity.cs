using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Domain;

namespace DTO.DataAccess;

public class VacationRequestEntity : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    [MaxLength(512)]
    public string? Comment { get; set; }
    public EVacationStatus Status { get; set; }
    
    [MaxLength(128)]
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    [MaxLength(128)]
    public string UpdatedBy { get; set; } = default!;
    public DateTime UpdatedAt { get; set; }
}