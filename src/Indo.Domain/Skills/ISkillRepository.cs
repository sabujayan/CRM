﻿using Indo.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Skills
{
    public interface  ISkillRepository : IRepository<Skill, Guid>
    {
    }
}