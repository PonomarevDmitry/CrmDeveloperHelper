using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class AttributeMetadataDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;
        private const string unknowedMessage = DefaultSolutionComponentDescriptionBuilder.unknowedMessage;

        private readonly SolutionComponentMetadataSource _source;

        public int ComponentTypeValue => (int)ComponentType.Attribute;

        public ComponentType? ComponentTypeEnum => ComponentType.Attribute;

        public string EntityLogicalName => string.Empty;

        public string EntityPrimaryIdAttribute => string.Empty;

        public AttributeMetadataDescriptionBuilder(SolutionComponentMetadataSource source)
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

            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.Attribute,
                    SchemaName = metaData.LogicalName,
                    ParentSchemaName = metaData.EntityLogicalName,
                    RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                    Description = GenerateDescriptionSingle(solutionComponent, false),
                });
            }
        }

        public void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null 
                || string.IsNullOrEmpty(solutionImageComponent.SchemaName)
                || string.IsNullOrEmpty(solutionImageComponent.ParentSchemaName)
                )
            {
                return;
            }

            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionImageComponent.ParentSchemaName, solutionImageComponent.SchemaName);

            if (metaData != null)
            {
                var component = new SolutionComponent()
                {
                    ComponentType = new OptionSetValue(this.ComponentTypeValue),
                    ObjectId = metaData.MetadataId.Value,
                };

                if (solutionImageComponent.RootComponentBehavior.HasValue)
                {
                    component.RootComponentBehavior = new OptionSetValue(solutionImageComponent.RootComponentBehavior.Value);
                }

                result.Add(component);
            }
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            FormatTextTableHandler handlerUnknowed = new FormatTextTableHandler();

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("AttributeName", "IsManaged", "Behaviour", "Url");

            foreach (var comp in components)
            {
                string behavior = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                AttributeMetadata metaData = _source.GetAttributeMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    var entityMetadata = _source.GetEntityMetadata(metaData.EntityLogicalName);

                    if (entityMetadata != null)
                    {
                        string name = string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);

                        handler.AddLine(name, metaData.IsManaged.ToString(), behavior, withUrls ? _source.Service.ConnectionData?.GetAttributeMetadataRelativeUrl(entityMetadata.MetadataId.Value, metaData.MetadataId.Value) : string.Empty);

                        continue;
                    }
                }

                handlerUnknowed.AddLine(comp.ObjectId.ToString(), string.Empty, behavior);
            }

            if (handlerUnknowed.Count > 0)
            {
                List<string> lines = handlerUnknowed.GetFormatedLines(true);
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }

            {
                List<string> lines = handler.GetFormatedLines(true);

                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls)
        {
            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                var entityMetadata = _source.GetEntityMetadata(metaData.EntityLogicalName);

                if (entityMetadata != null)
                {
                    string name = string.Format("Attribute {0}.{1}     IsManaged {2}{3}"
                        , metaData.EntityLogicalName
                        , metaData.LogicalName
                        , metaData.IsManaged.ToString()
                        , withUrls ? string.Format("     Url {0}", _source.Service.ConnectionData.GetAttributeMetadataRelativeUrl(entityMetadata.MetadataId.Value, metaData.MetadataId.Value)) : string.Empty
                    );

                    return name;
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

            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
            }

            return solutionComponent.ObjectId.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.DisplayName?.UserLocalizedLabel?.Label;
            }

            return null;
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsCustomizable?.Value.ToString();
            }

            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            AttributeMetadata metaData = _source.GetAttributeMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsManaged.ToString();
            }

            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            AttributeMetadata metaData = _source.GetAttributeMetadata(objectId);

            if (metaData != null)
            {
                return string.Format("{0}.Attribute {1}.{2} - {3}.{4}", connectionName, metaData.EntityLogicalName, metaData.LogicalName, fieldTitle, extension);
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}