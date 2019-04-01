using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class OrganizationComparer
    {
        public Task<string> CheckSecurityRolesAsync()
        {
            return Task.Run(async () => await CheckSecurityRoles());
        }

        private async Task<string> CheckSecurityRoles()
        {
            StringBuilder content = new StringBuilder();

            var privilegeComparer = new PrivilegeNameComparer();
            var privileteEquality = new PrivilegeEqualityComparer();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingSecurityRolesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var task1 = _comparerSource.GetRole1Async();
            var task2 = _comparerSource.GetRole2Async();

            var list1 = await task1;

            var taskPriv1 = new PrivilegeRepository(_comparerSource.Service1).GetListAsync(null);

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Security Roles in {0}: {1}", Connection1.Name, list1.Count()));



            var list2 = await task2;

            var taskPriv2 = new PrivilegeRepository(_comparerSource.Service2).GetListAsync(null);

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Security Roles in {0}: {1}", Connection2.Name, list2.Count()));



            var dictPrivilege1 = (await taskPriv1).ToDictionary(p => p.Name, StringComparer.InvariantCultureIgnoreCase);

            var taskPrivRole1 = new RolePrivilegesRepository(_comparerSource.Service1).GetListAsync(list1.Select(e => e.RoleId.Value));

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Security Privileges in {0}: {1}", Connection1.Name, dictPrivilege1.Count()));




            var dictPrivilege2 = (await taskPriv2).ToDictionary(p => p.Name, StringComparer.InvariantCultureIgnoreCase);

            var taskPrivRole2 = new RolePrivilegesRepository(_comparerSource.Service2).GetListAsync(list2.Select(e => e.RoleId.Value));

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Security Privileges in {0}: {1}", Connection2.Name, dictPrivilege2.Count()));



            var commonPrivileges = dictPrivilege1.Values.Intersect(dictPrivilege2.Values, privileteEquality).ToList();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Common Security Privileges in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonPrivileges.Count()));







            var listRolePrivilege1 = await taskPrivRole1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Security Roles Privileges in {0}: {1}", Connection1.Name, listRolePrivilege1.Count()));

            var listRolePrivilege2 = await taskPrivRole2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Security Roles Privileges in {0}: {1}", Connection2.Name, listRolePrivilege2.Count()));

            if (!list1.Any() && !list2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            var groupByRole1 = listRolePrivilege1.GroupBy(e => e.RoleId.Value).ToDictionary(g => g.Key, g => g.AsEnumerable());
            var groupByRole2 = listRolePrivilege2.GroupBy(e => e.RoleId.Value).ToDictionary(g => g.Key, g => g.AsEnumerable());

            FormatTextTableHandler rolesOnlyExistsIn1 = new FormatTextTableHandler();
            rolesOnlyExistsIn1.SetHeader("Name", "BusinessUnit", "IsManaged");

            FormatTextTableHandler rolesOnlyExistsIn2 = new FormatTextTableHandler();
            rolesOnlyExistsIn2.SetHeader("Name", "BusinessUnit", "IsManaged");

            FormatTextTableHandler privilegesOnlyExistsIn1 = new FormatTextTableHandler();
            privilegesOnlyExistsIn1.SetHeader("PrivilegeName", "PrivilegeType", "Linked Entities");

            FormatTextTableHandler privilegesOnlyExistsIn2 = new FormatTextTableHandler();
            privilegesOnlyExistsIn2.SetHeader("PrivilegeName", "PrivilegeType", "Linked Entities");

            foreach (var item1 in dictPrivilege1.Values
                .Except(dictPrivilege2.Values, privileteEquality)
                .ToList()
                .OrderBy(p => p.LinkedEntitiesSorted)
                .ThenBy(p => p.Name, privilegeComparer)
            )
            {
                privilegesOnlyExistsIn1.AddLine(item1.Name
                    , item1.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)item1.AccessRight.Value).ToString() : string.Empty
                    , item1.LinkedEntitiesSorted
                );
            }

            foreach (var item2 in dictPrivilege2.Values
                .Except(dictPrivilege1.Values, privileteEquality)
                .ToList()
                .OrderBy(p => p.LinkedEntitiesSorted)
                .ThenBy(p => p.Name, privilegeComparer)
            )
            {
                privilegesOnlyExistsIn2.AddLine(item2.Name
                    , item2.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)item2.AccessRight.Value).ToString() : string.Empty
                    , item2.LinkedEntitiesSorted
                );
            }

            var commonList = new List<LinkedEntities<Role>>();

            foreach (var role1 in list1)
            {
                var name1 = role1.Name;
                var businessUnit1 = role1.BusinessUnitId.Name;

                if (role1.BusinessUnitParentBusinessUnit == null)
                {
                    businessUnit1 = "Root Organization";
                }

                {
                    Role role2 = null;

                    if (role2 == null)
                    {
                        role2 = list2.FirstOrDefault(role => role.Id == role1.Id);
                    }

                    if (role2 == null && role1.RoleTemplateId != null)
                    {
                        role2 = list2.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role1.RoleTemplateId.Id);
                    }

                    if (role2 != null)
                    {
                        commonList.Add(new LinkedEntities<Role>(role1, role2));
                        continue;
                    }
                }

                string state = role1.FormattedValues[Role.Schema.Attributes.ismanaged];

                rolesOnlyExistsIn1.AddLine(name1, businessUnit1, state);

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Role, role1.Id);
            }

            foreach (var role2 in list2)
            {
                var name2 = role2.Name;
                var businessUnit2 = role2.BusinessUnitId.Name;

                if (role2.BusinessUnitParentBusinessUnit == null)
                {
                    businessUnit2 = "Root Organization";
                }

                {
                    Role role1 = null;

                    if (role1 == null)
                    {
                        role1 = list1.FirstOrDefault(role => role.Id == role2.Id);
                    }

                    if (role1 == null && role2.RoleTemplateId != null)
                    {
                        role1 = list1.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role2.RoleTemplateId.Id);
                    }

                    if (role1 != null)
                    {
                        continue;
                    }
                }

                string state = role2.FormattedValues[Role.Schema.Attributes.ismanaged];

                rolesOnlyExistsIn2.AddLine(name2, businessUnit2, state);

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Role, role2.Id);
            }

            var dictDifference = new Dictionary<LinkedEntities<Role>, List<string>>();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.RolesCommonFormat3, Connection1.Name, Connection2.Name, commonList.Count()));

            foreach (var commonRole in commonList)
            {
                groupByRole1.TryGetValue(commonRole.Entity1.Id, out IEnumerable<RolePrivileges> enumerable1);
                groupByRole2.TryGetValue(commonRole.Entity2.Id, out IEnumerable<RolePrivileges> enumerable2);

                List<string> diff = ComparePrivileges(enumerable1, enumerable2, commonPrivileges, dictPrivilege1, dictPrivilege2, privilegeComparer);

                if (diff.Count > 0)
                {
                    dictDifference.Add(commonRole, diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.Role, commonRole.Entity1.Id, commonRole.Entity2.Id, string.Join(Environment.NewLine, diff));
                }
            }

            if (privilegesOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Security Privileges ONLY EXISTS in {0}: {1}", Connection1.Name, privilegesOnlyExistsIn1.Count);

                privilegesOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
            }

            if (privilegesOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Security Privileges ONLY EXISTS in {0}: {1}", Connection2.Name, privilegesOnlyExistsIn2.Count);

                privilegesOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
            }

            if (rolesOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Security Roles ONLY EXISTS in {0}: {1}", Connection1.Name, rolesOnlyExistsIn1.Count);

                rolesOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
            }

            if (rolesOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Security Roles ONLY EXISTS in {0}: {1}", Connection2.Name, rolesOnlyExistsIn2.Count);

                rolesOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
            }

            if (dictDifference.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                var order = dictDifference.OrderBy(s => s.Key.Entity1.Name).ThenBy(s => s.Key.Entity1.BusinessUnitParentBusinessUnit == null ? "Root Organization" : s.Key.Entity1.BusinessUnitId.Name);

                content.AppendLine().AppendLine().AppendFormat("Security Roles DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                {
                    var table = new FormatTextTableHandler();
                    table.SetHeader("Name", "BusinessUnit");

                    foreach (var item in order)
                    {
                        table.AddLine(item.Key.Entity1.Name, item.Key.Entity1.BusinessUnitParentBusinessUnit == null ? "Root Organization" : item.Key.Entity1.BusinessUnitId.Name);
                    }

                    table.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
                }

                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Security Roles DIFFERENT Details in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                foreach (var item in order)
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + string.Format("Role: {0}         Business Unit: {1}", item.Key.Entity1.Name, item.Key.Entity1.BusinessUnitParentBusinessUnit == null ? "Root Organization" : item.Key.Entity1.BusinessUnitId.Name)).TrimEnd());

                    foreach (var str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (rolesOnlyExistsIn2.Count == 0
                && rolesOnlyExistsIn1.Count == 0
                && dictDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Security Roles.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Security Roles");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private List<string> ComparePrivileges(
            IEnumerable<RolePrivileges> enumerableRolePriv1
            , IEnumerable<RolePrivileges> enumerableRolePriv2
            , IEnumerable<Privilege> commonPrivileges
            , Dictionary<string, Privilege> listPrivilege1
            , Dictionary<string, Privilege> listPrivilege2
            , PrivilegeNameComparer privilegeComparer
        )
        {
            List<string> result = new List<string>();

            FormatTextTableHandler tableOnlyIn1 = new FormatTextTableHandler();
            tableOnlyIn1.SetHeader("PrivilegeName", "PrivilegeType", "Depth", "Linked Entities");

            FormatTextTableHandler tableOnlyIn2 = new FormatTextTableHandler();
            tableOnlyIn2.SetHeader("PrivilegeName", "PrivilegeType", "Depth", "Linked Entities");

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();
            tableDifferent.SetHeader("PrivilegeName", "PrivilegeType", Connection1.Name, Connection2.Name, "Linked Entities");

            foreach (var priv in commonPrivileges.OrderBy(s => s.LinkedEntitiesSorted).OrderBy(s => s.Name, privilegeComparer))
            {
                var priv1 = listPrivilege1[priv.Name];
                var priv2 = listPrivilege2[priv.Name];

                RolePrivileges rolePriv1 = null;
                RolePrivileges rolePriv2 = null;

                if (enumerableRolePriv1 != null)
                {
                    rolePriv1 = enumerableRolePriv1.FirstOrDefault(i => i.PrivilegeId == priv1?.PrivilegeId);
                }

                if (enumerableRolePriv2 != null)
                {
                    rolePriv2 = enumerableRolePriv2.FirstOrDefault(i => i.PrivilegeId == priv2?.PrivilegeId);
                }

                if (rolePriv1 != null && rolePriv2 == null)
                {
                    var privilegedepthmask = rolePriv1.PrivilegeDepthMask.GetValueOrDefault();

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
                    var privilegedepthmask = rolePriv2.PrivilegeDepthMask.GetValueOrDefault();

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
                    var privilegedepthmask1 = rolePriv1.PrivilegeDepthMask.GetValueOrDefault();
                    var privilegedepthmask2 = rolePriv2.PrivilegeDepthMask.GetValueOrDefault();

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

                result.Add(string.Format("Privileges ONLY in {0}: {1}", Connection1.Name, tableOnlyIn1.Count));
                tableOnlyIn1.GetFormatedLines(false).ForEach(s => result.Add(tabSpacer + s));
            }

            if (tableOnlyIn2.Count > 0)
            {
                if (result.Count > 0) { result.Add(string.Empty); }

                result.Add(string.Format("Privileges ONLY in {0}: {1}", Connection2.Name, tableOnlyIn2.Count));
                tableOnlyIn2.GetFormatedLines(false).ForEach(s => result.Add(tabSpacer + s));
            }

            if (tableDifferent.Count > 0)
            {
                if (result.Count > 0) { result.Add(string.Empty); }

                result.Add(string.Format("Different privileges in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableDifferent.Count));
                tableDifferent.GetFormatedLines(false).ForEach(s => result.Add(tabSpacer + s));
            }

            return result;
        }



        public Task<string> CheckFieldSecurityProfilesAsync()
        {
            return Task.Run(async () => await CheckFieldSecurityProfiles());
        }

        private async Task<string> CheckFieldSecurityProfiles()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingFieldSecurityProfilesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var task1 = _comparerSource.GetFieldSecurityProfile1Async();
            var task2 = _comparerSource.GetFieldSecurityProfile2Async();

            var list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Field Security Profiles in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Field Security Profiles in {0}: {1}", Connection2.Name, list2.Count()));

            if (!list1.Any() && !list2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            var taskPerm1 = _comparerSource.GetFieldPermission1Async();
            var taskPerm2 = _comparerSource.GetFieldPermission2Async();

            var listPermission1 = await taskPerm1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Field Security Profiles Permissions in {0}: {1}", Connection1.Name, listPermission1.Count()));

            var listPermission2 = await taskPerm2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Field Security Profiles Permissions in {0}: {1}", Connection2.Name, listPermission2.Count()));

            var group1 = listPermission1.GroupBy(e => e.FieldSecurityProfileId.Id);
            var group2 = listPermission2.GroupBy(e => e.FieldSecurityProfileId.Id);

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Name", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Name", "Id");

            var dictDifference = new Dictionary<Tuple<string, Guid>, List<string>>();

            foreach (var profile1 in list1)
            {
                {
                    var profile2 = list2.FirstOrDefault(profile => profile.Id == profile1.Id);

                    if (profile2 != null)
                    {
                        continue;
                    }
                }

                var name1 = profile1.Name;

                tableOnlyExistsIn1.AddLine(name1, profile1.Id.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.FieldSecurityProfile, profile1.Id);
            }

            foreach (var profile2 in list2)
            {
                var name2 = profile2.Name;

                {
                    var profile1 = list1.FirstOrDefault(profile => profile.Id == profile2.Id);

                    if (profile1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(name2, profile2.Id.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.FieldSecurityProfile, profile2.Id);
            }

            foreach (var profile1 in list1)
            {
                var profile2 = list2.FirstOrDefault(profile => profile.Id == profile1.Id);

                if (profile2 == null)
                {
                    continue;
                }

                var name1 = profile1.Name;

                List<string> diff = new List<string>();

                IEnumerable<FieldPermission> enumerable1 = group1.FirstOrDefault(g => g.Key == profile1.Id);
                IEnumerable<FieldPermission> enumerable2 = group2.FirstOrDefault(g => g.Key == profile2.Id);

                CompareFieldPermissions(diff, enumerable1, enumerable2, tabSpacer);

                if (diff.Count > 0)
                {
                    dictDifference.Add(Tuple.Create(name1, profile1.Id), diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.FieldSecurityProfile, profile1.Id, profile2.Id, string.Join(Environment.NewLine, diff));
                }
            }

            if (tableOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Field Security Profiles ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

                tableOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
            }

            if (tableOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Field Security Profiles ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
            }

            if (dictDifference.Count > 0)
            {
                var order = dictDifference.OrderBy(s => s.Key.Item1).ThenBy(s => s.Key.Item2);

                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Field Security Profiles DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                {
                    var table = new FormatTextTableHandler();
                    table.SetHeader("Name", "Id");

                    foreach (var item in order)
                    {
                        table.AddLine(item.Key.Item1, item.Key.Item2.ToString());
                    }

                    table.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Field Security Profiles DIFFERENT Details in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                foreach (var item in dictDifference.OrderBy(s => s.Key.Item1).ThenBy(s => s.Key.Item2))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key.Item1 + tabSpacer + item.Key.Item2).TrimEnd());

                    foreach (var str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (tableOnlyExistsIn2.Count == 0
                && tableOnlyExistsIn1.Count == 0
                && dictDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Field Security Profiles.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Field Security Profiles");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private void CompareFieldPermissions(List<string> diff, IEnumerable<FieldPermission> enumerable1, IEnumerable<FieldPermission> enumerable2, string tabSpacer)
        {
            FormatTextTableHandler tableOnlyIn1 = new FormatTextTableHandler();
            FormatTextTableHandler tableOnlyIn2 = new FormatTextTableHandler();

            tableOnlyIn1.SetHeader("Entity name", "Attribute Name", "Can Create", "Can Read", "Can Update");
            tableOnlyIn2.SetHeader("Entity name", "Attribute Name", "Can Create", "Can Read", "Can Update");

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();
            tableDifferent.SetHeader("FieldPermission"
                , Connection1.Name + " Can Create"
                , Connection2.Name + " Can Create"
                , Connection1.Name + " Can Read"
                , Connection2.Name + " Can Read"
                , Connection1.Name + " Can Update"
                , Connection2.Name + " Can Update"
                );

            if (enumerable1 != null)
            {
                foreach (var item1 in enumerable1)
                {
                    var entityName1 = item1.EntityName;
                    var attributeName1 = item1.AttributeLogicalName;

                    if (enumerable2 != null)
                    {
                        var item2 = enumerable2.FirstOrDefault(i =>
                            string.Equals(i.EntityName, entityName1, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(i.AttributeLogicalName, attributeName1, StringComparison.InvariantCultureIgnoreCase)
                            );

                        if (item2 != null)
                        {
                            continue;
                        }
                    }

                    var cancreate1 = item1.FormattedValues.Contains(FieldPermission.Schema.Attributes.cancreate) ? item1.FormattedValues[FieldPermission.Schema.Attributes.cancreate] : string.Empty;
                    var canread1 = item1.FormattedValues.Contains(FieldPermission.Schema.Attributes.canread) ? item1.FormattedValues[FieldPermission.Schema.Attributes.canread] : string.Empty;
                    var canupdate1 = item1.FormattedValues.Contains(FieldPermission.Schema.Attributes.canupdate) ? item1.FormattedValues[FieldPermission.Schema.Attributes.canupdate] : string.Empty;

                    tableOnlyIn1.AddLine(entityName1, attributeName1, cancreate1, canread1, canupdate1);

                    tableOnlyIn2.CalculateLineLengths(entityName1, attributeName1, cancreate1, canread1, canupdate1);

                    this.ImageBuilder.AddComponentSolution1((int)ComponentType.FieldPermission, item1.Id);
                }
            }

            if (enumerable2 != null)
            {
                foreach (var item2 in enumerable2)
                {
                    var entityName2 = item2.EntityName;
                    var attributeName2 = item2.AttributeLogicalName;

                    if (enumerable1 != null)
                    {
                        var item1 = enumerable1.FirstOrDefault(i =>
                            string.Equals(i.EntityName, entityName2, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(i.AttributeLogicalName, attributeName2, StringComparison.InvariantCultureIgnoreCase)
                            );

                        if (item1 != null)
                        {
                            continue;
                        }
                    }

                    var cancreate2 = item2.FormattedValues.Contains(FieldPermission.Schema.Attributes.cancreate) ? item2.FormattedValues[FieldPermission.Schema.Attributes.cancreate] : string.Empty;
                    var canread2 = item2.FormattedValues.Contains(FieldPermission.Schema.Attributes.canread) ? item2.FormattedValues[FieldPermission.Schema.Attributes.canread] : string.Empty;
                    var canupdate2 = item2.FormattedValues.Contains(FieldPermission.Schema.Attributes.canupdate) ? item2.FormattedValues[FieldPermission.Schema.Attributes.canupdate] : string.Empty;

                    tableOnlyIn2.AddLine(entityName2, attributeName2, cancreate2, canread2, canupdate2);

                    tableOnlyIn1.CalculateLineLengths(entityName2, attributeName2, cancreate2, canread2, canupdate2);

                    this.ImageBuilder.AddComponentSolution2((int)ComponentType.FieldPermission, item2.Id);
                }
            }

            if (enumerable1 != null && enumerable2 != null)
            {
                foreach (var item1 in enumerable1)
                {
                    var entityName1 = item1.EntityName;
                    var attributeName1 = item1.AttributeLogicalName;

                    var item2 = enumerable2.FirstOrDefault(i =>
                        string.Equals(i.EntityName, entityName1, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(i.AttributeLogicalName, attributeName1, StringComparison.InvariantCultureIgnoreCase)
                        );

                    if (item2 != null)
                    {
                        continue;
                    }

                    var cancreate1 = item1.FormattedValues.Contains(FieldPermission.Schema.Attributes.cancreate) ? item1.FormattedValues[FieldPermission.Schema.Attributes.cancreate] : string.Empty;
                    var canread1 = item1.FormattedValues.Contains(FieldPermission.Schema.Attributes.canread) ? item1.FormattedValues[FieldPermission.Schema.Attributes.canread] : string.Empty;
                    var canupdate1 = item1.FormattedValues.Contains(FieldPermission.Schema.Attributes.canupdate) ? item1.FormattedValues[FieldPermission.Schema.Attributes.canupdate] : string.Empty;

                    var cancreate2 = item2.FormattedValues.Contains(FieldPermission.Schema.Attributes.cancreate) ? item2.FormattedValues[FieldPermission.Schema.Attributes.cancreate] : string.Empty;
                    var canread2 = item2.FormattedValues.Contains(FieldPermission.Schema.Attributes.canread) ? item2.FormattedValues[FieldPermission.Schema.Attributes.canread] : string.Empty;
                    var canupdate2 = item2.FormattedValues.Contains(FieldPermission.Schema.Attributes.canupdate) ? item2.FormattedValues[FieldPermission.Schema.Attributes.canupdate] : string.Empty;

                    if (cancreate1 != cancreate2 || canread1 != canread2 || canupdate1 != canupdate2)
                    {
                        this.ImageBuilder.AddComponentDifferent((int)ComponentType.FieldPermission, item1.Id, item2.Id);

                        tableDifferent.AddLine(entityName1, attributeName1
                            , cancreate1, cancreate2
                            , canread1, canread2
                            , canupdate1, canupdate2
                        );
                    }
                }
            }

            if (tableOnlyIn1.Count > 0)
            {
                diff.Add(string.Format("Privileges ONLY in {0}: {1}", Connection1.Name, tableOnlyIn1.Count));
                tableOnlyIn1.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }

            if (tableOnlyIn2.Count > 0)
            {
                diff.Add(string.Format("Privileges ONLY in {0}: {1}", Connection2.Name, tableOnlyIn2.Count));
                tableOnlyIn2.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }

            if (tableDifferent.Count > 0)
            {
                diff.Add(string.Format("Different privileges {0} and {1}", Connection1.Name, Connection2.Name));
                tableDifferent.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }
        }
    }
}