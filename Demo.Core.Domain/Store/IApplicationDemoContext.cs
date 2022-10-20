using Demo.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Store
{
    public interface IApplicationDemoContext
    {
        DbSet<Employee> Employee { get; set; }
        DbSet<City> City { get; set; }
        Task<int> SaveChanges();
    }
}
