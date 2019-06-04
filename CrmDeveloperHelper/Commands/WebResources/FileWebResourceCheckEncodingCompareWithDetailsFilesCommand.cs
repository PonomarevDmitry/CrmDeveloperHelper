﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceCheckEncodingCompareWithDetailsFilesCommand : AbstractCommand
    {
        private FileWebResourceCheckEncodingCompareWithDetailsFilesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceCheckEncodingCompareWithDetailsFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny) { }

        public static FileWebResourceCheckEncodingCompareWithDetailsFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceCheckEncodingCompareWithDetailsFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            helper.HandleCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }
    }
}
