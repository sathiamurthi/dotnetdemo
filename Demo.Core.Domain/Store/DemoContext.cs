using Demo.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Store
{
    public class DemoContext
    {
        BaseContext db = new BaseContext();

        public IEnumerable<Employee> GetAllEmployees()
        {
            try
            {
                return db.Employee.ToList();
            }
            catch
            {
                throw;
            }
        }

        //To Add new employee record   
        public async Task<int> AddEmployee(Employee employee)
        {
            try
            {
                db.Employee.Add(employee);
                await db.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }

        //To Update the records of a particluar employee  
        public async Task<int> UpdateEmployee(Employee employee)
        {
            try
            {
                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        //Get the details of a particular employee  
        public async Task<Employee> GetEmployeeData(int id)
        {
            try
            {
                Employee employee = db.Employee.Find(id);
                return employee;
            }
            catch
            {
                throw;
            }
        }

        //To Delete the record of a particular employee  
        public async Task<int> DeleteEmployee(int id)
        {
            try
            {
                Employee? emp = db.Employee.Find(id);
                db.Employee.Remove(emp);
                await db.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }

        //To Get the list of Cities  
        public async Task<IEnumerable<City>> GetCities()
        {
            try
            {
                return db.City.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> AddCity(City city)
        {
            try
            {
                db.City.Add(city);
                await db.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }
    }
}
