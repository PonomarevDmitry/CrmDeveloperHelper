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
    public class SLADescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SLADescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SLA)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SLA;

        public override int ComponentTypeValue => (int)ComponentType.SLA;

        public override string EntityLogicalName => SLA.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SLA.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SLA.Schema.Attributes.objecttypecode
                    , SLA.Schema.Attributes.name
                    , SLA.Schema.Attributes.slaid
                    , SLA.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "Name", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SLA>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.FormattedValues.ContainsKey(SLA.Schema.Attributes.objecttypecode) ? entity.FormattedValues[SLA.Schema.Attributes.objecttypecode] : entity.ObjectTypeCode?.Value.ToString()
                , entity.Name
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
                    { SLA.Schema.Attributes.objecttypecode, "Entity" }
                    , { SLA.Schema.Attributes.name, "Name" }
                    , { SLA.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { SLA.Schema.Attributes.statuscode, "StatusCode" }
                    , { SLA.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}