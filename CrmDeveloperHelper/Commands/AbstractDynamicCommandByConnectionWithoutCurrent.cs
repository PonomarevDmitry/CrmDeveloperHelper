using Microsoft.VisualStudio.Shell;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnectionWithoutCurrent : AbstractDynamicCommandByConnection
    {
        public AbstractDynamicCommandByConnectionWithoutCurrent(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(
            commandService
            , baseIdStart
            , config => config.GetConnectionsWithoutCurrent()
            , connectionData => connectionData.Name
        )
        {

        }
    }
}
