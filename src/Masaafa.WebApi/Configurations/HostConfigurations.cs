using Masaafa.Infrastructure;
using Masaafa.Persistence;

namespace Masaafa.WebApi.Configurations;

public static partial class HostConfigurations
{
    public static async Task<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder.Services.AddPresentation(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        return await Task.FromResult(builder);
    }

    public static async Task<WebApplication> ConfigureAsync(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseStaticFiles();

        app.MapControllers();

        return await Task.FromResult(app);
    }
}
