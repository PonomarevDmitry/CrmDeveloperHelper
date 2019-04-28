using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.ToolWindowPanes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

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
        public static IServiceProvider ServiceProvider => Singleton;

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

            CodeWebResourceAddIntoSolutionLastCommand.Initialize(this);
            CodeWebResourceAddIntoSolutionInConnectionCommand.Initialize(this);

            CodeJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);
            CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(this);

            CodeXmlExecuteFetchXmlRequestCommand.Initialize(this);
            CodeXmlExecuteFetchXmlRequestInConnectionsCommand.Initialize(this);
            CodeXmlConvertFetchXmlToJavaScriptCodeCommand.Initialize(this);
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

            CodeXmlOpenXsdSchemaFolderCommand.Initialize(this);
            DocumentsXmlOpenXsdSchemaFolderCommand.Initialize(this);
            FileXmlOpenXsdSchemaFolderCommand.Initialize(this);
            FolderXmlOpenXsdSchemaFolderCommand.Initialize(this);

            CodeXmlSetXsdSchemaCommand.Initialize(this);
            DocumentsXmlSetXsdSchemaCommand.Initialize(this);
            FileXmlSetXsdSchemaCommand.Initialize(this);
            FolderXmlSetXsdSchemaCommand.Initialize(this);

            CodeXmlRemoveXsdSchemaCommand.Initialize(this);
            DocumentsXmlRemoveXsdSchemaCommand.Initialize(this);
            FileXmlRemoveXsdSchemaCommand.Initialize(this);
            FolderXmlRemoveXsdSchemaCommand.Initialize(this);

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

            CodeReportAddIntoSolutionLastCommand.Initialize(this);
            CodeReportAddIntoSolutionInConnectionCommand.Initialize(this);

            CodeCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(this);

            CodeCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            CodeCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(this);

            CodeCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(this);
            CodeCSharpUpdateProxyClassesCommand.Initialize(this);

            CodeCSharpProjectUpdatePluginAssemblyCommand.Initialize(this);
            CodeCSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);

            CodeCSharpProjectCompareToCrmAssemblyCommand.Initialize(this);
            CodeCSharpProjectCompareToCrmAssemblyInConnectionCommand.Initialize(this);

            CodeCSharpPluginTypeDescriptionCommand.Initialize(this);
            CodeCSharpPluginAssemblyDescriptionCommand.Initialize(this);
            CodeCSharpPluginTreeCommand.Initialize(this);

            CodeCSharpAddPluginStepCommand.Initialize(this);
            CodeCSharpAddPluginStepInConnectionCommand.Initialize(this);

            CodeCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyAddIntoSolutionInConnectionCommand.Initialize(this);

            CodeCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyStepsAddIntoSolutionInConnectionCommand.Initialize(this);

            CodeCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginTypeStepsAddIntoSolutionInConnectionCommand.Initialize(this);





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

            FileWebResourceAddIntoSolutionLastCommand.Initialize(this);
            FileWebResourceAddIntoSolutionInConnectionCommand.Initialize(this);

            FileJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);
            FileJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(this);

            FileReportDownloadCommand.Initialize(this);
            FileReportLinkClearCommand.Initialize(this);
            FileReportLinkCreateCommand.Initialize(this);
            FileReportUpdateCommand.Initialize(this);
            FileReportOpenInWebCommand.Initialize(this);

            FileReportAddIntoSolutionLastCommand.Initialize(this);
            FileReportAddIntoSolutionInConnectionCommand.Initialize(this);

            FileCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(this);

            FileCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(this);

            FileCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(this);
            FileCSharpUpdateProxyClassesCommand.Initialize(this);
            FileCSharpPluginAssemblyDescriptionCommand.Initialize(this);
            FileCSharpProjectUpdatePluginAssemblyCommand.Initialize(this);
            FileCSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);
            FileCSharpProjectCompareToCrmAssemblyCommand.Initialize(this);
            FileCSharpProjectCompareToCrmAssemblyInConnectionCommand.Initialize(this);
            FileCSharpPluginTypeDescriptionCommand.Initialize(this);
            FileCSharpPluginTreeCommand.Initialize(this);

            FileCSharpAddPluginStepCommand.Initialize(this);
            FileCSharpAddPluginStepInConnectionCommand.Initialize(this);

            FileCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyAddIntoSolutionInConnectionCommand.Initialize(this);

            FileCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyStepsAddIntoSolutionInConnectionCommand.Initialize(this);

            FileCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginTypeStepsAddIntoSolutionInConnectionCommand.Initialize(this);



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

            DocumentsWebResourceAddIntoSolutionLastCommand.Initialize(this);
            DocumentsWebResourceAddIntoSolutionInConnectionCommand.Initialize(this);

            DocumentsJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);

            DocumentsReportLinkClearCommand.Initialize(this);

            DocumentsReportAddIntoSolutionLastCommand.Initialize(this);
            DocumentsReportAddIntoSolutionInConnectionCommand.Initialize(this);

            DocumentsCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            DocumentsCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            DocumentsCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);

            DocumentsCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyAddIntoSolutionInConnectionCommand.Initialize(this);

            DocumentsCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyStepsAddIntoSolutionInConnectionCommand.Initialize(this);

            DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionInConnectionCommand.Initialize(this);








            FolderAddPluginConfigurationFileCommand.Initialize(this);
            FolderAddSolutionFileCommand.Initialize(this);
            FolderAddEntityMetadataFileInConnectionCommand.Initialize(this);





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

            FolderWebResourceAddIntoSolutionLastCommand.Initialize(this);
            FolderWebResourceAddIntoSolutionInConnectionCommand.Initialize(this);

            FolderJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);

            FolderCSharpProjectPluginAssemblyAddIntoSolutionInConnectionCommand.Initialize(this);
            FolderCSharpProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);

            FolderCSharpProjectPluginAssemblyStepsAddIntoSolutionInConnectionCommand.Initialize(this);
            FolderCSharpProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);

            FolderCSharpProjectPluginTypeStepsAddIntoSolutionInConnectionCommand.Initialize(this);
            FolderCSharpProjectPluginTypeStepsAddIntoSolutionLastCommand.Initialize(this);

            FolderCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            FolderCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            FolderCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);


            ProjectUpdatePluginAssemblyCommand.Initialize(this);
            ProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);

            ProjectRegisterPluginAssemblyInConnectionCommand.Initialize(this);

            ProjectCompareToCrmAssemblyCommand.Initialize(this);
            ProjectCompareToCrmAssemblyInConnectionCommand.Initialize(this);

            ProjectPluginAssemblyAddIntoSolutionLastCommand.Initialize(this);
            ProjectPluginAssemblyAddIntoSolutionInConnectionCommand.Initialize(this);

            ProjectPluginAssemblyStepsAddIntoSolutionLastCommand.Initialize(this);
            ProjectPluginAssemblyStepsAddIntoSolutionInConnectionCommand.Initialize(this);

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

            ListForPublishAddIntoSolutionLastCommand.Initialize(this);
            ListForPublishAddIntoSolutionInConnectionCommand.Initialize(this);



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
            CommonCurrentConnectionCommand.Initialize(this);
            CommonOpenConfigFolderCommand.Initialize(this);
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

            CommonSolutionExplorerCommand.Initialize(this);
            CommonSolutionExplorerInConnectionCommand.Initialize(this);
            CommonImportJobExplorerInConnectionCommand.Initialize(this);

            CommonOpenSolutionImageCommand.Initialize(this);
            CommonOpenSolutionDifferenceImageCommand.Initialize(this);
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
            CommonEditEntityObjectsByIdCommand.Initialize(this);
            CommonFindEntityObjectsByUniqueidentifierCommand.Initialize(this);
            CommonOrganizationComparerCommand.Initialize(this);
            CommonOpenOrganizationDifferenceImageCommand.Initialize(this);
            CommonTraceExportFileCommand.Initialize(this);
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
            CommonOpenDefaultSolutionInWebCommand.Initialize(this);




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

        internal void ExecuteFetchXmlQueryAsync(string filePath, ConnectionData connectionData, IWriteToOutput iWriteToOutput, bool strictConnection)
        {
            var panes = FindOrCreateFetchXmlExecutorToolWindowPane(filePath);

            var connectionPane = panes.FirstOrDefault(p => p.ConnectionData.ConnectionId == connectionData.ConnectionId);

            if (!panes.Any() || (strictConnection && connectionPane == null))
            {
                connectionPane = FindToolWindow(typeof(FetchXmlExecutorToolWindowPane), GetNextPaneId(), true) as FetchXmlExecutorToolWindowPane;
                connectionPane.SetSource(filePath, connectionData, iWriteToOutput);

                panes.Add(connectionPane);
            }

            foreach (var item in panes)
            {
                item.Execute();
            }

            if (connectionPane != null)
            {
                (connectionPane.Frame as IVsWindowFrame)?.Show();
            }
            else
            {
                (panes.FirstOrDefault()?.Frame as IVsWindowFrame)?.Show();
            }
        }

        private const int countPanes = 500;

        private List<FetchXmlExecutorToolWindowPane> FindOrCreateFetchXmlExecutorToolWindowPane(string filePath)
        {
            List<FetchXmlExecutorToolWindowPane> panes = new List<FetchXmlExecutorToolWindowPane>();

            for (int i = 0; i < countPanes; i++)
            {
                var pane = FindToolWindow(typeof(FetchXmlExecutorToolWindowPane), i, false) as FetchXmlExecutorToolWindowPane;

                if (pane != null && pane.Frame != null)
                {
                    if (string.Equals(pane.FilePath, filePath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        panes.Add(pane);
                    }
                }
            }

            return panes;
        }

        private int GetNextPaneId()
        {
            int num = 0;

            while (true)
            {
                var pane = FindToolWindow(typeof(FetchXmlExecutorToolWindowPane), num, false) as FetchXmlExecutorToolWindowPane;

                if (pane == null)
                {
                    return num;
                }

                num++;
            }
        }

        #endregion
    }
}
