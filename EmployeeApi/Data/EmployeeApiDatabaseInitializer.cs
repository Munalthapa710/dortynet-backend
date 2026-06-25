using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data;

public static class EmployeeApiDatabaseInitializer
{
    public static async Task EnsureSchemaAsync(ApplicationDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();
    }
}
