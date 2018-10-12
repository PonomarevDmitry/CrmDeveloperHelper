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
    public class EntityKeyMetadataDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;
        private const string unknowedMessage = DefaultSolutionComponentDescriptionBuilder.unknowedMessage;

        private readonly SolutionComponentMetadataSource _source;

        public int ComponentTypeValue => (int)ComponentType.EntityKey;

        public ComponentType? ComponentTypeEnum => ComponentType.EntityKey;

        public string EntityLogicalName => string.Empty;

        public string EntityPrimaryIdAttribute => string.Empty;

        public EntityKeyMetadataDescriptionBuilder(SolutionComponentMetadataSource source)
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
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.EntityKey,
                    SchemaName = metaData.LogicalName,
                    ParentSchemaName = metaData.EntityLogicalName,
                    RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                    ComponentTypeName = ComponentType.EntityKey.ToString(),
                    Description = GenerateDescriptionSingle(solutionComponent, false),
                });
            }
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "IsManaged", "KeyAttributes");

            foreach (var comp in components)
            {
                EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    handler.AddLine(
                        string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName)
                        , metaData.IsManaged.ToString()
                        , string.Join(",", metaData.KeyAttributes.OrderBy(s => s))
                        );
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString());
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                string name = string.Format("EntityKey {0}.{1}    IsManaged {2}", metaData.EntityLogicalName, metaData.LogicalName, metaData.IsManaged.ToString());

                return name;
            }

            return solutionComponent.ToString();
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "null";
            }

            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
            }

            return solutionComponent.ObjectId.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.DisplayName?.UserLocalizedLabel?.Label;
            }

            return null;
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsCustomizable?.Value.ToString();
            }

            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.IsManaged.ToString();
            }

            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(objectId);

            if (metaData != null)
            {
                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }
    }
}