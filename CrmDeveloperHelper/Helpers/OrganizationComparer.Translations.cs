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
        public Task<string> CheckDisplayStringsAsync()
        {
            return Task.Run(async () => await CheckDisplayStrings());
        }

        private async Task<string> CheckDisplayStrings()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingDisplayStringsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetDisplayString1Async();
            var task2 = _comparerSource.GetDisplayString2Async();
            var taskMap1 = _comparerSource.GetDisplayStringMap1Async();
            var taskMap2 = _comparerSource.GetDisplayStringMap2Async();

            var list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings in {0}: {1}", Connection2.Name, list2.Count()));

            var listMap1 = await taskMap1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings Maps in {0}: {1}", Connection1.Name, listMap1.Count()));

            var listMap2 = await taskMap2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings Maps in {0}: {1}", Connection1.Name, listMap2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Key", "LanguageCode", "Published", "Custom", "CustomComment", "FormatParameters");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Key", "LanguageCode", "Published", "Custom", "CustomComment", "FormatParameters");

            var dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (var displayString1 in list1)
            {
                var displaystringkey1 = displayString1.DisplayStringKey;
                var languagecode1 = displayString1.LanguageCode.Value;

                {
                    var displayString2 = list2.FirstOrDefault(displayString =>
                    {
                        var displaystringkey2 = displayString.DisplayStringKey;
                        var languagecode2 = displayString.LanguageCode.Value;

                        return displaystringkey1 == displaystringkey2 && languagecode1 == languagecode2;
                    });

                    if (displayString2 != null)
                    {
                        continue;
                    }
                }

                var customComment = displayString1.CustomComment;
                var customDisplayString = displayString1.CustomComment;
                var formatParameters = displayString1.FormatParameters.Value;
                var publishedDisplayString = displayString1.PublishedDisplayString;

                tableOnlyExistsIn1.AddLine(displaystringkey1, LanguageLocale.GetLocaleName(languagecode1), publishedDisplayString, customDisplayString, customComment, formatParameters.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.DisplayString, displayString1.Id);
            }

            foreach (var displayString2 in list2)
            {
                var displaystringkey2 = displayString2.DisplayStringKey;
                var languagecode2 = displayString2.LanguageCode.Value;

                {
                    var displayString1 = list1.FirstOrDefault(displayString =>
                    {
                        var displaystringkey1 = displayString.DisplayStringKey;
                        var languagecode1 = displayString.LanguageCode.Value;

                        return displaystringkey1 == displaystringkey2 && languagecode1 == languagecode2;
                    });

                    if (displayString1 != null)
                    {
                        continue;
                    }
                }

                var customComment = displayString2.CustomComment;
                var customDisplayString = displayString2.CustomDisplayString;
                var formatParameters = displayString2.FormatParameters.Value;
                var publishedDisplayString = displayString2.PublishedDisplayString;

                tableOnlyExistsIn2.AddLine(displaystringkey2, LanguageLocale.GetLocaleName(languagecode2), publishedDisplayString, customDisplayString, customComment, formatParameters.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.DisplayString, displayString2.Id);
            }

            foreach (var displayString1 in list1)
            {
                var displaystringkey1 = displayString1.DisplayStringKey;
                var languagecode1 = displayString1.LanguageCode.Value;

                var displayString2 = list2.FirstOrDefault(displayString =>
                {
                    var displaystringkey2 = displayString.DisplayStringKey;
                    var languagecode2 = displayString.LanguageCode.Value;

                    return displaystringkey1 == displaystringkey2 && languagecode1 == languagecode2;
                });

                if (displayString2 == null)
                {
                    continue;
                }

                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                var temp1 = listMap1.Where(s => s.DisplayStringId == displayString1.Id);
                var temp2 = listMap2.Where(s => s.DisplayStringId == displayString2.Id);

                if (temp1.SequenceEqual(temp2))
                {
                    tabDiff.AddLine("DisplayStringMap", Connection1.Name, "Differs");
                    tabDiff.AddLine("DisplayStringMap", Connection2.Name, "Differs");
                }

                List<string> fieldsToCompare = new List<string>()
                {
                    DisplayString.Schema.Attributes.customcomment
                    , DisplayString.Schema.Attributes.customdisplaystring
                    , DisplayString.Schema.Attributes.formatparameters
                    , DisplayString.Schema.Attributes.publisheddisplaystring
                };

                foreach (var fieldName in fieldsToCompare)
                {
                    if (ContentCoparerHelper.IsEntityDifferentInField(displayString1, displayString2, fieldName))
                    {
                        var str1 = EntityDescriptionHandler.GetAttributeString(displayString1, fieldName, Connection1);
                        var str2 = EntityDescriptionHandler.GetAttributeString(displayString2, fieldName, Connection2);

                        tabDiff.AddLine(fieldName, Connection1.Name, str1);
                        tabDiff.AddLine(fieldName, Connection2.Name, str2);
                    }
                }

                if (tabDiff.Count > 0)
                {
                    var diff = tabDiff.GetFormatedLines(false);

                    dictDifference.Add(Tuple.Create(displaystringkey1, LanguageLocale.GetLocaleName(languagecode1)), diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.DisplayString, displayString1.Id, displayString2.Id, string.Join(Environment.NewLine, diff));
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

                content.AppendLine().AppendLine().AppendFormat("Display Strings ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Display Strings ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Display Strings DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, dictDifference.Count);

                FormatTextTableHandler tableDifference = new FormatTextTableHandler();
                tableDifference.SetHeader("Key", "LanguageCode");

                foreach (var displayString in dictDifference)
                {
                    tableDifference.CalculateLineLengths(displayString.Key.Item1, displayString.Key.Item2);
                }

                foreach (var displayString in dictDifference
                                    .OrderBy(w => w.Key.Item1)
                                    .ThenBy(w => w.Key.Item2)
                                    )
                {
                    content.AppendLine().Append(tabSpacer + tableDifference.FormatLine(displayString.Key.Item1, displayString.Key.Item2));

                    foreach (var str in displayString.Value)
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
                content.AppendLine("No difference in Display Strings.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Display Strings");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckDefaultTranslationsAsync()
        {
            return Task.Run(async () => await CheckDefaultTranslations());
        }

        private async Task<string> CheckDefaultTranslations()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingDefaultTranslationsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = TranslationRepository.GetDefaultTranslationFromCacheAsync(Connection1.ConnectionId, _comparerSource.Service1);
            var task2 = TranslationRepository.GetDefaultTranslationFromCacheAsync(Connection2.ConnectionId, _comparerSource.Service2);

            var translation1 = await task1;

            if (translation1 != null)
            {
                content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings in {0}: {1}", Connection1.Name, translation1.DisplayStrings.Count()));
                content.AppendLine(_iWriteToOutput.WriteToOutput("Localized Labels in {0}: {1}", Connection1.Name, translation1.LocalizedLabels.Count()));
            }

            var translation2 = await task2;

            if (translation2 != null)
            {
                content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings in {0}: {1}", Connection2.Name, translation2.DisplayStrings.Count()));
                content.AppendLine(_iWriteToOutput.WriteToOutput("Localized Labels in {0}: {1}", Connection2.Name, translation2.LocalizedLabels.Count()));
            }

            if (translation1 != null && translation2 != null)
            {
                CompareTranslations(content, translation1, translation2);

                content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

                string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Default Translations");

                string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                return filePath;
            }

            return string.Empty;
        }

        public Task<string> CheckFieldTranslationsAsync()
        {
            return Task.Run(async () => await CheckFieldTranslations());
        }

        private async Task<string> CheckFieldTranslations()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingFieldTranslationsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = TranslationRepository.GetFieldTranslationFromCacheAsync(Connection1.ConnectionId, _comparerSource.Service1);
            var task2 = TranslationRepository.GetFieldTranslationFromCacheAsync(Connection2.ConnectionId, _comparerSource.Service2);

            var translation1 = await task1;

            if (translation1 != null)
            {
                content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings in {0}: {1}", Connection1.Name, translation1.DisplayStrings.Count()));
                content.AppendLine(_iWriteToOutput.WriteToOutput("Localized Labels in {0}: {1}", Connection1.Name, translation1.LocalizedLabels.Count()));
            }
            else
            {
                content.AppendLine(_iWriteToOutput.WriteToOutput("Field Translations not finded in {0}", Connection1.Name));
            }

            var translation2 = await task2;

            if (translation2 != null)
            {
                content.AppendLine(_iWriteToOutput.WriteToOutput("Display Strings in {0}: {1}", Connection2.Name, translation2.DisplayStrings.Count()));
                content.AppendLine(_iWriteToOutput.WriteToOutput("Localized Labels in {0}: {1}", Connection2.Name, translation2.LocalizedLabels.Count()));
            }
            else
            {
                content.AppendLine(_iWriteToOutput.WriteToOutput("Field Translations not finded in {0}", Connection2.Name));
            }

            if (translation1 != null && translation2 != null)
            {
                CompareTranslations(content, translation1, translation2);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Field Translations");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        private void CompareTranslations(StringBuilder content, Translation translation1, Translation translation2)
        {
            {
                FormatTextTableHandler displayStringsOnlyExistsIn1 = new FormatTextTableHandler();
                displayStringsOnlyExistsIn1.SetHeader("EntityName", "StringKey", "LanguageCode", "Label");

                FormatTextTableHandler displayStringsOnlyExistsIn2 = new FormatTextTableHandler();
                displayStringsOnlyExistsIn2.SetHeader("EntityName", "StringKey", "LanguageCode", "Label");

                FormatTextTableHandler displayStringsDifference = new FormatTextTableHandler();
                displayStringsDifference.SetHeader("EntityName", "StringKey", "LanguageCode", "Connection", "Label");

                foreach (var displayString1 in translation1.DisplayStrings)
                {
                    {
                        var displayString2 = translation2.DisplayStrings.FirstOrDefault(d =>
                            string.Equals(displayString1.EntityName, d.EntityName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(displayString1.StringKey, d.StringKey, StringComparison.OrdinalIgnoreCase)
                            );

                        if (displayString2 != null)
                        {
                            continue;
                        }
                    }

                    foreach (var label in displayString1.Labels)
                    {
                        displayStringsOnlyExistsIn1.AddLine(displayString1.EntityName, displayString1.StringKey, LanguageLocale.GetLocaleName(label.LanguageCode), label.Value);
                    }
                }

                foreach (var displayString2 in translation2.DisplayStrings)
                {
                    {
                        var displayString1 = translation1.DisplayStrings.FirstOrDefault(d =>
                            string.Equals(displayString2.EntityName, d.EntityName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(displayString2.StringKey, d.StringKey, StringComparison.OrdinalIgnoreCase)
                            );

                        if (displayString1 != null)
                        {
                            continue;
                        }
                    }

                    foreach (var label in displayString2.Labels)
                    {
                        displayStringsOnlyExistsIn2.AddLine(displayString2.EntityName, displayString2.StringKey, LanguageLocale.GetLocaleName(label.LanguageCode), label.Value);
                    }
                }

                foreach (var displayString1 in translation1.DisplayStrings)
                {
                    var displayString2 = translation2.DisplayStrings.FirstOrDefault(d =>
                            string.Equals(displayString1.EntityName, d.EntityName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(displayString1.StringKey, d.StringKey, StringComparison.OrdinalIgnoreCase)
                            );

                    if (displayString2 == null)
                    {
                        continue;
                    }

                    var diff = LabelComparer.GetDifference(displayString1, displayString2);

                    if (!diff.IsEmpty)
                    {
                        foreach (var item in diff.LabelsOnlyIn1)
                        {
                            displayStringsDifference.AddLine(displayString1.EntityName, displayString1.StringKey, LanguageLocale.GetLocaleName(item.LanguageCode), Connection1.Name, item.Value);
                        }

                        foreach (var item in diff.LabelsOnlyIn2)
                        {
                            displayStringsDifference.AddLine(displayString1.EntityName, displayString1.StringKey, LanguageLocale.GetLocaleName(item.LanguageCode), Connection2.Name, item.Value);
                        }

                        foreach (var item in diff.LabelDifference)
                        {
                            displayStringsDifference.AddLine(displayString1.EntityName, displayString1.StringKey, LanguageLocale.GetLocaleName(item.LanguageCode), Connection1.Name, item.Value1);
                            displayStringsDifference.AddLine(displayString1.EntityName, displayString1.StringKey, LanguageLocale.GetLocaleName(item.LanguageCode), Connection2.Name, item.Value2);
                        }
                    }
                }

                if (displayStringsOnlyExistsIn1.Count > 0)
                {
                    content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                    content.AppendLine().AppendLine().AppendFormat("Display Strings ONLY EXISTS in {0}: {1}", Connection1.Name, displayStringsOnlyExistsIn1.Count);

                    displayStringsOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
                }

                if (displayStringsOnlyExistsIn2.Count > 0)
                {
                    content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                    content.AppendLine().AppendLine().AppendFormat("Display Strings ONLY EXISTS in {0}: {1}", Connection2.Name, displayStringsOnlyExistsIn2.Count);

                    displayStringsOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
                }

                if (displayStringsDifference.Count > 0)
                {
                    content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                    content.AppendLine().AppendLine().AppendFormat("Display Strings DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, displayStringsDifference.Count);

                    displayStringsDifference.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()).AppendLine());
                }

                if (displayStringsOnlyExistsIn2.Count == 0
                    && displayStringsOnlyExistsIn1.Count == 0
                    && displayStringsDifference.Count == 0
                    )
                {
                    content.AppendLine("No difference in Display Strings.");
                }
            }

            {
                FormatTextTableHandler localizedLabelsOnlyExistsIn1 = new FormatTextTableHandler();
                localizedLabelsOnlyExistsIn1.SetHeader("EntityName", "ObjectId", "ColumnName", "LanguageCode", "Label");

                FormatTextTableHandler localizedLabelsOnlyExistsIn2 = new FormatTextTableHandler();
                localizedLabelsOnlyExistsIn2.SetHeader("EntityName", "ObjectId", "ColumnName", "LanguageCode", "Label");

                FormatTextTableHandler localizedLabelsDifference = new FormatTextTableHandler();
                localizedLabelsDifference.SetHeader("EntityName", "ObjectId", "ColumnName", "LanguageCode", "Connection", "Label");

                foreach (var locLabel1 in translation1.LocalizedLabels)
                {
                    {
                        var locLabel2 = translation2.LocalizedLabels.FirstOrDefault(d =>
                            string.Equals(locLabel1.EntityName, d.EntityName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(locLabel1.ColumnName, d.ColumnName, StringComparison.OrdinalIgnoreCase)
                            && locLabel1.ObjectId == d.ObjectId
                            );

                        if (locLabel2 != null)
                        {
                            continue;
                        }
                    }

                    foreach (var label in locLabel1.Labels)
                    {
                        localizedLabelsOnlyExistsIn1.AddLine(locLabel1.EntityName, locLabel1.ObjectId.ToString(), locLabel1.ColumnName, LanguageLocale.GetLocaleName(label.LanguageCode), label.Value);
                    }
                }

                foreach (var locLabel2 in translation2.LocalizedLabels)
                {
                    {
                        var locLabel1 = translation1.LocalizedLabels.FirstOrDefault(d =>
                            string.Equals(locLabel2.EntityName, d.EntityName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(locLabel2.ColumnName, d.ColumnName, StringComparison.OrdinalIgnoreCase)
                            && locLabel2.ObjectId == d.ObjectId
                            );

                        if (locLabel1 != null)
                        {
                            continue;
                        }
                    }

                    foreach (var label in locLabel2.Labels)
                    {
                        localizedLabelsOnlyExistsIn2.AddLine(locLabel2.EntityName, locLabel2.ObjectId.ToString(), locLabel2.ColumnName, LanguageLocale.GetLocaleName(label.LanguageCode), label.Value);
                    }
                }

                foreach (var locLabel1 in translation1.LocalizedLabels)
                {
                    var locLabel2 = translation2.LocalizedLabels.FirstOrDefault(d =>
                            string.Equals(locLabel1.EntityName, d.EntityName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(locLabel1.ColumnName, d.ColumnName, StringComparison.OrdinalIgnoreCase)
                            && locLabel1.ObjectId == d.ObjectId
                            );

                    if (locLabel2 == null)
                    {
                        continue;
                    }

                    var diff = LabelComparer.GetDifference(locLabel1, locLabel2);

                    if (!diff.IsEmpty)
                    {
                        foreach (var item in diff.LabelsOnlyIn1)
                        {
                            localizedLabelsDifference.AddLine(locLabel1.EntityName, locLabel1.ObjectId.ToString(), locLabel1.ColumnName, LanguageLocale.GetLocaleName(item.LanguageCode), Connection1.Name, item.Value);
                        }

                        foreach (var item in diff.LabelsOnlyIn2)
                        {
                            localizedLabelsDifference.AddLine(locLabel1.EntityName, locLabel1.ObjectId.ToString(), locLabel1.ColumnName, LanguageLocale.GetLocaleName(item.LanguageCode), Connection2.Name, item.Value);
                        }

                        foreach (var item in diff.LabelDifference)
                        {
                            localizedLabelsDifference.AddLine(locLabel1.EntityName, locLabel1.ObjectId.ToString(), locLabel1.ColumnName, LanguageLocale.GetLocaleName(item.LanguageCode), Connection1.Name, item.Value1);
                            localizedLabelsDifference.AddLine(locLabel1.EntityName, locLabel1.ObjectId.ToString(), locLabel1.ColumnName, LanguageLocale.GetLocaleName(item.LanguageCode), Connection2.Name, item.Value2);
                        }
                    }
                }

                if (localizedLabelsOnlyExistsIn1.Count > 0)
                {
                    content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                    content.AppendLine().AppendLine().AppendFormat("Localized Labels ONLY EXISTS in {0}: {1}", Connection1.Name, localizedLabelsOnlyExistsIn1.Count);

                    localizedLabelsOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
                }

                if (localizedLabelsOnlyExistsIn2.Count > 0)
                {
                    content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                    content.AppendLine().AppendLine().AppendFormat("Localized Labels ONLY EXISTS in {0}: {1}", Connection2.Name, localizedLabelsOnlyExistsIn2.Count);

                    localizedLabelsOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
                }

                if (localizedLabelsDifference.Count > 0)
                {
                    content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                    content.AppendLine().AppendLine().AppendFormat("Localized Labels DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, localizedLabelsDifference.Count);

                    localizedLabelsDifference.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()).AppendLine());
                }

                if (localizedLabelsOnlyExistsIn2.Count == 0
                    && localizedLabelsOnlyExistsIn1.Count == 0
                    && localizedLabelsDifference.Count == 0
                    )
                {
                    content.AppendLine("No difference in Localized Labels.");
                }
            }
        }
    }
}
