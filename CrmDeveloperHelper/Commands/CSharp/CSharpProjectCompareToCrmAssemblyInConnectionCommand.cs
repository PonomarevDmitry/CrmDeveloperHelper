using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectCompareToCrmAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectCompareToCrmAssemblyInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectCompareToCrmAssemblyInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectCompareToCrmAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectCompareToCrmAssemblyInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectCompareToCrmAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectCompareToCrmAssemblyInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectCompareToCrmAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectCompareToCrmAssemblyInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectCompareToCrmAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectCompareToCrmAssemblyInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectCompareToCrmAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectCompareToCrmAssemblyInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyComparingWithLocalAssemblyCommand(connectionData, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}