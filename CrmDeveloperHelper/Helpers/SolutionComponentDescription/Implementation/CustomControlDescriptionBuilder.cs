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
    public class CustomControlDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public CustomControlDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.CustomControl)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.CustomControl;

        public override int ComponentTypeValue => (int)ComponentType.CustomControl;

        public override string EntityLogicalName => CustomControl.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => CustomControl.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    CustomControl.Schema.Attributes.name
                    , CustomControl.Schema.Attributes.compatibledatatypes
                    , CustomControl.Schema.Attributes.manifest
                    , CustomControl.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "CompatibleDataTypes", "Manifest", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<CustomControl>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.CompatibleDataTypes
                , entity.Manifest
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { CustomControl.Schema.Attributes.name, "Name" }
                    , { CustomControl.Schema.Attributes.compatibledatatypes, "CompatibleDataTypes" }
                    , { CustomControl.Schema.Attributes.manifest, "Manifest" }
                    , { CustomControl.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}