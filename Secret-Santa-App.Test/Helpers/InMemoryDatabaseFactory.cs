using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Secret_Santa_App.Data;

namespace Secret_Santa_App.Test.Helpers;

/// <summary>
/// Database factory.
/// </summary>
public static class InMemoryDatabaseFactory
{
    /// <summary>
    /// Creates in memory database.
    /// </summary>
    /// <returns>In memory database context.</returns>
    public static DataContext CreateInMemoryDatabase()
    {
        var databaseName = Guid.NewGuid().ToString();

        var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName)
            .UseInternalServiceProvider(serviceProvider)
            .Options;
        var databaseContextMock = new DataContext(options);

        return databaseContextMock;
    }
}