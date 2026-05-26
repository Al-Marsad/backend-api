using DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Support;

internal static class TestDbContextFactory
{
    public static AlMarsadDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AlMarsadDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AlMarsadDbContext(options);
    }
}
