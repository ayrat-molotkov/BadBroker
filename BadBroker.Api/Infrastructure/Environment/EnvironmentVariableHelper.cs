using Microsoft.Extensions.Configuration;

namespace BadBroker.Api.Infrastructure.Environment;

public static class EnvironmentVariableHelper
{
    public static string Get(string key, IConfiguration configuration)
    {        
        var environmentVariable = configuration[key] ?? throw new Exception($"Couldn't obtain the value for '{key}' configuration key.");
        return System.Environment
            .GetEnvironmentVariable(environmentVariable) ?? throw new Exception($"Couldn't obtain the value for '{environmentVariable}' in configuration"); ;
    }
}
