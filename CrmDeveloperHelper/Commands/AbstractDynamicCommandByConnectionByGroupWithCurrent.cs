using Microsoft.VisualStudio.Shell;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnectionByGroupWithCurrent : AbstractDynamicCommandByConnection
    {
        public AbstractDynamicCommandByConnectionByGroupWithCurrent(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(
            commandService
            , baseIdStart
            , config => config.GetConnectionsByGroupWithCurrent()
            , connectionData => connectionData.NameWithCurrentMark
        )
        {

        }
    }
}
