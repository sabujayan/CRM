using Indo.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Indo.ClientesPermission
{
    public class ClientsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("Clients");
            myGroup.AddPermission("Clients_Get_By_Id");
            myGroup.AddPermission("Clients_Create_Authorize");
            myGroup.AddPermission("Clients_Delete_Authorize");
            myGroup.AddPermission("Clients_Update_Authorize");
        }
    }
}
