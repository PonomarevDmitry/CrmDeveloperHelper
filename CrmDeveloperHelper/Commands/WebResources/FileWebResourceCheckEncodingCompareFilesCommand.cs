﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceCheckEncodingCompareFilesCommand : AbstractCommand
    {
        private FileWebResourceCheckEncodingCompareFilesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceCheckEncodingCompareFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny) { }

        public static FileWebResourceCheckEncodingCompareFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceCheckEncodingCompareFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            helper.HandleCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }
    }
}
