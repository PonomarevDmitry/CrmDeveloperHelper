using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
        private string _OrgOrgName = string.Empty;

        private string _folder;

        private const string tabSpacer = "    ";

        private IWriteToOutput _iWriteToOutput;

        private IOrganizationComparerSource _comparerSource;

        public ConnectionData Connection1 => _comparerSource.Connection1;

        public ConnectionData Connection2 => _comparerSource.Connection2;

        private OrganizationDifferenceImageBuilder _ImageBuilder;
        private OrganizationDifferenceImageBuilder ImageBuilder
        {
            get
            {
                if (this._ImageBuilder == null)
                {
                    this._ImageBuilder = new OrganizationDifferenceImageBuilder(this._comparerSource.Service1, this._comparerSource.Service2);
                }

                return _ImageBuilder;
            }
        }

        public OrganizationComparer(IOrganizationComparerSource comparerSource, IWriteToOutput writeToOutput, string folder)
        {
            this._comparerSource = comparerSource;

            this._iWriteToOutput = writeToOutput;
            this._folder = folder;

            string[] arr = new string[] { Connection1.Name, Connection2.Name };

            Array.Sort(arr);

            _OrgOrgName = string.Format("{0} - {1}"
                , arr[0]
                , arr[1]
                );
        }

        public Task<string> CheckGlobalOptionSetsAsync()
        {
            return Task.Run(async () => await CheckGlobalOptionSets());
        }

        private async Task<string> CheckGlobalOptionSets()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingGlobalOptionSetsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetOptionSetMetadata1Async();
            var task2 = _comparerSource.GetOptionSetMetadata2Async();

            List<Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata> optionSetMetadata1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.GlobalOptionSetsInConnectionFormat2, Connection1.Name, optionSetMetadata1.Count()));

            List<Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata> optionSetMetadata2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.GlobalOptionSetsInConnectionFormat2, Connection2.Name, optionSetMetadata2.Count()));

            FormatTextTableHandler optionSetOnlyIn1 = new FormatTextTableHandler();
            optionSetOnlyIn1.SetHeader("Name", "IsCustomOptionSet", "IsManaged");

            FormatTextTableHandler optionSetOnlyIn2 = new FormatTextTableHandler();
            optionSetOnlyIn2.SetHeader("Name", "IsCustomOptionSet", "IsManaged");

            Dictionary<string, List<string>> optionSetDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            DependencyRepository dependencyRepository1 = new DependencyRepository(_comparerSource.Service1);
            DependencyRepository dependencyRepository2 = new DependencyRepository(_comparerSource.Service2);

            OptionSetComparer optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_comparerSource.Service1), new StringMapRepository(_comparerSource.Service2));

            foreach (Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata optionSet1 in optionSetMetadata1.OrderBy(e => e.Name))
            {
                if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet1.Name))
                {
                    continue;
                }

                {
                    Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata optionSet2 = optionSetMetadata2.FirstOrDefault(e => string.Equals(e.Name, optionSet1.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (optionSet2 != null)
                    {
                        continue;
                    }
                }

                optionSetOnlyIn1.AddLine(optionSet1.Name, optionSet1.IsCustomOptionSet.ToString(), optionSet1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.OptionSet, optionSet1.MetadataId.Value);
            }

            foreach (Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata optionSet2 in optionSetMetadata2.OrderBy(e => e.Name))
            {
                if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet2.Name))
                {
                    continue;
                }

                {
                    Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata optionSet1 = optionSetMetadata1.FirstOrDefault(e => string.Equals(e.Name, optionSet2.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (optionSet1 != null)
                    {
                        continue;
                    }
                }

                optionSetOnlyIn2.AddLine(optionSet2.Name, optionSet2.IsCustomOptionSet.ToString(), optionSet2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.OptionSet, optionSet2.MetadataId.Value);
            }

            foreach (Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata optionSet1 in optionSetMetadata1.OrderBy(e => e.Name))
            {
                if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet1.Name))
                {
                    continue;
                }

                Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata optionSet2 = optionSetMetadata2.FirstOrDefault(e => string.Equals(e.Name, optionSet1.Name, StringComparison.InvariantCultureIgnoreCase));

                if (optionSet2 == null)
                {
                    continue;
                }

                string entityname1 = null;
                string attributename1 = null;

                {
                    List<Dependency> dependent1 = await dependencyRepository1.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet1.MetadataId.Value);

                    if (dependent1.Any(e => e.DependentComponentType.Value == (int)ComponentType.Attribute))
                    {
                        Dependency attr = dependent1.FirstOrDefault(e => e.DependentComponentType.Value == (int)ComponentType.Attribute);

                        Microsoft.Xrm.Sdk.Metadata.AttributeMetadata attributeMetadata = ImageBuilder.Descriptor1.MetadataSource.GetAttributeMetadata(attr.DependentComponentObjectId.Value);

                        if (attributeMetadata != null)
                        {
                            entityname1 = attributeMetadata.EntityLogicalName;
                            attributename1 = attributeMetadata.LogicalName;
                        }
                    }
                }

                string entityname2 = null;
                string attributename2 = null;

                {
                    List<Dependency> dependent2 = await dependencyRepository2.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet2.MetadataId.Value);

                    if (dependent2.Any(e => e.DependentComponentType.Value == (int)ComponentType.Attribute))
                    {
                        Dependency attr = dependent2.FirstOrDefault(e => e.DependentComponentType.Value == (int)ComponentType.Attribute);

                        Microsoft.Xrm.Sdk.Metadata.AttributeMetadata attributeMetadata = ImageBuilder.Descriptor2.MetadataSource.GetAttributeMetadata(attr.DependentComponentObjectId.Value);

                        if (attributeMetadata != null)
                        {
                            entityname2 = attributeMetadata.EntityLogicalName;
                            attributename2 = attributeMetadata.LogicalName;
                        }
                    }
                }

                List<string> strDifference = await optionSetComparer.GetDifference(optionSet1, optionSet2, entityname1, attributename1, entityname2, attributename2);

                if (strDifference.Count > 0)
                {
                    optionSetDifference.Add(optionSet1.Name, strDifference.Select(s => tabSpacer + s).ToList());

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.OptionSet, optionSet1.MetadataId.Value, optionSet2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                }
            }

            if (optionSetOnlyIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.GlobalOptionSetsOnlyExistsInConnectionFormat2, Connection1.Name, optionSetOnlyIn1.Count);

                optionSetOnlyIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));

                content
                    .AppendLine()
                    .AppendLine();
            }

            if (optionSetOnlyIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.GlobalOptionSetsOnlyExistsInConnectionFormat2, Connection2.Name, optionSetOnlyIn2.Count);

                optionSetOnlyIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));

                content
                    .AppendLine()
                    .AppendLine();
            }

            if (optionSetDifference.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat(Properties.OrganizationComparerStrings.GlobalOptionSetsDifferentFormat3, Connection1.Name, Connection2.Name, optionSetDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in optionSetDifference.OrderBy(s => s.Key))
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

                content.AppendFormat(Properties.OrganizationComparerStrings.GlobalOptionSetsDifferentDetailsFormat3, Connection1.Name, Connection2.Name, optionSetDifference.Count);

                foreach (KeyValuePair<string, List<string>> item in optionSetDifference.OrderBy(s => s.Key))
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

            if (optionSetOnlyIn2.Count == 0
                && optionSetOnlyIn1.Count == 0
                && optionSetDifference.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.GlobalOptionSetsNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.GlobalOptionSetsFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckWebResourcesAsync(bool withDetails)
        {
            return Task.Run(async () => await CheckWebResources(withDetails));
        }

        private async Task<string> CheckWebResources(bool withDetails)
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingWebResourcesFormat2, Connection1.Name, Connection2.Name);

            if (withDetails)
            {
                operation = string.Format(Properties.OperationNames.CheckingWebResourcesWithDetailsFormat2, Connection1.Name, Connection2.Name);
            }

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetWebResource1Async();
            var task2 = _comparerSource.GetWebResource2Async();

            List<WebResource> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("WebResouces in {0}: {1}", Connection1.Name, list1.Count));

            List<WebResource> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("WebResouces in {0}: {1}", Connection2.Name, list2.Count));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Type", "Name", "Id", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Type", "Name", "Id", "IsManaged");

            FormatTextTableHandler tableEqualByTextNotContent = new FormatTextTableHandler();
            tableEqualByTextNotContent.SetHeader("Type", "Name", "Id");

            FormatTextTableHandler tableNotEqualByText = new FormatTextTableHandler();
            if (withDetails)
            {
                tableNotEqualByText.SetHeader("Type", "Name", "Id", "-Deletes", "(-Length)", "+Inserts", "(+Length)");
            }
            else
            {
                tableNotEqualByText.SetHeader("Type", "Name", "Id");
            }

            FormatTextTableHandler tableNotEqualByTextOnlyInserts = new FormatTextTableHandler();
            tableNotEqualByTextOnlyInserts.SetHeader("Type", "Name", "Id", "+Inserts", "(+Length)");

            FormatTextTableHandler tableNotEqualByTextOnlyDeletes = new FormatTextTableHandler();
            tableNotEqualByTextOnlyDeletes.SetHeader("Type", "Name", "Id", "-Deletes", "(-Length)");

            FormatTextTableHandler tableNotEqualByTextComplexChanges = new FormatTextTableHandler();
            tableNotEqualByTextComplexChanges.SetHeader("Type", "Name", "Id", "-Deletes", "(-Length)", "+Inserts", "(+Length)");

            FormatTextTableHandler tableNotEqualByTextMirror = new FormatTextTableHandler();
            tableNotEqualByTextMirror.SetHeader("Type", "Name", "Id", "-Deletes", "(-Length)", "+Inserts", "(+Length)");

            FormatTextTableHandler tableNotEqualByTextMirrorWithInserts = new FormatTextTableHandler();
            tableNotEqualByTextMirrorWithInserts.SetHeader("Type", "Name", "Id", "-Deletes", "(-Length)", "+Inserts", "(+Length)");

            FormatTextTableHandler tableNotEqualByTextMirrorWithDeletes = new FormatTextTableHandler();
            tableNotEqualByTextMirrorWithDeletes.SetHeader("Type", "Name", "Id", "-Deletes", "(-Length)", "+Inserts", "(+Length)");

            List<LinkedEntities<WebResource>> commonList = new List<LinkedEntities<WebResource>>();

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, list1.Count, 10, string.Format("Processing {0} WebResources", Connection1.Name));

                foreach (WebResource res1 in list1)
                {
                    reporter.Increase();

                    {
                        WebResource res2 = list2.FirstOrDefault(res => string.Equals(res.Name, res1.Name, StringComparison.InvariantCultureIgnoreCase));

                        if (res2 != null)
                        {
                            string content1 = res1.Content ?? string.Empty;
                            string content2 = res2.Content ?? string.Empty;

                            if (content1 != content2)
                            {
                                commonList.Add(new LinkedEntities<WebResource>(res1, res2));
                            }

                            continue;
                        }
                    }

                    string name1 = res1.Name;

                    string typeName1 = res1.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                    tableOnlyExistsIn1.AddLine(typeName1, name1, res1.Id.ToString(), res1.IsManaged.ToString());

                    this.ImageBuilder.AddComponentSolution1((int)ComponentType.WebResource, res1.Id);
                }
            }

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, list2.Count, 10, string.Format("Processing {0} WebResources", Connection2.Name));

                foreach (WebResource res2 in list2)
                {
                    reporter.Increase();

                    {
                        WebResource res1 = list1.FirstOrDefault(res => string.Equals(res.Name, res2.Name, StringComparison.InvariantCultureIgnoreCase));

                        if (res1 != null)
                        {
                            continue;
                        }
                    }

                    string typeName2 = res2.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                    string name2 = res2.Name;

                    tableOnlyExistsIn2.AddLine(typeName2, name2, res2.Id.ToString(), res2.IsManaged.ToString());

                    this.ImageBuilder.AddComponentSolution2((int)ComponentType.WebResource, res2.Id);
                }
            }

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, commonList.Count, 10, "Processing Common Different WebResources");

                foreach (LinkedEntities<WebResource> res in commonList)
                {
                    reporter.Increase();

                    string name1 = res.Entity1.Name;
                    int type1 = res.Entity1.WebResourceType.Value;

                    string typeName1 = res.Entity1.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                    string content1 = res.Entity1.Content ?? string.Empty;
                    string content2 = res.Entity2.Content ?? string.Empty;

                    if (content1 != content2)
                    {
                        byte[] array1 = Convert.FromBase64String(content1);
                        byte[] array2 = Convert.FromBase64String(content2);

                        string extension = WebResourceRepository.GetTypeMainExtension(type1);

                        ContentCopareResult compare = ContentCoparerHelper.CompareByteArrays(extension, array1, array2, withDetails);

                        if (compare.IsEqual)
                        {
                            tableEqualByTextNotContent.AddLine(typeName1, name1, res.Entity1.Id.ToString());
                        }
                        else
                        {
                            this.ImageBuilder.AddComponentDifferent((int)ComponentType.WebResource, res.Entity1.Id, res.Entity2.Id);

                            if (withDetails)
                            {
                                string[] values = new string[]
                                {
                                    typeName1, name1, res.Entity1.Id.ToString()
                                    , string.Format("-{0}", compare.Deletes)
                                    , string.Format("(-{0})", compare.DeleteLength)
                                    , string.Format("+{0}", compare.Inserts)
                                    , string.Format("(+{0})", compare.InsertLength)
                                };

                                tableNotEqualByText.AddLine(values);

                                if (compare.IsOnlyInserts)
                                {
                                    tableNotEqualByTextOnlyInserts.AddLine(typeName1, name1, res.Entity1.Id.ToString()
                                        , string.Format("+{0}", compare.Inserts)
                                        , string.Format("(+{0})", compare.InsertLength)
                                        );
                                }

                                if (compare.IsOnlyDeletes)
                                {
                                    tableNotEqualByTextOnlyDeletes.AddLine(typeName1, name1, res.Entity1.Id.ToString()
                                        , string.Format("-{0}", compare.Deletes)
                                        , string.Format("(-{0})", compare.DeleteLength)
                                        );
                                }

                                if (compare.IsComplexChanges)
                                {
                                    tableNotEqualByTextComplexChanges.AddLine(values);
                                }

                                if (compare.IsMirror)
                                {
                                    tableNotEqualByTextMirror.AddLine(values);
                                }

                                if (compare.IsMirrorWithInserts)
                                {
                                    tableNotEqualByTextMirrorWithInserts.AddLine(values);
                                }

                                if (compare.IsMirrorWithDeletes)
                                {
                                    tableNotEqualByTextMirrorWithDeletes.AddLine(values);
                                }
                            }
                            else
                            {
                                tableNotEqualByText.AddLine(typeName1, name1);
                            }
                        }
                    }
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

                content.AppendLine().AppendLine().AppendFormat("WebResources ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("WebResources ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByText.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL BY TEXT AND CONTENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByText.Count);

                tableNotEqualByText.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByTextOnlyInserts.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL WITH ONLY INSERTS in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByTextOnlyInserts.Count);

                tableNotEqualByTextOnlyInserts.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByTextOnlyDeletes.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL WITH ONLY DELETES in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByTextOnlyDeletes.Count);

                tableNotEqualByTextOnlyDeletes.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByTextComplexChanges.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL BY TEXT AND CONTENT WITH COMPLEX CHANGES in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByTextComplexChanges.Count);

                tableNotEqualByTextComplexChanges.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByTextMirror.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL BY TEXT AND CONTENT WITH MIRROR CHANGES in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByTextMirror.Count);

                tableNotEqualByTextMirror.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByTextMirrorWithInserts.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL BY TEXT AND CONTENT WITH MIRROR CHANGES AND INSERTS in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByTextMirrorWithInserts.Count);

                tableNotEqualByTextMirrorWithInserts.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableNotEqualByTextMirrorWithDeletes.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources NOT EQUAL BY TEXT AND CONTENT WITH MIRROR CHANGES AND DELETES in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableNotEqualByTextMirrorWithDeletes.Count);

                tableNotEqualByTextMirrorWithDeletes.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableEqualByTextNotContent.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("WebResources EQUAL BY TEXT, NOT CONTENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, tableEqualByTextNotContent.Count);

                tableEqualByTextNotContent.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableOnlyExistsIn2.Count == 0
                && tableOnlyExistsIn1.Count == 0
                && tableNotEqualByText.Count == 0
                )
            {
                content.AppendLine("No difference in WebResources.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, string.Format("WebResources{0}", withDetails ? " with details" : string.Empty));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private static List<string> _fieldsToCompareSiteMapOrdinal = new List<string>()
        {
            SiteMap.Schema.Attributes.sitemapname
            //, SiteMap.Schema.Attributes.sitemapnameunique
            , SiteMap.Schema.Attributes.isappaware
        };

        private static List<string> _fieldsToCompareSiteMapXml = new List<string>()
        {
            SiteMap.Schema.Attributes.sitemapxml
        };

        public Task<string> CheckSiteMapsAsync()
        {
            return Task.Run(async () => await CheckSiteMaps());
        }

        private async Task<string> CheckSiteMaps()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            var task1 = _comparerSource.GetSiteMap1Async();
            var task2 = _comparerSource.GetSiteMap2Async();

            string operation = string.Format(Properties.OperationNames.CheckingSitemapsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            List<SiteMap> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.SiteMapsInConnectionFormat2, Connection1.Name, list1.Count()));

            List<SiteMap> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.SiteMapsInConnectionFormat2, Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Id", "SiteMapName", "SiteMapNameUnique", "IsAppAware");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Id", "SiteMapName", "SiteMapNameUnique", "IsAppAware");

            var dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (SiteMap sitemap1 in list1)
            {
                {
                    SiteMap sitemap2 = list2.FirstOrDefault(sitemap => string.Equals(sitemap.SiteMapNameUnique ?? string.Empty, sitemap1.SiteMapNameUnique ?? string.Empty));

                    if (sitemap2 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn1.AddLine(sitemap1.Id.ToString(), sitemap1.SiteMapName, sitemap1.SiteMapNameUnique, sitemap1.IsAppAware.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.SiteMap, sitemap1.Id);
            }

            foreach (SiteMap sitemap2 in list2)
            {
                {
                    SiteMap sitemap1 = list1.FirstOrDefault(sitemap => string.Equals(sitemap.SiteMapNameUnique ?? string.Empty, sitemap2.SiteMapNameUnique ?? string.Empty));

                    if (sitemap1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(sitemap2.Id.ToString(), sitemap2.SiteMapName, sitemap2.SiteMapNameUnique, sitemap2.IsAppAware.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.SiteMap, sitemap2.Id);
            }

            foreach (SiteMap sitemap1 in list1)
            {
                SiteMap sitemap2 = list2.FirstOrDefault(sitemap => string.Equals(sitemap.SiteMapNameUnique ?? string.Empty, sitemap1.SiteMapNameUnique ?? string.Empty));

                if (sitemap2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                foreach (string fieldName in _fieldsToCompareSiteMapOrdinal)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(sitemap1, sitemap2, fieldName))
                    {
                        string str1 = EntityDescriptionHandler.GetAttributeString(sitemap1, fieldName, Connection1);
                        string str2 = EntityDescriptionHandler.GetAttributeString(sitemap2, fieldName, Connection2);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                foreach (string fieldName in _fieldsToCompareSiteMapXml)
                {
                    string xml1 = sitemap1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    string xml2 = sitemap2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                    if (!ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string reason = string.Empty;

                        ContentCopareResult compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true);

                        if (!compare.IsEqual)
                        {
                            reason = Properties.OrganizationComparerStrings.InCaseDifference;
                        }
                        else
                        {
                            reason = compare.GetCompareDescription();
                        }

                        tabDiff.AddLine(fieldName, string.Empty, string.Format(Properties.OrganizationComparerStrings.FieldDifferenceReasonFormat3, Connection1.Name, Connection2.Name, reason));
                    }
                }

                if (tabDiff.Count > 0)
                {
                    var diff = tabDiff.GetFormatedLines(false);
                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.SiteMap, sitemap1.Id, sitemap2.Id, string.Join(Environment.NewLine, diff));

                    dictDifference.Add(Tuple.Create(sitemap1.Id.ToString(), sitemap1.SiteMapName, sitemap1.SiteMapNameUnique, sitemap1.IsAppAware.ToString()), diff);
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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.SiteMapsOnlyExistsInConnectionFormat2, Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.SiteMapsOnlyExistsInConnectionFormat2, Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.SiteMapsDifferentFormat3, Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("AssociatedEntity", "Name", "Language", "Id");

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> sitemap in dictDifference)
                {
                    tableDifference.CalculateLineLengths(sitemap.Key.Item1, sitemap.Key.Item2, sitemap.Key.Item3, sitemap.Key.Item4);
                }

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> sitemap in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(sitemap.Key.Item1, sitemap.Key.Item2, sitemap.Key.Item3, sitemap.Key.Item4));

                    foreach (string str in sitemap.Value)
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
                content.AppendLine(Properties.OrganizationComparerStrings.SiteMapsNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.SiteMapsFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckOrganizationsAsync()
        {
            return Task.Run(async () => await CheckOrganizations());
        }

        private async Task<string> CheckOrganizations()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingOrganizationsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetOrganization1Async(new ColumnSet(true));
            var task2 = _comparerSource.GetOrganization2Async(new ColumnSet(true));

            Organization organization1 = await task1;
            Organization organization2 = await task2;

            if (organization1 == null)
            {
                content.AppendLine().AppendLine().AppendFormat("Not finded organization {0} at {1}.", Connection1.OrganizationId.ToString(), Connection1.Name).AppendLine();
            }

            if (organization2 == null)
            {
                content.AppendLine().AppendLine().AppendFormat("Not finded organization {0} at {1}.", Connection1.OrganizationId.ToString(), Connection1.Name).AppendLine();
            }

            if (organization1 != null && organization2 != null)
            {
                List<string> fieldsToCompare = new List<string>();

                foreach (string key in organization1.Attributes.Keys.Select(k => k.ToLower()))
                {
                    if (!fieldsToCompare.Contains(key))
                    {
                        fieldsToCompare.Add(key);
                    }
                }

                foreach (string key in organization2.Attributes.Keys.Select(k => k.ToLower()))
                {
                    if (!fieldsToCompare.Contains(key))
                    {
                        fieldsToCompare.Add(key);
                    }
                }

                List<string> complexFields = new List<string>()
                {
                    Organization.Schema.Attributes.defaultemailsettings
                    , Organization.Schema.Attributes.defaultthemedata
                    , Organization.Schema.Attributes.featureset
                    , Organization.Schema.Attributes.highcontrastthemedata
                    , Organization.Schema.Attributes.referencesitemapxml
                    , Organization.Schema.Attributes.sitemapxml
                    , Organization.Schema.Attributes.slapausestates
                };

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.CalculateLineLengths("Attribute", "Organization", "Value");

                Dictionary<string, List<Tuple<string, string>>> dict = new Dictionary<string, List<Tuple<string, string>>>(StringComparer.InvariantCultureIgnoreCase);

                foreach (string fieldName in fieldsToCompare.OrderBy(s => s))
                {
                    if (complexFields.Contains(fieldName))
                    {
                        continue;
                    }

                    if (ContentCoparerHelper.IsEntityDifferentInField(organization1, organization2, fieldName))
                    {
                        string str1 = EntityDescriptionHandler.GetAttributeString(organization1, fieldName, Connection1);
                        string str2 = EntityDescriptionHandler.GetAttributeString(organization2, fieldName, Connection2);

                        tableDifference.CalculateLineLengths(fieldName, Connection1.Name, str1);
                        tableDifference.CalculateLineLengths(fieldName, Connection2.Name, str2);

                        dict.Add(fieldName, new List<Tuple<string, string>>()
                    {
                        Tuple.Create(Connection1.Name, str1)
                        , Tuple.Create(Connection2.Name, str2)
                    });
                    }
                }

                foreach (string fieldName in complexFields)
                {
                    string xml1 = organization1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    string xml2 = organization2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                    if (!ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        ContentCopareResult compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true);

                        string reason = string.Empty;

                        if (compare.IsEqual)
                        {
                            reason = Properties.OrganizationComparerStrings.InCaseDifference;
                        }
                        else
                        {
                            reason = compare.GetCompareDescription();
                        }

                        tableDifference.CalculateLineLengths(fieldName, string.Empty, string.Format(Properties.OrganizationComparerStrings.FieldDifferenceReasonFormat3, Connection1.Name, Connection2.Name, reason));
                        tableDifference.CalculateLineLengths(fieldName, Connection1.Name, xml1);
                        tableDifference.CalculateLineLengths(fieldName, Connection2.Name, xml2);

                        dict.Add(fieldName, new List<Tuple<string, string>>()
                        {
                            Tuple.Create(string.Empty, string.Format(Properties.OrganizationComparerStrings.FieldDifferenceReasonFormat3, Connection1.Name, Connection2.Name, reason))
                            , Tuple.Create(Connection1.Name, xml1)
                            , Tuple.Create(Connection2.Name, xml2)

                        });
                    }
                }

                if (dict.Count > 0)
                {
                    content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.OrganizationsDifferentFormat3, Connection1.Name, Connection2.Name, dict.Count);

                    content.AppendLine();

                    foreach (string key in dict.Keys.OrderBy(s => s))
                    {
                        foreach (Tuple<string, string> item in dict[key])
                        {
                            content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(key, item.Item1, item.Item2).TrimEnd());
                        }

                        content.AppendLine();
                    }
                }
                else
                {
                    content.AppendLine().AppendLine().AppendLine(Properties.OrganizationComparerStrings.OrganizationsNoDifference);
                }
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.OrganizationsFileName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> CheckReportsAsync()
        {
            return Task.Run(async () => await CheckReports());
        }

        private async Task<string> CheckReports()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingReportsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetReport1Async();
            var task2 = _comparerSource.GetReport2Async();

            List<Report> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.ReportsInConnectionFormat2, Connection1.Name, list1.Count()));

            List<Report> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.ReportsInConnectionFormat2, Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("ReportName", "FileName", "ReportType", "Viewable By", "Owner", "IsCustomReport", "IsManaged", "ReportId");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("ReportName", "FileName", "ReportType", "ViewableBy", "Owner", "IsCustomReport", "IsManaged", "ReportId");

            List<LinkedEntities<Report>> commonReports = new List<LinkedEntities<Report>>();

            List<LinkedEntities<Report>> commonReportsBySignature = new List<LinkedEntities<Report>>();

            Dictionary<Tuple<string, string, string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            Dictionary<Tuple<string, string, string, string>, List<string>> dictDifferenceBySignature = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (Report report1 in list1)
            {
                {
                    Report report2 = list2.FirstOrDefault(report => report.Id == report1.Id);

                    if (report2 != null)
                    {
                        commonReports.Add(new LinkedEntities<Report>(report1, report2));

                        continue;
                    }
                    else
                    {
                        if (report1.SignatureDate.HasValue
                            && report1.SignatureId.HasValue
                            && report1.SignatureId.Value != Guid.Empty
                            && report1.SignatureLcid.HasValue
                            )
                        {
                            report2 = list2.FirstOrDefault(report =>
                            {
                                return string.Equals(report.Name, report1.Name, StringComparison.InvariantCulture)
                                    && string.Equals(report.FileName, report1.FileName, StringComparison.InvariantCulture)
                                    && report.ReportTypeCode.Value == report1.ReportTypeCode.Value
                                    && report.SignatureId == report1.SignatureId
                                    && report.SignatureLcid == report1.SignatureLcid
                                    && report.SignatureDate == report1.SignatureDate
                                    ;
                            });

                            if (report2 != null)
                            {
                                commonReportsBySignature.Add(new LinkedEntities<Report>(report1, report2));

                                continue;
                            }
                        }
                    }
                }

                string reportTypeName1 = report1.FormattedValues.ContainsKey(Report.Schema.Attributes.reporttypecode) ? report1.FormattedValues[Report.Schema.Attributes.reporttypecode] : string.Empty;

                string owner = string.Empty;

                if (report1.OwnerId != null)
                {
                    owner = report1.OwnerId.Name;
                }

                string ispersonal = report1.FormattedValues.ContainsKey(Report.Schema.Attributes.ispersonal) ? report1.FormattedValues[Report.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn1.AddLine(
                    report1.Name
                    , report1.FileName
                    , reportTypeName1
                    , ispersonal
                    , owner
                    , report1.IsCustomReport.ToString()
                    , report1.IsManaged.ToString()
                    , report1.Id.ToString()
                    );

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Report, report1.Id);
            }

            foreach (Report report2 in list2)
            {
                {
                    Report report1 = list1.FirstOrDefault(report => report.Id == report2.Id);

                    if (report1 != null)
                    {
                        continue;
                    }
                    else
                    {
                        if (report2.SignatureDate.HasValue
                            && report2.SignatureId.HasValue
                            && report2.SignatureId.Value != Guid.Empty
                            && report2.SignatureLcid.HasValue
                            )
                        {
                            report1 = list2.FirstOrDefault(report =>
                            {
                                return string.Equals(report.Name, report2.Name, StringComparison.InvariantCulture)
                                    && string.Equals(report.FileName, report2.FileName, StringComparison.InvariantCulture)
                                    && report.ReportTypeCode.Value == report2.ReportTypeCode.Value
                                    && report.SignatureId == report2.SignatureId
                                    && report.SignatureLcid == report2.SignatureLcid
                                    && report.SignatureDate == report2.SignatureDate
                                    ;
                            });

                            if (report1 != null)
                            {
                                continue;
                            }
                        }
                    }
                }

                string reportType = report2.FormattedValues.ContainsKey(Report.Schema.Attributes.reporttypecode) ? report2.FormattedValues[Report.Schema.Attributes.reporttypecode] : string.Empty;

                string owner = string.Empty;

                if (report2.OwnerId != null)
                {
                    owner = report2.OwnerId.Name;
                }

                string ispersonal = report2.FormattedValues.ContainsKey(Report.Schema.Attributes.ispersonal) ? report2.FormattedValues[Report.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn2.AddLine(
                    report2.Name
                    , report2.FileName
                    , reportType
                    , ispersonal
                    , owner
                    , report2.IsCustomReport.ToString()
                    , report2.IsManaged.ToString()
                    , report2.Id.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Report, report2.Id);
            }


            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.ReportsCommonFormat3, Connection1.Name, Connection2.Name, commonReports.Count()));

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.ReportsCommonBySignatureFormat3, Connection1.Name, Connection2.Name, commonReportsBySignature.Count()));

            FullfillReportDifferences(commonReports, dictDifference);

            FullfillReportDifferences(commonReportsBySignature, dictDifferenceBySignature);

            if (tableOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.ReportsOnlyExistsInConnectionFormat2, Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.ReportsOnlyExistsInConnectionFormat2, Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.ReportsDifferentFormat3, Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader(Connection1.Name, Connection2.Name, "ReportId");

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> report in dictDifference)
                {
                    tableDifference.CalculateLineLengths(report.Key.Item1, report.Key.Item2, report.Key.Item3);
                }

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> report in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(report.Key.Item1, report.Key.Item2, report.Key.Item3));

                    foreach (string str in report.Value)
                    {
                        content.AppendLine().Append(tabSpacer + tabSpacer + str);
                    }
                }
            }

            if (dictDifferenceBySignature.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.ReportsDifferentBySignatureFormat3, Connection1.Name, Connection2.Name, dictDifferenceBySignature.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader(Connection1.Name, Connection2.Name, "ReportId1", "ReportId2");

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> report in dictDifferenceBySignature)
                {
                    tableDifference.CalculateLineLengths(report.Key.Item1, report.Key.Item2, report.Key.Item3, report.Key.Item4);
                }

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> report in dictDifferenceBySignature
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(report.Key.Item1, report.Key.Item2, report.Key.Item3, report.Key.Item4));

                    foreach (string str in report.Value)
                    {
                        content.AppendLine().Append(tabSpacer + tabSpacer + str);
                    }
                }
            }

            if (tableOnlyExistsIn2.Count == 0
                && tableOnlyExistsIn1.Count == 0
                && dictDifference.Count == 0
                && dictDifferenceBySignature.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.ReportsNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Reports");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private void FullfillReportDifferences(List<LinkedEntities<Report>> commonReports, Dictionary<Tuple<string, string, string, string>, List<string>> dictDifferenceBySignature)
        {
            foreach (LinkedEntities<Report> commonItem in commonReports)
            {
                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        Report.Schema.Attributes.name
                        , Report.Schema.Attributes.ispersonal
                        , Report.Schema.Attributes.iscustomreport
                        , Report.Schema.Attributes.isscheduledreport
                        , Report.Schema.Attributes.signaturemajorversion
                        , Report.Schema.Attributes.signatureminorversion
                        , Report.Schema.Attributes.createdinmajorversion
                        , Report.Schema.Attributes.signaturelcid
                        , Report.Schema.Attributes.languagecode
                        , Report.Schema.Attributes.filename
                        , Report.Schema.Attributes.bodybinary
                        , Report.Schema.Attributes.filesize
                        , Report.Schema.Attributes.reporttypecode
                        , Report.Schema.Attributes.description
                        , Report.Schema.Attributes.signaturedate
                        , Report.Schema.Attributes.bodyurl
                        , Report.Schema.Attributes.mimetype
                    };

                    foreach (string fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(commonItem.Entity1, commonItem.Entity2, fieldName))
                        {
                            if (string.Equals(fieldName, Report.Schema.Attributes.bodybinary, StringComparison.OrdinalIgnoreCase))
                            {
                                tabDiff.AddLine(fieldName, string.Empty, string.Format(Properties.OrganizationComparerStrings.FieldDifferenceReasonFormat3, Connection1.Name, Connection2.Name, "Differs"));
                            }
                            else
                            {
                                string str1 = EntityDescriptionHandler.GetAttributeString(commonItem.Entity1, fieldName, Connection1);
                                string str2 = EntityDescriptionHandler.GetAttributeString(commonItem.Entity2, fieldName, Connection2);

                                tabDiff.AddLine(fieldName, Connection1.Name, str1);
                                tabDiff.AddLine(fieldName, Connection2.Name, str2);
                            }
                        }
                    }
                }

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        Report.Schema.Attributes.defaultfilter
                        , Report.Schema.Attributes.bodytext
                        , Report.Schema.Attributes.customreportxml
                        , Report.Schema.Attributes.schedulexml
                        , Report.Schema.Attributes.queryinfo
                        , Report.Schema.Attributes.originalbodytext
                    };

                    foreach (string fieldName in fieldsToCompare)
                    {
                        string xml1 = commonItem.Entity1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                        string xml2 = commonItem.Entity2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                        if (!ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                        {
                            string reason = string.Empty;

                            ContentCopareResult compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true);

                            if (!compare.IsEqual)
                            {
                                reason = "InCase";
                            }
                            else
                            {
                                reason = compare.GetCompareDescription();
                            }

                            tabDiff.AddLine(fieldName, string.Empty, string.Format(Properties.OrganizationComparerStrings.FieldDifferenceReasonFormat3, Connection1.Name, Connection2.Name, reason));
                        }
                    }
                }

                if (tabDiff.Count > 0)
                {
                    string name1 = commonItem.Entity1.Name;
                    string name2 = commonItem.Entity2.Name;

                    var diff = tabDiff.GetFormatedLines(false);
                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.Report, commonItem.Entity1.Id, commonItem.Entity2.Id, string.Join(Environment.NewLine, diff));

                    dictDifferenceBySignature.Add(Tuple.Create(name1, name2, commonItem.Entity1.Id.ToString(), commonItem.Entity2.Id.ToString()), diff);
                }
            }
        }

        public Task<string> CheckWorkflowsAsync(bool withDetails)
        {
            return Task.Run(async () => await CheckWorkflows(withDetails));
        }

        private async Task<string> CheckWorkflows(bool withDetails)
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingWorkflowsFormat2, Connection1.Name, Connection2.Name);

            if (withDetails)
            {
                operation = string.Format(Properties.OperationNames.CheckingWorkflowsWithDetailsFormat2, Connection1.Name, Connection2.Name);
            }

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetWorkflow1Async();
            var task2 = _comparerSource.GetWorkflow2Async();

            var taskTranslation1 = TranslationRepository.GetDefaultTranslationFromCacheAsync(Connection1.ConnectionId, _comparerSource.Service1);
            var taskTranslation2 = TranslationRepository.GetDefaultTranslationFromCacheAsync(Connection2.ConnectionId, _comparerSource.Service2);

            Translation translation1 = await taskTranslation1;
            Translation translation2 = await taskTranslation2;

            LabelReplacer labelReplacer1 = new LabelReplacer(translation1);
            LabelReplacer labelReplacer2 = new LabelReplacer(translation2);

            List<Workflow> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.WorkflowsInConnectionFormat2, Connection1.Name, list1.Count()));

            List<Workflow> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.WorkflowsInConnectionFormat2, Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Entity", "Category", "Name", "IsCrmUiWorkflow", "IsManaged", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Entity", "Category", "Name", "IsCrmUiWorkflow", "IsManaged", "Id");

            List<LinkedEntities<Workflow>> commonList = new List<LinkedEntities<Workflow>>();

            Dictionary<Tuple<string, string, string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (Workflow workflow1 in list1)
            {
                {
                    Workflow workflow2 = list2.FirstOrDefault(workflow => workflow.Id == workflow1.Id);

                    if (workflow2 != null)
                    {
                        commonList.Add(new LinkedEntities<Workflow>(workflow1, workflow2));
                        continue;
                    }
                }

                string entityName1 = workflow1.PrimaryEntity;
                string categoryName1 = workflow1.FormattedValues[Workflow.Schema.Attributes.category];
                string name1 = workflow1.Name;

                tableOnlyExistsIn1.AddLine(
                    entityName1
                    , categoryName1
                    , name1
                    , workflow1.IsCrmUIWorkflow.ToString()
                    , workflow1.IsManaged.ToString()
                    , workflow1.Id.ToString()
                    );

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.Workflow, workflow1.Id);
            }

            foreach (Workflow workflow2 in list2)
            {
                {
                    Workflow workflow1 = list1.FirstOrDefault(workflow => workflow.Id == workflow2.Id);

                    if (workflow1 != null)
                    {
                        continue;
                    }
                }

                string entityName2 = workflow2.PrimaryEntity;
                string categoryName2 = workflow2.FormattedValues[Workflow.Schema.Attributes.category];
                string name2 = workflow2.Name;

                tableOnlyExistsIn2.AddLine(
                    entityName2
                    , categoryName2
                    , name2
                    , workflow2.IsCrmUIWorkflow.ToString()
                    , workflow2.IsManaged.ToString()
                    , workflow2.Id.ToString()
                    );

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.Workflow, workflow2.Id);
            }

            content.AppendLine(_iWriteToOutput.WriteToOutput(Properties.OrganizationComparerStrings.WorkflowsCommonFormat3, Connection1.Name, Connection2.Name, commonList.Count()));

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, commonList.Count, 5, Properties.OrganizationComparerStrings.WorkflowsProcessingCommon);

                foreach (LinkedEntities<Workflow> workflow in commonList)
                {
                    reporter.Increase();

                    FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                    tabDiff.SetHeader("Attribute", "Organization", "Value");

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            Workflow.Schema.Attributes.asyncautodelete
                            //, Workflow.Schema.Attributes.businessprocesstype
                            , Workflow.Schema.Attributes.category
                            , Workflow.Schema.Attributes.createstage
                            , Workflow.Schema.Attributes.updatestage
                            , Workflow.Schema.Attributes.deletestage
                            , Workflow.Schema.Attributes.iscrmuiworkflow
                            , Workflow.Schema.Attributes.iscustomizable
                            //, Workflow.Schema.Attributes.ismanaged
                            , Workflow.Schema.Attributes.istransacted
                            , Workflow.Schema.Attributes.languagecode
                            , Workflow.Schema.Attributes.name
                            , Workflow.Schema.Attributes.mode
                            , Workflow.Schema.Attributes.ondemand
                            , Workflow.Schema.Attributes.primaryentity
                            , Workflow.Schema.Attributes.processorder
                            , Workflow.Schema.Attributes.rank
                            , Workflow.Schema.Attributes.runas
                            , Workflow.Schema.Attributes.scope
                            , Workflow.Schema.Attributes.statecode
                            , Workflow.Schema.Attributes.statuscode
                            , Workflow.Schema.Attributes.subprocess
                            , Workflow.Schema.Attributes.syncworkflowlogonfailure
                            , Workflow.Schema.Attributes.triggeroncreate
                            , Workflow.Schema.Attributes.triggeronupdateattributelist
                            , Workflow.Schema.Attributes.triggerondelete
                            , Workflow.Schema.Attributes.type
                        };

                        foreach (string fieldName in fieldsToCompare)
                        {
                            if (ContentCoparerHelper.IsEntityDifferentInField(workflow.Entity1, workflow.Entity2, fieldName))
                            {
                                string str1 = EntityDescriptionHandler.GetAttributeString(workflow.Entity1, fieldName, Connection1);
                                string str2 = EntityDescriptionHandler.GetAttributeString(workflow.Entity2, fieldName, Connection2);

                                tabDiff.AddLine(fieldName, Connection1.Name, str1);
                                tabDiff.AddLine(fieldName, Connection2.Name, str2);
                            }
                        }
                    }

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            Workflow.Schema.Attributes.xaml
                            , Workflow.Schema.Attributes.inputparameters
                            , Workflow.Schema.Attributes.clientdata
                        };

                        foreach (string fieldName in fieldsToCompare)
                        {
                            string xml1 = workflow.Entity1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                            string xml2 = workflow.Entity2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                            if (!ContentCoparerHelper.CompareWorkflowXAML(xml1, xml2, labelReplacer1, labelReplacer2).IsEqual)
                            {
                                string reason = string.Empty;

                                ContentCopareResult compare = ContentCoparerHelper.CompareWorkflowXAML(xml1.ToLower(), xml2.ToLower(), labelReplacer1, labelReplacer2, withDetails);

                                if (compare.IsEqual)
                                {
                                    reason = Properties.OrganizationComparerStrings.InCaseDifference;
                                }
                                else
                                {
                                    if (withDetails)
                                    {
                                        reason = compare.GetCompareDescription();
                                    }
                                    else
                                    {
                                        reason = Properties.OrganizationComparerStrings.FullDifference;
                                    }

                                    tabDiff.AddLine(fieldName, string.Empty, string.Format(Properties.OrganizationComparerStrings.FieldDifferenceReasonFormat3, Connection1.Name, Connection2.Name, reason));
                                }
                            }
                        }
                    }

                    if (tabDiff.Count > 0)
                    {
                        string entityName1 = workflow.Entity1.PrimaryEntity;
                        string categoryName1 = workflow.Entity1.FormattedValues[Workflow.Schema.Attributes.category];
                        string name1 = workflow.Entity1.Name;

                        var diff = tabDiff.GetFormatedLines(false);
                        this.ImageBuilder.AddComponentDifferent((int)ComponentType.Workflow, workflow.Entity1.Id, workflow.Entity2.Id, string.Join(Environment.NewLine, diff));

                        dictDifference.Add(Tuple.Create(entityName1, categoryName1, name1, workflow.Entity1.Id.ToString()), diff);
                    }
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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.WorkflowsOnlyExistsInConnectionFormat2, Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.WorkflowsOnlyExistsInConnectionFormat2, Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (dictDifference.Count > 0)
            {
                IOrderedEnumerable<KeyValuePair<Tuple<string, string, string, string>, List<string>>> ordered = dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4);

                content
                      .AppendLine()
                      .AppendLine()
                      .AppendLine()
                      .AppendLine(new string('-', 150))
                      .AppendLine()
                      .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.WorkflowsDifferentFormat3, Connection1.Name, Connection2.Name, dictDifference.Count);

                {
                    FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                    tableDifference.SetHeader("Entity", "Category", "Name", "Id");

                    foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> workflow in ordered)
                    {
                        tableDifference.AddLine(workflow.Key.Item1, workflow.Key.Item2, workflow.Key.Item3, workflow.Key.Item4);
                    }

                    tableDifference.GetFormatedLines(false).ForEach(s => content.AppendLine().Append(tabSpacer + s));
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.WorkflowsDifferentInformationFormat3, Connection1.Name, Connection2.Name, dictDifference.Count);

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> workflow in ordered)
                {
                    content.AppendLine().Append(tabSpacer + string.Format("{0} - {1} - {2} - {3}", workflow.Key.Item1, workflow.Key.Item2, workflow.Key.Item3, workflow.Key.Item4));

                    foreach (string str in workflow.Value)
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
                content.AppendLine(Properties.OrganizationComparerStrings.WorkflowsNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, Properties.OrganizationComparerStrings.WorkflowsFileName + (withDetails ? " with details" : string.Empty));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private async Task SaveOrganizationDifferenceImage()
        {
            string fileName = EntityFileNameFormatter.GetDifferenceImageFileName(_OrgOrgName);

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            var image = await ImageBuilder.GetImage();

            image.Save(filePath);

            _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedOrganizationDifferenceImageToFileFormat3, Connection1.Name, Connection2.Name, filePath);

            _iWriteToOutput.WriteToOutputFilePathUri(filePath);
        }
    }
}