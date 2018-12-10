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

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingSecurityRolesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetRole1Async();
            var task2 = _comparerSource.GetRole2Async();

            var list1 = await task1;

            var taskPriv1 = new PrivilegeRepository(_comparerSource.Service1).GetListAsync();

            content.AppendLine(_iWriteToOutput.WriteToOutput("Security Roles in {0}: {1}", Connection1.Name, list1.Count()));



            var list2 = await task2;

            var taskPriv2 = new PrivilegeRepository(_comparerSource.Service2).GetListAsync();

            content.AppendLine(_iWriteToOutput.WriteToOutput("Security Roles in {0}: {1}", Connection2.Name, list2.Count()));



            var listPrivilege1 = (await taskPriv1).Select(e => e.Name);

            var taskPrivRole1 = new RolePrivilegesRepository(_comparerSource.Service1).GetListAsync(list1.Select(e => e.RoleId.Value));

            content.AppendLine(_iWriteToOutput.WriteToOutput("Security Privileges in {0}: {1}", Connection1.Name, listPrivilege1.Count()));




            var listPrivilege2 = (await taskPriv2).Select(e => e.Name);

            var taskPrivRole2 = new RolePrivilegesRepository(_comparerSource.Service2).GetListAsync(list2.Select(e => e.RoleId.Value));

            content.AppendLine(_iWriteToOutput.WriteToOutput("Security Privileges in {0}: {1}", Connection2.Name, listPrivilege2.Count()));

            

            var commonPrivileges = new HashSet<string>(listPrivilege1.Intersect(listPrivilege2), StringComparer.OrdinalIgnoreCase);

            content.AppendLine(_iWriteToOutput.WriteToOutput("Common Security Privileges in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonPrivileges.Count()));

            var privilegesOnlyIn1 = listPrivilege1.Except(listPrivilege2).ToList();
            var privilegesOnlyIn2 = listPrivilege2.Except(listPrivilege1).ToList();



            var listRolePrivilege1 = await taskPrivRole1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Security Roles Privileges in {0}: {1}", Connection1.Name, listRolePrivilege1.Count()));

            var listRolePrivilege2 = await taskPrivRole2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Security Roles Privileges in {0}: {1}", Connection2.Name, listRolePrivilege2.Count()));




            var group1 = listRolePrivilege1.GroupBy(e => (Guid)e.GetAttributeValue<AliasedValue>("roleprivileges.roleid").Value);
            var group2 = listRolePrivilege2.GroupBy(e => (Guid)e.GetAttributeValue<AliasedValue>("roleprivileges.roleid").Value);

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Name", "BusinessUnit", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Name", "BusinessUnit", "IsManaged");

            var dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

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

                    //if (role2 == null)
                    //{
                    //    role2 = list2.FirstOrDefault(role =>
                    //    {
                    //        var name2 = role.Name;
                    //        var businessUnit2 = role.BusinessUnitId.Name;

                    //        if (role.BusinessUnitParentBusinessUnit == null)
                    //        {
                    //            businessUnit2 = "Root Organization";
                    //        }

                    //        return name1 == name2 && businessUnit1 == businessUnit2;
                    //    });
                    //}

                    if (role2 != null)
                    {
                        continue;
                    }
                }

                string state = role1.FormattedValues[Role.Schema.Attributes.ismanaged];

                tableOnlyExistsIn1.AddLine(name1, businessUnit1, state);

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
                        role1 = list2.FirstOrDefault(role => role.Id == role2.Id);
                    }

                    if (role1 == null && role2.RoleTemplateId != null)
                    {
                        role1 = list2.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role2.RoleTemplateId.Id);
                    }

                    //if (role1 == null)
                    //{
                    //    role1 = list2.FirstOrDefault(role =>
                    //    {
                    //        var name1 = role.Name;
                    //        var businessUnit1 = role.BusinessUnitId.Name;

                    //        if (role.BusinessUnitParentBusinessUnit == null)
                    //        {
                    //            businessUnit1 = "Root Organization";
                    //        }

                    //        return name1 == name2 && businessUnit1 == businessUnit2;
                    //    });
                    //}

                    if (role1 != null)
                    {
                        continue;
                    }
                }

                string state = role2.FormattedValues[Role.Schema.Attributes.ismanaged];

                tableOnlyExistsIn2.AddLine(name2, businessUnit2, state);

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Role, role2.Id);
            }

            foreach (var role1 in list1)
            {
                var name1 = role1.Name;
                var businessUnit1 = role1.BusinessUnitId.Name;

                if (role1.BusinessUnitParentBusinessUnit == null)
                {
                    businessUnit1 = "Root Organization";
                }

                Role role2 = null;

                if (role2 == null)
                {
                    role2 = list2.FirstOrDefault(role => role.Id == role1.Id);
                }

                if (role2 == null && role1.RoleTemplateId != null)
                {
                    role2 = list2.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role1.RoleTemplateId.Id);
                }

                //if (role2 == null)
                //{
                //    role2 = list2.FirstOrDefault(role =>
                //    {
                //        var name2 = role.Name;
                //        var businessUnit2 = role.BusinessUnitId.Name;

                //        if (role.BusinessUnitParentBusinessUnit == null)
                //        {
                //            businessUnit2 = "Root Organization";
                //        }

                //        return name1 == name2 && businessUnit1 == businessUnit2;
                //    });
                //}

                if (role2 == null)
                {
                    continue;
                }

                List<string> diff = new List<string>();

                IEnumerable<Privilege> enumerable1 = group1.FirstOrDefault(g => g.Key == role1.Id);
                IEnumerable<Privilege> enumerable2 = group2.FirstOrDefault(g => g.Key == role2.Id);

                ComparePrivileges(diff, enumerable1, enumerable2, commonPrivileges);

                if (diff.Count > 0)
                {
                    dictDifference.Add(Tuple.Create(name1, businessUnit1), diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.Role, role1.Id, role2.Id, string.Join(Environment.NewLine, diff));
                }
            }

            if (privilegesOnlyIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Security Privileges ONLY EXISTS in {0}: {1}", Connection1.Name, privilegesOnlyIn1.Count);

                foreach (var e in privilegesOnlyIn1.OrderBy(CategorizationPrivilege).ThenBy(FormatPrivilege).ThenBy(s => s))
                {
                    content.AppendLine().Append(tabSpacer + e.TrimEnd());
                }
            }

            if (privilegesOnlyIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Security Privileges ONLY EXISTS in {0}: {1}", Connection2.Name, privilegesOnlyIn2.Count);

                foreach (var e in privilegesOnlyIn2.OrderBy(CategorizationPrivilege).ThenBy(FormatPrivilege).ThenBy(s => s))
                {
                    content.AppendLine().Append(tabSpacer + e.TrimEnd());
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

                content.AppendLine().AppendLine().AppendFormat("Security Roles ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Security Roles ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append(tabSpacer + e.TrimEnd()));
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

                var order = dictDifference.OrderBy(s => s.Key.Item1).ThenBy(s => s.Key.Item2);

                content.AppendLine().AppendLine().AppendFormat("Security Roles DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                {
                    var table = new FormatTextTableHandler();
                    table.SetHeader("Name", "BusinessUnit");

                    foreach (var item in order)
                    {
                        table.AddLine(item.Key.Item1, item.Key.Item2);
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
                        .Append((tabSpacer + string.Format("Role: {0}         Business Unit: {1}", item.Key.Item1, item.Key.Item2)).TrimEnd());

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
                content.AppendLine("No difference in Security Roles.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Security Roles");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private void ComparePrivileges(List<string> diff, IEnumerable<Privilege> enumerable1, IEnumerable<Privilege> enumerable2, HashSet<string> commonPrivileges)
        {
            FormatTextTableHandler tableOnlyIn1 = new FormatTextTableHandler();
            FormatTextTableHandler tableOnlyIn2 = new FormatTextTableHandler();

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();
            tableDifferent.SetHeader("Privilege", Connection1.Name, Connection2.Name);

            if (enumerable1 != null)
            {
                foreach (var item1 in enumerable1.OrderBy(p => CategorizationPrivilege(p.Name)).ThenBy(p => FormatPrivilege(p.Name)).ThenBy(p => p.Name))
                {
                    var name1 = item1.Name;

                    if (!commonPrivileges.Contains(name1))
                    {
                        continue;
                    }

                    if (enumerable2 != null)
                    {
                        var item2 = enumerable2.FirstOrDefault(i => i.Name == name1);

                        if (item2 != null)
                        {
                            continue;
                        }
                    }

                    var privilegedepthmask1 = (int)item1.GetAttributeValue<AliasedValue>("roleprivileges.privilegedepthmask").Value;

                    tableOnlyIn1.AddLine(name1, RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask1));

                    tableOnlyIn2.CalculateLineLengths(name1, RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask1));
                }
            }

            if (enumerable2 != null)
            {
                foreach (var item2 in enumerable2.OrderBy(p => CategorizationPrivilege(p.Name)).ThenBy(p => FormatPrivilege(p.Name)).ThenBy(p => p.Name))
                {
                    var name2 = item2.Name;

                    if (!commonPrivileges.Contains(name2))
                    {
                        continue;
                    }

                    if (enumerable1 != null)
                    {
                        var item1 = enumerable1.FirstOrDefault(i => i.Name == name2);

                        if (item1 != null)
                        {
                            continue;
                        }
                    }

                    var privilegedepthmask2 = (int)item2.GetAttributeValue<AliasedValue>("roleprivileges.privilegedepthmask").Value;

                    tableOnlyIn2.AddLine(name2, RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask2));

                    tableOnlyIn1.CalculateLineLengths(name2, RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask2));
                }
            }

            if (enumerable1 != null && enumerable2 != null)
            {
                foreach (var item1 in enumerable1.OrderBy(p => CategorizationPrivilege(p.Name)).ThenBy(p => FormatPrivilege(p.Name)).ThenBy(p => p.Name))
                {
                    var name1 = item1.Name;

                    if (!commonPrivileges.Contains(name1))
                    {
                        continue;
                    }

                    var item2 = enumerable2.FirstOrDefault(i => i.Name == name1);

                    if (item2 == null)
                    {
                        continue;
                    }

                    var privilegedepthmask1 = (int)item1.GetAttributeValue<AliasedValue>("roleprivileges.privilegedepthmask").Value;
                    var privilegedepthmask2 = (int)item2.GetAttributeValue<AliasedValue>("roleprivileges.privilegedepthmask").Value;

                    if (privilegedepthmask1 != privilegedepthmask2)
                    {
                        tableDifferent.AddLine(name1, RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask1), RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmask2));
                    }
                }
            }

            if (tableOnlyIn1.Count > 0)
            {
                if (diff.Count > 0) { diff.Add(string.Empty); }

                diff.Add(string.Format("Privileges ONLY in {0}: {1}", Connection1.Name, tableOnlyIn1.Count));
                tableOnlyIn1.GetFormatedLines(false).ForEach(s => diff.Add(tabSpacer + s));
            }

            if (tableOnlyIn2.Count > 0)
            {
                if (diff.Count > 0) { diff.Add(string.Empty); }

                diff.Add(string.Format("Privileges ONLY in {0}: {1}", Connection2.Name, tableOnlyIn2.Count));
                tableOnlyIn2.GetFormatedLines(false).ForEach(s => diff.Add(tabSpacer + s));
            }

            if (tableDifferent.Count > 0)
            {
                if (diff.Count > 0) { diff.Add(string.Empty); }

                diff.Add(string.Format("Different privileges in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableDifferent.Count));
                tableDifferent.GetFormatedLines(false).ForEach(s => diff.Add(tabSpacer + s));
            }
        }

        private string[] _prefixes = {
                "prvAppendTo"
                , "prvAppend"
                , "prvAssign"
                , "prvCreate"
                , "prvDelete"
                , "prvDisable"
                , "prvRead"
                , "prvShare"
                , "prvWrite"
                , "prvRollup"
                , "prvPublish"
                , "prvReparent"
        };

        private int CategorizationPrivilege(string name)
        {
            foreach (var item in _prefixes)
            {
                if (name.StartsWith(item, StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }
            }

            return 2;
        }

        private string FormatPrivilege(string name)
        {
            foreach (var item in _prefixes)
            {
                if (name.StartsWith(item, StringComparison.OrdinalIgnoreCase))
                {
                    return Regex.Replace(name, item, string.Empty, RegexOptions.IgnoreCase);
                }
            }

            return name;
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

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetFieldSecurityProfile1Async();
            var task2 = _comparerSource.GetFieldSecurityProfile2Async();

            var taskPerm1 = _comparerSource.GetFieldPermission1Async();
            var taskPerm2 = _comparerSource.GetFieldPermission2Async();

            var list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Field Security Profiles in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Field Security Profiles in {0}: {1}", Connection2.Name, list2.Count()));

            var listPermission1 = await taskPerm1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Field Security Profiles Permissions in {0}: {1}", Connection1.Name, listPermission1.Count()));

            var listPermission2 = await taskPerm2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Field Security Profiles Permissions in {0}: {1}", Connection2.Name, listPermission2.Count()));

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

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

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
                            i.EntityName == entityName1
                            && i.AttributeLogicalName == attributeName1
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
                            i.EntityName == entityName2
                            && i.AttributeLogicalName == attributeName2
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
                        i.EntityName == entityName1
                        && i.AttributeLogicalName == attributeName1
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
