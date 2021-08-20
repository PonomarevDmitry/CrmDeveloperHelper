using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class OrganizationComparer
    {
        #region Entities Properties

        public Task<string> CheckEntitiesAsync()
        {
            return Task.Run(() => CheckEntities());
        }

        private async Task<string> CheckEntities()
        {
            var content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEntitiesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var taskEntityMetadata1 = _comparerSource.GetEntityMetadataCollection1Async();
            var taskEntityMetadata2 = _comparerSource.GetEntityMetadataCollection2Async();

            var listEntityMetadata1 = await taskEntityMetadata1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection1.Name, listEntityMetadata1.Count()));

            var listEntityMetadata2 = await taskEntityMetadata2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection2.Name, listEntityMetadata2.Count()));

            if (!listEntityMetadata1.Any() && !listEntityMetadata2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            var entityMetadataOnlyExistsIn1 = new FormatTextTableHandler("EntityName", "IsManaged");

            var entityMetadataOnlyExistsIn2 = new FormatTextTableHandler("EntityName", "IsManaged");

            var entityMetadataDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (EntityMetadata entityMetadata1 in listEntityMetadata1.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor1.MetadataSource.StoreEntityMetadata(entityMetadata1);

                {
                    EntityMetadata entityMetadata2 = listEntityMetadata2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn1.AddLine(entityMetadata1.LogicalName, entityMetadata1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Entity, entityMetadata1.MetadataId.Value);
            }

            foreach (EntityMetadata entityMetadata2 in listEntityMetadata2.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor2.MetadataSource.StoreEntityMetadata(entityMetadata2);

                {
                    EntityMetadata entityMetadata1 = listEntityMetadata1.FirstOrDefault(e => e.LogicalName == entityMetadata2.LogicalName);

                    if (entityMetadata1 != null)
                    {
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn2.AddLine(entityMetadata2.LogicalName, entityMetadata2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Entity, entityMetadata2.MetadataId.Value);
            }

            var listNotExists = new HashSet<string>(
                listEntityMetadata1.Select(e => e.LogicalName)
                .Union(listEntityMetadata2.Select(e => e.LogicalName), StringComparer.InvariantCultureIgnoreCase)
                .Except(commonEntityMetadata.Select(e => e.Entity1.LogicalName), StringComparer.InvariantCultureIgnoreCase
            ), StringComparer.InvariantCultureIgnoreCase);

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesCommonFormat3, Connection1.Name, Connection2.Name, commonEntityMetadata.Count()));

            var optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_comparerSource.Service1), new StringMapRepository(_comparerSource.Service2));

            var comparer = new EntityMetadataComparer(tabSpacer, Connection1.Name, Connection2.Name, optionSetComparer, listNotExists);

            {
                var reporter = new ProgressReporter(_iWriteToOutput, commonEntityMetadata.Count, 5, Properties.OrganizationComparerStrings.EntitiesProcessingCommon);

                foreach (LinkedEntities<EntityMetadata> commonEntity in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
                {
                    reporter.Increase();

                    List<string> strDifference = await comparer.GetDifferenceAsync(this.ImageBuilder, commonEntity.Entity1, commonEntity.Entity2);

                    if (strDifference.Count > 0)
                    {
                        entityMetadataDifference.Add(commonEntity.Entity1.LogicalName, strDifference.Select(s => tabSpacer + s).ToList());

                        this.ImageBuilder.AddComponentDifferent((int)ComponentType.Entity, commonEntity.Entity1.MetadataId.Value, commonEntity.Entity2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                    }
                }
            }

            if (entityMetadataOnlyExistsIn1.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection1.Name, entityMetadataOnlyExistsIn1.Count);

                entityMetadataOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection2.Name, entityMetadataOnlyExistsIn2.Count);

                entityMetadataOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataDifference.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentDetailsFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

                    foreach (string str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (entityMetadataOnlyExistsIn2.Count == 0
                && entityMetadataOnlyExistsIn1.Count == 0
                && entityMetadataDifference.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.EntitiesNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.EntitiesFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        #endregion Entities Properties

        #region Entities Privileges

        public Task<string> CheckEntitiesPrivilegesAsync()
        {
            return Task.Run(() => CheckEntitiesPrivileges());
        }

        private async Task<string> CheckEntitiesPrivileges()
        {
            var content = new StringBuilder();

            var privilegeComparer = PrivilegeNameComparer.Comparer;

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEntitiesPrivilegesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var taskEntityMetadata1 = _comparerSource.GetEntityMetadataCollection1Async();
            var taskEntityMetadata2 = _comparerSource.GetEntityMetadataCollection2Async();

            var listEntityMetadata1 = await taskEntityMetadata1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection1.Name, listEntityMetadata1.Count()));

            var listEntityMetadata2 = await taskEntityMetadata2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection2.Name, listEntityMetadata2.Count()));

            if (!listEntityMetadata1.Any() && !listEntityMetadata2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }



            var taskRoles1 = _comparerSource.GetRole1Async();
            var taskRoles2 = _comparerSource.GetRole2Async();

            var listRoles1 = await taskRoles1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.SecurityRolesInConnectionFormat2, Connection1.Name, listRoles1.Count()));

            var listRoles2 = await taskRoles2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.SecurityRolesInConnectionFormat2, Connection2.Name, listRoles2.Count()));



            var taskPrivRole1 = new RolePrivilegesRepository(_comparerSource.Service1).GetListAsync(listRoles1.Select(e => e.RoleId.Value));
            var taskPrivRole2 = new RolePrivilegesRepository(_comparerSource.Service2).GetListAsync(listRoles2.Select(e => e.RoleId.Value));

            var listRolePrivilege1 = await taskPrivRole1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.RolePrivilegesInConnectionFormat2, Connection1.Name, listRolePrivilege1.Count()));

            var listRolePrivilege2 = await taskPrivRole2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.RolePrivilegesInConnectionFormat2, Connection2.Name, listRolePrivilege2.Count()));

            if (!listRoles1.Any() && !listRoles2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }



            var entityMetadataOnlyExistsIn1 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn1.SetHeader("EntityName", "IsManaged");

            var entityMetadataOnlyExistsIn2 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn2.SetHeader("EntityName", "IsManaged");

            var entityMetadataDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (EntityMetadata entityMetadata1 in listEntityMetadata1.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor1.MetadataSource.StoreEntityMetadata(entityMetadata1);

                {
                    EntityMetadata entityMetadata2 = listEntityMetadata2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn1.AddLine(entityMetadata1.LogicalName, entityMetadata1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Entity, entityMetadata1.MetadataId.Value);
            }

            foreach (EntityMetadata entityMetadata2 in listEntityMetadata2.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor2.MetadataSource.StoreEntityMetadata(entityMetadata2);

                {
                    EntityMetadata entityMetadata1 = listEntityMetadata1.FirstOrDefault(e => e.LogicalName == entityMetadata2.LogicalName);

                    if (entityMetadata1 != null)
                    {
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn2.AddLine(entityMetadata2.LogicalName, entityMetadata2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Entity, entityMetadata2.MetadataId.Value);
            }

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesCommonFormat3, Connection1.Name, Connection2.Name, commonEntityMetadata.Count()));



            var groupByRole1 = listRolePrivilege1.GroupBy(e => e.RoleId.Value).ToDictionary(g => g.Key, g => g.AsEnumerable());
            var groupByRole2 = listRolePrivilege2.GroupBy(e => e.RoleId.Value).ToDictionary(g => g.Key, g => g.AsEnumerable());

            var rolesOnlyExistsIn1 = new FormatTextTableHandler("Name", "BusinessUnit", "IsManaged");

            var rolesOnlyExistsIn2 = new FormatTextTableHandler("Name", "BusinessUnit", "IsManaged");

            var commonRolesList = new List<LinkedEntities<Role>>();

            foreach (var role1 in listRoles1)
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
                        role2 = listRoles2.FirstOrDefault(role => role.Id == role1.Id);
                    }

                    if (role2 == null && role1.RoleTemplateId != null)
                    {
                        role2 = listRoles2.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role1.RoleTemplateId.Id);
                    }

                    if (role2 != null)
                    {
                        commonRolesList.Add(new LinkedEntities<Role>(role1, role2));
                        continue;
                    }
                }

                string state = role1.FormattedValues[Role.Schema.Attributes.ismanaged];

                rolesOnlyExistsIn1.AddLine(name1, businessUnit1, state);

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Role, role1.Id);
            }

            foreach (var role2 in listRoles2)
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
                        role1 = listRoles1.FirstOrDefault(role => role.Id == role2.Id);
                    }

                    if (role1 == null && role2.RoleTemplateId != null)
                    {
                        role1 = listRoles1.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role2.RoleTemplateId.Id);
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

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.RolesCommonFormat3, Connection1.Name, Connection2.Name, commonRolesList.Count()));




            {
                var reporter = new ProgressReporter(_iWriteToOutput, commonEntityMetadata.Count, 5, Properties.OrganizationComparerStrings.EntitiesProcessingCommon);

                foreach (LinkedEntities<EntityMetadata> commonEntity in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
                {
                    reporter.Increase();

                    List<string> strDifference = CompareEntityPrivileges(commonEntity.Entity1, commonEntity.Entity2, commonRolesList, groupByRole1, groupByRole2);

                    if (strDifference.Count > 0)
                    {
                        entityMetadataDifference.Add(commonEntity.Entity1.LogicalName, strDifference.Select(s => tabSpacer + s).ToList());

                        this.ImageBuilder.AddComponentDifferent((int)ComponentType.Entity, commonEntity.Entity1.MetadataId.Value, commonEntity.Entity2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                    }
                }
            }





            if (entityMetadataOnlyExistsIn1.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection1.Name, entityMetadataOnlyExistsIn1.Count);

                entityMetadataOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection2.Name, entityMetadataOnlyExistsIn2.Count);

                entityMetadataOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
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

            if (entityMetadataDifference.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentDetailsFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

                    foreach (string str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (entityMetadataOnlyExistsIn2.Count == 0
                && entityMetadataOnlyExistsIn1.Count == 0
                && entityMetadataDifference.Count == 0

                && rolesOnlyExistsIn1.Count == 0
                && rolesOnlyExistsIn2.Count == 0
            )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.EntitiesNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.EntitiesPrivilegesFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private static readonly PrivilegeType[] _entityPrivilegesOrder = new PrivilegeType[]
        {
            PrivilegeType.None
            , PrivilegeType.Create
            , PrivilegeType.Read
            , PrivilegeType.Write
            , PrivilegeType.Delete
            , PrivilegeType.Assign
            , PrivilegeType.Share
            , PrivilegeType.Append
            , PrivilegeType.AppendTo
        };

        private List<string> CompareEntityPrivileges(
            EntityMetadata entityMetadata1
            , EntityMetadata entityMetadata2
            , List<LinkedEntities<Role>> commonRolesList
            , Dictionary<Guid, IEnumerable<RolePrivileges>> groupByRole1
            , Dictionary<Guid, IEnumerable<RolePrivileges>> groupByRole2
        )
        {
            var tableFullDifferences = new FormatTextTableHandler(
                "Role",
                "BusinessUnit",
                "PrivilegeName",
                "PrivilegeType",
                Connection1.Name,
                Connection2.Name
            );

            foreach (var commonRole in commonRolesList)
            {
                groupByRole1.TryGetValue(commonRole.Entity1.Id, out IEnumerable<RolePrivileges> enumerable1);
                groupByRole2.TryGetValue(commonRole.Entity2.Id, out IEnumerable<RolePrivileges> enumerable2);

                for (int index = 0; index < _entityPrivilegesOrder.Length; index++)
                {
                    var privilegeType = _entityPrivilegesOrder[index];

                    SecurityPrivilegeMetadata entityPrivilege1 = entityMetadata1?.Privileges?.FirstOrDefault(i => i.PrivilegeType == privilegeType);
                    SecurityPrivilegeMetadata entityPrivilege2 = entityMetadata2?.Privileges?.FirstOrDefault(i => i.PrivilegeType == privilegeType);

                    if (entityPrivilege1 != null || entityPrivilege2 != null)
                    {
                        RolePrivileges rolePriv1 = null;
                        RolePrivileges rolePriv2 = null;

                        if (entityPrivilege1 != null)
                        {
                            rolePriv1 = enumerable1.FirstOrDefault(r => r.PrivilegeId == entityPrivilege1.PrivilegeId);
                        }

                        if (entityPrivilege2 != null)
                        {
                            rolePriv2 = enumerable2.FirstOrDefault(r => r.PrivilegeId == entityPrivilege2.PrivilegeId);
                        }

                        var privilegedepthmaskValue1 = rolePriv1?.PrivilegeDepthMask;
                        var privilegedepthmaskValue2 = rolePriv2?.PrivilegeDepthMask;

                        SecurityPrivilegeMetadata entityPrivilege = entityPrivilege1 ?? entityPrivilege2;

                        if (privilegedepthmaskValue1 != privilegedepthmaskValue2)
                        {
                            tableFullDifferences.AddLine(
                                commonRole.Entity1.Name,
                                commonRole.Entity1.BusinessUnitParentBusinessUnit == null ? "Root Organization" : commonRole.Entity1.BusinessUnitId.Name,
                                entityPrivilege.Name,
                                entityPrivilege.PrivilegeType.ToString(),
                                RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmaskValue1),
                                RolePrivilegesRepository.GetPrivilegeDepthMaskName(privilegedepthmaskValue2)
                            );
                        }
                    }
                }
            }

            var result = new List<string>();

            if (tableFullDifferences.Count > 0)
            {
                if (result.Count > 0) { result.Add(string.Empty); }

                result.Add(string.Format("Full Differences privileges in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableFullDifferences.Count));
                tableFullDifferences.GetFormatedLines(false).ForEach(s => result.Add(tabSpacer + s));
            }

            return result;
        }

        #endregion Entities Privileges

        #region Entities Audits

        public Task<string> CheckEntitiesByAuditAsync()
        {
            return Task.Run(() => CheckEntitiesByAudit());
        }

        private async Task<string> CheckEntitiesByAudit()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEntitiesAuditFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var listEntityMetadata1 = await _comparerSource.GetEntityMetadataCollection1Async();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection1.Name, listEntityMetadata1.Count()));

            var listEntityMetadata2 = await _comparerSource.GetEntityMetadataCollection2Async();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection2.Name, listEntityMetadata2.Count()));

            if (!listEntityMetadata1.Any() && !listEntityMetadata2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            FormatTextTableHandler entityMetadataOnlyExistsIn1 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn1.SetHeader("EntityName", "IsManaged", "IsCustomizable", "IsAuditEnabled");

            FormatTextTableHandler entityMetadataOnlyExistsIn2 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn2.SetHeader("EntityName", "IsManaged", "IsCustomizable", "IsAuditEnabled");

            FormatTextTableHandler entityMetadataDifference = new FormatTextTableHandler();
            entityMetadataDifference.SetHeader("EntityName"
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsManaged", Connection1.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsManaged", Connection2.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsCustomizable", Connection1.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsCustomizable", Connection2.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsAuditEnabled", Connection1.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsAuditEnabled", Connection2.Name)
            );

            FormatTextTableHandler attributeDifference = new FormatTextTableHandler();
            attributeDifference.SetHeader("EntityName", "AttributeName"
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsManaged", Connection1.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsManaged", Connection2.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsCustomizable", Connection1.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsCustomizable", Connection2.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsAuditEnabled", Connection1.Name)
                , string.Format(Properties.OrganizationComparerStrings.AttributeInConnectionFormat2, "IsAuditEnabled", Connection2.Name)
            );

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (EntityMetadata entityMetadata1 in listEntityMetadata1.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor1.MetadataSource.StoreEntityMetadata(entityMetadata1);

                {
                    EntityMetadata entityMetadata2 = listEntityMetadata2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn1.AddLine(entityMetadata1.LogicalName
                    , entityMetadata1.IsManaged.ToString()
                    , entityMetadata1.IsCustomizable?.Value.ToString()
                    , entityMetadata1.IsAuditEnabled?.Value.ToString()
                );

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Entity, entityMetadata1.MetadataId.Value);
            }

            foreach (EntityMetadata entityMetadata2 in listEntityMetadata2.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor2.MetadataSource.StoreEntityMetadata(entityMetadata2);

                {
                    EntityMetadata entityMetadata1 = listEntityMetadata1.FirstOrDefault(e => e.LogicalName == entityMetadata2.LogicalName);

                    if (entityMetadata1 != null)
                    {
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn2.AddLine(entityMetadata2.LogicalName
                    , entityMetadata2.IsManaged.ToString()
                    , entityMetadata2.IsCustomizable?.Value.ToString()
                    , entityMetadata2.IsAuditEnabled?.Value.ToString()
                );

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Entity, entityMetadata2.MetadataId.Value);
            }

            var listNotExists = new HashSet<string>(
                listEntityMetadata1.Select(e => e.LogicalName)
                .Union(listEntityMetadata2.Select(e => e.LogicalName), StringComparer.InvariantCultureIgnoreCase)
                .Except(commonEntityMetadata.Select(e => e.Entity1.LogicalName), StringComparer.InvariantCultureIgnoreCase
            ), StringComparer.InvariantCultureIgnoreCase);

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesCommonFormat3, Connection1.Name, Connection2.Name, commonEntityMetadata.Count()));

            OptionSetComparer optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_comparerSource.Service1), new StringMapRepository(_comparerSource.Service2));

            EntityMetadataComparer comparer = new EntityMetadataComparer(tabSpacer, Connection1.Name, Connection2.Name, optionSetComparer, listNotExists);

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, commonEntityMetadata.Count, 5, Properties.OrganizationComparerStrings.EntitiesProcessingCommon);

                foreach (LinkedEntities<EntityMetadata> commonEntity in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
                {
                    reporter.Increase();

                    if (commonEntity.Entity1.IsAuditEnabled?.Value != commonEntity.Entity2.IsAuditEnabled?.Value)
                    {
                        entityMetadataDifference.AddLine(commonEntity.Entity1.LogicalName
                            , commonEntity.Entity1.IsManaged.ToString()
                            , commonEntity.Entity2.IsManaged.ToString()
                            , commonEntity.Entity1.IsCustomizable?.Value.ToString()
                            , commonEntity.Entity2.IsCustomizable?.Value.ToString()
                            , commonEntity.Entity1.IsAuditEnabled?.Value.ToString()
                            , commonEntity.Entity2.IsAuditEnabled?.Value.ToString()
                        );
                    }

                    foreach (var attribute1 in commonEntity.Entity1.Attributes.OrderBy(a => a.LogicalName))
                    {
                        var attribute2 = commonEntity.Entity1.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, attribute1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                        if (attribute2 != null)
                        {
                            if (attribute1.IsAuditEnabled?.Value != attribute2.IsAuditEnabled?.Value)
                            {
                                attributeDifference.AddLine(commonEntity.Entity1.LogicalName
                                    , attribute1.LogicalName
                                    , attribute1.IsManaged.ToString()
                                    , attribute2.IsManaged.ToString()
                                    , attribute1.IsCustomizable?.Value.ToString()
                                    , attribute2.IsCustomizable?.Value.ToString()
                                    , attribute1.IsAuditEnabled?.Value.ToString()
                                    , attribute2.IsAuditEnabled?.Value.ToString()
                                );
                            }
                        }
                    }
                }
            }

            if (entityMetadataOnlyExistsIn1.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection1.Name, entityMetadataOnlyExistsIn1.Count);

                entityMetadataOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection2.Name, entityMetadataOnlyExistsIn2.Count);

                entityMetadataOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataDifference.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentByAuditFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                entityMetadataDifference.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (attributeDifference.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.AttributesDifferentByAuditFormat3, Connection1.Name, Connection2.Name, attributeDifference.Count);

                attributeDifference.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count == 0
                && entityMetadataOnlyExistsIn1.Count == 0
                && entityMetadataDifference.Count == 0
                && attributeDifference.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.EntitiesNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.EntitiesByAuditFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        #endregion Entities Audits

        #region Entities Labels

        public Task<string> CheckEntityLabelsAsync()
        {
            return Task.Run(() => CheckEntityLabels());
        }

        private async Task<string> CheckEntityLabels()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEntityLabelsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var listEntityMetadata1 = await this._comparerSource.GetEntityMetadataCollection1Async();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection1.Name, listEntityMetadata1.Count()));

            var listEntityMetadata2 = await this._comparerSource.GetEntityMetadataCollection2Async();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection2.Name, listEntityMetadata2.Count()));

            if (!listEntityMetadata1.Any() && !listEntityMetadata2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            FormatTextTableHandler entityMetadataOnlyExistsIn1 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn1.SetHeader("EntityName", "IsManaged");

            FormatTextTableHandler entityMetadataOnlyExistsIn2 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn2.SetHeader("EntityName", "IsManaged");

            Dictionary<string, List<string>> entityMetadataDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            List<LinkedEntities<EntityMetadata>> commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (EntityMetadata entityMetadata1 in listEntityMetadata1.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor1.MetadataSource.StoreEntityMetadata(entityMetadata1);

                {
                    EntityMetadata entityMetadata2 = listEntityMetadata2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn1.AddLine(entityMetadata1.LogicalName, entityMetadata1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Entity, entityMetadata1.MetadataId.Value);
            }

            foreach (EntityMetadata entityMetadata2 in listEntityMetadata2.OrderBy(e => e.LogicalName))
            {
                ImageBuilder.Descriptor2.MetadataSource.StoreEntityMetadata(entityMetadata2);

                {
                    EntityMetadata entityMetadata1 = listEntityMetadata1.FirstOrDefault(e => e.LogicalName == entityMetadata2.LogicalName);

                    if (entityMetadata1 != null)
                    {
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn2.AddLine(entityMetadata2.LogicalName, entityMetadata2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Entity, entityMetadata2.MetadataId.Value);
            }

            var listNotExists = new HashSet<string>(
                listEntityMetadata1.Select(e => e.LogicalName)
                .Union(listEntityMetadata2.Select(e => e.LogicalName), StringComparer.InvariantCultureIgnoreCase)
                .Except(commonEntityMetadata.Select(e => e.Entity1.LogicalName), StringComparer.InvariantCultureIgnoreCase
            ), StringComparer.InvariantCultureIgnoreCase);

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesCommonFormat3, Connection1.Name, Connection2.Name, commonEntityMetadata.Count()));

            OptionSetComparer optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_comparerSource.Service1), new StringMapRepository(_comparerSource.Service2));

            EntityLabelComparer comparer = new EntityLabelComparer(tabSpacer, Connection1.Name, Connection2.Name, optionSetComparer, listNotExists);

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, commonEntityMetadata.Count, 5, Properties.OrganizationComparerStrings.EntitiesProcessingCommon);

                foreach (LinkedEntities<EntityMetadata> commonEntity in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
                {
                    List<string> strDifference = await comparer.GetDifference(this.ImageBuilder, commonEntity.Entity1, commonEntity.Entity2);

                    if (strDifference.Count > 0)
                    {
                        entityMetadataDifference.Add(commonEntity.Entity1.LogicalName, strDifference.Select(s => tabSpacer + s).ToList());

                        this.ImageBuilder.AddComponentDifferent((int)ComponentType.Entity, commonEntity.Entity1.MetadataId.Value, commonEntity.Entity2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                    }
                }
            }

            if (entityMetadataOnlyExistsIn1.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection1.Name, entityMetadataOnlyExistsIn1.Count);

                entityMetadataOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesOnlyExistsInConnectionFormat2, Connection2.Name, entityMetadataOnlyExistsIn2.Count);

                entityMetadataOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataDifference.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntitiesDifferentDetailsFormat3, Connection1.Name, Connection2.Name, entityMetadataDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

                    foreach (string str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (entityMetadataOnlyExistsIn2.Count == 0
                && entityMetadataOnlyExistsIn1.Count == 0
                && entityMetadataDifference.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.EntitiesNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.EntityLabelsFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        #endregion Entities Labels

        #region EntityMaps

        public Task<string> CheckEntityMapsAsync()
        {
            return Task.Run(() => CheckEntityMaps());
        }

        private async Task<string> CheckEntityMaps()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEntityMapsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            Task<List<EntityMap>> task1 = _comparerSource.GetEntityMap1Async();
            Task<List<EntityMap>> task2 = _comparerSource.GetEntityMap2Async();

            List<EntityMap> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntityMapsInConnectionFormat2, Connection1.Name, list1.Count()));

            List<EntityMap> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntityMapsInConnectionFormat2, Connection2.Name, list2.Count()));

            if (!list1.Any() && !list2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            Task<List<AttributeMap>> taskAttr1 = _comparerSource.GetAttributeMap1Async();
            Task<List<AttributeMap>> taskAttr2 = _comparerSource.GetAttributeMap2Async();

            //new OrderExpression("sourceentityname", OrderType.Ascending),
            //new OrderExpression("targetentityname", OrderType.Ascending),

            List<AttributeMap> listPermission1 = await taskAttr1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.AttributeMapsInConnectionFormat2, Connection1.Name, listPermission1.Count()));

            List<AttributeMap> listPermission2 = await taskAttr2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.AttributeMapsInConnectionFormat2, Connection2.Name, listPermission2.Count()));

            IEnumerable<IGrouping<Guid, AttributeMap>> group1 = listPermission1.GroupBy(e => e.EntityMapId.Id);
            IEnumerable<IGrouping<Guid, AttributeMap>> group2 = listPermission2.GroupBy(e => e.EntityMapId.Id);

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Source", "", "Target", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Source", "", "Target", "IsManaged");

            Dictionary<Tuple<string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (EntityMap entityMap1 in list1)
            {
                string source1 = entityMap1.SourceEntityName;
                string target1 = entityMap1.TargetEntityName;

                {
                    EntityMap entityMap2 = list2.FirstOrDefault(entityMap =>
                    {
                        string source2 = entityMap.SourceEntityName;
                        string target2 = entityMap.TargetEntityName;

                        return source1 == source2 && target1 == target2;
                    });

                    if (entityMap2 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn1.AddLine(source1, "->", target1, entityMap1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.EntityMap, entityMap1.Id);
            }

            foreach (EntityMap entityMap2 in list2)
            {
                string source2 = entityMap2.SourceEntityName;
                string target2 = entityMap2.TargetEntityName;

                {
                    EntityMap entityMap1 = list1.FirstOrDefault(entityMap =>
                    {
                        string source1 = entityMap.SourceEntityName;
                        string target1 = entityMap.TargetEntityName;

                        return source1 == source2 && target1 == target2;
                    });

                    if (entityMap1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(source2, "->", target2, entityMap2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.EntityMap, entityMap2.Id);
            }

            foreach (EntityMap entityMap1 in list1)
            {
                string source1 = entityMap1.SourceEntityName;
                string target1 = entityMap1.TargetEntityName;

                EntityMap entityMap2 = list2.FirstOrDefault(entityMap =>
                {
                    string source2 = entityMap.SourceEntityName;
                    string target2 = entityMap.TargetEntityName;

                    return source1 == source2 && target1 == target2;
                });

                if (entityMap2 == null)
                {
                    continue;
                }

                IEnumerable<AttributeMap> enumerable1 = group1.FirstOrDefault(g => g.Key == entityMap1.Id);
                IEnumerable<AttributeMap> enumerable2 = group2.FirstOrDefault(g => g.Key == entityMap2.Id);

                List<string> diff = CompareAttributeMap(enumerable1, enumerable2, tabSpacer);

                if (diff.Count > 0)
                {
                    dictDifference.Add(Tuple.Create(source1, target1), diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.EntityMap, entityMap1.Id, entityMap2.Id, string.Join(Environment.NewLine, diff));
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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.EntityMapsOnlyExistsInConnectionFormat2, Connection1.Name, tableOnlyExistsIn1.Count);

                tableOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.EntityMapsOnlyExistsInConnectionFormat2, Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (dictDifference.Count > 0)
            {
                IOrderedEnumerable<KeyValuePair<Tuple<string, string>, List<string>>> order = dictDifference.OrderBy(s => s.Key.Item1).ThenBy(s => s.Key.Item2);

                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntityMapsDifferentFormat3, Connection1.Name, Connection2.Name, dictDifference.Count);

                {
                    FormatTextTableHandler table = new FormatTextTableHandler();

                    foreach (KeyValuePair<Tuple<string, string>, List<string>> item in order)
                    {
                        table.AddLine(item.Key.Item1, "->", item.Key.Item2);
                    }

                    table.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
                }


                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.EntityMapsDifferentDetailsFormat3, Connection1.Name, Connection2.Name, dictDifference.Count);

                foreach (KeyValuePair<Tuple<string, string>, List<string>> item in order)
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + string.Format(Properties.OrganizationComparerStrings.AttributeMapFormat2, item.Key.Item1, item.Key.Item2)).TrimEnd());

                    foreach (string str in item.Value)
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
                content.AppendLine(Properties.OrganizationComparerStrings.EntityMapsNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.EntityMapsFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private List<string> CompareAttributeMap(IEnumerable<AttributeMap> enumerable1, IEnumerable<AttributeMap> enumerable2, string tabSpacer)
        {
            List<string> diff = new List<string>();

            FormatTextTableHandler tableOnlyIn1 = new FormatTextTableHandler();
            tableOnlyIn1.SetHeader("SourceAttribute", "", "TargetAttribute", "IsSystem", "IsManaged");

            FormatTextTableHandler tableOnlyIn2 = new FormatTextTableHandler();
            tableOnlyIn2.SetHeader("SourceAttribute", "", "TargetAttribute", "IsSystem", "IsManaged");

            if (enumerable1 != null)
            {
                foreach (AttributeMap item1 in enumerable1)
                {
                    string source1 = item1.SourceAttributeName;
                    string target1 = item1.TargetAttributeName;

                    if (enumerable2 != null)
                    {
                        AttributeMap item2 = enumerable2.FirstOrDefault(i =>
                            i.SourceAttributeName == source1
                            && i.TargetAttributeName == target1
                            );

                        if (item2 != null)
                        {
                            continue;
                        }
                    }

                    tableOnlyIn1.AddLine(source1, "->", target1
                        , item1.IsSystem.ToString()
                        , item1.IsManaged.ToString()
                        );

                    tableOnlyIn2.CalculateLineLengths(source1, "->", target1
                        , item1.IsSystem.ToString()
                        , item1.IsManaged.ToString()
                        );
                }
            }

            if (enumerable2 != null)
            {
                foreach (AttributeMap item2 in enumerable2)
                {
                    string source2 = item2.SourceAttributeName;
                    string target2 = item2.TargetAttributeName;

                    if (enumerable1 != null)
                    {
                        AttributeMap item1 = enumerable1.FirstOrDefault(i =>
                            i.SourceAttributeName == source2
                            && i.TargetAttributeName == target2
                            );

                        if (item1 != null)
                        {
                            continue;
                        }
                    }

                    tableOnlyIn2.AddLine(source2, "->", target2
                        , item2.IsSystem.ToString()
                        , item2.IsManaged.ToString()
                        );

                    tableOnlyIn1.CalculateLineLengths(source2, "->", target2
                        , item2.IsSystem.ToString()
                        , item2.IsManaged.ToString()
                        );
                }
            }

            if (tableOnlyIn1.Count > 0)
            {
                diff.Add(string.Format(Properties.OrganizationComparerStrings.AttributeMapsOnlyExistsInConnectionFormat2, Connection1.Name, tableOnlyIn1.Count));
                tableOnlyIn1.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }

            if (tableOnlyIn2.Count > 0)
            {
                diff.Add(string.Format(Properties.OrganizationComparerStrings.AttributeMapsOnlyExistsInConnectionFormat2, Connection2.Name, tableOnlyIn2.Count));
                tableOnlyIn2.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }

            return diff;
        }

        #endregion EntityMaps

        #region Entities Ribbons

        public Task<string> CheckEntityRibbonsAsync(bool withDetails)
        {
            return Task.Run(() => CheckEntityRibbons(withDetails));
        }

        private async Task<string> CheckEntityRibbons(bool withDetails)
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEntityRibbonsFormat2, Connection1.Name, Connection2.Name);

            if (withDetails)
            {
                operation = string.Format(Properties.OperationNames.CheckingEntityRibbonsWithDetailsFormat2, Connection1.Name, Connection2.Name);
            }

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            Task<HashSet<string>> task1 = _comparerSource.GetEntitiesWithRibbonCustomization1Async();
            Task<HashSet<string>> task2 = _comparerSource.GetEntitiesWithRibbonCustomization2Async();

            HashSet<string> listRibbon1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.RibbonsInConnectionFormat2, Connection1.Name, listRibbon1.Count()));

            HashSet<string> listRibbon2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.RibbonsInConnectionFormat2, Connection2.Name, listRibbon2.Count()));

            var entities1 = await this._comparerSource.GetEntityMetadataCollection1Async();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection1.Name, entities1.Count()));

            var entities2 = await this._comparerSource.GetEntityMetadataCollection2Async();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.EntitiesInConnectionFormat2, Connection2.Name, entities2.Count()));

            if (!entities1.Any() && !entities2.Any())
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.ThereIsNothingToCompare);
                _iWriteToOutput.WriteToOutputEndOperation(null, operation);
                return null;
            }

            //var list1 = entities1.Select(e => e.LogicalName);
            //var list2 = entities2.Select(e => e.LogicalName);

            List<string> list = listRibbon1.Union(listRibbon2).Distinct().OrderBy(s => s).ToList();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Common Ribbons in {0} and {1}: {2}", Connection1.Name, Connection2.Name, list.Count()));

            List<string> listOnlyExistsIn1 = new List<string>();

            List<string> listOnlyExistsIn2 = new List<string>();

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();
            if (withDetails)
            {
                tableDifferent.SetHeader("EntityName", "-Deletes", "(-Length)", "+Inserts", "(+Length)", "Note");
            }

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, list.Count, 5, "Processing Common Ribbons");

                foreach (string entityName in list)
                {
                    reporter.Increase();

                    EntityMetadata entity1 = entities1.FirstOrDefault(e => e.LogicalName == entityName);
                    EntityMetadata entity2 = entities2.FirstOrDefault(e => e.LogicalName == entityName);

                    if (entity1 == null && entity2 != null)
                    {
                        listOnlyExistsIn2.Add(entityName);

                        this.ImageBuilder.AddComponentSolution2((int)ComponentType.Entity, entity2.MetadataId.Value);
                    }
                    else if (entity1 != null && entity2 == null)
                    {
                        listOnlyExistsIn1.Add(entityName);

                        this.ImageBuilder.AddComponentSolution1((int)ComponentType.Entity, entity1.MetadataId.Value);
                    }
                    else if (entity1 != null && entity2 != null)
                    {
                        RetrieveEntityRibbonRequest request = new RetrieveEntityRibbonRequest()
                        {
                            RibbonLocationFilter = RibbonLocationFilters.All,
                            EntityName = entityName,
                        };

                        string xml1 = string.Empty;
                        string xml2 = string.Empty;

                        try
                        {
                            RetrieveEntityRibbonResponse response1 = (RetrieveEntityRibbonResponse)_comparerSource.Service1.Execute(request);
                            RetrieveEntityRibbonResponse response2 = (RetrieveEntityRibbonResponse)_comparerSource.Service2.Execute(request);

                            byte[] array1 = FileOperations.UnzipRibbon(response1.CompressedEntityXml);
                            byte[] array2 = FileOperations.UnzipRibbon(response2.CompressedEntityXml);

                            xml1 = Encoding.UTF8.GetString(array1);
                            xml2 = Encoding.UTF8.GetString(array2);

                            xml1 = ContentComparerHelper.RemoveDiacritics(xml1);
                            xml2 = ContentComparerHelper.RemoveDiacritics(xml2);
                        }
                        catch (Exception ex)
                        {
                            this._iWriteToOutput.WriteErrorToOutput(null, ex);
                        }

                        ContentCompareResult compare = ContentComparerHelper.CompareXML(xml1, xml2, withDetails);

                        if (!compare.IsEqual)
                        {
                            this.ImageBuilder.AddComponentDifferent((int)ComponentType.Entity, entity1.MetadataId.Value, entity2.MetadataId.Value, ":RibbonDiffXml");

                            if (withDetails)
                            {
                                tableDifferent.AddLine(entityName
                                    , string.Format("-{0}", compare.Deletes)
                                    , string.Format("(-{0})", compare.DeleteLength)
                                    , string.Format("+{0}", compare.Inserts)
                                    , string.Format("(+{0})", compare.InsertLength)
                                    , compare.GetDescription()
                                    );
                            }
                            else
                            {
                                tableDifferent.AddLine(entityName);
                            }
                        }
                    }
                }
            }

            if (listOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Ribbon ONLY EXISTS in {0}: {1}", Connection1.Name, listOnlyExistsIn1.Count);

                listOnlyExistsIn1.ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (listOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Ribbon ONLY EXISTS in {0}: {1}", Connection2.Name, listOnlyExistsIn2.Count);

                listOnlyExistsIn2.ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableDifferent.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Ribbon DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableDifferent.Count);

                tableDifferent.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (listOnlyExistsIn1.Count == 0
                && listOnlyExistsIn2.Count == 0
                && tableDifferent.Count == 0
                )
            {
                content.AppendLine("No difference in Entity Ribbon.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, string.Format("Ribbons{0}", withDetails ? " with details" : string.Empty));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        #endregion Entities Ribbons
    }
}
