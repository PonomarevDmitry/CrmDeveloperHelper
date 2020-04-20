using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public partial class XmlCompletionSource
    {
        private void FillSessionForFormXmlCompletionSet(
            IList<CompletionSet> completionSets
            , XElement doc
            , ConnectionData connectionData
            , WebResourceIntellisenseDataRepository repositoryWebResource
            , XElement currentXmlNode
            , string currentNodeName
            , string currentAttributeName
            , ITrackingSpan applicableTo
        )
        {
            try
            {
                if (string.Equals(currentNodeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillWebResourcesNames(completionSets, applicableTo, repositoryWebResource.GetWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList(), "WebResources");
                    }
                }
                else if (string.Equals(currentNodeName, "Handler", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "libraryName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillLibrariesSet(completionSets, applicableTo, doc, "Libraries");
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }

        private void FillLibrariesSet(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string nameCompletionSet)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./formLibraries/Library").Where(e => e.Attribute("name") != null && !string.IsNullOrEmpty(e.Attribute("name").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("name")))
            {
                string libraryName = (string)label.Attribute("name");

                list.Add(CreateCompletion(libraryName, libraryName, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerWebResourcesText, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }
    }
}