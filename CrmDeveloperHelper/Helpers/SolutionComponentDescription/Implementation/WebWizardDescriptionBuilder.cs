using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class WebWizardDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public WebWizardDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.WebWizard)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.WebWizard;

        public override int ComponentTypeValue => (int)ComponentType.WebWizard;

        public override string EntityLogicalName => WebWizard.Schema.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => WebWizard.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    WebWizard.Schema.Attributes.name
                    , WebWizard.Schema.Attributes.titleresourcestring
                    , WebWizard.Schema.Attributes.isstaticpagesequence
                    , WebWizard.Schema.Attributes.accessprivileges
                    , WebWizard.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(WebWizard.Schema.Headers.name
                , WebWizard.Schema.Headers.titleresourcestring
                , WebWizard.Schema.Headers.isstaticpagesequence
                , WebWizard.Schema.Headers.accessprivileges
                , "Behavior"
            );

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Entity>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.GetAttributeValue<string>(WebWizard.Schema.Attributes.name)
                , entity.GetAttributeValue<string>(WebWizard.Schema.Attributes.titleresourcestring)
                , entity.GetAttributeValue<bool?>(WebWizard.Schema.Attributes.isstaticpagesequence)?.ToString()
                , entity.GetAttributeValue<string>(WebWizard.Schema.Attributes.accessprivileges)
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<Entity>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.GetAttributeValue<string>(WebWizard.Schema.Attributes.name);
            }

            return base.GetName(component);
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<Entity>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.GetAttributeValue<string>(WebWizard.Schema.Attributes.titleresourcestring);
            }

            return base.GetDisplayName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { WebWizard.Schema.Attributes.name, WebWizard.Schema.Headers.name }
                    , {  WebWizard.Schema.Attributes.titleresourcestring, WebWizard.Schema.Headers.titleresourcestring }
                    , { WebWizard.Schema.Attributes.isstaticpagesequence, WebWizard.Schema.Headers.isstaticpagesequence }
                    , { WebWizard.Schema.Attributes.accessprivileges, WebWizard.Schema.Headers.accessprivileges }
                    , { WebWizard.Schema.Attributes.ismanaged, WebWizard.Schema.Headers.ismanaged }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}
