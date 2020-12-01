using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("{id}/report-structure", Name = "getReportStructureById")]
        public IActionResult GetReportStructureById(String id)
        {
            _logger.LogDebug($"Received report structure get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            Stack<Employee> dirs = new Stack<Employee>();
            var noOfReports = new List<string>();

            if (employee.DirectReports != null) {

                foreach (var dir in employee.DirectReports)
                {
                    dirs.Push(dir);
                }

                while (dirs.Count > 0)
                {
                    var currEmployee = dirs.Pop();

                    noOfReports.Add(currEmployee.EmployeeId);

                    var subDirs = currEmployee.DirectReports;

                    if (subDirs != null)
                    {

                        foreach (var subDir in subDirs)
                        {
                            dirs.Push(subDir);
                        }

                    }
                }

            }

            var reportStructure = new ReportingStructure() {
                employee = employee,
                numberOfReports = noOfReports.Count
            };

            return Ok(reportStructure);
        }

    }
}
