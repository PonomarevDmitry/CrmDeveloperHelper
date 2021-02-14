using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractOutputWindowDynamicCommandOnSolutionLastSelected : AbstractOutputWindowDynamicCommand<string>
    {
        protected AbstractOutputWindowDynamicCommandOnSolutionLastSelected(OleMenuCommandService commandService, int baseIdStart)
            : base(commandService, PackageGuids.guidDynamicSolutionLastSelectedCommandSet, baseIdStart, ConnectionData.CountLastSolutions)
        {
        }

        protected override ICollection<string> GetElementSourceCollection(ConnectionData connectionData)
        {
            return connectionData.LastSelectedSolutionsUniqueName;
        }

        protected override string GetElementName(ConnectionData connectionData, string solutionUniqueName)
        {
            return solutionUniqueName;
        }
    }
}
