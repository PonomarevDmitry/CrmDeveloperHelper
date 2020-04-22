using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public class CrmCompletion : Completion
    {
        public IEnumerable<string> CompareValues { get; private set; }

        public CrmCompletion(string displayText, string insertionText, string description, ImageSource iconSource, string iconAutomationText, IEnumerable<string> compareValues)
            : base(displayText, insertionText, description, iconSource, iconAutomationText)
        {
            this.CompareValues = compareValues;
        }

        private const string _webresourcePrefix = "$webresource:";

        public bool IsMatch(string typedText)
        {
            if (IsMatchInternal(typedText))
            {
                return true;
            }

            if (typedText.StartsWith(_webresourcePrefix))
            {
                var typedTextChanged = typedText.Substring(_webresourcePrefix.Length);

                if (IsMatchInternal(typedTextChanged))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMatchInternal(string typedText)
        {
            if (string.IsNullOrEmpty(typedText))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(this.InsertionText))
            {
                int index = this.InsertionText.IndexOf(typedText, StringComparison.InvariantCultureIgnoreCase);

                if (index > -1)
                {
                    return true;
                }
            }

            if (this.CompareValues != null)
            {
                foreach (var item in this.CompareValues)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int index = item.IndexOf(typedText, StringComparison.InvariantCultureIgnoreCase);

                        if (index > -1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
