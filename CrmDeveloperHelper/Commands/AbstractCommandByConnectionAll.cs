using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractCommandByConnectionAll : AbstractCommandByConnection
    {
        public AbstractCommandByConnectionAll(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(
            commandService
            , baseIdStart
            , config => config.Connections
            , connectionData => connectionData.NameWithCurrentMark
        )
        {

        }
    }
}
