using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
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
        public Task<string> CheckMailMergeTemplatesAsync()
        {
            return Task.Run(() => CheckMailMergeTemplates());
        }

        private async Task<string> CheckMailMergeTemplates()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Mail Merge Templates started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new MailMergeTemplateRepository(_service1);
            var repository2 = new MailMergeTemplateRepository(_service2);

            var list1 = await repository1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Mail Merge Templates in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Mail Merge Templates in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("AssociatedEntity", "Name", "Language", "File Name", "Mail Merge Type", "Viewable By", "Owner", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("AssociatedEntity", "Name", "Language", "File Name", "Mail Merge Type", "Viewable By", "Owner", "Id");

            var dictDifference = new Dictionary<Tuple<string, string, string, string>, List<string>>();

            foreach (var template1 in list1)
            {
                {
                    var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                var name1 = template1.Name;
                var entityName1 = template1.TemplateTypeCode;
                var language1 = template1.LanguageCode.Value;

                string filename = template1.FileName;
                string reportType = template1.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? template1.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

                var ownerRef = template1.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template1.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.ispersonal) ? template1.FormattedValues[MailMergeTemplate.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn1.AddLine(entityName1, name1, LanguageLocale.GetLocaleName(language1), filename, reportType, ispersonal, owner, template1.Id.ToString());
            }

            foreach (var template2 in list2)
            {
                {
                    var template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                var name2 = template2.Name;
                var entityName2 = template2.TemplateTypeCode;
                var language2 = template2.LanguageCode.Value;

                string filename = template2.FileName;
                string templateType = template2.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? template2.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

                var ownerRef = template2.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template2.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.ispersonal) ? template2.FormattedValues[MailMergeTemplate.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn2.AddLine(entityName2, name2, LanguageLocale.GetLocaleName(language2), filename, templateType, ispersonal, owner, template2.Id.ToString());
            }

            foreach (var template1 in list1)
            {
                var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                {
                    List<string> fieldsToCompare = new List<string>()
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

                    foreach (var fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                        {
                            var str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName);
                            var str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName);

                            tabDiff.AddLine(fieldName, Connection1.Name, str1);
                            tabDiff.AddLine(fieldName, Connection2.Name, str2);
                        }
                    }
                }

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        MailMergeTemplate.Schema.Attributes.parameterxml
                    };

                    foreach (var fieldName in fieldsToCompare)
                    {
                        var xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                        var xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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
                    var name1 = template1.Name;
                    var entityName1 = template1.TemplateTypeCode;
                    var language1 = template1.LanguageCode.Value;

                    dictDifference.Add(Tuple.Create(entityName1, name1, LanguageLocale.GetLocaleName(language1), template1.Id.ToString()), tabDiff.GetFormatedLines(false));
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
                content.AppendLine("No difference in Mail Merge Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Mail Merge Templates ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} Mail Merge Templates.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckEMailTemplatesAsync()
        {
            return Task.Run(() => CheckEMailTemplates());
        }

        private async Task<string> CheckEMailTemplates()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking E-Mail Templates started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new TemplateRepository(_service1);
            var repository2 = new TemplateRepository(_service2);

            var list1 = await repository1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("E-Mail Templates in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("E-Mail Templates in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("TemplateType", "Title", "ViewableBy", "Owner", "Id");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("TemplateType", "Title", "ViewableBy", "Owner", "Id");

            var dictDifference = new Dictionary<Tuple<string, string, string>, List<string>>();

            foreach (var template1 in list1)
            {
                {
                    var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                var name1 = template1.Title;
                var entityName1 = template1.TemplateTypeCode;

                var ownerRef = template1.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template1.FormattedValues.ContainsKey(Template.Schema.Attributes.ispersonal) ? template1.FormattedValues[Template.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn1.AddLine(entityName1, name1, ispersonal, owner, template1.Id.ToString());
            }

            foreach (var template2 in list2)
            {
                var name2 = template2.Title;
                var entityName2 = template2.TemplateTypeCode;

                {
                    var template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                var ownerRef = template2.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = template2.FormattedValues.ContainsKey(Template.Schema.Attributes.ispersonal) ? template2.FormattedValues[Template.Schema.Attributes.ispersonal] : string.Empty;

                tableOnlyExistsIn2.AddLine(entityName2, name2, ispersonal, owner);
            }

            foreach (var template1 in list1)
            {
                var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

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

                    foreach (var fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                        {
                            var str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName);
                            var str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName);

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

                    foreach (var fieldName in fieldsToCompare)
                    {
                        var xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                        var xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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
                    var name1 = template1.Title;
                    var entityName1 = template1.TemplateTypeCode;

                    dictDifference.Add(Tuple.Create(entityName1, name1, template1.Id.ToString()), tabDiff.GetFormatedLines(false));
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
                content.AppendLine("No difference in E-Mail Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking E-Mail Templates ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} E-Mail Templates.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckKBArticleTemplatesAsync()
        {
            return Task.Run(() => CheckKBArticleTemplates());
        }

        private async Task<string> CheckKBArticleTemplates()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking KB Article Templates started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new KbArticleTemplateRepository(_service1);
            var repository2 = new KbArticleTemplateRepository(_service2);

            var list1 = await repository1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("KB Article Templates in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("KB Article Templates in {0}: {1}", Connection2.Name, list2.Count()));

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

            var dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (var template1 in list1)
            {
                {
                    var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                var name1 = template1.Title;

                tableOnlyExistsIn1.AddLine(name1, template1.Id.ToString());
            }

            foreach (var template2 in list2)
            {
                {
                    var template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                var name2 = template2.Title;

                tableOnlyExistsIn2.AddLine(name2, template2.Id.ToString());
            }

            foreach (var template1 in list1)
            {
                var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                foreach (var fieldName in fieldsToCompare)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                    {
                        var str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName);
                        var str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                foreach (var fieldName in xmlFieldsToCompare)
                {
                    var xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    var xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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

                if (tabDiff.Count > 0)
                {
                    var name1 = template1.Title;

                    dictDifference.Add(Tuple.Create(name1, template1.Id.ToString()), tabDiff.GetFormatedLines(false));
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

                foreach (var template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2);
                }

                foreach (var template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2));

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
                content.AppendLine("No difference in KB Article Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking KB Article Templates ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} KB Article Templates.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }

        public Task<string> CheckContractTemplatesAsync()
        {
            return Task.Run(async () => await CheckContractTemplates());
        }

        private async Task<string> CheckContractTemplates()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Contract Templates started at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            var repository1 = new ContractTemplateRepository(_service1);
            var repository2 = new ContractTemplateRepository(_service2);

            var list1 = await repository1.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Contract Templates in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await repository2.GetListAsync();

            content.AppendLine(_writeToOutput.WriteToOutput("Contract Templates in {0}: {1}", Connection2.Name, list2.Count()));

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

            var dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (var template1 in list1)
            {
                {
                    var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                    if (template2 != null)
                    {
                        continue;
                    }
                }

                var name1 = template1.Name;

                tableOnlyExistsIn1.AddLine(name1, template1.Id.ToString());
            }

            foreach (var template2 in list2)
            {
                {
                    var template1 = list1.FirstOrDefault(template => template.Id == template2.Id);

                    if (template1 != null)
                    {
                        continue;
                    }
                }

                var name2 = template2.Name;

                tableOnlyExistsIn2.AddLine(name2, template2.Id.ToString());
            }

            foreach (var template1 in list1)
            {
                var template2 = list2.FirstOrDefault(template => template.Id == template1.Id);

                if (template2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                foreach (var fieldName in fieldsToCompare)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(template1, template2, fieldName))
                    {
                        var str1 = EntityDescriptionHandler.GetAttributeString(template1, fieldName);
                        var str2 = EntityDescriptionHandler.GetAttributeString(template2, fieldName);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                foreach (var fieldName in xmlFieldsToCompare)
                {
                    var xml1 = template1.GetAttributeValue<string>(fieldName) ?? string.Empty;
                    var xml2 = template2.GetAttributeValue<string>(fieldName) ?? string.Empty;

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

                if (tabDiff.Count > 0)
                {
                    var name1 = template1.Name;

                    dictDifference.Add(Tuple.Create(name1, template1.Id.ToString()), tabDiff.GetFormatedLines(false));
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

                foreach (var template in dictDifference)
                {
                    tableDifference.CalculateLineLengths(template.Key.Item1, template.Key.Item2);
                }

                foreach (var template in dictDifference
                    .OrderBy(w => w.Key.Item1)
                    .ThenBy(w => w.Key.Item2)
                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(template.Key.Item1, template.Key.Item2));

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
                content.AppendLine("No difference in Contract Templates.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Contract Templates ended at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));

            string fileName = string.Format("OrgCompare {0} at {1} Contract Templates.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

            return filePath;
        }
    }
}
