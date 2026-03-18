using DTO.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<VacationRequestEntity> VacationRequests { get; set; }
}