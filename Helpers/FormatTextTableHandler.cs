using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class FormatTextTableHandler
    {
        private Dictionary<int, int> maxLength = new Dictionary<int, int>();

        private List<string[]> lines = new List<string[]>();

        private string[] header = null;

        public string Separator { get; set; }

        public bool ShowEmptyColumns { get; private set; }

        public int Count
        {
            get { return lines.Count; }
        }

        public FormatTextTableHandler()
        {
            this.Separator = "    ";
            this.ShowEmptyColumns = false;
        }

        public FormatTextTableHandler(bool show)
            : this()
        {
            this.ShowEmptyColumns = show;
        }

        public FormatTextTableHandler SetHeader(params string[] parts)
        {
            header = parts;

            return this;
        }

        public FormatTextTableHandler SetHeader(IEnumerable<string> parts)
        {
            return SetHeader(parts.ToArray());
        }

        public FormatTextTableHandler SetHeaderNotNull(params string[] parts)
        {
            var temp = parts.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (temp.Length > 0)
            {
                header = temp;
            }

            return this;
        }

        public FormatTextTableHandler SetHeaderNotNull(IEnumerable<string> parts)
        {
            return SetHeaderNotNull(parts.ToArray());
        }

        public FormatTextTableHandler AddLine(params string[] parts)
        {
            lines.Add(parts);

            CalculateLineLengths(parts);

            return this;
        }

        public FormatTextTableHandler AddLine(IEnumerable<string> parts)
        {
            return AddLine(parts.ToArray());
        }

        public FormatTextTableHandler AddLineNotNull(params string[] parts)
        {
            var temp = parts.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (temp.Length > 0)
            {
                lines.Add(temp);

                CalculateLineLengths(temp);
            }

            return this;
        }

        public FormatTextTableHandler AddLineNotNull(IEnumerable<string> parts)
        {
            return AddLineNotNull(parts.ToArray());
        }

        public FormatTextTableHandler CalculateLineLengths(params string[] parts)
        {
            for (int index = 0; index < parts.Length; index++)
            {
                var part = parts[index];

                int len = 0;

                if (!string.IsNullOrEmpty(part))
                {
                    len = part.Length;
                }

                if (maxLength.ContainsKey(index))
                {
                    maxLength[index] = Math.Max(maxLength[index], len);
                }
                else
                {
                    maxLength[index] = len;
                }
            }

            return this;
        }

        public FormatTextTableHandler CalculateLineLengths(IEnumerable<string> parts)
        {
            return CalculateLineLengths(parts.ToArray());
        }

        public List<string> GetFormatedLines(bool sorted)
        {
            List<string> list = new List<string>();

            if (this.lines.Count > 0)
            {
                foreach (var line in lines)
                {
                    var str = FormatLine(line);

                    list.Add(str);
                }

                if (sorted)
                {
                    list.Sort();
                }

                if (this.header != null)
                {
                    var str = FormatLine(header);

                    list.Insert(0, str);
                }
            }

            return list;
        }

        public string FormatLine(params string[] line)
        {
            StringBuilder str = new StringBuilder();

            for (int index = 0; index < line.Length; index++)
            {
                var part = line[index];

                var partMaxLength = 0;

                if (maxLength.ContainsKey(index))
                {
                    partMaxLength = maxLength[index];
                }

                if (this.ShowEmptyColumns || partMaxLength > 0)
                {
                    if (this.header != null && index < this.header.Length)
                    {
                        partMaxLength = Math.Max(partMaxLength, this.header[index].Length);
                    }

                    if (str.Length > 0)
                    {
                        str.Append(this.Separator);
                    }

                    if (!string.IsNullOrEmpty(part))
                    {
                        str.Append(part.PadRight(partMaxLength));
                    }
                    else
                    {
                        str.Append(new String(' ', partMaxLength));
                    }
                }
            }

            return str.ToString().TrimEnd();
        }

        public FormatTextTableHandler AddLineIfNotEqual(string fieldName, string value1, string value2)
        {
            if (!string.Equals(value1 ?? string.Empty, value2 ?? string.Empty))
            {
                this.AddLine(fieldName, value1, value2);
            }

            return this;
        }

        public FormatTextTableHandler AddLineIfNotEqual<T>(string fieldName, T value1, T value2) where T : struct
        {
            var str1 = value1.ToString();
            var str2 = value2.ToString();

            if (str1 != str2)
            {
                this.AddLine(fieldName, str1, str2);
            }

            return this;
        }

        public FormatTextTableHandler AddLineIfNotEqual(string fieldName, BooleanManagedProperty value1, BooleanManagedProperty value2)
        {
            var valueString1 = value1 != null ? value1.Value.ToString() : "null";
            var valueChange1 = value1 != null ? value1.CanBeChanged.ToString() : string.Empty;

            var valueString2 = value2 != null ? value2.Value.ToString() : "null";
            var valueChange2 = value2 != null ? value2.CanBeChanged.ToString() : string.Empty;

            var str1 = string.Format("{0} CanBeChanged {1}", valueString1, valueChange1);
            var str2 = string.Format("{0} CanBeChanged {1}", valueString2, valueChange2);

            if (valueString1 != valueString2)
            {
                this.AddLine(fieldName, str1, str2);
            }

            return this;
        }

        public FormatTextTableHandler AddLineIfNotEqual(string fieldName, AttributeRequiredLevelManagedProperty value1, AttributeRequiredLevelManagedProperty value2)
        {
            var valueString1 = value1 != null ? value1.Value.ToString() : "null";
            var valueChange1 = value1 != null ? value1.CanBeChanged.ToString() : string.Empty;

            var valueString2 = value2 != null ? value2.Value.ToString() : "null";
            var valueChange2 = value2 != null ? value2.CanBeChanged.ToString() : string.Empty;

            var str1 = string.Format("{0} CanBeChanged {1}", valueString1, valueChange1);
            var str2 = string.Format("{0} CanBeChanged {1}", valueString2, valueChange2);

            if (valueString1 != valueString2)
            {
                this.AddLine(fieldName, str1, str2);
            }

            return this;
        }

        public FormatTextTableHandler AddLineIfNotEqual<T>(string fieldName, Nullable<T> value1, Nullable<T> value2) where T : struct
        {
            var str1 = value1.HasValue ? value1.Value.ToString() : "null";
            var str2 = value2.HasValue ? value2.Value.ToString() : "null";

            if (str1 != str2)
            {
                this.AddLine(fieldName, str1, str2);
            }

            return this;
        }

        public FormatTextTableHandler AddEntityMetadataString(string fieldName, string valueString)
        {
            return this.AddLine(fieldName, valueString);
        }

        public FormatTextTableHandler AddEntityMetadataString(string fieldName, BooleanManagedProperty value)
        {
            var valueString = value != null ? value.Value.ToString() : "null";
            var valueChange = value != null ? value.CanBeChanged.ToString() : string.Empty;

            return this.AddLineNotNull(fieldName, valueString, valueChange);
        }

        public FormatTextTableHandler AddEntityMetadataString<T>(string fieldName, Nullable<T> value) where T : struct
        {
            var valueString = value.HasValue ? value.ToString() : "null";

            return this.AddLine(fieldName, valueString);
        }

        public FormatTextTableHandler AddEntityMetadataString<T>(string fieldName, T value) where T : struct
        {
            return this.AddLine(fieldName, value.ToString());
        }
    }
}
