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

        protected override ICollection<ConnectionData> GetElementSourceCollection()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            return connectionConfig.GetConnectionsWithoutCurrent();
        }

        protected override string GetElementName(ConnectionData connectionData)
        {
            return connectionData.Name;
        }
    }
}
