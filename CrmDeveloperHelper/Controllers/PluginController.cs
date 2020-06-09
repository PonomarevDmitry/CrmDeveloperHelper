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
            if (string.IsNullOrEmpty(projectName))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);

            var assembly = await repositoryAssembly.FindAssemblyAsync(projectName);

            if (assembly == null)
            {
                assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(projectName);
            }

            if (assembly == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, projectName);

                WindowHelper.OpenPluginAssemblyExplorer(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , projectName
                    );

                return;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

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
                    content.AppendLine(_tabSpacer + item);
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

        public async Task ExecuteUpdatingPluginAssembliesInWindow(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingPluginAssemblyInWindowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

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
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task UpdatingPluginAssembliesInWindow(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);

            foreach (var project in projectList)
            {
                var assembly = await repositoryAssembly.FindAssemblyAsync(project.Name);

                if (assembly == null)
                {
                    assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(project.Name);
                }

                if (assembly == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, project.Name);

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

        public async Task ExecuteBuildingProjectAndUpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList, bool registerPlugins)
        {
            string operation = string.Format(registerPlugins ? Properties.OperationNames.BuildingProjectAndUpdatingPluginAssemblyFormat1 : Properties.OperationNames.BuildingProjectUpdatingPluginAssemblyRegisteringPluginsFormat1
                , connectionData?.Name
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

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
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task BuildingProjectAndUpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList, bool registerPlugins)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);
            var repositoryType = new PluginTypeRepository(service);

            foreach (var project in projectList)
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
            var assembly = await repositoryAssembly.FindAssemblyAsync(project.Name);

            if (assembly == null)
            {
                assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(project.Name);
            }

            if (assembly == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, project.Name);

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

            var pluginTypes = await repositoryType.GetPluginTypesAsync(assembly.Id);

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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CannotUpdatePluginAssemblyFormat1, assembly.Name);

                return;
            }

            string workflowActivityGroupName = string.Format("{0} ({1})", assemblyLoad.Name, assemblyLoad.Version);

            service.ConnectionData.AddAssemblyMapping(assemblyLoad.Name, assemblyLoad.FilePath);
            service.ConnectionData.Save();

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.UpdatingPluginAssemblyFormat1, service.ConnectionData.Name);

            assembly.Content = Convert.ToBase64String(assemblyLoad.Content);

            try
            {
                await service.UpdateAsync(assembly);

                if (registerPlugins)
                {
                    var pluginsOnlyInLocalAssembly = assemblyPlugins.Except(crmPlugins, StringComparer.InvariantCultureIgnoreCase);
                    var workflowOnlyInLocalAssembly = assemblyWorkflows.Except(crmWorkflows, StringComparer.InvariantCultureIgnoreCase);

                    if (pluginsOnlyInLocalAssembly.Any() || workflowOnlyInLocalAssembly.Any())
                    {
                        int totalCount = pluginsOnlyInLocalAssembly.Count() + workflowOnlyInLocalAssembly.Count();

                        var assemblyRef = assembly.ToEntityReference();

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.RegisteringNewPluginTypesFormat2, service.ConnectionData.Name, totalCount);

                        await RegisterNewPluginTypes(service, pluginsOnlyInLocalAssembly, assemblyRef, false, workflowActivityGroupName);

                        await RegisterNewPluginTypes(service, workflowOnlyInLocalAssembly, assemblyRef, true, workflowActivityGroupName);

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.RegisteringNewPluginTypesCompletedFormat2, service.ConnectionData.Name, totalCount);
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.UpdatingPluginAssemblyFailedFormat1, service.ConnectionData.Name);

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
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.RegisteringPluginTypeFailedFormat2, service.ConnectionData.Name, pluginType);

                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }
        }

        #endregion Building Project, Updating Assembly and Register New Plugins

        #region Регистрация сборки плагинов.

        public async Task ExecuteRegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.RegisteringPluginAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

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
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task RegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
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
        }

        #endregion Регистрация сборки плагинов.

        #region Opening PluginAssembly

        public async Task ExecuteOpeningProjectPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList, ActionOnComponent actionOpen)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , PluginAssembly.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOpen)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningProjectPluginAssembly(connectionData, commonConfig, projectList, actionOpen);
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

        private async Task OpeningProjectPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList, ActionOnComponent actionOpen)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repositoryAssembly = new PluginAssemblyRepository(service);

            foreach (var project in projectList)
            {
                var assembly = await repositoryAssembly.FindAssemblyAsync(project.Name);

                if (assembly == null)
                {
                    assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(project.Name);
                }

                if (assembly == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, project.Name);

                    WindowHelper.OpenPluginAssemblyExplorer(
                        this._iWriteToOutput
                        , service
                        , commonConfig
                        , project.Name
                    );

                    continue;
                }

                if (actionOpen == ActionOnComponent.OpenDependentComponentsInWeb)
                {
                    connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.PluginAssembly, assembly.Id);
                }
                else if (actionOpen == ActionOnComponent.OpenDependentComponentsInExplorer)
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
                else if (actionOpen == ActionOnComponent.OpenSolutionsContainingComponentInExplorer)
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
            }
        }

        #endregion Opening PluginAssembly

        #region Creating PluginAssembly EntityDescription

        public async Task ExecuteCreatingPluginAssemblyEntityDescription(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.CreatingPluginAssemblyEntityDescriptionFormat1
                , connectionData?.Name
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CreatingPluginAssemblyEntityDescription(connectionData, commonConfig, projectList);
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

        private async Task CreatingPluginAssemblyEntityDescription(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            var repositoryAssembly = new PluginAssemblyRepository(service);

            foreach (var project in projectList)
            {
                var assembly = await repositoryAssembly.FindAssemblyAsync(project.Name);

                if (assembly == null)
                {
                    assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(project.Name);
                }

                if (assembly == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, project.Name);

                    WindowHelper.OpenPluginAssemblyExplorer(
                        this._iWriteToOutput
                        , service
                        , commonConfig
                        , project.Name
                    );

                    continue;
                }

                string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assembly.Name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, assembly, EntityFileNameFormatter.PluginAssemblyIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , assembly.LogicalName
                    , filePath
                );

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        #endregion Creating PluginAssembly EntityDescription

        #region Creating PluginAssembly Description

        public async Task ExecuteCreatingPluginAssemblyDescription(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            string operation = string.Format(Properties.OperationNames.CreatingPluginAssemblyDescriptionFormat1
                , connectionData?.Name
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CreatingPluginAssemblyDescription(connectionData, commonConfig, projectList);
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

        private async Task CreatingPluginAssemblyDescription(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AssemblyNameIsEmpty);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            var repositoryAssembly = new PluginAssemblyRepository(service);

            PluginAssemblyDescriptionHandler handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

            foreach (var project in projectList)
            {
                var assembly = await repositoryAssembly.FindAssemblyAsync(project.Name);

                if (assembly == null)
                {
                    assembly = await repositoryAssembly.FindAssemblyByLikeNameAsync(project.Name);
                }

                if (assembly == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssemblyNotFoundedByNameFormat1, project.Name);

                    WindowHelper.OpenPluginAssemblyExplorer(
                        this._iWriteToOutput
                        , service
                        , commonConfig
                        , project.Name
                    );

                    continue;
                }

                string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assembly.Name, "Description", "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await handler.CreateFileWithDescriptionAsync(filePath, assembly.Id, assembly.Name, DateTime.Now);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginAssembly.Schema.EntityLogicalName, assembly.Name, "Description", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        #endregion Creating PluginAssembly Description
    }
}