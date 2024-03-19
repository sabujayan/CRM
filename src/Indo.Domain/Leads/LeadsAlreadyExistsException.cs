using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Leads
{
    public class LeadsAlreadyExistsException : BusinessException
    {
        public LeadsAlreadyExistsException(string newFirstName, string newLastName)
            : base($"A lead with the name '{newFirstName} {newLastName}' already exists.")
        {
            WithData("newFirstName", newFirstName);
            WithData("newLastName", newLastName);
        }
    }
}
