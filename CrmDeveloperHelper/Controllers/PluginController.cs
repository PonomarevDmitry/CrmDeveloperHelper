using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class PluginController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        private const string _tabspacer = "      ";

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PluginController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Сравнение сборки плагинов и локальной сборки.

        public async Task ExecuteComparingAssemblyAndCrmSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string projectName, string defaultOutputFilePath)
        {
            string operation = string.Format(Properties.OperationNames.ComparingCrmPluginAssemblyAndLocalAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ComparingAssemblyAndCrmSolution(connectionData, commonConfig, projectName, defaultOutputFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ComparingAssemblyAndCrmSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string projectName, string defaultOutputFilePath)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (string.IsNullOrEmpty(projectName))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositoryAssembly = new PluginAssemblyRepository(service);

            var assembly = await repositoryAssembly.FindAssemblyAsync(projectName);

            if (assembly == null)
            {
                assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectName);
            }

            if (assembly == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectName);

                WindowHelper.OpenPluginAssemblyWindow(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , projectName
                    );

                return;
            }

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = await CreateFileWithAssemblyComparing(commonConfig.FolderForExport, service, assembly.Id, assembly.Name, defaultOutputFilePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        public async Task<string> CreateFileWithAssemblyComparing(string folder, IOrganizationServiceExtented service, Guid idPluginAssembly, string assemblyName, string defaultOutputFilePath)
        {
            var repositoryType = new PluginTypeRepository(service);

            var task = repositoryType.GetPluginTypesAsync(idPluginAssembly);

            string assemblyPath = service.ConnectionData.GetLastAssemblyPath(assemblyName);
            List<string> lastPaths = service.ConnectionData.GetAssemblyPaths(assemblyName).ToList();

            if (!string.IsNullOrEmpty(defaultOutputFilePath)
                && !lastPaths.Contains(defaultOutputFilePath, StringComparer.InvariantCultureIgnoreCase)
            )
            {
                lastPaths.Insert(0, defaultOutputFilePath);
            }

            bool isSuccess = false;

            var t = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "Plugin Assebmly (.dll)|*.dll",
                        FilterIndex = 1,
                        RestoreDirectory = true
                    };

                    if (!string.IsNullOrEmpty(assemblyPath))
                    {
                        openFileDialog1.InitialDirectory = Path.GetDirectoryName(assemblyPath);
                        openFileDialog1.FileName = Path.GetFileName(assemblyPath);
                    }
                    else if (!string.IsNullOrEmpty(defaultOutputFilePath))
                    {
                        openFileDialog1.InitialDirectory = Path.GetDirectoryName(defaultOutputFilePath);
                        openFileDialog1.FileName = Path.GetFileName(defaultOutputFilePath);
                    }

                    if (lastPaths.Any())
                    {
                        openFileDialog1.CustomPlaces = new List<Microsoft.Win32.FileDialogCustomPlace>(lastPaths.Select(p => new Microsoft.Win32.FileDialogCustomPlace(Path.GetDirectoryName(p))));
                    }

                    if (openFileDialog1.ShowDialog() == true)
                    {
                        isSuccess = true;
                        assemblyPath = openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (!isSuccess)
            {
                return string.Empty;
            }

            string filePath = string.Empty;

            service.ConnectionData.AddAssemblyMapping(assemblyName, assemblyPath);
            service.ConnectionData.Save();

            AssemblyReaderResult assemblyCheck = null;

            using (var reader = new AssemblyReader())
            {
                assemblyCheck = reader.ReadAssembly(assemblyPath);
            }

            if (assemblyCheck == null)
            {
                return string.Empty;
            }

            var pluginTypes = await task;

            var crmPlugins = new HashSet<string>(pluginTypes.Where(e => !e.IsWorkflowActivity.GetValueOrDefault()).Select(e => e.TypeName), StringComparer.InvariantCultureIgnoreCase);
            var crmWorkflows = new HashSet<string>(pluginTypes.Where(e => e.IsWorkflowActivity.GetValueOrDefault()).Select(e => e.TypeName), StringComparer.InvariantCultureIgnoreCase);

            var content = new StringBuilder();

            content.AppendLine(service.ConnectionData.GetConnectionInfo()).AppendLine();
            content.AppendFormat("Description for PluginAssembly '{0}' at {1}", assemblyName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)).AppendLine();

            content.AppendFormat("Local Assembly '{0}'", assemblyPath).AppendLine();

            HashSet<string> assemblyPlugins = new HashSet<string>(assemblyCheck.Plugins, StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> assemblyWorkflows = new HashSet<string>(assemblyCheck.Workflows, StringComparer.InvariantCultureIgnoreCase);

            var pluginsOnlyInCrm = crmPlugins.Except(assemblyPlugins, StringComparer.InvariantCultureIgnoreCase);
            var workflowOnlyInCrm = crmWorkflows.Except(assemblyWorkflows, StringComparer.InvariantCultureIgnoreCase);

            var pluginsOnlyInLocalAssembly = assemblyPlugins.Except(crmPlugins, StringComparer.InvariantCultureIgnoreCase);
            var workflowOnlyInLocalAssembly = assemblyWorkflows.Except(crmWorkflows, StringComparer.InvariantCultureIgnoreCase);



            FillInformation(content, "Plugins ONLY in Crm Assembly {0}", pluginsOnlyInCrm);
            FillInformation(content, "Workflows ONLY in Crm Assembly {0}", workflowOnlyInCrm);
            FillInformation(content, "Plugins ONLY in Local Assembly {0}", pluginsOnlyInLocalAssembly);
            FillInformation(content, "Workflows ONLY in Local Assembly {0}", workflowOnlyInLocalAssembly);

            if (!pluginsOnlyInCrm.Any()
                && !workflowOnlyInCrm.Any()
                && !pluginsOnlyInLocalAssembly.Any()
                && !workflowOnlyInLocalAssembly.Any()
                )
            {
                content.AppendLine().AppendLine().AppendLine();

                content.AppendLine("No difference in Assemblies.");
            }

            FillInformation(content, "Plugins in Crm Assembly {0}", crmPlugins);
            FillInformation(content, "Workflows in Crm Assembly {0}", crmWorkflows);
            FillInformation(content, "Plugins in Local Assembly {0}", assemblyPlugins);
            FillInformation(content, "Workflows in Local Assembly {0}", assemblyWorkflows);

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assemblyName, "Comparing", "txt");
            filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Assembly {0} Comparing exported to {1}", assemblyName, filePath);
            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            return filePath;
        }

        private static void FillInformation(StringBuilder content, string format, IEnumerable<string> collection)
        {
            if (collection.Any())
            {
                content.AppendLine().AppendLine().AppendLine();
                content.AppendLine(new string('-', 150));
                content.AppendLine().AppendLine().AppendLine();

                content.AppendFormat(format, collection.Count()).AppendLine();
                foreach (var item in collection.OrderBy(s => s))
                {
                    content.AppendLine(_tabspacer + item);
                }
            }
        }

        #endregion Сравнение сборки плагинов и локальной сборки.

        #region Добавление шага плагина.

        public async Task ExecuteAddingPluginStepForType(ConnectionData connectionData, CommonConfiguration commonConfig, string pluginTypeName)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginStepFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginStepForType(connectionData, commonConfig, pluginTypeName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task AddingPluginStepForType(ConnectionData connectionData, CommonConfiguration commonConfig, string pluginTypeName)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (string.IsNullOrEmpty(pluginTypeName))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "PluginType Name is empty.");
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new PluginTypeRepository(service);

            var pluginType = await repository.FindPluginTypeAsync(pluginTypeName);

            if (pluginType == null)
            {
                pluginType = await repository.FindPluginTypeByLikeNameAsync(pluginTypeName);
            }

            if (pluginType == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "PluginType not founded by name {0}.", pluginTypeName);

                WindowHelper.OpenPluginTypeWindow(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , pluginTypeName
                );

                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.GettingMessages);

            var repositoryFilters = new SdkMessageFilterRepository(service);

            List<SdkMessageFilter> filters = await repositoryFilters.GetAllAsync(new ColumnSet(SdkMessageFilter.Schema.Attributes.sdkmessageid, SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, SdkMessageFilter.Schema.Attributes.availability));

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.GettingMessagesCompleted);

            var step = new SdkMessageProcessingStep()
            {
                EventHandler = pluginType.ToEntityReference(),
            };

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowSdkMessageProcessingStep(_iWriteToOutput, service, filters, step);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Добавление шага плагина.

        #region Обновление сборки плагинов.

        public async Task ExecuteUpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.Project project, string defaultOutputFilePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingPluginAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingPluginAssembly(connectionData, commonConfig, project, defaultOutputFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task UpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.Project project, string defaultOutputFilePath)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (project == null || string.IsNullOrEmpty(project.Name))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositoryAssembly = new PluginAssemblyRepository(service);

            var assembly = await repositoryAssembly.FindAssemblyAsync(project.Name);

            if (assembly == null)
            {
                assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(project.Name);
            }

            if (assembly == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, project.Name);

                WindowHelper.OpenPluginAssemblyWindow(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , project.Name
                    );

                return;
            }

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginAssembly(_iWriteToOutput, service, assembly, defaultOutputFilePath, project);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Обновление сборки плагинов.

        #region Регистрация сборки плагинов.

        public async Task ExecuteRegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.Project project, string defaultOutputFilePath)
        {
            string operation = string.Format(Properties.OperationNames.RegisteringPluginAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await RegisterPluginAssembly(connectionData, commonConfig, project, defaultOutputFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task RegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.Project project, string defaultOutputFilePath)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (project == null || string.IsNullOrEmpty(project.Name))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var assembly = new PluginAssembly();

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginAssembly(_iWriteToOutput, service, assembly, defaultOutputFilePath, project);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Регистрация сборки плагинов.
    }
}