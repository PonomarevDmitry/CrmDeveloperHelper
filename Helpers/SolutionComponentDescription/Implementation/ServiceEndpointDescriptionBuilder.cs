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
    public class ServiceEndpointDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ServiceEndpointDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ServiceEndpoint)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ServiceEndpoint;

        public override int ComponentTypeValue => (int)ComponentType.ServiceEndpoint;

        public override string EntityLogicalName => ServiceEndpoint.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ServiceEndpoint.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ServiceEndpoint.Schema.Attributes.name
                    , ServiceEndpoint.Schema.Attributes.connectionmode
                    , ServiceEndpoint.Schema.Attributes.contract
                    , ServiceEndpoint.Schema.Attributes.messageformat
                    , ServiceEndpoint.Schema.Attributes.ismanaged
                    , ServiceEndpoint.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "ConnectionMode", "Contract", "MessageFormat", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ServiceEndpoint>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.connectionmode]
                , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.contract]
                , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.messageformat]
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
                    { ServiceEndpoint.Schema.Attributes.name, "Name" }
                    , { ServiceEndpoint.Schema.Attributes.connectionmode, "ConnectionMode" }
                    , { ServiceEndpoint.Schema.Attributes.contract, "Contract" }
                    , { ServiceEndpoint.Schema.Attributes.messageformat, "MessageFormat" }
                    , { ServiceEndpoint.Schema.Attributes.namespaceformat, "NamespaceFormat" }
                    , { ServiceEndpoint.Schema.Attributes.namespaceaddress, "NamespaceAddress" }
                    , { ServiceEndpoint.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ServiceEndpoint.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}