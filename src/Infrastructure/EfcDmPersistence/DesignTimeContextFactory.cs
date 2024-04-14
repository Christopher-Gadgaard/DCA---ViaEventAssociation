using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfcDmPersistence;

public class DesignTimeContextFactory: IDesignTimeDbContextFactory<ViaDbContext>
{
    public ViaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ViaDbContext>();
        optionsBuilder.UseSqlite(@"Data Source = VEADatabaseProduction.db");
        return new ViaDbContext(optionsBuilder.Options);
    } 
}