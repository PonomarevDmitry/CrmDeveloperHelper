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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<DisplayString>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Key", "LanguageCode", "Published", "Custom", "CustomComment", "FormatParameters", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var displayStringKey = entity.DisplayStringKey;
                var languageCode = entity.LanguageCode;
                var publishedDisplayString = entity.PublishedDisplayString;
                var customDisplayString = entity.CustomDisplayString;
                var customComment = entity.CustomComment;
                var formatParameters = entity.FormatParameters;

                handler.AddLine(displayStringKey
                    , LanguageLocale.GetLocaleName(languageCode.Value)
                    , publishedDisplayString
                    , customDisplayString
                    , customComment
                    , formatParameters.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var displayString = GetEntity<DisplayString>(component.ObjectId.Value);

            if (displayString != null)
            {
                string displaystringkey = displayString.DisplayStringKey;

                string customdisplaystring = displayString.CustomDisplayString;

                StringBuilder str = new StringBuilder();

                str.AppendFormat("DisplayString {0}", displaystringkey);

                if (!string.IsNullOrEmpty(customdisplaystring))
                {
                    str.AppendFormat(" - {0}", customdisplaystring);
                }

                str.AppendFormat(" - IsManaged {0}", displayString.IsManaged.ToString());

                str.AppendFormat(" - SolutionName {0}", EntityDescriptionHandler.GetAttributeString(displayString, "solution.uniquename"));

                return str.ToString();
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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