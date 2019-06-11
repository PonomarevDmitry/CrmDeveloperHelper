using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlShowDifferenceSiteMapDefaultCommand : AbstractDynamicCommandDefaultSiteMap
    {
        private CodeXmlShowDifferenceSiteMapDefaultCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeXmlShowDifferenceSiteMapDefaultCommandId
            )
        {

        }

        public static CodeXmlShowDifferenceSiteMapDefaultCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlShowDifferenceSiteMapDefaultCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string selectedSitemap)
        {
            var selectedFile = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).FirstOrDefault();

            if (selectedFile == null)
            {
                return;
            }

            helper.HandleShowDifferenceWithDefaultSitemap(selectedFile, selectedSitemap);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string selectedSitemap, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.RootSiteMap);
        }
    }
}
