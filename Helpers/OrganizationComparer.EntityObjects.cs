using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class OrganizationComparer
    {
        public Task<string> CheckSystemFormsAsync()
        {
            return Task.Run(async () => await CheckSystemForms());
        }

        private async Task<string> CheckSystemForms()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking System Forms started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            FormDescriptionHandler handler1 = new FormDescriptionHandler(_descriptor1, new DependencyRepository(_service1));
            FormDescriptionHandler handler2 = new FormDescriptionHandler(_descriptor2, new DependencyRepository(_service2));

            var repository1 = new SystemFormRepository(_service1);
            var repository2 = new SystemFormRepository(_service2);

            var list1 = await repository1.GetListAsync(string.Empty);

            content.AppendLine(_writeToOutput.WriteToOutput("System Forms in {0}: {1}", Connection1.Name, list1.Count));

            var list2 = await repository2.GetListAsync(string.Empty);

            content.AppendLine(_writeToOutput.WriteToOutput("System Forms in {0}: {1}", Connection2.Name, list2.Count));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Entity", "Type", "Name", "IsManaged", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Entity", "Type", "Name", "IsManaged", "Id");

            var dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            var commonList = new List<LinkedEntities<SystemForm>>();

            foreach (var form1 in list1)
            {
                {
                    var form2 = list2.FirstOrDefault(form => form.Id == form1.Id);

                    if (form2 != null)
                    {
                        commonList.Add(new LinkedEntities<SystemForm>(form1, form2));
                        continue;
                    }
                }

                var entityName1 = form1.ObjectTypeCode;
                var name1 = form1.Name;

                string typeName1 = form1.FormattedValues[SystemForm.Schema.Attributes.type];

                tableOnlyExistsIn1.AddLine(entityName1, typeName1, name1, form1.IsManaged.ToString(), form1.Id.ToString());
            }

            foreach (var form2 in list2)
            {
                {
                    var form1 = list1.FirstOrDefault(form => form.Id == form2.Id);

                    if (form1 != null)
                    {
                        continue;
                    }
                }

                var entityName2 = form2.ObjectTypeCode;
                var name2 = form2.Name;

                string typeName2 = form2.FormattedValues[SystemForm.Schema.Attributes.type];

                tableOnlyExistsIn2.AddLine(entityName2, typeName2, name2, form2.IsManaged.ToString(), form2.Id.ToString());
            }

            {
                var reporter = new ProgressReporter(_writeToOutput, commonList.Count, 5, "Processing Common Forms");

                foreach (var form in commonList)
                {
                    reporter.Increase();

                    FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                    tabDiff.SetHeader("Attribute", "Organization", "Value");

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            //SystemForm.Schema.Attributes.ancestorformid
                            SystemForm.Schema.Attributes.canbedeleted
                            , SystemForm.Schema.Attributes.componentstate
                            , SystemForm.Schema.Attributes.description
                            , SystemForm.Schema.Attributes.formactivationstate
                            , SystemForm.Schema.Attributes.formpresentation 
                            //, SystemForm.Schema.Attributes.formxml 
                            //, SystemForm.Schema.Attributes.formxmlmanaged 
                            , SystemForm.Schema.Attributes.introducedversion
                            , SystemForm.Schema.Attributes.isairmerged
                            , SystemForm.Schema.Attributes.iscustomizable
                            , SystemForm.Schema.Attributes.isdefault 
                            //, SystemForm.Schema.Attributes.ismanaged  
                            , SystemForm.Schema.Attributes.istabletenabled
                            , SystemForm.Schema.Attributes.name
                            , SystemForm.Schema.Attributes.objecttypecode 
                            //, SystemForm.Schema.Attributes.organizationid 
                            //, SystemForm.Schema.Attributes.overwritetime
                            //, SystemForm.Schema.Attributes.publishedon 
                            //, SystemForm.Schema.Attributes.solutionid 
                            //, SystemForm.Schema.Attributes.supportingsolutionid
                            , SystemForm.Schema.Attributes.type
                            , SystemForm.Schema.Attributes.version
                            , SystemForm.Schema.Attributes.versionnumber
                        };

                        foreach (var fieldName in fieldsToCompare)
                        {
                            if (ContentCoparerHelper.IsEntityDifferentInField(form.Entity1, form.Entity2, fieldName))
                            {
                                var str1 = EntityDescriptionHandler.GetAttributeString(form.Entity1, fieldName, _service1.ConnectionData);
                                var str2 = EntityDescriptionHandler.GetAttributeString(form.Entity2, fieldName, _service2.ConnectionData);

                                tabDiff.AddLine(fieldName, Connection1.Name, str1);
                                tabDiff.AddLine(fieldName, Connection2.Name, str2);
                            }
                        }
                    }

                    var entityName1 = form.Entity1.ObjectTypeCode;
                    var name1 = form.Entity1.Name;

                    string typeName1 = form.Entity1.FormattedValues[SystemForm.Schema.Attributes.type];
                    string typeName2 = form.Entity2.FormattedValues[SystemForm.Schema.Attributes.type];

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            SystemForm.Schema.Attributes.formxml
                        };

                        foreach (var fieldName in fieldsToCompare)
                        {
                            string formXml1 = form.Entity1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                            string formXml2 = form.Entity2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                            if (!ContentCoparerHelper.CompareXML(formXml1, formXml2).IsEqual)
                            {
                                string formReason = string.Empty;
                                string descReason = string.Empty;

                                {
                                    var compare = ContentCoparerHelper.CompareXML(formXml1.ToLower(), formXml2.ToLower(), true);

                                    if (compare.IsEqual)
                                    {
                                        formReason = "InCase";
                                    }
                                    else
                                    {
                                        formReason = compare.GetCompareDescription();
                                    }
                                }

                                if (ContentCoparerHelper.TryParseXml(formXml1, out var doc1) && ContentCoparerHelper.TryParseXml(formXml2, out var doc2))
                                {
                                    string desc1 = await handler1.GetFormDescriptionAsync(doc1, entityName1, form.Entity1.Id, name1, typeName1);
                                    string desc2 = await handler2.GetFormDescriptionAsync(doc2, entityName1, form.Entity2.Id, name1, typeName2);

                                    if (!string.Equals(desc1, desc2))
                                    {
                                        var compare = ContentCoparerHelper.CompareText(desc1.ToLower(), desc2.ToLower());

                                        if (compare.IsEqual)
                                        {
                                            descReason = "InCase";
                                        }
                                        else
                                        {
                                            descReason = compare.GetCompareDescription();
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(formReason))
                                {
                                    tabDiff.AddLine(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, formReason));
                                }

                                if (!string.IsNullOrEmpty(descReason))
                                {
                                    tabDiff.AddLine(fieldName + "Description", string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, descReason));
                                }
                            }
                        }
                    }

                    if (tabDiff.Count > 0)
                    {
                        dictDifference.Add(Tuple.Create(entityName1, typeName1, name1, form.Entity1.Id.ToString()), tabDiff.GetFormatedLines(false));
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

                content.AppendLine().AppendLine().AppendFormat("System Forms ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("System Forms ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("System Forms DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("Entity", "Type", "Name", "Id");

                foreach (var template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2, template.Key.Item3, template.Key.Item4);
                }

                foreach (var template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2, template.Key.Item3, template.Key.Item4));

                    foreach (var str in template.Value)
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
                content.AppendLine("No difference in System Forms.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput(
                "Checking System Forms ended at {0}"
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                ));

            string fileName = string.Format("OrgCompare {0} at {1} System Forms.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> CheckSystemSavedQueriesAsync()
        {
            return Task.Run(async () => await CheckSystemSavedQueries());
        }

        private async Task<string> CheckSystemSavedQueries()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking System Saved Queries started at {0}"
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                ));

            var repository1 = new SavedQueryRepository(_service1);
            var repository2 = new SavedQueryRepository(_service2);

            var list1 = await repository1.GetListCustomableAsync(string.Empty);

            content.AppendLine(_writeToOutput.WriteToOutput("System Saved Queries in {0}: {1}", Connection1.Name, list1.Count));

            var list2 = await repository2.GetListCustomableAsync(string.Empty);

            content.AppendLine(_writeToOutput.WriteToOutput("System Saved Queries in {0}: {1}", Connection2.Name, list2.Count));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Entity", "Name", "QueryType", "IsUserDefined", "IsManaged", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Entity", "Name", "QueryType", "IsUserDefined", "IsManaged", "Id");

            var dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            var commonList = new List<LinkedEntities<SavedQuery>>();

            foreach (var query1 in list1)
            {
                {
                    var query2 = list2.FirstOrDefault(query => query.Id == query1.Id);

                    if (query2 != null)
                    {
                        commonList.Add(new LinkedEntities<SavedQuery>(query1, query2));
                        continue;
                    }
                }

                var entityName1 = query1.ReturnedTypeCode;
                var name1 = query1.Name;
                var querytype1 = query1.QueryType.Value;

                var querytypeName1 = SavedQueryRepository.GetQueryTypeName(querytype1);

                tableOnlyExistsIn1.AddLine(
                    entityName1
                    , name1
                    , querytypeName1
                    , query1.IsUserDefined.ToString()
                    , query1.IsManaged.ToString()
                    , query1.Id.ToString());
            }

            foreach (var query2 in list2)
            {
                {
                    var query1 = list1.FirstOrDefault(query => query.Id == query2.Id);

                    if (query1 != null)
                    {
                        continue;
                    }
                }

                var entityName2 = query2.ReturnedTypeCode;
                var name2 = query2.Name;
                var querytype2 = query2.QueryType.Value;

                var querytypeName2 = SavedQueryRepository.GetQueryTypeName(querytype2);

                tableOnlyExistsIn2.AddLine(
                    entityName2
                    , name2
                    , querytypeName2
                    , query2.IsUserDefined.ToString()
                    , query2.IsManaged.ToString()
                    , query2.Id.ToString()
                    );
            }

            {
                var reporter = new ProgressReporter(_writeToOutput, commonList.Count, 5, "Processing Common Saved Queries");

                foreach (var query in commonList)
                {
                    reporter.Increase();

                    FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                    tabDiff.SetHeader("Attribute", "Organization", "Value");

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            SavedQuery.Schema.Attributes.advancedgroupby
                            , SavedQuery.Schema.Attributes.canbedeleted 
                            //, columnsetxml 
                            , SavedQuery.Schema.Attributes.componentstate
                            , SavedQuery.Schema.Attributes.conditionalformatting 
                            //, SavedQuery.Schema.Attributes.createdby 
                            //, SavedQuery.Schema.Attributes.createdon
                            //, SavedQuery.Schema.Attributes.createdonbehalfby 
                            , SavedQuery.Schema.Attributes.description
                            //, SavedQuery.Schema.Attributes.fetchxml 
                            , SavedQuery.Schema.Attributes.introducedversion
                            , SavedQuery.Schema.Attributes.iscustomizable
                            , SavedQuery.Schema.Attributes.isdefault 
                            //, SavedQuery.Schema.Attributes.ismanaged 
                            , SavedQuery.Schema.Attributes.isprivate
                            , SavedQuery.Schema.Attributes.isquickfindquery
                            , SavedQuery.Schema.Attributes.isuserdefined
                            //, SavedQuery.Schema.Attributes.layoutxml 
                            //, SavedQuery.Schema.Attributes.modifiedby
                            //, SavedQuery.Schema.Attributes.modifiedon
                            //, SavedQuery.Schema.Attributes.modifiedonbehalfby 
                            , SavedQuery.Schema.Attributes.name
                            //, SavedQuery.Schema.Attributes.organizationid 
                            , SavedQuery.Schema.Attributes.organizationtaborder 
                            //, SavedQuery.Schema.Attributes.overwritetime
                            , SavedQuery.Schema.Attributes.queryapi
                            , SavedQuery.Schema.Attributes.queryappusage
                            , SavedQuery.Schema.Attributes.querytype
                            , SavedQuery.Schema.Attributes.returnedtypecode
                            //, SavedQuery.Schema.Attributes.savedqueryid 
                            //, SavedQuery.Schema.Attributes.savedqueryidunique
                            //, SavedQuery.Schema.Attributes.solutionid 
                            , SavedQuery.Schema.Attributes.statecode
                            , SavedQuery.Schema.Attributes.statuscode 
                            //, SavedQuery.Schema.Attributes.supportingsolutionid 
                            , SavedQuery.Schema.Attributes.versionnumber
                        };

                        foreach (var fieldName in fieldsToCompare)
                        {
                            if (ContentCoparerHelper.IsEntityDifferentInField(query.Entity1, query.Entity2, fieldName))
                            {
                                var str1 = EntityDescriptionHandler.GetAttributeString(query.Entity1, fieldName, _service1.ConnectionData);
                                var str2 = EntityDescriptionHandler.GetAttributeString(query.Entity2, fieldName, _service2.ConnectionData);

                                tabDiff.AddLine(fieldName, Connection1.Name, str1);
                                tabDiff.AddLine(fieldName, Connection2.Name, str2);
                            }
                        }
                    }

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            SavedQuery.Schema.Attributes.fetchxml
                            , SavedQuery.Schema.Attributes.layoutxml
                            , SavedQuery.Schema.Attributes.columnsetxml
                        };

                        foreach (var fieldName in fieldsToCompare)
                        {
                            Action<XElement> action = null;

                            if (string.Equals(fieldName, SavedQuery.Schema.Attributes.layoutxml))
                            {
                                action = ContentCoparerHelper.RemoveLayoutObjectCode;
                            }

                            string xml1 = query.Entity1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                            string xml2 = query.Entity2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                            if (!ContentCoparerHelper.CompareXML(xml1, xml2, false, action).IsEqual)
                            {
                                string reason = string.Empty;

                                var compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true, action);

                                if (compare.IsEqual)
                                {
                                    reason = "InCase";
                                }
                                else
                                {
                                    reason = compare.GetCompareDescription();
                                }

                                if (!string.IsNullOrEmpty(reason))
                                {
                                    tabDiff.AddLine(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, reason));
                                }
                            }
                        }
                    }

                    if (tabDiff.Count > 0)
                    {
                        var entityName1 = query.Entity1.ReturnedTypeCode;
                        var name1 = query.Entity1.Name;
                        var querytype1 = query.Entity1.QueryType.Value;

                        var querytypeName1 = SavedQueryRepository.GetQueryTypeName(querytype1);

                        dictDifference.Add(Tuple.Create(entityName1, name1, querytypeName1, query.Entity1.Id.ToString()), tabDiff.GetFormatedLines(false));
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

                content.AppendLine().AppendLine().AppendFormat("System Saved Queries ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("System Saved Queries ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("System Saved Queries DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("Entity", "Name", "QueryType", "Id");

                foreach (var template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2, template.Key.Item3, template.Key.Item4);
                }

                foreach (var template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2, template.Key.Item3, template.Key.Item4));

                    foreach (var str in template.Value)
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
                content.AppendLine("No difference in System Saved Queries.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking System Saved Queries ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} System Saved Queries.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> CheckSystemSavedQueryVisualizationsAsync()
        {
            return Task.Run(async () => await CheckSystemSavedQueryVisualizations());
        }

        private async Task<string> CheckSystemSavedQueryVisualizations()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking System Saved Query Visualizations (Charts) started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            var repository1 = new SavedQueryVisualizationRepository(_service1);
            var repository2 = new SavedQueryVisualizationRepository(_service2);

            var list1 = await repository1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("System Saved Query Visualizations (Charts) in {0}: {1}", Connection1.Name, list1.Count));

            var list2 = await repository2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("System Saved Query Visualizations (Charts) in {0}: {1}", Connection2.Name, list2.Count));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Entity", "Name", "IsManaged", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Entity", "Name", "IsManaged", "Id");

            var dictDifference = new Dictionary<Tuple<string, string, string>, List<string>>();

            var commonList = new List<LinkedEntities<SavedQueryVisualization>>();

            foreach (var chart1 in list1)
            {
                {
                    var chart2 = list2.FirstOrDefault(chart => chart.Id == chart1.Id);

                    if (chart2 != null)
                    {
                        commonList.Add(new LinkedEntities<SavedQueryVisualization>(chart1, chart2));
                        continue;
                    }
                }

                var entityName1 = chart1.PrimaryEntityTypeCode;
                var name1 = chart1.Name;

                tableOnlyExistsIn1.AddLine(entityName1, name1, chart1.IsManaged.ToString(), chart1.Id.ToString());
            }

            foreach (var chart2 in list2)
            {
                var entityName2 = chart2.PrimaryEntityTypeCode;
                var name2 = chart2.Name;

                {
                    var chart1 = list1.FirstOrDefault(chart => chart.Id == chart2.Id);

                    if (chart1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(entityName2, name2, chart2.IsManaged.ToString(), chart2.Id.ToString());
            }

            {
                var reporter = new ProgressReporter(_writeToOutput, commonList.Count, 5, "Processing Common Saved Query Visualizations (Charts)");

                foreach (var chart in commonList)
                {
                    var chart1 = chart.Entity1;
                    var chart2 = chart.Entity2;

                    FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                    tabDiff.SetHeader("Attribute", "Organization", "Value");

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            SavedQueryVisualization.Schema.Attributes.componentstate
                            //, SavedQueryVisualization.Schema.Attributes.createdby 
                            //, SavedQueryVisualization.Schema.Attributes.createdon 
                            //, SavedQueryVisualization.Schema.Attributes.createdonbehalfby 
                            //, SavedQueryVisualization.Schema.Attributes.datadescription 
                            , SavedQueryVisualization.Schema.Attributes.description
                            , SavedQueryVisualization.Schema.Attributes.introducedversion
                            , SavedQueryVisualization.Schema.Attributes.iscustomizable
                            , SavedQueryVisualization.Schema.Attributes.isdefault 
                            //, SavedQueryVisualization.Schema.Attributes.ismanaged 
                            //, SavedQueryVisualization.Schema.Attributes.modifiedby
                            //, SavedQueryVisualization.Schema.Attributes.modifiedon
                            //, SavedQueryVisualization.Schema.Attributes.modifiedonbehalfby
                            , SavedQueryVisualization.Schema.Attributes.name
                            //, SavedQueryVisualization.Schema.Attributes.organizationid
                            //, SavedQueryVisualization.Schema.Attributes.overwritetime
                            //, SavedQueryVisualization.Schema.Attributes.presentationdescription
                            , SavedQueryVisualization.Schema.Attributes.primaryentitytypecode
                            //, SavedQueryVisualization.Schema.Attributes.savedqueryvisualizationid
                            //, SavedQueryVisualization.Schema.Attributes.savedqueryvisualizationidunique
                            //, SavedQueryVisualization.Schema.Attributes.solutionid
                            //, SavedQueryVisualization.Schema.Attributes.supportingsolutionid 
                            , SavedQueryVisualization.Schema.Attributes.versionnumber
                            , SavedQueryVisualization.Schema.Attributes.webresourceid
                        };

                        foreach (var fieldName in fieldsToCompare)
                        {
                            if (ContentCoparerHelper.IsEntityDifferentInField(chart1, chart2, fieldName))
                            {
                                var str1 = EntityDescriptionHandler.GetAttributeString(chart1, fieldName, _service1.ConnectionData);
                                var str2 = EntityDescriptionHandler.GetAttributeString(chart2, fieldName, _service2.ConnectionData);

                                tabDiff.AddLine(fieldName, Connection1.Name, str1);
                                tabDiff.AddLine(fieldName, Connection2.Name, str2);
                            }
                        }
                    }

                    var entityName1 = chart1.PrimaryEntityTypeCode;
                    var name1 = chart1.Name;

                    {
                        List<string> fieldsToCompare = new List<string>()
                        {
                            SavedQueryVisualization.Schema.Attributes.datadescription
                            , SavedQueryVisualization.Schema.Attributes.presentationdescription
                        };

                        foreach (var fieldName in fieldsToCompare)
                        {
                            string xml1 = chart1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                            string xml2 = chart2.GetAttributeValue<string>(fieldName) ?? string.Empty;

                            if (!ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                            {
                                string reason = string.Empty;

                                var compare = ContentCoparerHelper.CompareXML(xml1.ToLower(), xml2.ToLower(), true);

                                if (compare.IsEqual)
                                {
                                    reason = "InCase";
                                }
                                else
                                {
                                    reason = compare.GetCompareDescription();
                                }

                                if (!string.IsNullOrEmpty(reason))
                                {
                                    tabDiff.AddLine(fieldName, string.Empty, string.Format("{0} - {1} {2}", Connection1.Name, Connection2.Name, reason));
                                }
                            }
                        }
                    }

                    if (tabDiff.Count > 0)
                    {
                        dictDifference.Add(Tuple.Create(entityName1, name1, chart1.Id.ToString()), tabDiff.GetFormatedLines(false));
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

                content.AppendLine().AppendLine().AppendFormat("System Saved Query Visualizations (Charts) ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("System Saved Query Visualizations (Charts) ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("System Saved Query Visualizations (Charts) DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("Entity", "Name", "Id");

                foreach (var template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2, template.Key.Item3);
                }

                foreach (var template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2, template.Key.Item3));

                    foreach (var str in template.Value)
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
                content.AppendLine("No difference in System Saved Query Visualizations (Charts).");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking System Saved Query Visualizations (Charts) ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} Saved Query Visualizations (Charts).txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }
    }
}
