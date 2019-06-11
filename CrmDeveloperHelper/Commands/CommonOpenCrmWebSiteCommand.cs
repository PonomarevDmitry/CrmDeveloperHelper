using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenCrmWebSiteCommand : AbstractCommandByConnectionAll
    {
        private readonly OpenCrmWebSiteType _crmWebSiteType;

        private CommonOpenCrmWebSiteCommand(OleMenuCommandService commandService, int baseIdStart, OpenCrmWebSiteType crmWebSiteType)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._crmWebSiteType = crmWebSiteType;
        }

        public static List<CommonOpenCrmWebSiteCommand> Instances { get; private set; } = new List<CommonOpenCrmWebSiteCommand>();

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instances.AddRange(new[]
            {
                new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenCrmWebSiteCommandId, OpenCrmWebSiteType.CrmWebApplication)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenAdvancedFindCommandId, OpenCrmWebSiteType.AdvancedFind)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenSolutionsCommandId, OpenCrmWebSiteType.Solutions)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenWorkflowsCommandId, OpenCrmWebSiteType.Workflows)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenSystemJobsCommandId, OpenCrmWebSiteType.SystemJobs)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenCustomizationCommandId, OpenCrmWebSiteType.Customization)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenSystemUsersCommandId, OpenCrmWebSiteType.SystemUsers)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenTeamsCommandId, OpenCrmWebSiteType.Teams)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenRolesCommandId, OpenCrmWebSiteType.Roles)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenSecurityCommandId, OpenCrmWebSiteType.Security)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenAdministrationCommandId, OpenCrmWebSiteType.Administration)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenEngagementHubCommandId, OpenCrmWebSiteType.EngagementHub)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenBusinessCommandId, OpenCrmWebSiteType.Business)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenTemplatesCommandId, OpenCrmWebSiteType.Templates)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenProductCatalogCommandId, OpenCrmWebSiteType.ProductCatalog)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenServiceManagementCommandId, OpenCrmWebSiteType.ServiceManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenDataManagementCommandId, OpenCrmWebSiteType.DataManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenSocialCommandId, OpenCrmWebSiteType.Social)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenAuditCommandId, OpenCrmWebSiteType.Audit)

                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenMobileOfflineCommandId, OpenCrmWebSiteType.MobileOffline)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenExternAppManagementCommandId, OpenCrmWebSiteType.ExternAppManagement)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenAppsForCrmCommandId, OpenCrmWebSiteType.AppsForCrm)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenRelationshipIntelligenceCommandId, OpenCrmWebSiteType.RelationshipIntelligence)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenMicrosoftFlowCommandId, OpenCrmWebSiteType.MicrosoftFlow)
                , new CommonOpenCrmWebSiteCommand(commandService, PackageIds.CommonOpenAppModuleCommandId, OpenCrmWebSiteType.AppModule)
            });
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenCrmInWeb(connectionData, _crmWebSiteType);
        }
    }
}