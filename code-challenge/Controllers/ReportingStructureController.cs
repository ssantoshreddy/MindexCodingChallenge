using System.Collections.Generic;
using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace challenge.Controllers
{
    [Route("api/reporting-structure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public IActionResult GetReportStructureByEmployeeId(string id)
        {
            _logger.LogDebug($"Received report structure get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            // DFS can be used instead of this.

            Stack<Employee> dirs = new Stack<Employee>();
            var noOfReports = new List<string>();

            if (employee.DirectReports != null)
            {

                foreach (var dir in employee.DirectReports)
                {
                    dirs.Push(dir);
                }

                while (dirs.Count > 0)
                {
                    var currEmployee = dirs.Pop();

                    noOfReports.Add(currEmployee.EmployeeId);

                    var sub = _employeeService.GetById(currEmployee.EmployeeId);

                    if (sub != null)
                    {
                        var subDirs = sub.DirectReports;

                        if (subDirs != null)
                        {

                            foreach (var subDir in subDirs)
                            {
                                dirs.Push(subDir);
                            }

                        }
                    }
                }

            }

            var reportStructure = new ReportingStructure()
            {
                employee = employee,
                numberOfReports = noOfReports.Count
            };

            return Ok(reportStructure);

        }
    }

}
