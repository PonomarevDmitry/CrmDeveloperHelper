using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IWriteToOutput
    {
        string WriteToOutput(string format, params object[] args);

        string WriteToOutputStartOperation(string format, params object[] args);

        string WriteToOutputEndOperation(string format, params object[] args);

        void WriteToOutputFilePathUri(string filePath);

        void WriteToOutputSolutionUri(ConnectionData connectionData, string solutionUniqueName, Guid solutionId);

        void WriteErrorToOutput(Exception ex, string message = null, params object[] args);

        void ActivateOutputWindow();

        void ActivateVisualStudioWindow();

        void PerformAction(string filePath, bool hideFilePathUri = false);

        void OpenFile(string filePath);

        void OpenFileInVisualStudio(string filePath);

        void OpenFileInTextEditor(string filePath);

        void SelectFileInFolder(string filePath);

        void ProcessStartProgramComparer(string file1, string file2, string fileTitle1, string fileTitle2);

        void ProcessStartProgramComparerThreeWayFile(string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2);
    }
}