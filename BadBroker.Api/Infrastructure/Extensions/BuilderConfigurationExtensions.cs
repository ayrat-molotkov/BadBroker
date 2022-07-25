using BadBroker.Api.Infrastructure.Environment;
using BadBroker.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BadBroker.Api.Infrastructure.Extensions;

public static class BuilderConfigurationExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var serializationSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None
        };

        JsonConvert.DefaultSettings = () => serializationSettings;

        builder.Services.AddTransient<IRatesService, RatesService>();
        builder.Services.AddTransient<ExchangeRatesClient>();
        builder.Services.AddTransient<RatesStorage>();

        builder.Services.AddHttpClient(Constants.HttpClientName, client =>
        {            
            client.BaseAddress = new Uri(EnvironmentVariableHelper.Get("ExchangeRates:Endpoint", builder.Configuration));
            client.DefaultRequestHeaders.Add("apikey", EnvironmentVariableHelper.Get("ExchangeRates:ApiKey", builder.Configuration));
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();

        builder.Services.AddEasyCaching(options =>
        {
            options.UseInMemory(Constants.CacheName);
        });
    }


    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile(path: "appsettings.json",
            optional: false,
            reloadOnChange: true);        

        builder.Configuration.AddEnvironmentVariables();

        return builder;
    }
}
