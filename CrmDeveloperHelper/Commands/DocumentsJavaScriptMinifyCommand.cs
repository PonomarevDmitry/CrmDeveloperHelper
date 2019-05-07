using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsJavaScriptMinifyCommand : AbstractCommand
    {
        private DocumentsJavaScriptMinifyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsJavaScriptMinifyCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsSupportsMinification) { }

        public static DocumentsJavaScriptMinifyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsJavaScriptMinifyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var documents = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsMinification);

            helper.MinifyDocuments(documents);
        }
    }
}