using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpUpdateGlobalOptionSetsFileWithSelectCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private CSharpUpdateGlobalOptionSetsFileWithSelectCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpUpdateGlobalOptionSetsFileWithSelectCommand InstanceCode { get; private set; }

        public static CSharpUpdateGlobalOptionSetsFileWithSelectCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new CSharpUpdateGlobalOptionSetsFileWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpUpdateGlobalOptionSetsFileWithSelectCommand
            );

            InstanceFile = new CSharpUpdateGlobalOptionSetsFileWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpUpdateGlobalOptionSetsFileWithSelectCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleCSharpGlobalOptionSetsFileUpdateSchema(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
