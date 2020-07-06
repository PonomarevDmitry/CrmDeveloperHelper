using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSiteMapShowDifferenceDefaultCommand : AbstractDynamicCommandDefaultSiteMap
    {
        private CodeXmlSiteMapShowDifferenceDefaultCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeXmlSiteMapShowDifferenceDefaultCommandId)
        {

        }

        public static CodeXmlSiteMapShowDifferenceDefaultCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSiteMapShowDifferenceDefaultCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string selectedSiteMap)
        {
            var selectedFile = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).FirstOrDefault();

            if (selectedFile == null)
            {
                return;
            }

            helper.HandleShowDifferenceWithDefaultSiteMap(selectedFile, selectedSiteMap);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string selectedSiteMap, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.SiteMapXmlRoot);
        }
    }
}
