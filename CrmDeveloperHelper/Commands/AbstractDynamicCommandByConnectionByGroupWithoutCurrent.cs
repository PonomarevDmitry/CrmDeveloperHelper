using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnectionByGroupWithoutCurrent : AbstractDynamicCommandByConnection
    {
        public AbstractDynamicCommandByConnectionByGroupWithoutCurrent(OleMenuCommandService commandService, int baseIdStart)
            : base(commandService, baseIdStart)
        {
        }

        protected override ICollection<ConnectionData> GetElementSourceCollection()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            return connectionConfig.GetConnectionsByGroupWithoutCurrent();
        }

        protected override string GetElementName(ConnectionData connectionData)
        {
            return connectionData.Name;
        }
    }
}