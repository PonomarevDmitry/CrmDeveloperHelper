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
    public class FieldSecurityProfileDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public FieldSecurityProfileDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.FieldSecurityProfile)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.FieldSecurityProfile;

        public override int ComponentTypeValue => (int)ComponentType.FieldSecurityProfile;

        public override string EntityLogicalName => FieldSecurityProfile.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => FieldSecurityProfile.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    FieldSecurityProfile.Schema.Attributes.name
                    , FieldSecurityProfile.Schema.Attributes.description
                    , FieldSecurityProfile.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "Description", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<FieldSecurityProfile>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.Description
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { FieldSecurityProfile.Schema.Attributes.name, "Name" }
                    , { FieldSecurityProfile.Schema.Attributes.description, "Description" }
                    , { FieldSecurityProfile.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}