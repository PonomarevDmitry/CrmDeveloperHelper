using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class SolutionViewItem
    {
        public Solution Solution { get; private set; }

        public string UniqueName => Solution.UniqueName;

        public string FriendlyName => Solution.FriendlyName;

        public string SolutionType => Solution.FormattedValues[Solution.Schema.Attributes.ismanaged];

        public string Visible => Solution.FormattedValues[Solution.Schema.Attributes.isvisible];

        public DateTime? InstalledOn => Solution.InstalledOn?.ToLocalTime();

        public string PublisherName => Solution.PublisherId?.Name;

        public string Prefix => Solution.PublisherCustomizationPrefix;

        public string Version => Solution.Version;

        public string Description => Solution.Description;

        public bool HasDescription => !string.IsNullOrEmpty(Solution.Description);

        public SolutionViewItem(Solution solution)
        {
            this.Solution = solution;
        }

        public override string ToString()
        {
            return this.UniqueName;
        }
    }
}
