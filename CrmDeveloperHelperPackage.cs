using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.ToolWindowPanes;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Threading;

namespace Nav.Common.VSPackages.CrmDeveloperHelper
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuids.guidCrmDeveloperHelperPackageString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(FetchXmlExecutorToolWindowPane), Style = VsDockStyle.Tabbed, MultiInstances = true, DocumentLikeTool = true, Transient = true)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class CrmDeveloperHelperPackage : Package
    {
        public static IServiceProvider ServiceProvider => (IServiceProvider)Singleton;

        public static CrmDeveloperHelperPackage Singleton { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmDeveloperHelperPackage"/> class.
        /// </summary>
        public CrmDeveloperHelperPackage()
        {
            //AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            //System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_DomainUnload;
        }

        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            NLog.LogManager.Shutdown();
        }

        #region Package Members

        protected override void Initialize()
        {
            base.Initialize();

            CodeWebResourceCheckEncodingCommand.Initialize(this);
            CodeWebResourceCompareWithDetailsCommand.Initialize(this);
            CodeWebResourceCompareInConnectionGroupCommand.Initialize(this);
            CodeWebResourceShowDifferenceCommand.Initialize(this);
            CodeWebResourceShowDifferenceCustomCommand.Initialize(this);
            CodeWebResourceShowDifferenceInConnectionGroupCommand.Initialize(this);
            CodeWebResourceShowDifferenceThreeFileCommand.Initialize(this);
            CodeWebResourceDownloadCommand.Initialize(this);
            CodeWebResourceOpenInWebCommand.Initialize(this);
            CodeWebResourceLinkClearCommand.Initialize(this);
            CodeWebResourceLinkCreateCommand.Initialize(this);
            CodeWebResourceShowDependentComponentsCommand.Initialize(this);
            CodeWebResourceUpdateContentPublishCommand.Initialize(this);
            CodeWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(this);
            CodeWebResourceAddIntoSolutionCommand.Initialize(this);
            CodeWebResourceAddIntoSolutionLastCommand.Initialize(this);

            CodeXmlExecuteFetchXmlRequestCommand.Initialize(this);
            CodeXmlExecuteFetchXmlRequestInConnectionsCommand.Initialize(this);
            CodeXmlConvertFetchXmlToJavaScriptCodeCommand.Initialize(this);
            CodeXmlOpenXsdSchemaFolderCommand.Initialize(this);
            CodeXmlSetXsdSchemaCommand.Initialize(this);
            CodeXmlRemoveXsdSchemaCommand.Initialize(this);
            CodeXmlRibbonDiffInsertIntellisenseContextCommand.Initialize(this);
            CodeXmlRibbonDiffRemoveIntellisenseContextCommand.Initialize(this);

            CodeXmlShowDifferenceSiteMapDefaultCommand.Initialize(this);
            CodeXmlShowDifferenceSiteMapCommand.Initialize(this);
            CodeXmlShowDifferenceSiteMapInConnectionGroupCommand.Initialize(this);

            CodeXmlUpdateSiteMapCommand.Initialize(this);
            CodeXmlUpdateSiteMapInConnectionGroupCommand.Initialize(this);

            CodeXmlShowDifferenceSystemFormCommand.Initialize(this);
            CodeXmlShowDifferenceSystemFormInConnectionGroupCommand.Initialize(this);

            CodeXmlUpdateSystemFormCommand.Initialize(this);
            CodeXmlUpdateSystemFormInConnectionGroupCommand.Initialize(this);

            CodeXmlShowDifferenceSavedQueryCommand.Initialize(this);
            CodeXmlShowDifferenceSavedQueryInConnectionGroupCommand.Initialize(this);

            CodeXmlUpdateSavedQueryCommand.Initialize(this);
            CodeXmlUpdateSavedQueryInConnectionGroupCommand.Initialize(this);

            CodeXmlShowDifferenceRibbonCommand.Initialize(this);
            CodeXmlShowDifferenceRibbonInConnectionGroupCommand.Initialize(this);
            CodeXmlShowDifferenceRibbonDiffXmlCommand.Initialize(this);
            CodeXmlShowDifferenceRibbonDiffXmlInConnectionGroupCommand.Initialize(this);

            CodeXmlUpdateRibbonDiffXmlCommand.Initialize(this);
            CodeXmlUpdateRibbonDiffXmlInConnectionGroupCommand.Initialize(this);

            CodePublishListAddCommand.Initialize(this);
            CodePublishListRemoveCommand.Initialize(this);

            CodeReportLinkClearCommand.Initialize(this);
            CodeReportLinkCreateCommand.Initialize(this);
            CodeReportUpdateCommand.Initialize(this);
            CodeReportShowDifferenceCommand.Initialize(this);
            CodeReportShowDifferenceInConnectionGroupCommand.Initialize(this);
            CodeReportShowDifferenceThreeFileCommand.Initialize(this);
            CodeReportDownloadCommand.Initialize(this);
            CodeReportOpenInWebCommand.Initialize(this);
            CodeReportAddIntoSolutionCommand.Initialize(this);
            CodeReportAddIntoSolutionLastCommand.Initialize(this);

            CodeCSharpUpdateEntityMetadataFileCommand.Initialize(this);
            CodeCSharpUpdateEntityMetadataFileWithEntitySelectCommand.Initialize(this);
            CodeCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(this);
            CodeCSharpUpdateProxyClassesCommand.Initialize(this);

            CodeCSharpProjectCompareToCrmAssemblyCommand.Initialize(this);
            CodeCSharpProjectCompareToCrmAssemblyInConnectionGroupCommand.Initialize(this);

            CodeCSharpPluginTypeDescriptionCommand.Initialize(this);
            CodeCSharpPluginAssemblyDescriptionCommand.Initialize(this);
            CodeCSharpPluginTreeCommand.Initialize(this);

            CodeCSharpProjectPluginAssemblyAddIntoSolutionCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginTypeStepsAddIntoSolutionCommand.Initialize(this);
            CodeCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);





            FileWebResourceCheckEncodingCommand.Initialize(this);
            FileWebResourceCheckEncodingCompareFilesCommand.Initialize(this);
            FileWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(this);
            FileWebResourceCheckEncodingOpenFilesCommand.Initialize(this);
            FileWebResourceCompareCommand.Initialize(this);
            FileWebResourceCompareWithDetailsCommand.Initialize(this);
            FileWebResourceCompareInConnectionGroupCommand.Initialize(this);
            FileWebResourceDownloadCommand.Initialize(this);
            FileWebResourceOpenInWebCommand.Initialize(this);
            FileWebResourceLinkClearCommand.Initialize(this);
            FileWebResourceLinkCreateCommand.Initialize(this);
            FileWebResourceShowDependentComponentsCommand.Initialize(this);
            FileWebResourceShowDifferenceCommand.Initialize(this);
            FileWebResourceShowDifferenceCustomCommand.Initialize(this);
            FileWebResourceShowDifferenceInConnectionGroupCommand.Initialize(this);
            FileWebResourceShowDifferenceThreeFileCommand.Initialize(this);
            FileWebResourceUpdateContentPublishCommand.Initialize(this);
            FileWebResourceUpdateContentPublishEqualByTextCommand.Initialize(this);
            FileWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(this);
            FileWebResourceAddIntoSolutionCommand.Initialize(this);
            FileWebResourceAddIntoSolutionLastCommand.Initialize(this);

            FileReportDownloadCommand.Initialize(this);
            FileReportLinkClearCommand.Initialize(this);
            FileReportLinkCreateCommand.Initialize(this);
            FileReportUpdateCommand.Initialize(this);
            FileReportOpenInWebCommand.Initialize(this);
            FileReportAddIntoSolutionCommand.Initialize(this);
            FileReportAddIntoSolutionLastCommand.Initialize(this);

            FileCSharpUpdateEntityMetadataFileCommand.Initialize(this);
            FileCSharpUpdateEntityMetadataFileWithEntitySelectCommand.Initialize(this);
            FileCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(this);
            FileCSharpUpdateProxyClassesCommand.Initialize(this);
            FileCSharpPluginAssemblyDescriptionCommand.Initialize(this);
            FileCSharpProjectCompareToCrmAssemblyCommand.Initialize(this);
            FileCSharpProjectCompareToCrmAssemblyInConnectionGroupCommand.Initialize(this);
            FileCSharpPluginTypeDescriptionCommand.Initialize(this);
            FileCSharpPluginTreeCommand.Initialize(this);

            FileCSharpProjectPluginAssemblyAddIntoSolutionCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginTypeStepsAddIntoSolutionCommand.Initialize(this);
            FileCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);



            DocumentsWebResourceCheckEncodingCommand.Initialize(this);
            DocumentsWebResourceCheckEncodingCompareFilesCommand.Initialize(this);
            DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(this);
            DocumentsWebResourceCompareCommand.Initialize(this);
            DocumentsWebResourceCompareWithDetailsCommand.Initialize(this);
            DocumentsWebResourceCompareInConnectionGroupCommand.Initialize(this);
            DocumentsWebResouceLinkClearCommand.Initialize(this);
            DocumentsWebResouceLinkCreateCommand.Initialize(this);
            DocumentsWebResourceShowDependentComponentsCommand.Initialize(this);
            DocumentsWebResourceUpdateContentPublishEqualByTextCommand.Initialize(this);
            DocumentsWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(this);
            DocumentsWebResourceAddIntoSolutionCommand.Initialize(this);
            DocumentsWebResourceAddIntoSolutionLastCommand.Initialize(this);

            DocumentsReportLinkClearCommand.Initialize(this);
            DocumentsReportAddIntoSolutionCommand.Initialize(this);
            DocumentsReportAddIntoSolutionLastCommand.Initialize(this);

            DocumentsCSharpUpdateEntityMetadataFileCommand.Initialize(this);
            DocumentsCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommand.Initialize(this);
            DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);








            FolderAddPluginConfigurationFileCommand.Initialize(this);
            FolderAddSolutionFileCommand.Initialize(this);

            FolderWebResourceCheckEncodingCommand.Initialize(this);
            FolderWebResourceCheckEncodingCompareFilesCommand.Initialize(this);
            FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(this);
            FolderWebResourceCheckEncodingOpenFilesCommand.Initialize(this);
            FolderWebResourceCompareCommand.Initialize(this);
            FolderWebResourceCompareWithDetailsCommand.Initialize(this);
            FolderWebResourceCompareInConnectionGroupCommand.Initialize(this);
            FolderWebResourceLinkClearCommand.Initialize(this);
            FolderWebResourceLinkCreateCommand.Initialize(this);
            FolderWebResourceShowDependentComponentsCommand.Initialize(this);
            FolderWebResourceUpdateContentPublishCommand.Initialize(this);
            FolderWebResourceUpdateContentPublishEqualByTextCommand.Initialize(this);
            FolderWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(this);
            FolderWebResourceAddIntoSolutionCommand.Initialize(this);
            FolderWebResourceAddIntoSolutionLastCommand.Initialize(this);

            FolderCSharpUpdateEntityMetadataFileCommand.Initialize(this);
            FolderCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);


            ProjectCompareToCrmAssemblyCommand.Initialize(this);
            ProjectCompareToCrmAssemblyInConnectionGroupCommand.Initialize(this);

            ProjectPluginAssemblyAddIntoSolutionCommand.Initialize(this);
            ProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            ProjectPluginAssemblyStepsAddIntoSolutionCommand.Initialize(this);
            ProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            ProjectPluginAssemblyDescriptionCommand.Initialize(this);
            ProjectPluginTypeDescriptionCommand.Initialize(this);
            ProjectPluginTreeCommand.Initialize(this);



            ListForPublishClearListCommand.Initialize(this);
            ListForPublishCompareCommand.Initialize(this);
            ListForPublishCompareWithDetailsCommand.Initialize(this);
            ListForPublishCompareInConnectionGroupCommand.Initialize(this);
            ListForPublishFilesAddCommand.Initialize(this);
            ListForPublishFilesRemoveCommand.Initialize(this);
            ListForPublishPerformPublishEqualByTextCommand.Initialize(this);
            ListForPublishPerformPublishGroupConnectionCommand.Initialize(this);
            ListForPublishShowListCommand.Initialize(this);
            ListForPublishAddIntoSolutionCommand.Initialize(this);
            ListForPublishAddIntoSolutionLastCommand.Initialize(this);



            CommonCheckEntitiesNamesAndShowDependentComponentsCommand.Initialize(this);
            CommonCheckEntitiesNamesCommand.Initialize(this);
            CommonCheckEntitiesOwnerShipsCommand.Initialize(this);
            CommonCheckWorkflowsUsedEntitiesCommand.Initialize(this);
            CommonCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize(this);
            CommonCheckGlobalOptionSetDuplicateCommand.Initialize(this);
            CommonCheckManagedElementsCommand.Initialize(this);
            CommonCheckMarkedToDeleteAndShowDependentComponentsCommand.Initialize(this);
            CommonCheckPluginImagesCommand.Initialize(this);
            CommonCheckPluginImagesRequiredComponentsCommand.Initialize(this);
            CommonCheckPluginStepsCommand.Initialize(this);
            CommonCheckPluginStepsRequiredComponentsCommand.Initialize(this);
            CommonOpenCrmWebSiteCommand.Initialize(this);
            CommonOpenCurrentConnectionCrmInWebCommand.Initialize(this);
            CommonConfigCommand.Initialize(this);
            CommonPublishAllInCrmConnectionCommand.Initialize(this);
            CommonCrmConnectionCommand.Initialize(this);
            CommonCrmConnectionTestCommand.Initialize(this);
            CommonExportEntityMetadataCommand.Initialize(this);
            CommonEntityAttributeExplorerCommand.Initialize(this);
            CommonEntityKeyExplorerCommand.Initialize(this);
            CommonEntityRelationshipOneToManyExplorerCommand.Initialize(this);
            CommonEntityRelationshipManyToManyExplorerCommand.Initialize(this);
            CommonEntitySecurityRolesExplorerCommand.Initialize(this);
            CommonExportFormEventsCommand.Initialize(this);
            CommonExportGlobalOptionSetsCommand.Initialize(this);
            CommonExportOrganizationCommand.Initialize(this);
            CommonExportPluginAssemblyDescriptionCommand.Initialize(this);
            CommonExportPluginTypeDescriptionCommand.Initialize(this);
            CommonExportReportCommand.Initialize(this);
            CommonExportApplicationRibbonXmlCommand.Initialize(this);
            CommonExportSiteMapCommand.Initialize(this);
            CommonExportSolutionComponentsCommand.Initialize(this);
            CommonOpenSolutionImageCommand.Initialize(this);
            CommonExportSystemFormXmlCommand.Initialize(this);
            CommonExportSystemSavedQueryVisualizationXmlCommand.Initialize(this);
            CommonExportSystemSavedQueryXmlCommand.Initialize(this);
            CommonExportWebResourceCommand.Initialize(this);
            CommonExportWorkflowCommand.Initialize(this);

            CommonSystemUsersExplorerCommand.Initialize(this);
            CommonTeamsExplorerCommand.Initialize(this);
            CommonSecurityRolesExplorerCommand.Initialize(this);

            CommonExportDefaultSitemapsCommand.Initialize(this);
            CommonExportXsdSchemasCommand.Initialize(this);
            CommonOpenXsdSchemaFolderCommand.Initialize(this);

            CommonFindEntityObjectsByNameCommand.Initialize(this);
            CommonFindEntityObjectsContainsStringCommand.Initialize(this);
            CommonFindEntityObjectsByIdCommand.Initialize(this);
            CommonFindEntityObjectsByUniqueidentifierCommand.Initialize(this);
            CommonOrganizationComparerCommand.Initialize(this);
            CommonOpenOrganizationDifferenceImageCommand.Initialize(this);
            CommonTraceReaderCommand.Initialize(this);
            CommonPluginConfigurationComparerPluginAssemblyCommand.Initialize(this);
            CommonPluginConfigurationCreateCommand.Initialize(this);
            CommonPluginConfigurationPluginAssemblyCommand.Initialize(this);
            CommonPluginConfigurationPluginTreeCommand.Initialize(this);
            CommonPluginConfigurationPluginTypeCommand.Initialize(this);
            CommonPluginTreeCommand.Initialize(this);
            CommonSdkMessageTreeCommand.Initialize(this);
            CommonSdkMessageRequestTreeCommand.Initialize(this);
            CommonExportOpenLastSelectedSolutionCommand.Initialize(this);
            CommonCheckComponentTypeEnumCommand.Initialize(this);
            CommonCreateAllDependencyNodeDescriptionCommand.Initialize(this);





            OpenFilesCommand.Initialize(this);
            MultiDifferenceCommand.Initialize(this);
            AddFilesIntoListForPublishCommand.Initialize(this);

            CommonSelectCrmConnectionCommand.Initialize(this);


            //Folder.Initialize(this);
            //Folder.Initialize(this);
            //Folder.Initialize(this);
            //Folder.Initialize(this);
            //Folder.Initialize(this);

            Singleton = this;

            System.Threading.Tasks.Task.Run(() => Model.CommonConfiguration.Get());
            System.Threading.Tasks.Task.Run(() => Model.ConnectionConfiguration.Get());

            //Repository.ConnectionIntellisenseDataRepository.LoadIntellisenseCache();
        }

        internal void ExecuteFetchXmlQueryAsync(string filePath, ConnectionData connectionData)
        {
            FetchXmlExecutorToolWindowPane paneForFileAndConnection = FindOrCreateFetchXmlExecutorToolWindowPane(filePath, connectionData);

            if (paneForFileAndConnection != null)
            {
                paneForFileAndConnection.Execute();

                (paneForFileAndConnection.Frame as IVsWindowFrame).Show();
            }
        }

        private const int countPanes = 100;

        private FetchXmlExecutorToolWindowPane FindOrCreateFetchXmlExecutorToolWindowPane(string filePath, ConnectionData connectionData)
        {
            int? num = null;

            List<FetchXmlExecutorToolWindowPane> panes = new List<FetchXmlExecutorToolWindowPane>();

            for (int i = 0; i < countPanes; i++)
            {
                var pane = FindToolWindow(typeof(FetchXmlExecutorToolWindowPane), i, false) as FetchXmlExecutorToolWindowPane;

                if (pane == null)
                {
                    if (!num.HasValue)
                    {
                        num = i;
                    }

                    continue;
                }

                if (pane.Frame != null)
                {
                    if (string.Equals(pane.FilePath, filePath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return pane;
                    }
                }
            }

            if (num.HasValue)
            {
                FetchXmlExecutorToolWindowPane paneForFileAndConnection = FindToolWindow(typeof(FetchXmlExecutorToolWindowPane), num.Value, true) as FetchXmlExecutorToolWindowPane;

                paneForFileAndConnection.SetSource(filePath, connectionData);

                return paneForFileAndConnection;
            }

            return null;
        }

        #endregion
    }
}
