using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly RibbonPlacement _ribbonPlacement;

        private JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, RibbonPlacement ribbonPlacement)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._ribbonPlacement = ribbonPlacement;
        }

        public static JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand InstanceCodeCustomRules { get; private set; }

        public static JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand InstanceCodeJavaScriptFunctions { get; private set; }

        public static JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand InstanceFileCustomRules { get; private set; }

        public static JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand InstanceFileJavaScriptFunctions { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCodeCustomRules = new JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptCopyToClipboardRibbonCustomRulesInConnectionCommandId
                , sourceCode
                , RibbonPlacement.CustomRules
            );

            InstanceCodeJavaScriptFunctions = new JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptCopyToClipboardRibbonJavaScriptFunctionsInConnectionCommandId
                , sourceCode
                , RibbonPlacement.JavaScriptFunctions
            );

            InstanceFileCustomRules = new JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileJavaScriptCopyToClipboardRibbonCustomRulesInConnectionCommandId
                , sourceFile
                , RibbonPlacement.CustomRules
            );

            InstanceFileJavaScriptFunctions = new JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileJavaScriptCopyToClipboardRibbonJavaScriptFunctionsInConnectionCommandId
                , sourceFile
                , RibbonPlacement.JavaScriptFunctions
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceCopyToClipboardRibbonObjectsCommand(connectionData, selectedFiles.FirstOrDefault(), _ribbonPlacement);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}
