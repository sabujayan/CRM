using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Technologies
{
    public interface ITechnologyRepository : IRepository<Technology, Guid>
    {
    }
}
