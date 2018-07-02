using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IWriteToOutput
    {
        string WriteToOutput(string format, params object[] args);

        void WriteToOutputFilePathUri(string filePath);

        void WriteErrorToOutput(Exception ex);

        void ActivateOutputWindow();

        void ActivateVisualStudioWindow();

        void PerformAction(string filePath, CommonConfiguration commonConfig);

        void OpenFile(string filePath, CommonConfiguration commonConfig);

        void OpenFileInVisualStudio(string filePath);

        void OpenFileInTextEditor(string filePath, CommonConfiguration commonConfig);

        void SelectFileInFolder(string filePath);

        void ProcessStartProgramComparer(CommonConfiguration commonConfig, string file1, string file2, string fileTitle1, string fileTitle2);

        void ProcessStartProgramComparerThreeWayFile(CommonConfiguration commonConfig, string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2);
    }
}