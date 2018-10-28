using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RibbonCustomizationDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonCustomizationDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonCustomization)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonCustomization;

        public override int ComponentTypeValue => (int)ComponentType.RibbonCustomization;

        public override string EntityLogicalName => RibbonCustomization.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonCustomization.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonCustomization.Schema.Attributes.entity
                    , RibbonCustomization.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RibbonCustomization>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler table = new FormatTextTableHandler();

            table.SetHeader("Entity", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Guid");

            foreach (var entity in list)
            {
                string entityName = entity.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                table.AddLine(entityName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.Id.ToString()
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var ribbonCustomization = GetEntity<RibbonCustomization>(component.ObjectId.Value);

            if (ribbonCustomization != null)
            {
                string entityName = ribbonCustomization.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                return string.Format("RibbonCustomization {0}    IsManaged {1}    SolutionName {2}"
                    , entityName
                    , ribbonCustomization.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(ribbonCustomization, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonCustomization>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.ToEntity<RibbonCustomization>().Entity ?? "ApplicationRibbon";
            }

            return base.GetName(component);
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue
                )
            {
                return;
            }

            var entity = GetEntity<RibbonCustomization>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var repository = new RibbonCustomizationRepository(_service);

                result.Add(new SolutionImageComponent()
                {
                    SchemaName = string.Format("{0}:RibbonDiffXml", entity.Entity),
                    ComponentType = (solutionComponent.ComponentType?.Value).GetValueOrDefault(),
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, false),
                });
            }
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            if (string.Equals(solutionImageComponent.SchemaName, ":RibbonDiffXml", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = new RibbonCustomizationRepository(_service);

                var entity = repository.FindApplicationRibbonCustomization();

                if (entity != null)
                {
                    var component = new SolutionComponent()
                    {
                        ComponentType = new OptionSetValue(this.ComponentTypeValue),
                        ObjectId = entity.Id,
                        RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    };

                    if (solutionImageComponent.RootComponentBehavior.HasValue)
                    {
                        component.RootComponentBehavior = new OptionSetValue(solutionImageComponent.RootComponentBehavior.Value);
                    }

                    result.Add(component);
                }
            }
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { RibbonCustomization.Schema.Attributes.entity, "Entity" }
                    , { RibbonCustomization.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}