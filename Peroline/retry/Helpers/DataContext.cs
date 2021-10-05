using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using retry.Entities;

namespace retry.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<PersonalLoan> Users { get; set; }

        private readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //can use in memory database used for simplicity
            //options.UseInMemoryDatabase("sqliteDb");

            string dbPath = Environment.CurrentDirectory + @"/sqlite.db";

            options.UseSqlite($"Data Source={dbPath}");
        }
    }
}
