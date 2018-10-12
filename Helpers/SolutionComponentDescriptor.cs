using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private const string formatSpacer = "    {0}";

        private const string unknowedMessage = "Default Description for Unknowned Solution Components:";

        private ConcurrentDictionary<int, ISolutionComponentDescriptionBuilder> _cacheBuilders = new ConcurrentDictionary<int, ISolutionComponentDescriptionBuilder>();

        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;
        private readonly bool _withUrls;

        public SolutionComponentMetadataSource MetadataSource { get; private set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionComponentDescriptor(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, bool withUrls)
            : this(iWriteToOutput, service, withUrls, null)
        {

        }

        public SolutionComponentDescriptor(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, bool withUrls, SolutionComponentMetadataSource metadataSource)
        {
            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._withUrls = withUrls;
            this.MetadataSource = metadataSource;

            if (this.MetadataSource == null)
            {
                this.MetadataSource = new SolutionComponentMetadataSource(_service);
            }
        }

        public Task<string> GetSolutionComponentsDescriptionAsync(IEnumerable<SolutionComponent> components)
        {
            return Task.Run(() => GetSolutionComponentsDescription(components));
        }

        private string GetSolutionComponentsDescription(IEnumerable<SolutionComponent> components)
        {
            StringBuilder builder = new StringBuilder();

            var groups = components.GroupBy(comp => comp.ComponentType.Value).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    if (builder.Length > 0)
                    {
                        builder.AppendLine();
                    }

                    string name = components.First().ComponentTypeName;

                    builder.AppendFormat("ComponentType:   {0} ({1})            Count: {2}"
                        , name
                        , gr.Key.ToString()
                        , gr.Count().ToString()
                        ).AppendLine();

                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    descriptionBuilder.GenerateDescription(builder, gr, _withUrls);
                }
                catch (Exception ex)
                {
                    builder.AppendLine().AppendLine("Exception");

                    builder.AppendLine().AppendLine(DTEHelper.GetExceptionDescription(ex)).AppendLine();

                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            return builder.ToString();
        }

        public Task<List<SolutionImageComponent>> GetSolutionImageComponentsAsync(IEnumerable<SolutionComponent> components)
        {
            return Task.Run(() => GetSolutionImageComponents(components));
        }

        private List<SolutionImageComponent> GetSolutionImageComponents(IEnumerable<SolutionComponent> components)
        {
            List<SolutionImageComponent> result = new List<SolutionImageComponent>();

            var groups = components.GroupBy(comp => comp.ComponentType.Value).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    foreach (var item in gr)
                    {
                        descriptionBuilder.FillSolutionImageComponent(result, item);
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            return result;
        }

        public string GetComponentDescription(int type, Guid idEntity)
        {
            var solutionComponent = new SolutionComponent()
            {
                ObjectId = idEntity,
                ComponentType = new OptionSetValue(type),
            };

            var descriptionBuilder = GetDescriptionBuilder(type);

            return descriptionBuilder.GenerateDescriptionSingle(solutionComponent, _withUrls);
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetCustomizableName(solutionComponent);
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetManagedName(solutionComponent);
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "Unknown";
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetName(solutionComponent);
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetDisplayName(solutionComponent);
        }

        public string GetFileName(string connectionName, int type, Guid objectId, string fieldTitle, string extension)
        {
            var descriptionBuilder = GetDescriptionBuilder(type);

            return descriptionBuilder.GetFileName(connectionName, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns(int componentType)
        {
            var descriptionBuilder = GetDescriptionBuilder(componentType);

            return descriptionBuilder.GetComponentColumns();
        }

        private ISolutionComponentDescriptionBuilder GetDescriptionBuilder(int componentType)
        {
            if (!_cacheBuilders.ContainsKey(componentType))
            {
                var builder = new SolutionComponentDescriptionBuilderFactory().CreateBuilder(_service, this.MetadataSource, componentType);

                _cacheBuilders.TryAdd(componentType, builder);
            }

            return _cacheBuilders[componentType];
        }

        public List<T> GetEntities<T>(int componentType, IEnumerable<Guid> components) where T : Entity
        {
            return GetEntities<T>(componentType, components.Select(id => (Guid?)id));
        }

        public List<T> GetEntities<T>(int componentType, IEnumerable<Guid?> components) where T : Entity
        {
            var descriptionBuilder = GetDescriptionBuilder(componentType);

            return descriptionBuilder.GetEntities<T>(components);
        }

        public T GetEntity<T>(int componentType, Guid idEntity) where T : Entity
        {
            var descriptionBuilder = GetDescriptionBuilder(componentType);

            return descriptionBuilder.GetEntity<T>(idEntity);
        }
    }
}