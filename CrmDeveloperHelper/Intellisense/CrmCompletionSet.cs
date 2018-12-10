using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public sealed class CrmCompletionSet : CompletionSet
    {
        private readonly List<CrmCompletion> _allCompletions;

        public CrmCompletionSet(string moniker, string displayName, ITrackingSpan applicableTo, List<CrmCompletion> completions, IEnumerable<CrmCompletion> completionBuilders)
            : base(moniker, displayName, applicableTo, completions, completionBuilders)
        {
            _allCompletions = completions;
        }

        public override void SelectBestMatch()
        {
            ITextSnapshot snapshot = ApplicableTo.TextBuffer.CurrentSnapshot;
            string typedText = ApplicableTo.GetText(snapshot);

            if (string.IsNullOrWhiteSpace(typedText))
            {
                if (this.WritableCompletions.Any())
                    SelectionStatus = new CompletionSelectionStatus(WritableCompletions.First(), true, true);

                return;
            }

            foreach (CrmCompletion comp in WritableCompletions)
            {
                if (comp.IsMatch(typedText))
                {
                    SelectionStatus = new CompletionSelectionStatus(comp, true, true);
                    return;
                }
            }
        }

        public override void Filter()
        {
            ITextSnapshot snapshot = ApplicableTo.TextBuffer.CurrentSnapshot;
            string typedText = ApplicableTo.GetText(snapshot);

            if (!string.IsNullOrEmpty(typedText))
            {
                List<CrmCompletion> temp = _allCompletions.Where(c => c.IsMatch(typedText)).ToList();

                if (temp.Any())
                {
                    this.WritableCompletions.BeginBulkOperation();

                    this.WritableCompletions.Clear();

                    this.WritableCompletions.AddRange(temp);

                    this.WritableCompletions.EndBulkOperation();
                }
            }
            else
            {
                this.WritableCompletions.BeginBulkOperation();

                this.WritableCompletions.Clear();

                this.WritableCompletions.AddRange(_allCompletions);

                this.WritableCompletions.EndBulkOperation();
            }
        }
    }
}