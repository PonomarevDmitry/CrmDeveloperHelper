using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class SolutionDescriptor
    {
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;
        private readonly SolutionComponentDescriptor _descriptor;

        public SolutionDescriptor(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            )
        {
            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._descriptor = descriptor;
        }

        public Task CreateFileWithSolutionComponentsAsync(string filePath, Guid solutionId)
        {
            return Task.Run(async () => await CreateFileWithSolutionComponents(filePath, solutionId));
        }

        private async Task CreateFileWithSolutionComponents(string filePath, Guid solutionId)
        {
            try
            {
                var repositorySolution = new SolutionRepository(_service);

                var solution = await repositorySolution.GetSolutionByIdAsync(solutionId);

                var repository = new SolutionComponentRepository(_service);

                var components = await repository.GetSolutionComponentsAsync(solutionId, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                StringBuilder strFile = new StringBuilder();
                string message = null;

                message = string.Format("Analyzing Solution Components '{0}' at {1}.", solution.UniqueName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);

                strFile.AppendLine(message);

                message = string.Format("Solution '{0}' has {1} components:", solution.UniqueName, components.Count().ToString());

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);

                strFile.AppendLine().AppendLine(message);

                message = await _descriptor.GetSolutionComponentsDescriptionAsync(components);

                strFile.AppendLine(message);

                File.WriteAllText(filePath, strFile.ToString(), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        public Task CreateFileWithSolutionImageAsync(string filePath, Guid solutionId)
        {
            return Task.Run(async () => await CreateFileWithSolutionImage(filePath, solutionId));
        }

        private async Task CreateFileWithSolutionImage(string filePath, Guid solutionId)
        {
            try
            {
                var repositorySolution = new SolutionRepository(_service);

                //var solution = await repositorySolution.GetSolutionByIdAsync(solutionId);

                var repository = new SolutionComponentRepository(_service);

                var components = await repository.GetSolutionComponentsAsync(solutionId, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                await CreateSolutionImageWithComponents(filePath, components);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        public Task CreateSolutionImageWithComponentsAsync(string filePath, IEnumerable<SolutionComponent> components)
        {
            return Task.Run(async () => await CreateSolutionImageWithComponents(filePath, components));
        }

        private async Task CreateSolutionImageWithComponents(string filePath, IEnumerable<SolutionComponent> components)
        {
            List<SolutionImageComponent> imageComponents = await _descriptor.GetSolutionImageComponentsListAsync(components);

            SolutionImage image = new SolutionImage()
            {
                ConnectionName = _service.ConnectionData.Name,

                ConnectionOrganizationName = _service.ConnectionData.UniqueOrgName,
                ConnectionDiscoveryService = _service.ConnectionData.DiscoveryUrl,
                ConnectionOrganizationService = _service.ConnectionData.OrganizationUrl,
                ConnectionPublicUrl = _service.ConnectionData.PublicUrl,

                MachineName = Environment.MachineName,
                ExecuteUserDomainName = Environment.UserDomainName,
                ExecuteUserName = Environment.UserName,

                ConnectionSystemUserName = _service.ConnectionData.GetUsername,
            };

            foreach (var item in imageComponents)
            {
                if (!image.Components.Contains(item))
                {
                    image.Components.Add(item);
                }
            }

            await image.SaveAsync(filePath);
        }

        public Task CreateFileWithSolutionDependenciesForUninstallAsync(string filePath, Guid solutionId, ComponentsGroupBy showComponents, string showString)
        {
            return Task.Run(async () => await CreateFileWithSolutionDependenciesForUninstall(filePath, solutionId, showComponents, showString));
        }

        private async Task CreateFileWithSolutionDependenciesForUninstall(string filePath, Guid solutionId, ComponentsGroupBy showComponents, string showString)
        {
            try
            {
                var repository = new SolutionRepository(_service);

                var solution = await repository.GetSolutionByIdAsync(solutionId);

                string message = null;

                message = string.Format("Analyzing Solution '{0}' {1} at {2}.", solution.UniqueName, showString, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);

                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine(message);

                var dependencyRepository = new DependencyRepository(_service);

                var listComponents = await dependencyRepository.GetSolutionDependenciesForUninstallAsync(solution.UniqueName);

                message = string.Format("Solution '{0}' {1} {2}:", solution.UniqueName, showString, listComponents.Count.ToString());

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);

                strFile
                    .AppendLine()
                    .AppendLine(message)
                    .AppendLine();

                message = await new DependencyDescriptionHandler(_descriptor).GetDescriptionFullAsynс(listComponents, showComponents);

                strFile.AppendLine(message);

                File.WriteAllText(filePath, strFile.ToString(), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        public Task CreateFileWithSolutionMissingDependenciesAsync(string filePath, Guid solutionId, ComponentsGroupBy showComponents, string showString)
        {
            return Task.Run(async () => await CreateFileWithSolutionMissingDependencies(filePath, solutionId, showComponents, showString));
        }

        private async Task CreateFileWithSolutionMissingDependencies(string filePath, Guid solutionId, ComponentsGroupBy showComponents, string showString)
        {
            try
            {
                var repository = new SolutionRepository(_service);

                var solution = await repository.GetSolutionByIdAsync(solutionId);

                string message = null;

                message = string.Format("Analyzing Solution '{0}' {1} at {2}.", solution.UniqueName, showString, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);

                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine(message);

                var dependencyRepository = new DependencyRepository(_service);

                var listComponents = await dependencyRepository.GetSolutionMissingDependenciesAsync(solution.UniqueName);

                message = string.Format("Solution '{0}' {1} {2}:", solution.UniqueName, showString, listComponents.Count.ToString());

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);

                strFile
                    .AppendLine()
                    .AppendLine(message)
                    .AppendLine();

                message = await new DependencyDescriptionHandler(_descriptor).GetDescriptionFullAsynс(listComponents, showComponents);

                strFile.AppendLine(message);

                File.WriteAllText(filePath, strFile.ToString(), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        public static List<SolutionComponent> GetComponentsInFirstNotSecond(IEnumerable<SolutionComponent> componentsSource, IEnumerable<SolutionComponent> componentsTarget)
        {
            List<SolutionComponent> result = new List<SolutionComponent>();

            HashSet<Tuple<int, Guid>> hashTarget = new HashSet<Tuple<int, Guid>>(componentsTarget.Where(e => e.ComponentType != null && e.ObjectId.HasValue).Select(e => Tuple.Create(e.ComponentType.Value, e.ObjectId.Value)));

            HashSet<Tuple<int, Guid>> hashAdded = new HashSet<Tuple<int, Guid>>();

            foreach (var entityInSource in componentsSource)
            {
                int componentType1 = entityInSource.ComponentType.Value;
                Guid objectId1 = entityInSource.ObjectId.Value;

                var key = Tuple.Create(componentType1, objectId1);

                if (!hashTarget.Contains(key) && hashAdded.Add(key))
                {
                    result.Add(entityInSource);
                }
            }

            return result;
        }

        public Task FindUniqueComponentsInSolutionsAsync(Guid idSolution1, Guid idSolution2)
        {
            return Task.Run(async () => await FindUniqueComponentsInSolutions(idSolution1, idSolution2));
        }

        private async Task FindUniqueComponentsInSolutions(Guid idSolution1, Guid idSolution2)
        {
            SolutionRepository repositorySolution = new SolutionRepository(_service);

            var solution1 = await repositorySolution.GetSolutionByIdAsync(idSolution1);
            var solution2 = await repositorySolution.GetSolutionByIdAsync(idSolution2);

            SolutionComponentRepository repository = new SolutionComponentRepository(_service);

            var components1 = await repository.GetSolutionComponentsAsync(idSolution1, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));
            var components2 = await repository.GetSolutionComponentsAsync(idSolution2, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

            var componentsOnlyIn1 = GetComponentsInFirstNotSecond(components1, components2);
            var componentsOnlyIn2 = GetComponentsInFirstNotSecond(components2, components1);
            
            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionComponentsCountAndUniqueFormat4, solution1.UniqueName, components1.Count.ToString(), componentsOnlyIn1.Count.ToString(), solution2.UniqueName);
            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SolutionComponentsCountAndUniqueFormat4, solution2.UniqueName, components2.Count.ToString(), componentsOnlyIn2.Count.ToString(), solution1.UniqueName);
        }

        public Task CreateFileWithUniqueComponentsInSolution1Async(
            string filePath
            , Guid idSolution1
            , Guid idSolution2
            )
        {
            return Task.Run(async () => await CreateFileWithUniqueComponentsInSolution1(filePath, idSolution1, idSolution2));
        }

        private async Task CreateFileWithUniqueComponentsInSolution1(
            string filePath
            , Guid idSolution1
            , Guid idSolution2
        )
        {
            try
            {
                SolutionRepository repositorySolution = new SolutionRepository(_service);

                var solution1 = await repositorySolution.GetSolutionByIdAsync(idSolution1);
                var solution2 = await repositorySolution.GetSolutionByIdAsync(idSolution2);

                SolutionComponentRepository repository = new SolutionComponentRepository(_service);

                var components1 = await repository.GetSolutionComponentsAsync(solution1.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));
                var components2 = await repository.GetSolutionComponentsAsync(solution2.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentsOnlyIn1 = GetComponentsInFirstNotSecond(components1, components2);
                var componentsOnlyIn2 = GetComponentsInFirstNotSecond(components2, components1);

                await CreateFileWithComponentsInSolution1(filePath, solution1.UniqueName, solution2.UniqueName, components1.Count, components2.Count, componentsOnlyIn1, componentsOnlyIn2.Count);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        public Task CreateFileWithComponentsInSolution1Async(
            string filePath
            , string sourceName
            , string solutionTargetUniqueName
            , int componentsSourceCount
            , int componentsTargetCount
            , List<SolutionComponent> componentesOnlyInSource
            , int componentesOnlyInTargetCount
            )
        {
            return Task.Run(async () => await CreateFileWithComponentsInSolution1(filePath
                , sourceName
                , solutionTargetUniqueName
                , componentsSourceCount
                , componentsTargetCount
                , componentesOnlyInSource
                , componentesOnlyInTargetCount)
            );
        }

        private async Task CreateFileWithComponentsInSolution1(
            string filePath
            , string sourceName
            , string solutionTargetUniqueName
            , int componentsSourceCount
            , int componentsTargetCount
            , List<SolutionComponent> componentesOnlyInSource
            , int componentesOnlyInTargetCount
        )
        {
            try
            {
                StringBuilder strFile = new StringBuilder();
                string message = null;

                message = string.Format("Analyzing Solution Components '{0}' and '{1}' as {2}.", sourceName, solutionTargetUniqueName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);
                strFile.AppendLine(message);

                message = string.Format(Properties.OutputStrings.SolutionComponentsCountAndUniqueFormat4, sourceName, componentsSourceCount, componentesOnlyInSource.Count, solutionTargetUniqueName);

                this._iWriteToOutput.WriteToOutput(message);
                strFile.AppendLine(message);

                message = string.Format(Properties.OutputStrings.SolutionComponentsCountAndUniqueFormat4, solutionTargetUniqueName, componentsTargetCount, componentesOnlyInTargetCount, sourceName);

                this._iWriteToOutput.WriteToOutput(message);
                strFile.AppendLine(message);

                message = string.Format("Unique Solution Components '{0}': {1}", sourceName, componentesOnlyInSource.Count.ToString());

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(message);
                strFile.AppendLine().AppendLine(message);

                message = await _descriptor.GetSolutionComponentsDescriptionAsync(componentesOnlyInSource);

                strFile.AppendLine(message);

                File.WriteAllText(filePath, strFile.ToString(), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }
    }
}
