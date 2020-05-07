using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal class DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId)
        {
        }

        public static DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            System.Threading.Tasks.Task.WaitAll(ExecuteAsync(helper, connectionData));
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, ConnectionData connectionData)
        {
            try
            {
                var listFiles = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType).ToList();

                var pluginTypeNames = new List<string>();

                helper.ActivateOutputWindow(null);

                foreach (var item in listFiles)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, item?.FullName);
                    var typeName = await PropertiesHelper.GetTypeFullNameAsync(item);

                    if (!string.IsNullOrEmpty(typeName))
                    {
                        pluginTypeNames.Add(typeName);
                    }
                }

                if (pluginTypeNames.Any())
                {
                    helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(connectionData, null, true, pluginTypeNames.ToArray());
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
        }
    }
}