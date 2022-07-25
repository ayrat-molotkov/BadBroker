using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BadBroker.Api.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void Configure(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bad broker API V1");
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}
