using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectCompareTypesToCrmPluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectCompareTypesToCrmPluginAssemblyInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyComparingPluginTypesWithLocalAssemblyCommand(connectionData, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}