using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class LabelComparer
    {
        public static LabelDifferenceResult GetDifference(Label label1, Label label2)
        {
            List<int> listLocale = new List<int>();

            foreach (var item in label1.LocalizedLabels)
            {
                if (!string.IsNullOrEmpty(item.Label))
                {
                    if (!listLocale.Contains(item.LanguageCode))
                    {
                        listLocale.Add(item.LanguageCode);
                    }
                }
            }

            foreach (var item in label2.LocalizedLabels)
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

            LabelDifferenceResult result = new LabelDifferenceResult();

            if (listLocale.Any())
            {
                var sorted1 = label1.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label));
                var sorted2 = label2.LocalizedLabels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Label));

                foreach (var localeId in listLocale)
                {
                    var str = string.Format("({0}):", LanguageLocale.GetLocaleName(localeId));

                    var locLabel1 = sorted1.FirstOrDefault(l => l.LanguageCode == localeId);
                    var locLabel2 = sorted2.FirstOrDefault(l => l.LanguageCode == localeId);

                    if (locLabel1 != null && locLabel2 == null)
                    {
                        result.LabelsOnlyIn1.Add(new LabelString() { LanguageCode = localeId, Locale = str, Value = locLabel1.Label });
                    }
                    else if (locLabel1 == null && locLabel2 != null)
                    {
                        result.LabelsOnlyIn2.Add(new LabelString() { LanguageCode = localeId, Locale = str, Value = locLabel2.Label });
                    }
                    else if (locLabel1 != null && locLabel2 != null)
                    {
                        if (locLabel1.Label != locLabel2.Label)
                        {
                            result.LabelDifference.Add(new LabelStringDifference() { LanguageCode = localeId, Locale = str, Value1 = locLabel1.Label, Value2 = locLabel2.Label });
                        }
                    }
                }
            }

            return result;
        }

        internal static LabelDifferenceResult GetDifference(TranslationDisplayString displayString1, TranslationDisplayString displayString2)
        {
            List<int> listLocale = new List<int>();

            foreach (var item in displayString1.Labels)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (!listLocale.Contains(item.LanguageCode))
                    {
                        listLocale.Add(item.LanguageCode);
                    }
                }
            }

            foreach (var item in displayString2.Labels)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (!listLocale.Contains(item.LanguageCode))
                    {
                        listLocale.Add(item.LanguageCode);
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            LabelDifferenceResult result = new LabelDifferenceResult();

            if (listLocale.Any())
            {
                var sorted1 = displayString1.Labels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Value));
                var sorted2 = displayString2.Labels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Value));

                foreach (var localeId in listLocale)
                {
                    var str = string.Format("({0}):", LanguageLocale.GetLocaleName(localeId));

                    var locLabel1 = sorted1.FirstOrDefault(l => l.LanguageCode == localeId);
                    var locLabel2 = sorted2.FirstOrDefault(l => l.LanguageCode == localeId);

                    if (locLabel1 != null && locLabel2 == null)
                    {
                        result.LabelsOnlyIn1.Add(new LabelString() { LanguageCode = localeId, Locale = str, Value = locLabel1.Value });
                    }
                    else if (locLabel1 == null && locLabel2 != null)
                    {
                        result.LabelsOnlyIn2.Add(new LabelString() { LanguageCode = localeId, Locale = str, Value = locLabel2.Value });
                    }
                    else if (locLabel1 != null && locLabel2 != null)
                    {
                        if (locLabel1.Value != locLabel2.Value)
                        {
                            result.LabelDifference.Add(new LabelStringDifference() { LanguageCode = localeId, Locale = str, Value1 = locLabel1.Value, Value2 = locLabel2.Value });
                        }
                    }
                }
            }

            return result;
        }

        internal static LabelDifferenceResult GetDifference(Model.LocalizedLabel locLabel1, Model.LocalizedLabel locLabel2)
        {
            List<int> listLocale = new List<int>();

            foreach (var item in locLabel1.Labels)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (!listLocale.Contains(item.LanguageCode))
                    {
                        listLocale.Add(item.LanguageCode);
                    }
                }
            }

            foreach (var item in locLabel2.Labels)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (!listLocale.Contains(item.LanguageCode))
                    {
                        listLocale.Add(item.LanguageCode);
                    }
                }
            }

            listLocale.Sort(new LocaleComparer());

            LabelDifferenceResult result = new LabelDifferenceResult();

            if (listLocale.Any())
            {
                var sorted1 = locLabel1.Labels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Value));
                var sorted2 = locLabel2.Labels.Where(l => listLocale.Contains(l.LanguageCode) && !string.IsNullOrEmpty(l.Value));

                foreach (var localeId in listLocale)
                {
                    var str = string.Format("({0}):", LanguageLocale.GetLocaleName(localeId));

                    var label1 = sorted1.FirstOrDefault(l => l.LanguageCode == localeId);
                    var label2 = sorted2.FirstOrDefault(l => l.LanguageCode == localeId);

                    if (label1 != null && label2 == null)
                    {
                        result.LabelsOnlyIn1.Add(new LabelString() { LanguageCode = localeId, Locale = str, Value = label1.Value });
                    }
                    else if (label1 == null && label2 != null)
                    {
                        result.LabelsOnlyIn2.Add(new LabelString() { LanguageCode = localeId, Locale = str, Value = label2.Value });
                    }
                    else if (label1 != null && label2 != null)
                    {
                        if (label1.Value != label2.Value)
                        {
                            result.LabelDifference.Add(new LabelStringDifference() { LanguageCode = localeId, Locale = str, Value1 = label1.Value, Value2 = label2.Value });
                        }
                    }
                }
            }

            return result;
        }
    }
}
