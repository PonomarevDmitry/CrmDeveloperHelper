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

        protected override ICollection<ConnectionData> GetElementSourceCollection()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            return connectionConfig.GetConnectionsByGroupWithCurrent();
        }

        protected override string GetElementName(ConnectionData connectionData)
        {
            return connectionData.NameWithCurrentMark;
        }
    }
}
