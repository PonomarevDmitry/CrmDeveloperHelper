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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public sealed partial class XmlCompletionSource
    {
        private void FillSessionForSiteMapCompletionSet(IList<CompletionSet> completionSets, ITextSnapshot snapshot, ConnectionData connectionData, ConnectionIntellisenseDataRepository repositoryEntities, SiteMapIntellisenseDataRepository repositorySiteMap, WebResourceIntellisenseDataRepository repositoryWebResource, SnapshotSpan extent, string currentNodeName, string currentAttributeName, ITrackingSpan applicableTo)
        {
            try
            {
                if (string.Equals(currentNodeName, "SiteMap", StringComparison.InvariantCultureIgnoreCase))
                {
                    #region Urls

                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesTextWithWebResourcePrefix(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.GetHtmlWebResources()?.ToList(), "WebResources");
                    }

                    #endregion Urls
                }
                else if (string.Equals(currentNodeName, "Area", StringComparison.InvariantCultureIgnoreCase))
                {
                    #region Urls

                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesTextWithWebResourcePrefix(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.GetHtmlWebResources()?.ToList(), "WebResources");
                    }

                    #endregion Urls

                    #region Icons

                    else if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositoryWebResource.GetWebResourcesIcon()?.Values?.ToList(), "WebResources");
                    }

                    #endregion Icons

                    #region Resources

                    else if (string.Equals(currentAttributeName, "ResourceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().ResourceIds, "Resources");
                    }
                    else if (string.Equals(currentAttributeName, "DescriptionResourceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().DescriptionResourceIds, "Resources");
                    }
                    else if (string.Equals(currentAttributeName, "ToolTipResourseId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().ToolTipResourseIds, "Resources");
                    }

                    #endregion Resources
                }
                else if (string.Equals(currentNodeName, "Group", StringComparison.InvariantCultureIgnoreCase))
                {
                    #region Urls

                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesTextWithWebResourcePrefix(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.GetHtmlWebResources()?.ToList(), "WebResources");
                    }

                    #endregion Urls

                    #region Icons

                    else if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositoryWebResource.GetWebResourcesIcon()?.Values?.ToList(), "WebResources");
                    }

                    #endregion Icons

                    #region Resources

                    else if (string.Equals(currentAttributeName, "ResourceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().ResourceIds, "Resources");
                    }
                    else if (string.Equals(currentAttributeName, "DescriptionResourceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().DescriptionResourceIds, "Resources");
                    }
                    else if (string.Equals(currentAttributeName, "ToolTipResourseId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().ToolTipResourseIds, "Resources");
                    }

                    #endregion Resources
                }
                else if (string.Equals(currentNodeName, "SubArea", StringComparison.InvariantCultureIgnoreCase))
                {
                    #region Urls

                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesTextWithWebResourcePrefix(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.GetHtmlWebResources()?.ToList(), "WebResources");
                    }

                    #endregion Urls

                    #region Icons

                    else if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositoryWebResource.GetWebResourcesIcon()?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "OutlookShortcutIcon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositoryWebResource.GetWebResourcesIcon()?.Values?.ToList(), "WebResources");
                    }

                    #endregion Icons

                    #region Resources

                    else if (string.Equals(currentAttributeName, "ResourceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().ResourceIds, "Resources");
                    }
                    else if (string.Equals(currentAttributeName, "DescriptionResourceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().DescriptionResourceIds, "Resources");
                    }
                    else if (string.Equals(currentAttributeName, "ToolTipResourseId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().ToolTipResourseIds, "Resources");
                    }

                    #endregion Resources

                    #region Panes

                    else if (string.Equals(currentAttributeName, "GetStartedPanePath", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().GetStartedPanePaths, "Panes");
                    }
                    else if (string.Equals(currentAttributeName, "GetStartedPanePathOutlook", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().GetStartedPanePathOutlooks, "Panes");
                    }
                    else if (string.Equals(currentAttributeName, "GetStartedPanePathAdmin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().GetStartedPanePathAdmins, "Panes");
                    }
                    else if (string.Equals(currentAttributeName, "GetStartedPanePathAdminOutlook", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().GetStartedPanePathAdminOutlooks, "Panes");
                    }

                    #endregion Panes

                    else if (string.Equals(currentAttributeName, "Entity", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repositoryEntities, false);
                    }
                    else if (string.Equals(currentAttributeName, "Sku", StringComparison.InvariantCultureIgnoreCase))
                    {
                        applicableTo = SkipComma(snapshot, extent, applicableTo);

                        FillSku(completionSets, applicableTo);
                    }
                    else if (string.Equals(currentAttributeName, "Client", StringComparison.InvariantCultureIgnoreCase))
                    {
                        applicableTo = SkipComma(snapshot, extent, applicableTo);

                        FillClient(completionSets, applicableTo);
                    }

                    else if (string.Equals(currentAttributeName, "DefaultDashboard", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillDashboards(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.Dashboards?.Values?.ToList(), "Dashboards");
                    }

                    else if (string.Equals(currentAttributeName, "CheckExtensionProperty", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().CheckExtensionProperties, "Properties");
                    }
                }
                else if (string.Equals(currentNodeName, "Privilege", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Entity", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repositoryEntities, false);
                    }
                    else if (string.Equals(currentAttributeName, "Privilege", StringComparison.InvariantCultureIgnoreCase))
                    {
                        applicableTo = SkipComma(snapshot, extent, applicableTo);

                        FillPrivileges(completionSets, applicableTo);
                    }
                }
                else if (string.Equals(currentNodeName, "Title", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(currentNodeName, "Description", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    if (string.Equals(currentAttributeName, "LCID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillLCID(completionSets, applicableTo);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }

        private void FillDashboards(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<SystemFormIntellisenseData> dashboards, string nameCompletionSet)
        {
            if (dashboards == null || !dashboards.Any())
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var dashboard in dashboards.OrderBy(s => s.Name))
            {
                StringBuilder str = new StringBuilder();

                if (dashboard.ObjectTypeCode.IsValidEntityName())
                {
                    str.AppendFormat("{0} - ", dashboard.ObjectTypeCode);
                }

                str.Append(dashboard.Name);

                List<string> compareValues = new List<string>() { dashboard.Name, dashboard.ObjectTypeCode };

                list.Add(CreateCompletion(str.ToString(), dashboard.FormId.ToString().ToLower(), dashboard.Description, _defaultGlyph, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerDefaultSingle, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillIntellisenseBySet(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, SortedSet<string> values, string nameCompletionSet)
        {
            if (values == null || !values.Any())
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var value in values)
            {
                list.Add(CreateCompletion(value, value, null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerDefaultSingle, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private static ITrackingSpan SkipComma(ITextSnapshot snapshot, SnapshotSpan extent, ITrackingSpan applicableTo)
        {
            var value = extent.GetText();

            var indexComma = value.LastIndexOf(',');

            if (indexComma > -1)
            {
                indexComma++;

                applicableTo = snapshot.CreateTrackingSpan(extent.Start + indexComma, extent.Length - indexComma, SpanTrackingMode.EdgeInclusive);
            }

            return applicableTo;
        }

        private static readonly string[] _clients = { "All", "Web", "Outlook", "OutlookWorkstationClient", "OutlookLaptopClient" };

        private void FillClient(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var client in _clients)
            {
                list.Add(CreateCompletion(client, client, null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerDefaultSingle, "Client", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private static readonly string[] _skus = { "All", "OnPremise", "Live", "SPLA" };

        private void FillSku(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var sku in _skus)
            {
                list.Add(CreateCompletion(sku, sku, null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerDefaultSingle, "Sku", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private static readonly string[] _privileges = { "All", "Create", "Read", "Write", "Delete", "Append", "AppendTo", "Share", "Assign", "AllowQuickCampaign", "CreateEntity", "ImportCustomization", "UseInternetMarketing", "LearningPath" };

        private void FillPrivileges(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var priv in _privileges)
            {
                list.Add(CreateCompletion(priv, priv, null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerDefaultSingle, "Privileges", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }
    }
}