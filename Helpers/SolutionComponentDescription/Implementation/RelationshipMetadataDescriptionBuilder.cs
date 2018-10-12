using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void FillSolutionImageComponent(List<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.GetValueOrDefault());

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.EntityRelationship,
                        SchemaName = relationship.SchemaName,
                        ParentSchemaName = relationship.ReferencingEntity,
                        RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                        ComponentTypeName = ComponentType.EntityRelationship.ToString(),
                        Description = GenerateDescriptionSingle(solutionComponent, false),
                    });

                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.EntityRelationship,
                        SchemaName = relationship.SchemaName,
                        ParentSchemaName = relationship.ReferencedEntity,
                        RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                        ComponentTypeName = ComponentType.EntityRelationship.ToString(),
                        Description = GenerateDescriptionSingle(solutionComponent, false),
                    });
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.EntityRelationship,
                        SchemaName = relationship.SchemaName,
                        ParentSchemaName = relationship.Entity1LogicalName,
                        RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                        ComponentTypeName = ComponentType.EntityRelationship.ToString(),
                        Description = GenerateDescriptionSingle(solutionComponent, false),
                    });

                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.EntityRelationship,
                        SchemaName = relationship.SchemaName,
                        ParentSchemaName = relationship.Entity2LogicalName,
                        RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                        ComponentTypeName = ComponentType.EntityRelationship.ToString(),
                        Description = GenerateDescriptionSingle(solutionComponent, false),
                    });
                }
            }
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();

            FormatTextTableHandler handlerManyToOne = new FormatTextTableHandler();
            FormatTextTableHandler handlerManyToMany = new FormatTextTableHandler();

            foreach (var comp in components)
            {
                RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(comp.ObjectId.GetValueOrDefault());

                string behavior = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                if (metaData != null)
                {
                    if (metaData is OneToManyRelationshipMetadata)
                    {
                        var relationship = metaData as OneToManyRelationshipMetadata;

                        string relName = string.Format("{0}.{1}", relationship.ReferencingEntity, relationship.ReferencingAttribute);
                        string refEntity = relationship.ReferencedEntity;

                        string url = null;

                        if (withUrls)
                        {
                            var entityMetadata = _source.GetEntityMetadata(refEntity);

                            if (entityMetadata != null)
                            {
                                url = _source.Service.ConnectionData?.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value);
                            }
                        }

                        handlerManyToOne.AddLine(relName
                            , "Many to One"
                            , refEntity
                            , relationship.SchemaName
                            , behavior
                            , relationship.IsManaged.ToString()
                            , url
                            );

                        continue;
                    }
                    else if (metaData is ManyToManyRelationshipMetadata)
                    {
                        var relationship = metaData as ManyToManyRelationshipMetadata;

                        string relName = string.Format("{0} - {1}", relationship.Entity1LogicalName, relationship.Entity2LogicalName);

                        string url = null;

                        if (withUrls)
                        {
                            var entityMetadata = _source.GetEntityMetadata(relationship.Entity1LogicalName);

                            if (entityMetadata != null)
                            {
                                url = _source.Service.ConnectionData?.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value);
                            }
                        }

                        handlerManyToMany.AddLine(relName
                            , "Many to Many"
                            , relationship.SchemaName
                            , behavior
                            , relationship.IsManaged.ToString()
                            , url
                        );

                        continue;
                    }
                }

                handler.AddLine(comp.ObjectId.ToString(), string.Empty, behavior);
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

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    string relName = string.Format("{0}.{1}", relationship.ReferencingEntity, relationship.ReferencingAttribute);
                    string refEntity = relationship.ReferencedEntity;

                    var entityMetadata = _source.GetEntityMetadata(refEntity);

                    return string.Format("EntityRelationship {0} - {1} - {2} - {3} - {4}{5}"
                        , relName
                        , "Many to One"
                        , refEntity
                        , relationship.SchemaName
                        , relationship.IsManaged.ToString()
                        , withUrls ? string.Format("    Url {0}", _source.Service.ConnectionData.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value)) : string.Empty
                        );
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    string relName = string.Format("{0} - {1}", relationship.Entity1LogicalName, relationship.Entity2LogicalName);

                    var entityMetadata = _source.GetEntityMetadata(relationship.Entity1LogicalName);

                    return string.Format("EntityRelationship {0} - {1} - {2} - {3}{4}"
                        , relName
                        , "Many to Many"
                        , relationship.SchemaName
                        , relationship.IsManaged.ToString()
                        , withUrls ? string.Format("    Url {0}", _source.Service.ConnectionData.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value)) : string.Empty
                        );
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
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    return string.Format("{0}.{1}.{2}"
                        , relationship.ReferencingEntity
                        , relationship.ReferencingAttribute
                        , relationship.SchemaName
                        );
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    return string.Format("{0} - {1}.{2}", relationship.Entity1LogicalName, relationship.Entity2LogicalName, relationship.SchemaName);
                }
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

                    return relationship.AssociatedMenuConfiguration?.Label?.UserLocalizedLabel?.Label;
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    return (relationship.Entity1AssociatedMenuConfiguration ?? relationship.Entity1AssociatedMenuConfiguration)?.Label?.UserLocalizedLabel?.Label;
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

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            RelationshipMetadataBase metaData = _source.GetRelationshipMetadata(objectId);

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    return string.Format("{0}.{1}.{2}"
                        , relationship.ReferencingEntity
                        , relationship.ReferencingAttribute
                        , relationship.SchemaName
                        );
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    return string.Format("{0} - {1}.{2}", relationship.Entity1LogicalName, relationship.Entity2LogicalName, relationship.SchemaName);
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