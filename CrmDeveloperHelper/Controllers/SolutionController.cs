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
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class SolutionController
    {
        private const string _tabSpacer = "      ";

        private readonly IWriteToOutput _iWriteToOutput = null;

        public SolutionController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Solution Explorer.

        public async Task ExecuteOpeningSolutionExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
        {
            string operation = string.Format(Properties.OperationNames.SolutionExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSolutionExlorerWindow(selectedItem, connectionData, commonConfig);
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

        private async Task OpeningSolutionExlorerWindow(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
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

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , commonConfig
                , null
                , null
                , selectedItem
            );
        }

        #endregion Solution Explorer.

        #region ImportJob Explorer.

        public async Task ExecuteOpeningImportJobExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ImportJobExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningImportJobExlorerWindow(connectionData, commonConfig);
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

        private async Task OpeningImportJobExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
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

            WindowHelper.OpenExplorerImportJobWindow(
                _iWriteToOutput
                , service
                , commonConfig
                , null
            );
        }

        #endregion ImportJob Explorer.

        #region Окна с образами.

        public void ExecuteOpeningSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSolutionImageWindowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                OpeningSolutionImageWindow(connectionData, commonConfig);
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

        private void OpeningSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        public void ExecuteOpeningSolutionDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, Properties.OperationNames.ShowingSolutionDifferenceImageWindow);

            try
            {
                OpeningSolutionDifferenceImageWindow(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, Properties.OperationNames.ShowingSolutionDifferenceImageWindow);
            }
        }

        private void OpeningSolutionDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            WindowHelper.OpenSolutionDifferenceImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        public void ExecuteOpeningOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, Properties.OperationNames.ShowingOrganizationDifferenceImageWindow);

            try
            {
                OpeningOrganizationDifferenceImageWindow(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, Properties.OperationNames.ShowingOrganizationDifferenceImageWindow);
            }
        }

        private void OpeningOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        #endregion Окна с образами.

        #region Добавление веб-ресурса в решение.

        public async Task ExecuteAddingWebResourcesToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingWebResourcesToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingWebResourcesToSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
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

        private async Task AddingWebResourcesToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (selectedFiles == null || !selectedFiles.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
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

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var dictForAdding = new Dictionary<Guid, WebResource>();

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names);

                foreach (var selectedFile in gr)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                        continue;
                    }
                    
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.TryingToFindWebResourceByNameFormat1, selectedFile.Name);

                    string key = selectedFile.FriendlyFilePath.ToLower();

                    var contentFile = Convert.ToBase64String(File.ReadAllBytes(selectedFile.FilePath));

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, key, gr.Key);

                    if (webresource != null)
                    {
                        var webName = webresource.Name;

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceFoundedByNameFormat2, webresource.Id, webName);
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceNotFoundedByNameFormat1, selectedFile.Name);

                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.GetByIdAsync(webId.Value);
                        }
                    }

                    if (webresource != null)
                    {
                        // Запоминается файл
                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        if (!dictForAdding.ContainsKey(webresource.Id))
                        {
                            dictForAdding.Add(webresource.Id, webresource);
                        }
                    }
                }
            }

            connectionData.Save();

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
                return;
            }

            Solution solution = null;

            if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
            {
                var repositorySolution = new SolutionRepository(service);

                solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                if (solution != null && solution.IsManaged.GetValueOrDefault())
                {
                    solution = null;
                }
            }

            if (solution == null)
            {
                var t = new Thread(() =>
                {
                    try
                    {
                        var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, service);

                        formSelectSolution.ShowDialog().GetValueOrDefault();

                        solution = formSelectSolution.SelectedSolution;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();
            }
            else
            {
                this._iWriteToOutput.WriteToOutputSolutionUri(connectionData, solution.UniqueName, solution.Id);
            }

            if (solution == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.Save();

            var solutionRep = new SolutionComponentRepository(service);

            {
                var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.WebResource, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                {
                    if (dictForAdding.ContainsKey(item))
                    {
                        dictForAdding.Remove(item);
                    }
                }
            }

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoWebResourcesToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            })).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service)
            {
                WithManagedInfo = true,
                WithSolutionsInfo = true,
                WithUrls = true,
            };
            
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourcesToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(connectionData, desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
        }

        #endregion Добавление веб-ресурса в решение.

        #region Добавление отчета в решение.

        public async Task ExecuteAddingReportsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingReportsToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingReportsToSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
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

        private async Task AddingReportsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
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

            // Репозиторий для работы с веб-ресурсами
            var reportRepository = new ReportRepository(service);

            var dictForAdding = new Dictionary<Guid, Report>();

            foreach (var selectedFile in selectedFiles)
            {
                if (File.Exists(selectedFile.FilePath))
                {
                    var reportEntity = await reportRepository.FindAsync(selectedFile.FileName);

                    if (reportEntity != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportFoundedByNameFormat2, reportEntity.Id.ToString(), reportEntity.Name);
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedByNameFormat1, selectedFile.FileName);

                        Guid? idLastLink = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (idLastLink.HasValue)
                        {
                            reportEntity = await reportRepository.GetByIdAsync(idLastLink.Value);
                        }
                    }

                    if (reportEntity != null)
                    {
                        if (!dictForAdding.ContainsKey(reportEntity.Id))
                        {
                            dictForAdding.Add(reportEntity.Id, reportEntity);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedInConnectionFormat2, connectionData.Name, selectedFile.FileName);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                }
            }

            connectionData.Save();

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoReportsToAddToSolution);
                return;
            }

            Solution solution = null;

            if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
            {
                var repositorySolution = new SolutionRepository(service);

                solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                if (solution != null && solution.IsManaged.GetValueOrDefault())
                {
                    solution = null;
                }
            }

            if (solution == null)
            {
                var t = new Thread(() =>
                {
                    try
                    {
                        var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, service);

                        formSelectSolution.ShowDialog().GetValueOrDefault();

                        solution = formSelectSolution.SelectedSolution;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();
            }
            else
            {
                this._iWriteToOutput.WriteToOutputSolutionUri(connectionData, solution.UniqueName, solution.Id);
            }

            if (solution == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.Save();

            var solutionRep = new SolutionComponentRepository(service);

            {
                var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.Report, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                {
                    if (dictForAdding.ContainsKey(item))
                    {
                        dictForAdding.Remove(item);
                    }
                }
            }

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoReportsToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);
                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.Report),
                //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            })).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service)
            {
                WithManagedInfo = true,
                WithSolutionsInfo = true,
                WithUrls = true,
            };

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportsToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(connectionData, desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
        }

        #endregion Добавление отчета в решение.

        #region Добавление компонентов в решение.

        public static async Task AddSolutionComponentsGroupToSolution(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, SolutionComponentDescriptor descriptor, CommonConfiguration commonConfig, string solutionUniqueName, ComponentType componentType, IEnumerable<Guid> selectedObjects, SolutionComponent.Schema.OptionSets.rootcomponentbehavior? rootComponentBehavior, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingComponentsToSolutionFormat2, service?.ConnectionData?.Name, solutionUniqueName);

            iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            try
            {
                if (!selectedObjects.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToAddToSolution);
                    return;
                }

                if (descriptor == null)
                {
                    descriptor = new SolutionComponentDescriptor(service);
                    descriptor.SetSettings(commonConfig);
                }

                // Репозиторий для работы с веб-ресурсами
                var reportRepository = new ReportRepository(service);

                var dictForAdding = new HashSet<Guid>();

                if (SolutionComponent.IsComponentTypeMetadata(componentType))
                {
                    foreach (var item in selectedObjects)
                    {
                        dictForAdding.Add(item);
                    }
                }
                else
                {
                    var entities = descriptor.GetEntities<Entity>((int)componentType, selectedObjects.Select(e => (Guid?)e));

                    foreach (var entity in entities)
                    {
                        dictForAdding.Add(entity.Id);
                    }
                }

                service.ConnectionData.Save();

                if (!dictForAdding.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToAddToSolution);
                    return;
                }

                Solution solution = null;

                if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
                {
                    var repositorySolution = new SolutionRepository(service);

                    solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                    if (solution != null && solution.IsManaged.GetValueOrDefault())
                    {
                        solution = null;
                    }
                }

                if (solution == null)
                {
                    var t = new Thread(() =>
                    {
                        try
                        {
                            var formSelectSolution = new WindowSolutionSelect(iWriteToOutput, service);

                            formSelectSolution.ShowDialog().GetValueOrDefault();

                            solution = formSelectSolution.SelectedSolution;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                        }
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();
                }
                else
                {
                    iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);
                }

                if (solution == null)
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                service.ConnectionData.AddLastSelectedSolution(solution.UniqueName);
                service.ConnectionData.Save();

                var solutionRep = new SolutionComponentRepository(service);

                {
                    var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, componentType, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                    foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                    {
                        if (dictForAdding.Contains(item))
                        {
                            dictForAdding.Remove(item);
                        }
                    }
                }

                if (!dictForAdding.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);
                    return;
                }
                
                var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
                {
                    ObjectId = e,
                    ComponentType = new OptionSetValue((int)componentType),
                    RootComponentBehaviorEnum = rootComponentBehavior,
                }).ToList();
                
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ComponentsToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

                var desc = await descriptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
            }
            catch (Exception ex)
            {
                iWriteToOutput.WriteErrorToOutput(service?.ConnectionData, ex);
            }
            finally
            {
                iWriteToOutput.WriteToOutputEndOperation(service?.ConnectionData, operation);
            }
        }

        public static async Task AddSolutionComponentsCollectionToSolution(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, SolutionComponentDescriptor descriptor, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SolutionComponent> components, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingComponentsToSolutionFormat2, service?.ConnectionData?.Name, solutionUniqueName);

            iWriteToOutput.WriteToOutputStartOperation(service?.ConnectionData, operation);

            try
            {
                if (!components.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToAddToSolution);
                    return;
                }

                if (descriptor == null)
                {
                    descriptor = new SolutionComponentDescriptor(service);
                    descriptor.SetSettings(commonConfig);
                }

                var dictForAdding = new HashSet<Tuple<int, Guid>>();

                foreach (var grComponents in components.Where(en => en.ObjectId.HasValue && en.ComponentType != null).GroupBy(en => en.ComponentType.Value))
                {
                    if (SolutionComponent.IsDefinedComponentType(grComponents.Key))
                    {
                        var componentType = (ComponentType)grComponents.Key;

                        if (SolutionComponent.IsComponentTypeMetadata(componentType))
                        {
                            foreach (var item in grComponents)
                            {
                                var key = Tuple.Create(item.ComponentType.Value, item.ObjectId.Value);

                                dictForAdding.Add(key);
                            }
                        }
                        else
                        {
                            var entities = descriptor.GetEntities<Entity>((int)componentType, grComponents.Select(e => e.ObjectId));

                            if (entities != null)
                            {
                                var hash = new HashSet<Guid>(entities.Select(en => en.Id));

                                foreach (var item in grComponents)
                                {
                                    if (hash.Contains(item.ObjectId.Value))
                                    {
                                        var key = Tuple.Create(item.ComponentType.Value, item.ObjectId.Value);

                                        dictForAdding.Add(key);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in grComponents)
                        {
                            var key = Tuple.Create(item.ComponentType.Value, item.ObjectId.Value);

                            dictForAdding.Add(key);
                        }
                    }
                }

                service.ConnectionData.Save();

                if (!dictForAdding.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToAddToSolution);
                    return;
                }

                Solution solution = null;

                if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
                {
                    var repositorySolution = new SolutionRepository(service);

                    solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                    if (solution != null && solution.IsManaged.GetValueOrDefault())
                    {
                        solution = null;
                    }
                }

                if (solution == null)
                {
                    var t = new Thread(() =>
                    {
                        try
                        {
                            var formSelectSolution = new WindowSolutionSelect(iWriteToOutput, service);

                            formSelectSolution.ShowDialog().GetValueOrDefault();

                            solution = formSelectSolution.SelectedSolution;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                        }
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();
                }
                else
                {
                    iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);
                }

                if (solution == null)
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                service.ConnectionData.AddLastSelectedSolution(solution?.UniqueName);
                service.ConnectionData.Save();

                var solutionRep = new SolutionComponentRepository(service);

                {
                    var solutionComponents = await solutionRep.GetSolutionComponentsAsync(solution.Id, new ColumnSet(SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.componenttype));

                    foreach (var item in solutionComponents.Where(s => s.ObjectId.HasValue && s.ComponentType != null))
                    {
                        dictForAdding.Remove(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                    }
                }

                if (!dictForAdding.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);
                    return;
                }

                var componentsForAdding = new List<SolutionComponent>();

                componentsForAdding.AddRange(components.Where(en => en.ComponentType != null && en.ObjectId.HasValue && dictForAdding.Contains(Tuple.Create(en.ComponentType.Value, en.ObjectId.Value))));

                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ComponentsToAddToSolutionFormat2, solution.UniqueName, componentsForAdding.Count);

                var desc = await descriptor.GetSolutionComponentsDescriptionAsync(componentsForAdding);

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsForAdding);
            }
            catch (Exception ex)
            {
                iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
            finally
            {
                iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
            }
        }

        #endregion Добавление компонентов в решение.

        public static async Task RemoveSolutionComponentsCollectionFromSolution(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, SolutionComponentDescriptor descriptor, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SolutionComponent> components, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.RemovingComponentsFromSolutionFormat2, service?.ConnectionData?.Name, solutionUniqueName);

            iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            try
            {
                if (!components.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToRemoveFromSolutionFormat1, solutionUniqueName);
                    return;
                }

                if (descriptor == null)
                {
                    descriptor = new SolutionComponentDescriptor(service);
                    descriptor.SetSettings(commonConfig);
                }

                service.ConnectionData.Save();

                Solution solution = null;

                if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
                {
                    var repositorySolution = new SolutionRepository(service);

                    solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                    if (solution != null && solution.IsManaged.GetValueOrDefault())
                    {
                        solution = null;
                    }
                }

                if (solution == null)
                {
                    var t = new Thread(() =>
                    {
                        try
                        {
                            var formSelectSolution = new WindowSolutionSelect(iWriteToOutput, service);

                            formSelectSolution.ShowDialog().GetValueOrDefault();

                            solution = formSelectSolution.SelectedSolution;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                        }
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();
                }
                else
                {
                    iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);
                }

                if (solution == null)
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                service.ConnectionData.AddLastSelectedSolution(solution?.UniqueName);
                service.ConnectionData.Save();

                var solutionRep = new SolutionComponentRepository(service);

                var dictForRemoving = new HashSet<Tuple<int, Guid>>(components.Select(c => Tuple.Create(c.ComponentType.Value, c.ObjectId.Value)));

                {
                    var solutionComponents = await solutionRep.GetSolutionComponentsAsync(solution.Id, new ColumnSet(SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.componenttype));

                    var currentComponents = new HashSet<Tuple<int, Guid>>(solutionComponents.Select(c => Tuple.Create(c.ComponentType.Value, c.ObjectId.Value)));

                    dictForRemoving.IntersectWith(currentComponents);
                }

                if (!dictForRemoving.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoObjectsToRemoveFromSolutionFormat1, solutionUniqueName);
                    return;
                }

                var componentsForRemoving = new List<SolutionComponent>();

                componentsForRemoving.AddRange(components.Where(en => en.ComponentType != null && en.ObjectId.HasValue && dictForRemoving.Contains(Tuple.Create(en.ComponentType.Value, en.ObjectId.Value))));

                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ComponentsToRemoveFromSolutionFormat2, solution.UniqueName, componentsForRemoving.Count);

                var desc = await descriptor.GetSolutionComponentsDescriptionAsync(componentsForRemoving);

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
                }

                await solutionRep.RemoveSolutionComponentsAsync(solution.UniqueName, componentsForRemoving);
            }
            catch (Exception ex)
            {
                iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
            finally
            {
                iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
            }
        }

        #region Добавление сборки в решение по имени.

        public async Task ExecuteAddingPluginAssemblyToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginAssemblyToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginAssemblyToSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
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

        private async Task AddingPluginAssemblyToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (projectNames == null || !projectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
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

            var repository = new PluginAssemblyRepository(service);

            Dictionary<Guid, PluginAssembly> knownAssemblies = new Dictionary<Guid, PluginAssembly>();

            List<string> unknownProjectNames = new List<string>();

            foreach (var projectName in projectNames)
            {
                var assembly = await repository.FindAssemblyAsync(projectName);

                if (assembly == null)
                {
                    assembly = await repository.FindAssemblyByLikeNameAsync(projectName);
                }

                if (assembly != null)
                {
                    if (!knownAssemblies.ContainsKey(assembly.Id))
                    {
                        knownAssemblies.Add(assembly.Id, assembly);
                    }
                }
                else
                {
                    unknownProjectNames.Add(projectName);
                }
            }

            if (!knownAssemblies.Any() && !unknownProjectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginAssembliesToAddToSolution);
                return;
            }

            if (knownAssemblies.Any())
            {
                Solution solution = null;

                if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
                {
                    var repositorySolution = new SolutionRepository(service);

                    solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                    if (solution != null && solution.IsManaged.GetValueOrDefault())
                    {
                        solution = null;
                    }
                }

                if (solution == null)
                {
                    var t = new Thread(() =>
                    {
                        try
                        {
                            var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, service);

                            formSelectSolution.ShowDialog().GetValueOrDefault();

                            solution = formSelectSolution.SelectedSolution;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(connectionData, ex);
                        }
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();
                }
                else
                {
                    this._iWriteToOutput.WriteToOutputSolutionUri(connectionData, solution.UniqueName, solution.Id);
                }

                if (solution == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                connectionData.AddLastSelectedSolution(solution?.UniqueName);
                connectionData.Save();

                var solutionRep = new SolutionComponentRepository(service);

                {
                    var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.PluginAssembly, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                    foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                    {
                        if (knownAssemblies.ContainsKey(item))
                        {
                            knownAssemblies.Remove(item);
                        }
                    }
                }

                if (!knownAssemblies.Any())
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginAssembliesToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);

                    OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);

                    return;
                }

                var componentsToAdd = knownAssemblies.Select(e => new SolutionComponent(new
                {
                    ObjectId = e.Key,
                    ComponentType = new OptionSetValue((int)ComponentType.PluginAssembly),
                    //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                })).ToList();

                var solutionDesciptor = new SolutionComponentDescriptor(service);
                solutionDesciptor.SetSettings(commonConfig);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssembliesToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

                var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                if (!string.IsNullOrEmpty(desc))
                {
                    _iWriteToOutput.WriteToOutput(connectionData, desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
            }

            OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);
        }

        #endregion Добавление сборки в решение по имени.

        #region Добавление шагов плагинов сборки в решение по имени.

        public async Task ExecuteAddingPluginAssemblyProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginAssemblyProcessingStepsToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginAssemblyProcessingStepsToSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
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

        private async Task AddingPluginAssemblyProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (projectNames == null || !projectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
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

            var repository = new PluginAssemblyRepository(service);
            var stepRepository = new SdkMessageProcessingStepRepository(service);

            Dictionary<Guid, PluginAssembly> knownAssemblies = new Dictionary<Guid, PluginAssembly>();

            List<string> unknownProjectNames = new List<string>();

            foreach (var projectName in projectNames)
            {
                var assembly = await repository.FindAssemblyAsync(projectName);

                if (assembly == null)
                {
                    assembly = await repository.FindAssemblyByLikeNameAsync(projectName);
                }

                if (assembly != null)
                {
                    if (!knownAssemblies.ContainsKey(assembly.Id))
                    {
                        knownAssemblies.Add(assembly.Id, assembly);
                    }
                }
                else
                {
                    unknownProjectNames.Add(projectName);
                }
            }

            if (!knownAssemblies.Any() && !unknownProjectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginAssembliesToAddToSolution);
                return;
            }

            var dictForAdding = new Dictionary<Guid, SdkMessageProcessingStep>();

            foreach (var assembly in knownAssemblies.Values)
            {
                var pluginSteps = await stepRepository.GetAllStepsByPluginAssemblyAsync(assembly.Id);

                foreach (var step in pluginSteps)
                {
                    dictForAdding.Add(step.Id, step);
                }
            }

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProcessingStepsToAddToSolution);

                OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);

                return;
            }

            Solution solution = null;

            if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
            {
                var repositorySolution = new SolutionRepository(service);

                solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                if (solution != null && solution.IsManaged.GetValueOrDefault())
                {
                    solution = null;
                }
            }

            if (solution == null)
            {
                var t = new Thread(() =>
                {
                    try
                    {
                        var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, service);

                        formSelectSolution.ShowDialog().GetValueOrDefault();

                        solution = formSelectSolution.SelectedSolution;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();
            }
            else
            {
                this._iWriteToOutput.WriteToOutputSolutionUri(connectionData, solution.UniqueName, solution.Id);
            }

            if (solution == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.Save();

            var solutionRep = new SolutionComponentRepository(service);

            {
                var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.SdkMessageProcessingStep, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                {
                    if (dictForAdding.ContainsKey(item))
                    {
                        dictForAdding.Remove(item);
                    }
                }
            }

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProcessingStepsToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);

                OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);

                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.SdkMessageProcessingStep),
                RootComponentBehaviorEnum = SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
            }).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service);
            solutionDesciptor.SetSettings(commonConfig);

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ProcessingStepsToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(connectionData, desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);

            OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);
        }

        private void OpenWindowForUnknownProjects(CommonConfiguration commonConfig, IOrganizationServiceExtented service, List<string> unknownProjectNames)
        {
            if (unknownProjectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginAssembliesNotFoundedByNameFormat1, unknownProjectNames.Count);

                foreach (var projectName in unknownProjectNames)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "{0}{1}", _tabSpacer, projectName);
                }

                WindowHelper.OpenPluginAssemblyExplorer(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , unknownProjectNames.FirstOrDefault()
                    );
            }
        }

        #endregion Добавление шагов плагинов сборки в решение по имени.

        #region Добавление в решение шагов плагинов типа плагина по имени.

        public async Task ExecuteAddingPluginTypeProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginTypeProcessingStepsToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginTypeProcessingStepsToSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
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

        private async Task AddingPluginTypeProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
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

            Dictionary<Guid, PluginType> knownPluginTypes = new Dictionary<Guid, PluginType>();

            List<string> unknownPluginTypes = new List<string>();

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
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginTypesFounded);
                return;
            }

            var stepRepository = new SdkMessageProcessingStepRepository(service);

            var dictForAdding = new Dictionary<Guid, SdkMessageProcessingStep>();

            foreach (var pluginType in knownPluginTypes.Values)
            {
                var pluginSteps = await stepRepository.GetAllStepsByPluginTypeAsync(pluginType.Id);

                foreach (var step in pluginSteps)
                {
                    dictForAdding.Add(step.Id, step);
                }
            }

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProcessingStepsToAddToSolution);

                OpenWindowForUnknownPluginTypes(commonConfig, service, unknownPluginTypes);

                return;
            }

            Solution solution = null;

            if (!withSelect && !string.IsNullOrEmpty(solutionUniqueName))
            {
                var repositorySolution = new SolutionRepository(service);

                solution = await repositorySolution.GetSolutionByUniqueNameAsync(solutionUniqueName);

                if (solution != null && solution.IsManaged.GetValueOrDefault())
                {
                    solution = null;
                }
            }

            if (solution == null)
            {
                var t = new Thread(() =>
                {
                    try
                    {
                        var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, service);

                        formSelectSolution.ShowDialog().GetValueOrDefault();

                        solution = formSelectSolution.SelectedSolution;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();
            }
            else
            {
                this._iWriteToOutput.WriteToOutputSolutionUri(connectionData, solution.UniqueName, solution.Id);
            }

            if (solution == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.Save();

            var solutionRep = new SolutionComponentRepository(service);

            {
                var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.SdkMessageProcessingStep, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                {
                    if (dictForAdding.ContainsKey(item))
                    {
                        dictForAdding.Remove(item);
                    }
                }
            }

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProcessingStepsToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);

                OpenWindowForUnknownPluginTypes(commonConfig, service, unknownPluginTypes);

                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.SdkMessageProcessingStep),
                RootComponentBehaviorEnum = SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
            }).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service);
            solutionDesciptor.SetSettings(commonConfig);

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ProcessingStepsToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(connectionData, desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);

            OpenWindowForUnknownPluginTypes(commonConfig, service, unknownPluginTypes);
        }

        private void OpenWindowForUnknownPluginTypes(CommonConfiguration commonConfig, IOrganizationServiceExtented service, List<string> unknownPluginTypes)
        {
            if (unknownPluginTypes.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginTypesNotFoundedByNameFormat1, unknownPluginTypes.Count);

                foreach (var pluginTypeName in unknownPluginTypes)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "{0}{1}", _tabSpacer, pluginTypeName);
                }

                WindowHelper.OpenPluginTypeWindow(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , unknownPluginTypes.FirstOrDefault()
                );
            }
        }

        #endregion Добавление в решение шагов плагинов типа плагина по имени.
    }
}