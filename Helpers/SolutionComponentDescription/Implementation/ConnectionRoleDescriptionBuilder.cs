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
    public class ConnectionRoleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ConnectionRoleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ConnectionRole)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ConnectionRole;

        public override int ComponentTypeValue => (int)ComponentType.ConnectionRole;

        public override string EntityLogicalName => ConnectionRole.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ConnectionRole.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ConnectionRole.Schema.Attributes.name
                    , ConnectionRole.Schema.Attributes.ismanaged
                    , ConnectionRole.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ConnectionRole>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
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
                    {  ConnectionRole.Schema.Attributes.name, "Name" }
                    , { ConnectionRole.Schema.Attributes.category, "Category" }
                    , { ConnectionRole.Schema.Attributes.statuscode, "StatusCode" }
                    , { ConnectionRole.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ConnectionRole.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}