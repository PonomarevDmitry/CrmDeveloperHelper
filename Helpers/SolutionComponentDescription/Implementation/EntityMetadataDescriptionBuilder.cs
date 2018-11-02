using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class EntityMetadataDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;
        private const string unknowedMessage = DefaultSolutionComponentDescriptionBuilder.unknowedMessage;

        private readonly SolutionComponentMetadataSource _source;

        public int ComponentTypeValue => (int)ComponentType.Entity;

        public ComponentType? ComponentTypeEnum => ComponentType.Entity;

        public string EntityLogicalName => string.Empty;

        public string EntityPrimaryIdAttribute => string.Empty;

        public EntityMetadataDescriptionBuilder(SolutionComponentMetadataSource source)
        {
            this._source = source;
        }

        public List<T> GetEntities<T>(IEnumerable<Guid> components) where T : Entity
        {
            return new List<T>();
        }

        public List<T> GetEntities<T>(IEnumerable<Guid?> components) where T : Entity
        {
            return new List<T>();
        }

        public T GetEntity<T>(Guid idEntity) where T : Entity
        {
            return null;
        }

        public void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return;
            }

            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                var behaviour = solutionComponent.RootComponentBehavior?.Value;

                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.Entity,
                    SchemaName = metaData.LogicalName,
                    RootComponentBehavior = behaviour.GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingleInternal(metaData, behaviour, false, true, true),
                });
            }
        }

        public void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null || string.IsNullOrEmpty(solutionImageComponent.SchemaName))
            {
                return;
            }

            EntityMetadata metaData = _source.GetEntityMetadata(solutionImageComponent.SchemaName);

            if (metaData != null)
            {
                FillSolutionComponentInternal(result, metaData, solutionImageComponent.RootComponentBehavior);
            }
        }

        public void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            if (elementRootComponent == null)
            {
                return;
            }

            var schemaName = DefaultSolutionComponentDescriptionBuilder.GetSchemaNameFromXml(elementRootComponent);

            if (!string.IsNullOrEmpty(schemaName))
            {
                EntityMetadata metaData = _source.GetEntityMetadata(schemaName);

                if (metaData != null)
                {
                    int? behaviour = DefaultSolutionComponentDescriptionBuilder.GetBehaviorFromXml(elementRootComponent);

                    FillSolutionComponentInternal(result, metaData, behaviour);
                }
            }
        }

        private void FillSolutionComponentInternal(ICollection<SolutionComponent> result, EntityMetadata metaData, int? behaviour)
        {
            var component = new SolutionComponent()
            {
                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                ObjectId = metaData.MetadataId.Value,
                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            };

            if (behaviour.HasValue)
            {
                component.RootComponentBehavior = new OptionSetValue(behaviour.Value);
            }

            result.Add(component);
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "Behaviour");

            if (withManaged)
            {
                handler.AppendHeader("IsManaged");
            }

            if (withUrls)
            {
                handler.AppendHeader("Url");
            }

            foreach (var comp in components)
            {
                string behavior = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                EntityMetadata metaData = _source.GetEntityMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        metaData.LogicalName
                        , behavior
                    });

                    if (withManaged)
                    {
                        values.Add(metaData.IsManaged.ToString());
                    }

                    if (withUrls)
                    {
                        values.Add(_source.Service.ConnectionData?.GetEntityMetadataUrl(metaData.MetadataId.Value));
                    }

                    handler.AddLine(values);
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString(), behavior);
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                int? behaviourCode = solutionComponent.RootComponentBehavior?.Value;

                return GenerateDescriptionSingleInternal(metaData, behaviourCode, withUrls, withManaged, withSolutionInfo);
            }

            return solutionComponent.ToString();
        }

        private string GenerateDescriptionSingleInternal(EntityMetadata metaData, int? behaviourCode, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            string behavior = string.Empty;

            if (behaviourCode.HasValue)
            {
                behavior = SolutionComponent.GetRootComponentBehaviorName(behaviourCode.Value);
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "Behaviour");

            if (withManaged)
            {
                handler.AppendHeader("IsManaged");
            }

            if (withUrls)
            {
                handler.AppendHeader("Url");
            }

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                metaData.LogicalName
                , behavior
            });

            if (withManaged)
            {
                values.Add(metaData.IsManaged.ToString());
            }

            if (withUrls)
            {
                values.Add(_source.Service.ConnectionData?.GetEntityMetadataUrl(metaData.MetadataId.Value));
            }

            handler.AddLine(values);

            var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

            return string.Format("{0} {1}", this.ComponentTypeEnum.ToString(), str);
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "null";
            }

            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.LogicalName;
            }

            return solutionComponent.ObjectId.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.DisplayName?.UserLocalizedLabel?.Label;
            }

            return null;
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsCustomizable?.Value.ToString();
            }

            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsManaged.ToString();
            }

            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            EntityMetadata metaData = _source.GetEntityMetadata(objectId);

            if (metaData != null)
            {
                return string.Format("{0}.Entity {1} - {2}.{3}", connectionName, metaData.LogicalName, fieldTitle, extension);
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}