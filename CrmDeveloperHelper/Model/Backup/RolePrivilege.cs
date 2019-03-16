using Microsoft.Crm.Sdk.Messages;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Backup
{
    [DataContract]
    public class RolePrivilege
    {
        [DataMember(Order = 10)]
        public string Name { get; set; }

        [DataMember(Order = 20)]
        public PrivilegeDepth? Level { get; set; }
    }
}