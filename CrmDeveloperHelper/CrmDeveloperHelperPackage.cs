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
            CodeWebResourceExplorerCommand.Initialize(this);
            CodeWebResourceOpenInWebCommand.Initialize(this);
            CodeWebResourceLinkClearCommand.Initialize(this);
            CodeWebResourceLinkCreateCommand.Initialize(this);
            CodeWebResourceUpdateContentPublishCommand.Initialize(this);
            CodeWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(this);

            CodeWebResourceAddToSolutionLastCommand.Initialize(this);
            CodeWebResourceAddToSolutionInConnectionCommand.Initialize(this);

            CodeJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);
            CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(this);

            CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(this);
            CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(this);

            CodeJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(this);

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
            CodeReportCreateCommand.Initialize(this);
            CodeReportShowDifferenceCommand.Initialize(this);
            CodeReportShowDifferenceInConnectionGroupCommand.Initialize(this);
            CodeReportShowDifferenceThreeFileCommand.Initialize(this);
            CodeReportExplorerCommand.Initialize(this);
            CodeReportOpenInWebCommand.Initialize(this);

            CodeReportAddToSolutionLastCommand.Initialize(this);
            CodeReportAddToSolutionInConnectionCommand.Initialize(this);

            CodeCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(this);

            CodeCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            CodeCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(this);

            CodeCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(this);

            CodeCSharpProjectUpdatePluginAssemblyCommand.Initialize(this);
            CodeCSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);

            CodeCSharpProjectCompareToCrmAssemblyCommand.Initialize(this);
            CodeCSharpProjectCompareToCrmAssemblyInConnectionCommand.Initialize(this);

            CodeCSharpPluginTypeExplorerCommand.Initialize(this);
            CodeCSharpPluginAssemblyExplorerCommand.Initialize(this);
            CodeCSharpPluginTreeCommand.Initialize(this);

            CodeCSharpAddPluginStepCommand.Initialize(this);
            CodeCSharpAddPluginStepInConnectionCommand.Initialize(this);

            CodeCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(this);

            CodeCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(this);

            CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(this);
            CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(this);





            FileWebResourceCheckEncodingCommand.Initialize(this);
            FileWebResourceCheckEncodingCompareFilesCommand.Initialize(this);
            FileWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(this);
            FileWebResourceCheckEncodingOpenFilesCommand.Initialize(this);
            FileWebResourceCompareCommand.Initialize(this);
            FileWebResourceCompareWithDetailsCommand.Initialize(this);
            FileWebResourceCompareInConnectionGroupCommand.Initialize(this);
            FileWebResourceExplorerCommand.Initialize(this);
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

            FileWebResourceAddToSolutionLastCommand.Initialize(this);
            FileWebResourceAddToSolutionInConnectionCommand.Initialize(this);

            FileJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);
            FileJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(this);

            FileJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(this);
            FileJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(this);

            FileJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(this);

            FileReportExplorerCommand.Initialize(this);
            FileReportLinkClearCommand.Initialize(this);
            FileReportLinkCreateCommand.Initialize(this);
            FileReportUpdateCommand.Initialize(this);
            FileReportCreateCommand.Initialize(this);
            FileReportOpenInWebCommand.Initialize(this);

            FileReportAddToSolutionLastCommand.Initialize(this);
            FileReportAddToSolutionInConnectionCommand.Initialize(this);

            FileCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(this);

            FileCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(this);

            FileCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);
            FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(this);
            FileCSharpPluginAssemblyExplorerCommand.Initialize(this);
            FileCSharpProjectUpdatePluginAssemblyCommand.Initialize(this);
            FileCSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);
            FileCSharpProjectCompareToCrmAssemblyCommand.Initialize(this);
            FileCSharpProjectCompareToCrmAssemblyInConnectionCommand.Initialize(this);
            FileCSharpPluginTypeExplorerCommand.Initialize(this);
            FileCSharpPluginTreeCommand.Initialize(this);

            FileCSharpAddPluginStepCommand.Initialize(this);
            FileCSharpAddPluginStepInConnectionCommand.Initialize(this);

            FileCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(this);

            FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(this);

            FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(this);
            FileCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(this);



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

            DocumentsWebResourceAddToSolutionLastCommand.Initialize(this);
            DocumentsWebResourceAddToSolutionInConnectionCommand.Initialize(this);

            DocumentsJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);
            DocumentsJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(this);

            DocumentsReportLinkClearCommand.Initialize(this);

            DocumentsReportAddToSolutionLastCommand.Initialize(this);
            DocumentsReportAddToSolutionInConnectionCommand.Initialize(this);

            DocumentsCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            DocumentsCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            DocumentsCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);

            DocumentsCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(this);

            DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(this);

            DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(this);
            DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(this);








            FolderAddPluginConfigurationFileCommand.Initialize(this);
            FolderAddSolutionFileCommand.Initialize(this);
            FolderAddEntityMetadataFileInConnectionCommand.Initialize(this);
            FolderAddGlobalOptionSetFileInConnectionCommand.Initialize(this);
            FolderAddSdkMessageRequestFileInConnectionCommand.Initialize(this);
            FolderAddSystemFormJavaScriptFileInConnectionCommand.Initialize(this);



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

            FolderWebResourceAddToSolutionLastCommand.Initialize(this);
            FolderWebResourceAddToSolutionInConnectionCommand.Initialize(this);

            FolderJavaScriptUpdateEntityMetadataFileCommand.Initialize(this);
            FolderJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(this);

            FolderCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(this);
            FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(this);

            FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(this);
            FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(this);

            FolderCSharpProjectPluginTypeStepsAddIToSolutionInConnectionCommand.Initialize(this);
            FolderCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(this);

            FolderCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(this);
            FolderCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(this);
            FolderCSharpUpdateGlobalOptionSetsFileCommand.Initialize(this);


            ProjectUpdatePluginAssemblyCommand.Initialize(this);
            ProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);

            ProjectRegisterPluginAssemblyInConnectionCommand.Initialize(this);

            ProjectCompareToCrmAssemblyCommand.Initialize(this);
            ProjectCompareToCrmAssemblyInConnectionCommand.Initialize(this);

            ProjectPluginAssemblyAddToSolutionLastCommand.Initialize(this);
            ProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(this);

            ProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(this);
            ProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(this);

            ProjectPluginAssemblyExplorerCommand.Initialize(this);
            ProjectPluginTypeExplorerCommand.Initialize(this);
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

            ListForPublishAddToSolutionLastCommand.Initialize(this);
            ListForPublishAddToSolutionInConnectionCommand.Initialize(this);

            CommonCurrentConnectionCommand.Initialize(this);
            CommonPublishAllInCrmConnectionCommand.Initialize(this);
            CommonCrmConnectionCommand.Initialize(this);
            CommonCrmConnectionTestCommand.Initialize(this);

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
            CommonEntityMetadataExplorerCommand.Initialize(this);
            CommonEntityAttributeExplorerCommand.Initialize(this);
            CommonEntityKeyExplorerCommand.Initialize(this);
            CommonEntityRelationshipOneToManyExplorerCommand.Initialize(this);
            CommonEntityRelationshipManyToManyExplorerCommand.Initialize(this);
            CommonEntityPrivilegesExplorerCommand.Initialize(this);
            CommonGlobalOptionSetsExplorerCommand.Initialize(this);
            CommonOrganizationExplorerCommand.Initialize(this);
            CommonPluginAssemblyExplorerCommand.Initialize(this);
            CommonPluginTypeExplorerCommand.Initialize(this);
            CommonCustomControlExplorerCommand.Initialize(this);
            CommonApplicationRibbonExplorerCommand.Initialize(this);
            CommonSiteMapExplorerCommand.Initialize(this);

            CommonSolutionExplorerCommand.Initialize(this);
            CommonSolutionExplorerInConnectionCommand.Initialize(this);
            CommonImportJobExplorerInConnectionCommand.Initialize(this);

            CommonOpenCrmWebSiteCommand.Initialize(this);
            CommonOpenConfigFolderCommand.Initialize(this);
            CommonConfigCommand.Initialize(this);
            CommonExportFormEventsCommand.Initialize(this);
            CommonReportExplorerCommand.Initialize(this);

            CommonOpenSolutionImageCommand.Initialize(this);
            CommonOpenSolutionDifferenceImageCommand.Initialize(this);
            CommonSystemFormExplorerCommand.Initialize(this);
            CommonSystemSavedQueryVisualizationExplorerCommand.Initialize(this);
            CommonSystemSavedQueryExplorerCommand.Initialize(this);
            CommonWebResourceExplorerCommand.Initialize(this);
            CommonWorkflowExplorerCommand.Initialize(this);

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
            CommonCheckCreateAllDependencyNodeDescriptionCommand.Initialize(this);
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
