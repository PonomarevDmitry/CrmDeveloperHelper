using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OptionItem
    {
        public string FieldName { get; set; }

        public int Value { get; set; }

        public Microsoft.Xrm.Sdk.Label Label { get; set; }

        public Microsoft.Xrm.Sdk.Label Description { get; set; }

        public int? LinkedStateCode { get; set; }

        public string LinkedStateCodeName { get; set; }

        public int? DefaultStatusCode { get; set; }

        public string DefaultStatusCodeName { get; set; }

        public string InvariantName { get; set; }

        public int? DisplayOrder { get; set; }

        public OptionMetadata OptionMetadata { get; set; }

        public string MakeStrings()
        {
            StringBuilder strValue = new StringBuilder();

            strValue.Append(FieldName);
            strValue.Append(" = ");
            strValue.Append(Value);

            return strValue.ToString();
        }

        public string MakeStringJS()
        {
            StringBuilder strValue = new StringBuilder();

            strValue.Append("'" + FieldName + "'");

            strValue.Append(": {");

            strValue.AppendFormat(" 'value': {0}", Value);

            if (this.Label != null
                && this.Label.UserLocalizedLabel != null
                && !string.IsNullOrEmpty(this.Label.UserLocalizedLabel.Label)
                )
            {
                string langCode = string.Empty;

                strValue.AppendFormat(", 'text': '{0}'", this.Label.UserLocalizedLabel.Label);
            }

            if (this.DisplayOrder.HasValue)
            {
                strValue.AppendFormat(", 'displayOrder': {0}", this.DisplayOrder);
            }

            if (this.Label != null
                && this.Label.LocalizedLabels != null
                )
            {
                var lbls = this.Label.LocalizedLabels.Where(lbl => !string.IsNullOrEmpty(lbl.Label));

                HashSet<string> hashShowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var item in lbls.OrderBy(l => l.LanguageCode, new LocaleComparer()))
                {
                    if (!string.IsNullOrEmpty(item.Label) && !hashShowed.Contains(item.Label))
                    {
                        string langCode = string.Empty;

                        strValue.AppendFormat(", 'name{0}': '{1}'", item.LanguageCode, item.Label);

                        hashShowed.Add(item.Label);
                    }
                }
            }

            if (this.LinkedStateCode.HasValue)
            {
                strValue.AppendFormat(", 'linkedstatecode': {0}", this.LinkedStateCode.ToString());
            }

            strValue.Append(" },");

            return strValue.ToString();
        }
    }
}