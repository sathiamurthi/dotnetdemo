using Demo.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Linq;


namespace Demo.Core.Domain.Store
{
    public partial class BaseContext : DbContext, IApplicationDemoContext
    {

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<City> City { get; set; }


        public BaseContext() :base(GetOptions(@"Data Source=(localdb)\ProjectModels;Initial Catalog=Demo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
        {
        }
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString, o => o.CommandTimeout(300)).Options;
        }
        public virtual async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }

    }
}
