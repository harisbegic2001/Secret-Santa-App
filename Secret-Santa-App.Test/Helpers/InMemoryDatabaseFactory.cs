using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Secret_Santa_App.Data;

namespace Secret_Santa_App.Test.Helpers;

public static class InMemoryDatabaseFactory
{
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