using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class OptionSetComparer
    {
        private string _tabSpacer;
        private string _connectionName1;
        private string _connectionName2;

        private StringMapRepository _rep1;
        private StringMapRepository _rep2;

        private Dictionary<string, List<StringMap>> _cache1 = new Dictionary<string, List<StringMap>>(StringComparer.InvariantCultureIgnoreCase);
        private Dictionary<string, List<StringMap>> _cache2 = new Dictionary<string, List<StringMap>>(StringComparer.InvariantCultureIgnoreCase);

        public OptionSetComparer(string tabSpacer, string connectionName1, string connectionName2, StringMapRepository rep1, StringMapRepository rep2)
        {
            this._tabSpacer = tabSpacer;
            this._connectionName1 = connectionName1;
            this._connectionName2 = connectionName2;

            this._rep1 = rep1;
            this._rep2 = rep2;
        }

        public async Task<List<string>> GetDifference(OptionSetMetadata optionSet1, OptionSetMetadata optionSet2, string entityName1, string attributeName1, string entityName2 = null, string attributeName2 = null)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);

                table.CalculateLineLengths("LanguageCode", "Value");
                table.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentDisplayName = LabelComparer.GetDifference(optionSet1.DisplayName, optionSet2.DisplayName);
                var isDifferentDescription = LabelComparer.GetDifference(optionSet1.Description, optionSet2.Description);

                isDifferentDisplayName.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                isDifferentDescription.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentDisplayName.IsEmpty)
                {
                    if (isDifferentDisplayName.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName1, isDifferentDisplayName.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName2, isDifferentDisplayName.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDisplayName.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDisplayName.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }

                if (!isDifferentDescription.IsEmpty)
                {
                    if (isDifferentDescription.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName1, isDifferentDescription.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName2, isDifferentDescription.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDescription.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDescription.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }
            }

            {
                var table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                table.AddLineIfNotEqual("IsCustomizable", optionSet1.IsCustomizable, optionSet2.IsCustomizable);
                table.AddLineIfNotEqual("IsCustomOptionSet", optionSet1.IsCustomOptionSet, optionSet2.IsCustomOptionSet);
                table.AddLineIfNotEqual("IsGlobal", optionSet1.IsGlobal, optionSet2.IsGlobal);
                //table.AddLineIfNotEqual("IsManaged", optionSet1.IsManaged, optionSet2.IsManaged);
                table.AddLineIfNotEqual("Name", optionSet1.Name, optionSet2.Name);

                if (table.Count > 0)
                {
                    strDifference.AddRange(table.GetFormatedLines(true));
                }
            }

            {
                var optionValueOnly1 = new Dictionary<Tuple<int, bool?>, List<string>>();
                var optionValueOnly2 = new Dictionary<Tuple<int, bool?>, List<string>>();

                var optionValueDifferent = new Dictionary<int, List<string>>();

                foreach (var optionMetadata1 in optionSet1.Options.Where(e => e.Value.HasValue).OrderBy(e => e.Value))
                {
                    {
                        var optionMetadata2 = optionSet2.Options.FirstOrDefault(e => e.Value.HasValue && e.Value == optionMetadata1.Value);

                        if (optionMetadata2 != null)
                        {
                            continue;
                        }
                    }

                    List<string> listStrings = new List<string>();

                    CreateFileHandler.FillLabelDisplayNameAndDescription(listStrings, true, optionMetadata1.Label, optionMetadata1.Description, _tabSpacer);

                    optionValueOnly1.Add(Tuple.Create(optionMetadata1.Value.Value, optionMetadata1.IsManaged), listStrings);
                }

                foreach (var optionMetadata2 in optionSet2.Options.Where(e => e.Value.HasValue).OrderBy(e => e.Value))
                {
                    {
                        var optionMetadata1 = optionSet1.Options.FirstOrDefault(e => e.Value.HasValue && e.Value == optionMetadata2.Value);

                        if (optionMetadata1 != null)
                        {
                            continue;
                        }
                    }

                    List<string> listStrings = new List<string>();

                    CreateFileHandler.FillLabelDisplayNameAndDescription(listStrings, true, optionMetadata2.Label, optionMetadata2.Description, _tabSpacer);

                    optionValueOnly2.Add(Tuple.Create(optionMetadata2.Value.Value, optionMetadata2.IsManaged), listStrings);
                }

                foreach (var optionMetadata1 in optionSet1.Options.Where(e => e.Value.HasValue).OrderBy(e => e.Value))
                {
                    var optionMetadata2 = optionSet2.Options.FirstOrDefault(e => e.Value.HasValue && e.Value == optionMetadata1.Value);

                    if (optionMetadata2 == null)
                    {
                        continue;
                    }

                    List<string> optionDiff = await GetDifferenceOptionSetValue(optionMetadata1, optionMetadata2, entityName1, attributeName1, entityName2, attributeName2);

                    if (optionDiff.Count > 0)
                    {
                        optionValueDifferent.Add(optionMetadata1.Value.Value, optionDiff);
                    }
                }

                if (optionValueOnly1.Count > 0)
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Values ONLY EXISTS in {0}: {1}", _connectionName1, optionValueOnly1.Count));

                    foreach (var value in optionValueOnly1.OrderBy(s => s.Key.Item1))
                    {
                        strDifference.Add(_tabSpacer + string.Format("{0}   IsManaged: {1}", value.Key.Item1, value.Key.Item2));

                        foreach (var str in value.Value)
                        {
                            strDifference.Add(_tabSpacer + _tabSpacer + str);
                        }
                    }
                }

                if (optionValueOnly2.Count > 0)
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Values ONLY EXISTS in {0}: {1}", _connectionName2, optionValueOnly2.Count));

                    foreach (var value in optionValueOnly2)
                    {
                        strDifference.Add(_tabSpacer + string.Format("{0}   IsManaged: {1}", value.Key.Item1, value.Key.Item2));

                        foreach (var str in value.Value)
                        {
                            strDifference.Add(_tabSpacer + _tabSpacer + str);
                        }
                    }
                }

                if (optionValueDifferent.Count > 0)
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Values DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, optionValueDifferent.Count));

                    foreach (var value in optionValueDifferent.OrderBy(e => e.Key))
                    {
                        strDifference.Add(_tabSpacer + value.Key);

                        foreach (var str in value.Value)
                        {
                            strDifference.Add(_tabSpacer + _tabSpacer + str);
                        }
                    }
                }
            }

            return strDifference;
        }

        internal async Task<List<string>> GetDifference(BooleanOptionSetMetadata optionSet1, BooleanOptionSetMetadata optionSet2, string entityName, string attributeName)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);

                table.CalculateLineLengths("LanguageCode", "Value");
                table.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentDisplayName = LabelComparer.GetDifference(optionSet1.DisplayName, optionSet2.DisplayName);
                var isDifferentDescription = LabelComparer.GetDifference(optionSet1.Description, optionSet2.Description);

                isDifferentDisplayName.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                isDifferentDescription.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentDisplayName.IsEmpty)
                {
                    if (isDifferentDisplayName.LabelsOnlyIn1.Count > 0)
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName1, isDifferentDisplayName.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelsOnlyIn2.Count > 0)
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName2, isDifferentDisplayName.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelDifference.Count > 0)
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("DisplayNames DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDisplayName.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDisplayName.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }

                if (!isDifferentDescription.IsEmpty)
                {
                    if (isDifferentDescription.LabelsOnlyIn1.Count > 0)
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName1, isDifferentDescription.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelsOnlyIn2.Count > 0)
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName2, isDifferentDescription.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelDifference.Count > 0)
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("Descriptions DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDescription.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDescription.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }
            }

            {
                var table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                table.AddLineIfNotEqual("IsCustomizable", optionSet1.IsCustomizable, optionSet2.IsCustomizable);
                table.AddLineIfNotEqual("IsCustomOptionSet", optionSet1.IsCustomOptionSet, optionSet2.IsCustomOptionSet);
                table.AddLineIfNotEqual("IsGlobal", optionSet1.IsGlobal, optionSet2.IsGlobal);
                //table.AddLineIfNotEqual("IsManaged", optionSet1.IsManaged, optionSet2.IsManaged);
                table.AddLineIfNotEqual("Name", optionSet1.Name, optionSet2.Name);

                if (table.Count > 0)
                {
                    strDifference.AddRange(table.GetFormatedLines(true));
                }
            }

            {
                Dictionary<string, List<string>> optionValueDifferent = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

                {
                    List<string> optionDiff = await GetDifferenceOptionSetValue(optionSet1.FalseOption, optionSet2.FalseOption, entityName, attributeName);

                    if (optionDiff.Count > 0)
                    {
                        optionValueDifferent.Add("FalseOption", optionDiff);
                    }
                }

                {
                    List<string> optionDiff = await GetDifferenceOptionSetValue(optionSet1.TrueOption, optionSet2.TrueOption, entityName, attributeName);

                    if (optionDiff.Count > 0)
                    {
                        optionValueDifferent.Add("TrueOption", optionDiff);
                    }
                }

                if (optionValueDifferent.Count > 0)
                {
                    foreach (var item in optionValueDifferent.OrderBy(e => e.Key))
                    {
                        if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                        strDifference.Add(string.Format("Different Value {0}", item.Key));

                        foreach (var value in item.Value)
                        {
                            strDifference.Add(_tabSpacer + value);
                        }
                    }
                }
            }

            return strDifference;
        }

        private async Task<List<string>> GetDifferenceOptionSetValue(OptionMetadata optionMetadata1, OptionMetadata optionMetadata2, string entityName1, string attributeName1, string entityName2 = null, string attributeName2 = null)
        {
            List<string> optionDiff = new List<string>();

            int? displayOrder1 = null;
            int? displayOrder2 = null;

            if (entityName2 == null)
            {
                entityName2 = entityName1;
            }

            if (attributeName2 == null)
            {
                attributeName2 = attributeName1;
            }

            if (!string.IsNullOrEmpty(entityName1) && !string.IsNullOrEmpty(attributeName1))
            {
                {
                    if (!_cache1.ContainsKey(entityName1))
                    {
                        _cache1[entityName1] = await _rep1.GetListAsync(entityName1);
                    }

                    var stringMap1 = _cache1[entityName1].FirstOrDefault(e =>
                        string.Equals(e.AttributeName, attributeName1, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(e.ObjectTypeCode, entityName1, StringComparison.OrdinalIgnoreCase)
                        && e.AttributeValue == optionMetadata1.Value.Value
                        );

                    if (stringMap1 != null)
                    {
                        displayOrder1 = stringMap1.DisplayOrder;
                    }
                }

            }

            if (!string.IsNullOrEmpty(entityName2) && !string.IsNullOrEmpty(attributeName2))
            {
                {
                    if (!_cache2.ContainsKey(entityName2))
                    {
                        _cache2[entityName2] = await _rep2.GetListAsync(entityName2);
                    }

                    var stringMap2 = _cache2[entityName2].FirstOrDefault(e =>
                        string.Equals(e.AttributeName, attributeName2, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(e.ObjectTypeCode, entityName2, StringComparison.OrdinalIgnoreCase)
                        && e.AttributeValue == optionMetadata1.Value.Value
                        );

                    if (stringMap2 != null)
                    {
                        displayOrder2 = stringMap2.DisplayOrder;
                    }
                }
            }

            {
                FormatTextTableHandler tableFormatter = new FormatTextTableHandler(true);

                tableFormatter.CalculateLineLengths("LanguageCode", "Value");
                tableFormatter.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentLabel = LabelComparer.GetDifference(optionMetadata1.Label, optionMetadata2.Label);
                var isDifferentDescription = LabelComparer.GetDifference(optionMetadata1.Description, optionMetadata2.Description);

                isDifferentLabel.LabelsOnlyIn1.ForEach(i => tableFormatter.CalculateLineLengths(i.Locale, i.Value));
                isDifferentLabel.LabelsOnlyIn2.ForEach(i => tableFormatter.CalculateLineLengths(i.Locale, i.Value));
                isDifferentLabel.LabelDifference.ForEach(i =>
                {
                    tableFormatter.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    tableFormatter.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                isDifferentDescription.LabelsOnlyIn1.ForEach(i => tableFormatter.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelsOnlyIn2.ForEach(i => tableFormatter.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelDifference.ForEach(i =>
                {
                    tableFormatter.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    tableFormatter.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentLabel.IsEmpty)
                {
                    if (isDifferentLabel.LabelsOnlyIn1.Count > 0)
                    {
                        optionDiff.Add(string.Format("Labels ONLY in {0}: {1}", _connectionName1, isDifferentLabel.LabelsOnlyIn1.Count));
                        optionDiff.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Value"));
                        isDifferentLabel.LabelsOnlyIn1.ForEach(e => optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentLabel.LabelsOnlyIn2.Count > 0)
                    {
                        optionDiff.Add(string.Format("Labels ONLY in {0}: {1}", _connectionName2, isDifferentLabel.LabelsOnlyIn2.Count));
                        optionDiff.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Value"));
                        isDifferentLabel.LabelsOnlyIn2.ForEach(e => optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentLabel.LabelDifference.Count > 0)
                    {
                        optionDiff.Add(string.Format("Labels DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentLabel.LabelDifference.Count));
                        optionDiff.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentLabel.LabelDifference.ForEach(i =>
                        {
                            optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(i.Locale, _connectionName1, i.Value1));
                            optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }

                if (!isDifferentDescription.IsEmpty)
                {
                    if (isDifferentDescription.LabelsOnlyIn1.Count > 0)
                    {
                        optionDiff.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName1, isDifferentDescription.LabelsOnlyIn1.Count));
                        optionDiff.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn1.ForEach(e => optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelsOnlyIn2.Count > 0)
                    {
                        optionDiff.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName2, isDifferentDescription.LabelsOnlyIn2.Count));
                        optionDiff.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn2.ForEach(e => optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelDifference.Count > 0)
                    {
                        optionDiff.Add(string.Format("Descriptions DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDescription.LabelDifference.Count));
                        optionDiff.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDescription.LabelDifference.ForEach(i =>
                        {
                            optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(i.Locale, _connectionName1, i.Value1));
                            optionDiff.Add(_tabSpacer + tableFormatter.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }
            }

            {
                var table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                table.AddLineIfNotEqual("DisplayOrder", displayOrder1, displayOrder2);
                table.AddLineIfNotEqual("Value", optionMetadata1.Value, optionMetadata2.Value);
                table.AddLineIfNotEqual("Color", optionMetadata1.Color, optionMetadata2.Color);
                //table.AddLineIfNotEqual("IsManaged", optionMetadata1.IsManaged, optionMetadata2.IsManaged);

                if (optionMetadata1.GetType().FullName != optionMetadata2.GetType().FullName)
                {
                    table.AddLine("Type", optionMetadata1.GetType().Name, optionMetadata2.GetType().Name);
                }
                else
                {
                    if (optionMetadata1 is StateOptionMetadata)
                    {
                        var optionState1 = optionMetadata1 as StateOptionMetadata;
                        var optionState2 = optionMetadata2 as StateOptionMetadata;

                        table.AddLineIfNotEqual("DefaultStatus", optionState1.DefaultStatus, optionState2.DefaultStatus);
                        table.AddLineIfNotEqual("InvariantName", optionState1.InvariantName, optionState2.InvariantName);
                    }

                    if (optionMetadata1 is StatusOptionMetadata)
                    {
                        var optionState1 = optionMetadata1 as StatusOptionMetadata;
                        var optionState2 = optionMetadata2 as StatusOptionMetadata;

                        table.AddLineIfNotEqual("State", optionState1.State, optionState2.State);
                        table.AddLineIfNotEqual("TransitionData", optionState1.TransitionData, optionState2.TransitionData);
                    }
                }

                if (table.Count > 0)
                {
                    optionDiff.AddRange(table.GetFormatedLines(true));
                }
            }

            return optionDiff;
        }
    }
}
