using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportUpdateCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeReportUpdateCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeReportUpdateCommandId
            )
        {

        }

        public static CodeReportUpdateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeReportUpdateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (!connectionData.IsReadOnly)
            {
                helper.HandleReportUpdateCommand(connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            if (connectionData.IsReadOnly)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = true;

                CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
            }
        }
    }
}