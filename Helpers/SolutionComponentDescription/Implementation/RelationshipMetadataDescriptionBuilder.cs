using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RelationshipMetadataDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;
        private const string unknowedMessage = DefaultSolutionComponentDescriptionBuilder.unknowedMessage;

        private readonly SolutionComponentMetadataSource _source;

        public int ComponentTypeValue => (int)ComponentType.EntityRelationship;

        public ComponentType? ComponentTypeEnum => ComponentType.EntityRelationship;

        public string EntityLogicalName => string.Empty;

        public string EntityPrimaryIdAttribute => string.Empty;

        public RelationshipMetadataDescriptionBuilder(SolutionComponentMetadataSource source)
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
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.GetValueOrDefault());

            if (metaData != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.EntityRelationship,
                    SchemaName = metaData.SchemaName,
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                });
            }
        }

        public void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null
                || string.IsNullOrEmpty(solutionImageComponent.SchemaName)
                )
            {
                return;
            }

            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionImageComponent.SchemaName);

            if (metaData != null)
            {
                var component = new SolutionComponent()
                {
                    ComponentType = new OptionSetValue(this.ComponentTypeValue),
                    ObjectId = metaData.MetadataId.Value,
                    RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                };

                if (solutionImageComponent.RootComponentBehavior.HasValue)
                {
                    component.RootComponentBehavior = new OptionSetValue(solutionImageComponent.RootComponentBehavior.Value);
                }

                result.Add(component);
            }
        }

        public void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {

        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();

            FormatTextTableHandler handlerManyToOne = new FormatTextTableHandler();
            handlerManyToOne.SetHeader("ReferencingAttribute", "Type", "ReferencedEntity", "SchemaName", "IsCustomizable", "Behavior");

            FormatTextTableHandler handlerManyToMany = new FormatTextTableHandler();
            handlerManyToMany.SetHeader("Entity - Entity", "Type", "SchemaName", "IsCustomizable", "Behavior");

            if (withManaged)
            {
                handlerManyToOne.AppendHeader("IsManaged");
                handlerManyToMany.AppendHeader("IsManaged");
            }

            if (withUrls)
            {
                handlerManyToOne.AppendHeader("Url");
                handlerManyToMany.AppendHeader("Url");
            }

            foreach (var comp in components)
            {
                RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(comp.ObjectId.GetValueOrDefault());

                string behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior?.Value);

                if (metaData != null)
                {
                    if (metaData is OneToManyRelationshipMetadata)
                    {
                        var relationship = metaData as OneToManyRelationshipMetadata;

                        List<string> values = new List<string>();

                        values.AddRange(new[]
                        {
                            string.Format("{0}.{1}", relationship.ReferencingEntity, relationship.ReferencingAttribute)
                            , "Many to One"
                            , relationship.ReferencedEntity
                            , relationship.SchemaName
                            , (relationship.IsCustomizable?.Value).GetValueOrDefault().ToString()
                            , behavior
                        });

                        if (withManaged)
                        {
                            values.Add(metaData.IsManaged.ToString());
                        }

                        if (withUrls)
                        {
                            var entityMetadata = _source.GetEntityMetadata(relationship.ReferencedEntity);
                            if (entityMetadata != null)
                            {
                                values.Add(_source.Service.ConnectionData?.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value));
                            }
                        }

                        handlerManyToOne.AddLine(values);
                    }
                    else if (metaData is ManyToManyRelationshipMetadata)
                    {
                        var relationship = metaData as ManyToManyRelationshipMetadata;

                        List<string> values = new List<string>();

                        values.AddRange(new[]
                        {
                            string.Format("{0} - {1}", relationship.Entity1LogicalName, relationship.Entity2LogicalName)
                            , "Many to Many"
                            , relationship.SchemaName
                            , behavior
                            , (relationship.IsCustomizable?.Value).GetValueOrDefault().ToString()
                        });

                        if (withManaged)
                        {
                            values.Add(metaData.IsManaged.ToString());
                        }

                        if (withUrls)
                        {
                            var entityMetadata = _source.GetEntityMetadata(relationship.Entity1LogicalName);
                            if (entityMetadata != null)
                            {
                                values.Add(_source.Service.ConnectionData?.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value));
                            }
                        }

                        handlerManyToMany.AddLine(values);
                    }
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString(), behavior);
                }
            }

            List<string> linesUnknowed = handler.GetFormatedLines(true);

            List<string> linesOne = handlerManyToOne.GetFormatedLines(true);

            List<string> linesMany = handlerManyToMany.GetFormatedLines(true);

            if (linesUnknowed.Any())
            {
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                linesUnknowed.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }

            linesOne.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            linesMany.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                string behavior = SolutionComponent.GetRootComponentBehaviorName(solutionComponent.RootComponentBehavior?.Value);

                if (metaData is OneToManyRelationshipMetadata)
                {
                    FormatTextTableHandler handlerManyToOne = new FormatTextTableHandler();
                    handlerManyToOne.SetHeader("ReferencingAttribute", "Type", "ReferencedEntity", "SchemaName", "Behavior", "IsCustomizable");

                    if (withManaged)
                    {
                        handlerManyToOne.AppendHeader("IsManaged");
                    }

                    if (withUrls)
                    {
                        handlerManyToOne.AppendHeader("Url");
                    }

                    var relationship = metaData as OneToManyRelationshipMetadata;

                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        string.Format("{0}.{1}", relationship.ReferencingEntity, relationship.ReferencingAttribute)
                        , "Many to One"
                        , relationship.ReferencedEntity
                        , relationship.SchemaName
                        , behavior
                        , relationship.IsCustomizable?.Value.ToString()
                    });

                    if (withManaged)
                    {
                        values.Add(metaData.IsManaged.ToString());
                    }

                    if (withUrls)
                    {
                        var entityMetadata = _source.GetEntityMetadata(relationship.ReferencedEntity);
                        if (entityMetadata != null)
                        {
                            values.Add(_source.Service.ConnectionData?.GetRelationshipMetadataRelativeUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value));
                        }
                    }

                    handlerManyToOne.AddLine(values);

                    var str = handlerManyToOne.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                    return string.Format("{0} {1}", this.ComponentTypeEnum.ToString(), str);
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    FormatTextTableHandler handlerManyToMany = new FormatTextTableHandler();
                    handlerManyToMany.SetHeader("Entity - Entity", "Type", "SchemaName", "Behavior", "IsCustomizable");

                    if (withManaged)
                    {
                        handlerManyToMany.AppendHeader("IsManaged");
                    }

                    if (withUrls)
                    {
                        handlerManyToMany.AppendHeader("Url");
                    }

                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        string.Format("{0} - {1}", relationship.Entity1LogicalName, relationship.Entity2LogicalName)
                        , "Many to Many"
                        , relationship.SchemaName
                        , behavior
                        , relationship.IsCustomizable?.Value.ToString()
                    });

                    if (withManaged)
                    {
                        values.Add(metaData.IsManaged.ToString());
                    }

                    if (withUrls)
                    {
                        var entityMetadata = _source.GetEntityMetadata(relationship.Entity1LogicalName);
                        if (entityMetadata != null)
                        {
                            values.Add(_source.Service.ConnectionData?.GetRelationshipMetadataRelativeUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value));
                        }
                    }

                    handlerManyToMany.AddLine(values);

                    var str = handlerManyToMany.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                    return string.Format("{0} {1}", this.ComponentTypeEnum.ToString(), str);
                }
            }

            return solutionComponent.ToString();
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "null";
            }

            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.SchemaName;
            }

            return solutionComponent.ObjectId.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    return string.Format("{0}.{1} ManyToOne -> {2}"
                        , relationship.ReferencingEntity
                        , relationship.ReferencingAttribute
                        , relationship.ReferencedEntity
                    );
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    return string.Format("{0} - {1} ManyToMany", relationship.Entity1LogicalName, relationship.Entity2LogicalName);
                }
            }

            return null;
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsCustomizable?.Value.ToString();
            }

            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsManaged.ToString();
            }

            return null;
        }

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    return relationship.ReferencedEntity;
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    return relationship.Entity1LogicalName;
                }
            }

            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(objectId);

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    return string.Format("{0}.{1}.{2}.{3} - {4}.{5}"
                        , connectionName
                        , relationship.ReferencingEntity
                        , relationship.ReferencingAttribute
                        , relationship.SchemaName
                        , fieldTitle
                        , extension
                        );
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    return string.Format("{0}.{1} - {2}.{3} - {4}.{5}", connectionName, relationship.Entity1LogicalName, relationship.Entity2LogicalName, relationship.SchemaName, fieldTitle, extension);
                }
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}