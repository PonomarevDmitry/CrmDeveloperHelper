using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    public sealed class CommonOpenCrmWebSiteCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private readonly OpenCrmWebSiteType _crmWebSiteType;

        private readonly int _baseIdStart;

        private CommonOpenCrmWebSiteCommand(Package package, int baseIdStart, OpenCrmWebSiteType crmWebSiteType)
        {
            this._crmWebSiteType = crmWebSiteType;
            this._baseIdStart = baseIdStart;

            this._package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                for (int i = 0; i < ConnectionData.CountConnectionToQuickList; i++)
                {
                    var menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                    var menuCommand = new OleMenuCommand(this.menuItemCallback, menuCommandID);

                    menuCommand.Enabled = menuCommand.Visible = false;

                    menuCommand.BeforeQueryStatus += menuItem_BeforeQueryStatus;

                    commandService.AddCommand(menuCommand);
                }
            }
        }

        public static List<CommonOpenCrmWebSiteCommand> Instances { get; private set; } = new List<CommonOpenCrmWebSiteCommand>();

        public static void Initialize(Package package)
        {
            Instances.AddRange(new[]
            {
                new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenCrmWebSiteCommandId, OpenCrmWebSiteType.CrmWebApplication)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenAdvancedFindCommandId, OpenCrmWebSiteType.AdvancedFind)

                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenSolutionsCommandId, OpenCrmWebSiteType.Solutions)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenWorkflowsCommandId, OpenCrmWebSiteType.Workflows)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenSystemJobsCommandId, OpenCrmWebSiteType.SystemJobs)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenCustomizationCommandId, OpenCrmWebSiteType.Customization)

                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenSystemUsersCommandId, OpenCrmWebSiteType.SystemUsers)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenTeamsCommandId, OpenCrmWebSiteType.Teams)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenRolesCommandId, OpenCrmWebSiteType.Roles)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenSecurityCommandId, OpenCrmWebSiteType.Security)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenAdministrationCommandId, OpenCrmWebSiteType.Administration)

                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenEngagementHubCommandId, OpenCrmWebSiteType.EngagementHub)

                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenBusinessCommandId, OpenCrmWebSiteType.Business)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenTemplatesCommandId, OpenCrmWebSiteType.Templates)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenProductCatalogCommandId, OpenCrmWebSiteType.ProductCatalog)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenServiceManagementCommandId, OpenCrmWebSiteType.ServiceManagement)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenDataManagementCommandId, OpenCrmWebSiteType.DataManagement)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenSocialCommandId, OpenCrmWebSiteType.Social)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenAuditCommandId, OpenCrmWebSiteType.Audit)

                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenMobileOfflineCommandId, OpenCrmWebSiteType.MobileOffline)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenExternAppManagementCommandId, OpenCrmWebSiteType.ExternAppManagement)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenAppsForCrmCommandId, OpenCrmWebSiteType.AppsForCrm)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenRelationshipIntelligenceCommandId, OpenCrmWebSiteType.RelationshipIntelligence)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenMicrosoftFlowCommandId, OpenCrmWebSiteType.MicrosoftFlow)
                , new CommonOpenCrmWebSiteCommand(package, PackageIds.CommonOpenAppModuleCommandId, OpenCrmWebSiteType.AppModule)
            });
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var connectionConfig = Model.ConnectionConfiguration.Get();

                    if (0 <= index && index < connectionConfig.Connections.Count)
                    {
                        var connectionData = connectionConfig.Connections[index];

                        menuCommand.Text = connectionData.NameWithCurrentMark;

                        menuCommand.Enabled = menuCommand.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }

        private void menuItemCallback(object sender, EventArgs e)
        {
            try
            {
                OleMenuCommand menuCommand = sender as OleMenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                var applicationObject = this.ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var connectionConfig = Model.ConnectionConfiguration.Get();

                if (0 <= index && index < connectionConfig.Connections.Count)
                {
                    var connectionData = connectionConfig.Connections[index];

                    var helper = DTEHelper.Create(applicationObject);

                    helper.HandleOpenCrmInWeb(connectionData, _crmWebSiteType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}