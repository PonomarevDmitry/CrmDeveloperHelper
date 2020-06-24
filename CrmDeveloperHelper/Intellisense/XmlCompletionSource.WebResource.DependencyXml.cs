using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public partial class XmlCompletionSource
    {
        private void FillSessionForWebResourceDependencyXmlCompletionSet(
            IList<CompletionSet> completionSets
            , XElement doc
            , ConnectionData connectionData
            , ConnectionIntellisenseDataRepository repositoryEntities
            , WebResourceIntellisenseDataRepository repositoryWebResource
            , XElement currentXmlNode
            , string currentNodeName
            , string currentAttributeName
            , ITrackingSpan applicableTo
        )
        {
            try
            {
                if (string.Equals(currentNodeName, "Attribute", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "entityName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repositoryEntities, false);
                    }
                    else if (string.Equals(currentAttributeName, "attributeName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityAttributesInWebResourceDependencyXml(completionSets, applicableTo, repositoryEntities, currentXmlNode);
                    }
                    else if (string.Equals(currentAttributeName, "attributeId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityAttributeIdInWebResourceDependencyXml(completionSets, applicableTo, repositoryEntities, currentXmlNode);
                    }
                }
                else if (string.Equals(currentNodeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillWebResourcesNames(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "description", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillSingleWebResourceAttribute(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList(), currentXmlNode, WebResource.Schema.Attributes.description, w => w.Description);
                    }
                    else if (string.Equals(currentAttributeName, "displayName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillSingleWebResourceAttribute(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList(), currentXmlNode, WebResource.Schema.Attributes.displayname, w => w.DisplayName);
                    }
                    else if (string.Equals(currentAttributeName, "languagecode", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillSingleWebResourceAttribute(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList(), currentXmlNode, WebResource.Schema.Attributes.languagecode, w => w.LanguageCode.ToString());
                    }
                    else if (string.Equals(currentAttributeName, "libraryUniqueId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillNewGuid(completionSets, applicableTo);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }

        private void FillSingleWebResourceAttribute(
            IList<CompletionSet> completionSets
            , ITrackingSpan applicableTo
            , IEnumerable<WebResourceIntellisenseData> webResourcesDataList
            , XElement currentXmlNode
            , string fieldName
            , Func<WebResourceIntellisenseData, string> getValue
        )
        {
            if (webResourcesDataList == null || !webResourcesDataList.Any())
            {
                return;
            }

            var webResourceName = GetAttributeValue(currentXmlNode, "name");

            if (string.IsNullOrEmpty(webResourceName))
            {
                return;
            }

            var webResourceData = webResourcesDataList.FirstOrDefault(w => string.Equals(webResourceName, w.Name));

            if (webResourceData == null)
            {
                return;
            }

            string attributeValue = getValue(webResourceData);

            if (string.IsNullOrEmpty(attributeValue))
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            StringBuilder str = new StringBuilder(webResourceData.Name);

            List<string> compareValues = new List<string>() { webResourceData.Name };

            if (!string.IsNullOrEmpty(webResourceData.DisplayName))
            {
                compareValues.Add(webResourceData.DisplayName);

                str.AppendFormat(" - {0}", webResourceData.DisplayName);
            }

            str.AppendFormat(" - {0}", fieldName);

            list.Add(CreateCompletion(str.ToString(), attributeValue, webResourceData.Description, _defaultGlyph, compareValues));

            string nameCompletionSet = $"WebResource - {fieldName}";

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerSingleWebResourceAttribute, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillEntityAttributesInWebResourceDependencyXml(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repositoryEntities, XElement currentXmlNode)
        {
            var entityName = GetAttributeValue(currentXmlNode, "entityName");

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var entityData = repositoryEntities.GetEntityAttributeIntellisense(entityName);

            if (entityData == null || entityData.Attributes == null)
            {
                return;
            }

            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityData);
        }

        private void FillEntityAttributeIdInWebResourceDependencyXml(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repositoryEntities, XElement currentXmlNode)
        {
            var entityName = GetAttributeValue(currentXmlNode, "entityName");
            var attributeName = GetAttributeValue(currentXmlNode, "attributeName");

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var entityData = repositoryEntities.GetEntityAttributeIntellisense(entityName);

            if (entityData == null || entityData.Attributes == null || !entityData.Attributes.ContainsKey(attributeName))
            {
                return;
            }

            var attributeData = entityData.Attributes[attributeName];

            if (attributeData.MetadataId.HasValue)
            {
                List<CrmCompletion> list = new List<CrmCompletion>();

                string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);
                string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(entityData.EntityLogicalName, attributeData);

                var attributeIdString = attributeData.MetadataId.Value.ToString("B");

                List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForAttribute(attributeData);
                compareValues.Add(attributeIdString);

                list.Add(CreateCompletion(attributeDescription, attributeIdString, CrmIntellisenseCommon.CreateAttributeDescription(entityDescription, attributeData), _defaultGlyph, compareValues));

                completionSets.Add(new CrmCompletionSet(SourceNameMonikerAllAttributes, string.Format("{0}.{1}", entityName, attributeName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }
    }
}