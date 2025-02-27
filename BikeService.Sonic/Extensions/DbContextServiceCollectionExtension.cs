﻿using BikeService.Sonic.BikeDbContext;
using Microsoft.EntityFrameworkCore;

namespace BikeService.Sonic.Extensions;

public static class DbContextServiceCollectionExtension
{
    public static void AddDbContextService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<BikeServiceDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("MysqlServer");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }
    
    public static void RunMigrations(this IServiceCollection serviceCollection)
    {
        using var scope = serviceCollection.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BikeServiceDbContext>();
        db.Database.Migrate();
    }
}
