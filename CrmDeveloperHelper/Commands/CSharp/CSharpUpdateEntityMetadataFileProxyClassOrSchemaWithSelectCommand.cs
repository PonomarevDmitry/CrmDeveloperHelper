using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand InstanceCode { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand
            );

            InstanceFile = new CSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaWithSelectCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateProxyClass(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}