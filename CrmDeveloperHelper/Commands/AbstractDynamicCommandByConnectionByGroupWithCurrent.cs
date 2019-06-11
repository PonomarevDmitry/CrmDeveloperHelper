using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

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
        )
        {

        }

        protected sealed override ICollection<ConnectionData> GetConnectionDataSource(ConnectionConfiguration connectionConfig)
        {
            return connectionConfig.GetConnectionsByGroupWithCurrent();
        }

        protected override string GetConnectionName(ConnectionData connectionData)
        {
            return connectionData.NameWithCurrentMark;
        }
    }
}
