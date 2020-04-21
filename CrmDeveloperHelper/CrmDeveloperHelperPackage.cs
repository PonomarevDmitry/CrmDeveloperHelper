using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts;
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

            CodePublishListAddCommand.Initialize(commandService);
            CodePublishListRemoveCommand.Initialize(commandService);

            #region CodeWebResource

            CodeWebResourceCheckEncodingCommand.Initialize(commandService);
            CodeWebResourceCompareWithDetailsCommand.Initialize(commandService);
            CodeWebResourceCompareWithDetailsInConnectionGroupCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceCustomCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceInConnectionGroupCommand.Initialize(commandService);
            CodeWebResourceShowDifferenceThreeFileCommand.Initialize(commandService);
            CodeWebResourceCreateEntityDescriptionInConnectionCommand.Initialize(commandService);
            CodeWebResourceChangeInEditorInConnectionCommand.Initialize(commandService);
            CodeWebResourceGetAttributeInConnectionCommand.Initialize(commandService);
            CodeWebResourceExplorerCommand.Initialize(commandService);
            CodeWebResourceOpenInWebInConnectionCommand.Initialize(commandService);
            CodeWebResourceLinkClearCommand.Initialize(commandService);
            CodeWebResourceLinkCreateCommand.Initialize(commandService);
            CodeWebResourceUpdateContentPublishCommand.Initialize(commandService);
            CodeWebResourceUpdateContentPublishInConnectionGroupCommand.Initialize(commandService);

            CodeWebResourceAddToSolutionLastCommand.Initialize(commandService);
            CodeWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            #endregion CodeWebResource

            #region DocumentsWebResource

            DocumentsWebResourceCheckEncodingCommand.Initialize(commandService);
            DocumentsWebResourceCheckEncodingCompareFilesCommand.Initialize(commandService);
            DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(commandService);
            DocumentsWebResourceCompareCommand.Initialize(commandService);
            DocumentsWebResourceCompareWithDetailsCommand.Initialize(commandService);
            DocumentsWebResourceCompareInConnectionGroupCommand.Initialize(commandService);
            DocumentsWebResourceLinkClearCommand.Initialize(commandService);
            DocumentsWebResourceLinkCreateCommand.Initialize(commandService);
            DocumentsWebResourceShowDependentComponentsCommand.Initialize(commandService);
            DocumentsWebResourceUpdateContentPublishEqualByTextCommand.Initialize(commandService);
            DocumentsWebResourceUpdateContentPublishGroupConnectionCommand.Initialize(commandService);

            DocumentsWebResourceAddToSolutionLastCommand.Initialize(commandService);
            DocumentsWebResourceAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsWebResourceMultiDifferenceCommand.Initialize(commandService);

            DocumentsWebResourceAddFilesIntoListForPublishCommand.Initialize(commandService);

            #endregion DocumentsWebResource

            #region FileWebResource

            FileWebResourceCheckEncodingCommand.Initialize(commandService);
            FileWebResourceCheckEncodingCompareFilesCommand.Initialize(commandService);
            FileWebResourceCheckEncodingCompareWithDetailsFilesCommand.Initialize(commandService);
            FileWebResourceCheckEncodingOpenFilesCommand.Initialize(commandService);
            FileWebResourceCompareCommand.Initialize(commandService);
            FileWebResourceCompareWithDetailsCommand.Initialize(commandService);
            FileWebResourceCompareInConnectionGroupCommand.Initialize(commandService);
            FileWebResourceCreateEntityDescriptionInConnectionCommand.Initialize(commandService);
            FileWebResourceChangeInEditorInConnectionCommand.Initialize(commandService);
            FileWebResourceGetAttributeInConnectionCommand.Initialize(commandService);
            FileWebResourceExplorerCommand.Initialize(commandService);
            FileWebResourceOpenInWebInConnectionCommand.Initialize(commandService);
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

            FileWebResourceOpenFilesCommand.Initialize(commandService);

            FileWebResourceMultiDifferenceCommand.Initialize(commandService);

            FileWebResourceAddFilesIntoListForPublishCommand.Initialize(commandService);

            #endregion FileWebResource

            #region FolderWebResource

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

            FolderWebResourceOpenFilesCommand.Initialize(commandService);

            FolderWebResourceMultiDifferenceCommand.Initialize(commandService);

            FolderWebResourceAddFilesIntoListForPublishCommand.Initialize(commandService);

            #endregion FolderWebResource



            #region CodeJavaScript

            CodeJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(commandService);

            CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);
            CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(commandService);

            CodeJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(commandService);

            CodeJavaScriptLinkedSystemFormExplorerCommand.Initialize(commandService);
            CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand.Initialize(commandService);
            CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand.Initialize(commandService);

            CodeJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand.Initialize(commandService);
            CodeJavaScriptLinkedSystemFormAddToSolutionLastCommand.Initialize(commandService);
            CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommand.Initialize(commandService);

            // CodeJavaScriptLinkedSystemForm.Initialize(commandService);

            #endregion CodeJavaScript

            #region DocumentsJavaScript

            DocumentsJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            DocumentsJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);

            #endregion DocumentsJavaScript

            #region FileJavaScript

            FileJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            FileJavaScriptUpdateEntityMetadataFileWithSelectCommand.Initialize(commandService);

            FileJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);
            FileJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand.Initialize(commandService);

            FileJavaScriptUpdateGlobalOptionSetAllFileCommand.Initialize(commandService);

            #endregion FileJavaScript

            #region FolderJavaScript

            FolderJavaScriptUpdateEntityMetadataFileCommand.Initialize(commandService);
            FolderJavaScriptUpdateGlobalOptionSetSingleFileCommand.Initialize(commandService);

            #endregion FolderJavaScript



            #region CodeXml

            #region FetchXml

            CodeXmlFetchXmlExecuteRequestCommand.Initialize(commandService);
            CodeXmlFetchXmlExecuteRequestInConnectionsCommand.Initialize(commandService);
            CodeXmlFetchXmlConvertToQueryExpressionCommand.Initialize(commandService);

            CodeXmlFetchXmlPasteFromClipboardCommand.Initialize(commandService);

            #endregion FetchXml

            #region SiteMap

            CodeXmlSiteMapGetCurrentCommand.Initialize(commandService);
            CodeXmlSiteMapGetCurrentInConnectionCommand.Initialize(commandService);

            CodeXmlSiteMapOpenInWebInConnectionCommand.Initialize(commandService);
            CodeXmlSiteMapExplorerCommand.Initialize(commandService);

            CodeXmlSiteMapShowDifferenceDefaultCommand.Initialize(commandService);
            CodeXmlSiteMapShowDifferenceCommand.Initialize(commandService);
            CodeXmlSiteMapShowDifferenceInConnectionGroupCommand.Initialize(commandService);

            CodeXmlSiteMapUpdateCommand.Initialize(commandService);
            CodeXmlSiteMapUpdateInConnectionGroupCommand.Initialize(commandService);

            #endregion SiteMap

            #region SystemForm

            CodeXmlSystemFormGetCurrentCommand.Initialize(commandService);
            CodeXmlSystemFormGetCurrentInConnectionCommand.Initialize(commandService);

            CodeXmlSystemFormExplorerCommand.Initialize(commandService);
            CodeXmlSystemFormOpenInWebInConnectionCommand.Initialize(commandService);

            CodeXmlSystemFormShowDifferenceCommand.Initialize(commandService);
            CodeXmlSystemFormShowDifferenceInConnectionGroupCommand.Initialize(commandService);

            CodeXmlSystemFormUpdateCommand.Initialize(commandService);
            CodeXmlSystemFormUpdateInConnectionGroupCommand.Initialize(commandService);

            #endregion SystemForm

            #region SavedQuery

            CodeXmlSavedQueryGetCurrentCommand.Initialize(commandService);
            CodeXmlSavedQueryGetCurrentInConnectionCommand.Initialize(commandService);

            CodeXmlSavedQueryExplorerCommand.Initialize(commandService);
            CodeXmlSavedQueryOpenInWebInConnectionCommand.Initialize(commandService);

            CodeXmlSavedQueryShowDifferenceCommand.Initialize(commandService);
            CodeXmlSavedQueryShowDifferenceInConnectionGroupCommand.Initialize(commandService);

            CodeXmlSavedQueryUpdateCommand.Initialize(commandService);
            CodeXmlSavedQueryUpdateInConnectionGroupCommand.Initialize(commandService);

            #endregion SavedQuery

            #region Ribbon

            CodeXmlRibbonOpenInWebInConnectionCommand.Initialize(commandService);

            CodeXmlRibbonExplorerCommand.Initialize(commandService);

            CodeXmlRibbonShowDifferenceCommand.Initialize(commandService);
            CodeXmlRibbonShowDifferenceInConnectionGroupCommand.Initialize(commandService);

            CodeXmlRibbonGetCurrentCommand.Initialize(commandService);
            CodeXmlRibbonGetCurrentInConnectionCommand.Initialize(commandService);

            CodeXmlRibbonDiffInsertIntellisenseContextCommand.Initialize(commandService);
            CodeXmlRibbonDiffRemoveIntellisenseContextCommand.Initialize(commandService);

            CodeXmlRibbonDiffXmlShowDifferenceCommand.Initialize(commandService);
            CodeXmlRibbonDiffXmlShowDifferenceInConnectionGroupCommand.Initialize(commandService);

            CodeXmlRibbonDiffXmlUpdateCommand.Initialize(commandService);
            CodeXmlRibbonDiffXmlUpdateInConnectionGroupCommand.Initialize(commandService);

            CodeXmlRibbonDiffXmlGetCurrentCommand.Initialize(commandService);
            CodeXmlRibbonDiffXmlGetCurrentInConnectionCommand.Initialize(commandService);

            #endregion Ribbon

            #region Workflow

            CodeXmlWorkflowGetCurrentCommand.Initialize(commandService);
            CodeXmlWorkflowGetCurrentInConnectionCommand.Initialize(commandService);

            CodeXmlWorkflowExplorerCommand.Initialize(commandService);
            CodeXmlWorkflowOpenInWebInConnectionCommand.Initialize(commandService);

            CodeXmlWorkflowShowDifferenceCommand.Initialize(commandService);
            CodeXmlWorkflowShowDifferenceInConnectionGroupCommand.Initialize(commandService);

            CodeXmlWorkflowUpdateCommand.Initialize(commandService);
            CodeXmlWorkflowUpdateInConnectionGroupCommand.Initialize(commandService);

            #endregion Workflow

            #region WebResource DependencyXml

            CodeXmlWebResourceDependencyXmlGetCurrentCommand.Initialize(commandService);
            CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommand.Initialize(commandService);

            CodeXmlWebResourceDependencyXmlExplorerCommand.Initialize(commandService);
            CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommand.Initialize(commandService);

            CodeXmlWebResourceDependencyXmlShowDifferenceCommand.Initialize(commandService);
            CodeXmlWebResourceDependencyXmlShowDifferenceInConnectionCommand.Initialize(commandService);

            CodeXmlWebResourceDependencyXmlUpdateCommand.Initialize(commandService);
            CodeXmlWebResourceDependencyXmlUpdateInConnectionCommand.Initialize(commandService);

            #endregion WebResource DependencyXml

            CodeXmlCommonConvertToJavaScriptCodeCommand.Initialize(commandService);
            CodeXmlCommonCopyToClipboardWithoutSchemaCommand.Initialize(commandService);

            CodeXmlCommonUpdateCommand.Initialize(commandService);
            CodeXmlCommonShowDifferenceCommand.Initialize(commandService);

            CodeXmlCommonRemoveCustomAttributesCommand.Initialize(commandService);

            CodeXmlCommonXsdSchemaOpenFolderCommand.Initialize(commandService);
            CodeXmlCommonXsdSchemaSetCommand.Initialize(commandService);
            CodeXmlCommonXsdSchemaSetProperCommand.Initialize(commandService);
            CodeXmlCommonXsdSchemaRemoveCommand.Initialize(commandService);

            #endregion CodeXml

            #region DocumentsXml

            DocumentsXmlXsdSchemaOpenFolderCommand.Initialize(commandService);

            DocumentsXmlRemoveCustomAttributesCommand.Initialize(commandService);

            DocumentsXmlXsdSchemaSetCommand.Initialize(commandService);

            DocumentsXmlXsdSchemaRemoveCommand.Initialize(commandService);

            #endregion DocumentsXml

            #region FileXml

            FileXmlXsdSchemaOpenFolderCommand.Initialize(commandService);

            FileXmlXsdRemoveCustomAttributesCommand.Initialize(commandService);

            FileXmlXsdSchemaSetCommand.Initialize(commandService);

            FileXmlXsdSchemaRemoveCommand.Initialize(commandService);

            #endregion FileXml

            #region FolderXml

            FolderXmlXsdSchemaOpenFolderCommand.Initialize(commandService);

            FolderXmlRemoveCustomAttributesCommand.Initialize(commandService);

            FolderXmlXsdSchemaSetCommand.Initialize(commandService);

            FolderXmlXsdSchemaRemoveCommand.Initialize(commandService);

            #endregion FolderXml



            #region CodeReport

            CodeReportLinkClearCommand.Initialize(commandService);
            CodeReportLinkCreateCommand.Initialize(commandService);
            CodeReportUpdateCommand.Initialize(commandService);
            CodeReportCreateCommand.Initialize(commandService);
            CodeReportShowDifferenceCommand.Initialize(commandService);
            CodeReportShowDifferenceInConnectionGroupCommand.Initialize(commandService);
            CodeReportShowDifferenceThreeFileCommand.Initialize(commandService);
            CodeReportExplorerCommand.Initialize(commandService);
            CodeReportOpenInWebInConnectionCommand.Initialize(commandService);

            CodeReportAddToSolutionLastCommand.Initialize(commandService);
            CodeReportAddToSolutionInConnectionCommand.Initialize(commandService);

            #endregion CodeReport

            #region DocumentsReport

            DocumentsReportLinkClearCommand.Initialize(commandService);

            DocumentsReportAddToSolutionLastCommand.Initialize(commandService);
            DocumentsReportAddToSolutionInConnectionCommand.Initialize(commandService);

            #endregion DocumentsReport

            #region FileReport

            FileReportExplorerCommand.Initialize(commandService);
            FileReportLinkClearCommand.Initialize(commandService);
            FileReportLinkCreateCommand.Initialize(commandService);
            FileReportUpdateCommand.Initialize(commandService);
            FileReportCreateCommand.Initialize(commandService);
            FileReportOpenInWebInConnectionCommand.Initialize(commandService);

            FileReportAddToSolutionLastCommand.Initialize(commandService);
            FileReportAddToSolutionInConnectionCommand.Initialize(commandService);

            #endregion FileReport



            #region CodeCSharp

            CodeCSharpEntityMetadataFileGenerationOptionsCommand.Initialize(commandService);
            CodeCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand.Initialize(commandService);

            CodeCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand.Initialize(commandService);
            CodeCSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand.Initialize(commandService);

            CodeCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            CodeCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);

            CodeCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);
            CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(commandService);

            CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand.Initialize(commandService);
            CodeCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand.Initialize(commandService);

            CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand.Initialize(commandService);
            CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand.Initialize(commandService);

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

            #endregion CodeCSharp

            #region DocumentsCSharp

            DocumentsCSharpEntityMetadataFileGenerationOptionsCommand.Initialize(commandService);
            DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand.Initialize(commandService);

            DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand.Initialize(commandService);

            DocumentsCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            DocumentsCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);

            DocumentsCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);

            DocumentsCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);
            DocumentsCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);
            DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);
            DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);

            #endregion DocumentsCSharp

            #region FileCSharp

            FileCSharpEntityMetadataFileGenerationOptionsCommand.Initialize(commandService);
            FileCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand.Initialize(commandService);

            FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand.Initialize(commandService);
            FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand.Initialize(commandService);

            FileCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            FileCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);

            FileCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);
            FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand.Initialize(commandService);

            FileCSharpPluginAssemblyExplorerCommand.Initialize(commandService);

            FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand.Initialize(commandService);
            FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand.Initialize(commandService);

            FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand.Initialize(commandService);
            FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand.Initialize(commandService);

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

            #endregion FileCSharp

            #region FolderCSharp

            FolderCSharpEntityMetadataFileGenerationOptionsCommand.Initialize(commandService);
            FolderCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand.Initialize(commandService);

            FolderCSharpProjectPluginAssemblyAddToSolutionInConnectionCommand.Initialize(commandService);
            FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand.Initialize(commandService);

            FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand.Initialize(commandService);
            FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand.Initialize(commandService);

            FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand.Initialize(commandService);
            FolderCSharpProjectPluginTypeStepsAddToSolutionLastCommand.Initialize(commandService);

            FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand.Initialize(commandService);
            FolderCSharpUpdateEntityMetadataFileSchemaCommand.Initialize(commandService);
            FolderCSharpUpdateEntityMetadataFileProxyClassCommand.Initialize(commandService);

            FolderCSharpUpdateGlobalOptionSetsFileCommand.Initialize(commandService);

            #endregion FolderCSharp



            #region FolderAdd

            FolderAddPluginConfigurationFileCommand.Initialize(commandService);
            FolderAddSolutionFileCommand.Initialize(commandService);
            FolderAddEntityMetadataFileInConnectionCommand.Initialize(commandService);
            FolderAddGlobalOptionSetFileInConnectionCommand.Initialize(commandService);
            FolderAddSdkMessageRequestFileInConnectionCommand.Initialize(commandService);
            FolderAddSystemFormJavaScriptFileInConnectionCommand.Initialize(commandService);

            #endregion FolderAdd

            #region Project

            ProjectBuildLoadUpdatePluginAssemblyCommand.Initialize(commandService);
            ProjectBuildLoadUpdatePluginAssemblyInConnectionCommand.Initialize(commandService);

            ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand.Initialize(commandService);
            ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand.Initialize(commandService);

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

            #endregion Project

            #region ListForPublish

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

            ListForPublishOpenFilesCommand.Initialize(commandService);

            ListForPublishMultiDifferenceCommand.Initialize(commandService);

            #endregion ListForPublish

            #region CrmConnection

            CommonCurrentConnectionCommand.Initialize(commandService);

            CommonPublishAllInCrmConnectionCommand.Initialize(commandService);

            CommonCrmConnectionCommand.Initialize(commandService);
            CommonCrmConnectionTestCommand.Initialize(commandService);
            CommonCrmConnectionEditCommand.Initialize(commandService);
            CommonCrmConnectionSelectFileCommand.Initialize(commandService);

            CommonSelectCrmConnectionCommand.Initialize(commandService);

            #endregion CrmConnection

            #region Common Check

            CommonCheckEntitiesOwnerShipsCommand.Initialize(commandService);
            CommonCheckWorkflowsUsedEntitiesCommand.Initialize(commandService);
            CommonCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize(commandService);
            CommonCheckGlobalOptionSetDuplicateCommand.Initialize(commandService);
            CommonCheckManagedElementsCommand.Initialize(commandService);
            CommonCheckPluginImagesCommand.Initialize(commandService);
            CommonCheckPluginImagesRequiredComponentsCommand.Initialize(commandService);
            CommonCheckPluginStepsCommand.Initialize(commandService);
            CommonCheckPluginStepsRequiredComponentsCommand.Initialize(commandService);

            CommonCheckComponentTypeEnumCommand.Initialize(commandService);
            CommonCheckCreateAllDependencyNodeDescriptionCommand.Initialize(commandService);
            CommonCheckUnknownFormControlTypes.Initialize(commandService);

            #endregion Common Check

            #region Common Finds and Edits

            CommonFindEntityObjectsByPrefixCommand.Initialize(commandService);
            CommonFindEntityObjectsByPrefixInExplorerCommand.Initialize(commandService);

            CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommand.Initialize(commandService);

            CommonFindEntityObjectsMarkedToDeleteInExplorerCommand.Initialize(commandService);
            CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand.Initialize(commandService);

            CommonFindEntityObjectsByNameCommand.Initialize(commandService);
            CommonFindEntityObjectsByNameInExplorerCommand.Initialize(commandService);

            CommonFindEntityObjectsContainsStringCommand.Initialize(commandService);
            CommonFindEntityObjectsContainsStringInExplorerCommand.Initialize(commandService);

            CommonFindEntitiesByIdCommand.Initialize(commandService);
            CommonFindEntitiesByUniqueidentifierCommand.Initialize(commandService);

            CommonEditEntitiesByIdCommand.Initialize(commandService);

            #endregion Common Finds and Edits

            #region Common Entity

            CommonEntityMetadataExplorerCommand.Initialize(commandService);
            CommonEntityAttributeExplorerCommand.Initialize(commandService);
            CommonEntityKeyExplorerCommand.Initialize(commandService);
            CommonEntityRelationshipOneToManyExplorerCommand.Initialize(commandService);
            CommonEntityRelationshipManyToManyExplorerCommand.Initialize(commandService);
            CommonEntityPrivilegesExplorerCommand.Initialize(commandService);

            #endregion Common Entity

            #region Common Entity Objects

            CommonSystemFormExplorerCommand.Initialize(commandService);
            CommonSystemSavedQueryVisualizationExplorerCommand.Initialize(commandService);
            CommonSystemSavedQueryExplorerCommand.Initialize(commandService);

            #endregion Common Entity Objects

            #region Explorers

            CommonGlobalOptionSetsExplorerCommand.Initialize(commandService);
            CommonOrganizationExplorerCommand.Initialize(commandService);
            CommonCustomControlExplorerCommand.Initialize(commandService);
            CommonApplicationRibbonExplorerCommand.Initialize(commandService);
            CommonSiteMapExplorerCommand.Initialize(commandService);
            CommonReportExplorerCommand.Initialize(commandService);
            CommonWebResourceExplorerCommand.Initialize(commandService);
            CommonWorkflowExplorerCommand.Initialize(commandService);

            CommonExportFormEventsCommand.Initialize(commandService);

            #endregion Explorers

            #region Security

            CommonOtherPrivilegesExplorerCommand.Initialize(commandService);
            CommonSystemUsersExplorerCommand.Initialize(commandService);
            CommonTeamsExplorerCommand.Initialize(commandService);
            CommonSecurityRolesExplorerCommand.Initialize(commandService);

            #endregion Security

            #region Solutions

            CommonSolutionExplorerCommand.Initialize(commandService);
            CommonSolutionExplorerInConnectionCommand.Initialize(commandService);
            CommonImportJobExplorerInConnectionCommand.Initialize(commandService);

            CommonOpenSolutionImageCommand.Initialize(commandService);
            CommonOpenSolutionDifferenceImageCommand.Initialize(commandService);

            CommonOpenDefaultSolutionInWebCommand.Initialize(commandService);
            CommonExportOpenLastSelectedSolutionCommand.Initialize(commandService);

            #endregion Solutions

            #region PluginInfo

            CommonPluginAssemblyExplorerCommand.Initialize(commandService);
            CommonPluginTypeExplorerCommand.Initialize(commandService);

            CommonPluginTreeCommand.Initialize(commandService);
            CommonSdkMessageTreeCommand.Initialize(commandService);
            CommonSdkMessageRequestTreeCommand.Initialize(commandService);

            #endregion PluginInfo

            #region PluginConfiguration

            CommonPluginConfigurationComparerPluginAssemblyCommand.Initialize(commandService);
            CommonPluginConfigurationCreateCommand.Initialize(commandService);
            CommonPluginConfigurationPluginAssemblyCommand.Initialize(commandService);
            CommonPluginConfigurationPluginTreeCommand.Initialize(commandService);
            CommonPluginConfigurationPluginTypeCommand.Initialize(commandService);

            #endregion PluginConfiguration

            #region Trace

            CommonTraceExportFileCommand.Initialize(commandService);
            CommonTraceReaderCommand.Initialize(commandService);

            #endregion Trace

            #region OrganizationComparer

            CommonOrganizationComparerCommand.Initialize(commandService);

            CommonOpenOrganizationDifferenceImageCommand.Initialize(commandService);

            #endregion OrganizationComparer

            #region Config

            CommonFileGenerationOptionsCommand.Initialize(commandService);
            CommonFileGenerationConfigurationCommand.Initialize(commandService);

            CommonOpenConfigFolderCommand.Initialize(commandService);

            CommonConfigCommand.Initialize(commandService);

            #endregion Config

            CommonOpenCrmWebSiteCommand.Initialize(commandService);

            CommonExportDefaultSiteMapsCommand.Initialize(commandService);
            CommonXsdSchemaExportCommand.Initialize(commandService);

            CommonXsdSchemaOpenFolderCommand.Initialize(commandService);

            #region Output Windows

            OutputSelectCrmConnectionCommand.Initialize(commandService);
            OutputTestCrmConnectionCommand.Initialize(commandService);
            OutputEditCrmConnectionCommand.Initialize(commandService);
            OutputSelectFileCrmConnectionCommand.Initialize(commandService);

            OutputOpenCrmWebSiteCommand.Initialize(commandService);
            OutputExportOpenLastSelectedSolutionCommand.Initialize(commandService);
            OutputOpenDefaultSolutionInWebCommand.Initialize(commandService);

            OutputOpenOrganizationDifferenceImageCommand.Initialize(commandService);
            OutputOpenSolutionDifferenceImageCommand.Initialize(commandService);
            OutputOpenSolutionImageCommand.Initialize(commandService);

            OutputOrganizationComparerCommand.Initialize(commandService);

            OutputExportFormEventsCommand.Initialize(commandService);

            #region Finds and Edits

            OutputFindEntityObjectsByPrefixCommand.Initialize(commandService);
            OutputFindEntityObjectsByPrefixInExplorerCommand.Initialize(commandService);

            OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommand.Initialize(commandService);

            OutputFindEntityObjectsMarkedToDeleteInExplorerCommand.Initialize(commandService);
            OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand.Initialize(commandService);

            OutputFindEntityObjectsByNameCommand.Initialize(commandService);
            OutputFindEntityObjectsByNameInExplorerCommand.Initialize(commandService);

            OutputFindEntityObjectsContainsStringCommand.Initialize(commandService);
            OutputFindEntityObjectsContainsStringInExplorerCommand.Initialize(commandService);

            OutputFindEntitiesByIdCommand.Initialize(commandService);
            OutputFindEntitiesByUniqueidentifierCommand.Initialize(commandService);

            OutputEditEntitiesByIdCommand.Initialize(commandService);

            #endregion Finds and Edits

            #region PluginConfiguration

            OutputPluginConfigurationComparerPluginAssemblyCommand.Initialize(commandService);
            OutputPluginConfigurationCreateCommand.Initialize(commandService);
            OutputPluginConfigurationPluginAssemblyCommand.Initialize(commandService);
            OutputPluginConfigurationPluginTreeCommand.Initialize(commandService);
            OutputPluginConfigurationPluginTypeCommand.Initialize(commandService);

            #endregion PluginConfiguration

            OutputApplicationRibbonExplorerCommand.Initialize(commandService);
            OutputCheckComponentTypeEnumCommand.Initialize(commandService);
            OutputCheckCreateAllDependencyNodeDescriptionCommand.Initialize(commandService);
            OutputCheckEntitiesOwnerShipsCommand.Initialize(commandService);
            OutputCheckGlobalOptionSetDuplicateCommand.Initialize(commandService);
            OutputCheckManagedElementsCommand.Initialize(commandService);
            OutputCheckPluginImagesCommand.Initialize(commandService);
            OutputCheckPluginImagesRequiredComponentsCommand.Initialize(commandService);
            OutputCheckPluginStepsCommand.Initialize(commandService);
            OutputCheckPluginStepsRequiredComponentsCommand.Initialize(commandService);
            OutputCheckWorkflowsUsedEntitiesCommand.Initialize(commandService);
            OutputCheckWorkflowsUsedNotExistsEntitiesCommand.Initialize(commandService);
            OutputCheckUnknownFormControlTypes.Initialize(commandService);

            OutputCustomControlExplorerCommand.Initialize(commandService);
            OutputEntityAttributeExplorerCommand.Initialize(commandService);
            OutputEntityKeyExplorerCommand.Initialize(commandService);
            OutputEntityMetadataExplorerCommand.Initialize(commandService);
            OutputEntityPrivilegesExplorerCommand.Initialize(commandService);
            OutputEntityRelationshipManyToManyExplorerCommand.Initialize(commandService);
            OutputEntityRelationshipOneToManyExplorerCommand.Initialize(commandService);
            OutputOtherPrivilegesExplorerCommand.Initialize(commandService);

            OutputGlobalOptionSetsExplorerCommand.Initialize(commandService);
            OutputImportJobExplorerInConnectionCommand.Initialize(commandService);
            OutputOrganizationExplorerCommand.Initialize(commandService);
            OutputPluginAssemblyExplorerCommand.Initialize(commandService);
            OutputPluginTreeCommand.Initialize(commandService);
            OutputPluginTypeExplorerCommand.Initialize(commandService);
            OutputReportExplorerCommand.Initialize(commandService);
            OutputSdkMessageRequestTreeCommand.Initialize(commandService);
            OutputSdkMessageTreeCommand.Initialize(commandService);
            OutputSecurityRolesExplorerCommand.Initialize(commandService);
            OutputSiteMapExplorerCommand.Initialize(commandService);
            OutputSolutionExplorerCommand.Initialize(commandService);
            OutputSystemFormExplorerCommand.Initialize(commandService);
            OutputSystemSavedQueryExplorerCommand.Initialize(commandService);
            OutputSystemSavedQueryVisualizationExplorerCommand.Initialize(commandService);
            OutputSystemUsersExplorerCommand.Initialize(commandService);
            OutputTeamsExplorerCommand.Initialize(commandService);
            OutputTraceExportFileCommand.Initialize(commandService);
            OutputTraceReaderCommand.Initialize(commandService);
            OutputWebResourceExplorerCommand.Initialize(commandService);
            OutputWorkflowExplorerCommand.Initialize(commandService);

            OutputPublishAllInCrmConnectionCommand.Initialize(commandService);


            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);
            //Output.Initialize(commandService);

            #endregion Output Windows



            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);
            //Folder.Initialize(commandService);

            this.ApplicationObject = await this.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

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
