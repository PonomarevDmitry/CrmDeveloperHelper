using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnection : AbstractDynamicCommand<ConnectionData>
    {
        public AbstractDynamicCommandByConnection(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(commandService, baseIdStart, ConnectionData.CountConnectionToQuickList)
        {

        }
    }
}
