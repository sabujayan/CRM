using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Authorization.Permissions;

namespace Indo.EmployeePermission
{
    public class EmployeePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("Employee");
            myGroup.AddPermission("Employee_Get_By_ID");
            myGroup.AddPermission("Employee_Create_Authorize");
            myGroup.AddPermission("Employee_Delete_Authorize");
            myGroup.AddPermission("Employee_Update_Authorize");
        }
    }
}
