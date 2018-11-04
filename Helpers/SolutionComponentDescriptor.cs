using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private const string formatSpacer = "    {0}";

        private const string unknowedMessage = "Default Description for Unknowned Solution Components:";

        private ConcurrentDictionary<int, ISolutionComponentDescriptionBuilder> _cacheBuilders = new ConcurrentDictionary<int, ISolutionComponentDescriptionBuilder>();

        private readonly IOrganizationServiceExtented _service;

        public bool WithUrls { get; set; } = false;

        public bool WithSolutionsInfo { get; set; } = true;

        public bool WithManagedInfo { get; set; } = true;

        public SolutionComponentMetadataSource MetadataSource { get; private set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionComponentDescriptor(IOrganizationServiceExtented service, bool withUrls)
            : this(service, withUrls, null)
        {

        }

        public SolutionComponentDescriptor(IOrganizationServiceExtented service, bool withUrls, SolutionComponentMetadataSource metadataSource)
        {
            this._service = service;
            this.WithUrls = withUrls;
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

            var groups = components.Where(c => c.ComponentType != null).GroupBy(comp => comp.ComponentType.Value).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    if (builder.Length > 0)
                    {
                        builder.AppendLine();
                    }

                    string name = gr.First().ComponentTypeName;

                    builder.AppendFormat("ComponentType:   {0} ({1})            Count: {2}"
                        , name
                        , gr.Key.ToString()
                        , gr.Count().ToString()
                        ).AppendLine();

                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    descriptionBuilder.GenerateDescription(builder, gr, WithManagedInfo, WithSolutionsInfo, WithUrls);
                }
                catch (Exception ex)
                {
                    builder.AppendLine().AppendLine("Exception");

                    builder.AppendLine().AppendLine(DTEHelper.GetExceptionDescription(ex)).AppendLine();

                    DTEHelper.Singleton.WriteErrorToOutput(ex);
                }
            }

            return builder.ToString();
        }

        public Task<List<SolutionImageComponent>> GetSolutionImageComponentsListAsync(IEnumerable<SolutionComponent> components)
        {
            return Task.Run(() => GetSolutionImageComponentsList(components));
        }

        private List<SolutionImageComponent> GetSolutionImageComponentsList(IEnumerable<SolutionComponent> components)
        {
            List<SolutionImageComponent> result = new List<SolutionImageComponent>();

            var groups = components.Where(c => c.ComponentType != null).GroupBy(comp => comp.ComponentType.Value).OrderBy(gr => gr.Key);

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
                    DTEHelper.Singleton.WriteErrorToOutput(ex);
                }
            }

            result.Sort(new SolutionImageComponentSorter());

            return result;
        }

        public Task<List<SolutionComponent>> GetSolutionComponentsListAsync(IEnumerable<SolutionImageComponent> components)
        {
            return Task.Run(() => GetSolutionComponentsList(components));
        }

        private List<SolutionComponent> GetSolutionComponentsList(IEnumerable<SolutionImageComponent> components)
        {
            List<SolutionComponent> result = new List<SolutionComponent>();

            var groups = components.GroupBy(comp => comp.ComponentType).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    foreach (var item in gr)
                    {
                        descriptionBuilder.FillSolutionComponent(result, item);
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.Singleton.WriteErrorToOutput(ex);
                }
            }

            return result;
        }

        public Task<List<SolutionComponent>> LoadSolutionComponentsFromZipFileAsync(string selectedPath)
        {
            return Task.Run(() => LoadSolutionComponentsFromZipFile(selectedPath));
        }

        private List<SolutionComponent> LoadSolutionComponentsFromZipFile(string selectedPath)
        {
            List<SolutionComponent> result = new List<SolutionComponent>();

            if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
            {
                return result;
            }

            GetSolutionFiles(selectedPath, out XDocument docSolution, out XDocument docCustomizations);

            if (docSolution == null || docCustomizations == null)
            {
                return result;
            }

            var nodesRootcomponents = docSolution.XPathSelectElements("ImportExportXml/SolutionManifest/RootComponents/RootComponent");

            var groups = nodesRootcomponents.Where(e => e.Attribute("type") != null).GroupBy(e => (int)e.Attribute("type")).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    foreach (var item in gr)
                    {
                        descriptionBuilder.FillSolutionComponentFromXml(result, item, docCustomizations);
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.Singleton.WriteErrorToOutput(ex);
                }
            }

            return result;
        }

        private void GetSolutionFiles(string selectedPath, out XDocument docSolution, out XDocument docCustomizations)
        {
            docSolution = null;
            docCustomizations = null;

            using (FileStream fileStream = new FileStream(selectedPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipPackage package = (ZipPackage)ZipPackage.Open(fileStream, FileMode.Open, FileAccess.Read))
                {
                    {
                        ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/solution.xml", UriKind.Relative));

                        if (part != null)
                        {
                            using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                            {
                                docSolution = XDocument.Load(streamPart);
                            }
                        }
                    }

                    {
                        ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/customizations.xml", UriKind.Relative));

                        if (part != null)
                        {
                            using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                            {
                                docCustomizations = XDocument.Load(streamPart);
                            }
                        }
                    }
                }
            }
        }

        public string GetComponentDescription(int type, Guid idEntity)
        {
            var solutionComponent = new SolutionComponent()
            {
                ObjectId = idEntity,
                ComponentType = new OptionSetValue(type),
            };

            var descriptionBuilder = GetDescriptionBuilder(type);

            return descriptionBuilder.GenerateDescriptionSingle(solutionComponent, WithManagedInfo, WithSolutionsInfo, WithUrls);
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

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetLinkedEntityName(solutionComponent);
        }
    }
}