using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceOrganizationComparerCommand : AbstractDynamicCommandConnectionPair
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceOrganizationComparerCommand(OleMenuCommandService commandService, int baseIdStart, string formatButtonName, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart, formatButtonName)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceOrganizationComparerCommand InstanceCode { get; private set; }

        public static WebResourceOrganizationComparerCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new WebResourceOrganizationComparerCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.CodeWebResourceOrganizationComparerCommandId
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
                , sourceCode
            );

            InstanceFile = new WebResourceOrganizationComparerCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.FileWebResourceOrganizationComparerCommandId
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
                , sourceFile
            );
        }

        protected override void CommandAction(DTEHelper helper, Tuple<ConnectionData, ConnectionData> connectionDataPair)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileName = selectedFiles.FirstOrDefault()?.FileName;

                helper.HandleOpenWebResourceOrganizationComparerCommand(connectionDataPair.Item1, connectionDataPair.Item2, fileName);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<ConnectionData, ConnectionData> connectionDataPair, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}
