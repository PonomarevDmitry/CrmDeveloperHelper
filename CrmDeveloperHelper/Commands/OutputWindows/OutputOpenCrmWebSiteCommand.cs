using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenCrmWebSiteCommand : AbstractOutputWindowCommand
    {
        private readonly OpenCrmWebSiteType _crmWebSiteType;

        private OutputOpenCrmWebSiteCommand(OleMenuCommandService commandService, int baseIdStart, OpenCrmWebSiteType crmWebSiteType)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._crmWebSiteType = crmWebSiteType;
        }

        public static List<OutputOpenCrmWebSiteCommand> Instances { get; private set; } = new List<OutputOpenCrmWebSiteCommand>();

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instances.AddRange(new[]
            {
                new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenCrmWebSiteCommandId, OpenCrmWebSiteType.CrmWebApplication)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenAdvancedFindCommandId, OpenCrmWebSiteType.AdvancedFind)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenSolutionsCommandId, OpenCrmWebSiteType.Solutions)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenWorkflowsCommandId, OpenCrmWebSiteType.Workflows)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenSystemJobsCommandId, OpenCrmWebSiteType.SystemJobs)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenCustomizationCommandId, OpenCrmWebSiteType.Customization)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenSystemUsersCommandId, OpenCrmWebSiteType.SystemUsers)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenTeamsCommandId, OpenCrmWebSiteType.Teams)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenRolesCommandId, OpenCrmWebSiteType.Roles)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenSecurityCommandId, OpenCrmWebSiteType.Security)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenAdministrationCommandId, OpenCrmWebSiteType.Administration)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenEngagementHubCommandId, OpenCrmWebSiteType.EngagementHub)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenBusinessCommandId, OpenCrmWebSiteType.Business)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenTemplatesCommandId, OpenCrmWebSiteType.Templates)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenProductCatalogCommandId, OpenCrmWebSiteType.ProductCatalog)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenServiceManagementCommandId, OpenCrmWebSiteType.ServiceManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenDataManagementCommandId, OpenCrmWebSiteType.DataManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenSocialCommandId, OpenCrmWebSiteType.Social)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenAuditCommandId, OpenCrmWebSiteType.Audit)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenMobileOfflineCommandId, OpenCrmWebSiteType.MobileOffline)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenExternAppManagementCommandId, OpenCrmWebSiteType.ExternAppManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenAppsForCrmCommandId, OpenCrmWebSiteType.AppsForCrm)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenRelationshipIntelligenceCommandId, OpenCrmWebSiteType.RelationshipIntelligence)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenMicrosoftFlowCommandId, OpenCrmWebSiteType.MicrosoftFlow)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.OutputOpenAppModuleCommandId, OpenCrmWebSiteType.AppModule)
            });
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenCrmInWeb(connectionData, _crmWebSiteType);
        }
    }
}