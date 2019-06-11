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

        private EnvDTE80.DTE2 _applicationObject;

        public  EnvDTE80.DTE2 ApplicationObject
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

        protected override void Initialize()
        {
            base.Initialize();

            OleMenuCommandService commandService = this.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

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

            CodeWebResourceAddToSolutionLastCommand.Initialize(commandService);
            CodeWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(this);

            CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);
            CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(this);

            CodeJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(commandService);

            CodeXmlExecuteFetchXmlRequestCommand.Initialize(this);
            CodeXmlExecuteFetchXmlRequestInConnectionsCommand.Initialize(commandService);
            CodeXmlConvertFetchXmlToJavaScriptCodeCommand.Initialize(this);
            CodeXmlRibbonDiffInsertIntellisenseContextCommand.Initialize(this);
            CodeXmlRibbonDiffRemoveIntellisenseContextCommand.Initialize(this);

            CodeXmlSiteMapOpenInWebCommand.Initialize(commandService);
            CodeXmlSiteMapExplorerCommand.Initialize(this);
            CodeXmlShowDifferenceSiteMapDefaultCommand.Initialize(this);
            CodeXmlShowDifferenceSiteMapCommand.Initialize(this);
            CodeXmlShowDifferenceSiteMapInConnectionGroupCommand.Initialize(this);
            CodeXmlUpdateSiteMapCommand.Initialize(this);
            CodeXmlUpdateSiteMapInConnectionGroupCommand.Initialize(this);

            CodeXmlSystemFormExplorerCommand.Initialize(this);
            CodeXmlSystemFormOpenInWebCommand.Initialize(commandService);
            CodeXmlShowDifferenceSystemFormCommand.Initialize(this);
            CodeXmlShowDifferenceSystemFormInConnectionGroupCommand.Initialize(this);
            CodeXmlUpdateSystemFormCommand.Initialize(this);
            CodeXmlUpdateSystemFormInConnectionGroupCommand.Initialize(this);

            CodeXmlSavedQueryExplorerCommand.Initialize(this);
            CodeXmlSavedQueryOpenInWebCommand.Initialize(commandService);
            CodeXmlShowDifferenceSavedQueryCommand.Initialize(this);
            CodeXmlShowDifferenceSavedQueryInConnectionGroupCommand.Initialize(this);
            CodeXmlUpdateSavedQueryCommand.Initialize(this);
            CodeXmlUpdateSavedQueryInConnectionGroupCommand.Initialize(this);

            CodeXmlEntityRibbonOpenInWebCommand.Initialize(commandService);
            CodeXmlRibbonExplorerCommand.Initialize(this);
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
            CodeReportUpdateCommand.Initialize(commandService);
            CodeReportCreateCommand.Initialize(commandService);
            CodeReportShowDifferenceCommand.Initialize(this);
            CodeReportShowDifferenceInConnectionGroupCommand.Initialize(this);
            CodeReportShowDifferenceThreeFileCommand.Initialize(this);
            CodeReportExplorerCommand.Initialize(this);
            CodeReportOpenInWebCommand.Initialize(this);

            CodeReportAddToSolutionLastCommand.Initialize(commandService);
            CodeReportAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(this);

            CodeCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);
            CodeCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(this);

            CodeCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);
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

            CodeCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            CodeCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            CodeCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);
            CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);





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

            FileWebResourceAddToSolutionLastCommand.Initialize(commandService);
            FileWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            FileJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            FileJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(this);

            FileJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);
            FileJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(this);

            FileJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(commandService);

            FileReportExplorerCommand.Initialize(this);
            FileReportLinkClearCommand.Initialize(this);
            FileReportLinkCreateCommand.Initialize(this);
            FileReportUpdateCommand.Initialize(commandService);
            FileReportCreateCommand.Initialize(commandService);
            FileReportOpenInWebCommand.Initialize(this);

            FileReportAddToSolutionLastCommand.Initialize(commandService);
            FileReportAddToSolutionInConnectionCommand.Initialize(commandService);

            FileCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand.Initialize(this);

            FileCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);
            FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand.Initialize(this);

            FileCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);
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

            FileCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            FileCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);
            FileCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);



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

            DocumentsWebResourceAddToSolutionLastCommand.Initialize(commandService);
            DocumentsWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            DocumentsJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);

            DocumentsReportLinkClearCommand.Initialize(this);

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








            FolderAddPluginConfigurationFileCommand.Initialize(this);
            FolderAddSolutionFileCommand.Initialize(this);
            FolderAddEntityMetadataFileInConnectionCommand.Initialize(commandService);
            FolderAddGlobalOptionSetFileInConnectionCommand.Initialize(commandService);
            FolderAddSdkMessageRequestFileInConnectionCommand.Initialize(commandService);
            FolderAddSystemFormJavaScriptFileInConnectionCommand.Initialize(commandService);



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


            ProjectUpdatePluginAssemblyCommand.Initialize(this);
            ProjectUpdatePluginAssemblyInConnectionCommand.Initialize(this);

            ProjectRegisterPluginAssemblyInConnectionCommand.Initialize(commandService);

            ProjectCompareToCrmAssemblyCommand.Initialize(this);
            ProjectCompareToCrmAssemblyInConnectionCommand.Initialize(commandService);

            ProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            ProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            ProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            ProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

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

            ListForPublishAddToSolutionLastCommand.Initialize(commandService);
            ListForPublishAddToSolutionInConnectionCommand.Initialize(commandService);

            CommonCurrentConnectionCommand.Initialize(this);
            CommonPublishAllInCrmConnectionCommand.Initialize(commandService);
            CommonCrmConnectionCommand.Initialize(this);
            CommonCrmConnectionTestCommand.Initialize(commandService);

            CommonCheckEntitiesNamesAndShowDependentComponentsCommand.Initialize(this);
            CommonCheckEntitiesNamesCommand.Initialize(this);
            CommonCheckEntitiesOwnerShipsCommand.Initialize(commandService);
            CommonCheckWorkflowsUsedEntitiesCommand.Initialize(this);
            CommonCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize(this);
            CommonCheckGlobalOptionSetDuplicateCommand.Initialize(commandService);
            CommonCheckManagedElementsCommand.Initialize(commandService);
            CommonCheckMarkedToDeleteAndShowDependentComponentsCommand.Initialize(this);
            CommonCheckPluginImagesCommand.Initialize(commandService);
            CommonCheckPluginImagesRequiredComponentsCommand.Initialize(commandService);
            CommonCheckPluginStepsCommand.Initialize(commandService);
            CommonCheckPluginStepsRequiredComponentsCommand.Initialize(commandService);
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
            CommonImportJobExplorerInConnectionCommand.Initialize(commandService);

            CommonOpenCrmWebSiteCommand.Initialize(commandService);
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
            CommonTraceReaderCommand.Initialize(commandService);
            CommonPluginConfigurationComparerPluginAssemblyCommand.Initialize(this);
            CommonPluginConfigurationCreateCommand.Initialize(this);
            CommonPluginConfigurationPluginAssemblyCommand.Initialize(this);
            CommonPluginConfigurationPluginTreeCommand.Initialize(this);
            CommonPluginConfigurationPluginTypeCommand.Initialize(this);
            CommonPluginTreeCommand.Initialize(this);
            CommonSdkMessageTreeCommand.Initialize(this);
            CommonSdkMessageRequestTreeCommand.Initialize(this);
            CommonExportOpenLastSelectedSolutionCommand.Initialize(this);
            CommonCheckComponentTypeEnumCommand.Initialize(commandService);
            CommonCheckCreateAllDependencyNodeDescriptionCommand.Initialize(commandService);
            CommonOpenDefaultSolutionInWebCommand.Initialize(commandService);




            OpenFilesCommand.Initialize(this);
            MultiDifferenceCommand.Initialize(this);
            AddFilesIntoListForPublishCommand.Initialize(this);

            CommonSelectCrmConnectionCommand.Initialize(commandService);


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
