namespace Helpers;

public class EnvInitializer
{
    public string DbConnection { get; private set; } = string.Empty;
    public string DbConnectionDevelopment { get; private set; } = string.Empty;
    public string FrontendUrl { get; private set; } = string.Empty;
    public int BackendPort { get; private set; }
    public int DefaultVacationLength { get; private set; }

    public void InitializeEnv()
    {
        DbConnection = GetStringEnv("DB_CONNECTION");
        DbConnectionDevelopment = GetStringEnv("DB_CONNECTION_DEVELOPMENT");
        FrontendUrl = GetStringEnv("FRONTEND_URL");
        BackendPort = GetIntEnv("BACKEND_PORT");
        DefaultVacationLength = GetIntEnv("DEFAULT_VACATION_LENGTH");
    }
    
    private string GetStringEnv(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }
        
        return value;
    }

    private int GetIntEnv(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (int.TryParse(value, out var result))
        {
            return result;
        }
        
        return 0;
    }
}
