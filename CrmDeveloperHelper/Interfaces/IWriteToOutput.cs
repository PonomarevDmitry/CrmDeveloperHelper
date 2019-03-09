using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IWriteToOutput
    {
        // \.WriteToOutput\(.*, "
        string WriteToOutput(ConnectionData connectionData, string format, params object[] args);

        string WriteToOutputStartOperation(ConnectionData connectionData, string format, params object[] args);

        string WriteToOutputEndOperation(ConnectionData connectionData, string format, params object[] args);

        void WriteToOutputFilePathUri(ConnectionData connectionData, string filePath);

        void WriteToOutputSolutionUri(ConnectionData connectionData, string solutionUniqueName, Guid solutionId);

        void WriteErrorToOutput(ConnectionData connectionData, Exception ex, string message = null, params object[] args);

        void ActivateOutputWindow(ConnectionData connectionData);

        void ActivateVisualStudioWindow();

        void PerformAction(ConnectionData connectionData, string filePath, bool hideFilePathUri = false);

        void OpenFile(ConnectionData connectionData, string filePath);

        void OpenFileInVisualStudio(ConnectionData connectionData, string filePath);

        void OpenFileInTextEditor(ConnectionData connectionData, string filePath);

        void SelectFileInFolder(ConnectionData connectionData, string filePath);

        void ProcessStartProgramComparer(string file1, string file2, string fileTitle1, string fileTitle2);

        void ProcessStartProgramComparerThreeWayFile(string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2);
    }
}