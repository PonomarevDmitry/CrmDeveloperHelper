using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class RolePrivilegeComparerHelper
    {
        public string Entity1Name { get; private set; }

        public string Entity2Name { get; private set; }

        private readonly string _tabSpacer;

        public RolePrivilegeComparerHelper(string tabSpacer, string entity1Name, string entity2Name)
        {
            this._tabSpacer = tabSpacer;
            this.Entity1Name = entity1Name;
            this.Entity2Name = entity2Name;
        }

        public List<string> CompareRolePrivileges(
            IEnumerable<RolePrivilege> enumerableRolePriv1
            , IEnumerable<RolePrivilege> enumerableRolePriv2
            , IEnumerable<Privilege> commonPrivileges
            , PrivilegeNameComparer privilegeNameComparer
        )
        {
            List<string> result = new List<string>();

            FormatTextTableHandler tableOnlyIn1 = new FormatTextTableHandler();
            tableOnlyIn1.SetHeader("PrivilegeName", "PrivilegeType", "Depth", "Linked Entities");

            FormatTextTableHandler tableOnlyIn2 = new FormatTextTableHandler();
            tableOnlyIn2.SetHeader("PrivilegeName", "PrivilegeType", "Depth", "Linked Entities");

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();
            tableDifferent.SetHeader("PrivilegeName", "PrivilegeType", Entity1Name, Entity2Name, "Linked Entities");

            foreach (var priv in commonPrivileges.OrderBy(s => s.LinkedEntitiesSorted).OrderBy(s => s.Name, privilegeNameComparer))
            {
                RolePrivilege rolePriv1 = null;
                RolePrivilege rolePriv2 = null;

                if (enumerableRolePriv1 != null)
                {
                    rolePriv1 = enumerableRolePriv1.FirstOrDefault(i => i.PrivilegeId == priv.PrivilegeId);
                }

                if (enumerableRolePriv2 != null)
                {
                    rolePriv2 = enumerableRolePriv2.FirstOrDefault(i => i.PrivilegeId == priv.PrivilegeId);
                }

                if (rolePriv1 != null && rolePriv2 == null)
                {
                    var privilegedepthmask = rolePriv1.Depth;

                    tableOnlyIn1.AddLine(priv.Name
                        , priv.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)priv.AccessRight.Value).ToString() : string.Empty
                        , RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask)
                        , priv.LinkedEntitiesSorted
                    );
                    tableOnlyIn2.CalculateLineLengths(priv.Name
                        , priv.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)priv.AccessRight.Value).ToString() : string.Empty
                        , RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask)
                        , priv.LinkedEntitiesSorted
                    );
                }
                else if (rolePriv1 == null && rolePriv2 != null)
                {
                    var privilegedepthmask = rolePriv2.Depth;

                    tableOnlyIn2.AddLine(priv.Name
                        , priv.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)priv.AccessRight.Value).ToString() : string.Empty
                        , RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask)
                        , priv.LinkedEntitiesSorted
                    );
                    tableOnlyIn1.CalculateLineLengths(priv.Name
                        , priv.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)priv.AccessRight.Value).ToString() : string.Empty
                        , RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask)
                        , priv.LinkedEntitiesSorted
                    );
                }
                else if (rolePriv1 != null && rolePriv2 != null)
                {
                    var privilegedepthmask1 = rolePriv1.Depth;
                    var privilegedepthmask2 = rolePriv2.Depth;

                    if (privilegedepthmask1 != privilegedepthmask2)
                    {
                        tableDifferent.AddLine(priv.Name
                            , priv.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)priv.AccessRight.Value).ToString() : string.Empty
                            , RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask1)
                            , RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask2)
                            , priv.LinkedEntitiesSorted
                        );
                    }
                }
            }

            if (tableOnlyIn1.Count > 0)
            {
                if (result.Count > 0) { result.Add(string.Empty); }

                result.Add(string.Format("RolePrivileges ONLY in {0}: {1}", Entity1Name, tableOnlyIn1.Count));
                tableOnlyIn1.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
            }

            if (tableOnlyIn2.Count > 0)
            {
                if (result.Count > 0) { result.Add(string.Empty); }

                result.Add(string.Format("RolePrivileges ONLY in {0}: {1}", Entity2Name, tableOnlyIn2.Count));
                tableOnlyIn2.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
            }

            if (tableDifferent.Count > 0)
            {
                if (result.Count > 0) { result.Add(string.Empty); }

                result.Add(string.Format("Different RolePrivileges in {0} and {1}: {2}", Entity1Name, Entity2Name, tableDifferent.Count));
                tableDifferent.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
            }

            if (tableOnlyIn1.Count == 0
              && tableOnlyIn2.Count == 0
              && tableDifferent.Count == 0
              )
            {
                result.Add(string.Format("No difference RolePrivileges in {0} and {1}", Entity1Name, Entity2Name));
            }

            return result;
        }
    }
}
