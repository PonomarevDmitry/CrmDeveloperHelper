using Microsoft.VisualStudio.Shell;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnectionByGroupWithoutCurrent : AbstractDynamicCommandByConnection
    {
        public AbstractDynamicCommandByConnectionByGroupWithoutCurrent(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(
            commandService
            , baseIdStart
            , config => config.GetConnectionsByGroupWithoutCurrent()
            , connectionData => connectionData.Name
        )
        {

        }
    }
}
