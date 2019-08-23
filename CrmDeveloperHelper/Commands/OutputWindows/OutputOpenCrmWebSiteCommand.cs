using Microsoft.VisualStudio.Shell;
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
                new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenCrmWebSiteCommandId, OpenCrmWebSiteType.CrmWebApplication)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenAdvancedFindCommandId, OpenCrmWebSiteType.AdvancedFind)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenDashboardsCommandId, OpenCrmWebSiteType.Dashboards)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenActivitiesCommandId, OpenCrmWebSiteType.Activities)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenCustomizationCommandId, OpenCrmWebSiteType.Customization)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenSolutionsCommandId, OpenCrmWebSiteType.Solutions)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenWorkflowsCommandId, OpenCrmWebSiteType.Workflows)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenSystemJobsCommandId, OpenCrmWebSiteType.SystemJobs)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenTraceWallCommandId, OpenCrmWebSiteType.TraceWall)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenSystemUsersCommandId, OpenCrmWebSiteType.SystemUsers)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenTeamsCommandId, OpenCrmWebSiteType.Teams)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenRolesCommandId, OpenCrmWebSiteType.Roles)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenSecurityCommandId, OpenCrmWebSiteType.Security)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenAdministrationCommandId, OpenCrmWebSiteType.Administration)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenEngagementHubCommandId, OpenCrmWebSiteType.EngagementHub)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenBusinessCommandId, OpenCrmWebSiteType.Business)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenTemplatesCommandId, OpenCrmWebSiteType.Templates)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenProductCatalogCommandId, OpenCrmWebSiteType.ProductCatalog)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenServiceManagementCommandId, OpenCrmWebSiteType.ServiceManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenDataManagementCommandId, OpenCrmWebSiteType.DataManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenDocumentManagementCommandId, OpenCrmWebSiteType.DocumentManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenDuplicateDetectionJobsCommandId, OpenCrmWebSiteType.DuplicateDetectionJobs)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenSocialCommandId, OpenCrmWebSiteType.Social)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenAuditCommandId, OpenCrmWebSiteType.Audit)

                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenMobileOfflineCommandId, OpenCrmWebSiteType.MobileOffline)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenExternAppManagementCommandId, OpenCrmWebSiteType.ExternAppManagement)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenAppsForCrmCommandId, OpenCrmWebSiteType.AppsForCrm)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenRelationshipIntelligenceCommandId, OpenCrmWebSiteType.RelationshipIntelligence)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenMicrosoftFlowCommandId, OpenCrmWebSiteType.MicrosoftFlow)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenAppModuleCommandId, OpenCrmWebSiteType.AppModule)
                , new OutputOpenCrmWebSiteCommand(commandService, PackageIds.guidCommandSet.OutputOpenNewsCommandId, OpenCrmWebSiteType.News)
            });
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenCrmInWeb(connectionData, _crmWebSiteType);
        }
    }
}
