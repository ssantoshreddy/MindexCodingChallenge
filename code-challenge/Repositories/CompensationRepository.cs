using challenge.Data;
using challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();

            Employee employee = _employeeContext.Employees.Include(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == compensation.employee.EmployeeId);

            _employeeContext.Compensation.Update(compensation);

            return compensation;
        }

        public IEnumerable<Compensation> GetById(string id)
        {
            return _employeeContext.Compensation.Include(e => e.employee)
                .Where(e => e.employee.EmployeeId == id).ToList();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

    }
}
