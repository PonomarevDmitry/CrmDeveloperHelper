using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenConfigFolderCommand : AbstractCommand
    {
        private CommonOpenConfigFolderCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonOpenConfigFolderCommandId) { }

        public static CommonOpenConfigFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenConfigFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var directory = FileOperations.GetConfigurationFolder();

            if (!string.IsNullOrEmpty(directory))
            {
                if (Directory.Exists(directory))
                {
                    try
                    {
                        Process.Start(directory);
                    }
                    catch (Exception)
                    { }
                }
            }
        }
    }
}