using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using api.Entities;
using api.FaultTolerance;

namespace api.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<PersonalLoan> PersonalLoans { get; set; }

        public DbSet<Memo> ErrorMemo { get; set; }

        private readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //can use in memory database used for simplicity
            //options.UseInMemoryDatabase("sqliteDb");

            string dbPath = Environment.CurrentDirectory + @"/Db/sqlite.db";

            options.UseSqlite($"Data Source={dbPath}");
        }
    }
}
