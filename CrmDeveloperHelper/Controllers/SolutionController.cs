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
        private IWriteToOutput _iWriteToOutput = null;

        public SolutionController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Solution Explorer.

        public async Task ExecuteOpeningSolutionComponentWindow(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.SolutionComponentExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await OpeningSolutionComponentWindow(selectedItem, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task OpeningSolutionComponentWindow(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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

        #region Окна с образами.

        public void ExecuteOpeningSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSolutionImageWindowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                OpeningSolutionImageWindow(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private void OpeningSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        public void ExecuteOpeningOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ShowingOrganizationDifferenceImageWindow);

            try
            {
                OpeningOrganizationDifferenceImageWindow(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ShowingOrganizationDifferenceImageWindow);
            }
        }

        private void OpeningOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        #endregion Окна с образами.

        #region Добавление веб-ресурса в решение.

        public async Task ExecuteAddingWebResourcesIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingWebResourcesIntoSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await AddingWebResourcesIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task AddingWebResourcesIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (selectedFiles == null || !selectedFiles.Any())
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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
                        this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput("Try to find web-resource by name: {0}. Searching...", selectedFile.Name);

                    string key = selectedFile.FriendlyFilePath.ToLower();

                    var contentFile = Convert.ToBase64String(File.ReadAllBytes(selectedFile.FilePath));

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, key, gr.Key);

                    if (webresource != null)
                    {
                        var webName = webresource.Name;

                        this._iWriteToOutput.WriteToOutput("WebResource founded by name. WebResourceId: {0} Name: {1}", webresource.Id, webName);
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("WebResource not founded by name. FileName: {0}. Open linking dialog...", selectedFile.Name);

                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.FindByIdAsync(webId.Value);
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

            connectionData.ConnectionConfiguration.Save();

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
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
                        DTEHelper.WriteExceptionToOutput(ex);
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
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.ConnectionConfiguration.Save();

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
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoWebResourcesToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            })).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service, true);

            this._iWriteToOutput.WriteToOutput("WebResources to add into Solution {0}: {1}", solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
        }

        #endregion Добавление веб-ресурса в решение.

        #region Добавление отчета в решение.

        public async Task ExecuteAddingReportsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingReportsIntoSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await AddingReportsIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task AddingReportsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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
                        this._iWriteToOutput.WriteToOutput("Report founded by name: {0}", selectedFile.FileName);
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("Report not founded by name: {0}", selectedFile.FileName);

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
                        this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ReportNotFoundedInConnectionFormat2, connectionData.Name, selectedFile.FileName);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                }
            }

            connectionData.ConnectionConfiguration.Save();

            if (!dictForAdding.Any())
            {
                this._iWriteToOutput.WriteToOutput("No Reports to add.");
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
                        DTEHelper.WriteExceptionToOutput(ex);
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
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.ConnectionConfiguration.Save();

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
                this._iWriteToOutput.WriteToOutput("No Reports to add. All reports already in Solution {0}", solution.UniqueName);
                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.Report),
                //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            })).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service, true);

            this._iWriteToOutput.WriteToOutput("Reports to add into Solution {0}: {1}", solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
        }

        #endregion Добавление отчета в решение.

        #region Добавление компонентов в решение.

        public static async Task AddSolutionComponentsGroupIntoSolution(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, SolutionComponentDescriptor descriptor, CommonConfiguration commonConfig, string solutionUniqueName, ComponentType componentType, IEnumerable<Guid> selectedObjects, RootComponentBehavior? rootComponentBehavior, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingComponentsIntoSolutionFormat2, service?.ConnectionData?.Name, solutionUniqueName);

            iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                if (!selectedObjects.Any())
                {
                    iWriteToOutput.WriteToOutput("No Objects to add.");
                    return;
                }

                if (descriptor == null)
                {
                    descriptor = new SolutionComponentDescriptor(service, true);
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

                service.ConnectionData.ConnectionConfiguration.Save();

                if (!dictForAdding.Any())
                {
                    iWriteToOutput.WriteToOutput("No Objects to add.");
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
                            DTEHelper.WriteExceptionToOutput(ex);
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
                    iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                service.ConnectionData.AddLastSelectedSolution(solution.UniqueName);
                service.ConnectionData.ConnectionConfiguration.Save();

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
                    iWriteToOutput.WriteToOutput("No Objects to add. All components already in Solution {0}", solution.UniqueName);
                    return;
                }

                OptionSetValue rootBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents);

                if (rootComponentBehavior.HasValue)
                {
                    rootBehavior = new OptionSetValue((int)rootComponentBehavior.Value);
                }

                var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
                {
                    ObjectId = e,
                    ComponentType = new OptionSetValue((int)componentType),
                    RootComponentBehavior = rootBehavior,
                })).ToList();

                iWriteToOutput.WriteToOutput("Components to add into Solution {0}: {1}", solution.UniqueName, componentsToAdd.Count);

                var desc = await descriptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
            }
            catch (Exception xE)
            {
                iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        public static async Task AddSolutionComponentsCollectionIntoSolution(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, SolutionComponentDescriptor descriptor, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SolutionComponent> components, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingComponentsIntoSolutionFormat2, service?.ConnectionData?.Name, solutionUniqueName);

            iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                if (!components.Any())
                {
                    iWriteToOutput.WriteToOutput("No Objects to add.");
                    return;
                }

                if (descriptor == null)
                {
                    descriptor = new SolutionComponentDescriptor(service, true);
                }

                var dictForAdding = new HashSet<Tuple<int, Guid>>();

                var solutionDesciptor = new SolutionComponentDescriptor(service, true);

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
                            var entities = solutionDesciptor.GetEntities<Entity>((int)componentType, grComponents.Select(e => e.ObjectId));

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

                service.ConnectionData.ConnectionConfiguration.Save();

                if (!dictForAdding.Any())
                {
                    iWriteToOutput.WriteToOutput("No Objects to add.");
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
                            DTEHelper.WriteExceptionToOutput(ex);
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
                    iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                service.ConnectionData.AddLastSelectedSolution(solution?.UniqueName);
                service.ConnectionData.ConnectionConfiguration.Save();

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
                    iWriteToOutput.WriteToOutput("No Objects to add. All components already in Solution {0}", solution.UniqueName);
                    return;
                }

                var componentsForAdding = new List<SolutionComponent>();

                componentsForAdding.AddRange(components.Where(en => en.ComponentType != null && en.ObjectId.HasValue && dictForAdding.Contains(Tuple.Create(en.ComponentType.Value, en.ObjectId.Value))));

                iWriteToOutput.WriteToOutput("Components to add into Solution {0}: {1}", solution.UniqueName, componentsForAdding.Count);

                var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsForAdding);

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsForAdding);
            }
            catch (Exception xE)
            {
                iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        #endregion Добавление компонентов в решение.

        #region Добавление сборки в решение по имени.

        public async Task ExecuteAddingPluginAssemblyIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginAssemblyIntoSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await AddingPluginAssemblyIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task AddingPluginAssemblyIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (projectNames == null || !projectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput("No Project Names.");
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new PluginAssemblyRepository(service);

            Dictionary<Guid, PluginAssembly> knownAssemblies = new Dictionary<Guid, PluginAssembly>();

            List<string> unknownProjectNames = new List<string>();

            foreach (var projectName in projectNames)
            {
                var assembly = await repository.FindAssemblyAsync(projectName);

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
                this._iWriteToOutput.WriteToOutput("No Plugin Assemblies to add.");
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
                            DTEHelper.WriteExceptionToOutput(ex);
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
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                connectionData.AddLastSelectedSolution(solution?.UniqueName);
                connectionData.ConnectionConfiguration.Save();

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
                    this._iWriteToOutput.WriteToOutput("No PluginAssembly to add. All PluginAssemblies already in Solution {0}", solution.UniqueName);

                    OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);

                    return;
                }

                var componentsToAdd = knownAssemblies.Select(e => new SolutionComponent(new
                {
                    ObjectId = e.Key,
                    ComponentType = new OptionSetValue((int)ComponentType.PluginAssembly),
                    //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                })).ToList();

                var solutionDesciptor = new SolutionComponentDescriptor(service, true);

                this._iWriteToOutput.WriteToOutput("PluginAssemblies to add into Solution {0}: {1}", solution.UniqueName, componentsToAdd.Count);

                var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                if (!string.IsNullOrEmpty(desc))
                {
                    _iWriteToOutput.WriteToOutput(desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
            }

            OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);
        }

        #endregion Добавление сборки в решение по имени.

        #region Добавление шагов плагинов сборки в решение по имени.

        public async Task ExecuteAddingPluginAssemblyProcessingStepsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginAssemblyProcessingStepsIntoSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await AddingPluginAssemblyProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task AddingPluginAssemblyProcessingStepsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (projectNames == null || !projectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput("No Project Names.");
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new PluginAssemblyRepository(service);
            var stepRepository = new SdkMessageProcessingStepRepository(service);

            Dictionary<Guid, PluginAssembly> knownAssemblies = new Dictionary<Guid, PluginAssembly>();

            List<string> unknownProjectNames = new List<string>();

            foreach (var projectName in projectNames)
            {
                var assembly = await repository.FindAssemblyAsync(projectName);

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
                this._iWriteToOutput.WriteToOutput("No Plugin Assemblies founded.");
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
                this._iWriteToOutput.WriteToOutput("No Processing Steps to add.");

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
                        DTEHelper.WriteExceptionToOutput(ex);
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
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.ConnectionConfiguration.Save();

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
                this._iWriteToOutput.WriteToOutput("No Processing Steps to add. All Processing Steps already in Solution {0}", solution.UniqueName);

                OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);

                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.SdkMessageProcessingStep),
                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            })).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service, true);

            this._iWriteToOutput.WriteToOutput("Processing Steps to add into Solution {0}: {1}", solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);

            OpenWindowForUnknownProjects(commonConfig, service, unknownProjectNames);
        }

        private void OpenWindowForUnknownProjects(CommonConfiguration commonConfig, IOrganizationServiceExtented service, List<string> unknownProjectNames)
        {
            if (unknownProjectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput("PluginAssemblies not founded by name {0}.", unknownProjectNames.Count);

                foreach (var projectName in unknownProjectNames)
                {
                    this._iWriteToOutput.WriteToOutput("       {0}", projectName);
                }

                WindowHelper.OpenPluginAssemblyWindow(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , unknownProjectNames.FirstOrDefault()
                    );
            }
        }

        #endregion Добавление шагов плагинов сборки в решение по имени.

        #region Добавление в решение шагов плагинов типа плагина по имени.

        public async Task ExecuteAddingPluginTypeProcessingStepsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginTypeProcessingStepsIntoSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await AddingPluginTypeProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task AddingPluginTypeProcessingStepsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                this._iWriteToOutput.WriteToOutput("No Project Names.");
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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
                this._iWriteToOutput.WriteToOutput("No Plugin Types founded.");
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
                this._iWriteToOutput.WriteToOutput("No Processing Steps to add.");

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
                        DTEHelper.WriteExceptionToOutput(ex);
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
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionNotSelected);
                return;
            }

            connectionData.AddLastSelectedSolution(solution?.UniqueName);
            connectionData.ConnectionConfiguration.Save();

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
                this._iWriteToOutput.WriteToOutput("No Processing Steps to add. All Processing Steps already in Solution {0}", solution.UniqueName);

                OpenWindowForUnknownPluginTypes(commonConfig, service, unknownPluginTypes);

                return;
            }

            var componentsToAdd = dictForAdding.Select(e => new SolutionComponent(new
            {
                ObjectId = e.Key,
                ComponentType = new OptionSetValue((int)ComponentType.SdkMessageProcessingStep),
                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            })).ToList();

            var solutionDesciptor = new SolutionComponentDescriptor(service, true);

            this._iWriteToOutput.WriteToOutput("Processing Steps to add into Solution {0}: {1}", solution.UniqueName, componentsToAdd.Count);

            var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

            if (!string.IsNullOrEmpty(desc))
            {
                _iWriteToOutput.WriteToOutput(desc);
            }

            await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);

            OpenWindowForUnknownPluginTypes(commonConfig, service, unknownPluginTypes);
        }

        private void OpenWindowForUnknownPluginTypes(CommonConfiguration commonConfig, IOrganizationServiceExtented service, List<string> unknownPluginTypes)
        {
            if (unknownPluginTypes.Any())
            {
                this._iWriteToOutput.WriteToOutput("PluginTypes not founded by name {0}.", unknownPluginTypes.Count);

                foreach (var pluginTypeName in unknownPluginTypes)
                {
                    this._iWriteToOutput.WriteToOutput("       {0}", pluginTypeName);
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