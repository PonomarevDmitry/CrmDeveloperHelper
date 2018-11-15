using Microsoft.Xrm.Sdk;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public abstract class CreateFileHandler : IDisposable
    {
        private const string _defaultTabSpacer = "    ";

        private StreamWriter _writer;

        protected readonly string _tabSpacer;
        private readonly bool _allDescriptions;

        private int _tabCount = 0;

        public CreateFileHandler(string tabSpacer, bool allDescriptions)
        {
            this._tabSpacer = tabSpacer;
            this._allDescriptions = allDescriptions;
        }

        private void ChangeTabCount(int count)
        {
            this._tabCount += count;
        }

        private void WriteSingleLine(string line = "")
        {
            if (!string.IsNullOrEmpty(line))
            {
                int decrease = line.TakeWhile(ch => ch == '}').Count();

                ChangeTabCount(-decrease);

                WriteTabs();
            }

            _writer.WriteLine(line);

            var skip = line.SkipWhile(ch => ch == '}');

            int tabChange = skip.Count(ch => ch == '{') - skip.Count(ch => ch == '}');

            ChangeTabCount(tabChange);
        }

        private void WriteSingle(string line = "")
        {
            if (!string.IsNullOrEmpty(line))
            {
                int decrease = line.TakeWhile(ch => ch == '}').Count();

                ChangeTabCount(-decrease);

                WriteTabs();
            }

            _writer.Write(line.TrimEnd());

            var skip = line.SkipWhile(ch => ch == '}');

            int tabChange = skip.Count(ch => ch == '{') - skip.Count(ch => ch == '}');

            ChangeTabCount(tabChange);
        }

        private void WriteTabs()
        {
            for (int i = 0; i < this._tabCount; i++)
            {
                _writer.Write(this._tabSpacer);
            }
        }

        protected void StartWriting(string fileFilePath)
        {
            this._writer = new StreamWriter(fileFilePath, false, new UTF8Encoding(false));
        }

        protected void EndWriting()
        {
            this._writer.Close();
        }

        protected void Write(string str)
        {
            IEnumerable<string> split = str
                .Split(new[] { "\r\n" }, StringSplitOptions.None)
                .SelectMany(s => s.Split(new[] { "\n\r" }, StringSplitOptions.None))
                .SelectMany(s => s.Split(new[] { "\r", "\n" }, StringSplitOptions.None))
                ;

            foreach (var item in split.Take(split.Count() - 1))
            {
                WriteSingleLine(item);
            }

            var last = split.Last();

            WriteSingle(last);
        }

        protected void WriteLine(string line, params object[] args)
        {
            var str = line;

            if (args != null && args.Any())
            {
                str = string.Format(line, args);
            }

            WriteLine(str);
        }

        protected void WriteLine(string line = "")
        {
            IEnumerable<string> split = line
                .Split(new[] { "\r\n" }, StringSplitOptions.None)
                .SelectMany(s => s.Split(new[] { "\n\r" }, StringSplitOptions.None))
                .SelectMany(s => s.Split(new[] { "\r", "\n" }, StringSplitOptions.None))
                ;

            foreach (var item in split)
            {
                WriteSingleLine(item);
            }
        }

        protected void WriteSummaryStrings(IEnumerable<string> listStrings)
        {
            if (!listStrings.Any())
            {
                return;
            }

            WriteLine("///<summary>");

            foreach (var item in listStrings)
            {
                WriteLine("/// {0}", item);
            }

            WriteLine("///</summary>");
        }

        protected void WriteSummary(Label displayName, Label description, IEnumerable<string> headers, IEnumerable<string> footers)
        {
            bool displayNameIsNull = displayName == null || displayName.LocalizedLabels.Count == 0;

            bool descriptionIsNull = description == null || description.LocalizedLabels.Count == 0;

            bool headersIsNull = headers == null || !headers.Any();

            bool footersIsNull = footers == null || !footers.Any();

            if (displayNameIsNull && descriptionIsNull && headersIsNull && footersIsNull)
            {
                return;
            }

            List<string> listStrings = new List<string>();

            if (!headersIsNull)
            {
                listStrings.AddRange(headers);
            }

            CreateFileHandler.FillLabelDisplayNameAndDescription(listStrings, _allDescriptions, displayName, description, _tabSpacer);

            if (!footersIsNull)
            {
                if (listStrings.Count > 0)
                {
                    listStrings.Add(string.Empty);
                }

                listStrings.AddRange(footers);
            }

            WriteSummaryStrings(listStrings);
        }

        private static readonly Regex rexReplaceNonAlpha = new Regex(@"[^a-zA-Z_0-9]", RegexOptions.Compiled);
        private static readonly Regex rexReplaceDoubleSpace = new Regex(@"_+", RegexOptions.Compiled);

        //private static readonly Regex rexLiteral = new Regex(@"[^a-zA-Z0-9_]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static void FillLabelDisplayNameAndDescription(List<string> listStrings, bool allDescriptions, Label displayName, Label description, string tabSpacer = _defaultTabSpacer)
        {
            if (allDescriptions)
            {
                FillLabelDisplayNameAndDescriptionAll(listStrings, displayName, description, tabSpacer);
            }
            else
            {
                FillLabelDisplayNameAndDescriptionOnlyFirst(listStrings, displayName, description, tabSpacer);
            }
        }

        private static void FillLabelDisplayNameAndDescriptionAll(List<string> listStrings, Label displayName, Label description, string tabSpacer)
        {
            List<int> listLocale = new List<int>();

            if (displayName != null && displayName.LocalizedLabels != null)
            {
                foreach (var item in displayName.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            if (description != null && description.LocalizedLabels != null)
            {
                foreach (var item in description.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            if (listLocale.Any())
            {
                HashSet<string> hashShowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (displayName != null && displayName.LocalizedLabels != null)
                {
                    var sortedDisplay = displayName.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label) && !hashShowed.Contains(l.Label));

                    if (sortedDisplay.Any())
                    {
                        if (listStrings.Count > 0)
                        {
                            listStrings.Add(string.Empty);
                        }

                        listStrings.Add("DisplayName:");

                        foreach (var localeId in listLocale)
                        {
                            var locLabel = sortedDisplay.FirstOrDefault(l => l.LanguageCode == localeId);

                            if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                            {
                                var str = string.Format("({0}): {1}", LanguageLocale.GetLocaleName(localeId), locLabel.Label);

                                listStrings.Add(tabSpacer + str);

                                hashShowed.Add(locLabel.Label);
                            }
                        }
                    }
                }

                if (description != null && description.LocalizedLabels != null)
                {
                    var sortedDescription = description.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label) && !hashShowed.Contains(l.Label));

                    if (sortedDescription.Any())
                    {
                        if (listStrings.Count > 0)
                        {
                            listStrings.Add(string.Empty);
                        }

                        listStrings.Add("Description:");

                        foreach (var localeId in listLocale)
                        {
                            var locLabel = sortedDescription.FirstOrDefault(l => l.LanguageCode == localeId);

                            if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                            {
                                string[] split = string.Format("({0}): {1}", LanguageLocale.GetLocaleName(localeId), locLabel.Label).Split(new string[] { "\r", "\n" }, StringSplitOptions.None);

                                foreach (var item in split)
                                {
                                    listStrings.Add(tabSpacer + item);
                                }

                                hashShowed.Add(locLabel.Label);
                            }
                        }
                    }
                }
            }
        }

        private static void FillLabelDisplayNameAndDescriptionOnlyFirst(List<string> listStrings, Label displayName, Label description, string tabSpacer)
        {
            List<int> listLocale = new List<int>();

            if (displayName != null && displayName.LocalizedLabels != null)
            {
                foreach (var item in displayName.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            var localeId = listLocale.FirstOrDefault();

            if (localeId != default(int))
            {
                if (listStrings.Count > 0)
                {
                    listStrings.Add(string.Empty);
                }

                listStrings.Add(string.Format("({0}):", LanguageLocale.GetLocaleName(localeId)));

                HashSet<string> hashShowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (displayName != null && displayName.LocalizedLabels != null)
                {
                    var locLabel = displayName.LocalizedLabels.FirstOrDefault(lbl => lbl.LanguageCode == localeId && !hashShowed.Contains(lbl.Label));

                    if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                    {
                        string[] split = string.Format("DisplayName: {0}", locLabel.Label).Split(new string[] { "\r", "\n" }, StringSplitOptions.None);

                        foreach (var item in split)
                        {
                            listStrings.Add(tabSpacer + item);
                        }

                        hashShowed.Add(locLabel.Label);
                    }
                }

                if (description != null && description.LocalizedLabels != null)
                {
                    var locLabel = description.LocalizedLabels.FirstOrDefault(lbl => lbl.LanguageCode == localeId && !hashShowed.Contains(lbl.Label));

                    if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                    {
                        string[] split = string.Format("Description: {0}", locLabel.Label).Split(new string[] { "\r", "\n" }, StringSplitOptions.None);

                        foreach (var item in split)
                        {
                            listStrings.Add(tabSpacer + item);
                        }

                        hashShowed.Add(locLabel.Label);
                    }
                }
            }
        }

        public static List<string> GetAttributeDescription(AttributeMetadata attrib, bool allDescription, bool withManagedInfo, SolutionComponentDescriptor descriptor, string tabSpacer = _defaultTabSpacer)
        {
            List<string> result = new List<string>();

            AddStringIntoList(result, tabSpacer, string.Format("SchemaName: {0}", attrib.SchemaName));

            {
                var list = new List<string>();

                string attributeTypeName = string.Empty;
                string attrType = string.Empty;
                string requiredLevel = string.Empty;
                string attributeOf = string.Empty;

                if (attrib.AttributeType != null)
                {
                    attrType = string.Format("AttributeType: {0}", attrib.AttributeType);
                }

                if (attrib.AttributeTypeName != null && !string.IsNullOrEmpty(attrib.AttributeTypeName.Value))
                {
                    attributeTypeName = string.Format("AttributeTypeName: {0}", attrib.AttributeTypeName.Value);
                }

                if (attrib.RequiredLevel != null)
                {
                    requiredLevel = string.Format("RequiredLevel: {0}", attrib.RequiredLevel.Value);
                }

                if (!string.IsNullOrEmpty(attrib.AttributeOf))
                {
                    attributeOf = string.Format("AttributeOf '{0}'", attrib.AttributeOf);
                }

                string attributeType = attrib.GetType().Name;

                list.AddRange(new[] { attributeType, attrType, attributeTypeName, requiredLevel, attributeOf });

                if (withManagedInfo)
                {
                    string isManaged = string.Format("IsManaged {0}", attrib.IsManaged.ToString());

                    list.Add(isManaged);
                }

                AddStringIntoList(result, tabSpacer, list);
            }

            {
                var isValidForCreate = string.Format("IsValidForCreate: {0}", attrib.IsValidForCreate.GetValueOrDefault());
                var isValidForRead = string.Format("IsValidForRead: {0}", attrib.IsValidForRead.GetValueOrDefault());
                var isValidForUpdate = string.Format("IsValidForUpdate: {0}", attrib.IsValidForUpdate.GetValueOrDefault());

                var isValidForAdvancedFind = string.Format("IsValidForAdvancedFind: {0}", attrib.IsValidForAdvancedFind.Value);

                AddStringIntoList(result, tabSpacer, isValidForCreate, isValidForRead, isValidForUpdate, isValidForAdvancedFind);
            }

            {
                var isLogical = string.Format("IsLogical: {0}", attrib.IsLogical.GetValueOrDefault());
                var isSecured = string.Format("IsSecured: {0}", attrib.IsSecured.GetValueOrDefault());
                var isCustomAttribute = string.Format("IsCustomAttribute: {0}", attrib.IsCustomAttribute.GetValueOrDefault());
                var sourceType = string.Empty;

                if (attrib.SourceType == null)
                {
                    sourceType = "Simple";
                }
                else if (attrib.SourceType == 1)
                {
                    sourceType = "Calculated";
                }
                else if (attrib.SourceType == 2)
                {
                    sourceType = "Rollup";
                }
                else
                {
                    sourceType = attrib.SourceType.ToString();
                }

                sourceType = string.Format("SourceType: {0}", sourceType);

                AddStringIntoList(result, tabSpacer, isLogical, isSecured, isCustomAttribute, sourceType);
            }

            if (attrib is MemoAttributeMetadata memoAttrib)
            {
                string maxLength = string.Format("MaxLength = {0}", memoAttrib.MaxLength.HasValue ? memoAttrib.MaxLength.ToString() : "Null");
                string format = string.Format("Format = {0}", memoAttrib.Format.HasValue ? memoAttrib.Format.ToString() : "Null");
                string imeMode = string.Format("ImeMode = {0}", memoAttrib.ImeMode.HasValue ? memoAttrib.ImeMode.ToString() : "Null");
                string isLocalizable = string.Format("IsLocalizable = {0}", memoAttrib.IsLocalizable.HasValue ? memoAttrib.IsLocalizable.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, maxLength);
                AddStringIntoList(result, tabSpacer, format, imeMode, isLocalizable);
            }

            if (attrib is StringAttributeMetadata stringAttrib)
            {
                string maxLength = string.Format("MaxLength = {0}", stringAttrib.MaxLength.HasValue ? stringAttrib.MaxLength.ToString() : "Null");
                string format = string.Format("Format = {0}", stringAttrib.Format.HasValue ? stringAttrib.Format.ToString() : "Null");
                string imeMode = string.Format("ImeMode = {0}", stringAttrib.ImeMode.HasValue ? stringAttrib.ImeMode.ToString() : "Null");
                string isLocalizable = string.Format("IsLocalizable = {0}", stringAttrib.IsLocalizable.HasValue ? stringAttrib.IsLocalizable.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(stringAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, maxLength);
                AddStringIntoList(result, tabSpacer, format, imeMode, isLocalizable, formulaDefinition);
            }

            if (attrib is IntegerAttributeMetadata intAttrib)
            {
                string minValue = string.Format("MinValue = {0}", intAttrib.MinValue.HasValue ? intAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", intAttrib.MaxValue.HasValue ? intAttrib.MaxValue.ToString() : "Null");

                string format = string.Format("Format = {0}", intAttrib.Format.HasValue ? intAttrib.Format.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(intAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, minValue, maxValue);
                AddStringIntoList(result, tabSpacer, format, formulaDefinition);
            }

            if (attrib is BigIntAttributeMetadata bigIntAttrib)
            {
                string minValue = string.Format("MinValue = {0}", bigIntAttrib.MinValue.HasValue ? bigIntAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", bigIntAttrib.MaxValue.HasValue ? bigIntAttrib.MaxValue.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, minValue, maxValue);
            }

            if (attrib is ImageAttributeMetadata imageAttrib)
            {
                string isPrimaryImage = string.Format("IsPrimaryImage = {0}", imageAttrib.IsPrimaryImage.HasValue ? imageAttrib.IsPrimaryImage.ToString() : "Null");
                string maxHeight = string.Format("MaxHeight = {0}", imageAttrib.MaxHeight.HasValue ? imageAttrib.MaxHeight.ToString() : "Null");
                string maxValue = string.Format("MaxWidth = {0}", imageAttrib.MaxWidth.HasValue ? imageAttrib.MaxWidth.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, isPrimaryImage, maxHeight, maxValue);
            }

            if (attrib is MoneyAttributeMetadata moneyAttrib)
            {
                string minValue = string.Format("MinValue = {0}", moneyAttrib.MinValue.HasValue ? moneyAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", moneyAttrib.MaxValue.HasValue ? moneyAttrib.MaxValue.ToString() : "Null");

                string precision = string.Format("Precision = {0}", moneyAttrib.Precision.HasValue ? moneyAttrib.Precision.ToString() : "Null");
                string precisionSource = string.Format("PrecisionSource = {0}", moneyAttrib.PrecisionSource.HasValue ? moneyAttrib.PrecisionSource.ToString() : "Null");

                string isBaseCurrency = string.Format("IsBaseCurrency = {0}", moneyAttrib.IsBaseCurrency.HasValue ? moneyAttrib.IsBaseCurrency.ToString() : "Null");

                string imeMode = string.Format("ImeMode = {0}", moneyAttrib.ImeMode.HasValue ? moneyAttrib.ImeMode.ToString() : "Null");

                string formulaDefinition = string.Empty;
                string calculationOf = string.Empty;

                if (!string.IsNullOrEmpty(moneyAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                if (!string.IsNullOrEmpty(moneyAttrib.CalculationOf))
                {
                    calculationOf = string.Format("CalculationOf = {0}", moneyAttrib.CalculationOf);
                }

                AddStringIntoList(result, tabSpacer, minValue, maxValue, precision, precisionSource);
                AddStringIntoList(result, tabSpacer, isBaseCurrency);
                AddStringIntoList(result, tabSpacer, imeMode, formulaDefinition, calculationOf);
            }

            if (attrib is DecimalAttributeMetadata decimalAttrib)
            {
                string minValue = string.Format("MinValue = {0}", decimalAttrib.MinValue.HasValue ? decimalAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", decimalAttrib.MaxValue.HasValue ? decimalAttrib.MaxValue.ToString() : "Null");

                string precision = string.Format("Precision = {0}", decimalAttrib.Precision.HasValue ? decimalAttrib.Precision.ToString() : "Null");

                string imeMode = string.Format("ImeMode = {0}", decimalAttrib.ImeMode.HasValue ? decimalAttrib.ImeMode.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(decimalAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, minValue, maxValue, precision);
                AddStringIntoList(result, tabSpacer, imeMode, formulaDefinition);
            }

            if (attrib is DoubleAttributeMetadata doubleAttrib)
            {
                string minValue = string.Format("MinValue = {0}", doubleAttrib.MinValue.HasValue ? doubleAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", doubleAttrib.MaxValue.HasValue ? doubleAttrib.MaxValue.ToString() : "Null");

                string precision = string.Format("Precision = {0}", doubleAttrib.Precision.HasValue ? doubleAttrib.Precision.ToString() : "Null");

                string imeMode = string.Format("ImeMode = {0}", doubleAttrib.ImeMode.HasValue ? doubleAttrib.ImeMode.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, minValue, maxValue, precision);
                AddStringIntoList(result, tabSpacer, imeMode);
            }

            if (attrib is BooleanAttributeMetadata boolAttrib)
            {
                result.Add(string.Format("DefaultValue = {0}", boolAttrib.DefaultValue.HasValue ? boolAttrib.DefaultValue.ToString() : "Null"));

                if (boolAttrib.OptionSet.FalseOption.Label != null || boolAttrib.OptionSet.FalseOption.Description != null)
                {
                    CreateFileHandler.FillLabelDisplayNameAndDescription(result, allDescription, boolAttrib.OptionSet.FalseOption.Label, boolAttrib.OptionSet.FalseOption.Description, tabSpacer);
                    result.Add(string.Format("FalseOption = {0}", boolAttrib.OptionSet.FalseOption.Value.HasValue ? boolAttrib.OptionSet.FalseOption.Value.Value.ToString() : "Null"));
                }

                if (boolAttrib.OptionSet.TrueOption.Label != null || boolAttrib.OptionSet.TrueOption.Description != null)
                {
                    CreateFileHandler.FillLabelDisplayNameAndDescription(result, allDescription, boolAttrib.OptionSet.TrueOption.Label, boolAttrib.OptionSet.TrueOption.Description, tabSpacer);
                    result.Add(string.Format("TrueOption = {0}", boolAttrib.OptionSet.TrueOption.Value.HasValue ? boolAttrib.OptionSet.TrueOption.Value.Value.ToString() : "Null"));
                }

                if (!string.IsNullOrEmpty(boolAttrib.FormulaDefinition))
                {
                    result.Add("FormulaDefinition is not null");
                }
            }

            if (attrib is PicklistAttributeMetadata picklistAttrib)
            {
                string managedStr = string.Empty;

                if (withManagedInfo)
                {
                    managedStr = " " + (picklistAttrib.OptionSet.IsManaged.GetValueOrDefault() ? "Managed" : "Unmanaged");
                }

                string temp = string.Format("{0} {1} {2} OptionSet {3}"
                      , picklistAttrib.OptionSet.IsGlobal.GetValueOrDefault() ? "Global" : "Local"
                      , picklistAttrib.OptionSet.IsCustomOptionSet.GetValueOrDefault() ? "Custom" : "System"
                      , managedStr
                      , picklistAttrib.OptionSet.Name
                );

                AddStringIntoList(result, tabSpacer, temp);

                string defaultFormValue = string.Format("DefaultFormValue = {0}", picklistAttrib.DefaultFormValue.HasValue ? picklistAttrib.DefaultFormValue.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, defaultFormValue);

                if (!string.IsNullOrEmpty(picklistAttrib.FormulaDefinition))
                {
                    AddStringIntoList(result, tabSpacer, "FormulaDefinition is not null");
                }

                if (picklistAttrib.OptionSet.IsGlobal.GetValueOrDefault())
                {
                    List<string> lineOptionSetDescription = new List<string>();

                    FillLabelDisplayNameAndDescription(lineOptionSetDescription, allDescription, picklistAttrib.OptionSet.DisplayName, picklistAttrib.OptionSet.Description, tabSpacer);

                    if (lineOptionSetDescription.Any())
                    {
                        if (result.Any())
                        {
                            result.Add(string.Empty);
                        }

                        lineOptionSetDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                    }
                }
            }

            if (attrib is StatusAttributeMetadata statusAttrib)
            {
                string defaultFormValue = string.Format("DefaultFormValue = {0}", statusAttrib.DefaultFormValue.HasValue ? statusAttrib.DefaultFormValue.ToString() : "Null");
                AddStringIntoList(result, tabSpacer, defaultFormValue);
            }

            if (attrib is StateAttributeMetadata stateAttrib)
            {
                string defaultFormValue = string.Format("DefaultFormValue = {0}", stateAttrib.DefaultFormValue.HasValue ? stateAttrib.DefaultFormValue.ToString() : "Null");
                AddStringIntoList(result, tabSpacer, defaultFormValue);
            }

            if (attrib is EntityNameAttributeMetadata entityNameAttrib)
            {
                if (entityNameAttrib.OptionSet != null)
                {
                    string managedStr = string.Empty;

                    if (withManagedInfo)
                    {
                        managedStr = " " + (entityNameAttrib.OptionSet.IsManaged.GetValueOrDefault() ? "Managed" : "Unmanaged");
                    }

                    string temp = string.Format("{0} {1} {2} OptionSet {3}"
                        , entityNameAttrib.OptionSet.IsGlobal.GetValueOrDefault() ? "Global" : "Local"
                        , entityNameAttrib.OptionSet.IsCustomOptionSet.GetValueOrDefault() ? "Custom" : "System"
                        , managedStr
                        , entityNameAttrib.OptionSet.Name
                    );

                    AddStringIntoList(result, tabSpacer, temp);

                    string defaultFormValue = string.Format("DefaultFormValue = {0}", entityNameAttrib.DefaultFormValue.HasValue ? entityNameAttrib.DefaultFormValue.ToString() : "Null");

                    AddStringIntoList(result, tabSpacer, defaultFormValue);

                    if (entityNameAttrib.OptionSet.IsGlobal.GetValueOrDefault())
                    {
                        List<string> lineOptionSetDescription = new List<string>();

                        FillLabelDisplayNameAndDescription(lineOptionSetDescription, allDescription, entityNameAttrib.OptionSet.DisplayName, entityNameAttrib.OptionSet.Description, tabSpacer);

                        if (lineOptionSetDescription.Any())
                        {
                            if (result.Any())
                            {
                                result.Add(string.Empty);
                            }

                            lineOptionSetDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                        }
                    }
                }
            }

            //{
            //    var uniqueIdentifierAttrib = (attrib as UniqueIdentifierAttributeMetadata);
            //    if (uniqueIdentifierAttrib != null)
            //    {

            //    }
            //}

            if (attrib is ManagedPropertyAttributeMetadata managedAttrib)
            {

            }

            if (attrib is LookupAttributeMetadata lookupAttrib)
            {
                if (lookupAttrib.Targets != null && lookupAttrib.Targets.Length > 0)
                {
                    string targets = string.Format("Targets: {0}", string.Join(",", lookupAttrib.Targets.OrderBy(s => s)));
                    AddStringIntoList(result, tabSpacer, targets);

                    if (lookupAttrib.Targets.Length <= 6)
                    {
                        foreach (var target in lookupAttrib.Targets)
                        {
                            var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(target, new[] { "LogicalName", "DisplayName", "DisplayCollectionName", "Description", "PrimaryIdAttribute", "PrimaryNameAttribute" });

                            if (entityMetadata != null)
                            {
                                List<string> lineEntityDescription = new List<string>();

                                FillLabelEntity(lineEntityDescription, allDescription, entityMetadata.DisplayName, entityMetadata.DisplayCollectionName, entityMetadata.Description);

                                if (lineEntityDescription.Any())
                                {
                                    if (result.Any())
                                    {
                                        result.Add(string.Empty);
                                    }

                                    result.Add(tabSpacer
                                        + string.Format("Target {0}", target)
                                        + tabSpacer
                                        + string.Format("PrimaryIdAttribute {0}", entityMetadata.PrimaryIdAttribute)
                                        + (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute) ? tabSpacer + string.Format("PrimaryNameAttribute {0}", entityMetadata.PrimaryNameAttribute) : string.Empty)
                                        );

                                    lineEntityDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                                }
                            }
                        }
                    }
                }
            }

            if (attrib is DateTimeAttributeMetadata dateTimeAttrib)
            {
                string dateTimeBehavior = string.Empty;
                string canChangeDateTimeBehavior = string.Empty;

                if (dateTimeAttrib.DateTimeBehavior != null)
                {
                    dateTimeBehavior = string.Format("DateTimeBehavior = {0}", dateTimeAttrib.DateTimeBehavior.Value);
                }

                if (dateTimeAttrib.CanChangeDateTimeBehavior != null)
                {
                    canChangeDateTimeBehavior = string.Format("CanChangeDateTimeBehavior = {0}", dateTimeAttrib.CanChangeDateTimeBehavior.Value);
                }

                string format = string.Format("Format = {0}", dateTimeAttrib.Format.HasValue ? dateTimeAttrib.Format.ToString() : "Null");
                string imeMode = string.Format("ImeMode = {0}", dateTimeAttrib.ImeMode.HasValue ? dateTimeAttrib.ImeMode.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(dateTimeAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, dateTimeBehavior, canChangeDateTimeBehavior);

                AddStringIntoList(result, tabSpacer, imeMode, format, formulaDefinition);
            }

            return result;
        }

        public static void FillLabelEntity(List<string> summary, bool alldescriptions, Label displayName, Label displayCollectionName, Label description, string tabSpacer = _defaultTabSpacer)
        {
            if (alldescriptions)
            {
                FillLabelEntityAll(summary, displayName, displayCollectionName, description, tabSpacer);
            }
            else
            {
                FillLabelEntityOnlyFirst(summary, displayName, displayCollectionName, description, tabSpacer);
            }
        }

        private static void FillLabelEntityAll(List<string> listStrings, Label displayName, Label displayCollectionName, Label description, string tabSpacer)
        {
            List<int> listLocale = new List<int>();

            if (displayName != null && displayName.LocalizedLabels != null)
            {
                foreach (var item in displayName.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            if (description != null && description.LocalizedLabels != null)
            {
                foreach (var item in description.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            if (displayCollectionName != null && displayCollectionName.LocalizedLabels != null)
            {
                foreach (var item in displayCollectionName.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            if (listLocale.Any())
            {
                HashSet<string> hashShowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (displayName != null && displayName.LocalizedLabels != null)
                {
                    var sortedDisplay = displayName.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label) && !hashShowed.Contains(l.Label));

                    if (sortedDisplay.Any())
                    {
                        if (listStrings.Count > 0)
                        {
                            listStrings.Add(string.Empty);
                        }

                        listStrings.Add("DisplayName:");

                        foreach (var localeId in listLocale)
                        {
                            var locLabel = sortedDisplay.FirstOrDefault(l => l.LanguageCode == localeId);

                            if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                            {
                                var str = string.Format("({0}): {1}", LanguageLocale.GetLocaleName(localeId), locLabel.Label);

                                listStrings.Add(tabSpacer + str);

                                hashShowed.Add(locLabel.Label);
                            }
                        }
                    }
                }

                if (displayCollectionName != null && displayCollectionName.LocalizedLabels != null)
                {
                    var sortedDisplayCollectionName = displayCollectionName.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label) && !hashShowed.Contains(l.Label));

                    if (sortedDisplayCollectionName.Any())
                    {
                        if (listStrings.Count > 0)
                        {
                            listStrings.Add(string.Empty);
                        }

                        listStrings.Add("DisplayCollectionName:");

                        foreach (var localeId in listLocale)
                        {
                            var locLabel = sortedDisplayCollectionName.FirstOrDefault(l => l.LanguageCode == localeId);

                            if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                            {
                                string[] split = string.Format("({0}): {1}", LanguageLocale.GetLocaleName(localeId), locLabel.Label).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                                foreach (var item in split)
                                {
                                    listStrings.Add(tabSpacer + item);
                                }

                                hashShowed.Add(locLabel.Label);
                            }
                        }
                    }
                }

                if (description != null && description.LocalizedLabels != null)
                {
                    var sortedDescription = description.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label) && !hashShowed.Contains(l.Label));

                    if (sortedDescription.Any())
                    {
                        if (listStrings.Count > 0)
                        {
                            listStrings.Add(string.Empty);
                        }

                        listStrings.Add("Description:");

                        foreach (var localeId in listLocale)
                        {
                            var locLabel = sortedDescription.FirstOrDefault(l => l.LanguageCode == localeId);

                            if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                            {
                                string[] split = string.Format("({0}): {1}", LanguageLocale.GetLocaleName(localeId), locLabel.Label).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                                foreach (var item in split)
                                {
                                    listStrings.Add(tabSpacer + item);
                                }

                                hashShowed.Add(locLabel.Label);
                            }
                        }
                    }
                }
            }
        }

        private static void FillLabelEntityOnlyFirst(List<string> listStrings, Label displayName, Label displayCollectionName, Label description, string tabSpacer)
        {
            List<int> listLocale = new List<int>();

            if (displayName != null && displayName.LocalizedLabels != null)
            {
                foreach (var item in displayName.LocalizedLabels)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        if (!listLocale.Contains(item.LanguageCode))
                        {
                            listLocale.Add(item.LanguageCode);
                        }
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            var localeId = listLocale.FirstOrDefault();

            if (localeId != default(int))
            {
                if (listStrings.Count > 0)
                {
                    listStrings.Add(string.Empty);
                }

                listStrings.Add(string.Format("({0}):", LanguageLocale.GetLocaleName(localeId)));

                HashSet<string> hashShowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (displayName != null && displayName.LocalizedLabels != null)
                {
                    var locLabel = displayName.LocalizedLabels.FirstOrDefault(lbl => lbl.LanguageCode == localeId && !hashShowed.Contains(lbl.Label));

                    if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                    {
                        string[] split = string.Format("DisplayName: {0}", locLabel.Label).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        foreach (var item in split)
                        {
                            listStrings.Add(tabSpacer + item);
                        }

                        hashShowed.Add(locLabel.Label);
                    }
                }

                if (displayCollectionName != null && displayCollectionName.LocalizedLabels != null)
                {
                    var locLabel = displayCollectionName.LocalizedLabels.FirstOrDefault(lbl => lbl.LanguageCode == localeId && !hashShowed.Contains(lbl.Label));

                    if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                    {
                        string[] split = string.Format("DisplayCollectionName: {0}", locLabel.Label).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        foreach (var item in split)
                        {
                            listStrings.Add(tabSpacer + item);
                        }

                        hashShowed.Add(locLabel.Label);
                    }
                }

                if (description != null && description.LocalizedLabels != null)
                {
                    var locLabel = description.LocalizedLabels.FirstOrDefault(lbl => lbl.LanguageCode == localeId && !hashShowed.Contains(lbl.Label));

                    if (locLabel != null && !string.IsNullOrEmpty(locLabel.Label) && !hashShowed.Contains(locLabel.Label))
                    {
                        string[] split = string.Format("Description: {0}", locLabel.Label).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        foreach (var item in split)
                        {
                            listStrings.Add(tabSpacer + item);
                        }

                        hashShowed.Add(locLabel.Label);
                    }
                }
            }
        }

        private static void AddStringIntoList(List<string> result, string separator, params string[] lines)
        {
            var filtered = lines.Where(s => !string.IsNullOrEmpty(s));

            if (filtered.Any())
            {
                result.Add(string.Join(separator, filtered));
            }
        }

        private static void AddStringIntoList(List<string> result, string separator, IEnumerable<string> lines)
        {
            var filtered = lines.Where(s => !string.IsNullOrEmpty(s));

            if (filtered.Any())
            {
                result.Add(string.Join(separator, filtered));
            }
        }

        public static bool IgnoreAttribute(string entityName, string attributeName)
        {
            if (entityName.Equals("connection", StringComparison.OrdinalIgnoreCase))
            {
                if (attributeName.Equals("record1objecttypecode", StringComparison.OrdinalIgnoreCase)
                    || attributeName.Equals("record2objecttypecode", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("importmap", StringComparison.OrdinalIgnoreCase))
            {
                if (attributeName.Equals("targetentity", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("queueitem", StringComparison.OrdinalIgnoreCase)
                || entityName.Equals("sla", StringComparison.OrdinalIgnoreCase)
                )
            {
                if (attributeName.Equals("objecttypecode", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("duplicaterule", StringComparison.OrdinalIgnoreCase))
            {
                if (attributeName.Equals("baseentitytypecode", StringComparison.OrdinalIgnoreCase)
                    || attributeName.Equals("matchingentitytypecode", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("similarityrule", StringComparison.OrdinalIgnoreCase))
            {
                if (attributeName.Equals("baseentitytypecode", StringComparison.OrdinalIgnoreCase)
                    || attributeName.Equals("matchingentitytypecode", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            return false;
        }

        //private static string CleanLiteralName(string fn)
        //{
        //    var t0 = RemoveDiacritics(fn);
        //    t0 = rexLiteral.Replace(t0, string.Empty);
        //    return t0;
        //}

        private static string[] _lat_up_ =
        {
            "A", "B", "V", "G", "D", "E", "E", "Zh"
            , "Z", "I", "I", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh"
            , "Ts", "Ch", "Sh", "Shch", "Ie"
            , "Y", "", "E", "Iu", "Ia"
        };

        private static string[] _lat_low =
        {
            "a", "b", "v", "g", "d", "e", "e", "zh"
            , "z", "i", "i", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh"
            , "ts", "ch", "sh", "shch", "ie"
            , "y", "", "e", "iu", "ia"
        };

        private static string[] _rus_up_ =
        {
            "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж"
            , "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х"
            , "Ц", "Ч", "Ш", "Щ", "Ъ"
            , "Ы", "Ь", "Э", "Ю", "Я"
        };

        private static string[] _rus_low =
        {
            "а", "б", "в", "г", "д", "е", "ё", "ж"
            , "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х"
            , "ц", "ч", "ш", "щ", "ъ"
            , "ы", "ь", "э", "ю", "я"
        };

        private static string Translit(string str)
        {
            StringBuilder result = new StringBuilder(str);

            for (int i = 0; i <= 32; i++)
            {
                result
                    .Replace(_rus_up_[i], _lat_up_[i])
                    .Replace(_rus_low[i], _lat_low[i]);
            }

            return result.ToString();
        }

        public static string GetOptionSetValueName(string label, int value)
        {
            var result = ContentCoparerHelper.RemoveDiacritics(label);

            result = Translit(result);

            result = rexReplaceNonAlpha.Replace(result, "_");
            result = rexReplaceDoubleSpace.Replace(result, "_");
            result = result.Trim(' ', '_');

            if (string.IsNullOrEmpty(result))
            {
                return "V_" + value;
            }

            if (int.TryParse(result.Substring(0, 1), out _))
            {
                result = "V_" + result;
            }

            result += "_" + value;

            result = rexReplaceNonAlpha.Replace(result, "_");
            result = rexReplaceDoubleSpace.Replace(result, "_");
            result = result.Trim(' ', '_');

            return result;
        }

        public static bool IgnoreGlobalOptionSet(string name)
        {
            return string.Equals(name, "activitypointer_activitytypecode", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetLocalizedLabel(Label label)
        {
            if (label == null)
            {
                return string.Empty;
            }

            List<int> listLocale = new List<int>();

            foreach (var item in label.LocalizedLabels)
            {
                if (!string.IsNullOrEmpty(item.Label))
                {
                    if (!listLocale.Contains(item.LanguageCode))
                    {
                        listLocale.Add(item.LanguageCode);
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            var localeId = listLocale.FirstOrDefault();

            if (localeId != default(int))
            {
                var locLabel = label.LocalizedLabels.FirstOrDefault(lbl => lbl.LanguageCode == localeId);

                if (locLabel != null)
                {
                    if (!string.IsNullOrEmpty(locLabel.Label))
                    {
                        return locLabel.Label;
                    }
                }
            }

            return string.Empty;
        }

        public static string GetLocalizedLabel(List<Tuple<int, string>> lables)
        {
            if (lables == null || !lables.Any())
            {
                return string.Empty;
            }

            List<int> listLocale = new List<int>();

            foreach (var item in lables)
            {
                if (!string.IsNullOrEmpty(item.Item2))
                {
                    if (!listLocale.Contains(item.Item1))
                    {
                        listLocale.Add(item.Item1);
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            var localeId = listLocale.FirstOrDefault();

            if (localeId != default(int))
            {
                var locLabel = lables.FirstOrDefault(lbl => lbl.Item1 == localeId);

                if (locLabel != null)
                {
                    if (!string.IsNullOrEmpty(locLabel.Item2))
                    {
                        return locLabel.Item2;
                    }
                }
            }

            return string.Empty;
        }

        public static List<OptionItem> GetOptionItems(string entityName, string attributeName, OptionSetMetadata optionSet, List<StringMap> listStringMap)
        {
            var options = new List<OptionItem>();

            // Формируем атрибуты
            foreach (OptionMetadata optionValue in optionSet.Options)
            {
                if (!optionValue.Value.HasValue)
                {
                    continue;
                }

                string optionSetLabel = CreateFileHandler.GetLocalizedLabel(optionValue.Label);

                var optionSetValueName = CreateFileHandler.GetOptionSetValueName(optionSetLabel, optionValue.Value.Value);

                if (!string.IsNullOrEmpty(optionSetValueName))
                {
                    int? displayOrder = null;

                    if (listStringMap != null && !string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(attributeName))
                    {
                        var stringMap = listStringMap.FirstOrDefault(e =>
                            string.Equals(e.AttributeName, attributeName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(e.ObjectTypeCode, entityName, StringComparison.OrdinalIgnoreCase)
                            && e.AttributeValue == optionValue.Value.Value
                            );

                        if (stringMap != null)
                        {
                            displayOrder = stringMap.DisplayOrder;
                        }
                    }

                    var optionItem = new OptionItem()
                    {
                        FieldName = optionSetValueName,
                        Value = optionValue.Value.Value,

                        Label = optionValue.Label,
                        Description = optionValue.Description,

                        DisplayOrder = displayOrder,

                        OptionMetadata = optionValue,
                    };

                    options.Add(optionItem);
                }
            }

            if (options.All(o => o.DisplayOrder.HasValue))
            {
                options = options.OrderBy(op => op.DisplayOrder).ToList();
            }
            else
            {
                options = options.OrderBy(op => op.Value).ToList();
            }

            return options;
        }

        public static List<OptionItem> GetStateOptionItems(StatusAttributeMetadata statusAttr, StateAttributeMetadata stateAttr, List<StringMap> listStringMap)
        {
            var options = new List<OptionItem>();

            foreach (StateOptionMetadata optionValue in stateAttr.OptionSet.Options)
            {
                if (!optionValue.Value.HasValue)
                {
                    continue;
                }

                var defaultStatusCodeName = "Null";

                if (optionValue.DefaultStatus.HasValue)
                {
                    var statusOptionValue = statusAttr.OptionSet.Options.FirstOrDefault(op => op.Value.Value == optionValue.DefaultStatus.Value);

                    if (statusOptionValue != null)
                    {
                        string statusLabel = CreateFileHandler.GetLocalizedLabel(statusOptionValue.Label);

                        defaultStatusCodeName = CreateFileHandler.GetOptionSetValueName(statusLabel, statusOptionValue.Value.Value);
                    }
                    else
                    {
                        defaultStatusCodeName = string.Format("Not finded: {0}", optionValue.DefaultStatus.ToString());
                    }
                }

                var stateName = GetStateName(optionValue);

                if (!string.IsNullOrEmpty(stateName))
                {
                    int? displayOrder = null;

                    if (listStringMap != null)
                    {
                        var stringMap = listStringMap.FirstOrDefault(e =>
                            string.Equals(e.AttributeName, stateAttr.LogicalName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(e.ObjectTypeCode, stateAttr.EntityLogicalName, StringComparison.OrdinalIgnoreCase)
                            && e.AttributeValue == optionValue.Value.Value
                            );

                        if (stringMap != null)
                        {
                            displayOrder = stringMap.DisplayOrder;
                        }
                    }

                    var optionItem = new OptionItem()
                    {
                        FieldName = stateName,
                        Value = optionValue.Value.Value,

                        Label = optionValue.Label,
                        Description = optionValue.Description,

                        DefaultStatusCode = optionValue.DefaultStatus,
                        DefaultStatusCodeName = defaultStatusCodeName,

                        InvariantName = optionValue.InvariantName,

                        DisplayOrder = displayOrder,

                        OptionMetadata = optionValue,
                    };

                    options.Add(optionItem);
                }
            }

            options = options.OrderBy(op => op.DisplayOrder).ToList();

            return options;
        }

        private static string GetStateName(StateOptionMetadata optionValue)
        {
            string stateLabel = optionValue.InvariantName;

            if (string.IsNullOrEmpty(stateLabel))
            {
                stateLabel = CreateFileHandler.GetLocalizedLabel(optionValue.Label);
            }

            var stateName = CreateFileHandler.GetOptionSetValueName(stateLabel, optionValue.Value.Value);

            return stateName;
        }

        public static List<OptionItem> GetStatusOptionItems(StatusAttributeMetadata statusAttr, StateAttributeMetadata stateAttr, List<StringMap> listStringMap)
        {
            var options = new List<OptionItem>();

            foreach (StatusOptionMetadata optionValue in statusAttr.OptionSet.Options)
            {
                if (!optionValue.Value.HasValue)
                {
                    continue;
                }

                string stateCodeName = "UnknowState";

                if (optionValue.State.HasValue)
                {
                    var stateCodeOptionSetValue = stateAttr.OptionSet.Options.OfType<StateOptionMetadata>().FirstOrDefault(op => op.Value.Value == optionValue.State);

                    if (stateCodeOptionSetValue != null)
                    {
                        stateCodeName = GetStateName(stateCodeOptionSetValue);
                    }
                    else
                    {
                        stateCodeName += optionValue.State.ToString();
                    }
                }

                string statusLabel = CreateFileHandler.GetLocalizedLabel(optionValue.Label);

                var statusName = CreateFileHandler.GetOptionSetValueName(statusLabel, optionValue.Value.Value);

                statusName = string.Format("{0}_{1}", stateCodeName, statusName);

                if (!string.IsNullOrEmpty(statusName))
                {
                    int? displayOrder = null;

                    if (listStringMap != null)
                    {
                        var stringMap = listStringMap.FirstOrDefault(e =>
                            string.Equals(e.AttributeName, statusAttr.LogicalName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(e.ObjectTypeCode, statusAttr.EntityLogicalName, StringComparison.OrdinalIgnoreCase)
                            && e.AttributeValue == optionValue.Value.Value
                            );

                        if (stringMap != null)
                        {
                            displayOrder = stringMap.DisplayOrder;
                        }
                    }

                    var optionItem = new OptionItem()
                    {
                        FieldName = statusName,
                        Value = optionValue.Value.Value,

                        Label = optionValue.Label,
                        Description = optionValue.Description,

                        LinkedStateCode = optionValue.State,
                        LinkedStateCodeName = stateCodeName,

                        DisplayOrder = displayOrder,

                        OptionMetadata = optionValue,
                    };

                    options.Add(optionItem);
                }
            }

            options = options.OrderBy(op => op.LinkedStateCode).ThenBy(op => op.DisplayOrder).ToList();

            return options;
        }

        public static string GetTabSpacer(IndentType indentType, int tabCount)
        {
            switch (indentType)
            {
                case IndentType.Tab:
                    return "\t";

                case IndentType.Spaces:
                default:
                    return new string(' ', tabCount);
            }
        }

        public const string JavaScriptTry =
@"try {";

        public const string JavaScriptCatch =
@"} catch (e) {
handleError(e);

throw e;
}";

        public const string JavaScriptCommonConstants =
@"var FormTypeEnum = {
'Undefined': 0,
'Create': 1,
'Update': 2,
'ReadOnly': 3,
'Disabled': 4,
'Bulk Edit': 6,
};

var RequiredLevelEnum = {
'none': 'none',
'required': 'required',
'recommended': 'recommended',
};

var SubmitModeEnum = {
'always': 'always',
'never': 'never',
'dirty': 'dirty',
};";

        public const string JavaScriptCommonFunctions =
@"var handleError = function (e) {

if (!e.HandledByConsole) {

if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {

if (typeof e.name == 'string') { writeToConsoleError(e.name); }
if (typeof e.fileName == 'string') { writeToConsoleError(e.fileName); }
if (typeof e.lineNumber != 'undefined') { writeToConsoleError(e.lineNumber); }
if (typeof e.message == 'string') { writeToConsoleError(e.message); }
if (typeof e.description == 'string') { writeToConsoleError(e.description); }
if (typeof e.stack == 'string') { writeToConsoleError(e.stack); }

e.HandledByConsole = true;

debugger;

if (typeof e.message == 'string' && e.message != '') {
var message = e.message;
}

if (typeof e.description == 'string' && e.description != '') {
var message = e.description;
}

if (typeof message != 'undefined') {
Xrm.Utility.alertDialog('Возникла ошибка: ' + message);
}
}
}
};

var writeToConsoleInfo = function (message) {
if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.info == 'function') {
window.console.info(message);
}
};

var writeToConsoleError = function (message) {
if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {
window.console.error(message);
}
};

var getAttributeValue = function (attrName) {

try {
if (attrName == null || attrName == '') {
writeToConsoleError(new Error('В функцию getAttributeValue передано значение null').stack);
return;
}

var attr = Xrm.Page.getAttribute(attrName);

if (!attr) {
writeToConsoleInfo(new Error('Не найдено поле ' + attrName).stack);
return null;
}

return attr.getValue();
} catch (e) {
handleError(e);

throw e;
}
};

var setAttributeRequiredLevel = function (attrName, requiredLevel) {

try {
if (attrName == null || attrName == '') {
writeToConsoleError(new Error('В функцию setAttributeRequiredLevel передано значение null').stack);
return;
}

var attr = Xrm.Page.getAttribute(attrName);

if (attr && attr.setRequiredLevel) {
attr.setRequiredLevel(requiredLevel);
}
else {
writeToConsoleInfo(new Error('Не найдено поле ' + attrName).stack);
}
} catch (e) {
handleError(e);

throw e;
}
};

var fireOnChangeAttribute = function (attrName) {

try {
if (attrName == null || attrName == '') {
writeToConsoleError(new Error('В функцию fireOnChangeAttribute передано значение null').stack);
return;
}

var attr = Xrm.Page.getAttribute(attrName);

if (attr && attr.fireOnChange) {
attr.fireOnChange();
}
else {
writeToConsoleInfo(new Error('Не найдено поле ' + attrName).stack);
}
} catch (e) {
handleError(e);

throw e;
}
};

var setAttributeControlsVisible = function (attrName, visible) {

try {
if (attrName == null || attrName == '') {
writeToConsoleError(new Error('В функцию setAttributeControlsVisible передано значение null').stack);
return;
}

var attr = Xrm.Page.getAttribute(attrName);

if (attr) {
attr.controls.forEach(function (control, i) {
if (control && control.setVisible) {

control.setVisible(visible);
checkParentVisible(control);
}
})
}
else {
writeToConsoleInfo(new Error('Не найдено поле ' + attrName).stack);
}

var control = Xrm.Page.getControl('header_' + attrName);
if (control && control.setVisible) {
control.setVisible(visible);
}
} catch (e) {
handleError(e);

throw e;
}
};

var checkParentVisible = function (control) {
try {
if (!control.getParent) {
return;
}
var parent = control.getParent();

if (!parent) {
return;
}

if (parent.setVisible && parent.getVisible && parent.controls && parent.controls.forEach) {

var visible = false;

parent.controls.forEach(function (contr) {
if (contr && contr.getVisible) {
var vis = contr.getVisible();
if (vis) {
visible = true;
}
}
});

if (visible != parent.getVisible()) {
parent.setVisible(visible);
checkParentVisible(parent);
}
}

if (parent.setVisible && parent.getVisible && parent.sections && parent.sections.forEach) {

var visible = false;

parent.sections.forEach(function (contr) {
if (contr && contr.getVisible) {
var vis = contr.getVisible();
if (vis) {
visible = true;
}
}
});

if (visible != parent.getVisible()) {
parent.setVisible(visible);
}
}
} catch (e) {
handleError(e);

throw e;
}
};

var setAttributeControlsDisabled = function (attrName, disabled) {

try {
if (attrName == null || attrName == '') {
writeToConsoleError(new Error('В функцию setAttributeControlsDisabled передано значение null').stack);
return;
}

var attr = Xrm.Page.getAttribute(attrName);

if (attr) {
attr.controls.forEach(function (control, i) {
if (control && control.setDisabled) {
control.setDisabled(disabled);
}
})
}
else {
writeToConsoleInfo(new Error('Не найдено поле ' + attrName).stack);
}

var control = Xrm.Page.getControl('header_' + attrName);
if (control && control.setDisabled) {
control.setDisabled(disabled);
}
} catch (e) {
handleError(e);

throw e;
}
};";

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_writer != null)
                    {
                        _writer.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~CreateFileHandler()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable Support
    }
}
