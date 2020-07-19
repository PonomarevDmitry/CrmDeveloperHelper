using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    /// <summary>
    /// <see cref="Helpers.DTEHelper"/>
    /// </summary>
    public interface IWriteToOutput
    {
        // \.WriteToOutput\(.*, "

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutput(ConnectionData, string, object[])"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string WriteToOutput(ConnectionData connectionData, string format, params object[] args);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputEntityInstance(ConnectionData, Entity)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, Entity entity);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputEntityInstance(ConnectionData, EntityReference)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="entityRef"></param>
        /// <returns></returns>
        IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, EntityReference entityRef);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputEntityInstance(ConnectionData, string, Guid)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, string entityName, Guid id);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputStartOperation(ConnectionData, string, object[])"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string WriteToOutputStartOperation(ConnectionData connectionData, string format, params object[] args);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputEndOperation(ConnectionData, string, object[])"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string WriteToOutputEndOperation(ConnectionData connectionData, string format, params object[] args);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputFilePathUri(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput WriteToOutputFilePathUri(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputFilePathUriToOpenInExcel(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput WriteToOutputFilePathUriToOpenInExcel(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteToOutputSolutionUri(ConnectionData, string, Guid)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="solutionUniqueName"></param>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        IWriteToOutput WriteToOutputSolutionUri(ConnectionData connectionData, string solutionUniqueName, Guid solutionId);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.WriteErrorToOutput(ConnectionData, Exception, string, object[])"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        IWriteToOutput WriteErrorToOutput(ConnectionData connectionData, Exception ex, string message = null, params object[] args);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.ActivateOutputWindow(ConnectionData)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <returns></returns>
        IWriteToOutput ActivateOutputWindow(ConnectionData connectionData);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.ActivateOutputWindow(ConnectionData, System.Windows.Window)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        IWriteToOutput ActivateOutputWindow(ConnectionData connectionData, System.Windows.Window window);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.ActivateVisualStudioWindow"/>
        /// </summary>
        /// <returns></returns>
        IWriteToOutput ActivateVisualStudioWindow();

        /// <summary>
        /// <see cref="Helpers.DTEHelper.PerformAction(ConnectionData, string, bool)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <param name="hideFilePathUri"></param>
        /// <returns></returns>
        IWriteToOutput PerformAction(ConnectionData connectionData, string filePath, bool hideFilePathUri = false);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.OpenFile(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput OpenFile(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.OpenFileInVisualStudio(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput OpenFileInVisualStudio(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.OpenFetchXmlFile(ConnectionData, CommonConfiguration, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="commonConfig"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        IWriteToOutput OpenFetchXmlFile(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.OpenFileInTextEditor(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput OpenFileInTextEditor(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.OpenFileInExcel(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput OpenFileInExcel(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.SelectFileInFolder(ConnectionData, string)"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IWriteToOutput SelectFileInFolder(ConnectionData connectionData, string filePath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.OpenFolder(ConnectionData, string)"/>
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        IWriteToOutput OpenFolder(ConnectionData connectionData, string folderPath);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.ProcessStartProgramComparerAsync(ConnectionData, string, string, string, string, ConnectionData)"/>
        /// </summary>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <param name="fileTitle1"></param>
        /// <param name="fileTitle2"></param>
        Task ProcessStartProgramComparerAsync(ConnectionData connectionData1, string filePath1, string filePath2, string fileTitle1, string fileTitle2, ConnectionData connectionData2 = null);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.ProcessStartProgramComparerThreeWayFile(string, string, string, string, string, string)"/>
        /// </summary>
        /// <param name="fileLocalPath"></param>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <param name="fileLocalTitle"></param>
        /// <param name="fileTitle1"></param>
        /// <param name="fileTitle2"></param>
        /// <returns></returns>
        IWriteToOutput ProcessStartProgramComparerThreeWayFile(string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.BuildProjectAsync(EnvDTE.Project)"/>
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task<int> BuildProjectAsync(EnvDTE.Project project);
    }
}