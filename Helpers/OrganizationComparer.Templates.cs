using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
        private static List<string> _fieldsToCompareMailMergeTemplateOrdinal = new List<string>()
        {
            MailMergeTemplate.Schema.Attributes.name
            , MailMergeTemplate.Schema.Attributes.templatetypecode
            , MailMergeTemplate.Schema.Attributes.languagecode
            , MailMergeTemplate.Schema.Attributes.description
            , MailMergeTemplate.Schema.Attributes.mailmergetype
            , MailMergeTemplate.Schema.Attributes.mimetype
            , MailMergeTemplate.Schema.Attributes.filename
            , MailMergeTemplate.Schema.Attributes.filesize
            , MailMergeTemplate.Schema.Attributes.body
            , MailMergeTemplate.Schema.Attributes.languagecode
            , MailMergeTemplate.Schema.Attributes.ispersonal
            , MailMergeTemplate.Schema.Attributes.statuscode
            , MailMergeTemplate.Schema.Attributes.iscustomizable
            //, MailMergeTemplate.Schema.Attributes.ismanaged
        };

        private static List<string> _fieldsToCompareMailMergeTemplateXml = new List<string>()
        {
            MailMergeTemplate.Schema.Attributes.parameterxml
        };

        public Task<string> CheckMailMergeTemplatesAsync()
        {
            return Task.Run(async () => await CheckMailMergeTemplates());
        }

        private async Task<string> CheckMailMergeTemplates()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingMailMergeTemplatesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            Task<List<MailMergeTemplate>> task1 = _comparerSource.GetMailMergeTemplate1Async();
            Task<List<MailMergeTemplate>> task2 = _comparerSource.GetMailMergeTemplate2Async();

            List<MailMergeTemplate> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Mail Merge Templates in {0}: {1}", Connection1.Name, list1.Count()));

            List<MailMergeTemplate> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Mail Merge Templates in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("AssociatedEntity", "Name", "Language", "File Name", "Mail Merge Type", "Viewable By", "Owner", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("AssociatedEntity", "Name", "Language", "File Name", "Mail Merge Type", "Viewable By", "Owner", "Id");

            Dictionary<Tuple<string, string, string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (MailMergeTemplate template1 in list1)
            {
                {
                    MailMergeTemplate template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                string name1 = template1.Name;
                string entityName1 = template1.TemplateTypeCode;
                int language1 = template1.LanguageCode.Value;

                string filename = template1.FileName;
                string reportType = template1.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? template1.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

                Microsoft.Xrm.Sdk.EntityReference ownerRef = template1.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template1.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.ispersonal) ? template1.FormattedValues[MailMergeTemplate.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn1.AddLine(entityName1, name1, LanguageLocale.GetLocaleName(language1), filename, reportType, ispersonal, owner, template1.Id.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.MailMergeTemplate, template1.Id);
            }

            foreach (MailMergeTemplate template2 in list2)
            {
                {
                    MailMergeTemplate template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                string name2 = template2.Name;
                string entityName2 = template2.TemplateTypeCode;
                int language2 = template2.LanguageCode.Value;

                string filename = template2.FileName;
                string templateType = template2.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? template2.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

                Microsoft.Xrm.Sdk.EntityReference ownerRef = template2.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template2.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.ispersonal) ? template2.FormattedValues[MailMergeTemplate.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn2.AddLine(entityName2, name2, LanguageLocale.GetLocaleName(language2), filename, templateType, ispersonal, owner, template2.Id.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.MailMergeTemplate, template2.Id);
            }

            foreach (MailMergeTemplate template1 in list1)
            {
                MailMergeTemplate template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                foreach (string fieldName in _fieldsToCompareMailMergeTemplateOrdinal)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                    {
                        string str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName, Connection1);
                        string str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName, Connection2);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                foreach (string fieldName in _fieldsToCompareMailMergeTemplateXml)
                {
                    string xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    string xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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

                if (tabDiff.Count > 0)
                {
                    string name1 = template1.Name;
                    string entityName1 = template1.TemplateTypeCode;
                    int language1 = template1.LanguageCode.Value;

                    var diff = tabDiff.GetFormatedLines(false);
                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.MailMergeTemplate, template1.Id, template2.Id, string.Join(Environment.NewLine, diff));

                    dictDifference.Add(Tuple.Create(entityName1, name1, LanguageLocale.GetLocaleName(language1), template1.Id.ToString()), diff);
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

                content.AppendLine().AppendLine().AppendFormat("Mail Merge Templates ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Mail Merge Templates ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Mail Merge Templates DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("AssociatedEntity", "Name", "Language", "Id");

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2, template.Key.Item3, template.Key.Item4);
                }

                foreach (KeyValuePair<Tuple<string, string, string, string>, List<string>> template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    .ThenBy(w => w.Key.Item4)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2, template.Key.Item3, template.Key.Item4));

                    foreach (string str in template.Value)
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
                content.AppendLine("No difference in Mail Merge Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Mail Merge Templates");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckEMailTemplatesAsync()
        {
            return Task.Run(async () => await CheckEMailTemplates());
        }

        private async Task<string> CheckEMailTemplates()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingEMailTemplatesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            Task<List<Template>> task1 = _comparerSource.GetTemplate1Async();
            Task<List<Template>> task2 = _comparerSource.GetTemplate2Async();

            List<Template> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("E-Mail Templates in {0}: {1}", Connection1.Name, list1.Count()));

            List<Template> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("E-Mail Templates in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("TemplateType", "Title", "ViewableBy", "Owner", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("TemplateType", "Title", "ViewableBy", "Owner", "Id");

            Dictionary<Tuple<string, string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string, string>, List<string>>();

            foreach (Template template1 in list1)
            {
                {
                    Template template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                string name1 = template1.Title;
                string entityName1 = template1.TemplateTypeCode;

                Microsoft.Xrm.Sdk.EntityReference ownerRef = template1.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template1.FormattedValues.ContainsKey(Template.Schema.Attributes.ispersonal) ? template1.FormattedValues[Template.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn1.AddLine(entityName1, name1, ispersonal, owner, template1.Id.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.EmailTemplate, template1.Id);
            }

            foreach (Template template2 in list2)
            {
                string name2 = template2.Title;
                string entityName2 = template2.TemplateTypeCode;

                {
                    Template template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                Microsoft.Xrm.Sdk.EntityReference ownerRef = template2.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template2.FormattedValues.ContainsKey(Template.Schema.Attributes.ispersonal) ? template2.FormattedValues[Template.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn2.AddLine(entityName2, name2, ispersonal, owner);

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.EmailTemplate, template2.Id);
            }

            foreach (Template template1 in list1)
            {
                Template template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        Template.Schema.Attributes.title
                        , Template.Schema.Attributes.templatetypecode
                        , Template.Schema.Attributes.description
                        , Template.Schema.Attributes.ispersonal
                        , Template.Schema.Attributes.languagecode
                        //, Template.Schema.Attributes.ismanaged
                        , Template.Schema.Attributes.iscustomizable
                        , Template.Schema.Attributes.generationtypecode
                    };

                    foreach (string fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                        {
                            string str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName, Connection1);
                            string str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName, Connection2);

                            tabDiff.AddLine(fieldName, Connection1.Name, str1);
                            tabDiff.AddLine(fieldName, Connection2.Name, str2);
                        }
                    }
                }

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        Template.Schema.Attributes.subject
                        , Template.Schema.Attributes.body
                        , Template.Schema.Attributes.presentationxml
                        , Template.Schema.Attributes.subjectpresentationxml
                    };

                    foreach (string fieldName in fieldsToCompare)
                    {
                        string xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                        string xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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
                    string name1 = template1.Title;
                    string entityName1 = template1.TemplateTypeCode;

                    var diff = tabDiff.GetFormatedLines(false);
                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.EmailTemplate, template1.Id, template2.Id, string.Join(Environment.NewLine, diff));

                    dictDifference.Add(Tuple.Create(entityName1, name1, template1.Id.ToString()), diff);

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

                content.AppendLine().AppendLine().AppendFormat("E-Mail Templates ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("E-Mail Templates ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("E-Mail Templates DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("TemplateType", "Title", "Id");

                foreach (KeyValuePair<Tuple<string, string, string>, List<string>> template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2, template.Key.Item3);
                }

                foreach (KeyValuePair<Tuple<string, string, string>, List<string>> template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    .ThenBy(w => w.Key.Item3)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2, template.Key.Item3));

                    foreach (string str in template.Value)
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
                content.AppendLine("No difference in E-Mail Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "E-Mail Templates");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckKBArticleTemplatesAsync()
        {
            return Task.Run(async () => await CheckKBArticleTemplates());
        }

        private async Task<string> CheckKBArticleTemplates()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingKBArticleTemplatesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            Task<List<KbArticleTemplate>> task1 = _comparerSource.GetKbArticleTemplate1Async();
            Task<List<KbArticleTemplate>> task2 = _comparerSource.GetKbArticleTemplate2Async();

            List<KbArticleTemplate> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("KB Article Templates in {0}: {1}", Connection1.Name, list1.Count()));

            List<KbArticleTemplate> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("KB Article Templates in {0}: {1}", Connection2.Name, list2.Count()));

            List<string> fieldsToCompare = new List<string>()
            {
                KbArticleTemplate.Schema.Attributes.title
                , KbArticleTemplate.Schema.Attributes.description
                , KbArticleTemplate.Schema.Attributes.isactive
                , KbArticleTemplate.Schema.Attributes.languagecode
                //, KbArticleTemplate.Schema.Attributes.ismanaged
            };

            List<string> xmlFieldsToCompare = new List<string>()
            {
                KbArticleTemplate.Schema.Attributes.structurexml
                , KbArticleTemplate.Schema.Attributes.formatxml
            };

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Title", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Title", "Id");

            Dictionary<Tuple<string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (KbArticleTemplate template1 in list1)
            {
                {
                    KbArticleTemplate template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                string name1 = template1.Title;

                tableOnlyExistsIn1.AddLine(name1, template1.Id.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.KbArticleTemplate, template1.Id);
            }

            foreach (KbArticleTemplate template2 in list2)
            {
                {
                    KbArticleTemplate template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                string name2 = template2.Title;

                tableOnlyExistsIn2.AddLine(name2, template2.Id.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.KbArticleTemplate, template2.Id);
            }

            foreach (KbArticleTemplate template1 in list1)
            {
                KbArticleTemplate template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                foreach (string fieldName in fieldsToCompare)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                    {
                        string str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName, Connection1);
                        string str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName, Connection2);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                foreach (string fieldName in xmlFieldsToCompare)
                {
                    string xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    string xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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

                if (tabDiff.Count > 0)
                {
                    string name1 = template1.Title;

                    var diff = tabDiff.GetFormatedLines(false);
                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.KbArticleTemplate, template1.Id, template2.Id, string.Join(Environment.NewLine, diff));

                    dictDifference.Add(Tuple.Create(name1, template1.Id.ToString()), diff);
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

                content.AppendLine().AppendLine().AppendFormat("KB Article Templates ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("KB Article Templates ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("KB Article Templates DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("Title", "Id");

                foreach (KeyValuePair<Tuple<string, string>, List<string>> template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2);
                }

                foreach (KeyValuePair<Tuple<string, string>, List<string>> template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2));

                    foreach (string str in template.Value)
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
                content.AppendLine("No difference in KB Article Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "KB Article Templates");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckContractTemplatesAsync()
        {
            return Task.Run(async () => await CheckContractTemplates());
        }

        private async Task<string> CheckContractTemplates()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingContractTemplatesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            Task<List<ContractTemplate>> task1 = _comparerSource.GetContractTemplate1Async();
            Task<List<ContractTemplate>> task2 = _comparerSource.GetContractTemplate2Async();

            List<ContractTemplate> list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Contract Templates in {0}: {1}", Connection1.Name, list1.Count()));

            List<ContractTemplate> list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Contract Templates in {0}: {1}", Connection2.Name, list2.Count()));

            List<string> fieldsToCompare = new List<string>()
            {
                ContractTemplate.Schema.Attributes.abbreviation
                , ContractTemplate.Schema.Attributes.allotmenttypecode
                , ContractTemplate.Schema.Attributes.billingfrequencycode
                //, ContractTemplate.Schema.Attributes.componentstate
                , ContractTemplate.Schema.Attributes.contractservicelevelcode
                //, ContractTemplate.Schema.Attributes.contracttemplateid
                //, ContractTemplate.Schema.Attributes.contracttemplateidunique
                //, ContractTemplate.Schema.Attributes.createdby
                //, ContractTemplate.Schema.Attributes.createdon
                //, ContractTemplate.Schema.Attributes.createdonbehalfby
                , ContractTemplate.Schema.Attributes.description
                , ContractTemplate.Schema.Attributes.effectivitycalendar
                //, ContractTemplate.Schema.Attributes.importsequencenumber
                //, ContractTemplate.Schema.Attributes.introducedversion
                , ContractTemplate.Schema.Attributes.iscustomizable
                //, ContractTemplate.Schema.Attributes.ismanaged
                //, ContractTemplate.Schema.Attributes.modifiedby
                //, ContractTemplate.Schema.Attributes.modifiedon
                //, ContractTemplate.Schema.Attributes.modifiedonbehalfby
                , ContractTemplate.Schema.Attributes.name
                //, ContractTemplate.Schema.Attributes.organizationid
                //, ContractTemplate.Schema.Attributes.overriddencreatedon
                //, ContractTemplate.Schema.Attributes.overwritetime
                //, ContractTemplate.Schema.Attributes.solutionid
                //, ContractTemplate.Schema.Attributes.supportingsolutionid
                , ContractTemplate.Schema.Attributes.usediscountaspercentage
                //, ContractTemplate.Schema.Attributes.versionnumber
            };

            List<string> xmlFieldsToCompare = new List<string>()
            {
                //ContractTemplate.Schema.Attributes.body
                //, ContractTemplate.Schema.Attributes.presentationxml
                //, ContractTemplate.Schema.Attributes.subjectpresentationxml
            };

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Name", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Name", "Id");

            Dictionary<Tuple<string, string>, List<string>> dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (ContractTemplate template1 in list1)
            {
                {
                    ContractTemplate template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                string name1 = template1.Name;

                tableOnlyExistsIn1.AddLine(name1, template1.Id.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.ContractTemplate, template1.Id);
            }

            foreach (ContractTemplate template2 in list2)
            {
                {
                    ContractTemplate template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                string name2 = template2.Name;

                tableOnlyExistsIn2.AddLine(name2, template2.Id.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.ContractTemplate, template2.Id);
            }

            foreach (ContractTemplate template1 in list1)
            {
                ContractTemplate template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                foreach (string fieldName in fieldsToCompare)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                    {
                        string str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName, Connection1);
                        string str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName, Connection2);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                foreach (string fieldName in xmlFieldsToCompare)
                {
                    string xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    string xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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

                if (tabDiff.Count > 0)
                {
                    string name1 = template1.Name;

                    var diff = tabDiff.GetFormatedLines(false);
                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.ContractTemplate, template1.Id, template2.Id, string.Join(Environment.NewLine, diff));

                    dictDifference.Add(Tuple.Create(name1, template1.Id.ToString()), diff);
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

                content.AppendLine().AppendLine().AppendFormat("Contract Templates ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Contract Templates ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Contract Templates DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("Title", "Id");

                foreach (KeyValuePair<Tuple<string, string>, List<string>> template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2);
                }

                foreach (KeyValuePair<Tuple<string, string>, List<string>> template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2));

                    foreach (string str in template.Value)
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
                content.AppendLine("No difference in Contract Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Contract Templates");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }
    }
}