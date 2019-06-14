using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.ToolWindowPanes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nav.Common.VSPackages.CrmDeveloperHelper
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]

    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About

    [Guid(PackageGuids.guidCrmDeveloperHelperPackageString)]

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]

    [ProvideMenuResource("Menus.ctmenu", 1)]

    [ProvideToolWindow(typeof(FetchXmlExecutorToolWindowPane), Style = VsDockStyle.Tabbed, MultiInstances = true, DocumentLikeTool = true, Transient = true)]

    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]

    public sealed class CrmDeveloperHelperPackage : AsyncPackage
    {
        public static CrmDeveloperHelperPackage Singleton { get; private set; }

        private EnvDTE80.DTE2 _applicationObject;

        public EnvDTE80.DTE2 ApplicationObject
        {
            get
            {
                if (_applicationObject == null)
                {
                    _applicationObject = this.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                }

                return _applicationObject;
            }
        }

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

        protected override async System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, we *may* be on a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            // Otherwise, remove the switch to the UI thread if you don't need it.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            OleMenuCommandService commandService = await this.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            CodeWebResourceCheckEncodingCommand.Initialize(commandService);
            CodeWebResourceCompareWithDetailsCommand.Initialize(commandService);
            CodeWebResourceCompareWithDetailsInConnectionGroupCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceCustomCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceInConnectionGroupCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceThreeFileCommand.Initialize(commandService);
            CodeWebResourceExplorerCommand.Initialize(commandService);
            CodeWebResourceOpenInWebCommand.Initialize(commandService);
            CodeWebResourceLinkClearCommand.Initialize(commandService);
            CodeWebResourceLinkCreateCommand.Initialize(commandService);
            CodeWebResourceUpdateContentPublishCommand.Initialize(commandService);
            CodeWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(commandService);

            CodeWebResourceAddToSolutionLastCommand.Initialize(commandService);
            CodeWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(commandService);

            CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);
            CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(commandService);

            CodeJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(commandService);

            CodeXmlExecuteFetchXmlRequestCommand.Initialize(commandService);
            CodeXmlExecuteFetchXmlRequestInConnectionsCommand.Initialize(commandService);
            CodeXmlConvertFetchXmlToJavaScriptCodeCommand.Initialize(commandService);
            CodeXmlRibbonDiffInsertIntellisenseContextCommand.Initialize(commandService);
            CodeXmlRibbonDiffRemoveIntellisenseContextCommand.Initialize(commandService);

            CodeXmlSiteMapOpenInWebCommand.Initialize(commandService);
            CodeXmlSiteMapExplorerCommand.Initialize(commandService);
            CodeXmlShowDifferenceSiteMapDefaultCommand.Initialize(commandService);
            CodeXmlShowDifferenceSiteMapCommand.Initialize(commandService);
            CodeXmlShowDifferenceSiteMapInConnectionGroupCommand.Initialize(commandService);
            CodeXmlUpdateSiteMapCommand.Initialize(commandService);
            CodeXmlUpdateSiteMapInConnectionGroupCommand.Initialize(commandService);

            CodeXmlSystemFormExplorerCommand.Initialize(commandService);
            CodeXmlSystemFormOpenInWebCommand.Initialize(commandService);
            CodeXmlShowDifferenceSystemFormCommand.Initialize(commandService);
            CodeXmlShowDifferenceSystemFormInConnectionGroupCommand.Initialize(commandService);
            CodeXmlUpdateSystemFormCommand.Initialize(commandService);
            CodeXmlUpdateSystemFormInConnectionGroupCommand.Initialize(commandService);

            CodeXmlSavedQueryExplorerCommand.Initialize(commandService);
            CodeXmlSavedQueryOpenInWebCommand.Initialize(commandService);
            CodeXmlShowDifferenceSavedQueryCommand.Initialize(commandService);
            CodeXmlShowDifferenceSavedQueryInConnectionGroupCommand.Initialize(commandService);
            CodeXmlUpdateSavedQueryCommand.Initialize(commandService);
            CodeXmlUpdateSavedQueryInConnectionGroupCommand.Initialize(commandService);

            CodeXmlEntityRibbonOpenInWebCommand.Initialize(commandService);
            CodeXmlRibbonExplorerCommand.Initialize(commandService);
            CodeXmlShowDifferenceRibbonCommand.Initialize(commandService);
            CodeXmlShowDifferenceRibbonInConnectionGroupCommand.Initialize(commandService);
            CodeXmlShowDifferenceRibbonDiffXmlCommand.Initialize(commandService);
            CodeXmlShowDifferenceRibbonDiffXmlInConnectionGroupCommand.Initialize(commandService);
            CodeXmlUpdateRibbonDiffXmlCommand.Initialize(commandService);
            CodeXmlUpdateRibbonDiffXmlInConnectionGroupCommand.Initialize(commandService);

            CodeXmlOpenXsdSchemaFolderCommand.Initialize(commandService);
            DocumentsXmlOpenXsdSchemaFolderCommand.Initialize(commandService);
            FileXmlOpenXsdSchemaFolderCommand.Initialize(commandService);
            FolderXmlOpenXsdSchemaFolderCommand.Initialize(commandService);

            CodeXmlSetXsdSchemaCommand.Initialize(commandService);
            DocumentsXmlSetXsdSchemaCommand.Initialize(commandService);
            FileXmlSetXsdSchemaCommand.Initialize(commandService);
            FolderXmlSetXsdSchemaCommand.Initialize(commandService);

            CodeXmlRemoveXsdSchemaCommand.Initialize(commandService);
            DocumentsXmlRemoveXsdSchemaCommand.Initialize(commandService);
            FileXmlRemoveXsdSchemaCommand.Initialize(commandService);
            FolderXmlRemoveXsdSchemaCommand.Initialize(commandService);

            CodePublishListAddCommand.Initialize(commandService);
            CodePublishListRemoveCommand.Initialize(commandService);

            CodeReportLinkClearCommand.Initialize(commandService);
            CodeReportLinkCreateCommand.Initialize(commandService);
            CodeReportUpdateCommand.Initialize(commandService);
            CodeReportCreateCommand.Initialize(commandService);
            CodeReportShowDifferenceCommand.Initialize(commandService);
            CodeReportShowDifferenceInConnectionGroupCommand.Initialize(commandService);
            CodeReportShowDifferenceThreeFileCommand.Initialize(commandService);
            CodeReportExplorerCommand.Initialize(commandService);
            CodeReportOpenInWebCommand.Initialize(commandService);

            CodeReportAddToSolutionLastCommand.Initialize(commandService);
            CodeReportAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(commandService);

            CodeCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);
            CodeCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(commandService);

            CodeCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);
            CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(commandService);

            CodeCSharpProjectUpdatePluginAssemblyCommand.Initialize(commandService);
            CodeCSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectCompareToCrmAssemblyCommand.Initialize(commandService);
            CodeCSharpProjectCompareToCrmAssemblyInConnectionCommand.Initialize(commandService);

            CodeCSharpPluginTypeExplorerCommand.Initialize(commandService);
            CodeCSharpPluginAssemblyExplorerCommand.Initialize(commandService);
            CodeCSharpPluginTreeCommand.Initialize(commandService);

            CodeCSharpAddPluginStepCommand.Initialize(commandService);
            CodeCSharpAddPluginStepInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            CodeCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            CodeCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);
            CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);





            FileWebResourceCheckEncodingCommand.Initialize(commandService);
            FileWebResourceCheckEncodingCompareFilesCommand.Initialize(commandService);
            FileWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(commandService);
            FileWebResourceCheckEncodingOpenFilesCommand.Initialize(commandService);
            FileWebResourceCompareCommand.Initialize(commandService);
            FileWebResourceCompareWithDetailsCommand.Initialize(commandService);
            FileWebResourceCompareInConnectionGroupCommand.Initialize(commandService);
            FileWebResourceExplorerCommand.Initialize(commandService);
            FileWebResourceOpenInWebCommand.Initialize(commandService);
            FileWebResourceLinkClearCommand.Initialize(commandService);
            FileWebResourceLinkCreateCommand.Initialize(commandService);
            FileWebResourceShowDependentComponentsCommand.Initialize(commandService);
            FileWebResourceShowDifferenceCommand.Initialize(commandService);
            FileWebResourceShowDifferenceCustomCommand.Initialize(commandService);
            FileWebResourceShowDifferenceInConnectionGroupCommand.Initialize(commandService);
            FileWebResourceShowDifferenceThreeFileCommand.Initialize(commandService);
            FileWebResourceUpdateContentPublishCommand.Initialize(commandService);
            FileWebResourceUpdateContentPublishEqualByTextCommand.Initialize(commandService);
            FileWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(commandService);

            FileWebResourceAddToSolutionLastCommand.Initialize(commandService);
            FileWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            FileJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            FileJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(commandService);

            FileJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);
            FileJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(commandService);

            FileJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(commandService);

            FileReportExplorerCommand.Initialize(commandService);
            FileReportLinkClearCommand.Initialize(commandService);
            FileReportLinkCreateCommand.Initialize(commandService);
            FileReportUpdateCommand.Initialize(commandService);
            FileReportCreateCommand.Initialize(commandService);
            FileReportOpenInWebCommand.Initialize(commandService);

            FileReportAddToSolutionLastCommand.Initialize(commandService);
            FileReportAddToSolutionInConnectionCommand.Initialize(commandService);

            FileCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(commandService);

            FileCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);
            FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(commandService);

            FileCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);
            FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(commandService);
            FileCSharpPluginAssemblyExplorerCommand.Initialize(commandService);
            FileCSharpProjectUpdatePluginAssemblyCommand.Initialize(commandService);
            FileCSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize(commandService);
            FileCSharpProjectCompareToCrmAssemblyCommand.Initialize(commandService);
            FileCSharpProjectCompareToCrmAssemblyInConnectionCommand.Initialize(commandService);
            FileCSharpPluginTypeExplorerCommand.Initialize(commandService);
            FileCSharpPluginTreeCommand.Initialize(commandService);

            FileCSharpAddPluginStepCommand.Initialize(commandService);
            FileCSharpAddPluginStepInConnectionCommand.Initialize(commandService);

            FileCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            FileCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);
            FileCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);



            DocumentsWebResourceCheckEncodingCommand.Initialize(commandService);
            DocumentsWebResourceCheckEncodingCompareFilesCommand.Initialize(commandService);
            DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(commandService);
            DocumentsWebResourceCompareCommand.Initialize(commandService);
            DocumentsWebResourceCompareWithDetailsCommand.Initialize(commandService);
            DocumentsWebResourceCompareInConnectionGroupCommand.Initialize(commandService);
            DocumentsWebResouceLinkClearCommand.Initialize(commandService);
            DocumentsWebResouceLinkCreateCommand.Initialize(commandService);
            DocumentsWebResourceShowDependentComponentsCommand.Initialize(commandService);
            DocumentsWebResourceUpdateContentPublishEqualByTextCommand.Initialize(commandService);
            DocumentsWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(commandService);

            DocumentsWebResourceAddToSolutionLastCommand.Initialize(commandService);
            DocumentsWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            DocumentsJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);

            DocumentsReportLinkClearCommand.Initialize(commandService);

            DocumentsReportAddToSolutionLastCommand.Initialize(commandService);
            DocumentsReportAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            DocumentsCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);
            DocumentsCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);

            DocumentsCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            DocumentsCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);
            DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);








            FolderAddPluginConfigurationFileCommand.Initialize(commandService);
            FolderAddSolutionFileCommand.Initialize(commandService);
            FolderAddEntityMetadataFileInConnectionCommand.Initialize(commandService);
            FolderAddGlobalOptionSetFileInConnectionCommand.Initialize(commandService);
            FolderAddSdkMessageRequestFileInConnectionCommand.Initialize(commandService);
            FolderAddSystemFormJavaScriptFileInConnectionCommand.Initialize(commandService);



            FolderWebResourceCheckEncodingCommand.Initialize(commandService);
            FolderWebResourceCheckEncodingCompareFilesCommand.Initialize(commandService);
            FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(commandService);
            FolderWebResourceCheckEncodingOpenFilesCommand.Initialize(commandService);
            FolderWebResourceCompareCommand.Initialize(commandService);
            FolderWebResourceCompareWithDetailsCommand.Initialize(commandService);
            FolderWebResourceCompareInConnectionGroupCommand.Initialize(commandService);
            FolderWebResourceLinkClearCommand.Initialize(commandService);
            FolderWebResourceLinkCreateCommand.Initialize(commandService);
            FolderWebResourceShowDependentComponentsCommand.Initialize(commandService);
            FolderWebResourceUpdateContentPublishCommand.Initialize(commandService);
            FolderWebResourceUpdateContentPublishEqualByTextCommand.Initialize(commandService);
            FolderWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(commandService);

            FolderWebResourceAddToSolutionLastCommand.Initialize(commandService);
            FolderWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            FolderJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            FolderJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);

            FolderCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);
            FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);

            FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);
            FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);

            FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);
            FolderCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);

            FolderCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            FolderCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);
            FolderCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);


            ProjectUpdatePluginAssemblyCommand.Initialize(commandService);
            ProjectUpdatePluginAssemblyInConnectionCommand.Initialize(commandService);

            ProjectRegisterPluginAssemblyInConnectionCommand.Initialize(commandService);

            ProjectCompareToCrmAssemblyCommand.Initialize(commandService);
            ProjectCompareToCrmAssemblyInConnectionCommand.Initialize(commandService);

            ProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            ProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            ProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            ProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            ProjectPluginAssemblyExplorerCommand.Initialize(commandService);
            ProjectPluginTypeExplorerCommand.Initialize(commandService);
            ProjectPluginTreeCommand.Initialize(commandService);



            ListForPublishClearListCommand.Initialize(commandService);
            ListForPublishCompareCommand.Initialize(commandService);
            ListForPublishCompareWithDetailsCommand.Initialize(commandService);
            ListForPublishCompareInConnectionGroupCommand.Initialize(commandService);
            ListForPublishFilesAddCommand.Initialize(commandService);
            ListForPublishFilesRemoveCommand.Initialize(commandService);
            ListForPublishPerformPublishEqualByTextCommand.Initialize(commandService);
            ListForPublishPerformPublishGroupConnectionCommand.Initialize(commandService);
            ListForPublishShowListCommand.Initialize(commandService);

            ListForPublishAddToSolutionLastCommand.Initialize(commandService);
            ListForPublishAddToSolutionInConnectionCommand.Initialize(commandService);

            CommonCurrentConnectionCommand.Initialize(commandService);
            CommonPublishAllInCrmConnectionCommand.Initialize(commandService);
            CommonCrmConnectionCommand.Initialize(commandService);
            CommonCrmConnectionTestCommand.Initialize(commandService);

            CommonFindEntitiesNamesAndShowDependentComponentsCommand.Initialize(commandService);
            CommonFindEntitiesNamesCommand.Initialize(commandService);
            CommonCheckEntitiesOwnerShipsCommand.Initialize(commandService);
            CommonCheckWorkflowsUsedEntitiesCommand.Initialize(commandService);
            CommonCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize(commandService);
            CommonCheckGlobalOptionSetDuplicateCommand.Initialize(commandService);
            CommonCheckManagedElementsCommand.Initialize(commandService);
            CommonFindMarkedToDeleteAndShowDependentComponentsCommand.Initialize(commandService);
            CommonCheckPluginImagesCommand.Initialize(commandService);
            CommonCheckPluginImagesRequiredComponentsCommand.Initialize(commandService);
            CommonCheckPluginStepsCommand.Initialize(commandService);
            CommonCheckPluginStepsRequiredComponentsCommand.Initialize(commandService);
            CommonEntityMetadataExplorerCommand.Initialize(commandService);
            CommonEntityAttributeExplorerCommand.Initialize(commandService);
            CommonEntityKeyExplorerCommand.Initialize(commandService);
            CommonEntityRelationshipOneToManyExplorerCommand.Initialize(commandService);
            CommonEntityRelationshipManyToManyExplorerCommand.Initialize(commandService);
            CommonEntityPrivilegesExplorerCommand.Initialize(commandService);
            CommonGlobalOptionSetsExplorerCommand.Initialize(commandService);
            CommonOrganizationExplorerCommand.Initialize(commandService);
            CommonPluginAssemblyExplorerCommand.Initialize(commandService);
            CommonPluginTypeExplorerCommand.Initialize(commandService);
            CommonCustomControlExplorerCommand.Initialize(commandService);
            CommonApplicationRibbonExplorerCommand.Initialize(commandService);
            CommonSiteMapExplorerCommand.Initialize(commandService);

            CommonSolutionExplorerCommand.Initialize(commandService);
            CommonSolutionExplorerInConnectionCommand.Initialize(commandService);
            CommonImportJobExplorerInConnectionCommand.Initialize(commandService);

            CommonOpenCrmWebSiteCommand.Initialize(commandService);
            CommonOpenConfigFolderCommand.Initialize(commandService);
            CommonConfigCommand.Initialize(commandService);
            CommonExportFormEventsCommand.Initialize(commandService);
            CommonReportExplorerCommand.Initialize(commandService);

            CommonOpenSolutionImageCommand.Initialize(commandService);
            CommonOpenSolutionDifferenceImageCommand.Initialize(commandService);
            CommonSystemFormExplorerCommand.Initialize(commandService);
            CommonSystemSavedQueryVisualizationExplorerCommand.Initialize(commandService);
            CommonSystemSavedQueryExplorerCommand.Initialize(commandService);
            CommonWebResourceExplorerCommand.Initialize(commandService);
            CommonWorkflowExplorerCommand.Initialize(commandService);

            CommonSystemUsersExplorerCommand.Initialize(commandService);
            CommonTeamsExplorerCommand.Initialize(commandService);
            CommonSecurityRolesExplorerCommand.Initialize(commandService);

            CommonExportDefaultSitemapsCommand.Initialize(commandService);
            CommonExportXsdSchemasCommand.Initialize(commandService);
            CommonOpenXsdSchemaFolderCommand.Initialize(commandService);

            CommonFindEntityObjectsByNameCommand.Initialize(commandService);
            CommonFindEntityObjectsContainsStringCommand.Initialize(commandService);
            CommonFindEntityObjectsByIdCommand.Initialize(commandService);
            CommonEditEntityObjectsByIdCommand.Initialize(commandService);
            CommonFindEntityObjectsByUniqueidentifierCommand.Initialize(commandService);
            CommonOrganizationComparerCommand.Initialize(commandService);
            CommonOpenOrganizationDifferenceImageCommand.Initialize(commandService);
            CommonTraceExportFileCommand.Initialize(commandService);
            CommonTraceReaderCommand.Initialize(commandService);
            CommonPluginConfigurationComparerPluginAssemblyCommand.Initialize(commandService);
            CommonPluginConfigurationCreateCommand.Initialize(commandService);
            CommonPluginConfigurationPluginAssemblyCommand.Initialize(commandService);
            CommonPluginConfigurationPluginTreeCommand.Initialize(commandService);
            CommonPluginConfigurationPluginTypeCommand.Initialize(commandService);
            CommonPluginTreeCommand.Initialize(commandService);
            CommonSdkMessageTreeCommand.Initialize(commandService);
            CommonSdkMessageRequestTreeCommand.Initialize(commandService);
            CommonExportOpenLastSelectedSolutionCommand.Initialize(commandService);
            CommonCheckComponentTypeEnumCommand.Initialize(commandService);
            CommonCheckCreateAllDependencyNodeDescriptionCommand.Initialize(commandService);
            CommonOpenDefaultSolutionInWebCommand.Initialize(commandService);




            FileWebResourceOpenFilesCommand.Initialize(commandService);
            FolderWebResourceOpenFilesCommand.Initialize(commandService);
            ListForPublishOpenFilesCommand.Initialize(commandService);

            DocumentsWebResourceMultiDifferenceCommand.Initialize(commandService);
            FileWebResourceMultiDifferenceCommand.Initialize(commandService);
            FolderWebResourceMultiDifferenceCommand.Initialize(commandService);
            ListForPublishMultiDifferenceCommand.Initialize(commandService);

            FileWebResourceAddFilesIntoListForPublishCommand.Initialize(commandService);
            DocumentsWebResourceAddFilesIntoListForPublishCommand.Initialize(commandService);
            FolderWebResourceAddFilesIntoListForPublishCommand.Initialize(commandService);

            CommonSelectCrmConnectionCommand.Initialize(commandService);


            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);

            Singleton = this;

            System.Threading.Tasks.Task.Run(() => CommonConfiguration.Get());
            System.Threading.Tasks.Task.Run(() => ConnectionConfiguration.Get());

            //Repository.ConnectionIntellisenseDataRepository.LoadIntellisenseCache();
        }

        internal async System.Threading.Tasks.Task ExecuteFetchXmlQueryAsync(string filePath, ConnectionData connectionData, IWriteToOutput iWriteToOutput, bool strictConnection)
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

            await this.JoinableTaskFactory.SwitchToMainThreadAsync();

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
