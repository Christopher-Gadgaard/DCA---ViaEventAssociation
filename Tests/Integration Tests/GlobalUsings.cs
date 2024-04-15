using EfcDmPersistence;
using Microsoft.EntityFrameworkCore;

namespace Integration_Tests;

public class GlobalUsings
{
    internal static ViaDbContext SetupContext()
    {
        DbContextOptionsBuilder<ViaDbContext> optionsBuilder = new();
        string testDbName = "Test" + Guid.NewGuid() +".db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDbName);
        ViaDbContext context = new(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
    
    internal static async Task SaveAndClearAsync<T>(T entity, ViaDbContext context) 
        where T : class
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}