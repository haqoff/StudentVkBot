using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StudentBotCore.Model
{
    internal class StuDbContextDesignFactory : IDesignTimeDbContextFactory<StuDbContext>
    {
        public StuDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            var dbConnectionString = config["dbConnection"];

            return new StuDbContext(dbConnectionString);
        }
    }
}