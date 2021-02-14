using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.XsdModels;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper : IWriteToOutputAndPublishList
    {
        private const string _loggerOutputName = "OutputLogger";
        private const string _loggerErrors = "ErrorLogger";

        private const string _outputWindowPaneName = "Crm Developer Helper";

        private static readonly Guid _outputWindowPaneId = new Guid("6997AAB4-2C13-46E9-9E24-BE82DD295BC9");

        private const string _tabSpacer = "    ";

        private const int _timeDelay = 2000;

        private static readonly Guid Tools_DiffFilesCommandGuid = new Guid("5D4C0442-C0A2-4BE8-9B4D-AB1C28450942");
        private const int Tools_DiffFilesCommandId = 256;

        private System.ComponentModel.Design.CommandID ToolsDiffCommand = new System.ComponentModel.Design.CommandID(Tools_DiffFilesCommandGuid, Tools_DiffFilesCommandId);

        private static readonly ConcurrentDictionary<Guid, Logger> _loggersOutputCache = new ConcurrentDictionary<Guid, Logger>();

        private readonly ConcurrentDictionary<string, bool> _firstLineInOutput = new ConcurrentDictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly Logger Log;

        private static readonly string _logLayoutHeader = new string('-', 150) + "${newline}${newline}";
        private static readonly string _logLayoutFooter = "${newline}${newline}" + new string('-', 150);
        private static readonly string _logLayout = "$${longdate}|${level}${newline}${message}${newline}${newline}${exception}${newline}${newline}${stacktrace:format=Flat:topFrames=10000}${newline}${newline}${exception:format=toString,Data:exceptionDataSeparator=\r\n}${newline}${newline}";

        private static readonly string _splitter = new string('-', 150);

        private readonly HashSet<string> _ListForPublish = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly ConditionalWeakTable<Exception, object> _logExceptions = new ConditionalWeakTable<Exception, object>();

        private static readonly ConditionalWeakTable<Exception, object> _outputExceptions = new ConditionalWeakTable<Exception, object>();

        private const string _formatStringInQuotes = "\"{0}\"";

        private const string _formatEntityFetchXmlFileName = "{0}.xml";

        private const string _programExplorer = "explorer.exe";
        private const string _programExcel = "Excel.exe";

        static DTEHelper()
        {
            LogManager.Configuration = new LoggingConfiguration();

            var targetGenFile = new FileTarget()
            {
                Name = _loggerErrors + "Target",
                LineEnding = LineEndingMode.CRLF,
                Encoding = Encoding.UTF8,
                WriteBom = true,
                CreateDirs = true,
                FileName = Path.Combine(FileOperations.GetLogsFilePath(), @"Log ${date:format=yyyy-MM-dd}.log"),

                Layout = _logLayout,
                Header = _logLayoutHeader,
                Footer = _logLayoutFooter,
            };

            LogManager.Configuration.AddTarget(targetGenFile);
            LogManager.Configuration.AddRuleForAllLevels(targetGenFile, _loggerErrors);

            LogManager.ReconfigExistingLoggers();

            Log = LogManager.GetLogger(_loggerErrors);
        }

        public string WriteToOutput(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Any())
            {
                message = string.Format(format, args);
            }

            var loggerOutput = GetOutputLogger(connectionData?.ConnectionId);
            loggerOutput.Info(message);

            var task = WriteToOutputInternal(connectionData, message);

            return message;
        }

        private async System.Threading.Tasks.Task WriteToOutputInternal(ConnectionData connectionData, string message)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var outputWindowLocal = GetOutputWindow(connectionData);

                if (outputWindowLocal != null)
                {
                    bool firstWriteToOutput = true;

                    if (_firstLineInOutput.TryGetValue(outputWindowLocal.Guid, out bool temp))
                    {
                        firstWriteToOutput = temp;
                    }

                    if (!(string.IsNullOrEmpty(message) && firstWriteToOutput))
                    {
                        outputWindowLocal.OutputString(message);
                        outputWindowLocal.OutputString(Environment.NewLine);

                        _firstLineInOutput[outputWindowLocal.Guid] = false;

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
                WriteExceptionToLog(ex);
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
            }
        }

        public IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, Entity entity)
        {
            return WriteToOutputEntityInstance(connectionData, entity.LogicalName, entity.Id);
        }

        public IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, EntityReference entityRef)
        {
            WriteToOutputEntityInstance(connectionData, entityRef.LogicalName, entityRef.Id);

            if (!string.IsNullOrEmpty(entityRef.Name))
            {
                this.WriteToOutput(connectionData, Properties.OutputStrings.EntityReferenceNameFormat1, entityRef.Name);
            }

            return this;
        }

        public IWriteToOutput WriteToOutputEntityInstance(ConnectionData connectionData, string entityName, Guid id)
        {
            this.WriteToOutput(connectionData, Properties.OutputStrings.EntityReferenceLogicalNameFormat1, entityName);

            this.WriteToOutput(connectionData, Properties.OutputStrings.EntityReferenceIdFormat1, id);
            if (connectionData != null)
            {
                this.WriteToOutput(connectionData, Properties.OutputStrings.EntityReferenceUrlFormat1, connectionData.GetEntityInstanceUrl(entityName, id));
            }

            return this;
        }

        public string WriteToOutputStartOperation(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Any())
            {
                message = string.Format(format, args);
            }

            this.WriteToOutput(connectionData, string.Empty);

            return this.WriteToOutput(connectionData, Properties.OutputStrings.StartOperationFormat2
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

            var result = this.WriteToOutput(connectionData, Properties.OutputStrings.EndOperationFormat2
                , message
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
            );

            this.WriteToOutput(connectionData, string.Empty);

            return result;
        }

        public IWriteToOutput WriteErrorToOutput(ConnectionData connectionData, Exception ex, string message = null, params object[] args)
        {
            var description = GetExceptionDescription(ex);

            if (!string.IsNullOrEmpty(message) && args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            if (!_logExceptions.TryGetValue(ex, out _))
            {
                _logExceptions.Add(ex, new object());

                Log.Info(_splitter);

                if (!string.IsNullOrEmpty(message))
                {
                    Log.Info(message);
                }

                Log.Error(ex, description);
                Log.Info(_splitter);
            }

            if (!_outputExceptions.TryGetValue(ex, out _))
            {
                _outputExceptions.Add(ex, new object());

                this.WriteToOutput(connectionData, string.Empty);

                this.WriteToOutput(connectionData, _splitter);

                if (!string.IsNullOrEmpty(message))
                {
                    Log.Info(message);
                }

                this.WriteToOutput(connectionData, description);
                this.WriteToOutput(connectionData, _splitter);

                this.ActivateOutputWindow(connectionData);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
            }

            return this;
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

        public static void WriteToLog(string message = null, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (args != null && args.Length > 0)
                {
                    message = string.Format(message, args);
                }

                Log.Info(message);
            }
        }

        public static void WriteErrorToLog(string message = null, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (args != null && args.Length > 0)
                {
                    message = string.Format(message, args);
                }

                Log.Error(message);
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

            if (ex is ReflectionTypeLoadException reflectionTypeLoadException)
            {
                if (reflectionTypeLoadException.Types != null && reflectionTypeLoadException.Types.Any())
                {
                    stringBuilder.AppendFormat("ReflectionTypeLoadException.Types : {0}", reflectionTypeLoadException.Types.Length).AppendLine();

                    foreach (var loadType in reflectionTypeLoadException.Types)
                    {
                        if (loadType != null)
                        {
                            stringBuilder.AppendLine(loadType.FullName);
                        }
                    }
                }

                if (reflectionTypeLoadException.LoaderExceptions != null)
                {
                    foreach (var inner in reflectionTypeLoadException.LoaderExceptions)
                    {
                        if (inner != null)
                        {
                            FillExceptionInformation(inner, stringBuilder);
                        }
                    }
                }
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
            stringBuilder.AppendFormat("{0}.ActivityId - {1}", prefix, fault.ActivityId).AppendLine();

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
            var task = WriteToOutputEmptyLinesInternal(connectionData, config);
        }

        private async System.Threading.Tasks.Task WriteToOutputEmptyLinesInternal(ConnectionData connectionData, CommonConfiguration config)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var loggerOutput = GetOutputLogger(connectionData?.ConnectionId);

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

                        _firstLineInOutput[outputWindowLocal.Guid] = true;
                    }
                    else
                    {
                        bool firstWriteToOutput = true;

                        if (_firstLineInOutput.TryGetValue(outputWindowLocal.Guid, out bool temp))
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

                            _firstLineInOutput[outputWindowLocal.Guid] = false;
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

        public IWriteToOutput ActivateOutputWindow(ConnectionData connectionData)
        {
            return ActivateOutputWindow(connectionData, null);
        }

        public IWriteToOutput ActivateOutputWindow(ConnectionData connectionData, System.Windows.Window window)
        {
            var task = ActivateOutputWindowInternal(connectionData);

            if (window != null)
            {
                window.Dispatcher.Invoke(() =>
                {
                    window.Focus();
                    window.Activate();
                });
            }

            return this;
        }

        private async System.Threading.Tasks.Task ActivateOutputWindowInternal(ConnectionData connectionData)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

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
                }
                catch (Exception ex)
                {
                    WriteExceptionToLog(ex);
                }
            }
        }

        public IWriteToOutput ActivateVisualStudioWindow()
        {
            ApplicationObject?.MainWindow?.Activate();

            return this;
        }

        #region Методы для работы со списком на публикацию.

        public IWriteToOutputAndPublishList AddToListForPublish(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return this;
            }

            if (connectionData == null)
            {
                var connectionConfig = Model.ConnectionConfiguration.Get();

                connectionData = connectionConfig.CurrentConnectionData;
            }

            WriteToOutput(connectionData, string.Empty);

            var tableAlreadyInPublishList = new FormatTextTableHandler(nameof(SelectedFile.FileName), nameof(SelectedFile.FriendlyFilePath), Properties.OutputStrings.HeaderOpenInVisualStudio, Properties.OutputStrings.HeaderOpenFileInTextEditor);
            var tableAddedInPublishList = new FormatTextTableHandler(nameof(SelectedFile.FileName), nameof(SelectedFile.FriendlyFilePath), Properties.OutputStrings.HeaderOpenInVisualStudio, Properties.OutputStrings.HeaderOpenFileInTextEditor);

            var commonConfig = CommonConfiguration.Get();

            foreach (var selectedFile in selectedFiles.OrderBy(f => f.FriendlyFilePath).ThenBy(f => f.FileName))
            {
                if (string.IsNullOrEmpty(selectedFile.FilePath))
                {
                    continue;
                }

                if (!File.Exists(selectedFile.FilePath))
                {
                    WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FriendlyFilePath);
                    continue;
                }

                string uriFileOpenInVisualStudio = UrlCommandFilter.GetUriOpenInVisualStudioByFilePath(selectedFile.FilePath);

                var lines = new List<string>() { selectedFile.FileName, selectedFile.FriendlyFilePath, uriFileOpenInVisualStudio };

                if (commonConfig.TextEditorProgramExists())
                {
                    string uriFileOpenInTextEditor = UrlCommandFilter.GetUriOpenInTextEditorByFilePath(selectedFile.FilePath);
                    lines.Add(uriFileOpenInTextEditor);
                }

                if (!_ListForPublish.Contains(selectedFile.FilePath))
                {
                    _ListForPublish.Add(selectedFile.FilePath);

                    tableAddedInPublishList.AddLine(lines);
                }
                else
                {
                    tableAlreadyInPublishList.AddLine(lines);
                }
            }

            ActivateOutputWindow(connectionData);

            if (tableAlreadyInPublishList.Count > 0)
            {
                WriteToOutput(connectionData, Properties.OutputStrings.FilesAlreadyInPublishListFormat1, tableAlreadyInPublishList.Count);

                tableAlreadyInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(connectionData, _tabSpacer + s));
            }

            if (tableAddedInPublishList.Count > 0)
            {
                WriteToOutput(connectionData, Properties.OutputStrings.AddedInPublishListFormat1, tableAddedInPublishList.Count);

                tableAddedInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(connectionData, _tabSpacer + s));
            }

            return this;
        }

        public IWriteToOutputAndPublishList RemoveFromListForPublish(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles)
        {
            if (connectionData == null)
            {
                var connectionConfig = Model.ConnectionConfiguration.Get();

                connectionData = connectionConfig.CurrentConnectionData;
            }

            if (!selectedFiles.Any())
            {
                return this;
            }

            WriteToOutput(connectionData, string.Empty);

            var tableNotInPublishList = new FormatTextTableHandler(nameof(SelectedFile.FileName), nameof(SelectedFile.FriendlyFilePath), Properties.OutputStrings.HeaderOpenInVisualStudio, Properties.OutputStrings.HeaderOpenFileInTextEditor);

            var tableRemovedFromPublishList = new FormatTextTableHandler(nameof(SelectedFile.FileName), nameof(SelectedFile.FriendlyFilePath), Properties.OutputStrings.HeaderOpenInVisualStudio, Properties.OutputStrings.HeaderOpenFileInTextEditor);

            var commonConfig = CommonConfiguration.Get();

            foreach (var selectedFile in selectedFiles.OrderBy(f => f.FriendlyFilePath).ThenBy(f => f.FileName))
            {
                string uriFileOpenInVisualStudio = UrlCommandFilter.GetUriOpenInVisualStudioByFilePath(selectedFile.FilePath);

                var lines = new List<string>() { selectedFile.FileName, selectedFile.FriendlyFilePath, uriFileOpenInVisualStudio };

                if (commonConfig.TextEditorProgramExists())
                {
                    string uriFileOpenInTextEditor = UrlCommandFilter.GetUriOpenInTextEditorByFilePath(selectedFile.FilePath);
                    lines.Add(uriFileOpenInTextEditor);
                }

                if (_ListForPublish.Contains(selectedFile.FilePath))
                {
                    _ListForPublish.Remove(selectedFile.FilePath);

                    tableRemovedFromPublishList.AddLine(lines);
                }
                else
                {
                    tableNotInPublishList.AddLine(lines);
                }
            }

            ActivateOutputWindow(connectionData);

            if (tableNotInPublishList.Count > 0)
            {
                WriteToOutput(connectionData, Properties.OutputStrings.FilesNotInPublishListFormat1, tableNotInPublishList.Count);

                tableNotInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(connectionData, _tabSpacer + s));
            }

            if (tableRemovedFromPublishList.Count > 0)
            {
                WriteToOutput(connectionData, Properties.OutputStrings.RemovedFromPublishListFormat1, tableRemovedFromPublishList.Count);

                tableRemovedFromPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(connectionData, _tabSpacer + s));
            }

            return this;
        }

        public IWriteToOutput ClearListForPublish(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                var connectionConfig = Model.ConnectionConfiguration.Get();

                connectionData = connectionConfig.CurrentConnectionData;
            }

            _ListForPublish.Clear();

            ActivateOutputWindow(connectionData);

            WriteToOutput(connectionData, string.Empty);

            WriteToOutput(connectionData, Properties.OutputStrings.PublishListCleared);

            return this;
        }

        public IWriteToOutput ShowListForPublish(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                var connectionConfig = Model.ConnectionConfiguration.Get();

                connectionData = connectionConfig.CurrentConnectionData;
            }

            ActivateOutputWindow(connectionData);

            WriteToOutput(connectionData, string.Empty);

            if (!_ListForPublish.Any())
            {
                WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                return this;
            }

            var commonConfig = CommonConfiguration.Get();

            var tableInPublishList = new FormatTextTableHandler(nameof(SelectedFile.FileName), nameof(SelectedFile.FriendlyFilePath), Properties.OutputStrings.HeaderOpenInVisualStudio, Properties.OutputStrings.HeaderOpenFileInTextEditor);

            string solutionDirectoryPath = GetSolutionDirectory();

            foreach (var filePath in _ListForPublish.OrderBy(s => s))
            {
                string uriFileOpenInVisualStudio = UrlCommandFilter.GetUriOpenInVisualStudioByFilePath(filePath);

                var lines = new List<string>() { Path.GetFileName(filePath), SelectedFile.GetFriendlyPath(filePath, solutionDirectoryPath), uriFileOpenInVisualStudio };

                if (commonConfig.TextEditorProgramExists())
                {
                    string uriFileOpenInTextEditor = UrlCommandFilter.GetUriOpenInTextEditorByFilePath(filePath);
                    lines.Add(uriFileOpenInTextEditor);
                }

                tableInPublishList.AddLine(lines);
            }

            WriteToOutput(connectionData, Properties.OutputStrings.PublishListContentCountFormat1, _ListForPublish.Count.ToString());

            tableInPublishList.GetFormatedLines(false).ForEach(s => WriteToOutput(connectionData, _tabSpacer + s));

            return this;
        }

        #endregion Методы для работы со списком на публикацию.

        private Logger GetOutputLogger(Guid? connectionDataId)
        {
            string folderPath = string.Empty;

            if (connectionDataId.HasValue)
            {
                folderPath = FileOperations.GetConnectionOutputFolderPath(connectionDataId.Value);
            }
            else
            {
                folderPath = FileOperations.GetOutputFolderPath();
            }

            Guid computedConnectionId = connectionDataId ?? _outputWindowPaneId;

            if (_loggersOutputCache.ContainsKey(computedConnectionId))
            {
                return _loggersOutputCache[computedConnectionId];
            }

            string loggerName = _loggerOutputName + computedConnectionId.ToString();

            var targetGenFile = new FileTarget()
            {
                Name = loggerName + "Target",
                LineEnding = LineEndingMode.CRLF,
                Encoding = Encoding.UTF8,
                WriteBom = true,
                CreateDirs = true,
                FileName = Path.Combine(folderPath, "Output ${date:format=yyyy-MM-dd}.log"),
                Layout = "${message}"
            };

            LogManager.Configuration.AddTarget(targetGenFile);
            LogManager.Configuration.AddRuleForAllLevels(targetGenFile, loggerName);

            LogManager.ReconfigExistingLoggers();

            var loggerOutput = LogManager.GetLogger(loggerName);

            _loggersOutputCache.TryAdd(computedConnectionId, loggerOutput);

            return loggerOutput;
        }

        private OutputWindowPane GetOutputWindow(ConnectionData connectionData)
        {
            try
            {
                string outputWindowPaneName = _outputWindowPaneName;

                Guid outputWindowPaneId = _outputWindowPaneId;

                if (connectionData != null)
                {
                    if (connectionData.ConnectionId == Guid.Empty)
                    {
                        connectionData.ConnectionId = Guid.NewGuid();
                    }

                    string connectionName = !string.IsNullOrEmpty(connectionData.Name) ? connectionData.Name : connectionData.ConnectionId.ToString();

                    outputWindowPaneName += $" - {connectionName}";

                    outputWindowPaneId = connectionData.ConnectionId;
                }

                ThreadHelper.ThrowIfNotOnUIThread();

                var iVsOutputWindowService = (IVsOutputWindow)Package.GetGlobalService(typeof(SVsOutputWindow));

                iVsOutputWindowService.GetPane(ref outputWindowPaneId, out IVsOutputWindowPane outputWindowPane);

                if (outputWindowPane == null)
                {
                    iVsOutputWindowService.CreatePane(ref outputWindowPaneId, outputWindowPaneName, 1, 1);
                    iVsOutputWindowService.GetPane(ref outputWindowPaneId, out outputWindowPane);
                }

                outputWindowPane.SetName(outputWindowPaneName);

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
                            if (Guid.TryParse(pane.Guid, out var tempGuid)
                                && tempGuid == outputWindowPaneId
                            )
                            {
                                return pane;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToLog(ex);
            }

            return null;
        }

        public IWriteToOutput WriteToOutputFilePathUri(ConnectionData connectionData, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return this;
            }

            var commonConfig = CommonConfiguration.Get();

            var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.FileUriFormat1, uriFile);
            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.OpenFileInVisualStudioUriFormat1, UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile));
            if (commonConfig.TextEditorProgramExists())
            {
                this.WriteToOutput(connectionData, string.Empty);
                this.WriteToOutput(connectionData, Properties.OutputStrings.OpenFileInTextEditorUriFormat1, UrlCommandFilter.GetUriOpenInTextEditorByFileUri(uriFile));
            }
            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.SelectFileInFolderUriFormat1, UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile));
            this.WriteToOutput(connectionData, string.Empty);

            return this;
        }

        public IWriteToOutput WriteToOutputFilePathUriToOpenInExcel(ConnectionData connectionData, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return this;
            }

            var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

            this.WriteToOutput(connectionData, Properties.OutputStrings.OpenFileInExcelUriFormat1, UrlCommandFilter.GetUriOpenInExcelByFileUri(uriFile));
            this.WriteToOutput(connectionData, string.Empty);

            return this;
        }

        public IWriteToOutput WriteToOutputSolutionUri(ConnectionData connectionData, string solutionUniqueName, Guid solutionId)
        {
            if (string.IsNullOrEmpty(solutionUniqueName))
            {
                return this;
            }

            string solutionUrl = connectionData.GetSolutionUrl(solutionId);

            string uriOpenSolution = UrlCommandFilter.GetUriOpenSolution(connectionData.ConnectionId, solutionUniqueName);

            string urlOpenSolutionExplorer = UrlCommandFilter.GetUriOpenSolutionExplorer(connectionData.ConnectionId);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.SelectedSolutionNameFormat1, solutionUniqueName);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.OpenSolutionListInBrowserSolutionUriFormat1, connectionData.GetOpenCrmWebSiteUrl(OpenCrmWebSiteType.Solutions));

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.OpenSolutionInBrowserSolutionUriFormat1, solutionUrl);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.OpenSolutionsExplorerSolutionUriFormat1, urlOpenSolutionExplorer);

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.OpenSolutionInExplorerSolutionUriFormat1, uriOpenSolution);

            this.WriteToOutput(connectionData, string.Empty);

            return this;
        }

        public IWriteToOutput SelectFileInFolder(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return this;
            }

            this.WriteToOutput(connectionData, Properties.OutputStrings.SelectingFileInFolderFormat1, filePath);

            var info = new ProcessStartInfo
            {
                FileName = _programExplorer,
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

            return this;
        }

        public IWriteToOutput OpenFolder(ConnectionData connectionData, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                this.WriteToOutput(connectionData, Properties.OutputStrings.FolderDoesNotExistsFormat1, folderPath);
                return this;
            }

            this.WriteToOutput(connectionData, Properties.OutputStrings.OpeningFolderFormat1, folderPath);

            var info = new ProcessStartInfo
            {
                FileName = _programExplorer,
                Arguments = string.Format(_formatStringInQuotes, folderPath),

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

            return this;
        }

        public IWriteToOutput PerformAction(ConnectionData connectionData, string filePath, bool hideFilePathUri = false)
        {
            if (!File.Exists(filePath))
            {
                return this;
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

            return this;
        }

        public IWriteToOutput OpenFile(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return this;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            FileAction fileAction = commonConfig.GetFileActionByExtension(Path.GetExtension(filePath));

            if (fileAction == FileAction.OpenFileInTextEditor && commonConfig.TextEditorProgramExists())
            {
                OpenFileInTextEditor(connectionData, filePath);
            }
            else
            {
                OpenFileInVisualStudio(connectionData, filePath);
            }

            return this;
        }

        public IWriteToOutput OpenFileInVisualStudio(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath)
                || ApplicationObject == null
            )
            {
                return this;
            }

            this.WriteToOutput(connectionData, Properties.OutputStrings.OpeningFileInVisualStudioFormat1, filePath);

            ApplicationObject.ItemOperations.OpenFile(filePath);
            ApplicationObject.MainWindow.Activate();

            return this;
        }

        public IWriteToOutput OpenFetchXmlFile(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName)
        {
            if (connectionData == null || string.IsNullOrEmpty(entityName))
            {
                return this;
            }

            entityName = entityName.ToLower();

            string fileName = string.Format(_formatEntityFetchXmlFileName, entityName);
            string folder = FileOperations.GetConnectionFetchXmlFolderPath(connectionData.ConnectionId);

            string filePath = Path.Combine(folder, fileName);

            if (!File.Exists(filePath))
            {
                var fetchXmlStringBuilder = new StringBuilder();

                try
                {
                    List<object> fetchContent = new List<object>();

                    var entityIntellisenseData = connectionData.EntitiesIntellisenseData?.Entities?[entityName];

                    if (entityIntellisenseData != null)
                    {
                        fetchContent.Add(new FetchAttributeType()
                        {
                            name = entityIntellisenseData.EntityPrimaryIdAttribute,
                        });

                        if (!string.IsNullOrEmpty(entityIntellisenseData.EntityPrimaryNameAttribute))
                        {
                            fetchContent.Add(new FetchAttributeType()
                            {
                                name = entityIntellisenseData.EntityPrimaryNameAttribute,
                            });

                            fetchContent.Add(new FetchOrderType()
                            {
                                attribute = entityIntellisenseData.EntityPrimaryNameAttribute,
                                descending = false,
                            });
                        }
                        else
                        {
                            fetchContent.Add(new FetchOrderType()
                            {
                                attribute = entityIntellisenseData.EntityPrimaryIdAttribute,
                                descending = false,
                            });
                        }
                    }
                    else
                    {
                        fetchContent.Add(new allattributes());
                    }

                    FetchType fetchXml = new FetchType()
                    {
                        nolock = true,
                        version = "1.0",

                        mapping = FetchTypeMapping.logical,
                        mappingSpecified = true,

                        outputformat = FetchTypeOutputformat.xmlplatform,
                        outputformatSpecified = true,

                        Items = new object[]
                        {
                            new FetchEntityType()
                            {
                                name = entityName,

                                Items = fetchContent.ToArray(),
                            },
                        },
                    };

                    var serializer = new XmlSerializer(typeof(FetchType));

                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);

                    using (var writer = new StringWriter(fetchXmlStringBuilder))
                    {
                        serializer.Serialize(writer, fetchXml, namespaces);
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }

                string xmlContent = fetchXmlStringBuilder.ToString();

                if (ContentComparerHelper.TryParseXml(xmlContent, out XElement doc))
                {
                    var attributesAll = doc.Attributes().ToList();

                    foreach (var attribute in attributesAll)
                    {
                        if (attribute.IsNamespaceDeclaration)
                        {
                            attribute.Remove();
                        }
                    }

                    xmlContent = doc.ToString();
                }

                try
                {
                    xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                        xmlContent
                        , commonConfig
                        , XmlOptionsControls.SavedQueryXmlOptions
                        , schemaName: Commands.AbstractDynamicCommandXsdSchemas.FetchSchema
                        , entityName: entityName
                    );

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(connectionData, ex);
                }
            }

            this.WriteToOutputFilePathUri(connectionData, filePath);

            this.OpenFileInVisualStudio(connectionData, filePath);
            return this;
        }

        public bool TryFindFileByRelativePath(ConnectionData connectionData, string fileRelativePath, out string filePath)
        {
            filePath = string.Empty;

            if (ApplicationObject == null || string.IsNullOrEmpty(fileRelativePath))
            {
                return false;
            }

            filePath = fileRelativePath.Replace("/", "\\").Trim('\\', '/');

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
                return false;
            }

            return true;
        }

        public IWriteToOutput OpenFileInVisualStudioRelativePath(ConnectionData connectionData, string fileRelativePath)
        {
            return OpenFileInVisualStudioRelativePath(connectionData, fileRelativePath, out _);
        }

        public IWriteToOutput OpenFileInVisualStudioRelativePath(ConnectionData connectionData, string fileRelativePath, out bool success)
        {
            success = false;

            if (!TryFindFileByRelativePath(connectionData, fileRelativePath, out string filePath))
            {
                return this;
            }

            this.WriteToOutput(connectionData, Properties.OutputStrings.OpeningFileInVisualStudioFormat1, filePath);

            ApplicationObject.ItemOperations.OpenFile(filePath);
            ApplicationObject.MainWindow.Activate();

            success = true;

            return this;
        }

        public IWriteToOutput ShowDifference(Uri uri)
        {
            if (!File.Exists(uri.LocalPath))
            {
                return this;
            }

            ConnectionData connectionData = null;

            string fieldName = string.Empty;
            string fieldTitle = string.Empty;

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var queryDictionary = HttpUtility.ParseQueryString(uri.Query);

                if (queryDictionary.AllKeys.Contains(UrlCommandFilter.uriComponentConnectionId, StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary[UrlCommandFilter.uriComponentConnectionId])
                )
                {
                    var idStr = queryDictionary[UrlCommandFilter.uriComponentConnectionId];

                    if (Guid.TryParse(idStr, out var tempGuid))
                    {
                        var connectionConfig = Model.ConnectionConfiguration.Get();

                        connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == tempGuid);
                    }
                }

                if (queryDictionary.AllKeys.Contains(UrlCommandFilter.uriComponentFieldName, StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary[UrlCommandFilter.uriComponentFieldName])
                )
                {
                    fieldName = queryDictionary[UrlCommandFilter.uriComponentFieldName];
                }

                if (queryDictionary.AllKeys.Contains(UrlCommandFilter.uriComponentFieldTitle, StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary[UrlCommandFilter.uriComponentFieldTitle])
                )
                {
                    fieldTitle = queryDictionary[UrlCommandFilter.uriComponentFieldTitle];
                }
            }

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return this;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            string solutionDirectoryPath = GetSolutionDirectory();

            SelectedFile selectedFile = new SelectedFile(uri.LocalPath, solutionDirectoryPath);

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    if (FileOperations.SupportsWebResourceTextType(uri.LocalPath))
                    {
                        Controller.StartWebResourceDifference(connectionData, commonConfig, selectedFile, false);
                    }
                    else if (FileOperations.SupportsReportType(uri.LocalPath))
                    {
                        Controller.StartReportDifference(connectionData, commonConfig, selectedFile, fieldName, fieldTitle, false);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }

            return this;
        }

        private string GetSolutionDirectory()
        {
            string result = ApplicationObject?.Solution?.FullName;

            if (!string.IsNullOrEmpty(result))
            {
                result = Path.GetDirectoryName(result);
            }

            return result;
        }

        public IWriteToOutput OpenSolution(Uri uri)
        {
            ConnectionData connectionData = null;
            string solutionUniqueName = null;

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var queryDictionary = HttpUtility.ParseQueryString(uri.Query);

                if (queryDictionary.AllKeys.Contains(UrlCommandFilter.uriComponentConnectionId, StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary[UrlCommandFilter.uriComponentConnectionId])
                )
                {
                    var idStr = queryDictionary[UrlCommandFilter.uriComponentConnectionId];

                    if (Guid.TryParse(idStr, out var tempGuid))
                    {
                        var connectionConfig = Model.ConnectionConfiguration.Get();

                        connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == tempGuid);
                    }
                }

                if (queryDictionary.AllKeys.Contains(UrlCommandFilter.uriComponentSolutionUniqueName, StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary[UrlCommandFilter.uriComponentSolutionUniqueName])
                )
                {
                    solutionUniqueName = queryDictionary[UrlCommandFilter.uriComponentSolutionUniqueName];
                }
            }

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return this;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && !string.IsNullOrEmpty(solutionUniqueName))
            {
                this.HandleSolutionOpenLastSelected(connectionData, solutionUniqueName, ActionOnComponent.OpenInExplorer);
            }

            return this;
        }

        public IWriteToOutput OpenSolutionList(Uri uri)
        {
            ConnectionData connectionData = null;

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var queryDictionary = HttpUtility.ParseQueryString(uri.Query);

                if (queryDictionary.AllKeys.Contains(UrlCommandFilter.uriComponentConnectionId, StringComparer.InvariantCultureIgnoreCase)
                    && !string.IsNullOrEmpty(queryDictionary[UrlCommandFilter.uriComponentConnectionId])
                )
                {
                    var idStr = queryDictionary[UrlCommandFilter.uriComponentConnectionId];

                    if (Guid.TryParse(idStr, out var tempGuid))
                    {
                        var connectionConfig = Model.ConnectionConfiguration.Get();

                        connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == tempGuid);
                    }
                }
            }

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return this;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                this.HandleOpenSolutionExplorerWindow(connectionData);
            }

            return this;
        }

        public IWriteToOutput OpenFileInTextEditor(ConnectionData connectionData, string filePath)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!File.Exists(filePath) || !commonConfig.TextEditorProgramExists())
            {
                return this;
            }

            this.WriteToOutput(connectionData, Properties.OutputStrings.OpeningFileInTextEditorFormat1, filePath);

            var info = new ProcessStartInfo
            {
                FileName = string.Format(_formatStringInQuotes, commonConfig.TextEditorProgram),
                Arguments = string.Format(_formatStringInQuotes, filePath),

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

            return this;
        }

        public IWriteToOutput OpenFileInExcel(ConnectionData connectionData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return this;
            }

            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, Properties.OutputStrings.OpeningFileInExcelFormat1, filePath);

            var info = new ProcessStartInfo
            {
                FileName = _programExcel,
                Arguments = string.Format(_formatStringInQuotes, filePath),

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

            return this;
        }

        public async System.Threading.Tasks.Task ProcessStartProgramComparerAsync(ConnectionData connectionData1, string filePath1, string filePath2, string fileTitle1, string fileTitle2, ConnectionData connectionData2 = null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this.WriteToOutput(connectionData1, string.Empty);
                this.WriteToOutput(connectionData1, string.Empty);
                this.WriteToOutput(connectionData1, string.Empty);

                this.WriteToOutput(connectionData1, Properties.OutputStrings.ExecutingDifferenceProgramForFiles);

                this.WriteToOutput(connectionData1, filePath1);
                this.WriteToOutputFilePathUri(connectionData1, filePath1);

                this.WriteToOutput(connectionData1, string.Empty);
                this.WriteToOutput(connectionData1, string.Empty);
                this.WriteToOutput(connectionData1, filePath2);
                this.WriteToOutputFilePathUri(connectionData1, filePath2);

                if (connectionData2 != null)
                {
                    this.WriteToOutput(connectionData2, string.Empty);
                    this.WriteToOutput(connectionData2, string.Empty);
                    this.WriteToOutput(connectionData2, string.Empty);

                    this.WriteToOutput(connectionData2, Properties.OutputStrings.ExecutingDifferenceProgramForFiles);

                    this.WriteToOutput(connectionData2, filePath1);
                    this.WriteToOutputFilePathUri(connectionData2, filePath1);

                    this.WriteToOutput(connectionData2, string.Empty);
                    this.WriteToOutput(connectionData2, string.Empty);
                    this.WriteToOutput(connectionData2, filePath2);
                    this.WriteToOutputFilePathUri(connectionData2, filePath2);
                }

                if (commonConfig.DifferenceProgramExists())
                {
                    var info = new ProcessStartInfo
                    {
                        FileName = string.Format(_formatStringInQuotes, commonConfig.CompareProgram)
                    };

                    var arguments = new StringBuilder(commonConfig.CompareArgumentsFormat);

                    arguments = arguments.Replace(CommonConfiguration.placeholderFile1Path, filePath1);
                    arguments = arguments.Replace(CommonConfiguration.placeholderFile2Path, filePath2);

                    arguments = arguments.Replace(CommonConfiguration.placeholderFile1Title, fileTitle1);
                    arguments = arguments.Replace(CommonConfiguration.placeholderFile2Title, fileTitle2);

                    info.Arguments = arguments.ToString();

                    info.UseShellExecute = false;
                    info.WindowStyle = ProcessWindowStyle.Normal;

                    try
                    {
                        var process = System.Diagnostics.Process.Start(info);

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
                        this.WriteErrorToOutput(connectionData1, ex);

                        if (connectionData2 != null)
                        {
                            this.WriteErrorToOutput(connectionData2, ex);
                        }
                    }
                }
                else
                {
                    bool diffExecuted = false;

                    if (CrmDeveloperHelperPackage.Singleton != null)
                    {
                        try
                        {
                            //Tools.DiffFiles
                            var args = $"\"{filePath1}\" \"{filePath2}\" \"{fileTitle1}\" \"{fileTitle2}\"";

                            var commandService = await CrmDeveloperHelperPackage.Singleton?.GetServiceAsync(typeof(System.ComponentModel.Design.IMenuCommandService)) as OleMenuCommandService;
                            if (commandService != null)
                            {
                                diffExecuted = commandService.GlobalInvoke(ToolsDiffCommand, args);
                            }
                            else
                            {
                                this.WriteToOutput(connectionData1, Properties.OutputStrings.CannotGetOleMenuCommandService);

                                if (connectionData2 != null)
                                {
                                    this.WriteToOutput(connectionData2, Properties.OutputStrings.CannotGetOleMenuCommandService);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.WriteErrorToOutput(connectionData1, ex);

                            if (connectionData2 != null)
                            {
                                this.WriteErrorToOutput(connectionData2, ex);
                            }
                        }
                    }

                    if (diffExecuted)
                    {
                        this.ActivateVisualStudioWindow();
                    }
                    else
                    {
                        this.WriteToOutput(connectionData1, Properties.OutputStrings.CannotExecuteVisualStudioDiffProgram);

                        if (connectionData2 != null)
                        {
                            this.WriteToOutput(connectionData2, Properties.OutputStrings.CannotExecuteVisualStudioDiffProgram);
                        }
                    }
                }
            }
            else
            {
                this.OpenFile(connectionData1, filePath1);

                this.OpenFile(connectionData1 ?? connectionData2, filePath2);
            }
        }

        public IWriteToOutput ProcessStartProgramComparerThreeWayFile(string fileLocalPath, string filePath1, string filePath2, string fileLocalTitle, string fileTitle1, string fileTitle2)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!commonConfig.DifferenceThreeWayAvaliable())
            {
                this.WriteToOutput(null, Properties.OutputStrings.NoValidConfigurationForThreeWayDifference);
                return this;
            }

            if (File.Exists(filePath1) && File.Exists(filePath2) && File.Exists(fileLocalPath))
            {
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);

                this.WriteToOutput(null, Properties.OutputStrings.ExecutingThreeWayDifferenceProgramForFiles);

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

                var info = new ProcessStartInfo
                {
                    FileName = string.Format(_formatStringInQuotes, commonConfig.CompareProgram)
                };

                var arguments = new StringBuilder(commonConfig.CompareArgumentsThreeWayFormat);

                arguments = arguments.Replace(CommonConfiguration.placeholderFileLocalPath, fileLocalPath);
                arguments = arguments.Replace(CommonConfiguration.placeholderFile1Path, filePath1);
                arguments = arguments.Replace(CommonConfiguration.placeholderFile2Path, filePath2);

                arguments = arguments.Replace(CommonConfiguration.placeholderFileLocalTitle, fileLocalTitle);
                arguments = arguments.Replace(CommonConfiguration.placeholderFile1Title, fileTitle1);
                arguments = arguments.Replace(CommonConfiguration.placeholderFile2Title, fileTitle2);

                info.Arguments = arguments.ToString();

                info.UseShellExecute = false;
                info.WindowStyle = ProcessWindowStyle.Normal;

                try
                {
                    var process = System.Diagnostics.Process.Start(info);

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

            return this;
        }

        public System.Threading.Tasks.Task<int> BuildProjectAsync(Project project)
        {
            if (project == null
                || string.IsNullOrEmpty(project.Name)
                || string.IsNullOrEmpty(project.UniqueName)
                || this.ApplicationObject.Solution == null
                || this.ApplicationObject.Solution.SolutionBuild == null
                || this.ApplicationObject.Solution.SolutionBuild.BuildState == vsBuildState.vsBuildStateInProgress
            )
            {
                return System.Threading.Tasks.Task.FromResult<int>(-1);
            }

            var build = this.ApplicationObject.Solution.SolutionBuild;

            return System.Threading.Tasks.Task.Run(() => BuildProject(build, project));
        }

        private static int BuildProject(SolutionBuild build, Project project)
        {
            build.BuildProject(project.ConfigurationManager.ActiveConfiguration.ConfigurationName, project.UniqueName, WaitForBuildToFinish: true);

            var info = build.LastBuildInfo;

            return info;
        }
    }
}
