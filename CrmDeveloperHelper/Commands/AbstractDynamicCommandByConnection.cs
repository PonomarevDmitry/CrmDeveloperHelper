using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnection : AbstractDynamicCommand<ConnectionData>
    {
        protected AbstractDynamicCommandByConnection(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(commandService, PackageGuids.guidDynamicCommandSet, baseIdStart, ConnectionData.CountConnectionToQuickList)
        {

        }
    }
}
