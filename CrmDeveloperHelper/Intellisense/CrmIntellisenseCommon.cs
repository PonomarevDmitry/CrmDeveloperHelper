using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public static class CrmIntellisenseCommon
    {
        public static string CreateEntityDescription(EntityIntellisenseData entity)
        {
            List<string> lines = new List<string>();

            if (entity.IsIntersectEntity)
            {
                lines.Add("IntersectEntity");

                if (entity.ManyToManyRelationships != null)
                {
                    var relations = entity.ManyToManyRelationships.Values.Where(r => string.Equals(r.IntersectEntityName, entity.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase));

                    foreach (var rel in relations.OrderBy(r => r.Entity1Name).ThenBy(r => r.Entity2Name).ThenBy(r => r.Entity1IntersectAttributeName).ThenBy(r => r.Entity2IntersectAttributeName))
                    {
                        lines.Add(string.Format("{0} - {1}", rel.Entity1Name, rel.Entity2Name));
                    }
                }
            }

            CreateFileHandler.FillLabelEntity(lines, true, entity.DisplayName, entity.DisplayCollectionName, entity.Description);

            return string.Join(System.Environment.NewLine, lines);
        }

        public static string CreateAttributeDescription(string entityDescription, AttributeIntellisenseData attribute)
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(entityDescription))
            {
                lines.Add(string.Format("Entity:\t{0}", entityDescription));
            }

            FillDescriptionAttribute(lines, attribute);

            return string.Join(System.Environment.NewLine, lines);
        }

        private static void FillDescriptionAttribute(List<string> lines, AttributeIntellisenseData attribute)
        {
            if (attribute.IsPrimaryIdAttribute.GetValueOrDefault())
            {
                lines.Add("PrimaryId");
            }

            if (attribute.IsPrimaryNameAttribute.GetValueOrDefault())
            {
                lines.Add("PrimaryName");
            }

            if (attribute.Targets != null && attribute.Targets.Count > 0)
            {
                if (attribute.Targets.Count <= 6)
                {
                    string targets = string.Join(",", attribute.Targets.OrderBy(s => s));

                    lines.Add(string.Format("Targets:\t{0}", targets));
                }
                else
                {
                    lines.Add(string.Format("Targets Count:\t{0}", attribute.Targets.Count));
                }
            }

            CreateFileHandler.FillLabelDisplayNameAndDescription(lines, true, attribute.DisplayName, attribute.Description);
        }

        public static string CreateEntityAndAttributeDescription(EntityIntellisenseData entity, AttributeIntellisenseData attribute)
        {
            List<string> lines = new List<string>();

            if (entity.IsIntersectEntity)
            {
                lines.Add("IntersectEntity");

                if (entity.ManyToManyRelationships != null)
                {
                    var relations = entity.ManyToManyRelationships.Values.Where(r => string.Equals(r.IntersectEntityName, entity.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase));

                    foreach (var rel in relations.OrderBy(r => r.Entity1Name).ThenBy(r => r.Entity2Name).ThenBy(r => r.Entity1IntersectAttributeName).ThenBy(r => r.Entity2IntersectAttributeName))
                    {
                        lines.Add(string.Format("{0} - {1}", rel.Entity1Name, rel.Entity2Name));
                    }
                }
            }

            CreateFileHandler.FillLabelEntity(lines, true, entity.DisplayName, entity.DisplayCollectionName, entity.Description);

            lines.Add(string.Empty);

            lines.Add(string.Format("Attribute:\t{0}", GetDisplayTextAttribute(entity.EntityLogicalName, attribute)));

            FillDescriptionAttribute(lines, attribute);

            return string.Join(System.Environment.NewLine, lines);
        }

        public static string CreateOptionValueDescription(string entityDescription, string attributeDescription, OptionMetadata optionMetadata)
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(entityDescription))
            {
                lines.Add(string.Format("Entity:\t\t{0}", entityDescription));
            }

            if (!string.IsNullOrEmpty(attributeDescription))
            {
                lines.Add(string.Format("Attribute:\t{0}", attributeDescription));
            }

            if (optionMetadata is StateOptionMetadata stateOption)
            {
                lines.Add(string.Format("DefaultStatusValue:\t{0}", stateOption.DefaultStatus));
            }

            if (optionMetadata is StatusOptionMetadata statusOption)
            {
                lines.Add(string.Format("State:\t{0}", statusOption.State));
            }

            CreateFileHandler.FillLabelDisplayNameAndDescription(lines, true, optionMetadata.Label, optionMetadata.Description, "    ");

            return string.Join(System.Environment.NewLine, lines);
        }

        public static string GetDisplayTextEntity(EntityIntellisenseData entityData)
        {
            StringBuilder result = new StringBuilder(entityData.EntityLogicalName);

            string temp = CreateFileHandler.GetLocalizedLabel(entityData.DisplayName);

            if (!string.IsNullOrEmpty(temp))
            {
                result.AppendFormat(" - {0}", temp);
            }

            if (entityData.IsIntersectEntity)
            {
                result.Append(" - IntersectEntity");
            }

            return result.ToString();
        }

        public static string GetDisplayTextAttribute(string entityName, AttributeIntellisenseData attribute)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}.{1}", entityName, attribute.LogicalName);

            string temp = CreateFileHandler.GetLocalizedLabel(attribute.DisplayName);

            if (!string.IsNullOrEmpty(temp))
            {
                result.AppendFormat(" - {0}", temp);
            }

            if (attribute.IsPrimaryIdAttribute.GetValueOrDefault())
            {
                result.Append(" - PrimaryId");
            }

            if (attribute.IsPrimaryNameAttribute.GetValueOrDefault())
            {
                result.Append(" - PrimaryName");
            }

            if (attribute.AttributeType.HasValue)
            {
                result.AppendFormat(" - {0}", attribute.AttributeType.ToString());
            }

            return result.ToString();
        }

        public static string GetDisplayTextEntityAndAttribute(EntityIntellisenseData entityData, AttributeIntellisenseData attribute)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}.{1}", entityData.EntityLogicalName, attribute.LogicalName);

            {
                string temp = CreateFileHandler.GetLocalizedLabel(entityData.DisplayName);

                if (!string.IsNullOrEmpty(temp))
                {
                    result.AppendFormat(" - {0}", temp);
                }

                if (entityData.IsIntersectEntity)
                {
                    result.Append(" - IntersectEntity");
                }
            }

            {
                string temp = CreateFileHandler.GetLocalizedLabel(attribute.DisplayName);

                if (!string.IsNullOrEmpty(temp))
                {
                    result.AppendFormat(" - {0}", temp);
                }

                if (attribute.IsPrimaryIdAttribute.GetValueOrDefault())
                {
                    result.Append(" - PrimaryId");
                }

                if (attribute.IsPrimaryNameAttribute.GetValueOrDefault())
                {
                    result.Append(" - PrimaryName");
                }
            }

            if (attribute.AttributeType.HasValue)
            {
                result.AppendFormat(" - {0}", attribute.AttributeType.ToString());
            }

            return result.ToString();
        }

        public static string GetDisplayTextOptionSetValue(string entityName, string attributeName, OptionMetadata optionMetadata)
        {
            return string.Format("{0}.{1}    {2} - {3}", entityName, attributeName, CreateFileHandler.GetLocalizedLabel(optionMetadata.Label), optionMetadata.Value);
        }

        public static List<string> GetCompareValuesForEntity(EntityIntellisenseData entityData)
        {
            List<string> result = GetCompareValues(entityData.DisplayName);

            result.Add(entityData.EntityLogicalName);

            if (entityData.IsIntersectEntity)
            {
                result.Add("IntersectEntity");
            }

            if (entityData.ObjectTypeCode.HasValue)
            {
                result.Add(entityData.ObjectTypeCode.Value.ToString());
            }

            return result;
        }

        public static List<string> GetCompareValuesForAttribute(AttributeIntellisenseData attribute)
        {
            List<string> result = new List<string>();

            FillCompareValuesForAttribute(result, attribute);

            return result;
        }

        public static void FillCompareValuesForAttribute(List<string> result, AttributeIntellisenseData attribute)
        {
            result.AddRange(GetCompareValues(attribute.DisplayName));

            result.Add(attribute.LogicalName);

            if (attribute.AttributeType.HasValue)
            {
                result.Add(attribute.AttributeType.ToString());
            }

            if (attribute.IsPrimaryIdAttribute.GetValueOrDefault())
            {
                result.Add("PrimaryId");
            }

            if (attribute.IsPrimaryNameAttribute.GetValueOrDefault())
            {
                result.Add("PrimaryName");
            }

            if (attribute.Targets != null && attribute.Targets.Count > 0)
            {
                result.AddRange(attribute.Targets);
            }
        }

        public static List<string> GetCompareValues(Label label)
        {
            List<string> result = new List<string>();

            if (label != null)
            {
                result.AddRange(label.LocalizedLabels.Where(l => !string.IsNullOrEmpty(l.Label)).Select(l => l.Label).Distinct());
            }

            return result;
        }
    }
}