using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
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
    public class DependencyDescriptionHandler
    {
        private SolutionComponentDescriptor _descriptor;

        public DependencyDescriptionHandler(SolutionComponentDescriptor descriptor)
        {
            this._descriptor = descriptor;
        }

        public Task<string> GetDescriptionDependentAsync(List<Dependency> coll)
        {
            return Task.Run(async () => await GetDescriptionDependent(coll));
        }

        private async Task<string> GetDescriptionDependent(List<Dependency> coll)
        {
            var list = coll.Select(d => d.DependentToSolutionComponent()).ToList();

            return await _descriptor.GetSolutionComponentsDescriptionAsync(list);
        }

        public Task<string> GetDescriptionRequiredAsync(List<Dependency> coll)
        {
            return Task.Run(async () => await GetDescriptionRequired(coll));
        }

        private async Task<string> GetDescriptionRequired(List<Dependency> coll)
        {
            var list = coll.Select(d => d.RequiredToSolutionComponent()).ToList();

            return await _descriptor.GetSolutionComponentsDescriptionAsync(list);
        }

        public Task<string> GetDescriptionFullAsynс(List<Dependency> list, ComponentsGroupBy showInList)
        {
            return Task.Run(async () => await GetDescriptionFull(list, showInList));
        }

        private async Task<string> GetDescriptionFull(List<Dependency> list, ComponentsGroupBy showInList)
        {
            if (showInList == ComponentsGroupBy.RequiredComponents)
            {
                return await GetDescriptionGroupByRequiredComponents(list);
            }
            else
            {
                return await GetDescriptionGroupByDependentComponents(list);
            }
        }

        private async Task<string> GetDescriptionGroupByRequiredComponents(List<Dependency> list)
        {
            StringBuilder builder = new StringBuilder();
            string format = "Dependent components:";

            var groupsComponent = list
                .GroupBy(d => d.RequiredComponentType)
                .OrderBy(gr => gr.Key?.Value);

            foreach (var grComp in groupsComponent)
            {
                var groupsObject = grComp
                      .GroupBy(d => new { RequiredComponentType = d.RequiredComponentType.Value, RequiredComponentObjectId = d.RequiredComponentObjectId.Value });

                Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var grObj in groupsObject)
                {
                    try
                    {
                        var entityDesc = _descriptor.GetComponentDescription(grObj.Key.RequiredComponentType, grObj.Key.RequiredComponentObjectId);

                        var grList = grObj.Select(d => d.DependentToSolutionComponent()).ToList();

                        var collectionDesc = await _descriptor.GetSolutionComponentsDescriptionAsync(grList);

                        if (!string.IsNullOrEmpty(collectionDesc))
                        {
                            dict.Add(entityDesc, collectionDesc);
                        }
                    }
                    catch (Exception ex)
                    {
                        builder.AppendLine().AppendLine("Exception");
                        builder.AppendLine().AppendLine(DTEHelper.GetExceptionDescription(ex)).AppendLine();

                        DTEHelper.WriteExceptionToOutput(ex);

#if DEBUG
                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                    }
                }

                foreach (var entityDesc in dict.Keys.OrderBy(s => s))
                {
                    if (builder.Length > 0)
                    {
                        builder
                            .AppendLine()
                            .AppendLine(new string('-', 100))
                            .AppendLine();
                    }

                    builder.AppendLine(entityDesc);
                    builder.AppendLine(format);
                    builder.AppendLine(dict[entityDesc]);
                }
            }

            return builder.ToString();
        }

        private async Task<string> GetDescriptionGroupByDependentComponents(List<Dependency> list)
        {
            StringBuilder builder = new StringBuilder();
            string format = "Required components:";

            var groupsComponent = list
                .GroupBy(d => d.DependentComponentType)
                .OrderBy(gr => gr.Key?.Value);

            foreach (var grComp in groupsComponent)
            {
                var groupsObject = grComp
                     .GroupBy(d => new { d.DependentComponentType, DependentComponentObjectId = d.DependentComponentObjectId.Value });

                Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var grObj in groupsObject)
                {
                    try
                    {
                        var entityDesc = _descriptor.GetComponentDescription(grObj.Key.DependentComponentType.Value, grObj.Key.DependentComponentObjectId);

                        var grList = grObj.Select(d => d.RequiredToSolutionComponent()).ToList();

                        var collectionDesc = await _descriptor.GetSolutionComponentsDescriptionAsync(grList);

                        if (!string.IsNullOrEmpty(collectionDesc))
                        {
                            dict.Add(entityDesc, collectionDesc);
                        }
                    }
                    catch (Exception ex)
                    {
                        builder.AppendLine().AppendLine("Exception");
                        builder.AppendLine().AppendLine(DTEHelper.GetExceptionDescription(ex)).AppendLine();

                        DTEHelper.WriteExceptionToOutput(ex);

#if DEBUG
                        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                    }
                }

                foreach (var entityDesc in dict.Keys.OrderBy(s => s))
                {
                    if (builder.Length > 0)
                    {
                        builder
                            .AppendLine()
                            .AppendLine(new string('-', 100))
                            .AppendLine();
                    }

                    builder.AppendLine(entityDesc);
                    builder.AppendLine(format);
                    builder.AppendLine(dict[entityDesc]);
                }
            }

            return builder.ToString();
        }

        public async Task<bool> CreateFileWithDependentComponentsAsync(DependencyRepository dependencyRepository, string connectionInfo, string filePath, ComponentType componentType, Guid idComponent, string componentName)
        {
            var coll = await dependencyRepository.GetDependentComponentsAsync((int)componentType, idComponent);

            string description = await this.GetDescriptionDependentAsync(coll);

            if (string.IsNullOrEmpty(description))
            {
                return false;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(connectionInfo).AppendLine();
            content.AppendFormat("Dependent Components for {0} '{1}' at {2}", componentType.ToString(), componentName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)).AppendLine();

            content.Append(description);

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return true;
        }
    }
}