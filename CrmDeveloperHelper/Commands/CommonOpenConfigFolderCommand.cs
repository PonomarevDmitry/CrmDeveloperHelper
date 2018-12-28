using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenConfigFolderCommand : AbstractCommand
    {
        private CommonOpenConfigFolderCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOpenConfigFolderCommandId, ActionExecute, null) { }

        public static CommonOpenConfigFolderCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOpenConfigFolderCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
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