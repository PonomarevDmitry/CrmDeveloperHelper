using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OtherPrivilegeListViewItem
    {
        public Privilege Privilege { get; private set; }

        public string Name => Privilege.Name;

        public string EntityLogicalName => Privilege.LinkedEntitiesSorted;

        public AccessRights? PrivilegeAccessRights { get; private set; }

        public string PrivilegeType => PrivilegeAccessRights.ToString();

        public OtherPrivilegeListViewItem(Privilege privilege)
        {
            this.Privilege = privilege;

            if (privilege.AccessRight.HasValue && Enum.IsDefined(typeof(AccessRights), privilege.AccessRight.Value))
            {
                this.PrivilegeAccessRights = (AccessRights)privilege.AccessRight.Value;
            }
        }
    }
}