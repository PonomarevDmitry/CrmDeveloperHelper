using EnvDTE;
using EnvDTE80;
using NLog;
using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NLog.Targets;
using NLog.Config;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class DTEHelper : IWriteToOutputAndPublishList
    {
        private const string _outputWindowName = "Crm Developer Helper";

        private const int timeDelay = 2000;

        private static readonly Guid ToolsDiffCommandGuid = new Guid("5D4C0442-C0A2-4BE8-9B4D-AB1C28450942");
        private const int ToolsDiffCommandId = 256;

        public DTE2 ApplicationObject { get; private set; }

        private MainController Controller;

        private HashSet<string> _ListForPublish = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static DTEHelper _singleton;

        public static DTEHelper Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    var applicationObject = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

                    if (applicationObject != null)
                    {
                        _singleton = new DTEHelper(applicationObject);
                    }
                }

                return _singleton;
            }
        }

        private static object syncObject = new object();

        private static Logger _loggerOutput;

        public static Logger Log { get; private set; }

        //FormatTextTableHandler table = new FormatTextTableHandler();
        //table.SetHeader("Name", "LocalizedName", "Guid", "ID");

        //foreach (var item in applicationObject.Commands.OfType<EnvDTE.Command>())
        //{
        //    table.AddLine(item.Name, item.LocalizedName, item.Guid, item.ID.ToString());
        //}

        //var result = new StringBuilder();
        //File.WriteAllLines(@"C:\Temp\Commands.txt", table.GetFormatedLines(true), Encoding.UTF8);

        public static DTEHelper Create(DTE2 applicationObject)
        {
            if (applicationObject == null)
            {
                throw new ArgumentNullException(nameof(applicationObject));
            }

            lock (syncObject)
            {
                if (_singleton != null)
                {
                    return _singleton;
                }

                _singleton = new DTEHelper(applicationObject);

                return _singleton;
            }
        }

        private const string loggerOutput = "OutputLogger";
        private const string loggerErrors = "ErrorLogger";

        static DTEHelper()
        {
            ConfigureNLog();
        }

        private static void ConfigureNLog()
        {
            var config = new LoggingConfiguration();

            {
                FileTarget targetGenFile = new FileTarget(loggerOutput);
                targetGenFile.Name = loggerOutput;
                targetGenFile.LineEnding = LineEndingMode.CRLF;
                targetGenFile.Encoding = Encoding.UTF8;
                targetGenFile.WriteBom = true;
                targetGenFile.CreateDirs = true;
                targetGenFile.FileName = Path.Combine(FileOperations.GetOutputPath(), @"Output ${date:format=yyyy-MM-dd}.log");
                targetGenFile.Layout = "${message}";

                config.AddTarget(loggerOutput, targetGenFile);

                config.AddRuleForAllLevels(targetGenFile, loggerOutput);
            }

            {
                FileTarget targetGenFile = new FileTarget(loggerErrors);
                targetGenFile.Name = loggerErrors;
                targetGenFile.LineEnding = LineEndingMode.CRLF;
                targetGenFile.Encoding = Encoding.UTF8;
                targetGenFile.WriteBom = true;
                targetGenFile.CreateDirs = true;
                targetGenFile.FileName = Path.Combine(FileOperations.GetLogsPath(), @"Log ${date:format=yyyy-MM-dd}.log");
                targetGenFile.Layout = "${newline}${newline}${newline}${newline}${newline}${newline}${longdate}|${level}${newline}${message}${newline}${newline}${exception}${newline}${newline}${stacktrace:format=Raw:topFrames=7}";

                config.AddTarget(loggerErrors, targetGenFile);

                config.AddRuleForAllLevels(targetGenFile, loggerErrors);
            }

            LogManager.Configuration = config;

            _loggerOutput = LogManager.GetLogger(loggerOutput);
            Log = LogManager.GetLogger(loggerErrors);
        }

        private DTEHelper(DTE2 applicationObject)
        {
            this.ApplicationObject = applicationObject ?? throw new ArgumentNullException(nameof(applicationObject));

            this.Controller = new MainController(this);

            System.Threading.Thread clearTempFiles = new System.Threading.Thread(ClearTemporaryFiles);
            clearTempFiles.Start();
        }

        private void ClearTemporaryFiles()
        {
            string directory = FileOperations.GetTempFileFolder();

            if (Directory.Exists(directory))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);

                    var files = dir.GetFiles();

                    foreach (var item in files)
                    {
                        try
                        {
                            item.Delete();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void WriteExceptionToOutput(Exception ex)
        {
            if (Singleton != null)
            {
                Singleton.WriteErrorToOutput(ex);
            }
            else
            {
                WriteExceptionToLog(ex);
            }
        }

        public static void WriteExceptionToLog(Exception ex)
        {
            var description = GetExceptionDescription(ex);

            Log.Error(ex, description);
        }

        public static string GetExceptionDescription(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();

            FillExceptionInformation(ex, stringBuilder);

            return stringBuilder.ToString();
        }

        private static void FillExceptionInformation(Exception ex, StringBuilder stringBuilder)
        {
            if (ex == null)
            {
                return;
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Exception.Message - {0}", ex.Message).AppendLine();
            stringBuilder.AppendFormat("Exception.HResult - {0}", ex.HResult).AppendLine();
            stringBuilder.AppendLine(ex.Source);
            stringBuilder.AppendLine(ex.StackTrace);
            stringBuilder.AppendLine(ex.ToString());

            if (ex is FaultException<OrganizationServiceFault> fault)
            {
                if (!string.IsNullOrEmpty(fault.Action))
                {
                    stringBuilder.AppendFormat("FaultException.Action - {0}", fault.Action).AppendLine();
                }
                if (!string.IsNullOrEmpty(fault.Message))
                {
                    stringBuilder.AppendFormat("FaultException.Message - {0}", fault.Message).AppendLine();
                }


                WriteFaultCodeInformation(fault.Code, "FaultException.Code", stringBuilder);

                WriteFaultReasonInformation(fault.Reason, "FaultException.Reason", stringBuilder);

                WriteOrganizationServiceFaultInformation(fault.Detail, "FaultException.Detail", stringBuilder);
            }

            if (ex is AggregateException aggregateException)
            {
                if (aggregateException.InnerExceptions != null)
                {
                    foreach (var inner in aggregateException.InnerExceptions)
                    {
                        FillExceptionInformation(inner, stringBuilder);
                    }
                }
            }
            else if (ex.InnerException != null)
            {
                FillExceptionInformation(ex.InnerException, stringBuilder);
            }
        }

        private static void WriteFaultCodeInformation(FaultCode faultCode, string prefix, StringBuilder stringBuilder)
        {
            if (faultCode == null)
            {
                return;
            }

            stringBuilder.AppendFormat("{0} - {1}", prefix, faultCode).AppendLine();
            if (!string.IsNullOrEmpty(faultCode.Name))
            {
                stringBuilder.AppendFormat("{0}.Name - {1}", prefix, faultCode.Name).AppendLine();
            }
            if (!string.IsNullOrEmpty(faultCode.Namespace))
            {
                stringBuilder.AppendFormat("{0}.Namespace - {1}", prefix, faultCode.Namespace).AppendLine();
            }
            stringBuilder.AppendFormat("{0}.IsPredefinedFault - {1}", prefix, faultCode.IsPredefinedFault).AppendLine();
            stringBuilder.AppendFormat("{0}.IsReceiverFault - {1}", prefix, faultCode.IsReceiverFault).AppendLine();
            stringBuilder.AppendFormat("{0}.IsSenderFault - {1}", prefix, faultCode.IsSenderFault).AppendLine();

            if (faultCode.SubCode != null)
            {
                WriteFaultCodeInformation(faultCode.SubCode, prefix + ".SubCode", stringBuilder);
            }
        }

        private static void WriteFaultReasonInformation(FaultReason faultReason, string prefix, StringBuilder stringBuilder)
        {
            if (faultReason == null)
            {
                return;
            }

            foreach (var item in faultReason.Translations)
            {
                stringBuilder.AppendFormat("{0}.{1} - {2}", prefix, item.XmlLang, item.Text).AppendLine();
            }
        }

        private static void WriteOrganizationServiceFaultInformation(OrganizationServiceFault fault, string prefix, StringBuilder stringBuilder)
        {
            if (fault == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(fault.Message))
            {
                stringBuilder.AppendFormat("{0}.Message - {1}", prefix, fault.Message).AppendLine();
            }
            stringBuilder.AppendFormat("{0}.ErrorCode - {1}", prefix, fault.ErrorCode).AppendLine();
            stringBuilder.AppendFormat("{0}.Timestamp - {1}", prefix, fault.Timestamp).AppendLine();

            if (fault.ErrorDetails != null && fault.ErrorDetails.Any())
            {
                stringBuilder.AppendFormat("{0}.ErrorDetails:", prefix).AppendLine();

                foreach (var item in fault.ErrorDetails)
                {
                    stringBuilder.AppendFormat("{0}.ErrorDetails.{1} - {2}", prefix, item.Key, item.Value).AppendLine();
                }
            }

            if (!string.IsNullOrEmpty(fault.TraceText))
            {
                stringBuilder.AppendFormat("{0}.TraceText - {1}", prefix, fault.TraceText).AppendLine();
            }

            if (fault.InnerFault != null)
            {
                WriteOrganizationServiceFaultInformation(fault.InnerFault, prefix + ".InnerFault", stringBuilder);
            }
        }

        #region Получение списков файлов.

        private string GetSelectedText()
        {
            string result = string.Empty;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Selection != null
                && ApplicationObject.ActiveWindow.Selection is TextSelection
                )
            {
                result = ((TextSelection)ApplicationObject.ActiveWindow.Selection).Text;
            }
            else if (ApplicationObject.ActiveWindow.Object != null
                && ApplicationObject.ActiveWindow.Object is OutputWindow)
            {
                var outputWindow = (OutputWindow)ApplicationObject.ActiveWindow.Object;

                if (outputWindow.ActivePane != null
                    && outputWindow.ActivePane.TextDocument != null
                    && outputWindow.ActivePane.TextDocument.Selection != null)
                {
                    result = outputWindow.ActivePane.TextDocument.Selection.Text;
                }
            }
            else if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

                if (items.Count == 1)
                {
                    var item = items[0];

                    if (item.Project != null)
                    {
                        result = item.Project.Name;
                    }

                    if (string.IsNullOrEmpty(result) && item.ProjectItem != null && item.ProjectItem.FileCount > 0)
                    {
                        result = Path.GetFileName(item.ProjectItem.FileNames[1]).Split('.').FirstOrDefault();
                    }

                    if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(item.Name))
                    {
                        result = item.Name;
                    }
                }
            }

            result = result ?? string.Empty;

            return result;
        }

        public List<SelectedFile> GetOpenedFileInCodeWindow(Func<string, bool> checkerFunction)
        {
            List<SelectedFile> selectedFiles = new List<SelectedFile>();

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
                )
            {
                if (!string.IsNullOrEmpty(ApplicationObject?.Solution?.FullName))
                {
                    string solutionPath = Path.GetDirectoryName(ApplicationObject.Solution.FullName);

                    string path = ApplicationObject.ActiveWindow.Document.FullName;

                    if (checkerFunction(path))
                    {
                        selectedFiles.Add(new SelectedFile(path, GetFriendlyPath(solutionPath, path)));
                    }
                }
            }

            return selectedFiles;
        }

        public EnvDTE.Document GetOpenedDocumentInCodeWindow(Func<string, bool> checkerFunction)
        {
            EnvDTE.Document result = null;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
                )
            {
                string solutionPath = Path.GetDirectoryName(ApplicationObject.Solution.FullName);

                string path = ApplicationObject.ActiveWindow.Document.FullName;

                if (checkerFunction(path))
                {
                    result = ApplicationObject.ActiveWindow.Document;
                }
            }

            return result;
        }

        public List<SelectedFile> GetOpenedDocuments(Func<string, bool> checkerFunction)
        {
            List<SelectedFile> selectedFiles = new List<SelectedFile>();

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
                )
            {
                if (!string.IsNullOrEmpty(ApplicationObject?.Solution?.FullName))
                {
                    string solutionPath = Path.GetDirectoryName(ApplicationObject.Solution.FullName);

                    foreach (var document in ApplicationObject.Documents.OfType<EnvDTE.Document>())
                    {
                        if (document.ProjectItem != null
                            &&
                            (
                                document.ProjectItem.IsOpen[EnvDTE.Constants.vsViewKindTextView]
                                || document.ProjectItem.IsOpen[EnvDTE.Constants.vsViewKindCode]
                            )
                        )
                        {
                            string path = document.FullName;

                            if (checkerFunction(path))
                            {
                                selectedFiles.Add(new SelectedFile(path, GetFriendlyPath(solutionPath, path)));
                            }
                        }
                    }
                }
            }

            return selectedFiles;
        }

        public List<SelectedFile> GetSelectedFilesInSolutionExplorer(Func<string, bool> checkerFunction, bool recursive)
        {
            List<SelectedFile> selectedFiles = new List<SelectedFile>();

            if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null
            )
            {
                if (!string.IsNullOrEmpty(ApplicationObject?.Solution?.FullName))
                {
                    var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

                    HashSet<string> hash = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    items.ForEach(item =>
                    {
                        if (item.ProjectItem != null)
                        {
                            string path = item.ProjectItem.FileNames[1];

                            if (!string.IsNullOrEmpty(path) && checkerFunction(path) && !hash.Contains(path))
                            {
                                hash.Add(path);
                            }

                            if (recursive)
                            {
                                FillListProjectItems(hash, item.ProjectItem.ProjectItems, checkerFunction);
                            }
                        }

                        if (recursive && item.Project != null)
                        {
                            FillListProjectItems(hash, item.Project.ProjectItems, checkerFunction);
                        }
                    });

                    string solutionPath = Path.GetDirectoryName(ApplicationObject.Solution.FullName);

                    hash.OrderBy(path => path).ToList().ForEach(
                        path => selectedFiles.Add(new SelectedFile(path, GetFriendlyPath(solutionPath, path)))
                    );
                }
            }

            return selectedFiles;
        }

        public SelectedItem GetSingleSelectedItemInSolutionExplorer(Func<string, bool> checkerFunction)
        {
            SelectedItem result = null;

            if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null
            )
            {
                if (!string.IsNullOrEmpty(ApplicationObject?.Solution?.FullName))
                {
                    var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

                    var filtered = items.Where(a =>
                    {
                        try
                        {
                            string file = a.Name.ToLower();

                            return checkerFunction(file);
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(ex);

                            return false;
                        }
                    });

                    if (filtered.Count() == 1)
                    {
                        result = filtered.FirstOrDefault();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Файл в окне редактирования или выделенные файлы в Solution Explorer.
        /// </summary>
        /// <returns></returns>
        public List<SelectedFile> GetSelectedFilesAll(Func<string, bool> checkerFunction, bool recursive)
        {
            List<SelectedFile> selectedFiles = new List<SelectedFile>();

            selectedFiles.AddRange(GetOpenedFileInCodeWindow(checkerFunction));
            selectedFiles.AddRange(GetSelectedFilesInSolutionExplorer(checkerFunction, recursive));

            return selectedFiles;
        }

        private string GetFriendlyPath(string solutionPath, string filePath)
        {
            return filePath.Replace(solutionPath, string.Empty);
        }

        private SelectedItem GetSelectedProjectItem()
        {
            SelectedItem result = null;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

                if (items.Count == 1)
                {
                    result = items[0];
                }
            }

            return result;
        }

        public EnvDTE.Project GetSelectedProject()
        {
            if (ApplicationObject.ActiveWindow != null
                   && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                   && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                if (items.Count() == 1)
                {
                    var item = items.First();

                    if (item.Project != null)
                    {
                        return item.Project;
                    }
                }
            }

            return null;
        }

        private void FillListProjectItems(HashSet<string> hash, ProjectItems projectItems, Func<string, bool> checkerFunction)
        {
            if (projectItems != null)
            {
                foreach (ProjectItem projItem in projectItems)
                {
                    string path = projItem.FileNames[1];

                    if (checkerFunction(path))
                    {
                        if (!hash.Contains(path))
                        {
                            hash.Add(path);
                        }
                    }

                    FillListProjectItems(hash, projItem.ProjectItems, checkerFunction);

                    if (projItem.SubProject != null)
                    {
                        FillListProjectItems(hash, projItem.SubProject.ProjectItems, checkerFunction);
                    }
                }
            }
        }

        public List<SelectedFile> GetSelectedFilesFromListForPublish()
        {
            List<SelectedFile> selectedFiles = new List<SelectedFile>();

            if (!string.IsNullOrEmpty(ApplicationObject?.Solution?.FullName))
            {
                string solutionPath = Path.GetDirectoryName(ApplicationObject.Solution.FullName);

                selectedFiles.AddRange(_ListForPublish.OrderBy(s => s).Select(path => new SelectedFile(path, GetFriendlyPath(solutionPath, path))));
            }

            return selectedFiles;
        }

        #endregion Получение списков файлов.

        public bool HasCRMConnection(out ConnectionConfiguration connectionConfig)
        {
            bool result = false;

            connectionConfig = Model.ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData == null)
            {
                var crmConnection = new WindowCrmConnectionList(this, connectionConfig);

                crmConnection.ShowDialog();

                connectionConfig.Save();
            }

            result = connectionConfig.CurrentConnectionData != null;

            return result;
        }

        /// <summary>
        /// Вывод в Output VS
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public string WriteToOutput(string format, params object[] args)
        {
            StringBuilder str = new StringBuilder();

            string message = format;

            if (args != null && args.Any())
            {
                message = string.Format(format, args);
            }

            str.Append(message);

            _loggerOutput.Info(message);

            try
            {
                var outputWindowLocal = GetOutputWindow();

                if (outputWindowLocal != null)
                {
                    outputWindowLocal.OutputString(message);
                    outputWindowLocal.OutputString(Environment.NewLine);

                    if (outputWindowLocal.TextDocument != null
                        && outputWindowLocal.TextDocument.Selection != null)
                    {
                        outputWindowLocal.TextDocument.Selection.EndOfDocument(false);
                    }
                }
            }
            catch (Exception ex)
            {
                str.AppendLine().Append(GetExceptionDescription(ex));

                WriteExceptionToLog(ex);
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
            }

            return str.ToString();
        }

        public void WriteErrorToOutput(Exception ex)
        {
            var description = GetExceptionDescription(ex);

            Log.Error(ex, description);

            this.WriteToOutput(string.Empty);
            this.WriteToOutput(description);

            this.ActivateOutputWindow();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
        }

        public void WriteToOutputFilePathUri(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            {
                var uri = new Uri(filePath, UriKind.Absolute).AbsoluteUri;
                this.WriteToOutput("File Uri        : {0}", uri);
            }

            {
                var uri = new Uri(Path.GetDirectoryName(filePath), UriKind.Absolute).AbsoluteUri;
                this.WriteToOutput("File Folder Uri : {0}", uri);
            }
        }

        private void WriteExceptionDescriptionToOutput(string description)
        {

        }

        /// <summary>
        /// Вывод в Output Window VS пустые строки для разделения операций.
        /// </summary>
        public void WriteToOutputEmptyLines(CommonConfiguration config)
        {
            try
            {
                _loggerOutput.Info(string.Empty);
                _loggerOutput.Info(string.Empty);
                _loggerOutput.Info(string.Empty);
                _loggerOutput.Info(string.Empty);
                _loggerOutput.Info(string.Empty);

                var outputWindowLocal = GetOutputWindow();

                if (outputWindowLocal != null)
                {
                    if (config != null && config.ClearOutputWindowBeforeCRMOperation)
                    {
                        outputWindowLocal.Clear();
                    }
                    else
                    {
                        if (outputWindowLocal.TextDocument.EndPoint.Line > 1)
                        {
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(ex);
            }
        }

        public void ActivateOutputWindow()
        {
            var outputWindowLocal = GetOutputWindow();

            if (outputWindowLocal != null)
            {
                try
                {
                    outputWindowLocal.Activate();

                    if (outputWindowLocal.Collection != null
                        && outputWindowLocal.Collection.Parent != null
                        && outputWindowLocal.Collection.Parent.Parent != null
                        )
                    {
                        outputWindowLocal.Collection.Parent.Parent.SetFocus();
                        outputWindowLocal.Collection.Parent.Parent.Visible = true;
                    }

                    ApplicationObject?.MainWindow?.Activate();
                }
                catch (Exception ex)
                {
                    WriteExceptionToLog(ex);
                }
            }
        }

        public void ActivateVisualStudioWindow() => ApplicationObject?.MainWindow?.Activate();

        #region Методы для работы со списком на публикацию.

        public void AddToListForPublish(IEnumerable<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Any())
            {
                ActivateOutputWindow();

                WriteToOutput(string.Empty);

                foreach (SelectedFile selectedFile in selectedFiles)
                {
                    if (!string.IsNullOrEmpty(selectedFile.FilePath))
                    {
                        if (File.Exists(selectedFile.FilePath))
                        {
                            if (!_ListForPublish.Contains(selectedFile.FilePath))
                            {
                                _ListForPublish.Add(selectedFile.FilePath);

                                WriteToOutput("Added into Publish List: {0}", selectedFile.FriendlyFilePath);
                            }
                            else
                            {
                                WriteToOutput("File already in Publish List: {0}", selectedFile.FriendlyFilePath);
                            }
                        }
                        else
                        {
                            WriteToOutput("File not exists: {0}", selectedFile.FriendlyFilePath);
                        }
                    }
                }
            }
        }

        public void RemoveFromListForPublish(List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count > 0)
            {
                ActivateOutputWindow();

                WriteToOutput(string.Empty);

                foreach (SelectedFile selectedFile in selectedFiles)
                {
                    if (_ListForPublish.Contains(selectedFile.FilePath))
                    {
                        _ListForPublish.Remove(selectedFile.FilePath);

                        WriteToOutput("Removed from Publish List: {0}", selectedFile.FriendlyFilePath);
                    }
                    else
                    {
                        WriteToOutput("File not in Publish List: {0}", selectedFile.FriendlyFilePath);
                    }
                }
            }
        }

        public void ClearListForPublish()
        {
            _ListForPublish.Clear();

            ActivateOutputWindow();

            WriteToOutput(string.Empty);

            WriteToOutput("Publish List has cleaned.");
        }

        public void ShowListForPublish()
        {
            ActivateOutputWindow();

            WriteToOutput(string.Empty);

            if (_ListForPublish.Count > 0)
            {
                WriteToOutput("Publish List: {0}", _ListForPublish.Count.ToString());

                if (!string.IsNullOrEmpty(ApplicationObject?.Solution?.FullName))
                {
                    string solutionPath = Path.GetDirectoryName(ApplicationObject.Solution.FullName);

                    foreach (var path in _ListForPublish.OrderBy(s => s))
                    {
                        WriteToOutput(GetFriendlyPath(solutionPath, path));
                    }
                }
            }
            else
            {
                WriteToOutput("Publish List is empty.");
            }
        }

        #endregion Методы для работы со списком на публикацию.

        public void HandleFileCompareCommand(List<SelectedFile> selectedFiles, bool withDetails)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartComparing(selectedFiles, crmConfig, withDetails);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleUpdateContentWebResourcesAndPublishCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format("Continue create/update web resources and publish {0} files on\r\n{1}?", selectedFiles.Count, crmConfig.CurrentConnectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPropmPublishMessage = dialog.DoNotPromtPublishMessage;

                        commonConfig.Save();

                        canPublish = true;
                    }
                }

                if (canPublish)
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartUpdateContentAndPublish(selectedFiles, crmConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleAddingWebResourcesIntoSolutionCommand(List<SelectedFile> selectedFiles, bool withSelect, string solutionUniqueName)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartAddingWebResourcesIntoSolution(crmConfig.CurrentConnectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleAddingPluginAssemblyIntoSolutionByProjectCommand(EnvDTE.Project project, bool withSelect, string solutionUniqueName)
        {
            if (project == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartAddingPluginAssemblyIntoSolution(crmConfig.CurrentConnectionData, commonConfig, solutionUniqueName, project.Name, withSelect);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleAddingPluginAssemblyProcessingStepsByProjectCommand(EnvDTE.Project project, bool withSelect, string solutionUniqueName)
        {
            if (project == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartAddingPluginAssemblyProcessingStepsIntoSolution(crmConfig.CurrentConnectionData, commonConfig, solutionUniqueName, project.Name, withSelect);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleAddingPluginTypeProcessingStepsByProjectCommand(string pluginTypeName, bool withSelect, string solutionUniqueName)
        {
            if (string.IsNullOrEmpty(pluginTypeName))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartAddingPluginTypeProcessingStepsIntoSolution(crmConfig.CurrentConnectionData, commonConfig, solutionUniqueName, pluginTypeName, withSelect);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleAddingReportsIntoSolutionCommand(List<SelectedFile> selectedFiles, bool withSelect, string solutionUniqueName)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartAddingReportsIntoSolution(crmConfig.CurrentConnectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleComparingPluginAssemblyAndLocalAssemblyCommand(ConnectionData connectionData, EnvDTE.Project project)
        {
            if (project == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                var defaultFolder = GetOutputPath(project);

                try
                {
                    Controller.StartComparingPluginAssemblyAndLocalAssembly(connectionData, commonConfig, project.Name, defaultFolder);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExecutingFetchXml(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CrmDeveloperHelperPackage.Singleton?.ExecuteFetchXmlQueryAsync(selectedFile.FilePath, connectionData);
            }
        }

        public void HandleConvertingFetchXmlToJavaScriptCode(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CrmDeveloperHelperPackage.Singleton?.ExecuteConvertFetchXmlToJavaScriptCodeAsync(selectedFile.FilePath, connectionData);
            }
        }

        public string GetOutputPath(Project proj)
        {
            var prjPath = GetProjectPath(proj);
            if (string.IsNullOrWhiteSpace(prjPath))
            {
                return string.Empty;
            }

            Properties prop = null;

            string probKey = string.Empty;
            if (proj.ConfigurationManager.ActiveConfiguration.Properties == null)
            {
                if (TryGetPropertyByName(proj.Properties, "ActiveConfiguration", out _) == false)
                {
                    return string.Empty;
                }

                prop = proj.Properties.Item("ActiveConfiguration").Value as Properties;
                if (TryGetPropertyByName(prop, "PrimaryOutput", out _))
                {
                    probKey = "PrimaryOutput";
                }
            }
            else
            {
                prop = proj.ConfigurationManager.ActiveConfiguration.Properties;
                if (TryGetPropertyByName(prop, "OutputPath", out _))
                {
                    probKey = "OutputPath";
                }
            }

            if (TryGetPropertyByName(prop, probKey, out _) == false)
            {
                return string.Empty;
            }

            var filePath = prop.Item(probKey).Value.ToString();
            if (Path.IsPathRooted(filePath) == false)
            {
                filePath = Path.Combine(prjPath, filePath);
            }

            var attr = File.GetAttributes(filePath);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                return filePath;
            }
            else
            {
                return new FileInfo(filePath).Directory.FullName;
            }
        }

        public string GetProjectPath(Project proj)
        {
            // C# Project has the FullPath property
            if (TryGetPropertyByName(proj.Properties, "FullPath", out var propertyFullPath))
            {
                var filePath = propertyFullPath.Value.ToString();
                var fullPath = new FileInfo(filePath);
                return fullPath.Directory.FullName;
            }

            // C++ Project has the ProjectFile property
            if (TryGetPropertyByName(proj.Properties, "ProjectFile", out var propertyProjectFile))
            {
                var filePath = propertyProjectFile.Value.ToString();
                var fullPath = new FileInfo(filePath);
                return fullPath.Directory.FullName;
            }

            return string.Empty;
        }

        public static bool TryGetPropertyByName(Properties properties, string propertyName, out Property result)
        {
            result = null;

            if (properties != null)
            {
                foreach (Property item in properties)
                {
                    if (item != null && string.Equals(item.Name, propertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = item;

                        return true;
                    }
                }
            }

            return false;
        }

        public void HandleUpdateGlobalOptionSetsFile(List<SelectedFile> selectedFiles, bool withSelect)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartUpdatingFileWithGlobalOptionSets(crmConfig, crmConfig.CurrentConnectionData, commonConfig, selectedFiles, withSelect);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleUpdateEntityMetadataFile(List<SelectedFile> selectedFiles, bool selectEntity)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartUpdatingFileWithEntityMetadata(selectedFiles, crmConfig.CurrentConnectionData, commonConfig, selectEntity);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleUpdateProxyClasses()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsCSharpType, false);

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                string filePath = selectedFiles[0].FilePath;

                var form = new WindowCreateProxyClasses(commonConfig, crmConfig, filePath);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.UpdateProxyClasses(filePath, crmConfig, commonConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleReportDifferenceCommand(ConnectionData connectionData, bool isCustom)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartReportDifference(selectedFiles[0], isCustom, connectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleReportThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartReportThreeFileDifference(selectedFiles[0], connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleReportUpdateCommand()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartReportUpdate(selectedFiles[0], crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleReportDownloadCommand()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartDownloadCustomReport(crmConfig.CurrentConnectionData, commonConfig, selectedFile.FileName);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleOpenReportCommand(ConnectionData connectionData, ActionOpenComponent action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (action)
                    {
                        case ActionOpenComponent.OpenInWeb:
                            connectionData.OpenSolutionComponentInWeb(Entities.ComponentType.Report, objectId.Value, null, null);
                            return;

                        case ActionOpenComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, objectId.Value);
                            return;
                    }
                }

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOpeningReport(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleOpenLastSelectedSolution(string solutionUniqueName, ActionOpenComponent action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && !string.IsNullOrEmpty(solutionUniqueName) && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOpeningSolution(commonConfig, crmConfig.CurrentConnectionData, solutionUniqueName, action);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCreateLaskLinkReportCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCreatingLastLinkReport(selectedFile, crmConfig.CurrentConnectionData);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleFileClearLink(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartClearingLastLink(selectedFiles, crmConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleWebResourceDifferenceCommand(ConnectionData connectionData, bool isCustom)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartWebResourceDifference(selectedFiles[0], isCustom, connectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleWebResourceThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false);

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartWebResourceThreeFileDifference(selectedFiles[0], connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckFileEncodingCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckFileEncoding(selectedFiles);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleWebResourceDownloadCommand()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceType, false);

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartDownloadCustomWebResource(crmConfig.CurrentConnectionData, commonConfig, selectedFile.FileName);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleOpenWebResource(ConnectionData connectionData, ActionOpenComponent action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (action)
                    {
                        case ActionOpenComponent.OpenInWeb:
                            connectionData.OpenSolutionComponentInWeb(Entities.ComponentType.WebResource, objectId.Value, null, null);
                            return;

                        case ActionOpenComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, objectId.Value);
                            return;
                    }
                }

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOpeningWebResource(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleShowingWebResourcesDependentComponents(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                var form = new WindowSelectFolderForExport(commonConfig.FolderForExport, commonConfig.AfterCreateFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.AfterCreateFileAction = form.GetFileAction();

                    commonConfig.Save();

                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.ShowingWebResourcesDependentComponents(selectedFiles, crmConfig, crmConfig.CurrentConnectionData, commonConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleOpenFilesCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                if (inTextEditor && !File.Exists(commonConfig.TextEditorProgram))
                {
                    return;
                }

                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                Controller.StartOpeningFiles(selectedFiles, openFilesType, inTextEditor, crmConfig, commonConfig);
            }
        }

        public void HandleFileCompareListForPublishCommand(bool withDetails)
        {
            List<SelectedFile> selectedFiles = this.GetSelectedFilesFromListForPublish();

            if (selectedFiles.Count > 0)
            {
                this.ShowListForPublish();

                this.HandleFileCompareCommand(selectedFiles, withDetails);
            }
            else
            {
                this.WriteToOutput("Publish List is empty.");
                this.ActivateOutputWindow();
            }
        }

        public void HandleMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartMultiDifferenceFiles(selectedFiles, openFilesType, crmConfig, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void OpenConnectionList()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowCrmConnectionList(this, connectionConfig);

                    form.ShowDialog();

                    connectionConfig.Save();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public void TestCurrentConnection()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                QuickConnection.TestConnectAsync(crmConfig.CurrentConnectionData, this);
            }
        }

        public void OpenCommonConfiguration()
        {
            CommonConfiguration config = CommonConfiguration.Get();

            if (config != null)
            {
                var form = new WindowCommonConfiguration(config);

                form.ShowDialog();
            }
        }

        public string GetCurrentConnectionName()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData?.Name;
        }

        public string GetLastSolutionUniqueName()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData?.LastSelectedSolutionsUniqueName?.FirstOrDefault();
        }

        public void HandleCheckEntitiesNames()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, "Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartCheckEntitiesNames(crmConfig.CurrentConnectionData, commonConfig, dialog.GetText());
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleCheckEntitiesNamesAndShowDependentComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, "Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartCheckEntitiesNamesAndShowDependentComponents(crmConfig.CurrentConnectionData, commonConfig, dialog.GetText());
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleCheckMarkedAndShowDependentComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, "Select mark to delete", "Mark to delete");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartCheckMarkedToDeleteAndShowDependentComponents(crmConfig.CurrentConnectionData, commonConfig, dialog.GetText());
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleCheckEntitiesOwnership()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckEntitiesOwnership(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckGlobalOptionSetDuplicates()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckGlobalOptionSetDuplicates(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckComponentTypeEnum()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckComponentTypeEnum(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCreateAllDependencyNodesDescription()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCreateAllDependencyNodesDescription(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckManagedEntities()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckManagedEntities(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckPluginSteps()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckPluginSteps(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckPluginImages()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckPluginImages(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckPluginStepsRequiredComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckPluginStepsRequiredComponents(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckPluginImagesRequiredComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCheckPluginImagesRequiredComponents(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleFindByName()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, "Select Element Name", "Element Name");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartFindEntityElementsByName(crmConfig.CurrentConnectionData, commonConfig, dialog.GetText());
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleFindContainsString()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, "Select String for contain", "String for contain");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartFindEntityElementsContainsString(crmConfig.CurrentConnectionData, commonConfig, dialog.GetText());
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleFindEntityById()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, string.Format("Find Entity in {0} by Id", crmConfig.CurrentConnectionData.Name), "Entity Id");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartFindEntityById(crmConfig.CurrentConnectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleFindEntityByUniqueidentifier()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, string.Format("Find Entity in {0} by Uniqueidentifier", crmConfig.CurrentConnectionData.Name), "Uniqueidentifier");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartFindEntityByUniqueidentifier(crmConfig.CurrentConnectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandlePluginTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingPluginTree(crmConfig.CurrentConnectionData, commonConfig, selection);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleSdkMessageTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingSdkMessageTree(crmConfig.CurrentConnectionData, commonConfig, selection);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleSdkMessageRequestTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingSdkMessageRequestTree(crmConfig.CurrentConnectionData, commonConfig, selection);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandlePluginConfigurationCreate()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportPluginConfiguration(crmConfig.CurrentConnectionData, commonConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandlePluginConfigurationPluginAssemblyDescription()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            if (commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationAssemblyDescription(commonConfig, filePath);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandlePluginConfigurationPluginTypeDescription()
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationTypeDescription(commonConfig, filePath);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandlePluginConfigurationTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationTree(crmConfig.CurrentConnectionData, commonConfig, filePath);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandlePluginConfigurationComparerPluginAssembly()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ConnectionConfiguration crmConfig = Model.ConnectionConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            if (commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationComparer(commonConfig, filePath);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleOrganizationComparer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ConnectionConfiguration crmConfig = Model.ConnectionConfiguration.Get();

            if (crmConfig != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOrganizationComparer(crmConfig, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportEntityAttributesDependentComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportEntityAttributesDependentComponents(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportFileWithEntityMetadata()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCreatingFileWithEntityMetadata(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportFormEvents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                var form = new WindowExportFormEvents(commonConfig);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportingFormEvents(crmConfig.CurrentConnectionData, commonConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleExportGlobalOptionSets()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCreatingFileWithGlobalOptionSets(crmConfig, crmConfig.CurrentConnectionData, commonConfig, selection);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportOrganizationInformation()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportOrganizationInformation(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportPluginAssembly()
        {
            string selection = GetSelectedText();

            HandleExportPluginAssembly(selection);
        }

        public void HandleExportPluginAssembly(string selection)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportPluginAssembly(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportPluginTypeDescription()
        {
            string selection = GetSelectedText();

            HandleExportPluginTypeDescription(selection);
        }

        public void HandleExportPluginTypeDescription(string selection)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportPluginTypeDescription(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportReport()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartDownloadCustomReport(crmConfig.CurrentConnectionData, commonConfig, selection);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportRibbon()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportRibbonXml(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportSitemap()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportSitemapXml(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleOpenSolutionComponentExplorerWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOpenSolutionComponentExplorerWindow(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleOpenExportSolutionWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOpenExportSolutionWindow(crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportSystemForm()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportSystemFormXml(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportSystemSavedQuery()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportSystemSavedQueryXml(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportSystemSavedQueryVisualization()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportSystemSavedQueryVisualizationXml(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportWebResource()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartDownloadCustomWebResource(crmConfig.CurrentConnectionData, commonConfig, selection);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportWorkflows()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportWorkflow(selection, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format("Seach equal by text among {0} files, update them and publish on\r\n{1}?", selectedFiles.Count, crmConfig.CurrentConnectionData.GetDescription());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPropmPublishMessage = dialog.DoNotPromtPublishMessage;

                        commonConfig.Save();

                        canPublish = true;
                    }
                }

                if (canPublish)
                {
                    ActivateOutputWindow();
                    WriteToOutputEmptyLines(commonConfig);

                    try
                    {
                        Controller.StartUpdateContentAndPublishEqualByText(selectedFiles, crmConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleAddingIntoPublishListFilesByTypeCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartAddingIntoPublishListFilesByType(selectedFiles, openFilesType, crmConfig, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCheckOpenFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartOpenFilesWithouUTF8Encoding(selectedFiles);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCompareFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles, bool withDetails)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartComparingFilesWithWrongEncoding(selectedFiles, crmConfig, withDetails);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleCreateLaskLinkWebResourcesMultipleCommand(List<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && commonConfig != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartCreatingLastLinkWebResourceMultiple(selectedFiles, crmConfig.CurrentConnectionData);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportSolution()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            SelectedItem selectedItem = GetSelectedProjectItem();

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && selectedItem != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                try
                {
                    Controller.StartExportSolution(selectedItem, crmConfig.CurrentConnectionData, commonConfig);
                }
                catch (Exception xE)
                {
                    WriteErrorToOutput(xE);
                }
            }
        }

        public void HandleExportPluginConfigurationInfoFolder()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var selectedItem = GetSelectedProjectItem();

            if (crmConfig != null && crmConfig.CurrentConnectionData != null && selectedItem != null)
            {
                ActivateOutputWindow();
                WriteToOutputEmptyLines(commonConfig);

                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportPluginConfigurationIntoFolder(selectedItem, crmConfig.CurrentConnectionData, commonConfig);
                    }
                    catch (Exception xE)
                    {
                        WriteErrorToOutput(xE);
                    }
                }
            }
        }

        public void HandleOpenAdvancedFind(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                connectionData.OpenAdvancedFindInWeb();
            }
        }

        public void HandleOpenCrmInWeb(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                connectionData.OpenCrmInWeb();
            }
        }

        public OutputWindowPane GetOutputWindow()
        {
            OutputWindow outputWindow = ApplicationObject?.ToolWindows?.OutputWindow;

            if (outputWindow != null
                && outputWindow.OutputWindowPanes != null
                )
            {
                for (int i = 1; i <= outputWindow.OutputWindowPanes.Count; i++)
                {
                    var result = outputWindow.OutputWindowPanes.Item(i);

                    if (result != null)
                    {
                        if (string.Equals(result.Name, _outputWindowName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return result;
                        }
                    }
                }

                return outputWindow.OutputWindowPanes.Add(_outputWindowName);
            }

            return null;
        }

        public static string OpenFile(string filePath)
        {
            StringBuilder result = new StringBuilder();

            if (File.Exists(filePath))
            {
                result.AppendFormat("Opening file {0}", filePath);

                ProcessStartInfo info = new ProcessStartInfo();

                info.FileName = filePath;

                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Normal;

                try
                {
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

                    if (process != null)
                    {
                        if (!process.HasExited)
                        {
                            process.WaitForInputIdle();
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AppendLine().Append(GetExceptionDescription(ex));

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                }
            }

            return result.ToString();
        }

        public void SelectFileInFolder(string filePath)
        {
            if (File.Exists(filePath))
            {
                this.WriteToOutput("Selecting file in folder {0}", filePath);
                this.WriteToOutputFilePathUri(filePath);

                ProcessStartInfo info = new ProcessStartInfo();

                info.FileName = "explorer.exe";
                info.Arguments = @"/select, """ + filePath + "\"";

                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Normal;

                try
                {
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

                    if (process != null)
                    {
                        if (!process.HasExited)
                        {
                            process.WaitForInputIdle();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(ex);

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                }
            }
        }

        public void PerformAction(string filePath, CommonConfiguration commonConfig)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            if (commonConfig.AfterCreateFileAction == FileAction.OpenFileInTextEditor || commonConfig.AfterCreateFileAction == FileAction.OpenFileInVisualStudio)
            {
                OpenFile(filePath, commonConfig);
            }
            else if (commonConfig.AfterCreateFileAction == FileAction.SelectFileInFolder)
            {
                SelectFileInFolder(filePath);
            }
            else
            {
                this.WriteToOutput("No Action on file {0}", filePath);
                this.WriteToOutputFilePathUri(filePath);
            }
        }

        public void OpenFile(string filePath, CommonConfiguration commonConfig)
        {
            if (File.Exists(filePath))
            {
                if (commonConfig.AfterCreateFileAction == FileAction.OpenFileInTextEditor && File.Exists(commonConfig.TextEditorProgram))
                {
                    OpenFileInTextEditor(filePath, commonConfig);
                }
                else
                {
                    if (ApplicationObject != null)
                    {
                        this.WriteToOutput("Opening in Visual Studio file {0}", filePath);
                        this.WriteToOutputFilePathUri(filePath);

                        ApplicationObject.ItemOperations.OpenFile(filePath);
                        ApplicationObject.MainWindow.Activate();
                    }
                }
            }
        }

        public void OpenFileInVisualStudio(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (ApplicationObject != null)
                {
                    this.WriteToOutput("Opening in Visual Studio file {0}", filePath);
                    this.WriteToOutputFilePathUri(filePath);

                    ApplicationObject.ItemOperations.OpenFile(filePath);
                    ApplicationObject.MainWindow.Activate();
                }
            }
        }

        public void OpenFileInTextEditor(string filePath, CommonConfiguration commonConfig)
        {
            if (File.Exists(filePath) && File.Exists(commonConfig.TextEditorProgram))
            {
                this.WriteToOutput("Opening in Text Editor file {0}", filePath);
                this.WriteToOutputFilePathUri(filePath);

                ProcessStartInfo info = new ProcessStartInfo();

                info.FileName = string.Format("\"{0}\"", commonConfig.TextEditorProgram);
                info.Arguments = string.Format("\"{0}\"", filePath);

                info.UseShellExecute = false;
                info.WindowStyle = ProcessWindowStyle.Normal;

                try
                {
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

                    if (process != null)
                    {
                        if (!process.HasExited)
                        {
                            process.WaitForInputIdle();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(ex);
                }
            }
        }

        public void ProcessStartProgramComparer(CommonConfiguration commonConfig, string filePath1, string filePath2, string fileTitle1, string fileTitle2)
        {
            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this.WriteToOutput("Starting Difference Programm for files:");
                this.WriteToOutput(filePath1);
                this.WriteToOutputFilePathUri(filePath1);
                this.WriteToOutput(string.Empty);
                this.WriteToOutput(filePath2);
                this.WriteToOutputFilePathUri(filePath2);

                if (commonConfig.DifferenceProgramExists())
                {
                    ProcessStartInfo info = new ProcessStartInfo();

                    info.FileName = string.Format("\"{0}\"", commonConfig.CompareProgram);

                    StringBuilder arguments = new StringBuilder(commonConfig.CompareArgumentsFormat);

                    arguments = arguments.Replace("%f1", filePath1);
                    arguments = arguments.Replace("%f2", filePath2);

                    arguments = arguments.Replace("%ft1", fileTitle1);
                    arguments = arguments.Replace("%ft2", fileTitle2);

                    info.Arguments = arguments.ToString();

                    info.UseShellExecute = false;
                    info.WindowStyle = ProcessWindowStyle.Normal;

                    try
                    {
                        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

                        if (process != null)
                        {
                            this.ActivateVisualStudioWindow();

                            System.Threading.Thread.Sleep(timeDelay);

                            if (!process.HasExited)
                            {
                                process.WaitForInputIdle(timeDelay);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.WriteErrorToOutput(ex);
                    }
                }
                else
                {
                    bool diffExecuted = false;

                    var commandService = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(System.ComponentModel.Design.IMenuCommandService)) as Microsoft.VisualStudio.Shell.OleMenuCommandService;
                    if (commandService != null)
                    {
                        this.WriteToOutput("Cannot get OleMenuCommandService.");

                        //Tools.DiffFiles
                        var args = $"\"{filePath1}\" \"{filePath2}\" \"{fileTitle1}\" \"{fileTitle2}\"";
                        diffExecuted = commandService.GlobalInvoke(new System.ComponentModel.Design.CommandID(ToolsDiffCommandGuid, ToolsDiffCommandId), args);

                        if (diffExecuted)
                        {
                            this.ActivateVisualStudioWindow();
                        }
                    }

                    if (!diffExecuted)
                    {
                        this.WriteToOutput("Cannot execute Visual Studio Diff Program.");
                    }
                }
            }
            else
            {
                this.OpenFile(filePath1, commonConfig);

                this.OpenFile(filePath2, commonConfig);
            }
        }

        public void ProcessStartProgramComparerThreeWayFile(CommonConfiguration commonConfig, string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2)
        {
            if (!commonConfig.DifferenceThreeWayAvaliable())
            {
                this.WriteToOutput("There is no valid configuration for ThreeWay Difference.");
                return;
            }

            if (File.Exists(filePath1) && File.Exists(filePath2) && File.Exists(fileLocalPath))
            {
                this.WriteToOutput("Starting ThreeWay Difference Programm for files:");

                this.WriteToOutput(fileLocalPath);
                this.WriteToOutputFilePathUri(fileLocalPath);
                this.WriteToOutput(string.Empty);
                this.WriteToOutput(filePath1);
                this.WriteToOutputFilePathUri(filePath1);
                this.WriteToOutput(string.Empty);
                this.WriteToOutput(filePath2);
                this.WriteToOutputFilePathUri(filePath2);

                ProcessStartInfo info = new ProcessStartInfo();

                info.FileName = string.Format("\"{0}\"", commonConfig.CompareProgram);

                StringBuilder arguments = new StringBuilder(commonConfig.CompareArgumentsThreeWayFormat);

                arguments = arguments.Replace("%fl", fileLocalPath);
                arguments = arguments.Replace("%f1", filePath1);
                arguments = arguments.Replace("%f2", filePath2);

                arguments = arguments.Replace("%flt", fileLocalTitle);
                arguments = arguments.Replace("%ft1", fileTitle1);
                arguments = arguments.Replace("%ft2", fileTitle2);

                info.Arguments = arguments.ToString();

                info.UseShellExecute = false;
                info.WindowStyle = ProcessWindowStyle.Normal;

                try
                {
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

                    if (process != null)
                    {
                        this.ActivateVisualStudioWindow();

                        System.Threading.Thread.Sleep(timeDelay);

                        if (!process.HasExited)
                        {
                            process.WaitForInputIdle(timeDelay);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this.OpenFile(fileLocalPath, commonConfig);

                this.OpenFile(filePath1, commonConfig);

                this.OpenFile(filePath2, commonConfig);
            }
        }
    }
}