using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IWriteToOutput
    {
        // \.WriteToOutput\(.*, "
        string WriteToOutput(ConnectionData connectionData, string format, params object[] args);

        IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, Entity entity);

        IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, EntityReference entityRef);

        IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, string entityName, Guid id);

        string WriteToOutputStartOperation(ConnectionData connectionData, string format, params object[] args);

        string WriteToOutputEndOperation(ConnectionData connectionData, string format, params object[] args);

        IWriteToOutput WriteToOutputFilePathUri(ConnectionData connectionData, string filePath);

        IWriteToOutput WriteToOutputFilePathUriToOpenInExcel(ConnectionData connectionData, string filePath);

        IWriteToOutput WriteToOutputSolutionUri(ConnectionData connectionData, string solutionUniqueName, Guid solutionId);

        IWriteToOutput WriteErrorToOutput(ConnectionData connectionData, Exception ex, string message = null, params object[] args);

        IWriteToOutput ActivateOutputWindow(ConnectionData connectionData);

        IWriteToOutput ActivateVisualStudioWindow();

        IWriteToOutput PerformAction(ConnectionData connectionData, string filePath, bool hideFilePathUri = false);

        IWriteToOutput OpenFile(ConnectionData connectionData, string filePath);

        IWriteToOutput OpenFileInVisualStudio(ConnectionData connectionData, string filePath);

        IWriteToOutput OpenFileInTextEditor(ConnectionData connectionData, string filePath);

        IWriteToOutput OpenFileInExcel(ConnectionData connectionData, string filePath);

        IWriteToOutput SelectFileInFolder(ConnectionData connectionData, string filePath);

        void ProcessStartProgramComparer(string file1, string file2, string fileTitle1, string fileTitle2);

        IWriteToOutput ProcessStartProgramComparerThreeWayFile(string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2);

        Task<int> BuildProjectAsync(EnvDTE.Project project);
    }
}