using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
    public class SolutionController : BaseController<IWriteToOutput>
    {
        public SolutionController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Solution Explorer.

        public async Task ExecuteOpeningSolutionExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.SolutionExplorerFormat1
                , (service) => WindowHelper.OpenExplorerSolutionExplorer(this._iWriteToOutput, service, commonConfig, null, null, selectedItem)
            );
        }

        #endregion Solution Explorer.

        #region ImportJob Explorer.

        public async Task ExecuteOpeningImportJobExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ImportJobExplorerFormat1
                , (service) => WindowHelper.OpenImportJobExplorer(this._iWriteToOutput, service, commonConfig, null)
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

        private static async Task<Solution> FindOrSelectSolution(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, string solutionUniqueName, bool withSelect)
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

            return solution;
        }

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
            if (selectedFiles == null || !selectedFiles.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                // Репозиторий для работы с веб-ресурсами
                var webResourceRepository = new WebResourceRepository(service);

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

                var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

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

                var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
                {
                    ObjectId = e.Key,
                    ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                    //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                }).ToList();

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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
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

                var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

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

                var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
                {
                    ObjectId = e.Key,
                    ComponentType = new OptionSetValue((int)ComponentType.Report),
                    //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                }).ToList();

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
        }

        #endregion Добавление отчета в решение.

        #region Добавление компонентов в решение.

        public static async Task AddSolutionComponentsGroupToSolution(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , ComponentType componentType
            , IEnumerable<Guid> selectedObjects
            , SolutionComponent.Schema.OptionSets.rootcomponentbehavior? rootComponentBehavior
            , bool withSelect
        )
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

                var solution = await FindOrSelectSolution(iWriteToOutput, service, solutionUniqueName, withSelect);

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

        public static async Task AddSolutionComponentsCollectionToSolution(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , IEnumerable<SolutionComponent> components
            , bool withSelect
        )
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

                var solution = await FindOrSelectSolution(iWriteToOutput, service, solutionUniqueName, withSelect);

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

        public static async Task RemoveSolutionComponentsCollectionFromSolution(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , IEnumerable<SolutionComponent> components
            , bool withSelect
        )
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

                var solution = await FindOrSelectSolution(iWriteToOutput, service, solutionUniqueName, withSelect);

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

        #endregion Добавление компонентов в решение.

        #region Добавление сборки в решение по имени.

        public async Task ExecuteAddingPluginAssemblyToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> projectNames, string solutionUniqueName, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginAssemblyToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginAssemblyToSolution(connectionData, commonConfig, projectNames, solutionUniqueName, withSelect);
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

        private async Task AddingPluginAssemblyToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> projectNames, string solutionUniqueName, bool withSelect)
        {
            if (projectNames == null || !projectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var repository = new PluginAssemblyRepository(service);

                var knownAssemblies = new Dictionary<Guid, PluginAssembly>();

                var unknownProjectNames = new List<string>();

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
                    var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

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

                        OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);

                        return;
                    }

                    var componentsToAdd = knownAssemblies.Select(e => new SolutionComponent()
                    {
                        ObjectId = e.Key,
                        ComponentType = new OptionSetValue((int)ComponentType.PluginAssembly),
                        //RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    }).ToList();

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

                OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);
            }
        }

        #endregion Добавление сборки в решение по имени.

        #region Добавление шагов плагинов сборки в решение по имени.

        public async Task ExecuteAddingPluginAssemblyProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> projectNames, string solutionUniqueName, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginAssemblyProcessingStepsToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginAssemblyProcessingStepsToSolution(connectionData, commonConfig, projectNames, solutionUniqueName, withSelect);
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

        private async Task AddingPluginAssemblyProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> projectNames, string solutionUniqueName, bool withSelect)
        {
            if (projectNames == null || !projectNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoProjectNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var repository = new PluginAssemblyRepository(service);
                var stepRepository = new SdkMessageProcessingStepRepository(service);

                var knownAssemblies = new Dictionary<Guid, PluginAssembly>();

                var unknownProjectNames = new List<string>();

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

                    OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);

                    return;
                }

                var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

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

                    OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);

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

                OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);
            }
        }

        private async void OpenWindowForUnknownProjects(ConnectionData connectionData, CommonConfiguration commonConfig, List<string> unknownProjectNames)
        {
            if (!unknownProjectNames.Any())
            {
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssembliesNotFoundedByNameFormat1, unknownProjectNames.Count);

            foreach (var projectName in unknownProjectNames)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, projectName);
            }

            var service = await QuickConnection.ConnectAsync(connectionData);

            WindowHelper.OpenPluginAssemblyExplorer(
                this._iWriteToOutput
                , service
                , commonConfig
                , unknownProjectNames.FirstOrDefault()
            );
        }

        #endregion Добавление шагов плагинов сборки в решение по имени.

        #region Добавление в решение шагов плагинов типа плагина по имени.

        public async Task ExecuteAddingPluginTypeProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> pluginTypeNames, string solutionUniqueName, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.AddingPluginTypeProcessingStepsToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingPluginTypeProcessingStepsToSolution(connectionData, commonConfig, pluginTypeNames, solutionUniqueName, withSelect);
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

        private async Task AddingPluginTypeProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> pluginTypeNames, string solutionUniqueName, bool withSelect)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginTypesNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
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

                    OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);

                    return;
                }

                var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

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

                    OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);

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

                OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);
            }
        }

        #endregion Добавление в решение шагов плагинов типа плагина по имени.

        public async Task ExecuteAddingLinkedSystemFormToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, string entityName, Guid formId, int formType)
        {
            string operation = string.Format(Properties.OperationNames.AddingLinkedSystemFormToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingLinkedSystemFormToSolution(connectionData, commonConfig, solutionUniqueName, withSelect, entityName, formId, formType);
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

        private async Task AddingLinkedSystemFormToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, string entityName, Guid formId, int formType)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

                if (solution == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                connectionData.AddLastSelectedSolution(solution?.UniqueName);
                connectionData.Save();

                var dictForAdding = new HashSet<Guid>() { formId };

                var solutionRep = new SolutionComponentRepository(service);

                {
                    var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.SystemForm, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoSystemFormsToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
                    return;
                }

                var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
                {
                    ObjectId = e,
                    ComponentType = new OptionSetValue((int)ComponentType.SystemForm),
                }).ToList();

                var solutionDesciptor = new SolutionComponentDescriptor(service)
                {
                    WithManagedInfo = true,
                    WithSolutionsInfo = true,
                    WithUrls = true,
                };

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SystemFormsToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

                var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                if (!string.IsNullOrEmpty(desc))
                {
                    _iWriteToOutput.WriteToOutput(connectionData, desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
            }
        }

        public async Task ExecuteAddingEntityToSolution(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , bool withSelect
            , string entityName
            , SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior
        )
        {
            string operation = string.Format(Properties.OperationNames.AddingEntityToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingEntityToSolution(connectionData, commonConfig, solutionUniqueName, withSelect, entityName, rootComponentBehavior);
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

        private async Task AddingEntityToSolution(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , bool withSelect
            , string entityName
            , SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior
        )
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new EntityMetadataRepository(service);

            var entityMetadata = await repository.GetEntityMetadataAsync(entityName);

            if (entityMetadata == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityName);

                return;
            }

            using (service.Lock())
            {
                var solution = await FindOrSelectSolution(_iWriteToOutput, service, solutionUniqueName, withSelect);

                if (solution == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SolutionNotSelected);
                    return;
                }

                connectionData.AddLastSelectedSolution(solution?.UniqueName);
                connectionData.Save();

                var dictForAdding = new HashSet<Guid>() { entityMetadata.MetadataId.Value };

                var solutionRep = new SolutionComponentRepository(service);

                {
                    var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.Entity, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoEntitiesToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
                    return;
                }

                var componentsToAdd = dictForAdding.Select(e => new SolutionComponent()
                {
                    ObjectId = e,
                    ComponentType = new OptionSetValue((int)ComponentType.Entity),
                    RootComponentBehaviorEnum = rootComponentBehavior
                }).ToList();

                var solutionDesciptor = new SolutionComponentDescriptor(service)
                {
                    WithManagedInfo = true,
                    WithSolutionsInfo = true,
                    WithUrls = true,
                };

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntitiesToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

                var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                if (!string.IsNullOrEmpty(desc))
                {
                    _iWriteToOutput.WriteToOutput(connectionData, desc);
                }

                await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
            }
        }
    }
}