using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectCompareToCrmAssemblyCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly string _commandNameForCorrection;

        private CSharpProjectCompareToCrmAssemblyCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpProjectCompareToCrmAssemblyCommand InstanceCode { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyCommand InstanceDocuments { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyCommand InstanceFile { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyCommand InstanceFolder { get; private set; }

        public static CSharpProjectCompareToCrmAssemblyCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectCompareToCrmAssemblyCommand(commandService, PackageIds.guidCommandSet.CodeCSharpProjectCompareToCrmAssemblyCommandId, sourceCode, Properties.CommandNames.CodeCSharpProjectCompareToCrmAssemblyCommand);

            InstanceDocuments = new CSharpProjectCompareToCrmAssemblyCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpProjectCompareToCrmAssemblyCommandId, sourceDocuments, Properties.CommandNames.DocumentsCSharpProjectCompareToCrmAssemblyCommand);

            InstanceFile = new CSharpProjectCompareToCrmAssemblyCommand(commandService, PackageIds.guidCommandSet.FileCSharpProjectCompareToCrmAssemblyCommandId, sourceFile, Properties.CommandNames.FileCSharpProjectCompareToCrmAssemblyCommand);

            InstanceFolder = new CSharpProjectCompareToCrmAssemblyCommand(commandService, PackageIds.guidCommandSet.FolderCSharpProjectCompareToCrmAssemblyCommandId, sourceFolder, Properties.CommandNames.FolderCSharpProjectCompareToCrmAssemblyCommand);

            InstanceProject = new CSharpProjectCompareToCrmAssemblyCommand(commandService, PackageIds.guidCommandSet.ProjectCompareToCrmAssemblyCommandId, sourceProject, Properties.CommandNames.ProjectCompareToCrmAssemblyCommand);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            //helper.HandlePluginAssemblyComparingWithLocalAssemblyCommand(null, selectedProjects.FirstOrDefault());
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
