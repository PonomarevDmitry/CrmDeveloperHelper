using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SLAItemDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SLAItemDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SLAItem)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SLAItem;

        public override int ComponentTypeValue => (int)ComponentType.SLAItem;

        public override string EntityLogicalName => SLAItem.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SLAItem.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SLAItem.Schema.Attributes.slaid
                    , SLAItem.Schema.Attributes.name
                    , SLAItem.Schema.Attributes.relatedfield
                    , SLAItem.Schema.Attributes.slaitemid
                    , SLAItem.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("SLA", "Name", "RelatedField", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SLAItem>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.SLAId?.Name
                , entity.Name
                , entity.RelatedField
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SLAItem.Schema.Attributes.slaid, "SLA" }
                    , { SLAItem.Schema.Attributes.name, "Name" }
                    , { SLAItem.Schema.Attributes.relatedfield, "RelatedField" }
                    , { SLAItem.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { SLAItem.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}