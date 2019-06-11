using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

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
        )
        {

        }

        protected sealed override ICollection<ConnectionData> GetConnectionDataSource(ConnectionConfiguration connectionConfig)
        {
            return connectionConfig.GetConnectionsWithoutCurrent();
        }

        protected override string GetConnectionName(ConnectionData connectionData)
        {
            return connectionData.Name;
        }
    }
}
