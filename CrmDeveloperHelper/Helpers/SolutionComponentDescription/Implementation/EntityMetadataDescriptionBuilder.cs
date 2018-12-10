using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Microsoft.Xrm.Sdk.Query;

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
                var behavior = solutionComponent.RootComponentBehavior?.Value;

                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.Entity,
                    SchemaName = metaData.LogicalName,
                    RootComponentBehavior = behavior.GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingleInternal(metaData, behavior, false, true, false),
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
                    int? behavior = DefaultSolutionComponentDescriptionBuilder.GetBehaviorFromXml(elementRootComponent);

                    FillSolutionComponentInternal(result, metaData, behavior);

                    if (behavior == (int)RootComponentBehavior.IncludeAsShellOnly
                        || behavior == (int)RootComponentBehavior.DoNotIncludeSubcomponents
                        )
                    {
                        var elementEntity = GetEntityElement(docCustomizations, metaData.LogicalName);

                        if (elementEntity != null)
                        {
                            FillAttributesFromCustomization(result, metaData, elementEntity);

                            FillEntityKeysFromCustomization(result, metaData, elementEntity);

                            FillSavedQueriesFromCustomization(result, metaData, elementEntity);

                            FillChartsFromCustomization(result, metaData, elementEntity);

                            FillDisplayStringsFromCustomization(result, metaData, elementEntity);

                            FillRelationsFromCustomization(result, metaData, docCustomizations);
                        }
                    }
                }
            }
        }

        private void FillRelationsFromCustomization(ICollection<SolutionComponent> result, EntityMetadata metaData, XDocument docCustomizations)
        {
            var elements = docCustomizations.XPathSelectElements("ImportExportXml/EntityRelationships/EntityRelationship").Where(e => e.Attribute("Name") != null && !string.IsNullOrEmpty((string)e.Attribute("Name")));

            foreach (var item in elements)
            {
                var relationType = item.Element("EntityRelationshipType");
                var relationName = (string)item.Attribute("Name");

                if (relationType == null || string.IsNullOrEmpty(relationName))
                {
                    continue;
                }

                if (string.Equals(relationType.Value, "OneToMany", StringComparison.InvariantCultureIgnoreCase))
                {
                    if ((item.Element("ReferencingEntityName") != null && string.Equals(item.Element("ReferencingEntityName").Value, metaData.LogicalName, StringComparison.InvariantCultureIgnoreCase))
                        || (item.Element("ReferencedEntityName") != null && string.Equals(item.Element("ReferencedEntityName").Value, metaData.LogicalName, StringComparison.InvariantCultureIgnoreCase))
                        )
                    {
                        var relationMetadata = _source.GetRelationshipMetadata(relationName);

                        if (relationMetadata != null)
                        {
                            var component = new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = relationMetadata.MetadataId.Value,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            };

                            result.Add(component);
                        }
                    }
                }
                else if (string.Equals(relationType.Value, "ManyToMany", StringComparison.InvariantCultureIgnoreCase))
                {
                    if ((item.Element("FirstEntityName") != null && string.Equals(item.Element("FirstEntityName").Value, metaData.LogicalName, StringComparison.InvariantCultureIgnoreCase))
                           || (item.Element("SecondEntityName") != null && string.Equals(item.Element("SecondEntityName").Value, metaData.LogicalName, StringComparison.InvariantCultureIgnoreCase))
                           )
                    {
                        var relationMetadata = _source.GetRelationshipMetadata(relationName);

                        if (relationMetadata != null)
                        {
                            var component = new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = relationMetadata.MetadataId.Value,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            };

                            result.Add(component);
                        }
                    }
                }
            }
        }

        private void FillDisplayStringsFromCustomization(ICollection<SolutionComponent> result, EntityMetadata metaData, XElement elementEntity)
        {
            var repository = new DisplayStringRepository(_source.Service);

            var elements = elementEntity.XPathSelectElements("./Strings/Strings");

            foreach (var stringKey in elements)
            {
                if (stringKey.Attribute("ResourceKey") == null || string.IsNullOrEmpty((string)stringKey.Attribute("ResourceKey")))
                {
                    continue;
                }

                string key = (string)stringKey.Attribute("ResourceKey");

                foreach (var item in stringKey.Elements("String"))
                {
                    if (item.Attribute("languagecode") == null 
                        || string.IsNullOrEmpty((string)item.Attribute("languagecode"))
                        || !int.TryParse((string)item.Attribute("languagecode"), out var langCode)
                        )
                    {
                        continue;
                    }

                    var entity = repository.GetByKeyAndLanguage(key, langCode, new ColumnSet(false));

                    if (entity != null)
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.DisplayString),
                            ObjectId = entity.Id,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        };

                        result.Add(component);
                    }
                }
            }
        }

        private void FillChartsFromCustomization(ICollection<SolutionComponent> result, EntityMetadata metaData, XElement elementEntity)
        {
            var repository = new SavedQueryVisualizationRepository(_source.Service);

            var elements = elementEntity.XPathSelectElements("./Visualizations/visualization/savedqueryvisualizationid");

            foreach (var item in elements)
            {
                if (string.IsNullOrEmpty(item.Value) || !Guid.TryParse(item.Value, out var idChart))
                {
                    continue;
                }

                var entity = repository.GetById(idChart, new ColumnSet(false));

                if (entity != null)
                {
                    var component = new SolutionComponent()
                    {
                        ComponentType = new OptionSetValue((int)ComponentType.SavedQueryVisualization),
                        ObjectId = entity.Id,
                        RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    };

                    result.Add(component);
                }
            }
        }

        private void FillSavedQueriesFromCustomization(ICollection<SolutionComponent> result, EntityMetadata metaData, XElement elementEntity)
        {
            var repository = new SavedQueryRepository(_source.Service);

            var elements = elementEntity.XPathSelectElements("./SavedQueries/savedqueries/savedquery/savedqueryid");

            foreach (var item in elements)
            {
                if (string.IsNullOrEmpty(item.Value) || !Guid.TryParse(item.Value, out var idSavedQuery))
                {
                    continue;
                }

                var entity = repository.GetById(idSavedQuery, new ColumnSet(false));

                if (entity != null)
                {
                    var component = new SolutionComponent()
                    {
                        ComponentType = new OptionSetValue((int)ComponentType.SavedQuery),
                        ObjectId = entity.Id,
                        RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    };

                    result.Add(component);
                }
            }
        }

        private void FillEntityKeysFromCustomization(ICollection<SolutionComponent> result, EntityMetadata metaData, XElement elementEntity)
        {
            var elements = elementEntity.XPathSelectElements("./EntityInfo/entity/EntityKeys/EntityKey/LogicalName");

            foreach (var item in elements)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    continue;
                }

                var entityKeyMetadata = _source.GetEntityKeyMetadata(metaData.LogicalName, item.Value);

                if (entityKeyMetadata != null)
                {
                    var component = new SolutionComponent()
                    {
                        ComponentType = new OptionSetValue((int)ComponentType.EntityKey),
                        ObjectId = entityKeyMetadata.MetadataId.Value,
                        RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    };

                    result.Add(component);
                }
            }
        }

        private void FillAttributesFromCustomization(ICollection<SolutionComponent> result, EntityMetadata metaData, XElement elementEntity)
        {
            var elements = elementEntity.XPathSelectElements("./EntityInfo/entity/attributes/attribute/LogicalName");

            foreach (var item in elements)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    continue;
                }

                var attributeMetadata = _source.GetAttributeMetadata(metaData.LogicalName, item.Value);

                if (attributeMetadata != null)
                {
                    var component = new SolutionComponent()
                    {
                        ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                        ObjectId = attributeMetadata.MetadataId.Value,
                        RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    };

                    result.Add(component);
                }
            }
        }

        private static XElement GetEntityElement(XDocument docCustomizations, string entityName)
        {
            var elements = docCustomizations.XPathSelectElements("ImportExportXml/Entities/Entity/EntityInfo/entity").Where(a => a.Attribute("Name") != null);

            var filteredElements = elements.Where(a => string.Equals(entityName, (string)a.Attribute("Name"), StringComparison.InvariantCultureIgnoreCase)).Select(e => e.Parent.Parent);

            return filteredElements.Count() == 1 ? filteredElements.SingleOrDefault() : null;
        }

        private void FillSolutionComponentInternal(ICollection<SolutionComponent> result, EntityMetadata metaData, int? behavior)
        {
            var component = new SolutionComponent()
            {
                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                ObjectId = metaData.MetadataId.Value,
                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            };

            if (behavior.HasValue)
            {
                component.RootComponentBehavior = new OptionSetValue(behavior.Value);
            }

            result.Add(component);
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "IsCustomizable", "Behavior");

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
                string behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior?.Value);

                EntityMetadata metaData = _source.GetEntityMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        metaData.LogicalName
                        , metaData.IsCustomizable?.Value.ToString()
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

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                int? behaviorCode = solutionComponent.RootComponentBehavior?.Value;

                return GenerateDescriptionSingleInternal(metaData, behaviorCode, withUrls, withManaged, withSolutionInfo);
            }

            return solutionComponent.ToString();
        }

        private string GenerateDescriptionSingleInternal(EntityMetadata metaData, int? behaviorCode, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            string behavior = SolutionComponent.GetRootComponentBehaviorName(behaviorCode);

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "IsCustomizable", "Behavior");

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
                , metaData.IsCustomizable?.Value.ToString()
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

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
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