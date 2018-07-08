using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsMatch(string typedText)
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
