using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class EntityDataProviderDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public EntityDataProviderDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.EntityDataProvider)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.EntityDataProvider;

        public override int ComponentTypeValue => (int)ComponentType.EntityDataProvider;

        public override string EntityLogicalName => EntityDataProvider.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => EntityDataProvider.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
            (
                EntityDataProvider.Schema.Attributes.name
                , EntityDataProvider.Schema.Attributes.datasourcelogicalname
                , EntityDataProvider.Schema.Attributes.ismanaged
                , EntityDataProvider.Schema.Attributes.iscustomizable
            );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(
                EntityDataProvider.Schema.Headers.name
                , EntityDataProvider.Schema.Headers.datasourcelogicalname
                , EntityDataProvider.Schema.Headers.iscustomizable
                , "Behavior"
            );

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<EntityDataProvider>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.DataSourceLogicalName
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
                { EntityDataProvider.Schema.Attributes.name, EntityDataProvider.Schema.Headers.name }
                , { EntityDataProvider.Schema.Attributes.datasourcelogicalname, EntityDataProvider.Schema.Headers.datasourcelogicalname }
                , { EntityDataProvider.Schema.Attributes.iscustomizable, EntityDataProvider.Schema.Headers.iscustomizable }
                , { EntityDataProvider.Schema.Attributes.ismanaged, EntityDataProvider.Schema.Headers.ismanaged }
                , { "solution.uniquename", "SolutionName" }
                , { "solution.ismanaged", "SolutionIsManaged" }
                , { "suppsolution.uniquename", "SupportingName" }
                , { "suppsolution.ismanaged", "SupportingIsManaged" }
            };
        }
    }
}
