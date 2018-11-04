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

        public void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.EntityKey,
                    SchemaName = metaData.LogicalName,
                    ParentSchemaName = metaData.EntityLogicalName,
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
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

            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionImageComponent.ParentSchemaName, solutionImageComponent.SchemaName);

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
            handler.SetHeader("Name", "IsCustomizable", "Behavior");

            if (withManaged)
            {
                handler.AppendHeader("IsManaged");
            }

            handler.AppendHeader("KeyAttributes");

            if (withUrls)
            {
                handler.AppendHeader("Url");
            }

            foreach (var comp in components)
            {
                string behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior?.Value);

                EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName)
                        , metaData.IsCustomizable?.Value.ToString()
                        , behavior
                    });

                    if (withManaged)
                    {
                        values.Add(metaData.IsManaged.ToString());
                    }

                    values.Add(string.Join(",", metaData.KeyAttributes.OrderBy(s => s)));

                    if (withUrls)
                    {
                        var entityMetadata = _source.GetEntityMetadata(metaData.EntityLogicalName);
                        if (entityMetadata != null)
                        {
                            values.Add(_source.Service.ConnectionData?.GetEntityKeyMetadataUrl(entityMetadata.MetadataId.Value, metaData.MetadataId.Value));
                        }
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
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                string behavior = SolutionComponent.GetRootComponentBehaviorName(solutionComponent.RootComponentBehavior?.Value);

                FormatTextTableHandler handler = new FormatTextTableHandler();
                handler.SetHeader("EntityKeyName", "IsCustomizable", "Behavior");

                if (withManaged)
                {
                    handler.AppendHeader("IsManaged");
                }

                handler.AppendHeader("KeyAttributes");

                if (withUrls)
                {
                    handler.AppendHeader("Url");
                }

                List<string> values = new List<string>();

                values.AddRange(new[]
                {
                    string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName)
                    , metaData.IsCustomizable?.Value.ToString()
                    , behavior
                });

                if (withManaged)
                {
                    values.Add(metaData.IsManaged.ToString());
                }

                values.Add(string.Join(",", metaData.KeyAttributes.OrderBy(s => s)));

                if (withUrls)
                {
                    var entityMetadata = _source.GetEntityMetadata(metaData.EntityLogicalName);

                    if (entityMetadata != null)
                    {
                        values.Add(_source.Service.ConnectionData?.GetEntityKeyMetadataUrl(entityMetadata.MetadataId.Value, metaData.MetadataId.Value));
                    }
                }

                handler.AddLine(values);

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("{0} {1}", this.ComponentTypeEnum.ToString(), str);
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

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(solutionComponent.ObjectId.Value);

            if (metaData != null)
            {
                return metaData.EntityLogicalName;
            }

            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            EntityKeyMetadata metaData = _source.GetEntityKeyMetadata(objectId);

            if (metaData != null)
            {
                return string.Format("{0}.EntityKey {1}.{2} - {3}.{4}", connectionName, metaData.EntityLogicalName, metaData.LogicalName, fieldTitle, extension);
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}