using Microsoft.Xrm.Sdk.Messages;
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
        public Task<string> CheckConnectionRolesAsync()
        {
            return Task.Run(async () => await CheckConnectionRoles());
        }

        private async Task<string> CheckConnectionRoles()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Connection Roles started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            var repRole1 = new ConnectionRoleRepository(_service1);
            var repRole2 = new ConnectionRoleRepository(_service2);

            var repRoleAssociation1 = new ConnectionRoleAssociationRepository(_service1);
            var repRoleAssociation2 = new ConnectionRoleAssociationRepository(_service2);

            var repRoleObjectTypeCode1 = new ConnectionRoleObjectTypeCodeRepository(_service1);
            var repRoleObjectTypeCode2 = new ConnectionRoleObjectTypeCodeRepository(_service2);




            var listRole1 = await repRole1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Connection Roles in {0}: {1}", Connection1.Name, listRole1.Count));

            var listRole2 = await repRole2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Connection Roles in {0}: {1}", Connection2.Name, listRole2.Count));





            var listRoleAssociation1 = await repRoleAssociation1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Connection Role Associations in {0}: {1}", Connection1.Name, listRoleAssociation1.Count));

            var listRoleAssociation2 = await repRoleAssociation2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Connection Role Associations in {0}: {1}", Connection2.Name, listRoleAssociation2.Count));





            var listRoleObjectTypeCode1 = await repRoleObjectTypeCode1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Connection Role ObjectTypeCodes in {0}: {1}", Connection1.Name, listRoleObjectTypeCode1.Count));

            var listRoleObjectTypeCode2 = await repRoleObjectTypeCode2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Connection Role ObjectTypeCodes in {0}: {1}", Connection2.Name, listRoleObjectTypeCode2.Count));








            var commonRolesList = new List<LinkedEntities<ConnectionRole>>();

            var dictDifference = new Dictionary<LinkedEntities<ConnectionRole>, List<string>>();



            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Category", "Name", "Id", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Category", "Name", "Id", "IsManaged");

            foreach (var role1 in listRole1)
            {
                {
                    var role2 = listRole2.FirstOrDefault(role => role.Id == role1.Id);

                    if (role2 != null)
                    {
                        commonRolesList.Add(new LinkedEntities<ConnectionRole>(role1, role2));

                        continue;
                    }
                }

                var category1 = role1.FormattedValues[ConnectionRole.Schema.Attributes.category];
                var name1 = role1.Name;

                tableOnlyExistsIn1.AddLine(category1, name1, role1.Id.ToString(), role1.IsManaged.ToString());
            }

            foreach (var role2 in listRole2)
            {
                {
                    var role1 = listRole1.FirstOrDefault(role => role.Id == role2.Id);

                    if (role1 != null)
                    {
                        continue;
                    }
                }

                var name2 = role2.Name;
                var category2 = role2.FormattedValues[ConnectionRole.Schema.Attributes.category];

                tableOnlyExistsIn2.AddLine(category2, name2, role2.Id.ToString(), role2.IsManaged.ToString());
            }

            content.AppendLine(_writeToOutput.WriteToOutput("Common Roles in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonRolesList.Count()));

            foreach (var commonItem in commonRolesList)
            {
                var diff = new List<string>();

                {
                    FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                    tabDiff.SetHeader("Attribute", "Organization", "Value");

                    List<string> fieldsToCompare = new List<string>()
                    {
                        ConnectionRole.Schema.Attributes.category
                        , ConnectionRole.Schema.Attributes.componentstate
                        //, ConnectionRole.Schema.Attributes.connectionroleid
                        //, ConnectionRole.Schema.Attributes.connectionroleidunique
                        //, ConnectionRole.Schema.Attributes.createdby
                        //, ConnectionRole.Schema.Attributes.createdon
                        //, ConnectionRole.Schema.Attributes.createdonbehalfby
                        , ConnectionRole.Schema.Attributes.description
                        //, ConnectionRole.Schema.Attributes.importsequencenumber
                        //, ConnectionRole.Schema.Attributes.introducedversion
                        , ConnectionRole.Schema.Attributes.iscustomizable
                        //, ConnectionRole.Schema.Attributes.ismanaged
                        //, ConnectionRole.Schema.Attributes.modifiedby
                        //, ConnectionRole.Schema.Attributes.modifiedon
                        //, ConnectionRole.Schema.Attributes.modifiedonbehalfby
                        , ConnectionRole.Schema.Attributes.name
                        //, ConnectionRole.Schema.Attributes.organizationid
                        //, ConnectionRole.Schema.Attributes.overwritetime
                        //, ConnectionRole.Schema.Attributes.solutionid
                        , ConnectionRole.Schema.Attributes.statecode
                        , ConnectionRole.Schema.Attributes.statuscode
                        //, ConnectionRole.Schema.Attributes.supportingsolutionid
                        //, ConnectionRole.Schema.Attributes.versionnumber
                    };

                    foreach (var fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(commonItem.Entity1, commonItem.Entity2, fieldName))
                        {
                            var str1 = EntityDescriptionHandler.GetAttributeString(commonItem.Entity1, fieldName, _service1.ConnectionData);
                            var str2 = EntityDescriptionHandler.GetAttributeString(commonItem.Entity2, fieldName, _service2.ConnectionData);

                            tabDiff.AddLine(fieldName, Connection1.Name, str1);
                            tabDiff.AddLine(fieldName, Connection2.Name, str2);
                        }
                    }

                    if (tabDiff.Count > 0)
                    {
                        diff.AddRange(tabDiff.GetFormatedLines(false));
                    }
                }

                {
                    var enumerable1 = listRoleObjectTypeCode1
                        .Where(e => e.ConnectionRoleId != null && e.ConnectionRoleId.Id == commonItem.Entity1.ConnectionRoleId)
                        .Select(e => e.AssociatedObjectTypeCode)
                        .Distinct()
                        ;

                    var enumerable2 = listRoleObjectTypeCode2
                        .Where(e => e.ConnectionRoleId != null && e.ConnectionRoleId.Id == commonItem.Entity2.ConnectionRoleId)
                        .Select(e => e.AssociatedObjectTypeCode)
                        .Distinct()
                        ;

                    CompareConnectionRoleAssociatedObjects("Associated Object Type Codes", diff, enumerable1, enumerable2);
                }

                {
                    var enumerable1 = listRoleAssociation1
                        .Where(e => e.ConnectionRoleId != null && e.ConnectionRoleId == commonItem.Entity1.ConnectionRoleId)
                        .Select(e => e.AssociatedConnectionRoleName)
                        .Distinct()
                        ;

                    var enumerable2 = listRoleAssociation2
                        .Where(e => e.ConnectionRoleId != null && e.ConnectionRoleId == commonItem.Entity2.ConnectionRoleId)
                        .Select(e => e.AssociatedConnectionRoleName)
                        .Distinct()
                        ;

                    CompareConnectionRoleAssociatedObjects("Associated Connection Roles", diff, enumerable1, enumerable2);
                }

                if (diff.Count > 0)
                {
                    dictDifference.Add(commonItem, diff);
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

                content.AppendLine().AppendLine().AppendFormat("Connection Roles ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Connection Roles ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
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

                content.AppendLine().AppendLine().AppendFormat("Connection Roles DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader(Connection1.Name, Connection2.Name, "Id");

                foreach (var item in dictDifference)
                {
                    tableDifference.CalculateLineLengths(item.Key.Entity1.Name, item.Key.Entity2.Name, item.Key.Entity1.Id.ToString());
                }

                foreach (var item in dictDifference
                    .OrderBy(w => w.Key.Entity1.Name)
                    .ThenBy(w => w.Key.Entity2.Name)
                    .ThenBy(w => w.Key.Entity1.Id.ToString())
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(item.Key.Entity1.Name, item.Key.Entity2.Name, item.Key.Entity1.Id.ToString()));

                    foreach (var str in item.Value)
                    {
                        content.AppendLine().Append(tabSpacer + tabSpacer + str);
                    }
                }
            }

            if (tableOnlyExistsIn2.Count == 0
                && tableOnlyExistsIn1.Count == 0
                && dictDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Connection Roles.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Connection Roles ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} Connection Roles.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        private void CompareConnectionRoleAssociatedObjects(string prefix, List<string> diff, IEnumerable<string> enumerable1, IEnumerable<string> enumerable2)
        {
            var tableOnlyIn1 = enumerable1.Except(enumerable2).OrderBy(s => s);
            var tableOnlyIn2 = enumerable2.Except(enumerable1).OrderBy(s => s);

            if (tableOnlyIn1.Any())
            {
                if (diff.Count > 0) { diff.Add(string.Empty); }

                diff.Add(string.Format("{0} ONLY in {1}: {2}", prefix, Connection1.Name, tableOnlyIn1.Count()));

                foreach (var item in tableOnlyIn1)
                {
                    diff.Add(tabSpacer + item);
                }
            }

            if (tableOnlyIn2.Any())
            {
                if (diff.Count > 0) { diff.Add(string.Empty); }

                diff.Add(string.Format("{0} ONLY in {1}: {2}", prefix, Connection2.Name, tableOnlyIn2.Count()));

                foreach (var item in tableOnlyIn2)
                {
                    diff.Add(tabSpacer + item);
                }
            }
        }

        public Task<string> CheckConnectionRoleCategoriesAsync()
        {
            return Task.Run(() => CheckConnectionRoleCategories());
        }

        private async Task<string> CheckConnectionRoleCategories()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Connection Role Categories started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            const string optionSetName = "connectionrole_category";

            var request = new RetrieveOptionSetRequest()
            {
                Name = optionSetName,
            };

            var response1 = (RetrieveOptionSetResponse)_service1.Execute(request);
            var response2 = (RetrieveOptionSetResponse)_service2.Execute(request);

            var optionSet1 = (OptionSetMetadata)response1.OptionSetMetadata;
            var optionSet2 = (OptionSetMetadata)response2.OptionSetMetadata;

            if (optionSet1 == null && optionSet2 == null)
            {
                content.AppendFormat("In organizations NOT EXISTS Global OptionSet {0}.", optionSetName);
            }
            else if (optionSet1 != null && optionSet2 == null)
            {
                content.AppendFormat("Global OptionSet {0} ONLY EXISTS in {0}.", optionSetName, Connection2.Name);
            }
            else if (optionSet1 == null && optionSet2 != null)
            {
                content.AppendFormat("Global OptionSet {0} ONLY EXISTS in {0}.", optionSetName, Connection1.Name);
            }
            else
            {
                List<string> strDifference = await this._optionSetComparer.GetDifference(optionSet1, optionSet2, "connectionrole", "category");

                if (strDifference.Count > 0)
                {
                    content.AppendLine().AppendLine().AppendFormat("Global OptionSet {0} DIFFERENT in {1} and {2}", optionSetName, Connection1.Name, Connection2.Name);

                    foreach (var str in strDifference)
                    {
                        content.AppendLine().Append((tabSpacer + tabSpacer + str).TrimEnd());
                    }
                }
                else
                {
                    content.AppendLine("No difference in Connection Role Categories.");
                }
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Connection Role Categories ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} Connection Role Category.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }
    }
}
