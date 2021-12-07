using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public abstract class CreateFileHandler
    {
        public const string _defaultTabSpacer = "    ";

        private readonly TextWriter _writer;

        protected readonly string _tabSpacer;
        private readonly bool _allDescriptions;

        private int _tabCount = 0;

        protected CreateFileHandler(TextWriter writer, string tabSpacer, bool allDescriptions)
        {
            this._writer = writer;
            this._tabSpacer = tabSpacer;
            this._allDescriptions = allDescriptions;
        }

        private void ChangeTabCount(int count)
        {
            this._tabCount += count;
        }

        private bool _currentLineHasCharacters = false;

        private void WriteSingleLine(string line = "")
        {
            if (!string.IsNullOrEmpty(line))
            {
                int decrease = line.TakeWhile(ch => ch == '}').Count();

                ChangeTabCount(-decrease);

                WriteTabs();
            }

            _writer.WriteLine(line);
            _currentLineHasCharacters = false;

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
            if (_currentLineHasCharacters)
            {
                return;
            }

            _currentLineHasCharacters = true;

            if (this._tabCount > 0)
            {
                _writer.Write(new StringBuilder(_tabSpacer.Length * this._tabCount).Insert(0, _tabSpacer, this._tabCount).ToString());
            }
        }

        protected void Write(string line, params object[] args)
        {
            var str = line;

            if (args != null && args.Any())
            {
                str = string.Format(line, args);
            }

            Write(str);
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

        protected void WriteLine(StringBuilder stringBuilder) => WriteLine(stringBuilder.ToString());

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

        protected void WriteLineIfHasLine(ref bool isFirstLineWrited)
        {
            if (isFirstLineWrited)
            {
                WriteLine();
            }
            else
            {
                isFirstLineWrited = true;
            }
        }

        protected void WriteSummary(Label displayName, Label description, IEnumerable<string> headers, IEnumerable<string> footers)
        {
            var listStrings = UnionStrings(displayName, description, headers, footers, _allDescriptions, _tabSpacer);

            WriteSummaryStrings(listStrings);
        }

        protected virtual void WriteSummaryStrings(IEnumerable<string> listStrings)
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

        public static IEnumerable<string> UnionStrings(Label displayName, Label description, IEnumerable<string> headers, IEnumerable<string> footers, bool allDescriptions, string tabSpacer)
        {
            bool displayNameIsNull = displayName == null || displayName.LocalizedLabels.Count == 0;

            bool descriptionIsNull = description == null || description.LocalizedLabels.Count == 0;

            bool headersIsNull = headers == null || !headers.Any();

            bool footersIsNull = footers == null || !footers.Any();

            if (displayNameIsNull && descriptionIsNull && headersIsNull && footersIsNull)
            {
                return Enumerable.Empty<string>();
            }

            List<string> listStrings = new List<string>();

            if (!headersIsNull)
            {
                listStrings.AddRange(headers);
            }

            FillLabelDisplayNameAndDescription(listStrings, allDescriptions, displayName, description, tabSpacer);

            if (!footersIsNull)
            {
                if (listStrings.Count > 0)
                {
                    listStrings.Add(string.Empty);
                }

                listStrings.AddRange(footers);
            }

            return listStrings;
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

            listLocale.Sort(LocaleComparer.Comparer);

            if (listLocale.Any())
            {
                HashSet<string> hashShowed = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

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

            listLocale.Sort(LocaleComparer.Comparer);

            var localeId = listLocale.FirstOrDefault();

            if (localeId != default(int))
            {
                if (listStrings.Count > 0)
                {
                    listStrings.Add(string.Empty);
                }

                listStrings.Add(string.Format("({0}):", LanguageLocale.GetLocaleName(localeId)));

                HashSet<string> hashShowed = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

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

            listLocale.Sort(LocaleComparer.Comparer);

            if (listLocale.Any())
            {
                HashSet<string> hashShowed = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

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

            listLocale.Sort(LocaleComparer.Comparer);

            var localeId = listLocale.FirstOrDefault();

            if (localeId != default(int))
            {
                if (listStrings.Count > 0)
                {
                    listStrings.Add(string.Empty);
                }

                listStrings.Add(string.Format("({0}):", LanguageLocale.GetLocaleName(localeId)));

                HashSet<string> hashShowed = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

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

        public static bool IgnoreAttribute(string entityName, string attributeName)
        {
            if (entityName.Equals("connection", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attributeName.Equals("record1objecttypecode", StringComparison.InvariantCultureIgnoreCase)
                    || attributeName.Equals("record2objecttypecode", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("importmap", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attributeName.Equals("targetentity", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("queueitem", StringComparison.InvariantCultureIgnoreCase)
                || entityName.Equals("sla", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                if (attributeName.Equals("objecttypecode", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("duplicaterule", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attributeName.Equals("baseentitytypecode", StringComparison.InvariantCultureIgnoreCase)
                    || attributeName.Equals("matchingentitytypecode", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("similarityrule", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attributeName.Equals("baseentitytypecode", StringComparison.InvariantCultureIgnoreCase)
                    || attributeName.Equals("matchingentitytypecode", StringComparison.InvariantCultureIgnoreCase)
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

        private static readonly string[] _lat_up_ =
        {
            "A", "B", "V", "G", "D", "E", "E", "Zh"
            , "Z", "I", "I", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh"
            , "Ts", "Ch", "Sh", "Shch", "Ie"
            , "Y", "", "E", "Iu", "Ia"
        };

        private static readonly string[] _lat_low =
        {
            "a", "b", "v", "g", "d", "e", "e", "zh"
            , "z", "i", "i", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh"
            , "ts", "ch", "sh", "shch", "ie"
            , "y", "", "e", "iu", "ia"
        };

        private static readonly string[] _rus_up_ =
        {
            "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж"
            , "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х"
            , "Ц", "Ч", "Ш", "Щ", "Ъ"
            , "Ы", "Ь", "Э", "Ю", "Я"
        };

        private static readonly string[] _rus_low =
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
            var result = ContentComparerHelper.RemoveDiacritics(label);

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
            return string.Equals(name, "activitypointer_activitytypecode", StringComparison.InvariantCultureIgnoreCase);
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

            listLocale.Sort(LocaleComparer.Comparer);

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

            listLocale.Sort(LocaleComparer.Comparer);

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

                string optionSetLabel = GetLocalizedLabel(optionValue.Label);

                var optionSetValueName = GetOptionSetValueName(optionSetLabel, optionValue.Value.Value);

                if (!string.IsNullOrEmpty(optionSetValueName))
                {
                    int? displayOrder = null;

                    if (listStringMap != null && !string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(attributeName))
                    {
                        var stringMap = listStringMap.FirstOrDefault(e =>
                            string.Equals(e.AttributeName, attributeName, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(e.ObjectTypeCode, entityName, StringComparison.InvariantCultureIgnoreCase)
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
                        string statusLabel = GetLocalizedLabel(statusOptionValue.Label);

                        defaultStatusCodeName = GetOptionSetValueName(statusLabel, statusOptionValue.Value.Value);
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
                            string.Equals(e.AttributeName, stateAttr.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(e.ObjectTypeCode, stateAttr.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase)
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
                stateLabel = GetLocalizedLabel(optionValue.Label);
            }

            var stateName = GetOptionSetValueName(stateLabel, optionValue.Value.Value);

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

                string statusLabel = GetLocalizedLabel(optionValue.Label);

                var statusName = GetOptionSetValueName(statusLabel, optionValue.Value.Value);

                statusName = string.Format("{0}_{1}", stateCodeName, statusName);

                if (!string.IsNullOrEmpty(statusName))
                {
                    int? displayOrder = null;

                    if (listStringMap != null)
                    {
                        var stringMap = listStringMap.FirstOrDefault(e =>
                            string.Equals(e.AttributeName, statusAttr.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(e.ObjectTypeCode, statusAttr.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase)
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

        protected static string ToCSharpLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (System.CodeDom.Compiler.CodeDomProvider provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        protected static string ToCSharpLiteral(int input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        protected static string ToCSharpLiteral(long input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        protected static string ToCSharpLiteral(decimal input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        protected static string ToCSharpLiteral(double input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        protected static string ToCSharpLiteral(bool input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        protected void WriteCrmDeveloperAttributeForGlobalOptionSet(string optionSetName)
        {
            string attributeValue = $@"/// <crmdeveloperhelper globaloptionsetname=""{optionSetName}"" />";

            WriteLine(attributeValue);
        }

        protected void WriteCommaIfNotFirstLine(ref bool firstLine)
        {
            if (firstLine)
            {
                firstLine = false;
            }
            else
            {
                WriteLine(",");
            }
        }
    }
}
