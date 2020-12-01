using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.employee.FirstName} {compensation.employee.LastName}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute(new { id = compensation.employee.EmployeeId }, compensation);
        }

        [HttpGet("{id}")]
        public IActionResult GetCompensationByEmployeeId(string id)
        {
            _logger.LogDebug($"Received compensation get request for employee id '{id}'");

            var compensations = _compensationService.GetByEmployeeId(id);

            if (compensations == null || compensations.Count == 0)
            {
                return NotFound();
            }

            return Ok(compensations);
        }

    }
}
