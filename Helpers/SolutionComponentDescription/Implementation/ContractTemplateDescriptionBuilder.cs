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
    public class ContractTemplateDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ContractTemplateDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ContractTemplate)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ContractTemplate;

        public override int ComponentTypeValue => (int)ComponentType.ContractTemplate;

        public override string EntityLogicalName => ContractTemplate.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ContractTemplate.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ContractTemplate.Schema.Attributes.name
                    , ContractTemplate.Schema.Attributes.ismanaged
                    , ContractTemplate.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withUrls, bool withManaged, bool withSolutionInfo, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "IsCustomizable");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, bool withUrls, bool withManaged, bool withSolutionInfo, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ContractTemplate>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.IsCustomizable?.Value.ToString()
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ContractTemplate.Schema.Attributes.name, "Name" }
                    , { ContractTemplate.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ContractTemplate.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}