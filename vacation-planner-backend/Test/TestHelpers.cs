using DataAccess;
using DTO.DataAccess.Mappers;
using DTO.Presentation.Mappers;
using Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Test;

public static class TestHelpers
{
    public static EnvInitializer CreateEnvInitializer(int defaultVacationLength = 14)
    {
        Environment.SetEnvironmentVariable("DEFAULT_VACATION_LENGTH", defaultVacationLength.ToString());
        var env = new EnvInitializer();
        env.InitializeEnv();
        return env;
    }

    public static (AppDbContext Context, SqliteConnection Connection) CreateSqliteContext()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return (context, connection);
    }

    public static VacationRequestMapper CreateDataAccessMapper(int defaultVacationLength = 14)
        => new(CreateEnvInitializer(defaultVacationLength));

    public static VacationRequestDtoMapper CreateDtoMapper(int defaultVacationLength = 14)
        => new(CreateEnvInitializer(defaultVacationLength));
}

