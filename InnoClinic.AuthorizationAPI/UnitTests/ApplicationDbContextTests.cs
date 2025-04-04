using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public void OnModelCreatingAppliesRoleConfiguration()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            
            using var context = new ApplicationDbContext(options);

            var entityType = context.Model.FindEntityType(typeof(Role));

            Assert.NotNull(entityType);

            Assert.Equal("AspNetRoles", entityType.GetTableName());
            Assert.NotNull(entityType.FindPrimaryKey());
        }
    }
}