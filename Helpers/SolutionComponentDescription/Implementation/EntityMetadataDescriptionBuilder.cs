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

        public void FillSolutionImageComponent(List<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.Entity,
                    SchemaName = metaData.LogicalName,
                    RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                    Description = GenerateDescriptionSingle(solutionComponent, false),
                });
            }
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "IsManaged", "Behaviour", "Url");

            foreach (var comp in components)
            {
                string behaviorName = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behaviorName = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                EntityMetadata metaData = _source.GetEntityMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    string logicalName = metaData.LogicalName;

                    handler.AddLine(logicalName, metaData.IsManaged.ToString(), behaviorName, withUrls ? _source.Service.ConnectionData?.GetEntityMetadataUrl(metaData.MetadataId.Value) : string.Empty);
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString(), string.Empty, behaviorName);
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls)
        {
            EntityMetadata metaData = _source.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return string.Format("Entity {0}    IsManaged {1}{2}"
                    , metaData.LogicalName
                    , metaData.IsManaged.ToString()
                    , withUrls ? string.Format("    Url {0}", _source.Service.ConnectionData.GetEntityMetadataUrl(metaData.MetadataId.Value)) : string.Empty
                    );
            }

            return solutionComponent.ToString();
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
                return metaData.LogicalName;
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}