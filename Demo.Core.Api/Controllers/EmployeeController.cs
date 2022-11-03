﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.Core.Domain.Models;
using System.Reflection.Metadata.Ecma335;
using Demo.Core.Domain.Store;
using System.Linq;
using System.Linq.Expressions;
using Demo.Core.Api.Extensions;
using Microsoft.Data.SqlClient;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.Core.Api.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ILogger<EmployeeController> _logger;
        private IEnumerable<Employee> empData;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
             empData = new Employee[]
           {
                             new Employee {
                                EmployeeId = "Slade Wilcox",
                                EmployeeName = "Ulla Diaz",
                                City = "Steven Williamson",
                                Department = "Brian Gibson",
                                Gender = "YHT28GNF2CY"

                },
                 new Employee{
                        EmployeeId= "Ainsley Owens",
                        EmployeeName= "Chava Crawford",
                        City= "Heather Nixon",
                        Department= "Dustin Rosales",
                        Gender= "HDB78TQJ4SQ"
                },
                 new Employee{
                        EmployeeId= "Adria Wheeler",
                        EmployeeName= "Nash Rosales",
                        City= "Price Avery",
                        Department= "Scarlet Burks",
                        Gender= "QFY79KEU1KI"
                },
                 new Employee{
                        EmployeeId= "Rudyard Wilkerson",
                        EmployeeName= "Kai Sosa",
                        City= "Patrick Payne",
                        Department= "Stone Dean",
                        Gender= "QBZ84YMU3RL"
                }
           };
            _logger = logger;

        }
        [HttpGet]
        [Route("employees")]
        public APIResponse Index([FromQuery] Sorting<Employee> sorting, Filtering<Employee> filtering, Pagination pagination)
        {

            Sort[] sortList = Helper.GetSorting(sorting.sort);
            Filter[] filterList = Helper.GetFilter(filtering.filter);

            DemoContext empCtx = new DemoContext();
            var data = empCtx.GetAllEmployees();
            data = EntityFrameworkExtensions.ApplyFilter(data.AsQueryable(), filterList);
            data = EntityFrameworkExtensions.ApplySort(data.AsQueryable(), sortList);
            // Assign File download option
            //data = data.AsEnumerable().Take((pageStart==0?1:pageStart) * (totalRecords==0?10:totalRecords));     
            Parallel.ForEach(data, item =>
            {
                item.DownloadUrl = Helper.GetBaseUrl(Request) + "/download/" + new Random().Next(1,25).ToString();
            });
            const string contentType = "application/json";
            HttpContext.Response.ContentType = contentType;
            APIResponse response = new APIResponse
            {
                Success = "true",
                Message = "Retrieved Successfully",
                Data = data.ToArray<Employee>()
            };


            return response;
        }

        [HttpPost]
        [Route("employees/Create")]
        public async Task<int> Create([FromBody] Employee employee)
        {
            DemoContext empCtx = new DemoContext();
            int response = await empCtx.AddEmployee(employee);
            return response;
        }

        [HttpPut]
        [Route("employees/Update")]
        public async Task<int> Update([FromBody] Employee employee)
        {
            DemoContext empCtx = new DemoContext();
            int response = await empCtx.UpdateEmployee(employee);
            return response;

        }
        [HttpGet]
        [Route("employees/{id}")]
        public async Task<APIResponse> Index(int id)
        {
            DemoContext empCtx = new DemoContext();
            var data = await empCtx.GetEmployeeData(id);

            APIResponse response = new APIResponse
            {
                Success = "true",
                Message = "Retrieved Successfully",
                Data = new Employee[] { data },
            };
            return response;
        }

        [HttpGet]
        [Route("cities")]
        public async Task<APIResponse> Get()
        {
            DemoContext empCtx = new DemoContext();
            var data = await empCtx.GetCities();

            APIResponse response = new APIResponse
            {
                Success = "true",
                Message = "Retrieved Successfully",
                Data = data.ToArray<City>(),
            };
            return response;
        }

        [HttpPost]
        [Route("cities/Create")]
        public async Task<int> Create([FromBody] City city)
        {
            DemoContext empCtx = new DemoContext();
            int response = await empCtx.AddCity(city);
            return response;

        }
        [HttpDelete]
        [Route("employees/delete/{id}")]
        public async Task<int> Delete(int id)
        {
            DemoContext empCtx = new DemoContext();
            var data = await empCtx.DeleteEmployee(id);
            return data;
        }

    }
}
