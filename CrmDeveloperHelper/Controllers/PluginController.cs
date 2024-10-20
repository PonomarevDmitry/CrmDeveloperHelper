﻿using Microsoft.Xrm.Sdk;
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
    public class PluginController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PluginController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        public async Task<string> SelecteFileCreateFileWithAssemblyComparing(string folder, IOrganizationServiceExtented service, Guid idPluginAssembly, string assemblyName, string defaultOutputFilePath)
        {
            var repositoryType = new PluginTypeRepository(service);

            var taskGetPluginTypes = repositoryType.GetPluginTypesAsync(idPluginAssembly);

            string assemblyPath = service.ConnectionData.GetLastAssemblyPath(assemblyName);
            List<string> lastPaths = service.ConnectionData.GetAssemblyPaths(assemblyName).ToList();

            if (!string.IsNullOrEmpty(defaultOutputFilePath)
                && !lastPaths.Contains(defaultOutputFilePath, StringComparer.InvariantCultureIgnoreCase)
            )
            {
                lastPaths.Insert(0, defaultOutputFilePath);
            }

            bool isSuccess = false;

            var thread = new Thread(() =>
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

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            thread.Join();

            if (!isSuccess)
            {
                return string.Empty;
            }

            return await CreateFileWithAssemblyComparing(folder, service, assemblyName, taskGetPluginTypes, assemblyPath);
        }

        public async Task<string> CreateFileWithAssemblyComparing(string folder, IOrganizationServiceExtented service, string assemblyName, Task<List<PluginType>> taskGetPluginTypes, string assemblyPath)
        {
            service.ConnectionData.AddAssemblyMapping(assemblyName, assemblyPath);
            service.ConnectionData.Save();

            var pluginTypes = await taskGetPluginTypes;

            AssemblyReaderResult assemblyCheck = null;

            using (var reader = new AssemblyReader())
            {
                assemblyCheck = reader.ReadAssembly(assemblyPath);
            }

            if (assemblyCheck == null)
            {
                return string.Empty;
            }

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



            FillInformation(content, "PluginTypes ONLY in Crm Assembly {0}", pluginsOnlyInCrm);
            FillInformation(content, "WorkflowCodeActivities ONLY in Crm Assembly {0}", workflowOnlyInCrm);
            FillInformation(content, "PluginTypes ONLY in Local Assembly {0}", pluginsOnlyInLocalAssembly);
            FillInformation(content, "WorkflowCodeActivities ONLY in Local Assembly {0}", workflowOnlyInLocalAssembly);

            if (!pluginsOnlyInCrm.Any()
                && !workflowOnlyInCrm.Any()
                && !pluginsOnlyInLocalAssembly.Any()
                && !workflowOnlyInLocalAssembly.Any()
                )
            {
                content.AppendLine().AppendLine().AppendLine();

                content.AppendLine("No difference in Assemblies.");
            }

            FillInformation(content, "PluginTypes in Crm Assembly {0}", crmPlugins);
            FillInformation(content, "WorkflowCodeActivities in Crm Assembly {0}", crmWorkflows);
            FillInformation(content, "PluginTypes in Local Assembly {0}", assemblyPlugins);
            FillInformation(content, "WorkflowCodeActivities in Local Assembly {0}", assemblyWorkflows);

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assemblyName, "Comparing", FileExtension.txt);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

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
                    content.AppendLine(_tabSpacer + item);
                }
            }
        }

        private void WriteToOutputProjectList(ConnectionData connectionData, IEnumerable<EnvDTE.Project> projectList)
        {
            foreach (var project in projectList.Where(p => !string.IsNullOrEmpty(p.Name)).OrderBy(p => p.Name))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, _tabSpacer + project.Name);
            }
        }

        #region Comparing Local Assembly and PluginAssembly by PluginTypes.

        public async Task ExecuteComparingPluginTypesLocalAssemblyAndPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.ComparingCrmPluginAssemblyAndLocalAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            WriteToOutputProjectList(connectionData, projectList);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

            try
            {
                await ComparingPluginTypesLocalAssemblyAndPluginAssembly(connectionData, commonConfig, projectList);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                WriteToOutputProjectList(connectionData, projectList);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ComparingPluginTypesLocalAssemblyAndPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);
            var repositoryType = new PluginTypeRepository(service);

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            foreach (var project in projectList)
            {
                string operation = string.Format(
                    Properties.OperationNames.BuildingProjectAndComparingCrmPluginAssemblyFormat2
                    , connectionData?.Name
                    , project.Name
                );

                this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

                try
                {
                    string projectAssemblyName = PropertiesHelper.GetAssemblyName(project);

                    var pluginAssembly = await repositoryAssembly.FindAssemblyAsync(projectAssemblyName);

                    if (pluginAssembly == null)
                    {
                        pluginAssembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectAssemblyName);
                    }

                    if (pluginAssembly == null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectAssemblyName);

                        WindowHelper.OpenPluginAssemblyExplorer(
                            this._iWriteToOutput
                            , service
                            , commonConfig
                            , project.Name
                        );

                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectFormat1, project.Name);

                    var buildResult = await _iWriteToOutput.BuildProjectAsync(project);

                    if (buildResult != 0)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectFailedFormat1, project.Name);
                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectCompletedFormat1, project.Name);

                    string defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

                    if (!File.Exists(defaultOutputFilePath))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, defaultOutputFilePath);
                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.LoadingAssemblyFromPathFormat1, defaultOutputFilePath);

                    var taskGetPluginTypes = repositoryType.GetPluginTypesAsync(pluginAssembly.Id);

                    string filePath = await CreateFileWithAssemblyComparing(commonConfig.FolderForExport, service, pluginAssembly.Name, taskGetPluginTypes, defaultOutputFilePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
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

            service.TryDispose();
        }

        #endregion Comparing Local Assembly and PluginAssembly by PluginTypes.

        #region Comparing Local Assembly and PluginAssembly by byte array.

        public async Task ExecuteComparingByteArrayLocalAssemblyAndPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.ComparingByteArrayLocalAssemblyAndPluginAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            WriteToOutputProjectList(connectionData, projectList);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

            try
            {
                await ComparingByteArrayLocalAssemblyAndPluginAssembly(connectionData, commonConfig, projectList);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                WriteToOutputProjectList(connectionData, projectList);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ComparingByteArrayLocalAssemblyAndPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            var notEqualByContent = new List<string>();

            foreach (var project in projectList)
            {
                string operation = string.Format(
                    Properties.OperationNames.BuildingProjectAndComparingCrmPluginAssemblyFormat2
                    , connectionData?.Name
                    , project.Name
                );

                this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

                try
                {
                    string projectAssemblyName = PropertiesHelper.GetAssemblyName(project);

                    var pluginAssembly = await repositoryAssembly.FindAssemblyAsync(projectAssemblyName);

                    if (pluginAssembly == null)
                    {
                        pluginAssembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectAssemblyName);
                    }

                    if (pluginAssembly == null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectAssemblyName);

                        WindowHelper.OpenPluginAssemblyExplorer(
                            this._iWriteToOutput
                            , service
                            , commonConfig
                            , project.Name
                        );

                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectFormat1, project.Name);

                    var buildResult = await _iWriteToOutput.BuildProjectAsync(project);

                    if (buildResult != 0)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectFailedFormat1, project.Name);
                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectCompletedFormat1, project.Name);

                    string defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

                    if (!File.Exists(defaultOutputFilePath))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, defaultOutputFilePath);
                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.LoadingAssemblyFromPathFormat1, defaultOutputFilePath);

                    byte[] outputFileByteArray = File.ReadAllBytes(defaultOutputFilePath);
                    string outputFileBase64String = Convert.ToBase64String(outputFileByteArray);

                    var pluginAssemblyContent = await repositoryAssembly.GetAssemblyByIdRetrieveRequestAsync(pluginAssembly.Id, new ColumnSet(PluginAssembly.Schema.Attributes.content));

                    if (pluginAssemblyContent.Content != outputFileBase64String)
                    {
                        notEqualByContent.Add(pluginAssembly.Name);
                    }
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

            if (notEqualByContent.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "PluginAssemblies different from Local Assemblies: {0}", notEqualByContent.Count);

                foreach (var item in notEqualByContent)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, _tabSpacer + item);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "PluginAssemblies and Local Assemblies are equal by byte array.");
            }

            service.TryDispose();
        }

        #endregion Comparing Local Assembly and PluginAssembly by byte array.

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
            if (string.IsNullOrEmpty(pluginTypeName))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "PluginType Name is empty.");
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new PluginTypeRepository(service);

            var pluginType = await repository.FindPluginTypeAsync(pluginTypeName);

            if (pluginType == null)
            {
                pluginType = await repository.FindPluginTypeByLikeNameAsync(pluginTypeName);
            }

            if (pluginType == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "PluginType not founded by name {0}.", pluginTypeName);

                WindowHelper.OpenPluginTypeExplorer(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , pluginTypeName
                );

                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingMessages);

            var repositoryFilters = new SdkMessageFilterRepository(service);

            List<SdkMessageFilter> filters = await repositoryFilters.GetAllAsync(new ColumnSet(SdkMessageFilter.Schema.Attributes.sdkmessageid, SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, SdkMessageFilter.Schema.Attributes.availability));

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingMessagesCompleted);

            var step = new SdkMessageProcessingStep()
            {
                EventHandler = pluginType.ToEntityReference(),
            };

            var thread = new System.Threading.Thread(() =>
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

            thread.SetApartmentState(System.Threading.ApartmentState.STA);

            thread.Start();
        }

        #endregion Добавление шага плагина.

        #region Обновление сборки плагинов.

        public async Task ExecuteUpdatingPluginAssembliesInWindow(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingPluginAssemblyInWindowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            WriteToOutputProjectList(connectionData, projectList);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

            try
            {
                await UpdatingPluginAssembliesInWindow(connectionData, commonConfig, projectList);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                WriteToOutputProjectList(connectionData, projectList);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task UpdatingPluginAssembliesInWindow(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);

            foreach (var project in projectList.Where(p => !string.IsNullOrEmpty(p.Name)).OrderBy(p => p.Name))
            {
                string projectAssemblyName = PropertiesHelper.GetAssemblyName(project);

                var assembly = await repositoryAssembly.FindAssemblyAsync(projectAssemblyName);

                if (assembly == null)
                {
                    assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectAssemblyName);
                }

                if (assembly == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectAssemblyName);

                    WindowHelper.OpenPluginAssemblyExplorer(
                        this._iWriteToOutput
                        , service
                        , commonConfig
                        , project.Name
                    );
                }
                else
                {
                    WindowHelper.OpenPluginAssemblyUpdateWindow(
                        this._iWriteToOutput
                        , service
                        , assembly
                        , project
                    );
                }
            }
        }

        #endregion Обновление сборки плагинов.

        #region Building Project, Updating Assembly and Register New Plugins

        public async Task ExecuteBuildingProjectAndUpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList, bool registerPlugins)
        {
            string formatOperation = registerPlugins ? Properties.OperationNames.BuildingProjectAndUpdatingPluginAssemblyFormat1 : Properties.OperationNames.BuildingProjectUpdatingPluginAssemblyRegisteringPluginsFormat1;

            string operation = string.Format(formatOperation, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            WriteToOutputProjectList(connectionData, projectList);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

            try
            {
                await BuildingProjectAndUpdatingPluginAssembly(connectionData, commonConfig, projectList, registerPlugins);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                WriteToOutputProjectList(connectionData, projectList);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task BuildingProjectAndUpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList, bool registerPlugins)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);
            var repositoryType = new PluginTypeRepository(service);

            foreach (var project in projectList.Where(p => !string.IsNullOrEmpty(p.Name)).OrderBy(p => p.Name))
            {
                string operation = string.Format(registerPlugins ? Properties.OperationNames.BuildingProjectAndUpdatingPluginAssemblyFormat2 : Properties.OperationNames.BuildingProjectUpdatingPluginAssemblyRegisteringPluginsFormat2
                    , connectionData?.Name
                    , project.Name
                );

                this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

                try
                {
                    await BuildProjectUpdatePluginAssembly(connectionData, commonConfig, service, repositoryAssembly, repositoryType, project, registerPlugins);
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

            service.TryDispose();
        }

        private async Task BuildProjectUpdatePluginAssembly(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , PluginAssemblyRepository repositoryAssembly
            , PluginTypeRepository repositoryType
            , EnvDTE.Project project
            , bool registerPlugins
        )
        {
            string projectAssemblyName = PropertiesHelper.GetAssemblyName(project);

            var pluginAssembly = await repositoryAssembly.FindAssemblyAsync(projectAssemblyName);

            if (pluginAssembly == null)
            {
                pluginAssembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectAssemblyName);
            }

            if (pluginAssembly == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectAssemblyName);

                WindowHelper.OpenPluginAssemblyExplorer(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , project.Name
                );

                return;
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectFormat1, project.Name);

            var buildResult = await _iWriteToOutput.BuildProjectAsync(project);

            if (buildResult != 0)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectFailedFormat1, project.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.BuildingProjectCompletedFormat1, project.Name);

            string defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

            if (!File.Exists(defaultOutputFilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, defaultOutputFilePath);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.LoadingAssemblyFromPathFormat1, defaultOutputFilePath);

            AssemblyReaderResult assemblyLoad = null;

            using (var reader = new AssemblyReader())
            {
                assemblyLoad = reader.ReadAssembly(defaultOutputFilePath);
            }

            if (assemblyLoad == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.LoadingAssemblyFromPathFailedFormat1, defaultOutputFilePath);
                return;
            }

            assemblyLoad.Content = File.ReadAllBytes(defaultOutputFilePath);

            var crmPlugins = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var crmWorkflows = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            var pluginTypes = await repositoryType.GetPluginTypesAsync(pluginAssembly.Id);

            foreach (var item in pluginTypes.Where(e => !e.IsWorkflowActivity.GetValueOrDefault()).Select(e => e.TypeName))
            {
                crmPlugins.Add(item);
            }

            foreach (var item in pluginTypes.Where(e => e.IsWorkflowActivity.GetValueOrDefault()).Select(e => e.TypeName))
            {
                crmWorkflows.Add(item);
            }

            HashSet<string> assemblyPlugins = new HashSet<string>(assemblyLoad.Plugins, StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> assemblyWorkflows = new HashSet<string>(assemblyLoad.Workflows, StringComparer.InvariantCultureIgnoreCase);

            var pluginsOnlyInCrm = crmPlugins.Except(assemblyPlugins, StringComparer.InvariantCultureIgnoreCase).ToList();
            var workflowOnlyInCrm = crmWorkflows.Except(assemblyWorkflows, StringComparer.InvariantCultureIgnoreCase).ToList();

            if (pluginsOnlyInCrm.Any() || workflowOnlyInCrm.Any())
            {
                if (pluginsOnlyInCrm.Any())
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginTypesExistsOnlyInCRMFormat1, pluginsOnlyInCrm.Count);

                    foreach (var item in pluginsOnlyInCrm.OrderBy(s => s))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, _tabSpacer + item);
                    }
                }

                if (workflowOnlyInCrm.Any())
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WorkflowTypesExistsOnlyInCRMFormat1, workflowOnlyInCrm.Count);

                    foreach (var item in workflowOnlyInCrm.OrderBy(s => s))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, _tabSpacer + item);
                    }
                }

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CannotUpdatePluginAssemblyFormat1, pluginAssembly.Name);

                return;
            }

            string workflowActivityGroupName = string.Format("{0} ({1})", assemblyLoad.Name, assemblyLoad.Version);

            service.ConnectionData.AddAssemblyMapping(assemblyLoad.Name, assemblyLoad.FilePath);
            service.ConnectionData.Save();

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionUpdatingPluginAssemblyFormat2, service.ConnectionData.Name, pluginAssembly.Name);

            pluginAssembly.Content = Convert.ToBase64String(assemblyLoad.Content);

            try
            {
                await service.UpdateAsync(pluginAssembly);

                if (registerPlugins)
                {
                    var pluginsOnlyInLocalAssembly = assemblyPlugins.Except(crmPlugins, StringComparer.InvariantCultureIgnoreCase);
                    var workflowOnlyInLocalAssembly = assemblyWorkflows.Except(crmWorkflows, StringComparer.InvariantCultureIgnoreCase);

                    if (pluginsOnlyInLocalAssembly.Any() || workflowOnlyInLocalAssembly.Any())
                    {
                        int totalCount = pluginsOnlyInLocalAssembly.Count() + workflowOnlyInLocalAssembly.Count();

                        var assemblyRef = pluginAssembly.ToEntityReference();

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionRegisteringNewPluginTypesFormat2, service.ConnectionData.Name, totalCount);

                        await RegisterNewPluginTypes(service, pluginsOnlyInLocalAssembly, assemblyRef, false, workflowActivityGroupName);

                        await RegisterNewPluginTypes(service, workflowOnlyInLocalAssembly, assemblyRef, true, workflowActivityGroupName);

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionRegisteringNewPluginTypesCompletedFormat2, service.ConnectionData.Name, totalCount);
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionUpdatingPluginAssemblyFailedFormat2, service.ConnectionData.Name, pluginAssembly.Name);

                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }
        }

        private async Task RegisterNewPluginTypes(IOrganizationServiceExtented service, IEnumerable<string> typesToRegister, EntityReference assemblyRef, bool isWorkflowActivity, string workflowActivityGroupName)
        {
            foreach (var pluginType in typesToRegister.OrderBy(s => s))
            {
                var pluginTypeEntity = new PluginType()
                {
                    Name = pluginType,
                    TypeName = pluginType,
                    FriendlyName = pluginType,

                    PluginAssemblyId = assemblyRef,
                };

                if (isWorkflowActivity)
                {
                    pluginTypeEntity.WorkflowActivityGroupName = workflowActivityGroupName;
                }

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, pluginType);

                try
                {
                    pluginTypeEntity.Id = await service.CreateAsync(pluginTypeEntity);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionRegisteringPluginTypeFailedFormat2, service.ConnectionData.Name, pluginType);

                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }
        }

        #endregion Building Project, Updating Assembly and Register New Plugins

        #region Регистрация сборки плагинов.

        public async Task ExecuteRegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.RegisteringPluginAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            WriteToOutputProjectList(connectionData, projectList);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

            try
            {
                await RegisterPluginAssembly(connectionData, commonConfig, projectList);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                WriteToOutputProjectList(connectionData, projectList);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task RegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            foreach (var project in projectList)
            {
                var assembly = new PluginAssembly();

                WindowHelper.OpenPluginAssemblyUpdateWindow(
                    this._iWriteToOutput
                    , service
                    , assembly
                    , project
                );
            }

            service.TryDispose();
        }

        #endregion Регистрация сборки плагинов.

        #region Action on PluginAssembly

        public async Task ExecuteActionOnProjectPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList, ActionOnComponent actionOnComponent)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , PluginAssembly.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            WriteToOutputProjectList(connectionData, projectList);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

            try
            {
                await ExecutingActionOnProjectPluginAssembly(connectionData, commonConfig, projectList, actionOnComponent);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                WriteToOutputProjectList(connectionData, projectList);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ExecutingActionOnProjectPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList, ActionOnComponent actionOnComponent)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

            var repositoryAssembly = new PluginAssemblyRepository(service);

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            foreach (var project in projectList)
            {
                string projectAssemblyName = PropertiesHelper.GetAssemblyName(project);

                var assembly = await repositoryAssembly.FindAssemblyAsync(projectAssemblyName);

                if (assembly == null)
                {
                    assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectAssemblyName);
                }

                if (assembly == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectAssemblyName);

                    WindowHelper.OpenPluginAssemblyExplorer(
                        this._iWriteToOutput
                        , service
                        , commonConfig
                        , project.Name
                    );

                    continue;
                }

                if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
                {
                    connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.PluginAssembly, assembly.Id);
                }
                else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInExplorer)
                {
                    WindowHelper.OpenSolutionComponentDependenciesExplorer(
                        _iWriteToOutput
                        , service
                        , null
                        , commonConfig
                        , (int)ComponentType.PluginAssembly
                        , assembly.Id
                        , null
                    );
                }
                else if (actionOnComponent == ActionOnComponent.OpenSolutionsListWithComponentInExplorer)
                {
                    WindowHelper.OpenExplorerSolutionExplorer(
                        _iWriteToOutput
                        , service
                        , commonConfig
                        , (int)ComponentType.PluginAssembly
                        , assembly.Id
                        , null
                    );
                }
                else if (actionOnComponent == ActionOnComponent.EntityDescription)
                {
                    string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assembly.Name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, assembly, service.ConnectionData);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                        , service.ConnectionData.Name
                        , assembly.LogicalName
                        , filePath
                    );

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else if (actionOnComponent == ActionOnComponent.Description)
                {
                    string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assembly.Name, "Description", FileExtension.txt);
                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await handler.CreateFileWithDescriptionAsync(filePath, assembly.Id, assembly.Name, DateTime.Now);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, PluginAssembly.Schema.EntityLogicalName, assembly.Name, "Description", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
            }

            if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb
                || actionOnComponent == ActionOnComponent.EntityDescription
                || actionOnComponent == ActionOnComponent.Description
                || actionOnComponent == ActionOnComponent.SingleXmlField
            )
            {
                service.TryDispose();
            }
        }

        #endregion Action on PluginAssembly

        #region Action on PluginType

        public async Task ExecuteActionOnPluginTypes(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> pluginTypeNames, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , PluginType.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ExecutingActionOnPluginTypes(connectionData, commonConfig, pluginTypeNames, actionOnComponent, fieldName, fieldTitle);
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

        private async Task ExecutingActionOnPluginTypes(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> pluginTypeNames, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any(p => !string.IsNullOrEmpty(p)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginTypesNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new PluginTypeRepository(service);

            var knownPluginTypes = new Dictionary<Guid, PluginType>();

            var unknownPluginTypes = new List<string>();

            foreach (var pluginTypeName in pluginTypeNames)
            {
                var pluginType = await repository.FindPluginTypeByLikeNameAsync(pluginTypeName);

                if (pluginType != null)
                {
                    if (!knownPluginTypes.ContainsKey(pluginType.Id))
                    {
                        knownPluginTypes.Add(pluginType.Id, pluginType);
                    }
                }
                else
                {
                    unknownPluginTypes.Add(pluginTypeName);
                }
            }

            if (!knownPluginTypes.Any() && !unknownPluginTypes.Any())
            {
                service.TryDispose();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginTypesFounded);
                return;
            }

            OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);

            if (!knownPluginTypes.Any())
            {
                service.TryDispose();
                return;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            var repStep = new SdkMessageProcessingStepRepository(service);
            var repImage = new SdkMessageProcessingStepImageRepository(service);
            var repSecure = new SdkMessageProcessingStepSecureConfigRepository(service);

            var listSecure = await repSecure.GetAllSdkMessageProcessingStepSecureConfigAsync();

            foreach (var pluginType in knownPluginTypes.Values.OrderBy(p => p.TypeName))
            {
                if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
                {
                    connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.PluginType, pluginType.Id);
                }
                else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInExplorer)
                {
                    WindowHelper.OpenSolutionComponentDependenciesExplorer(
                        _iWriteToOutput
                        , service
                        , null
                        , commonConfig
                        , (int)ComponentType.PluginType
                        , pluginType.Id
                        , null
                    );
                }
                else if (actionOnComponent == ActionOnComponent.OpenSolutionsListWithComponentInExplorer)
                {
                    WindowHelper.OpenExplorerSolutionExplorer(
                        _iWriteToOutput
                        , service
                        , commonConfig
                        , (int)ComponentType.PluginType
                        , pluginType.Id
                        , null
                    );
                }
                else if (actionOnComponent == ActionOnComponent.EntityDescription)
                {
                    string fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, pluginType, service.ConnectionData);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                        , service.ConnectionData.Name
                        , pluginType.LogicalName
                        , filePath
                    );

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else if (actionOnComponent == ActionOnComponent.Description)
                {
                    string fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, "Description");
                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    var allSteps = await repStep.GetAllStepsByPluginTypeAsync(pluginType.Id);
                    var queryImage = await repImage.GetImagesByPluginTypeAsync(pluginType.Id);

                    bool hasDescription = await PluginTypeDescriptionHandler.CreateFileWithDescriptionAsync(
                        service.ConnectionData.GetConnectionInfo()
                        , filePath
                        , pluginType.Id
                        , pluginType.TypeName
                        , allSteps
                        , queryImage
                        , listSecure
                    );

                    if (hasDescription)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, PluginType.EntitySchemaName, pluginType.TypeName, "Description", filePath);
                        this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, PluginType.EntitySchemaName, pluginType.TypeName, "Description");
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    }
                }
                else if (actionOnComponent == ActionOnComponent.SingleXmlField)
                {
                    string xmlContent = pluginType.GetAttributeValue<string>(fieldName);

                    if (!string.IsNullOrEmpty(xmlContent))
                    {
                        try
                        {
                            string fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, fieldTitle, FileExtension.xml);
                            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                            xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                                xmlContent
                                , commonConfig
                                , XmlOptionsControls.PluginTypeCustomWorkflowActivityInfoXmlOptions
                                , schemaName: Commands.AbstractDynamicCommandXsdSchemas.PluginTypeCustomWorkflowActivityInfoSchema
                            );

                            File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, PluginType.Schema.EntitySchemaName, pluginType.TypeName, fieldTitle, filePath);

                            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                        }
                        catch (Exception ex)
                        {
                            this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, PluginType.Schema.EntitySchemaName, pluginType.TypeName, fieldTitle);
                        this._iWriteToOutput.ActivateOutputWindow(connectionData);
                    }
                }
            }

            if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb
                || actionOnComponent == ActionOnComponent.EntityDescription
                || actionOnComponent == ActionOnComponent.Description
                || actionOnComponent == ActionOnComponent.SingleXmlField
            )
            {
                service.TryDispose();
            }
        }

        #endregion Action on PluginType
    }
}