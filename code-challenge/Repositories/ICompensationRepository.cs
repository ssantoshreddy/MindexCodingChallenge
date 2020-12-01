using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        IEnumerable<Compensation> GetById(string id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();

    }
}
