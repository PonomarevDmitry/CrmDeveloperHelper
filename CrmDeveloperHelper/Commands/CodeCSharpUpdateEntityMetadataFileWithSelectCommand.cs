﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpUpdateEntityMetadataFileWithSelectCommand : AbstractCommand
    {
        private CodeCSharpUpdateEntityMetadataFileWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpUpdateEntityMetadataFileWithSelectCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpUpdateEntityMetadataFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpUpdateEntityMetadataFileWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType);

            helper.HandleUpdateEntityMetadataFileCSharp(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeCSharpUpdateEntityMetadataFileWithSelectCommand);
        }
    }
}
