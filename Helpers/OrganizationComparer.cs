using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
        public ConnectionData Connection1 { get; private set; }

        public ConnectionData Connection2 { get; private set; }

        private string _OrgOrgName = string.Empty;

        private string _folder;

        private const string tabSpacer = "    ";

        private IWriteToOutput _writeToOutput;

        private IOrganizationServiceExtented _service1;
        private IOrganizationServiceExtented _service2;

        private OptionSetComparer _optionSetComparer;

        private DependencyRepository _dependencyRepository1;
        private SolutionComponentDescriptor _descriptor1;

        private DependencyRepository _dependencyRepository2;
        private SolutionComponentDescriptor _descriptor2;

        public OrganizationComparer(ConnectionData connection1, ConnectionData connection2, IWriteToOutput writeToOutput, string folder)
        {
            this.Connection1 = connection1;
            this.Connection2 = connection2;
            this._writeToOutput = writeToOutput;
            this._folder = folder;

            string[] arr = new string[] { connection1.Name, connection2.Name };

            Array.Sort(arr);

            this._OrgOrgName = string.Format("{0} - {1}"
                , arr[0]
                , arr[1]
                );
        }

        public async Task InitializeConnection(StringBuilder content)
        {
            bool service1IsNull = this._service1 == null;
            bool service2IsNull = this._service2 == null;

            {
                var mess1 = "Connection 1 to CRM.";
                var mess2 = Connection1.GetConnectionDescription();

                if (service1IsNull)
                {
                    _writeToOutput.WriteToOutput(mess1);
                    _writeToOutput.WriteToOutput(mess2);
                    _service1 = await QuickConnection.ConnectAsync(Connection1);

                    this._descriptor1 = new SolutionComponentDescriptor(this._writeToOutput, this._service1, false);
                    this._dependencyRepository1 = new DependencyRepository(this._service1);
                }

                var mess3 = string.Format("Current Service Endpoint: {0}", _service1.CurrentServiceEndpoint);

                if (service1IsNull)
                {
                    _writeToOutput.WriteToOutput(mess3);
                }

                if (content != null)
                {
                    content.AppendLine(mess1);
                    content.AppendLine(mess2);
                    content.AppendLine(mess3);
                }
            }

            if (service1IsNull)
            {
                _writeToOutput.WriteToOutput(string.Empty);
            }

            if (content != null)
            {
                content.AppendLine();
            }

            {
                var mess1 = "Connection 2 to CRM.";
                var mess2 = Connection2.GetConnectionDescription();

                if (service2IsNull)
                {
                    _writeToOutput.WriteToOutput(mess1);
                    _writeToOutput.WriteToOutput(mess2);
                    _service2 = await QuickConnection.ConnectAsync(Connection2);

                    this._descriptor2 = new SolutionComponentDescriptor(this._writeToOutput, this._service2, false);
                    this._dependencyRepository2 = new DependencyRepository(this._service2);
                }

                var mess3 = string.Format("Current Service Endpoint: {0}", _service2.CurrentServiceEndpoint);

                if (service2IsNull)
                {
                    _writeToOutput.WriteToOutput(mess3);
                }

                if (content != null)
                {
                    content.AppendLine(mess1);
                    content.AppendLine(mess2);
                    content.AppendLine(mess3);
                }
            }

            if (content != null)
            {
                content.AppendLine();
            }

            if (this._optionSetComparer == null)
            {
                this._optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_service1), new StringMapRepository(_service2));
            }
        }

        public Task<string> CheckGlobalOptionSetsAsync()
        {
            return Task.Run(async () => await CheckGlobalOptionSets());
        }

        private async Task<string> CheckGlobalOptionSets()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Global OptionSets started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var request = new RetrieveAllOptionSetsRequest();

            var response1 = (RetrieveAllOptionSetsResponse)_service1.Execute(request);

            var optionSetMetadata1 = response1.OptionSetMetadata.OfType<OptionSetMetadata>();

            content.AppendLine(_writeToOutput.WriteToOutput("Global OptionSets in {0}: {1}", Connection1.Name, optionSetMetadata1.Count()));

            var response2 = (RetrieveAllOptionSetsResponse)_service2.Execute(request);

            var optionSetMetadata2 = response2.OptionSetMetadata.OfType<OptionSetMetadata>();

            content.AppendLine(_writeToOutput.WriteToOutput("Global OptionSets in {0}: {1}", Connection2.Name, optionSetMetadata2.Count()));

            var optionSetOnlyIn1 = new FormatTextTableHandler();
            optionSetOnlyIn1.SetHeader("Name", "IsCustomOptionSet", "IsManaged");

            var optionSetOnlyIn2 = new FormatTextTableHandler();
            optionSetOnlyIn2.SetHeader("Name", "IsCustomOptionSet", "IsManaged");

            Dictionary<string, List<string>> optionSetDifference = new Dictionary<string, List<string>>();

            foreach (var optionSet1 in optionSetMetadata1.OrderBy(e => e.Name))
            {
                if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet1.Name))
                {
                    continue;
                }

                {
                    var optionSet2 = optionSetMetadata2.FirstOrDefault(e => e.Name == optionSet1.Name);

                    if (optionSet2 != null)
                    {
                        continue;
                    }
                }

                optionSetOnlyIn1.AddLine(optionSet1.Name, optionSet1.IsCustomOptionSet.ToString(), optionSet1.IsManaged.ToString());
            }

            foreach (var optionSet2 in optionSetMetadata2.OrderBy(e => e.Name))
            {
                if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet2.Name))
                {
                    continue;
                }

                {
                    var optionSet1 = optionSetMetadata1.FirstOrDefault(e => e.Name == optionSet2.Name);

                    if (optionSet1 != null)
                    {
                        continue;
                    }
                }

                optionSetOnlyIn2.AddLine(optionSet2.Name, optionSet2.IsCustomOptionSet.ToString(), optionSet2.IsManaged.ToString());
            }

            foreach (var optionSet1 in optionSetMetadata1.OrderBy(e => e.Name))
            {
                if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet1.Name))
                {
                    continue;
                }

                var optionSet2 = optionSetMetadata2.FirstOrDefault(e => e.Name == optionSet1.Name);

                if (optionSet2 == null)
                {
                    continue;
                }

                string entityname1 = null;
                string attributename1 = null;

                {
                    var dependent1 = await _dependencyRepository1.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet1.MetadataId.Value);

                    if (dependent1.Any(e => e.DependentComponentType.Value == (int)ComponentType.Attribute))
                    {
                        var attr = dependent1.FirstOrDefault(e => e.DependentComponentType.Value == (int)ComponentType.Attribute);

                        var attributeMetadata = _descriptor1.GetAttributeMetadata(attr.DependentComponentObjectId.Value);

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
                    var dependent2 = await _dependencyRepository2.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet2.MetadataId.Value);

                    if (dependent2.Any(e => e.DependentComponentType.Value == (int)ComponentType.Attribute))
                    {
                        var attr = dependent2.FirstOrDefault(e => e.DependentComponentType.Value == (int)ComponentType.Attribute);

                        var attributeMetadata = _descriptor2.GetAttributeMetadata(attr.DependentComponentObjectId.Value);

                        if (attributeMetadata != null)
                        {
                            entityname2 = attributeMetadata.EntityLogicalName;
                            attributename2 = attributeMetadata.LogicalName;
                        }
                    }
                }

                List<string> strDifference = await this._optionSetComparer.GetDifference(optionSet1, optionSet2, entityname1, attributename1, entityname2, attributename2);

                if (strDifference.Count > 0)
                {
                    optionSetDifference.Add(optionSet1.Name, strDifference.Select(s => tabSpacer + s).ToList());
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

                content.AppendFormat("Global OptionSets ONLY EXISTS in {0}: {1}", Connection1.Name, optionSetOnlyIn1.Count);

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

                content.AppendFormat("Global OptionSets ONLY EXISTS in {0}: {1}", Connection2.Name, optionSetOnlyIn2.Count);

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

                content.AppendFormat("Global OptionSets DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, optionSetDifference.Count);

                foreach (var item in optionSetDifference.OrderBy(s => s.Key))
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

                content.AppendFormat("Global OptionSets DIFFERENT Details in {0} and {1}: {2}", Connection1.Name, Connection2.Name, optionSetDifference.Count);

                foreach (var item in optionSetDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

                    foreach (var str in item.Value)
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
                content.AppendLine("No difference in Global OptionSets.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Global OptionSets ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} Global OptionSets.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckWebResourcesAsync(bool withDetails)
        {
            return Task.Run(() => CheckWebResources(withDetails));
        }

        private async Task<string> CheckWebResources(bool withDetails)
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            var withDetailsName = withDetails ? " with details" : string.Empty;

            content.AppendLine(_writeToOutput.WriteToOutput("Checking WebResources{0} started at {1}"
                , withDetailsName
                , DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
                ));

            var repository1 = new WebResourceRepository(_service1);
            var repository2 = new WebResourceRepository(_service2);

            var list1 = await repository1.GetListAllWithContentAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("WebResouces in {0}: {1}", Connection1.Name, list1.Count));

            var list2 = await repository2.GetListAllWithContentAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("WebResouces in {0}: {1}", Connection2.Name, list2.Count));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Type", "Name", "Id", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Type", "Name", "Id", "IsManaged");

            FormatTextTableHandler tableEqualByTextNotContent = new FormatTextTableHandler();
            tableEqualByTextNotContent.SetHeader("Type", "Name", "Id");

            FormatTextTableHandler tableNotEqualByText = new FormatTextTableHandler();
            if (withDetails)
            {
                tableNotEqualByText.SetHeader("Type", "Name", "Id", "+Inserts", "(+Length)", "-Deletes", "(-Length)");
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
            tableNotEqualByTextComplexChanges.SetHeader("Type", "Name", "Id", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableNotEqualByTextMirror = new FormatTextTableHandler();
            tableNotEqualByTextMirror.SetHeader("Type", "Name", "Id", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableNotEqualByTextMirrorWithInserts = new FormatTextTableHandler();
            tableNotEqualByTextMirrorWithInserts.SetHeader("Type", "Name", "Id", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableNotEqualByTextMirrorWithDeletes = new FormatTextTableHandler();
            tableNotEqualByTextMirrorWithDeletes.SetHeader("Type", "Name", "Id", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            var commonList = new List<LinkedEntities<WebResource>>();

            {
                var reporter = new ProgressReporter(_writeToOutput, list1.Count, 10, string.Format("Processing {0} WebResources", Connection1.Name));

                foreach (var res1 in list1)
                {
                    reporter.Increase();

                    {
                        var res2 = list2.FirstOrDefault(res => res.Id == res1.Id);

                        if (res2 != null)
                        {
                            var content1 = res1.Content ?? string.Empty;
                            var content2 = res2.Content ?? string.Empty;

                            if (content1 != content2)
                            {
                                commonList.Add(new LinkedEntities<WebResource>(res1, res2));
                            }

                            continue;
                        }
                    }

                    var name1 = res1.Name;

                    var typeName1 = res1.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                    tableOnlyExistsIn1.AddLine(typeName1, name1, res1.Id.ToString(), res1.IsManaged.ToString());
                }
            }

            {
                var reporter = new ProgressReporter(_writeToOutput, list2.Count, 10, string.Format("Processing {0} WebResources", Connection2.Name));

                foreach (var res2 in list2)
                {
                    reporter.Increase();

                    {
                        var res1 = list1.FirstOrDefault(res => res.Id == res2.Id);

                        if (res1 != null)
                        {
                            continue;
                        }
                    }

                    var typeName2 = res2.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                    var name2 = res2.Name;

                    tableOnlyExistsIn2.AddLine(typeName2, name2, res2.Id.ToString(), res2.IsManaged.ToString());
                }
            }

            {
                var reporter = new ProgressReporter(_writeToOutput, commonList.Count, 10, "Processing Common Different WebResources");

                foreach (var res in commonList)
                {
                    reporter.Increase();

                    var name1 = res.Entity1.Name;
                    var type1 = res.Entity1.WebResourceType.Value;

                    var typeName1 = res.Entity1.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                    var content1 = res.Entity1.Content ?? string.Empty;
                    var content2 = res.Entity2.Content ?? string.Empty;

                    if (content1 != content2)
                    {
                        var array1 = Convert.FromBase64String(content1);
                        var array2 = Convert.FromBase64String(content2);

                        var extension = WebResourceRepository.GetTypeMainExtension(type1);

                        var compare = ContentCoparerHelper.CompareByteArrays(extension, array1, array2, withDetails);

                        if (compare.IsEqual)
                        {
                            tableEqualByTextNotContent.AddLine(typeName1, name1, res.Entity1.Id.ToString());
                        }
                        else
                        {
                            if (withDetails)
                            {
                                var values = new string[]
                                {
                                    typeName1, name1, res.Entity1.Id.ToString()
                                    , string.Format("+{0}", compare.Inserts)
                                    , string.Format("(+{0})", compare.InsertLength)
                                    , string.Format("-{0}", compare.Deletes)
                                    , string.Format("(-{0})", compare.DeleteLength)
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

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking WebResources{0} ended at {1}"
                , withDetailsName
                , DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
                ));

            string fileName = string.Format("OrgCompare {0} at {1} WebResources{2}.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                , withDetailsName
                );

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckSiteMapsAsync()
        {
            return Task.Run(async () => await CheckSiteMaps());
        }

        private async Task<string> CheckSiteMaps()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Sitemaps started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new SitemapRepository(_service1);
            var repository2 = new SitemapRepository(_service2);

            var list1 = await repository1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Sitemaps in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Sitemaps in {0}: {1}", Connection2.Name, list2.Count()));

            if (list1.Count != 1)
            {
                content.AppendLine().AppendLine().AppendFormat("{0} has more than 1 Sitemap.", Connection1.Name).AppendLine();
            }

            if (list2.Count != 1)
            {
                content.AppendLine().AppendLine().AppendFormat("{0} has more than 1 Sitemap.", Connection2.Name).AppendLine();
            }

            if (list1.Count == 1 && list2.Count == 1)
            {
                var sitemap1 = list1.FirstOrDefault();
                var sitemap2 = list2.FirstOrDefault();

                string sitemapxml1 = sitemap1.SiteMapXml;
                string sitemapxml2 = sitemap2.SiteMapXml;

                var compare = ContentCoparerHelper.CompareXML(sitemapxml1, sitemapxml2, true);

                if (!compare.IsEqual)
                {
                    content.AppendLine().AppendLine().AppendLine("Sitemaps not equal.");

                    content.AppendFormat("Inserts {0}   InsertLength {1}   Deletes {2}    DeleteLength {3}   {4}"
                           , string.Format("+{0}", compare.Inserts)
                           , string.Format("(+{0})", compare.InsertLength)
                           , string.Format("-{0}", compare.Deletes)
                           , string.Format("(-{0})", compare.DeleteLength)
                           , compare.GetDescription()
                           );
                }
                else
                {
                    content.AppendLine().AppendLine().AppendLine("No difference in Sitemaps.");
                }
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Sitemaps ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} Sitemaps.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckOrganizationsAsync()
        {
            return Task.Run(async () => await CheckOrganizations());
        }

        private async Task<string> CheckOrganizations()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Organizations started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new OrganizationRepository(_service1);
            var repository2 = new OrganizationRepository(_service2);

            var organization1 = await repository1.GetByIdAsync(Connection1.OrganizationId.Value, new ColumnSet(true));

            var organization2 = await repository2.GetByIdAsync(Connection2.OrganizationId.Value, new ColumnSet(true));

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

                foreach (var key in organization1.Attributes.Keys.Select(k => k.ToLower()))
                {
                    if (!fieldsToCompare.Contains(key))
                    {
                        fieldsToCompare.Add(key);
                    }
                }

                foreach (var key in organization2.Attributes.Keys.Select(k => k.ToLower()))
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

                var dict = new Dictionary<string, List<Tuple<string, string>>>();

                foreach (var fieldName in fieldsToCompare.OrderBy(s => s))
                {
                    if (complexFields.Contains(fieldName))
                    {
                        continue;
                    }

                    if (ContentCoparerHelper.IsEntityDifferentInField(organization1, organization2, fieldName))
                    {
                        var str1 = EntityDescriptionHandler.GetAttributeString(organization1, fieldName);
                        var str2 = EntityDescriptionHandler.GetAttributeString(organization2, fieldName);

                        tableDifference.CalculateLineLengths(fieldName, Connection1.Name, str1);
                        tableDifference.CalculateLineLengths(fieldName, Connection2.Name, str2);

                        dict.Add(fieldName, new List<Tuple<string, string>>()
                    {
                        Tuple.Create(Connection1.Name, str1)
                        , Tuple.Create(Connection2.Name, str2)
                    });
                    }
                }

                foreach (var fieldName in complexFields)
                {
                    var xml1 = organization1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    var xml2 = organization2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                    if (!ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        var compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true);

                        string reason = string.Empty;

                        if (compare.IsEqual)
                        {
                            reason = "InCase";
                        }
                        else
                        {
                            reason = compare.GetCompareDescription();
                        }

                        tableDifference.CalculateLineLengths(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, reason));
                        tableDifference.CalculateLineLengths(fieldName, Connection1.Name, xml1);
                        tableDifference.CalculateLineLengths(fieldName, Connection2.Name, xml2);

                        dict.Add(fieldName, new List<Tuple<string, string>>()
                    {
                        Tuple.Create(string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, reason))
                        , Tuple.Create(Connection1.Name, xml1)
                        , Tuple.Create(Connection2.Name, xml2)

                    });
                    }
                }

                if (dict.Count > 0)
                {
                    content.AppendLine().AppendLine().AppendFormat("Organizations DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dict.Count);

                    content.AppendLine();

                    foreach (var key in dict.Keys.OrderBy(s => s))
                    {
                        foreach (var item in dict[key])
                        {
                            content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(key, item.Item1, item.Item2).TrimEnd());
                        }

                        content.AppendLine();
                    }
                }
                else
                {
                    content.AppendLine().AppendLine().AppendLine("No difference in Organizations.");
                }
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Organizations ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} Organizations.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckReportsAsync()
        {
            return Task.Run(() => CheckReports());
        }

        private async Task<string> CheckReports()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Reports started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new ReportRepository(_service1);
            var repository2 = new ReportRepository(_service2);

            var list1 = await repository1.GetListAllForCompareAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Reports in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAllForCompareAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Reports in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("ReportName", "FileName", "ReportType", "Viewable By", "Owner", "IsCustomReport", "IsManaged", "ReportId");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("ReportName", "FileName", "ReportType", "ViewableBy", "Owner", "IsCustomReport", "IsManaged", "ReportId");

            var commonReports = new List<LinkedEntities<Report>>();

            var commonReportsBySignature = new List<LinkedEntities<Report>>();

            var dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            var dictDifferenceBySignature = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (var report1 in list1)
            {
                {
                    var report2 = list2.FirstOrDefault(report => report.Id == report1.Id);

                    if (report2 != null)
                    {
                        commonReports.Add(new LinkedEntities<Report>(report1, report2));

                        continue;
                    }
                    else
                    {
                        if (report1.SignatureDate.HasValue && report1.SignatureId.HasValue && report1.SignatureId.Value != Guid.Empty)
                        {
                            report2 = list2.FirstOrDefault(report =>
                            {
                                return report.Name == report1.Name
                                    && report.FileName == report1.FileName
                                    && report.ReportTypeCode.Value == report1.ReportTypeCode.Value
                                    && report.SignatureId == report1.SignatureId
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
            }

            foreach (var report2 in list2)
            {
                {
                    var report1 = list1.FirstOrDefault(report => report.Id == report2.Id);

                    if (report1 != null)
                    {
                        continue;
                    }

                    if (report1 != null)
                    {
                        continue;
                    }
                    else
                    {
                        if (report2.SignatureDate.HasValue && report2.SignatureId.HasValue)
                        {
                            report1 = list2.FirstOrDefault(report =>
                            {
                                return report.Name == report2.Name
                                    && report.FileName == report2.FileName
                                    && report.ReportTypeCode.Value == report2.ReportTypeCode.Value
                                    && report.SignatureId == report2.SignatureId
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
            }

            content.AppendLine(_writeToOutput.WriteToOutput("Common Reports in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonReports.Count()));

            content.AppendLine(_writeToOutput.WriteToOutput("Common Reports by Signature in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonReportsBySignature.Count()));

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

                content.AppendLine().AppendLine().AppendFormat("Reports ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Reports ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Reports DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader(Connection1.Name, Connection2.Name, "ReportId");

                foreach (var report in dictDifference)
                {
                    tableDifference.CalculateLineLengths(report.Key.Item1, report.Key.Item2, report.Key.Item3);
                }

                foreach (var report in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(report.Key.Item1, report.Key.Item2, report.Key.Item3));

                    foreach (var str in report.Value)
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

                content.AppendLine().AppendLine().AppendFormat("Reports by Signature DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifferenceBySignature.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader(Connection1.Name, Connection2.Name, "ReportId1", "ReportId2");

                foreach (var report in dictDifferenceBySignature)
                {
                    tableDifference.CalculateLineLengths(report.Key.Item1, report.Key.Item2, report.Key.Item3, report.Key.Item4);
                }

                foreach (var report in dictDifferenceBySignature
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(report.Key.Item1, report.Key.Item2, report.Key.Item3, report.Key.Item4));

                    foreach (var str in report.Value)
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
                content.AppendLine("No difference in Reports.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Reports ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} Reports.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        private void FullfillReportDifferences(List<LinkedEntities<Report>> commonReports, Dictionary<Tuple<string, string, string, string>, List<string>> dictDifferenceBySignature)
        {
            foreach (var commonItem in commonReports)
            {
                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        "name"
                        , Report.Schema.Attributes.ispersonal
                        , "iscustomreport"
                        , "isscheduledreport"
                        , "signaturemajorversion"
                        , "signatureminorversion"
                        , "createdinmajorversion"
                        , "signaturelcid"
                        , "languagecode"
                        , "filename"
                        , "bodybinary"
                        , "filesize"
                        , Report.Schema.Attributes.reporttypecode
                        , "description"
                        , "signaturedate"
                        , "bodyurl"
                        , "mimetype"
                    };

                    foreach (var fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(commonItem.Entity1, commonItem.Entity2, fieldName))
                        {
                            if (string.Equals(fieldName, "bodybinary", StringComparison.OrdinalIgnoreCase))
                            {
                                tabDiff.AddLine(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, "Differs"));
                            }
                            else
                            {
                                var str1 = EntityDescriptionHandler.GetAttributeString(commonItem.Entity1, fieldName);
                                var str2 = EntityDescriptionHandler.GetAttributeString(commonItem.Entity2, fieldName);

                                tabDiff.AddLine(fieldName, Connection1.Name, str1);
                                tabDiff.AddLine(fieldName, Connection2.Name, str2);
                            }
                        }
                    }
                }

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        "defaultfilter"
                        , "bodytext"
                        , "customreportxml"
                        , "schedulexml"
                        , "queryinfo"
                        , "originalbodytext"
                    };

                    foreach (var fieldName in fieldsToCompare)
                    {
                        var xml1 = commonItem.Entity1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                        var xml2 = commonItem.Entity2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                        if (!ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                        {
                            string reason = string.Empty;

                            var compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true);

                            if (!compare.IsEqual)
                            {
                                reason = "InCase";
                            }
                            else
                            {
                                reason = compare.GetCompareDescription();
                            }

                            tabDiff.AddLine(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, reason));
                        }
                    }
                }

                if (tabDiff.Count > 0)
                {
                    var name1 = commonItem.Entity1.Name;
                    var name2 = commonItem.Entity2.Name;

                    dictDifferenceBySignature.Add(Tuple.Create(name1, name2, commonItem.Entity1.Id.ToString(), commonItem.Entity2.Id.ToString()), tabDiff.GetFormatedLines(false));
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

            await InitializeConnection(content);

            var withDetailsName = withDetails ? " with details" : string.Empty;

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Workflows{0} started at {1}"
                , withDetailsName
                , DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"))
                );

            var repository1 = new WorkflowRepository(_service1);
            var repository2 = new WorkflowRepository(_service2);

            var translation1 = await TranslationRepository.GetDefaultTranslationFromCacheAsync(Connection1.ConnectionId, _service1);
            var translation2 = await TranslationRepository.GetDefaultTranslationFromCacheAsync(Connection2.ConnectionId, _service2);

            var labelReplacer1 = new LabelReplacer(translation1);
            var labelReplacer2 = new LabelReplacer(translation2);

            var list1 = await repository1.GetListAsync(null, null, null);

            content.AppendLine(_writeToOutput.WriteToOutput("Workflows in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAsync(null, null, null);

            content.AppendLine(_writeToOutput.WriteToOutput("Workflows in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Entity", "Category", "Name", "IsCrmUiWorkflow", "IsManaged", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Entity", "Category", "Name", "IsCrmUiWorkflow", "IsManaged", "Id");

            var commonList = new List<LinkedEntities<Workflow>>();

            var dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (var workflow1 in list1)
            {
                {
                    var workflow2 = list2.FirstOrDefault(workflow => workflow.Id == workflow1.Id);

                    if (workflow2 != null)
                    {
                        commonList.Add(new LinkedEntities<Workflow>(workflow1, workflow2));
                        continue;
                    }
                }

                var entityName1 = workflow1.PrimaryEntity;
                var categoryName1 = workflow1.FormattedValues[Workflow.Schema.Attributes.category];
                var name1 = workflow1.Name;

                tableOnlyExistsIn1.AddLine(
                    entityName1
                    , categoryName1
                    , name1
                    , workflow1.IsCrmUIWorkflow.ToString()
                    , workflow1.IsManaged.ToString()
                    , workflow1.Id.ToString()
                    );
            }

            foreach (var workflow2 in list2)
            {
                {
                    var workflow1 = list1.FirstOrDefault(workflow => workflow.Id == workflow2.Id);

                    if (workflow1 != null)
                    {
                        continue;
                    }
                }

                var entityName2 = workflow2.PrimaryEntity;
                var categoryName2 = workflow2.FormattedValues[Workflow.Schema.Attributes.category];
                var name2 = workflow2.Name;

                tableOnlyExistsIn2.AddLine(
                    entityName2
                    , categoryName2
                    , name2
                    , workflow2.IsCrmUIWorkflow.ToString()
                    , workflow2.IsManaged.ToString()
                    , workflow2.Id.ToString()
                    );
            }

            content.AppendLine(_writeToOutput.WriteToOutput("Common Workflows in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonList.Count()));

            {
                var reporter = new ProgressReporter(_writeToOutput, commonList.Count, 5, "Processing Common Workflows");

                foreach (var workflow in commonList)
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

                        foreach (var fieldName in fieldsToCompare)
                        {
                            if (ContentCoparerHelper.IsEntityDifferentInField(workflow.Entity1, workflow.Entity2, fieldName))
                            {
                                var str1 = EntityDescriptionHandler.GetAttributeString(workflow.Entity1, fieldName);
                                var str2 = EntityDescriptionHandler.GetAttributeString(workflow.Entity2, fieldName);

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

                        foreach (var fieldName in fieldsToCompare)
                        {
                            var xml1 = workflow.Entity1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                            var xml2 = workflow.Entity2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                            if (!ContentCoparerHelper.CompareWorkflowXAML(xml1, xml2, labelReplacer1, labelReplacer2).IsEqual)
                            {
                                string reason = string.Empty;

                                var compare = ContentCoparerHelper.CompareWorkflowXAML(xml1.ToLower(), xml2.ToLower(), labelReplacer1, labelReplacer2, withDetails);

                                if (compare.IsEqual)
                                {
                                    reason = "InCase";
                                }
                                else
                                {
                                    if (withDetails)
                                    {
                                        reason = compare.GetCompareDescription();
                                    }
                                    else
                                    {
                                        reason = "Differes";
                                    }

                                    tabDiff.AddLine(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, reason));
                                }
                            }
                        }
                    }

                    if (tabDiff.Count > 0)
                    {
                        var entityName1 = workflow.Entity1.PrimaryEntity;
                        var categoryName1 = workflow.Entity1.FormattedValues[Workflow.Schema.Attributes.category];
                        var name1 = workflow.Entity1.Name;

                        dictDifference.Add(Tuple.Create(entityName1, categoryName1, name1, workflow.Entity1.Id.ToString()), tabDiff.GetFormatedLines(false));
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

                content.AppendLine().AppendLine().AppendFormat("Workflows ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Workflows ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (dictDifference.Count > 0)
            {
                var ordered = dictDifference
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

                content.AppendLine().AppendLine().AppendFormat("Workflows DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                {
                    var tableDifference = new FormatTextTableHandler();
                    tableDifference.SetHeader("Entity", "Category", "Name", "Id");

                    foreach (var workflow in ordered)
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

                content.AppendLine().AppendLine().AppendFormat("Workflows DIFFERENT information in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                foreach (var workflow in ordered)
                {
                    content.AppendLine().Append(tabSpacer + string.Format("{0} - {1} - {2} - {3}", workflow.Key.Item1, workflow.Key.Item2, workflow.Key.Item3, workflow.Key.Item4));

                    foreach (var str in workflow.Value)
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
                content.AppendLine("No difference in Workflows.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Workflows{0} ended at {1}"
                , withDetailsName
                , DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"))
                );

            string fileName = string.Format("OrgCompare {0} at {1} Workflows{2}.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                , withDetailsName
                );

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }
    }
}
