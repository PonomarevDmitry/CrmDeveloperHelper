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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class DisplayStringDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public DisplayStringDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.DisplayString)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DisplayString;

        public override int ComponentTypeValue => (int)ComponentType.DisplayString;

        public override string EntityLogicalName => DisplayString.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DisplayString.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    DisplayString.Schema.Attributes.displaystringkey
                    , DisplayString.Schema.Attributes.languagecode
                    , DisplayString.Schema.Attributes.publisheddisplaystring
                    , DisplayString.Schema.Attributes.customdisplaystring
                    , DisplayString.Schema.Attributes.customcomment
                    , DisplayString.Schema.Attributes.formatparameters
                    , DisplayString.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Key", "LanguageCode", "Published", "Custom", "CustomComment", "FormatParameters", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<DisplayString>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.DisplayStringKey
                , LanguageLocale.GetLocaleName(entity.LanguageCode.Value)
                , entity.PublishedDisplayString
                , entity.CustomDisplayString
                , entity.CustomComment
                , entity.FormatParameters.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<DisplayString>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.DisplayStringKey;
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

            var entity = GetEntity<DisplayString>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var imageComponent = new SolutionImageComponent()
                {
                    ComponentType = this.ComponentTypeValue,
                    SchemaName = entity.DisplayStringKey,
                    ParentSchemaName = entity.LanguageCode.ToString(),

                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                };

                result.Add(imageComponent);
            }
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(solutionImageComponent.SchemaName)
                && !string.IsNullOrEmpty(solutionImageComponent.ParentSchemaName)
                && int.TryParse(solutionImageComponent.ParentSchemaName, out var langCode)
                )
            {
                string key = solutionImageComponent.SchemaName;
                int? behavior = solutionImageComponent.RootComponentBehavior;

                var repository = new DisplayStringRepository(_service);

                var entity = repository.GetByKeyAndLanguage(key, langCode, new ColumnSet(false));

                if (entity != null)
                {
                    FillSolutionComponentInternal(result, entity.Id, behavior);
                }
            }
        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { DisplayString.Schema.Attributes.displaystringkey, "Key" }
                    , { DisplayString.Schema.Attributes.languagecode, "LanguageCode" }
                    , { DisplayString.Schema.Attributes.publisheddisplaystring, "Published" }
                    , { DisplayString.Schema.Attributes.customdisplaystring, "Custom" }
                    , { DisplayString.Schema.Attributes.customcomment, "CustomComment" }
                    , { DisplayString.Schema.Attributes.formatparameters, "FormatParameters" }
                    , { DisplayString.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}