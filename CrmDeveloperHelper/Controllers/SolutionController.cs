using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
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
                var thread = new Thread(() =>
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
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                thread.Join();
            }
            else
            {
                iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);
            }

            return solution;
        }

        private async Task OpenWindowForUnknownProjects(ConnectionData connectionData, CommonConfiguration commonConfig, List<string> unknownProjectNames)
        {
            if (!unknownProjectNames.Any())
            {
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginAssembliesNotFoundedByNameFormat1, unknownProjectNames.Count);

            foreach (var projectName in unknownProjectNames)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, _formatWithTabSpacer, _tabSpacer, projectName);
            }

            var service = await QuickConnection.ConnectAsync(connectionData);

            WindowHelper.OpenPluginAssemblyExplorer(
                this._iWriteToOutput
                , service
                , commonConfig
                , unknownProjectNames.FirstOrDefault()
            );
        }

        private async Task OpenWindowForUnknownGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, List<string> unknownGlobalOptionSets)
        {
            if (!unknownGlobalOptionSets.Any())
            {
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GlobalOptionSetsNotFoundedByNameFormat1, unknownGlobalOptionSets.Count);

            foreach (var projectName in unknownGlobalOptionSets)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, _formatWithTabSpacer, _tabSpacer, projectName);
            }

            var service = await QuickConnection.ConnectAsync(connectionData);

            WindowHelper.OpenGlobalOptionSetsExplorer(
                this._iWriteToOutput
                , service
                , commonConfig
                , unknownGlobalOptionSets.FirstOrDefault()
            );
        }

        #region Adding WebResources to Solution

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
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionNoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionNoWebResourcesToAddInSolutionFormat2, connectionData.Name, solutionUniqueName);
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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionNoWebResourcesToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
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

        #endregion Adding WebResources to Solution

        #region Adding Reports to Solution

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
                            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionReportWasNotFoundFormat2, connectionData.Name, selectedFile.FileName);
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

        #endregion Adding Reports to Solution

        #region Adding Components to Solution

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

        #endregion Adding Components to Solution

        #region Removing Components from Solution

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

        #endregion Removing Components from Solution

        #region Adding PluginAssemblies to Solution

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
                    }
                    else
                    {
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

                        if (knownAssemblies.Any())
                        {
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
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginAssembliesToAddToSolutionAllComponentsInSolutionFormant1, solution.UniqueName);
                        }
                    }
                }

                await OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);
            }
        }

        #endregion Adding PluginAssemblies to Solution

        #region Adding PluginAssemblies Processing Steps to Solution

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

                    await OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);

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

                    await OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);

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

                await OpenWindowForUnknownProjects(connectionData, commonConfig, unknownProjectNames);
            }
        }

        #endregion Adding Plugin Steps to Solution

        #region Adding PluginTypes Processing Steps to Solution

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

                    await OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);

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

                    await OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);

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

                await OpenWindowForUnknownPluginTypes(connectionData, commonConfig, unknownPluginTypes);
            }
        }

        #endregion Adding PluginTypes Processing Steps to Solution

        #region Adding Linked SystemForms to Solution

        public async Task ExecuteAddingLinkedSystemFormToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, IEnumerable<Guid> formIdList)
        {
            string operation = string.Format(Properties.OperationNames.AddingLinkedSystemFormToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingLinkedSystemFormToSolution(connectionData, commonConfig, solutionUniqueName, withSelect, formIdList);
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

        private async Task AddingLinkedSystemFormToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, IEnumerable<Guid> formIdList)
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

                var dictForAdding = new HashSet<Guid>(formIdList);

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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionNoSystemFormsToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
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

        #endregion Adding Linked SystemForms to Solution

        #region Adding Entity to Solution

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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionNoEntitiesToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
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

        #endregion Adding Entity to Solution

        #region Adding GlobalOptionSet to Solution

        public async Task ExecuteAddingGlobalOptionSetToSolution(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , bool withSelect
            , IEnumerable<string> optionSetNames
        )
        {
            string operation = string.Format(Properties.OperationNames.AddingGlobalOptionSetToSolutionFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await AddingGlobalOptionSetToSolution(connectionData, commonConfig, solutionUniqueName, withSelect, optionSetNames);
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

        private async Task AddingGlobalOptionSetToSolution(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , bool withSelect
            , IEnumerable<string> optionSetNamesList
        )
        {
            if (optionSetNamesList == null || !optionSetNamesList.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoGlobalOptionSetNames);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var repository = new OptionSetRepository(service);

                var knownGlobalOptionSets = new HashSet<Guid>();

                var unknownGlobalOptionSets = new List<string>();

                foreach (var optionSetName in optionSetNamesList)
                {
                    OptionSetMetadata optionSetMetadata = await repository.GetOptionSetByNameAsync(optionSetName);

                    if (optionSetMetadata != null)
                    {
                        if (!knownGlobalOptionSets.Contains(optionSetMetadata.MetadataId.Value))
                        {
                            knownGlobalOptionSets.Add(optionSetMetadata.MetadataId.Value);
                        }
                    }
                    else
                    {
                        unknownGlobalOptionSets.Add(optionSetName);
                    }
                }

                if (!knownGlobalOptionSets.Any() && !unknownGlobalOptionSets.Any())
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoPluginAssembliesToAddToSolution);
                    return;
                }

                if (knownGlobalOptionSets.Any())
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
                        var components = await solutionRep.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.OptionSet, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                        foreach (var item in components.Where(s => s.ObjectId.HasValue).Select(s => s.ObjectId.Value))
                        {
                            if (knownGlobalOptionSets.Contains(item))
                            {
                                knownGlobalOptionSets.Remove(item);
                            }
                        }
                    }

                    if (knownGlobalOptionSets.Any())
                    {
                        var componentsToAdd = knownGlobalOptionSets.Select(e => new SolutionComponent()
                        {
                            ObjectId = e,
                            ComponentType = new OptionSetValue((int)ComponentType.OptionSet),
                            RootComponentBehaviorEnum = SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
                        }).ToList();

                        var solutionDesciptor = new SolutionComponentDescriptor(service)
                        {
                            WithManagedInfo = true,
                            WithSolutionsInfo = true,
                            WithUrls = true,
                        };

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GlobalOptionSetsToAddToSolutionFormat2, solution.UniqueName, componentsToAdd.Count);

                        var desc = await solutionDesciptor.GetSolutionComponentsDescriptionAsync(componentsToAdd);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            _iWriteToOutput.WriteToOutput(connectionData, desc);
                        }

                        await solutionRep.AddSolutionComponentsAsync(solution.UniqueName, componentsToAdd);
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionNoGlobalOptionSetsToAddInSolutionAllAllreadyInSolutionFormat2, connectionData.Name, solution.UniqueName);
                    }
                }

                await OpenWindowForUnknownGlobalOptionSets(connectionData, commonConfig, unknownGlobalOptionSets);
            }
        }

        #endregion Adding GlobalOptionSet to Solution

        #region Opening Solution in Browser or Explorer

        public async Task ExecuteOpeningSolutionAsync(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, ActionOnComponent actionOnComponent)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , Solution.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSolutionAsync(commonConfig, connectionData, solutionUniqueName, actionOnComponent);
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

        private async Task OpeningSolutionAsync(CommonConfiguration commonConfig, ConnectionData connectionData, string solutionUniqueName, ActionOnComponent actionOnComponent)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new SolutionRepository(service);

            var solution = await repository.GetSolutionByUniqueNameAsync(solutionUniqueName);

            if (solution == null)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , null
                    , null
                    , null
                );

                return;
            }

            _iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);

            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                connectionData.OpenSolutionInWeb(solution.Id);
                service.TryDispose();
            }
            else if (actionOnComponent == ActionOnComponent.OpenInExplorer)
            {
                WindowHelper.OpenSolutionComponentsExplorer(this._iWriteToOutput, service, null, commonConfig, solution.UniqueName, null);
            }
        }

        #endregion Opening Solution in Browser or Explorer

        public async Task ExecuteOpeningSolutionWebResourcesAsync(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool inTextEditor, OpenFilesType openFilesType)
        {
            string operation = string.Format(Properties.OperationNames.OpeningSolutionWebResourcesFormat2, connectionData?.Name, solutionUniqueName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSolutionWebResourcesAsync(commonConfig, connectionData, solutionUniqueName, inTextEditor, openFilesType);
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

        private async Task OpeningSolutionWebResourcesAsync(CommonConfiguration commonConfig, ConnectionData connectionData, string solutionUniqueName, bool inTextEditor, OpenFilesType openFilesType)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new SolutionRepository(service);

            var solution = await repository.GetSolutionByUniqueNameAsync(solutionUniqueName);

            if (solution == null)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , null
                    , null
                    , null
                );

                return;
            }

            _iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);

            using (service.Lock())
            {
                var solutionDescriptor = new SolutionComponentDescriptor(service)
                {
                    WithManagedInfo = true,
                    WithSolutionsInfo = true,
                    WithUrls = true,
                };

                await OpenSolutionWebResources(this._iWriteToOutput, service, solutionDescriptor, commonConfig, solution.Id, solution.UniqueName, inTextEditor, openFilesType);
            }
        }

        public static async Task OpenSolutionWebResources(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor solutionDescriptor
            , CommonConfiguration commonConfig
            , Guid solutionId
            , string solutionUniqueName
            , bool inTextEditor
            , OpenFilesType openFilesType
        )
        {
            var solutionRep = new SolutionComponentRepository(service);

            var components = await solutionRep.GetSolutionComponentsByTypeAsync(solutionId, ComponentType.WebResource, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

            if (!components.Any())
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionSolutionNotContainsWebResourcesFormat2, service.ConnectionData.Name, solutionUniqueName);
                return;
            }

            var desc = await solutionDescriptor.GetSolutionComponentsDescriptionAsync(components);

            if (!string.IsNullOrEmpty(desc))
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourcesToAddToSolutionFormat2, solutionUniqueName, components.Count);
                iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
            }

            var webResourceRepository = new WebResourceRepository(service);

            var webResourcesList = await webResourceRepository.GetListByIdListAsync(components.Select(c => c.ObjectId.Value), ColumnSetInstances.AllColumns);

            var comparedFilesAndWebResources = new List<CompareTuple>();

            var unknownedWebResources = new List<WebResource>();

            bool isTextEditorProgramExists = commonConfig.TextEditorProgramExists();

            var tableOpenedFiles = new FormatTextTableHandler();

            if (isTextEditorProgramExists)
            {
                tableOpenedFiles.SetHeader("File Uri", "Open File in Visual Studio", "Open File in TextEditor", "Select File in Folder");
            }
            else
            {
                tableOpenedFiles.SetHeader("File Uri", "Open File in Visual Studio", "Select File in Folder");
            }

            string solutionDirectoryPath = DTEHelper.Singleton.GetSolutionDirectory();

            foreach (var webResource in webResourcesList.OrderBy(w => w.Name))
            {
                bool success = false;
                string filePath = string.Empty;

                if (service.ConnectionData.TryGetFriendlyPathByGuid(webResource.WebResourceId.Value, out string friendlyPath))
                {
                    success = Helpers.DTEHelper.Singleton.TryFindFileByRelativePath(service.ConnectionData, friendlyPath, out filePath);
                }

                if (success)
                {
                    comparedFilesAndWebResources.Add(new CompareTuple(new SelectedFile(filePath, solutionDirectoryPath), webResource));

                    var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

                    if (isTextEditorProgramExists)
                    {
                        tableOpenedFiles.AddLine(
                            uriFile
                            , UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile)
                            , UrlCommandFilter.GetUriOpenInTextEditorByFileUri(uriFile)
                            , UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile)
                        );
                    }
                    else
                    {
                        tableOpenedFiles.AddLine(
                            uriFile
                            , UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile)
                            , UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile)
                        );
                    }

                    
                }
                else
                {
                    unknownedWebResources.Add(webResource);
                }
            }

            if (tableOpenedFiles.Count > 0)
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.OpenedFilesFormat1, tableOpenedFiles.Count);

                foreach (var item in tableOpenedFiles.GetFormatedLines(false))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, item);
                }
            }

            if (unknownedWebResources.Any())
            {
                desc = await solutionDescriptor.GetSolutionComponentsDescriptionAsync(unknownedWebResources.Select(w => new SolutionComponent()
                {
                    ObjectId = w.WebResourceId.Value,
                    ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                }));

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionWebResourcesNotOpenedFormat2, solutionUniqueName, unknownedWebResources.Count);
                    iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
                }
            }

            if (comparedFilesAndWebResources.Any())
            {
                var listFiles = GetWebResourcesAndFilesWithType(iWriteToOutput, service.ConnectionData, comparedFilesAndWebResources, openFilesType);

                if (!listFiles.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoFilesToMakeDifference);
                    return;
                }

                var orderEnumrator = listFiles.Select(s => s.SelectedFile).OrderBy(s => s.FriendlyFilePath);

                if (inTextEditor)
                {
                    foreach (var item in orderEnumrator)
                    {
                        iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, item.FilePath);
                        iWriteToOutput.OpenFileInTextEditor(service.ConnectionData, item.FilePath);
                    }
                }
                else
                {
                    foreach (var item in orderEnumrator)
                    {
                        iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, item.FilePath);
                        iWriteToOutput.OpenFileInVisualStudio(service.ConnectionData, item.FilePath);
                    }
                }
            }
        }

        public static async Task CompareSolutionWebResources(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor solutionDescriptor
            , CommonConfiguration commonConfig
            , Guid solutionId
            , string solutionUniqueName
            , bool withDetails
        )
        {
            var solutionRep = new SolutionComponentRepository(service);

            var components = await solutionRep.GetSolutionComponentsByTypeAsync(solutionId, ComponentType.WebResource, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

            if (!components.Any())
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionSolutionNotContainsWebResourcesFormat2, service.ConnectionData.Name, solutionUniqueName);
                return;
            }

            var desc = await solutionDescriptor.GetSolutionComponentsDescriptionAsync(components);

            if (!string.IsNullOrEmpty(desc))
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourcesToAddToSolutionFormat2, solutionUniqueName, components.Count);
                iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
            }

            var webResourceRepository = new WebResourceRepository(service);

            var webResourcesList = await webResourceRepository.GetListByIdListAsync(components.Select(c => c.ObjectId.Value), ColumnSetInstances.AllColumns);

            var comparedFilesAndWebResources = new List<CompareTuple>();
            var unknownedWebResources = new List<WebResource>();

            bool isTextEditorProgramExists = commonConfig.TextEditorProgramExists();

            var tableComparedFiles = new FormatTextTableHandler();

            if (isTextEditorProgramExists)
            {
                tableComparedFiles.SetHeader("File Uri", "Open File in Visual Studio", "Open File in TextEditor", "Select File in Folder");
            }
            else
            {
                tableComparedFiles.SetHeader("File Uri", "Open File in Visual Studio", "Select File in Folder");
            }

            string solutionDirectoryPath = DTEHelper.Singleton.GetSolutionDirectory();

            foreach (var webResource in webResourcesList.OrderBy(w => w.Name))
            {
                bool success = false;
                string filePath = string.Empty;

                if (service.ConnectionData.TryGetFriendlyPathByGuid(webResource.WebResourceId.Value, out string friendlyPath))
                {
                    success = DTEHelper.Singleton.TryFindFileByRelativePath(service.ConnectionData, friendlyPath, out filePath);
                }

                if (success)
                {
                    comparedFilesAndWebResources.Add(new CompareTuple(new SelectedFile(filePath, solutionDirectoryPath), webResource));

                    var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

                    if (isTextEditorProgramExists)
                    {
                        tableComparedFiles.AddLine(
                            uriFile
                            , UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile)
                            , UrlCommandFilter.GetUriOpenInTextEditorByFileUri(uriFile)
                            , UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile)
                        );
                    }
                    else
                    {
                        tableComparedFiles.AddLine(
                            uriFile
                            , UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile)
                            , UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile)
                        );
                    }
                }
                else
                {
                    unknownedWebResources.Add(webResource);
                }
            }

            if (tableComparedFiles.Count > 0)
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ComparedFilesFormat1, tableComparedFiles.Count);

                foreach (var item in tableComparedFiles.GetFormatedLines(false))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, item);
                }
            }

            if (unknownedWebResources.Any())
            {
                desc = await solutionDescriptor.GetSolutionComponentsDescriptionAsync(unknownedWebResources.Select(w => new SolutionComponent()
                {
                    ObjectId = w.WebResourceId.Value,
                    ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                }));

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionWebResourcesNotOpenedFormat2, solutionUniqueName, unknownedWebResources.Count);
                    iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
                }
            }

            if (comparedFilesAndWebResources.Any())
            {
                CompareTuples(
                    iWriteToOutput
                    , service.ConnectionData
                    , comparedFilesAndWebResources.Count
                    , withDetails
                    , Enumerable.Empty<string>()
                    , Enumerable.Empty<string>()
                    , comparedFilesAndWebResources
                    , Enumerable.Empty<CompareTuple>()
                );
            }
        }

        public static async Task ShowDifferenceSolutionWebResources(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor solutionDescriptor
            , CommonConfiguration commonConfig
            , Guid solutionId
            , string solutionUniqueName
            , OpenFilesType openFilesType
        )
        {
            var solutionRep = new SolutionComponentRepository(service);

            var components = await solutionRep.GetSolutionComponentsByTypeAsync(solutionId, ComponentType.WebResource, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

            if (!components.Any())
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionSolutionNotContainsWebResourcesFormat2, service.ConnectionData.Name, solutionUniqueName);
                return;
            }

            var desc = await solutionDescriptor.GetSolutionComponentsDescriptionAsync(components);

            if (!string.IsNullOrEmpty(desc))
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourcesToAddToSolutionFormat2, solutionUniqueName, components.Count);
                iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
            }

            var webResourceRepository = new WebResourceRepository(service);

            var webResourcesList = await webResourceRepository.GetListByIdListAsync(components.Select(c => c.ObjectId.Value), ColumnSetInstances.AllColumns);

            var comparedFilesAndWebResources = new List<CompareTuple>();
            var unknownedWebResources = new List<WebResource>();

            bool isTextEditorProgramExists = commonConfig.TextEditorProgramExists();

            var tableComparedFiles = new FormatTextTableHandler();

            if (isTextEditorProgramExists)
            {
                tableComparedFiles.SetHeader("File Uri", "Open File in Visual Studio", "Open File in TextEditor", "Select File in Folder");
            }
            else
            {
                tableComparedFiles.SetHeader("File Uri", "Open File in Visual Studio", "Select File in Folder");
            }

            string solutionDirectoryPath = DTEHelper.Singleton.GetSolutionDirectory();

            foreach (var webResource in webResourcesList.OrderBy(w => w.Name))
            {
                bool success = false;
                string filePath = string.Empty;

                if (service.ConnectionData.TryGetFriendlyPathByGuid(webResource.WebResourceId.Value, out string friendlyPath))
                {
                    success = DTEHelper.Singleton.TryFindFileByRelativePath(service.ConnectionData, friendlyPath, out filePath);
                }

                if (success)
                {
                    comparedFilesAndWebResources.Add(new CompareTuple(new SelectedFile(filePath, solutionDirectoryPath), webResource));

                    var uriFile = new Uri(filePath, UriKind.Absolute).AbsoluteUri;

                    if (isTextEditorProgramExists)
                    {
                        tableComparedFiles.AddLine(
                            uriFile
                            , UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile)
                            , UrlCommandFilter.GetUriOpenInTextEditorByFileUri(uriFile)
                            , UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile)
                        );
                    }
                    else
                    {
                        tableComparedFiles.AddLine(
                            uriFile
                            , UrlCommandFilter.GetUriOpenInVisualStudioByFileUri(uriFile)
                            , UrlCommandFilter.GetUriSelectFileInFolderByFileUri(uriFile)
                        );
                    }
                }
                else
                {
                    unknownedWebResources.Add(webResource);
                }
            }

            if (tableComparedFiles.Count > 0)
            {
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ComparedFilesFormat1, tableComparedFiles.Count);

                foreach (var item in tableComparedFiles.GetFormatedLines(false))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, item);
                }
            }

            if (unknownedWebResources.Any())
            {
                desc = await solutionDescriptor.GetSolutionComponentsDescriptionAsync(unknownedWebResources.Select(w => new SolutionComponent()
                {
                    ObjectId = w.WebResourceId.Value,
                    ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                }));

                if (!string.IsNullOrEmpty(desc))
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionWebResourcesNotOpenedFormat2, solutionUniqueName, unknownedWebResources.Count);
                    iWriteToOutput.WriteToOutput(service.ConnectionData, desc);
                }
            }

            if (comparedFilesAndWebResources.Any())
            {
                var listFilesToDifference = GetWebResourcesAndFilesWithType(iWriteToOutput, service.ConnectionData, comparedFilesAndWebResources, openFilesType);

                if (!listFilesToDifference.Any())
                {
                    iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NoFilesToMakeDifference);
                    return;
                }

                iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.StartingCompareProgramForCountFilesFormat1, listFilesToDifference.Count());

                foreach (var item in listFilesToDifference.OrderBy(file => file.SelectedFile.FilePath))
                {
                    var selectedFile = item.SelectedFile;
                    var webresource = item.WebResource;

                    if (webresource != null)
                    {
                        var contentWebResource = webresource.Content;

                        var webResourceName = webresource.Name;

                        var array = Convert.FromBase64String(contentWebResource);

                        string tempFilePath = FileOperations.GetNewTempFilePath(webResourceName, selectedFile.Extension);

                        File.WriteAllBytes(tempFilePath, array);

                        string fileLocalPath = selectedFile.FilePath;
                        string fileLocalTitle = selectedFile.FileName;

                        string filePath2 = tempFilePath;
                        string fileTitle2 = service.ConnectionData.Name + "." + selectedFile.FileName + " - " + tempFilePath;

                        await iWriteToOutput.ProcessStartProgramComparerAsync(service.ConnectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
                    }
                }
            }
        }
    }
}
