using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class ExportSolutionOverrideInformation
    {
        public bool OverrideNameAndVersion { get; private set; }

        public string UniqueName { get; private set; }

        public string DisplayName { get; private set; }

        public string Version { get; private set; }

        public bool OverrideDescription { get; private set; }

        public string Description { get; private set; }

        public ExportSolutionOverrideInformation(bool overrideNameAndVersion, string uniqueName, string displayName, string version, bool overrideDescription, string description)
        {
            this.OverrideNameAndVersion = overrideNameAndVersion;

            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Version = version;

            this.OverrideDescription = overrideDescription;
            this.Description = description;
        }
    }
}
