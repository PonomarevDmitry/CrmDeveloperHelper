using EnvDTE;
using EnvDTE80;
using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Properties;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
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
using System.Web;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class DTEHelper : IWriteToOutputAndPublishList
    {
        private const string _outputWindowName = "Crm Developer Helper";

        private const string _tabSpacer = "    ";

        private const int timeDelay = 2000;

        private static readonly Guid ToolsDiffCommandGuid = new Guid("5D4C0442-C0A2-4BE8-9B4D-AB1C28450942");
        private const int ToolsDiffCommandId = 256;

        public DTE2 ApplicationObject { get; private set; }

        private MainController Controller;

        private HashSet<string> _ListForPublish = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static DTEHelper _singleton;

        private static ConditionalWeakTable<Exception, object> _logExceptions = new ConditionalWeakTable<Exception, object>();

        private static ConditionalWeakTable<Exception, object> _outputExceptions = new ConditionalWeakTable<Exception, object>();

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

        private static readonly object syncObject = new object();

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

        private const string _loggerOutputName = "OutputLogger";
        private const string _loggerErrors = "ErrorLogger";

        private static readonly ConcurrentDictionary<string, Logger> _loggersOutputCache = new ConcurrentDictionary<string, Logger>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly Logger Log;

        private static readonly string _logLayout = new string('-', 150) + "${newline}${newline}${newline}${newline}${newline}${newline}${longdate}|${level}${newline}${message}${newline}${newline}${exception}${newline}${newline}${stacktrace:format=Raw:topFrames=10}${newline}" + new string('-', 150);

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

        #region Получение списков файлов.

        public string GetSelectedText()
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
                string path = ApplicationObject.ActiveWindow.Document.FullName;

                if (checkerFunction(path))
                {
                    selectedFiles.Add(new SelectedFile(path, GetFriendlyPath(path)));
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
                foreach (var document in ApplicationObject.Documents.OfType<EnvDTE.Document>())
                {
                    if (document.ActiveWindow == null
                        || document.ActiveWindow.Type != EnvDTE.vsWindowType.vsWindowTypeDocument
                        || document.ActiveWindow.Visible == false
                        )
                    {
                        continue;
                    }

                    if (ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindTextView)
                        || ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindCode)
                        )
                    {
                        string path = document.FullName;

                        if (checkerFunction(path))
                        {
                            selectedFiles.Add(new SelectedFile(path, GetFriendlyPath(path)));
                        }
                    }
                }
            }

            return selectedFiles;
        }

        public List<EnvDTE.Document> GetOpenedDocumentsAsDocument(Func<string, bool> checkerFunction)
        {
            List<EnvDTE.Document> selectedFiles = new List<EnvDTE.Document>();

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
                )
            {
                foreach (var document in ApplicationObject.Documents.OfType<EnvDTE.Document>())
                {
                    if (document.ActiveWindow == null
                      || document.ActiveWindow.Type != EnvDTE.vsWindowType.vsWindowTypeDocument
                      || document.ActiveWindow.Visible == false
                      )
                    {
                        continue;
                    }

                    if (ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindTextView)
                        || ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindCode)
                        )
                    {
                        string path = document.FullName;

                        if (checkerFunction(path))
                        {
                            selectedFiles.Add(document);
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

                hash.OrderBy(path => path).ToList().ForEach(
                    path => selectedFiles.Add(new SelectedFile(path, GetFriendlyPath(path)))
                );
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
                        DTEHelper.WriteExceptionToOutput(null, ex);

                        return false;
                    }
                });

                if (filtered.Count() == 1)
                {
                    result = filtered.FirstOrDefault();
                }
            }

            return result;
        }

        public List<SelectedItem> GetListSelectedItemInSolutionExplorer(Func<string, bool> checkerFunction)
        {
            List<SelectedItem> result = new List<SelectedItem>();

            if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

                var filtered = items.Where(a =>
                {
                    try
                    {
                        return a.ProjectItem != null && a.ProjectItem.ContainingProject != null && checkerFunction(a.Name.ToLower());
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);

                        return false;
                    }
                });

                result.AddRange(filtered);
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

        private string GetFriendlyPath(string filePath)
        {
            string solutionPath = ApplicationObject?.Solution?.FullName;

            if (!string.IsNullOrEmpty(solutionPath))
            {
                return filePath.Replace(Path.GetDirectoryName(solutionPath), string.Empty);
            }

            return filePath;
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

        public IEnumerable<EnvDTE.Project> GetSelectedProjects()
        {
            if (ApplicationObject.ActiveWindow != null
                   && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                   && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject
                    .SelectedItems
                    .Cast<EnvDTE.SelectedItem>()
                    .Where(e => e.Project != null)
                    .Select(e => e.Project)
                    .Distinct()
                    .ToList();

                return items;
            }

            return Enumerable.Empty<EnvDTE.Project>();
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

            selectedFiles.AddRange(_ListForPublish.OrderBy(s => s).Select(path => new SelectedFile(path, GetFriendlyPath(path))));

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

        private readonly ConcurrentDictionary<string, bool> _firstLineInOutput = new ConcurrentDictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

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

        public void ClearListForPublish()
        {
            _ListForPublish.Clear();

            ActivateOutputWindow(null);

            WriteToOutput(null, string.Empty);

            WriteToOutput(null, "Publish List has cleaned.");
        }

        public void ShowListForPublish()
        {
            ActivateOutputWindow(null);

            WriteToOutput(null, string.Empty);

            if (_ListForPublish.Count > 0)
            {
                WriteToOutput(null, "Publish List: {0}", _ListForPublish.Count.ToString());

                foreach (var path in _ListForPublish.OrderBy(s => s))
                {
                    WriteToOutput(null, GetFriendlyPath(path));
                }
            }
            else
            {
                WriteToOutput(null, Properties.OutputStrings.PublishListIsEmpty);
            }
        }

        #endregion Методы для работы со списком на публикацию.

        public void HandleFileCompareCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (connectionData == null)
            {
                connectionData = crmConfig.CurrentConnectionData;
            }

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartComparing(selectedFiles, connectionData, withDetails);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateContentWebResourcesAndPublishCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
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

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.PublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

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
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    try
                    {
                        Controller.StartUpdateContentAndPublish(selectedFiles, connectionData);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
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

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.PublishWebResourcesEqualByTextFormat2, selectedFiles.Count, connectionData.GetDescription());

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
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    try
                    {
                        Controller.StartUpdateContentAndPublishEqualByText(selectedFiles, connectionData);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingWebResourcesIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingPluginAssemblyIntoSolutionByProjectCommand(string solutionUniqueName, bool withSelect, params string[] projectNames)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingPluginAssemblyIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingPluginAssemblyProcessingStepsByProjectCommand(string solutionUniqueName, bool withSelect, params string[] projectNames)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingPluginAssemblyProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingPluginTypeProcessingStepsByProjectCommand(string solutionUniqueName, bool withSelect, params string[] pluginTypeNames)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingPluginTypeProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingReportsIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var defaultFolder = PropertiesHelper.GetOutputPath(project);

                try
                {
                    Controller.StartComparingPluginAssemblyAndLocalAssembly(connectionData, commonConfig, project.Name, defaultFolder);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExecutingFetchXml(ConnectionData connectionData, SelectedFile selectedFile, bool strictConnection)
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
                CrmDeveloperHelperPackage.Singleton?.ExecuteFetchXmlQueryAsync(selectedFile.FilePath, connectionData, strictConnection);
            }
        }

        public void HandleUpdateGlobalOptionSetsFile(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withSelect)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (connectionData == null)
            {
                connectionData = crmConfig.CurrentConnectionData;
            }

            if (crmConfig != null && connectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartUpdatingFileWithGlobalOptionSets(connectionData, commonConfig, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateEntityMetadataFile(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (connectionData == null)
            {
                connectionData = crmConfig.CurrentConnectionData;
            }

            if (crmConfig != null && connectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartUpdatingFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                string filePath = selectedFiles[0].FilePath;

                var form = new WindowCreateProxyClasses(commonConfig, crmConfig, filePath);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.UpdateProxyClasses(filePath, connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleReportDifferenceCommand(ConnectionData connectionData, string fieldName, string fieldTitle, bool isCustom)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartReportDifference(selectedFiles[0], fieldName, fieldTitle, isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleReportThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartReportThreeFileDifference(selectedFiles[0], fieldName, fieldTitle, connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartReportUpdate(selectedFiles[0], connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomReport(connectionData, commonConfig, selectedFile.FileName);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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
                            connectionData.OpenEntityInstanceInWeb(Entities.Report.EntityLogicalName, objectId.Value);
                            return;

                        case ActionOpenComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, objectId.Value);
                            return;
                    }
                }

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpeningReport(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenLastSelectedSolution(ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
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

            if (connectionData != null && !string.IsNullOrEmpty(solutionUniqueName) && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpeningSolution(commonConfig, connectionData, solutionUniqueName, action);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingLastLinkReport(selectedFile, connectionData);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartClearingLastLink(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartWebResourceDifference(selectedFiles[0], isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRibbonDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartRibbonDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSiteMapDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartSiteMapDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSiteMapUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartSiteMapUpdate(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSystemFormDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartSystemFormDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSystemFormUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartSystemFormUpdate(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSavedQueryDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartSavedQueryDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSavedQueryUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartSavedQueryUpdate(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRibbonDiffXmlDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartRibbonDiffXmlDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRibbonDiffXmlUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                if (connectionData.IsReadOnly)
                {
                    this.WriteToOutput(null, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                    return;
                }

                string message = string.Format(Properties.MessageBoxStrings.PublishRibbonDiffXmlFormat2, selectedFile.FileName, connectionData.GetDescription());

                var dialog = new WindowConfirmPublish(message, false);

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartRibbonDiffXmlUpdate(selectedFile, connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleWebResourceThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false);

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartWebResourceThreeFileDifference(selectedFiles[0], connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleCheckFileEncodingCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartCheckFileEncoding(selectedFiles);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomWebResource(connectionData, commonConfig, selectedFile.FileName);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (action)
                    {
                        case ActionOpenComponent.OpenInWeb:
                            connectionData.OpenEntityInstanceInWeb(Entities.WebResource.EntityLogicalName, objectId.Value);
                            return;

                        case ActionOpenComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, objectId.Value);
                            return;
                    }
                }

                try
                {
                    Controller.StartOpeningWebResource(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                var form = new WindowSelectFolderForExport(connectionData, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    connectionData = form.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.ShowingWebResourcesDependentComponents(selectedFiles, connectionData, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckingWorkflowsUsedEntities()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var form = new WindowSelectFolderForExport(connectionData, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    connectionData = form.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.ExecuteCheckingWorkflowsUsedEntities(connectionData, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckingWorkflowsNotExistingUsedEntities()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var form = new WindowSelectFolderForExport(connectionData, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    connectionData = form.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.ExecuteCheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                if (inTextEditor && !File.Exists(commonConfig.TextEditorProgram))
                {
                    return;
                }

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                Controller.StartOpeningFiles(selectedFiles, openFilesType, inTextEditor, connectionData, commonConfig);
            }
        }

        public void HandleFileCompareListForPublishCommand(ConnectionData connectionData, bool withDetails)
        {
            List<SelectedFile> selectedFiles = this.GetSelectedFilesFromListForPublish();

            if (selectedFiles.Count > 0)
            {
                this.ShowListForPublish();

                this.HandleFileCompareCommand(connectionData, selectedFiles, withDetails);
            }
            else
            {
                this.WriteToOutput(null, Properties.OutputStrings.PublishListIsEmpty);
                this.ActivateOutputWindow(connectionData);
            }
        }

        public void HandleMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartMultiDifferenceFiles(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public void TestConnection(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (connectionData == null)
            {
                connectionData = crmConfig.CurrentConnectionData;
            }

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                QuickConnection.TestConnectAsync(connectionData, this);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.StartCheckEntitiesNames(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.StartCheckEntitiesNamesAndShowDependentComponents(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select mark to delete", "Mark to delete");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.StartCheckMarkedToDeleteAndShowDependentComponents(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckEntitiesOwnership(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckEntitiesOwnership(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckGlobalOptionSetDuplicates(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckGlobalOptionSetDuplicates(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckComponentTypeEnum(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckComponentTypeEnum(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCreateAllDependencyNodesDescription(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreateAllDependencyNodesDescription(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckManagedEntities(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckManagedEntities(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginSteps(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckPluginSteps(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginImages(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckPluginImages(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginStepsRequiredComponents(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckPluginStepsRequiredComponents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginImagesRequiredComponents(ConnectionData connectionData)
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

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCheckPluginImagesRequiredComponents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select Element Name", "Element Name");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.StartFindEntityElementsByName(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select String for contain", "String for contain");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        try
                        {
                            Controller.StartFindEntityElementsContainsString(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, string.Format("Find Entity in {0} by Id", connectionData.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    connectionData = dialog.GetConnectionData();

                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    try
                    {
                        Controller.StartFindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, string.Format("Find Entity in {0} by Uniqueidentifier", connectionData.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    connectionData = dialog.GetConnectionData();

                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    try
                    {
                        Controller.StartFindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleOpenPluginTree(string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingPluginTree(connectionData, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSdkMessageTree(connectionData, commonConfig, selection, null);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSdkMessageRequestTree(connectionData, commonConfig, selection, null);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSystemUsersExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSystemUserExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenTeamsExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingTeamsExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSecurityRolesExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSecurityRolesExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportPluginConfiguration(connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
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
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationAssemblyDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
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
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationTypeDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationTree(connectionData, commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationComparer(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleOrganizationComparer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ConnectionConfiguration crmConfig = Model.ConnectionConfiguration.Get();

            if (crmConfig != null && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartOrganizationComparer(crmConfig, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleTraceReaderWindow(ConnectionData connectionData)
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

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    this.Controller.StartTraceReaderWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingFileWithEntityMetadata(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityAttributeExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityAttributeExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityKeyExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityKeyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityRelationshipOneToManyExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityRelationshipOneToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityRelationshipManyToManyExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityRelationshipManyToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntitySecurityRolesExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntitySecurityRolesExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var form = new WindowExportFormEvents(commonConfig);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportingFormEvents(connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingFileWithGlobalOptionSets(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportOrganizationInformation(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportPluginAssembly(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportPluginTypeDescription(string selection)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportPluginTypeDescription(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddPluginStep(string pluginTypeName, ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (connectionData == null)
            {
                connectionData = crmConfig.CurrentConnectionData;
            }

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddPluginStep(pluginTypeName, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomReport(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportRibbonXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSitemapXml(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSolutionExplorerWindow(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            if (connectionData == null)
            {
                connectionData = crmConfig.CurrentConnectionData;
            }

            if (crmConfig != null && connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenSolutionExplorerWindow(null, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSolutionImageWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenSolutionImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenOrganizationDifferenceImageWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenOrganizationDifferenceImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSystemFormXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSystemSavedQueryXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSystemSavedQueryVisualizationXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomWebResource(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportWorkflow(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingIntoPublishListFilesByType(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckOpenFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartOpenFilesWithouUTF8Encoding(selectedFiles);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartComparingFilesWithWrongEncoding(selectedFiles, connectionData, withDetails);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingLastLinkWebResourceMultiple(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && selectedItem != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenSolutionExplorerWindow(selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
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

            var connectionData = crmConfig.CurrentConnectionData;

            if (crmConfig != null && connectionData != null && selectedItem != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportPluginConfigurationIntoFolder(selectedItem, connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleOpenCrmInWeb(ConnectionData connectionData, OpenCrmWebSiteType crmWebSiteType)
        {
            try
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
                    connectionData.OpenCrmWebSite(crmWebSiteType);
                }
            }
            catch (Exception ex)
            {
                this.WriteErrorToOutput(connectionData, ex);
            }
        }

        public void HandlePublishAll(ConnectionData connectionData)
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

            if (connectionData != null)
            {
                string message = string.Format(Properties.MessageBoxStrings.PublishAllInConnectionFormat1, connectionData.Name);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    try
                    {
                        this.Controller.StartPublishAll(connectionData);
                    }
                    catch (Exception ex)
                    {
                        this.WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleExportDefaultSitemap(string selectedSitemap)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                string fileName = string.Format("SiteMap.{0}.xml", selectedSitemap);

                var dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    DefaultExt = ".xml",

                    Filter = "SiteMap (.xml)|*.xml",
                    FilterIndex = 1,

                    RestoreDirectory = true,
                    FileName = fileName,

                    InitialDirectory = commonConfig.FolderForExport,
                };

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    ActivateOutputWindow(null);
                    WriteToOutputEmptyLines(null, commonConfig);

                    try
                    {
                        Uri uri = FileOperations.GetSiteMapResourceUri(selectedSitemap);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        var doc = XDocument.Load(info.Stream);
                        info.Stream.Dispose();

                        var filePath = dialog.FileName;

                        doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutput(null, "{0} exported.", fileName);

                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutputFilePathUri(null, filePath);

                        PerformAction(null, filePath, true);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(null, ex);
                    }
                }
            }
        }

        public void HandleShowDifferenceWithDefaultSitemap(SelectedFile selectedFile, string selectedSitemap)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig == null)
            {
                return;
            }

            ActivateOutputWindow(null);
            WriteToOutputEmptyLines(null, commonConfig);

            try
            {
                Uri uri = FileOperations.GetSiteMapResourceUri(selectedSitemap);
                StreamResourceInfo info = Application.GetResourceStream(uri);

                var doc = XDocument.Load(info.Stream);
                info.Stream.Dispose();

                string fileName = string.Format("SiteMap.{0}.xml", selectedSitemap);

                var filePath = Path.Combine(FileOperations.GetTempFileFolder(), fileName);

                doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);

                this.WriteToOutput(null, "{0} exported.", fileName);

                this.WriteToOutput(null, string.Empty);

                this.WriteToOutputFilePathUri(null, filePath);

                this.ProcessStartProgramComparer(selectedFile.FilePath, filePath, selectedFile.FileName, fileName);
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(null, ex);
            }
        }

        public void HandleExportXsdSchema(string[] fileNamesColl)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                var form = new WindowSelectFolderForExport(null, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    commonConfig.Save();

                    ActivateOutputWindow(null);
                    WriteToOutputEmptyLines(null, commonConfig);

                    try
                    {
                        foreach (var fileName in fileNamesColl)
                        {
                            Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                            StreamResourceInfo info = Application.GetResourceStream(uri);

                            var doc = XDocument.Load(info.Stream);
                            info.Stream.Dispose();

                            var filePath = Path.Combine(commonConfig.FolderForExport, fileName);

                            doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                            this.WriteToOutput(null, string.Empty);
                            this.WriteToOutput(null, string.Empty);
                            this.WriteToOutput(null, string.Empty);

                            this.WriteToOutput(null, "{0} exported.", fileName);

                            this.WriteToOutput(null, string.Empty);

                            this.WriteToOutputFilePathUri(null, filePath);

                            PerformAction(null, filePath, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(null, ex);
                    }
                }
            }
        }

        public void HandleExportTraceEnableFile()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                string fileName = "TraceEnable.reg";

                var dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    DefaultExt = ".reg",

                    Filter = "Registry Edit (.reg)|*.reg",
                    FilterIndex = 1,

                    RestoreDirectory = true,
                    FileName = fileName,
                };

                if (!string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    dialog.InitialDirectory = commonConfig.FolderForExport;
                }

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow(null);
                    WriteToOutputEmptyLines(null, commonConfig);

                    try
                    {
                        Uri uri = FileOperations.GetResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        var filePath = dialog.FileName;

                        byte[] buffer = new byte[16345];
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int read;
                            while ((read = info.Stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, read);
                            }
                        }

                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutput(null, "{0} exported.", fileName);

                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutputFilePathUri(null, filePath);

                        SelectFileInFolder(null, filePath);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(null, ex);
                    }
                }
            }
        }

        public void HandleOpenXsdSchemaFolder()
        {
            var folder = FileOperations.GetSchemaXsdFolder();

            this.OpenFolder(folder);
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
            this.WriteToOutput(connectionData, "Open File in Visual Studio :    {0}", uriFile.Replace("file:", "openinvisualstudio:"));
            if (File.Exists(commonConfig.TextEditorProgram))
            {
                this.WriteToOutput(connectionData, string.Empty);
                this.WriteToOutput(connectionData, "Open File in TextEditor    :    {0}", uriFile.Replace("file:", "openintexteditor:"));
            }
            this.WriteToOutput(connectionData, string.Empty);
            this.WriteToOutput(connectionData, "Select File in Folder      :    {0}", uriFile.Replace("file:", "selectfileinfolder:"));
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
            if (File.Exists(filePath))
            {
                this.WriteToOutput(null, "Selecting file in folder {0}", filePath);

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

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                }
            }
        }

        public void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                this.WriteToOutput(null, "Opening folder {0}", folderPath);

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = folderPath,

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

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                }
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
            if (File.Exists(filePath))
            {
                if (ApplicationObject != null)
                {
                    this.WriteToOutput(connectionData, "Opening in Visual Studio file {0}", filePath);

                    ApplicationObject.ItemOperations.OpenFile(filePath);
                    ApplicationObject.MainWindow.Activate();
                }
            }
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

            if (File.Exists(filePath))
            {
                this.WriteToOutput(connectionData, "Opening in Visual Studio file {0}", filePath);

                ApplicationObject.ItemOperations.OpenFile(filePath);
                ApplicationObject.MainWindow.Activate();
            }
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

            if (File.Exists(filePath) && File.Exists(commonConfig.TextEditorProgram))
            {
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

                            System.Threading.Thread.Sleep(timeDelay);

                            if (!process.HasExited)
                            {
                                process.WaitForInputIdle(timeDelay);
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

                        System.Threading.Thread.Sleep(timeDelay);

                        if (!process.HasExited)
                        {
                            process.WaitForInputIdle(timeDelay);
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
    }
}