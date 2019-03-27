using EnvDTE;
using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Properties;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper : IWriteToOutputAndPublishList
    {
        private const string _loggerOutputName = "OutputLogger";
        private const string _loggerErrors = "ErrorLogger";

        private const string _outputWindowName = "Crm Developer Helper";

        private const string _tabSpacer = "    ";

        private const int _timeDelay = 2000;

        private static readonly Guid ToolsDiffCommandGuid = new Guid("5D4C0442-C0A2-4BE8-9B4D-AB1C28450942");
        private const int ToolsDiffCommandId = 256;

        private static readonly ConcurrentDictionary<string, Logger> _loggersOutputCache = new ConcurrentDictionary<string, Logger>(StringComparer.InvariantCultureIgnoreCase);

        private readonly ConcurrentDictionary<string, bool> _firstLineInOutput = new ConcurrentDictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly Logger Log;

        private static readonly string _logLayout = new string('-', 150) + "${newline}${newline}${newline}${newline}${newline}${newline}${longdate}|${level}${newline}${message}${newline}${newline}${exception}${newline}${newline}${stacktrace:format=Raw:topFrames=10}${newline}" + new string('-', 150);

        private readonly HashSet<string> _ListForPublish = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private static ConditionalWeakTable<Exception, object> _logExceptions = new ConditionalWeakTable<Exception, object>();

        private static ConditionalWeakTable<Exception, object> _outputExceptions = new ConditionalWeakTable<Exception, object>();

        static DTEHelper()
        {
            LogManager.Configuration = new LoggingConfiguration();

            FileTarget targetGenFile = new FileTarget()
            {
                Name = _loggerErrors + "Target",
                LineEnding = LineEndingMode.CRLF,
                Encoding = Encoding.UTF8,
                WriteBom = true,
                CreateDirs = true,
                FileName = Path.Combine(FileOperations.GetLogsFilePath(), @"Log ${date:format=yyyy-MM-dd}.log"),
                Layout = _logLayout,
            };

            LogManager.Configuration.AddTarget(targetGenFile);
            LogManager.Configuration.AddRuleForAllLevels(targetGenFile, _loggerErrors);

            LogManager.ReconfigExistingLoggers();

            Log = LogManager.GetLogger(_loggerErrors);
        }

        public string WriteToOutput(ConnectionData connectionData, string format, params object[] args)
        {
            StringBuilder str = new StringBuilder();

            string message = format;

            if (args != null && args.Any())
            {
                message = string.Format(format, args);
            }

            str.Append(message);

            var loggerOutput = GetOutputLogger(connectionData);
            loggerOutput.Info(message);

            try
            {
                var outputWindowLocal = GetOutputWindow(connectionData);

                if (outputWindowLocal != null)
                {
                    bool firstWriteToOutput = true;

                    if (_firstLineInOutput.TryGetValue(outputWindowLocal.Name, out bool temp))
                    {
                        firstWriteToOutput = temp;
                    }

                    if (!(string.IsNullOrEmpty(message) && firstWriteToOutput))
                    {
                        outputWindowLocal.OutputString(message);
                        outputWindowLocal.OutputString(Environment.NewLine);

                        _firstLineInOutput[outputWindowLocal.Name] = false;

                        if (outputWindowLocal.TextDocument != null
                            && outputWindowLocal.TextDocument.Selection != null)
                        {
                            outputWindowLocal.TextDocument.Selection.EndOfDocument(false);
                        }

                        outputWindowLocal.Activate();
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

        public string WriteToOutputStartOperation(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Any())
            {
                message = string.Format(format, args);
            }

            this.WriteToOutput(connectionData, string.Empty);

            return this.WriteToOutput(connectionData, OutputStrings.StartOperationFormat2
                , message
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                );
        }

        public string WriteToOutputEndOperation(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Any())
            {
                message = string.Format(format, args);
            }

            var result = this.WriteToOutput(connectionData, OutputStrings.EndOperationFormat2
                , message
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                );

            this.WriteToOutput(connectionData, string.Empty);

            return result;
        }

        public void WriteErrorToOutput(ConnectionData connectionData, Exception ex, string message = null, params object[] args)
        {
            var description = GetExceptionDescription(ex);

            if (!string.IsNullOrEmpty(message) && args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            if (!_logExceptions.TryGetValue(ex, out _))
            {
                _logExceptions.Add(ex, new object());

                Log.Info(new string('-', 150));

                if (!string.IsNullOrEmpty(message))
                {
                    Log.Info(message);
                }

                Log.Error(ex, description);
                Log.Info(new string('-', 150));
            }

            if (!_outputExceptions.TryGetValue(ex, out _))
            {
                _outputExceptions.Add(ex, new object());

                this.WriteToOutput(connectionData, string.Empty);

                this.WriteToOutput(connectionData, new string('-', 150));

                if (!string.IsNullOrEmpty(message))
                {
                    Log.Info(message);
                }

                this.WriteToOutput(connectionData, description);
                this.WriteToOutput(connectionData, new string('-', 150));

                this.ActivateOutputWindow(connectionData);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
            }
        }

        public static void WriteExceptionToOutput(ConnectionData connectionData, Exception ex, string message = null, params object[] args)
        {
            if (Singleton != null)
            {
                Singleton.WriteErrorToOutput(connectionData, ex, message, args);
            }
            else
            {
                WriteExceptionToLog(ex, message, args);
            }
        }

        public static void WriteExceptionToLog(Exception ex, string message = null, params object[] args)
        {
            if (!_logExceptions.TryGetValue(ex, out _))
            {
                _logExceptions.Add(ex, new object());

                var description = GetExceptionDescription(ex);

                if (!string.IsNullOrEmpty(message))
                {
                    if (args != null && args.Length > 0)
                    {
                        message = string.Format(message, args);
                    }

                    Log.Error(message);
                }

                Log.Error(ex, description);
            }
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

        /// <summary>
        /// Вывод в Output Window VS пустые строки для разделения операций.
        /// </summary>
        private void WriteToOutputEmptyLines(ConnectionData connectionData, CommonConfiguration config)
        {
            try
            {
                var loggerOutput = GetOutputLogger(connectionData);

                loggerOutput.Info(string.Empty);
                loggerOutput.Info(string.Empty);
                loggerOutput.Info(string.Empty);
                loggerOutput.Info(string.Empty);
                loggerOutput.Info(string.Empty);

                var outputWindowLocal = GetOutputWindow(connectionData);

                if (outputWindowLocal != null)
                {
                    if (config != null && config.ClearOutputWindowBeforeCRMOperation)
                    {
                        outputWindowLocal.Clear();

                        _firstLineInOutput[outputWindowLocal.Name] = true;
                    }
                    else
                    {
                        bool firstWriteToOutput = true;

                        if (_firstLineInOutput.TryGetValue(outputWindowLocal.Name, out bool temp))
                        {
                            firstWriteToOutput = temp;
                        }

                        if (!firstWriteToOutput)
                        {
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);
                            outputWindowLocal.OutputString(Environment.NewLine);

                            _firstLineInOutput[outputWindowLocal.Name] = false;
                        }
                    }

                    outputWindowLocal.Activate();
                }
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(connectionData, ex);
            }
        }

        public void ActivateOutputWindow(ConnectionData connectionData)
        {
            var outputWindowLocal = GetOutputWindow(connectionData);

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

        public void ActivateVisualStudioWindow()
        {
            ApplicationObject?.MainWindow?.Activate();
        }

        #region Методы для работы со списком на публикацию.

        public void AddToListForPublish(IEnumerable<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            WriteToOutput(null, string.Empty);

            FormatTextTableHandler tableAlreadyInPublishList = new FormatTextTableHandler();
            tableAlreadyInPublishList.SetHeader("FileName", "FriendlyFilePath");

            FormatTextTableHandler tableAddedInPublishList = new FormatTextTableHandler();
            tableAddedInPublishList.SetHeader("FileName", "FriendlyFilePath");

            foreach (SelectedFile selectedFile in selectedFiles.OrderBy(f => f.FriendlyFilePath).ThenBy(f => f.FileName))
            {
                if (!string.IsNullOrEmpty(selectedFile.FilePath))
                {
                    if (File.Exists(selectedFile.FilePath))
                    {
                        if (!_ListForPublish.Contains(selectedFile.FilePath))
                        {
                            _ListForPublish.Add(selectedFile.FilePath);

                            tableAddedInPublishList.AddLine(selectedFile.FileName, selectedFile.FriendlyFilePath);
                        }
                        else
                        {
                            tableAlreadyInPublishList.AddLine(selectedFile.FileName, selectedFile.FriendlyFilePath);
                        }
                    }
                    else
                    {
                        WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FriendlyFilePath);
                    }
                }
            }

            ActivateOutputWindow(null);

            if (tableAlreadyInPublishList.Count > 0)
            {
                WriteToOutput(null, Properties.OutputStrings.FilesAlreadyInPublishListFormat1, tableAlreadyInPublishList.Count);

                tableAlreadyInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(null, _tabSpacer + s));
            }

            if (tableAddedInPublishList.Count > 0)
            {
                WriteToOutput(null, Properties.OutputStrings.AddedInPublishListFormat1, tableAddedInPublishList.Count);

                tableAddedInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(null, _tabSpacer + s));
            }
        }

        public void RemoveFromListForPublish(List<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            WriteToOutput(null, string.Empty);

            FormatTextTableHandler tableNotInPublishList = new FormatTextTableHandler();
            tableNotInPublishList.SetHeader("FileName", "FriendlyFilePath");

            FormatTextTableHandler tableRemovedFromPublishList = new FormatTextTableHandler();
            tableRemovedFromPublishList.SetHeader("FileName", "FriendlyFilePath");

            foreach (SelectedFile selectedFile in selectedFiles.OrderBy(f => f.FriendlyFilePath).ThenBy(f => f.FileName))
            {
                if (_ListForPublish.Contains(selectedFile.FilePath))
                {
                    _ListForPublish.Remove(selectedFile.FilePath);

                    tableRemovedFromPublishList.AddLine(selectedFile.FileName, selectedFile.FriendlyFilePath);
                }
                else
                {
                    tableNotInPublishList.AddLine(selectedFile.FileName, selectedFile.FriendlyFilePath);
                }
            }

            ActivateOutputWindow(null);

            if (tableNotInPublishList.Count > 0)
            {
                WriteToOutput(null, Properties.OutputStrings.FilesNotInPublishListFormat1, tableNotInPublishList.Count);

                tableNotInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(null, _tabSpacer + s));
            }

            if (tableRemovedFromPublishList.Count > 0)
            {
                WriteToOutput(null, Properties.OutputStrings.RemovedFromPublishListFormat1, tableRemovedFromPublishList.Count);

                tableRemovedFromPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(null, _tabSpacer + s));
            }
        }

        public void ClearListForPublish(ConnectionData connectionData)
        {
            _ListForPublish.Clear();

            ActivateOutputWindow(connectionData);

            WriteToOutput(connectionData, string.Empty);

            WriteToOutput(connectionData, "Publish List has cleaned.");
        }

        public void ShowListForPublish(ConnectionData connectionData)
        {
            ActivateOutputWindow(connectionData);

            WriteToOutput(connectionData, string.Empty);

            if (_ListForPublish.Count > 0)
            {
                WriteToOutput(connectionData, "Publish List: {0}", _ListForPublish.Count.ToString());

                foreach (var path in _ListForPublish.OrderBy(s => s))
                {
                    WriteToOutput(connectionData, GetFriendlyPath(path));
                }
            }
            else
            {
                WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
            }
        }

        #endregion Методы для работы со списком на публикацию.

        private Logger GetOutputLogger(ConnectionData connectionData)
        {
            string loggerName = _loggerOutputName;
            string suffix = string.Empty;

            if (connectionData != null)
            {
                string connectionName = !string.IsNullOrEmpty(connectionData.Name) ? connectionData.Name : connectionData.ConnectionId.ToString();

                loggerName += connectionName;

                suffix = $".{connectionName}";
            }

            if (_loggersOutputCache.ContainsKey(loggerName))
            {
                return _loggersOutputCache[loggerName];
            }

            FileTarget targetGenFile = new FileTarget()
            {
                Name = loggerName + "Target",
                LineEnding = LineEndingMode.CRLF,
                Encoding = Encoding.UTF8,
                WriteBom = true,
                CreateDirs = true,
                FileName = Path.Combine(FileOperations.GetOutputFilePath(), "Output" + suffix + @" ${date:format=yyyy-MM-dd}.log"),
                Layout = "${message}"
            };

            LogManager.Configuration.AddTarget(targetGenFile);
            LogManager.Configuration.AddRuleForAllLevels(targetGenFile, loggerName);

            LogManager.ReconfigExistingLoggers();

            var loggerOutput = LogManager.GetLogger(loggerName);

            _loggersOutputCache.TryAdd(loggerName, loggerOutput);

            return loggerOutput;
        }

        private OutputWindowPane GetOutputWindow(ConnectionData connectionData)
        {
            try
            {
                string outputWindowName = _outputWindowName;

                if (connectionData != null)
                {
                    string connectionName = !string.IsNullOrEmpty(connectionData.Name) ? connectionData.Name : connectionData.ConnectionId.ToString();

                    outputWindowName += $" - {connectionName}";
                }

                OutputWindow outputWindow = ApplicationObject?.ToolWindows?.OutputWindow;

                if (outputWindow != null
                    && outputWindow.OutputWindowPanes != null
                    )
                {
                    for (int i = 1; i <= outputWindow.OutputWindowPanes.Count; i++)
                    {
                        var pane = outputWindow.OutputWindowPanes.Item(i);

                        if (pane != null)
                        {
                            if (string.Equals(pane.Name, outputWindowName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return pane;
                            }
                        }
                    }

                    return outputWindow.OutputWindowPanes.Add(outputWindowName);
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToLog(ex);
            }

            return null;
        }

        public void WriteToOutputFilePathUri(ConnectionData connectionData, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "File Uri                   :    {0}", uriFile);
            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Open File in Visual Studio :    {0}", uriFile.Replace("file:", $"{Intellisense.UrlCommandFilter.PrefixOpenInVisualStudio}:"));
            if (File.Exists(commonConfig.TextEditorProgram))
            {
                this.WriteToOutput(connectionData, string.Empty);
                this.WriteToOutput(connectionData, "Open File in TextEditor    :    {0}", uriFile.Replace("file:", $"{Intellisense.UrlCommandFilter.PrefixOpenInTextEditor}:"));
            }
            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Select File in Folder      :    {0}", uriFile.Replace("file:", $"{Intellisense.UrlCommandFilter.PrefixSelectFileInFolder}:"));
            this.WriteToOutput(connectionData, string.Empty);
        }

        public void WriteToOutputFilePathUriToOpenInExcel(ConnectionData connectionData, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

            this.WriteToOutput(connectionData, "Open File in Excel         :    {0}", uriFile.Replace("file:", $"{Intellisense.UrlCommandFilter.PrefixOpenInExcel}:"));
            this.WriteToOutput(connectionData, string.Empty);
        }

        public void WriteToOutputSolutionUri(ConnectionData connectionData, string solutionUniqueName, Guid solutionId)
        {
            if (string.IsNullOrEmpty(solutionUniqueName))
            {
                return;
            }

            string solutionUrl = connectionData.GetSolutionUrl(solutionId);

            string urlOpenSolution = string.Format("{0}:///crm.com?ConnectionId={1}&SolutionUniqueName={2}"
                , UrlCommandFilter.PrefixOpenSolution
                , connectionData.ConnectionId.ToString()
                , HttpUtility.UrlEncode(solutionUniqueName)
                );

            string urlOpenSolutionList = string.Format("{0}:///crm.com?ConnectionId={1}"
                , UrlCommandFilter.PrefixOpenSolutionList
                , connectionData.ConnectionId.ToString()
                );


            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Selected Solution            : {0}", solutionUniqueName);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Open Solution List in Web    : {0}", connectionData.GetOpenCrmWebSiteUrl(OpenCrmWebSiteType.Solutions));

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Open Solution in Web         : {0}", solutionUrl);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Open Solution Explorer       : {0}", urlOpenSolutionList);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Open Solution in Windows     : {0}", urlOpenSolution);

            this.WriteToOutput(connectionData, string.Empty);
        }

        public void SelectFileInFolder(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            this.WriteToOutput(connectionData, "Selecting file in folder {0}", filePath);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = @"/select, """ + filePath + "\"",

                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
            };

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
                this.WriteErrorToOutput(connectionData, ex);
            }
        }

        public void OpenFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            this.WriteToOutput(null, "Opening folder {0}", folderPath);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = string.Format("\"{0}\"", folderPath),

                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
            };

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
                this.WriteErrorToOutput(null, ex);
            }
        }

        public void PerformAction(ConnectionData connectionData, string filePath, bool hideFilePathUri = false)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            if (!hideFilePathUri)
            {
                this.WriteToOutput(connectionData, string.Empty);
                this.WriteToOutput(connectionData, string.Empty);

                this.WriteToOutputFilePathUri(connectionData, filePath);
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            FileAction fileAction = commonConfig.GetFileActionByExtension(Path.GetExtension(filePath));

            if (fileAction == FileAction.OpenFileInTextEditor || fileAction == FileAction.OpenFileInVisualStudio)
            {
                OpenFile(connectionData, filePath);
            }
            else if (fileAction == FileAction.SelectFileInFolder)
            {
                SelectFileInFolder(connectionData, filePath);
            }
        }

        public void OpenFile(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            FileAction fileAction = commonConfig.GetFileActionByExtension(Path.GetExtension(filePath));

            if (fileAction == FileAction.OpenFileInTextEditor && File.Exists(commonConfig.TextEditorProgram))
            {
                OpenFileInTextEditor(connectionData, filePath);
            }
            else
            {
                OpenFileInVisualStudio(connectionData, filePath);
            }
        }

        public void OpenFileInVisualStudio(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath)
                || ApplicationObject == null
            )
            {
                return;
            }

            this.WriteToOutput(connectionData, "Opening in Visual Studio file {0}", filePath);

            ApplicationObject.ItemOperations.OpenFile(filePath);
            ApplicationObject.MainWindow.Activate();
        }

        public void OpenFileInVisualStudioRelativePath(ConnectionData connectionData, string filePath)
        {
            if (ApplicationObject == null || string.IsNullOrEmpty(filePath))
            {
                return;
            }

            filePath = filePath.Replace("/", "\\").Trim('\\', '/');

            string solutionPath = ApplicationObject?.Solution?.FullName;

            if (!string.IsNullOrEmpty(solutionPath))
            {
                var pathFolder = Path.GetDirectoryName(solutionPath).TrimEnd('\\', '/');

                if (!filePath.StartsWith(pathFolder))
                {
                    filePath = Path.Combine(pathFolder, filePath);
                }
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            this.WriteToOutput(connectionData, "Opening in Visual Studio file {0}", filePath);

            ApplicationObject.ItemOperations.OpenFile(filePath);
            ApplicationObject.MainWindow.Activate();
        }

        public void ShowDifference(Uri uri)
        {
            if (!File.Exists(uri.LocalPath))
            {
                return;
            }

            ConnectionData connectionData = null;

            string fieldName = string.Empty;
            string fieldTitle = string.Empty;

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var queryDictionary = HttpUtility.ParseQueryString(uri.Query);

                if (queryDictionary.AllKeys.Contains("ConnectionId", StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary["ConnectionId"])
                    )
                {
                    var idStr = queryDictionary["ConnectionId"];

                    if (Guid.TryParse(idStr, out var tempGuid))
                    {
                        var connectionConfig = Model.ConnectionConfiguration.Get();

                        connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == tempGuid);
                    }
                }

                if (queryDictionary.AllKeys.Contains("fieldName", StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary["fieldName"])
                    )
                {
                    fieldName = queryDictionary["fieldName"];
                }

                if (queryDictionary.AllKeys.Contains("fieldTitle", StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary["fieldTitle"])
                    )
                {
                    fieldTitle = queryDictionary["fieldTitle"];
                }
            }

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            SelectedFile selectedFile = new SelectedFile(uri.LocalPath, GetFriendlyPath(uri.LocalPath));

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    if (FileOperations.SupportsWebResourceTextType(uri.LocalPath))
                    {
                        Controller.StartWebResourceDifference(selectedFile, false, connectionData, commonConfig);
                    }
                    else if (FileOperations.SupportsReportType(uri.LocalPath))
                    {
                        Controller.StartReportDifference(selectedFile, fieldName, fieldTitle, false, connectionData, commonConfig);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void OpenSolution(Uri uri)
        {
            ConnectionData connectionData = null;
            string solutionUniqueName = null;

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var queryDictionary = HttpUtility.ParseQueryString(uri.Query);

                if (!string.IsNullOrEmpty(queryDictionary["ConnectionId"]))
                {
                    var idStr = queryDictionary["ConnectionId"];

                    if (Guid.TryParse(idStr, out var tempGuid))
                    {
                        var connectionConfig = Model.ConnectionConfiguration.Get();

                        connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == tempGuid);
                    }
                }

                if (!string.IsNullOrEmpty(queryDictionary["SolutionUniqueName"]))
                {
                    solutionUniqueName = queryDictionary["SolutionUniqueName"];
                }
            }

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && !string.IsNullOrEmpty(solutionUniqueName))
            {
                this.HandleOpenLastSelectedSolution(connectionData, solutionUniqueName, ActionOpenComponent.OpenInWindow);
            }
        }

        public void OpenSolutionList(Uri uri)
        {
            ConnectionData connectionData = null;

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var queryDictionary = HttpUtility.ParseQueryString(uri.Query);

                if (!string.IsNullOrEmpty(queryDictionary["ConnectionId"]))
                {
                    var idStr = queryDictionary["ConnectionId"];

                    if (Guid.TryParse(idStr, out var tempGuid))
                    {
                        var connectionConfig = Model.ConnectionConfiguration.Get();

                        connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == tempGuid);
                    }
                }
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
                this.HandleOpenSolutionExplorerWindow(connectionData);
            }
        }

        public void OpenFileInTextEditor(ConnectionData connectionData, string filePath)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!File.Exists(filePath)
                || !File.Exists(commonConfig.TextEditorProgram)
            )
            {
                return;
            }

            this.WriteToOutput(connectionData, "Opening in Text Editor file {0}", filePath);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = string.Format("\"{0}\"", commonConfig.TextEditorProgram),
                Arguments = string.Format("\"{0}\"", filePath),

                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

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
                this.WriteErrorToOutput(connectionData, ex);
            }
        }

        public void OpenFileInExcel(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Opening in Excel file {0}", filePath);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "Excel.exe",
                Arguments = string.Format("\"{0}\"", filePath),

                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
            };

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
                this.WriteErrorToOutput(connectionData, ex);
            }
        }

        public void ProcessStartProgramComparer(string filePath1, string filePath2, string fileTitle1, string fileTitle2)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);

                this.WriteToOutput(null, "Starting Difference Programm for files:");
                this.WriteToOutput(null, filePath1);
                this.WriteToOutputFilePathUri(null, filePath1);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, filePath2);
                this.WriteToOutputFilePathUri(null, filePath2);

                if (commonConfig.DifferenceProgramExists())
                {
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        FileName = string.Format("\"{0}\"", commonConfig.CompareProgram)
                    };

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

                            System.Threading.Thread.Sleep(_timeDelay);

                            if (!process.HasExited)
                            {
                                process.WaitForInputIdle(_timeDelay);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.WriteErrorToOutput(null, ex);
                    }
                }
                else
                {
                    bool diffExecuted = false;

                    var commandService = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(System.ComponentModel.Design.IMenuCommandService)) as Microsoft.VisualStudio.Shell.OleMenuCommandService;
                    if (commandService != null)
                    {
                        //Tools.DiffFiles
                        var args = $"\"{filePath1}\" \"{filePath2}\" \"{fileTitle1}\" \"{fileTitle2}\"";
                        diffExecuted = commandService.GlobalInvoke(new System.ComponentModel.Design.CommandID(ToolsDiffCommandGuid, ToolsDiffCommandId), args);

                        if (diffExecuted)
                        {
                            this.ActivateVisualStudioWindow();
                        }
                    }
                    else
                    {
                        this.WriteToOutput(null, "Cannot get OleMenuCommandService.");
                    }

                    if (!diffExecuted)
                    {
                        this.WriteToOutput(null, "Cannot execute Visual Studio Diff Program.");
                    }
                }
            }
            else
            {
                this.OpenFile(null, filePath1);

                this.OpenFile(null, filePath2);
            }
        }

        public void ProcessStartProgramComparerThreeWayFile(string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!commonConfig.DifferenceThreeWayAvaliable())
            {
                this.WriteToOutput(null, "There is no valid configuration for ThreeWay Difference.");
                return;
            }

            if (File.Exists(filePath1) && File.Exists(filePath2) && File.Exists(fileLocalPath))
            {
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);

                this.WriteToOutput(null, "Starting ThreeWay Difference Programm for files:");

                this.WriteToOutput(null, fileLocalPath);
                this.WriteToOutputFilePathUri(null, fileLocalPath);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, filePath1);
                this.WriteToOutputFilePathUri(null, filePath1);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, filePath2);
                this.WriteToOutputFilePathUri(null, filePath2);

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = string.Format("\"{0}\"", commonConfig.CompareProgram)
                };

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

                        System.Threading.Thread.Sleep(_timeDelay);

                        if (!process.HasExited)
                        {
                            process.WaitForInputIdle(_timeDelay);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(null, ex);
                }
            }
            else
            {
                this.OpenFile(null, fileLocalPath);

                this.OpenFile(null, filePath1);

                this.OpenFile(null, filePath2);
            }
        }

        public Task<int> BuildProjectAsync(Project project)
        {
            if (project == null
                || string.IsNullOrEmpty(project.Name)
                || string.IsNullOrEmpty(project.UniqueName)
                || this.ApplicationObject.Solution == null
                || this.ApplicationObject.Solution.SolutionBuild == null
                || this.ApplicationObject.Solution.SolutionBuild.BuildState == vsBuildState.vsBuildStateInProgress
            )
            {
                return Task.FromResult<int>(-1);
            }

            var build = this.ApplicationObject.Solution.SolutionBuild;

            return Task.Run(() => BuildProject(build, project));
        }

        private int BuildProject(SolutionBuild build, Project project)
        {
            build.BuildProject(project.ConfigurationManager.ActiveConfiguration.ConfigurationName, project.UniqueName, WaitForBuildToFinish: true);

            var info = build.LastBuildInfo;

            return info;
        }
    }
}