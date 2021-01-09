using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts;
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

        public EnvDTE80.DTE2 ApplicationObject { get; private set; }

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

            foreach (var actionInitialize in _initializeActions)
            {
                actionInitialize(commandService);
            }

            this.ApplicationObject = await this.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

            Singleton = this;

            var task1 = System.Threading.Tasks.Task.Run(() => CommonConfiguration.Get());
            var task2 = System.Threading.Tasks.Task.Run(() => ConnectionConfiguration.Get());
        }

        internal async System.Threading.Tasks.Task ExecuteFetchXmlQueryAsync(string filePath, ConnectionData connectionData, IWriteToOutput iWriteToOutput, bool strictConnection)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync();

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

        private readonly static List<Action<OleMenuCommandService>> _initializeActions = new List<Action<OleMenuCommandService>>()
        {
            CodePublishListAddCommand.Initialize,
            CodePublishListRemoveCommand.Initialize,

            WebResourceOpenFilesByTypeCommand.Initialize,

            WebResourceMultiDifferenceCommand.Initialize,

            WebResourceAddFilesIntoListForPublishCommand.Initialize,

            WebResourceRemoveFilesFromListForPublishCommand.Initialize,

            WebResourceUpdateContentPublishCommand.Initialize,
            WebResourceUpdateContentPublishInConnectionGroupWithCurrentCommand.Initialize,
            WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand.Initialize,

            WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand.Initialize,

            WebResourceGetCurrentContentCommand.Initialize,
            WebResourceGetCurrentContentInConnectionGroupCommand.Initialize,

            WebResourceCheckEncodingCommand.Initialize,
            WebResourceCheckEncodingCompareFilesCommand.Initialize,
            WebResourceCheckEncodingOpenFilesCommand.Initialize,

            WebResourceLinkCreateCommand.Initialize,
            WebResourceLinkClearCommand.Initialize,

            WebResourceShowDependentComponentsCommand.Initialize,

            WebResourceAddToSolutionLastCommand.Initialize,
            WebResourceAddToSolutionInConnectionCommand.Initialize,

            WebResourceCompareCommand.Initialize,
            WebResourceCompareInConnectionGroupCommand.Initialize,

            WebResourceShowDifferenceCommand.Initialize,
            WebResourceShowDifferenceInConnectionGroupCommand.Initialize,

            WebResourceShowDifferenceThreeFileCommand.Initialize,

            WebResourceExplorerCommand.Initialize,
            WebResourceOrganizationComparerCommand.Initialize,

            WebResourceChangeInEditorInConnectionCommand.Initialize,
            WebResourceCreateEntityDescriptionInConnectionCommand.Initialize,
            WebResourceGetAttributeInConnectionCommand.Initialize,

            WebResourceOpenInWebInConnectionCommand.Initialize,
            WebResourceOpenInExplorerInConnectionCommand.Initialize,




            JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand.Initialize,

            JavaScriptLinkedSystemFormAddToSolutionInConnectionCommand.Initialize,
            JavaScriptLinkedSystemFormAddToSolutionLastCommand.Initialize,

            JavaScriptUpdateEntityMetadataFileCommand.Initialize,
            JavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize,

            JavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize,
            JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize,

            JavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize,

            JavaScriptFileGenerationOptionsCommand.Initialize,

            JavaScriptIncludeReferencesToDependencyXmlCommand.Initialize,
            JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand.Initialize,

            JavaScriptShowDifferenceReferencesAndDependencyXmlCommand.Initialize,
            JavaScriptShowDifferenceReferencesAndDependencyXmlInConnectionGroupCommand.Initialize,

            JavaScriptIncludeReferencesToLinkedSystemFormCommand.Initialize,
            JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand.Initialize,

            JavaScriptUpdateContentIncludeReferencesPublishCommand.Initialize,
            JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand.Initialize,
            JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithCurrentCommand.Initialize,

            JavaScriptCopyToClipboardRibbonObjectsInConnectionCommand.Initialize,

            #region CodeJavaScript

            CodeJavaScriptLinkedSystemFormExplorerCommand.Initialize,
            CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand.Initialize,
            CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand.Initialize,

            CodeJavaScriptLinkedSystemFormAddToSolutionLastCommand.Initialize,
            CodeJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand.Initialize,

            CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommand.Initialize,

            CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand.Initialize,

            CodeJavaScriptLinkedEntityAddToSolutionLastCommand.Initialize,
            CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand.Initialize,

            CodeJavaScriptLinkedEntityExplorerCommand.Initialize,
            CodeJavaScriptLinkedEntityPublishInConnectionCommand.Initialize,
            CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand.Initialize,

            #endregion CodeJavaScript

            

            XmlCommonRemoveCustomAttributesCommand.Initialize,

            XmlCommonXsdSchemaOpenFolderCommand.Initialize,
            XmlCommonXsdSchemaSetCommand.Initialize,
            XmlCommonXsdSchemaRemoveCommand.Initialize,

            #region CodeXml

            #region FetchXml

            CodeXmlFetchXmlExecuteRequestCommand.Initialize,
            CodeXmlFetchXmlExecuteRequestInConnectionsCommand.Initialize,
            CodeXmlFetchXmlConvertToQueryExpressionCommand.Initialize,

            CodeXmlFetchXmlPasteFromClipboardCommand.Initialize,

            #endregion FetchXml

            #region SiteMap

            CodeXmlSiteMapGetCurrentCommand.Initialize,
            CodeXmlSiteMapGetCurrentInConnectionCommand.Initialize,

            CodeXmlSiteMapExplorerCommand.Initialize,
            CodeXmlSiteMapOrganizationComparerCommand.Initialize,

            CodeXmlSiteMapOpenInWebInConnectionCommand.Initialize,

            CodeXmlSiteMapShowDifferenceDefaultCommand.Initialize,
            CodeXmlSiteMapShowDifferenceCommand.Initialize,
            CodeXmlSiteMapShowDifferenceInConnectionGroupCommand.Initialize,

            CodeXmlSiteMapUpdateCommand.Initialize,
            CodeXmlSiteMapUpdateInConnectionGroupCommand.Initialize,

            #endregion SiteMap

            #region SystemForm

            CodeXmlSystemFormGetCurrentCommand.Initialize,
            CodeXmlSystemFormGetCurrentInConnectionCommand.Initialize,

            CodeXmlSystemFormExplorerCommand.Initialize,
            CodeXmlSystemFormOrganizationComparerCommand.Initialize,

            CodeXmlSystemFormOpenInWebInConnectionCommand.Initialize,

            CodeXmlSystemFormShowDifferenceCommand.Initialize,
            CodeXmlSystemFormShowDifferenceInConnectionGroupCommand.Initialize,

            CodeXmlSystemFormUpdateCommand.Initialize,
            CodeXmlSystemFormUpdateInConnectionGroupCommand.Initialize,

            #endregion SystemForm

            #region SavedQuery

            CodeXmlSavedQueryGetCurrentCommand.Initialize,
            CodeXmlSavedQueryGetCurrentInConnectionCommand.Initialize,

            CodeXmlSavedQueryExplorerCommand.Initialize,
            CodeXmlSavedQueryOrganizationComparerCommand.Initialize,

            CodeXmlSavedQueryOpenInWebInConnectionCommand.Initialize,

            CodeXmlSavedQueryShowDifferenceCommand.Initialize,
            CodeXmlSavedQueryShowDifferenceInConnectionGroupCommand.Initialize,

            CodeXmlSavedQueryUpdateCommand.Initialize,
            CodeXmlSavedQueryUpdateInConnectionGroupCommand.Initialize,

            #endregion SavedQuery

            #region Ribbon

            CodeXmlRibbonOpenInWebInConnectionCommand.Initialize,

            CodeXmlRibbonExplorerCommand.Initialize,
            CodeXmlRibbonOrganizationComparerCommand.Initialize,

            CodeXmlRibbonShowDifferenceCommand.Initialize,
            CodeXmlRibbonShowDifferenceInConnectionGroupCommand.Initialize,

            CodeXmlRibbonGetCurrentCommand.Initialize,
            CodeXmlRibbonGetCurrentInConnectionGroupCommand.Initialize,

            CodeXmlRibbonDiffInsertIntellisenseContextCommand.Initialize,
            CodeXmlRibbonDiffRemoveIntellisenseContextCommand.Initialize,

            CodeXmlRibbonDiffXmlShowDifferenceCommand.Initialize,
            CodeXmlRibbonDiffXmlShowDifferenceInConnectionGroupCommand.Initialize,

            CodeXmlRibbonDiffXmlUpdateCommand.Initialize,
            CodeXmlRibbonDiffXmlUpdateInConnectionGroupCommand.Initialize,

            CodeXmlRibbonDiffXmlGetCurrentCommand.Initialize,
            CodeXmlRibbonDiffXmlGetCurrentInConnectionGroupCommand.Initialize,

            CodeXmlRibbonAndRibbonDiffXmlGetCurrentCommand.Initialize,
            CodeXmlRibbonAndRibbonDiffXmlGetCurrentInConnectionGroupCommand.Initialize,

            #endregion Ribbon

            #region Workflow

            CodeXmlWorkflowGetCurrentCommand.Initialize,
            CodeXmlWorkflowGetCurrentInConnectionCommand.Initialize,

            CodeXmlWorkflowExplorerCommand.Initialize,
            CodeXmlWorkflowOrganizationComparerCommand.Initialize,

            CodeXmlWorkflowOpenInWebInConnectionCommand.Initialize,

            CodeXmlWorkflowShowDifferenceCommand.Initialize,
            CodeXmlWorkflowShowDifferenceInConnectionGroupCommand.Initialize,

            CodeXmlWorkflowUpdateCommand.Initialize,
            CodeXmlWorkflowUpdateInConnectionGroupCommand.Initialize,

            #endregion Workflow

            #region WebResource DependencyXml

            CodeXmlWebResourceDependencyXmlGetCurrentCommand.Initialize,
            CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommand.Initialize,

            CodeXmlWebResourceDependencyXmlExplorerCommand.Initialize,
            CodeXmlWebResourceDependencyXmlOrganizationComparerCommand.Initialize,

            CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommand.Initialize,

            CodeXmlWebResourceDependencyXmlShowDifferenceCommand.Initialize,
            CodeXmlWebResourceDependencyXmlShowDifferenceInConnectionCommand.Initialize,

            CodeXmlWebResourceDependencyXmlUpdateCommand.Initialize,
            CodeXmlWebResourceDependencyXmlUpdateInConnectionCommand.Initialize,

            #endregion WebResource DependencyXml

            #region PluginType CustomWorkflowActivityInfoExplorer

            CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommand.Initialize,

            CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentCommand.Initialize,
            CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentInConnectionCommand.Initialize,

            CodeXmlPluginTypeCustomWorkflowActivityInfoShowDifferenceCommand.Initialize,
            CodeXmlPluginTypeCustomWorkflowActivityInfoShowDifferenceInConnectionCommand.Initialize,

            #endregion PluginType CustomWorkflowActivityInfoExplorer

            CodeXmlCommonConvertToJavaScriptCodeCommand.Initialize,
            CodeXmlCommonCopyToClipboardWithoutSchemaCommand.Initialize,

            CodeXmlCommonUpdateCommand.Initialize,
            CodeXmlCommonShowDifferenceCommand.Initialize,

            CodeXmlCommonXsdSchemaSetProperCommand.Initialize,

            #endregion CodeXml

            

            ReportLinkClearCommand.Initialize,

            ReportAddToSolutionLastCommand.Initialize,
            ReportAddToSolutionInConnectionCommand.Initialize,

            ReportCreateCommand.Initialize,
            ReportUpdateCommand.Initialize,
            ReportLinkCreateCommand.Initialize,
            ReportActionOnComponentInConnectionCommand.Initialize,

            ReportExplorerCommand.Initialize,

            #region CodeReport

            CodeReportShowDifferenceCommand.Initialize,
            CodeReportShowDifferenceInConnectionGroupCommand.Initialize,
            CodeReportShowDifferenceThreeFileCommand.Initialize,

            #endregion CodeReport


            CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand.Initialize,
            CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand.Initialize,

            CSharpUpdateEntityMetadataFileProxyClassCommand.Initialize,
            CSharpUpdateEntityMetadataFileSchemaCommand.Initialize,

            CSharpUpdateGlobalOptionSetsFileCommand.Initialize,
            CSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize,

            CSharpEntityMetadataFileGenerationOptionsCommand.Initialize,

            CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand.Initialize,

            CSharpProjectPluginTypeActionOnComponentCommand.Initialize,
            CSharpProjectPluginTypeActionOnComponentInConnectionCommand.Initialize,
            CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand.Initialize,

            CSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize,
            CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize,

            CSharpAddPluginStepCommand.Initialize,
            CSharpAddPluginStepInConnectionCommand.Initialize,



            CSharpPluginStepsExplorerCommand.Initialize,

            CSharpPluginTreeCommand.Initialize,
            CSharpPluginTypeExplorerCommand.Initialize,
            CSharpPluginAssemblyExplorerCommand.Initialize,


            CSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize,
            CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize,

            CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize,
            CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize,


            CSharpProjectBuildLoadUpdatePluginAssemblyCommand.Initialize,
            CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand.Initialize,

            CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand.Initialize,
            CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand.Initialize,

            CSharpProjectUpdatePluginAssemblyCommand.Initialize,
            CSharpProjectUpdatePluginAssemblyInConnectionCommand.Initialize,

            CSharpProjectCompareTypesToCrmPluginAssemblyCommand.Initialize,
            CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand.Initialize,

            CSharpProjectPluginAssemblyActionOnComponentCommand.Initialize,
            CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand.Initialize,
            CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand.Initialize,

            #region Project

            ProjectRegisterPluginAssemblyInConnectionCommand.Initialize,

            ProjectPluginTypeExplorerCommand.Initialize,
            ProjectPluginTreeCommand.Initialize,
            ProjectPluginStepsExplorerCommand.Initialize,

            #endregion Project

            #region FolderAdd

            FolderAddPluginConfigurationFileCommand.Initialize,
            FolderAddSolutionFileCommand.Initialize,
            FolderAddEntityMetadataFileInConnectionCommand.Initialize,
            FolderAddGlobalOptionSetFileInConnectionCommand.Initialize,
            FolderAddSdkMessageRequestFileInConnectionCommand.Initialize,
            FolderAddSystemFormJavaScriptFileInConnectionCommand.Initialize,

            #endregion FolderAdd

            #region ListForPublish

            ListForPublishClearListCommand.Initialize,
            ListForPublishCompareCommand.Initialize,
            ListForPublishCompareWithDetailsCommand.Initialize,
            ListForPublishCompareInConnectionGroupCommand.Initialize,
            ListForPublishFilesAddCommand.Initialize,
            ListForPublishFilesRemoveCommand.Initialize,

            ListForPublishPerformPublishEqualByTextInConnectionGroupCommand.Initialize,
            ListForPublishPerformPublishInConnectionGroupCommand.Initialize,

            ListForPublishPerformUpdateContentIncludeReferencesPublishInConnectionGroupCommand.Initialize,
            ListForPublishPerformUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand.Initialize,

            ListForPublishPerformIncludeReferencesToDependencyXmlInConnectionGroupCommand.Initialize,
            ListForPublishPerformIncludeReferencesToLinkedSystemFormInConnectionGroupCommand.Initialize,

            ListForPublishJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand.Initialize,
            ListForPublishJavaScriptLinkedSystemFormAddToSolutionLastCommand.Initialize,

            ListForPublishShowListCommand.Initialize,

            ListForPublishAddToSolutionLastCommand.Initialize,
            ListForPublishAddToSolutionInConnectionCommand.Initialize,

            ListForPublishOpenFilesCommand.Initialize,

            ListForPublishMultiDifferenceCommand.Initialize,

            #endregion ListForPublish

            #region CrmConnection

            CommonCurrentConnectionCommand.Initialize,

            CommonCrmConnectionSelectAndPublishEntityCommand.Initialize,
            CommonCrmConnectionPublishAllCommand.Initialize,

            CommonCrmConnectionListCommand.Initialize,
            CommonCrmConnectionTestCommand.Initialize,
            CommonCrmConnectionEditCommand.Initialize,

            CommonCrmConnectionPoolShowStateCommand.Initialize,
            CommonCrmConnectionPoolClearCommand.Initialize,

            CommonCrmConnectionSelectFileCommand.Initialize,

            CommonCrmConnectionSelectCommand.Initialize,

            #endregion CrmConnection

            #region Common Check

            CommonCheckEntitiesOwnerShipsCommand.Initialize,
            CommonCheckWorkflowsUsedEntitiesCommand.Initialize,
            CommonCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize,
            CommonCheckGlobalOptionSetDuplicateCommand.Initialize,
            CommonCheckManagedElementsCommand.Initialize,
            CommonCheckPluginImagesCommand.Initialize,
            CommonCheckPluginImagesRequiredComponentsCommand.Initialize,
            CommonCheckPluginStepsCommand.Initialize,
            CommonCheckPluginStepsRequiredComponentsCommand.Initialize,

            CommonCheckComponentTypeEnumCommand.Initialize,
            CommonCheckCreateAllDependencyNodeDescriptionCommand.Initialize,

            CommonCheckUnknownFormControlTypesCommand.Initialize,
            CommonCheckSystemFormsWithNonExistentTeamTemplateCommand.Initialize,
            CommonCheckTeamTemplatesCommand.Initialize,

            #endregion Common Check

            #region Common Finds and Edits

            CommonFindEntityObjectsByPrefixCommand.Initialize,
            CommonFindEntityObjectsByPrefixInExplorerCommand.Initialize,

            CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommand.Initialize,

            CommonFindEntityObjectsMarkedToDeleteInExplorerCommand.Initialize,
            CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand.Initialize,

            CommonFindEntityObjectsByNameCommand.Initialize,
            CommonFindEntityObjectsByNameInExplorerCommand.Initialize,

            CommonFindEntityObjectsContainsStringCommand.Initialize,
            CommonFindEntityObjectsContainsStringInExplorerCommand.Initialize,

            CommonFindEntitiesByIdCommand.Initialize,
            CommonFindEntitiesByUniqueidentifierCommand.Initialize,

            CommonEditEntitiesByIdCommand.Initialize,

            #endregion Common Finds and Edits

            #region Common Entity

            CommonEntityMetadataExplorerCommand.Initialize,
            CommonEntityAttributeExplorerCommand.Initialize,
            CommonEntityKeyExplorerCommand.Initialize,
            CommonEntityRelationshipOneToManyExplorerCommand.Initialize,
            CommonEntityRelationshipManyToManyExplorerCommand.Initialize,
            CommonEntityPrivilegesExplorerCommand.Initialize,

            #endregion Common Entity

            #region Common Entity Objects

            CommonSystemFormExplorerCommand.Initialize,
            CommonSystemSavedQueryVisualizationExplorerCommand.Initialize,
            CommonSystemSavedQueryExplorerCommand.Initialize,

            #endregion Common Entity Objects

            #region Explorers

            CommonGlobalOptionSetsExplorerCommand.Initialize,
            CommonOrganizationExplorerCommand.Initialize,
            CommonCustomControlExplorerCommand.Initialize,
            CommonApplicationRibbonExplorerCommand.Initialize,
            CommonSiteMapExplorerCommand.Initialize,
            CommonReportExplorerCommand.Initialize,
            CommonWebResourceExplorerCommand.Initialize,
            CommonWorkflowExplorerCommand.Initialize,

            CommonExportFormEventsCommand.Initialize,

            #endregion Explorers

            #region Security

            CommonOtherPrivilegesExplorerCommand.Initialize,
            CommonSystemUsersExplorerCommand.Initialize,
            CommonTeamsExplorerCommand.Initialize,
            CommonSecurityRolesExplorerCommand.Initialize,

            #endregion Security

            #region Solutions

            CommonSolutionExplorerCommand.Initialize,
            CommonSolutionExplorerInConnectionCommand.Initialize,
            CommonImportJobExplorerInConnectionCommand.Initialize,

            CommonOpenSolutionImageCommand.Initialize,
            CommonOpenSolutionDifferenceImageCommand.Initialize,

            CommonOpenDefaultSolutionInWebCommand.Initialize,
            CommonExportOpenLastSelectedSolutionCommand.Initialize,

            #endregion Solutions

            #region PluginInfo

            CommonPluginAssemblyExplorerCommand.Initialize,
            CommonPluginTypeExplorerCommand.Initialize,

            CommonPluginTreeCommand.Initialize,

            CommonPluginStepsExplorerCommand.Initialize,

            CommonSdkMessageExplorerCommand.Initialize,
            CommonSdkMessageFilterExplorerCommand.Initialize,
            CommonSdkMessageFilterTreeCommand.Initialize,
            CommonSdkMessageRequestTreeCommand.Initialize,

            #endregion PluginInfo

            #region PluginConfiguration

            CommonPluginConfigurationComparerPluginAssemblyCommand.Initialize,
            CommonPluginConfigurationCreateCommand.Initialize,
            CommonPluginConfigurationPluginAssemblyCommand.Initialize,
            CommonPluginConfigurationPluginTreeCommand.Initialize,
            CommonPluginConfigurationPluginTypeCommand.Initialize,

            #endregion PluginConfiguration

            #region Trace

            CommonTraceExportFileCommand.Initialize,
            CommonTraceReaderCommand.Initialize,

            #endregion Trace

            #region OrganizationComparer

            CommonOrganizationComparerCommand.Initialize,

            CommonOpenOrganizationDifferenceImageCommand.Initialize,

            #endregion OrganizationComparer

            #region Config

            CommonFileGenerationOptionsCommand.Initialize,
            CommonFileGenerationConfigurationCommand.Initialize,

            CommonOpenConfigFolderCommand.Initialize,

            CommonConfigCommand.Initialize,

            #endregion Config

            CommonOpenCrmWebSiteCommand.Initialize,
            CommonOpenCrmWebSiteEntityMetadataCommand.Initialize,
            CommonOpenCrmWebSiteEntityListCommand.Initialize,
            CommonOpenCrmWebSiteEntityInstanceByIdCommand.Initialize,

            CommonFetchXmlOpenEntityFileInConnectionCommand.Initialize,

            CommonCrmConnectionOpenInfoFolderCommand.Initialize,
            CommonCrmConnectionOpenFetchXmlFolderCommand.Initialize,

            CommonExportDefaultSiteMapsCommand.Initialize,
            CommonXsdSchemaExportCommand.Initialize,

            CommonXsdSchemaOpenFolderCommand.Initialize,

            #region Output Windows

            OutputCrmConnectionListCommand.Initialize,
            OutputCommonConfigCommand.Initialize,

            OutputCrmConnectionSelectCommand.Initialize,
            OutputCrmConnectionTestCommand.Initialize,
            OutputCrmConnectionEditCommand.Initialize,

            OutputCrmConnectionPoolShowStateCommand.Initialize,
            OutputCrmConnectionPoolClearCommand.Initialize,

            OutputCrmConnectionSelectFileCommand.Initialize,

            OutputOpenCrmWebSiteCommand.Initialize,
            OutputOpenCrmWebSiteEntityMetadataCommand.Initialize,
            OutputOpenCrmWebSiteEntityListCommand.Initialize,

            OutputExportOpenLastSelectedSolutionCommand.Initialize,
            OutputOpenDefaultSolutionInWebCommand.Initialize,

            OutputOpenOrganizationDifferenceImageCommand.Initialize,
            OutputOpenSolutionDifferenceImageCommand.Initialize,
            OutputOpenSolutionImageCommand.Initialize,

            OutputOrganizationComparerCommand.Initialize,

            OutputExportFormEventsCommand.Initialize,

            #region Finds and Edits

            OutputFindEntityObjectsByPrefixCommand.Initialize,
            OutputFindEntityObjectsByPrefixInExplorerCommand.Initialize,

            OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommand.Initialize,

            OutputFindEntityObjectsMarkedToDeleteInExplorerCommand.Initialize,
            OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand.Initialize,

            OutputFindEntityObjectsByNameCommand.Initialize,
            OutputFindEntityObjectsByNameInExplorerCommand.Initialize,

            OutputFindEntityObjectsContainsStringCommand.Initialize,
            OutputFindEntityObjectsContainsStringInExplorerCommand.Initialize,

            OutputFindEntitiesByIdCommand.Initialize,
            OutputFindEntitiesByUniqueidentifierCommand.Initialize,

            OutputEditEntitiesByIdCommand.Initialize,

            #endregion Finds and Edits

            #region PluginConfiguration

            OutputPluginConfigurationComparerPluginAssemblyCommand.Initialize,
            OutputPluginConfigurationCreateCommand.Initialize,
            OutputPluginConfigurationPluginAssemblyCommand.Initialize,
            OutputPluginConfigurationPluginTreeCommand.Initialize,
            OutputPluginConfigurationPluginTypeCommand.Initialize,

            #endregion PluginConfiguration

            OutputApplicationRibbonExplorerCommand.Initialize,
            OutputCheckComponentTypeEnumCommand.Initialize,
            OutputCheckCreateAllDependencyNodeDescriptionCommand.Initialize,
            OutputCheckEntitiesOwnerShipsCommand.Initialize,
            OutputCheckGlobalOptionSetDuplicateCommand.Initialize,
            OutputCheckManagedElementsCommand.Initialize,
            OutputCheckPluginImagesCommand.Initialize,
            OutputCheckPluginImagesRequiredComponentsCommand.Initialize,
            OutputCheckPluginStepsCommand.Initialize,
            OutputCheckPluginStepsRequiredComponentsCommand.Initialize,
            OutputCheckWorkflowsUsedEntitiesCommand.Initialize,
            OutputCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize,

            OutputCheckUnknownFormControlTypesCommand.Initialize,
            OutputCheckSystemFormsWithNonExistentTeamTemplateCommand.Initialize,
            OutputCheckTeamTemplatesCommand.Initialize,

            OutputCustomControlExplorerCommand.Initialize,
            OutputEntityAttributeExplorerCommand.Initialize,
            OutputEntityKeyExplorerCommand.Initialize,
            OutputEntityMetadataExplorerCommand.Initialize,
            OutputEntityPrivilegesExplorerCommand.Initialize,
            OutputEntityRelationshipManyToManyExplorerCommand.Initialize,
            OutputEntityRelationshipOneToManyExplorerCommand.Initialize,
            OutputOtherPrivilegesExplorerCommand.Initialize,

            OutputGlobalOptionSetsExplorerCommand.Initialize,
            OutputImportJobExplorerInConnectionCommand.Initialize,
            OutputOrganizationExplorerCommand.Initialize,
            OutputPluginAssemblyExplorerCommand.Initialize,

            OutputPluginTypeExplorerCommand.Initialize,
            OutputReportExplorerCommand.Initialize,

            OutputPluginStepsExplorerCommand.Initialize,
            OutputSdkMessageExplorerCommand.Initialize,
            OutputSdkMessageFilterExplorerCommand.Initialize,

            OutputPluginTreeCommand.Initialize,
            OutputSdkMessageFilterTreeCommand.Initialize,
            OutputSdkMessageRequestTreeCommand.Initialize,

            OutputSecurityRolesExplorerCommand.Initialize,
            OutputSiteMapExplorerCommand.Initialize,
            OutputSolutionExplorerCommand.Initialize,
            OutputSystemFormExplorerCommand.Initialize,
            OutputSystemSavedQueryExplorerCommand.Initialize,
            OutputSystemSavedQueryVisualizationExplorerCommand.Initialize,
            OutputSystemUsersExplorerCommand.Initialize,
            OutputTeamsExplorerCommand.Initialize,
            OutputTraceExportFileCommand.Initialize,
            OutputTraceReaderCommand.Initialize,
            OutputWebResourceExplorerCommand.Initialize,
            OutputWorkflowExplorerCommand.Initialize,

            OutputCrmConnectionSelectAndPublishEntityCommand.Initialize,
            OutputCrmConnectionPublishAllCommand.Initialize,

            OutputOpenConfigFolderCommand.Initialize,
            OutputFetchXmlOpenEntityFileCommand.Initialize,

            OutputCrmConnectionOpenInfoFolderCommand.Initialize,
            OutputCrmConnectionOpenFetchXmlFolderCommand.Initialize,


            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,
            //Output.Initialize,

            #endregion Output Windows

            //Folder.Initialize,
            //Folder.Initialize,
            //Folder.Initialize,
            //Folder.Initialize,
            //Folder.Initialize,
        };

        #endregion
    }
}