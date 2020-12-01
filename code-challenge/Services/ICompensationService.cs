﻿using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface ICompensationService
    {
        List<Compensation> GetByEmployeeId(string id);
        Compensation Create(Compensation compensation);
    }
}
