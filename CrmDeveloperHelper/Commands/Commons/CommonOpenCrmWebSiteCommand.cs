using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenCrmWebSiteCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly OpenCrmWebSiteType _crmWebSiteType;

        private CommonOpenCrmWebSiteCommand(OleMenuCommandService commandService, int baseIdStart, OpenCrmWebSiteType crmWebSiteType)
            : base(commandService, baseIdStart)
        {
            this._crmWebSiteType = crmWebSiteType;
        }

        public static List<CommonOpenCrmWebSiteCommand> Instances { get; private set; } = new List<CommonOpenCrmWebSiteCommand>();

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instances.AddRange(new[]
            {
                new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenCrmWebSiteCommandId, OpenCrmWebSiteType.CrmWebApplication)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenAdvancedFindCommandId, OpenCrmWebSiteType.AdvancedFind)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenDashboardsCommandId, OpenCrmWebSiteType.Dashboards)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenActivitiesCommandId, OpenCrmWebSiteType.Activities)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenSolutionsCommandId, OpenCrmWebSiteType.Solutions)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenWorkflowsCommandId, OpenCrmWebSiteType.Workflows)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenSystemJobsCommandId, OpenCrmWebSiteType.SystemJobs)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenTraceWallCommandId, OpenCrmWebSiteType.TraceWall)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenCustomizationCommandId, OpenCrmWebSiteType.Customization)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenSystemUsersCommandId, OpenCrmWebSiteType.SystemUsers)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenTeamsCommandId, OpenCrmWebSiteType.Teams)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenRolesCommandId, OpenCrmWebSiteType.Roles)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenSecurityCommandId, OpenCrmWebSiteType.Security)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenAdministrationCommandId, OpenCrmWebSiteType.Administration)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenEngagementHubCommandId, OpenCrmWebSiteType.EngagementHub)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenBusinessCommandId, OpenCrmWebSiteType.Business)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenTemplatesCommandId, OpenCrmWebSiteType.Templates)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenProductCatalogCommandId, OpenCrmWebSiteType.ProductCatalog)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenServiceManagementCommandId, OpenCrmWebSiteType.ServiceManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenDataManagementCommandId, OpenCrmWebSiteType.DataManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenDocumentManagementCommandId, OpenCrmWebSiteType.DocumentManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenDuplicateDetectionJobsCommandId, OpenCrmWebSiteType.DuplicateDetectionJobs)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenSocialCommandId, OpenCrmWebSiteType.Social)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenAuditCommandId, OpenCrmWebSiteType.Audit)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenMobileOfflineCommandId, OpenCrmWebSiteType.MobileOffline)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenExternAppManagementCommandId, OpenCrmWebSiteType.ExternAppManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenAppsForCrmCommandId, OpenCrmWebSiteType.AppsForCrm)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenRelationshipIntelligenceCommandId, OpenCrmWebSiteType.RelationshipIntelligence)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenMicrosoftFlowCommandId, OpenCrmWebSiteType.MicrosoftFlow)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenAppModuleCommandId, OpenCrmWebSiteType.AppModule)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.guidDynamicCommandSet.CommonOpenNewsCommandId, OpenCrmWebSiteType.News)
            });
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenCrmWebSiteInWeb(connectionData, _crmWebSiteType);
        }
    }
}