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
    public class CustomControlDefaultConfigDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public CustomControlDefaultConfigDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.CustomControlDefaultConfig)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.CustomControlDefaultConfig;

        public override int ComponentTypeValue => (int)ComponentType.CustomControlDefaultConfig;

        public override string EntityLogicalName => CustomControlDefaultConfig.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => CustomControlDefaultConfig.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    CustomControlDefaultConfig.Schema.Attributes.primaryentitytypecode
                    , CustomControlDefaultConfig.Schema.Attributes.customcontroldefaultconfigid
                    , CustomControlDefaultConfig.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<CustomControlDefaultConfig>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.PrimaryEntityTypeCode
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<CustomControlDefaultConfig>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , entity.PrimaryEntityTypeCode
                    , entity.Id.ToString()
                    );
            }

            return base.GetName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { CustomControlDefaultConfig.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                    , { CustomControlDefaultConfig.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { CustomControlDefaultConfig.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}