using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Api.DbContext;
using Api.Models.Entities;

namespace Api;

public class Program
{               
    public static async Task Main(string[] args)
    {
        var host =  CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = services.GetRequiredService<DocumentFlowDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            await context.Database.MigrateAsync();

            await ContextHelper.Seeding(context, userManager, roleManager);
            logger.LogInformation("Migrate successful");

        }
        catch ( Exception ex)
        {
            logger.LogError(ex.Message);
        }
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}