using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class EntityDataSourceDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        private readonly SolutionComponentMetadataSource _source;

        public EntityDataSourceDescriptionBuilder(IOrganizationServiceExtented service, SolutionComponentMetadataSource source)
            : base(service, (int)ComponentType.EntityDataSource)
        {
            this._source = source;
        }

        public override ComponentType? ComponentTypeEnum => ComponentType.EntityDataSource;

        public override int ComponentTypeValue => (int)ComponentType.EntityDataSource;

        public override string EntityLogicalName => EntityDataSource.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => EntityDataSource.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
            (
                EntityDataSource.Schema.Attributes.name
                , EntityDataSource.Schema.Attributes.entitydataproviderid
                , EntityDataSource.Schema.Attributes.entityname
                , EntityDataSource.Schema.Attributes.iscustomizable
                , EntityDataSource.Schema.Attributes.ismanaged
            );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();

            handler.SetHeader(
                EntityDataSource.Schema.Headers.entitydataproviderid
                , EntityDataSource.Schema.Headers.name
                , EntityDataSource.Schema.Headers.entityname
                , EntityDataSource.Schema.Headers.iscustomizable
                , "Behavior"
            );

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<EntityDataSource>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.EntityDataProviderId?.Name
                , entity.Name
                , entity.EntityName
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
            {
                { EntityDataSource.Schema.Attributes.entitydataproviderid, EntityDataSource.Schema.Headers.entitydataproviderid }
                , { EntityDataSource.Schema.Attributes.name, EntityDataSource.Schema.Headers.name }
                , { EntityDataSource.Schema.Attributes.entityname, EntityDataSource.Schema.Headers.entityname }
                , { EntityDataSource.Schema.Attributes.iscustomizable, EntityDataSource.Schema.Headers.iscustomizable }
                , { EntityDataSource.Schema.Attributes.ismanaged, EntityDataSource.Schema.Headers.ismanaged }
                , { "solution.uniquename", "SolutionName" }
                , { "solution.ismanaged", "SolutionIsManaged" }
                , { "suppsolution.uniquename", "SupportingName" }
                , { "suppsolution.ismanaged", "SupportingIsManaged" }
            };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<EntityDataSource>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.EntityName))
                {
                    var entityMetadata = _source.GetEntityMetadata(entity.EntityName);

                    if (entityMetadata != null)
                    {
                        result.Add(new SolutionComponent()
                        {
                            ObjectId = entityMetadata.MetadataId,
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                        });
                    }
                }
            }

            return result;
        }
    }
}
