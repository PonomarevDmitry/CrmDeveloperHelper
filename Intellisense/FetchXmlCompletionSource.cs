using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public sealed class FetchXmlCompletionSource : ICompletionSource, IDisposable
    {
        private const string SourceNameMoniker = "CrmFetchXml.{8A22EFE3-4F6D-47A8-A511-82DE08260BA7}";

        private const string SourceNameMonikerAll = "CrmFetchXml.{E438F5CE-FBBD-4754-A2F7-F1AEB6752499}";

        private const string SourceNameMonikerAttributes = "CrmFetchXml.{417B4B2F-EC8A-4EBC-A6CD-A45013171817}";

        private readonly FetchXmlCompletionSourceProvider _sourceProvider;

        private ITextBuffer _buffer;
        private IClassifier _classifier;
        private ITextStructureNavigatorSelectorService _navigator;

        private readonly ImageSource _defaultGlyph;
        private readonly ImageSource _builtInGlyph;
        private readonly IGlyphService _glyphService;

        public FetchXmlCompletionSource(FetchXmlCompletionSourceProvider sourceProvider, ITextBuffer buffer, IClassifierAggregatorService classifier, ITextStructureNavigatorSelectorService navigator, IGlyphService glyphService)
        {
            _sourceProvider = sourceProvider;

            _buffer = buffer;
            _classifier = classifier.GetClassifier(buffer);
            _navigator = navigator;
            _glyphService = glyphService;
            _defaultGlyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
            _builtInGlyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupProperty, StandardGlyphItem.TotalGlyphItems);
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_isDisposed)
                return;

            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig?.CurrentConnectionData == null)
            {
                return;
            }

            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            var triggerPoint = session.GetTriggerPoint(snapshot);

            if (triggerPoint == null)
            {
                return;
            }

            XElement doc = ReadXmlDocument(snapshot.GetText());

            if (doc == null)
            {
                return;
            }

            if (string.Equals(doc.Name.LocalName, "fetch", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForFetchXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.LocalName, "grid", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForGridXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.LocalName, "savedquery", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForFetchXml(session, completionSets, snapshot, doc, repository);

                FillSessionForGridXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.LocalName, "SiteMap", StringComparison.InvariantCultureIgnoreCase))
            {
                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                var repositorySiteMap = SiteMapIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForSiteMap(triggerPoint.Value, session, completionSets, snapshot, doc, repositoryEntities, repositorySiteMap);
            }
        }

        private void FillSessionForSiteMap(
            SnapshotPoint triggerPoint
            , ICompletionSession session
            , IList<CompletionSet> completionSets
            , ITextSnapshot snapshot
            , XElement doc
            , ConnectionIntellisenseDataRepository repositoryEntities
            , SiteMapIntellisenseDataRepository repositorySiteMap
            )
        {
            {
                HashSet<string> usedEntities = GetUsedEntities(doc);

                if (usedEntities.Any())
                {
                    repositoryEntities.GetEntityDataForNamesAsync(usedEntities);
                }
            }

            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(snapshot, 0, snapshot.Length));

            var firstSpans = spans.Where(s =>
                    s.Span.Start <= currentPoint.Position
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .ToList();

            var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            var lastSpans = spans.Where(s =>
                    s.Span.Start > currentPoint.Position
                    )
                    .OrderBy(s => s.Span.Start.Position)
                    .ToList();

            var lastDelimiter = lastSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            SnapshotSpan? extentTemp = null;

            if (firstDelimiter != null && firstDelimiter.Span.GetText() == "\"\"")
            {
                extentTemp = new SnapshotSpan(firstDelimiter.Span.Start.Add(1), firstDelimiter.Span.Start.Add(1));
            }
            else if (firstDelimiter != null && lastDelimiter != null && firstDelimiter.Span.GetText() == "\"" && lastDelimiter.Span.GetText() == "\"")
            {
                extentTemp = new SnapshotSpan(firstDelimiter.Span.End, lastDelimiter.Span.Start);
            }

            if (!extentTemp.HasValue)
            {
                return;
            }

            var extent = extentTemp.Value;

            {
                var extentText = extent.GetText();

                if (extentText == ",\"")
                {
                    extent = new SnapshotSpan(extent.Snapshot, extent.Start, extent.Length - 1);
                }
            }

            var currentXmlNode = GetCurrentXmlNode(doc, extent);

            if (currentXmlNode == null)
            {
                return;
            }

            var containingAttributeSpans = spans
                .Where(s => s.Span.Contains(extent.Start)
                && s.Span.Contains(extent)
                && s.ClassificationType.IsOfType("XML Attribute Value"))
                .OrderByDescending(s => s.Span.Start.Position)
                .ToList();

            var containingAttributeValue = containingAttributeSpans.FirstOrDefault();

            if (containingAttributeValue == null)
            {
                containingAttributeValue = spans
                    .Where(s => s.Span.Contains(extent.Start)
                    && s.Span.Contains(extent)
                    && s.ClassificationType.IsOfType("XML Attribute Quotes")
                    && s.Span.GetText() == "\"\""
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();
            }

            if (containingAttributeValue == null)
            {
                return;
            }

            ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

            if (currentAttr == null)
            {
                return;
            }

            string currentNodeName = currentXmlNode.Name.LocalName;

            string currentAttributeName = currentAttr.Span.GetText();

            ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

            try
            {
                if (string.Equals(currentNodeName, "SiteMap", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesHtml(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesHtml?.Values?.ToList(), "WebResources");
                    }
                }
                else if (string.Equals(currentNodeName, "Area", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesHtml(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesHtml?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesIcon?.Values?.ToList(), "WebResources");
                    }
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
                }
                else if (string.Equals(currentNodeName, "Group", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesHtml(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesHtml?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesIcon?.Values?.ToList(), "WebResources");
                    }
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
                }
                else if (string.Equals(currentNodeName, "SubArea", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Entity", StringComparison.InvariantCultureIgnoreCase))
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

                    else if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Urls, "Urls");

                        FillWebResourcesHtml(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesHtml?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesIcon?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "OutlookShortcutIcon", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData().Icons, "Icons");

                        FillWebResourcesIcons(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.WebResourcesIcon?.Values?.ToList(), "WebResources");
                    }

                    else if (string.Equals(currentAttributeName, "DefaultDashboard", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillDashboards(completionSets, applicableTo, repositorySiteMap.GetSiteMapIntellisenseData()?.Dashboards?.Values?.ToList(), "Dashboards");
                    }
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
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }

        private void FillDashboards(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<SystemForm> dashboards, string name)
        {
            if (dashboards == null || !dashboards.Any())
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var dashboard in dashboards.OrderBy(s => s.Name))
            {
                StringBuilder str = new StringBuilder();

                if (!string.Equals(dashboard.ObjectTypeCode, "none", StringComparison.InvariantCultureIgnoreCase))
                {
                    str.AppendFormat("{0} - ", dashboard.ObjectTypeCode);
                }

                str.Append(dashboard.Name);

                List<string> compareValues = new List<string>() { dashboard.Name, dashboard.ObjectTypeCode };

                list.Add(CreateCompletion(str.ToString(), dashboard.Id.ToString().ToLower(), dashboard.Description, _defaultGlyph, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillWebResourcesHtml(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<WebResource> webResources, string name)
        {
            if (webResources == null || !webResources.Any())
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var resource in webResources.OrderBy(s => s.Name))
            {
                StringBuilder str = new StringBuilder(resource.Name);

                List<string> compareValues = new List<string>() { resource.Name };

                if (!string.IsNullOrEmpty(resource.DisplayName))
                {
                    compareValues.Add(resource.DisplayName);

                    str.AppendFormat(" - {0}", resource.DisplayName);
                }

                string insertText = string.Format("$webresource:{0}", resource.Name);

                list.Add(CreateCompletion(str.ToString(), insertText, resource.Description, _defaultGlyph, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerAll, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillWebResourcesIcons(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<WebResource> webResources, string name)
        {
            if (webResources == null || !webResources.Any())
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var resource in webResources.OrderBy(s => s.Name))
            {
                StringBuilder str = new StringBuilder(resource.Name);

                List<string> compareValues = new List<string>() { resource.Name };

                if (!string.IsNullOrEmpty(resource.DisplayName))
                {
                    compareValues.Add(resource.DisplayName);

                    str.AppendFormat(" - {0}", resource.DisplayName);
                }

                ImageSource image = _defaultGlyph;

                if (!string.IsNullOrEmpty(resource.Content))
                {
                    try
                    {
                        var array = Convert.FromBase64String(resource.Content);

                        if (array != null && array.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream(array);

                            if (resource.WebResourceType?.Value == (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5)
                            {
                                PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                                if (decoder.Frames.Count > 0)
                                {
                                    var bitmapSource = decoder.Frames[0];

                                    image = bitmapSource;
                                }
                            }
                            else
                            {
                                BitmapImage biImg = new BitmapImage();

                                biImg.DecodePixelHeight = biImg.DecodePixelWidth = 16;

                                biImg.BeginInit();
                                biImg.StreamSource = ms;
                                biImg.EndInit();

                                image = biImg;
                            }

                            if (image != null)
                            {
                                str.AppendFormat(" - {0}x{1}", Convert.ToInt32(image.Width), Convert.ToInt32(image.Height));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        image = _defaultGlyph;
                    }
                }

                string insertText = string.Format("$webresource:{0}", resource.Name);

                list.Add(CreateCompletion(str.ToString(), insertText, resource.Description, image, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerAll, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillIntellisenseBySet(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, SortedSet<string> values, string name)
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

            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
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

            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Client", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private static readonly string[] _skus = { "All", "OnPremise", "Live", "SPLA" };

        private void FillSku(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var sku in _skus)
            {
                list.Add(CreateCompletion(sku, sku, null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Sku", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private static readonly string[] _privileges = { "All", "Create", "Read", "Write", "Delete", "Append", "AppendTo", "Share", "Assign", "AllowQuickCampaign", "CreateEntity", "ImportCustomization", "UseInternetMarketing", "LearningPath" };

        private void FillPrivileges(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var priv in _privileges)
            {
                list.Add(CreateCompletion(priv, priv, null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Privileges", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillLCID(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var keys = LanguageLocale.KnownLocales.Keys;

            foreach (var lcid in keys.OrderBy(i => i))
            {
                string entityDescription = string.Format("{0} - {1}", LanguageLocale.KnownLocales[lcid], lcid);

                List<string> compareValues = new List<string>
                {
                    LanguageLocale.KnownLocales[lcid],
                    lcid.ToString()
                };

                var insertionText = lcid.ToString();

                list.Add(CreateCompletion(entityDescription, insertionText, null, _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All LCID", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillSessionForGridXml(ICompletionSession session, IList<CompletionSet> completionSets, ITextSnapshot snapshot, XElement doc, ConnectionIntellisenseDataRepository repository)
        {
            {
                HashSet<int> usedEntityCodes = GetUsedEntityObjectTypeCodes(doc);

                if (usedEntityCodes.Any())
                {
                    repository.GetEntityDataForObjectTypeCodesAsync(usedEntityCodes);
                }
            }

            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(snapshot, 0, snapshot.Length));

            var firstSpans = spans.Where(s =>
                    s.Span.Start <= currentPoint.Position
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .ToList();

            var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            var lastSpans = spans.Where(s =>
                    s.Span.Start > currentPoint.Position
                    )
                    .OrderBy(s => s.Span.Start.Position)
                    .ToList();

            var lastDelimiter = lastSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            SnapshotSpan? extentTemp = null;

            if (firstDelimiter != null && firstDelimiter.Span.GetText() == "\"\"")
            {
                extentTemp = new SnapshotSpan(firstDelimiter.Span.Start.Add(1), firstDelimiter.Span.Start.Add(1));
            }
            else if (firstDelimiter != null && lastDelimiter != null && firstDelimiter.Span.GetText() == "\"" && lastDelimiter.Span.GetText() == "\"")
            {
                extentTemp = new SnapshotSpan(firstDelimiter.Span.End, lastDelimiter.Span.Start);
            }

            if (!extentTemp.HasValue)
            {
                return;
            }

            var extent = extentTemp.Value;

            {
                var extentText = extent.GetText();

                if (extentText == ",\"")
                {
                    extent = new SnapshotSpan(extent.Snapshot, extent.Start, extent.Length - 1);
                }
            }

            var currentXmlNode = GetCurrentXmlNode(doc, extent);

            if (currentXmlNode == null)
            {
                return;
            }

            var containingAttributeSpans = spans
                .Where(s => s.Span.Contains(extent.Start)
                && s.Span.Contains(extent)
                && s.ClassificationType.IsOfType("XML Attribute Value"))
                .OrderByDescending(s => s.Span.Start.Position)
                .ToList();

            var containingAttributeValue = containingAttributeSpans.FirstOrDefault();

            if (containingAttributeValue == null)
            {
                containingAttributeValue = spans
                    .Where(s => s.Span.Contains(extent.Start)
                    && s.Span.Contains(extent)
                    && s.ClassificationType.IsOfType("XML Attribute Quotes")
                    && s.Span.GetText() == "\"\""
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();
            }

            if (containingAttributeValue == null)
            {
                return;
            }

            ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

            if (currentAttr == null)
            {
                return;
            }

            string currentNodeName = currentXmlNode.Name.LocalName;

            string currentAttributeName = currentAttr.Span.GetText();

            ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

            try
            {
                if (string.Equals(currentNodeName, "grid", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "object", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repository, true);
                    }
                    else if (string.Equals(currentAttributeName, "jump", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityPrimaryAttributeForGrid(completionSets, applicableTo, repository, currentXmlNode, true);

                        FillEntityAttributesInListForGrid(completionSets, applicableTo, repository, currentXmlNode);
                    }
                }
                else if (string.Equals(currentNodeName, "row", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityPrimaryAttributeForGrid(completionSets, applicableTo, repository, currentXmlNode, false);

                        FillEntityAttributesInListForGrid(completionSets, applicableTo, repository, currentXmlNode);
                    }
                }
                else if (string.Equals(currentNodeName, "cell", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityAttributesInListForGrid(completionSets, applicableTo, repository, currentXmlNode);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }

        private void FillSessionForFetchXml(ICompletionSession session, IList<CompletionSet> completionSets, ITextSnapshot snapshot, XElement doc, ConnectionIntellisenseDataRepository repository)
        {
            {
                HashSet<string> usedEntities = GetUsedEntities(doc);

                if (usedEntities.Any())
                {
                    repository.GetEntityDataForNamesAsync(usedEntities);
                }
            }

            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(snapshot, 0, snapshot.Length));

            var firstSpans = spans.Where(s =>
                    s.Span.Start <= currentPoint.Position
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .ToList();

            var lastSpans = spans.Where(s =>
                    s.Span.Start > currentPoint.Position
                    )
                    .OrderBy(s => s.Span.Start.Position)
                    .ToList();

            SnapshotSpan? extentTemp = null;

            if (!extentTemp.HasValue)
            {
                var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

                if (firstDelimiter != null && firstDelimiter.Span.GetText() == "\"\"")
                {
                    extentTemp = new SnapshotSpan(firstDelimiter.Span.Start.Add(1), firstDelimiter.Span.Start.Add(1));
                }
            }

            if (!extentTemp.HasValue)
            {
                var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));
                var lastDelimiter = lastSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

                if (firstDelimiter != null && lastDelimiter != null && firstDelimiter.Span.GetText() == "\"" && lastDelimiter.Span.GetText() == "\"")
                {
                    extentTemp = new SnapshotSpan(firstDelimiter.Span.End, lastDelimiter.Span.Start);
                }
            }

            if (extentTemp.HasValue)
            {
                var extent = extentTemp.Value;

                {
                    var extentText = extent.GetText();

                    if (extentText == ",\"")
                    {
                        extent = new SnapshotSpan(extent.Snapshot, extent.Start, extent.Length - 1);
                    }
                }

                var currentXmlNode = GetCurrentXmlNode(doc, extent);

                if (currentXmlNode == null)
                {
                    return;
                }

                var containingAttributeSpans = spans
                    .Where(s => s.Span.Contains(extent.Start)
                    && s.Span.Contains(extent)
                    && s.ClassificationType.IsOfType("XML Attribute Value"))
                    .OrderByDescending(s => s.Span.Start.Position)
                    .ToList();

                var containingAttributeValue = containingAttributeSpans.FirstOrDefault();

                if (containingAttributeValue == null)
                {
                    containingAttributeValue = spans
                        .Where(s => s.Span.Contains(extent.Start)
                        && s.Span.Contains(extent)
                        && s.ClassificationType.IsOfType("XML Attribute Quotes")
                        && s.Span.GetText() == "\"\""
                        )
                        .OrderByDescending(s => s.Span.Start.Position)
                        .FirstOrDefault();
                }

                if (containingAttributeValue != null)
                {
                    ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

                    if (currentAttr == null)
                    {
                        return;
                    }

                    string currentNodeName = currentXmlNode.Name.LocalName;

                    string currentAttributeName = currentAttr.Span.GetText();

                    ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

                    try
                    {
                        if (string.Equals(currentNodeName, "entity", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillEntityNamesInList(completionSets, applicableTo, repository, false);
                            }
                        }
                        else if (string.Equals(currentNodeName, "attribute", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Dictionary<string, string> aliases = GetEntityAliases(doc);

                                FillEntityAttributesInList(completionSets, applicableTo, repository, currentXmlNode, aliases);
                            }
                        }
                        else if (string.Equals(currentNodeName, "order", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (string.Equals(currentAttributeName, "attribute", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Dictionary<string, string> aliases = GetEntityAliases(doc);

                                FillEntityAttributesInList(completionSets, applicableTo, repository, currentXmlNode, aliases);
                            }
                        }
                        else if (string.Equals(currentNodeName, "condition", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Dictionary<string, string> aliases = GetEntityAliases(doc);

                            if (string.Equals(currentAttributeName, "attribute", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillEntityAttributesInList(completionSets, applicableTo, repository, currentXmlNode, aliases);
                            }
                            else if (string.Equals(currentAttributeName, "value", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillEntityAttributeValuesInList(completionSets, applicableTo, repository, currentXmlNode, aliases);
                            }
                            else if (string.Equals(currentAttributeName, "entityname", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillAliases(completionSets, applicableTo, aliases);
                            }
                        }
                        else if (string.Equals(currentNodeName, "link-entity", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillLinkedEntityNames(completionSets, applicableTo, repository, currentXmlNode);

                                FillEntityNamesInList(completionSets, applicableTo, repository, false);
                            }
                            else if (string.Equals(currentAttributeName, "from", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillLinkedEntityFromField(completionSets, applicableTo, repository, currentXmlNode);
                            }
                            else if (string.Equals(currentAttributeName, "to", StringComparison.InvariantCultureIgnoreCase))
                            {
                                FillLinkedEntityToField(completionSets, applicableTo, repository, currentXmlNode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(ex);
                    }
                }
            }
            else
            {
                extentTemp = null;

                if (!extentTemp.HasValue)
                {
                    var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Delimiter") && s.Span.GetText().EndsWith("></"));

                    if (firstDelimiter != null && firstDelimiter.Span.GetText() == "></")
                    {
                        extentTemp = new SnapshotSpan(firstDelimiter.Span.Start.Add(1), firstDelimiter.Span.Start.Add(1));
                    }
                }

                if (!extentTemp.HasValue)
                {
                    var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Delimiter") && s.Span.GetText().Trim().EndsWith(">"));
                    var lastDelimiter = lastSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Delimiter") && s.Span.GetText().Trim().StartsWith("</"));

                    if (firstDelimiter != null && lastDelimiter != null)
                    {
                        var temp = new SnapshotSpan(firstDelimiter.Span.End, lastDelimiter.Span.Start);

                        if (string.IsNullOrWhiteSpace(temp.GetText()))
                        {
                            extentTemp = new SnapshotSpan(firstDelimiter.Span.End, 0);
                        }
                        else
                        {
                            int spacesStart = temp.GetText().TakeWhile(ch => char.IsWhiteSpace(ch)).Count();
                            int spacesEnd = temp.GetText().Reverse().TakeWhile(ch => char.IsWhiteSpace(ch)).Count();

                            extentTemp = new SnapshotSpan(firstDelimiter.Span.End.Add(spacesStart), lastDelimiter.Span.Start.Add(-spacesEnd));
                        }
                    }
                }

                if (!extentTemp.HasValue)
                {
                    return;
                }

                var extent = extentTemp.Value;

                var currentXmlNode = GetCurrentXmlNode(doc, extent);

                if (currentXmlNode == null)
                {
                    return;
                }

                if (!string.Equals(currentXmlNode.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var nodeCondition = currentXmlNode.Parent;

                if (nodeCondition == null || !string.Equals(nodeCondition.Name.LocalName, "condition", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);

                Dictionary<string, string> aliases = GetEntityAliases(doc);

                FillEntityAttributeValuesInList(completionSets, applicableTo, repository, nodeCondition, aliases);
            }
        }

        private void FillLinkedEntityFromField(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            var parentEntityName = GetParentEntityName(currentXmlNode);

            var linkEntityName = GetAttributeValue(currentXmlNode, "name");

            FillPrimaryEntityAttributes(completionSets, applicableTo, repository, linkEntityName, parentEntityName);
        }

        private void FillLinkedEntityToField(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            var parentEntityName = GetParentEntityName(currentXmlNode);

            var linkEntityName = GetAttributeValue(currentXmlNode, "name");

            FillPrimaryEntityAttributes(completionSets, applicableTo, repository, parentEntityName, linkEntityName);
        }

        private void FillPrimaryEntityAttributes(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, string primaryEntityName, string secondaryName)
        {
            if (string.IsNullOrEmpty(primaryEntityName))
            {
                return;
            }

            var primaryEntityData = repository.GetEntityAttributeIntellisense(primaryEntityName);

            if (primaryEntityData == null || primaryEntityData.Attributes == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(secondaryName))
            {
                var secondaryEntityData = repository.GetEntityAttributeIntellisense(secondaryName);

                if (secondaryEntityData != null)
                {
                    var secondaryReferenceEntities = GetReferencedEntities(secondaryEntityData);

                    var primaryReferenceEntities = GetReferencedEntities(primaryEntityData);

                    var commonEntities = new HashSet<string>(secondaryReferenceEntities.Intersect(primaryReferenceEntities), StringComparer.InvariantCultureIgnoreCase);

                    if (commonEntities.Any())
                    {
                        var attributes = GetRefererenceAttributes(primaryEntityData, commonEntities);

                        if (attributes.Any())
                        {
                            List<CrmCompletion> list = new List<CrmCompletion>();

                            string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(primaryEntityData);

                            foreach (var attribute in primaryEntityData.AttributesOrdered())
                            {
                                if (attributes.Contains(attribute.LogicalName))
                                {
                                    string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(primaryEntityData.EntityLogicalName, attribute);

                                    List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForAttribute(attribute);

                                    list.Add(CreateCompletion(attributeDescription.ToString(), attribute.LogicalName, CrmIntellisenseCommon.CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
                                }
                            }

                            if (list.Count > 0)
                            {
                                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0} Calculated", primaryEntityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                            }
                        }
                    }
                }
            }

            {
                List<CrmCompletion> list = new List<CrmCompletion>();

                string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(primaryEntityData);

                foreach (var attribute in primaryEntityData.AttributesOrdered())
                {
                    if (attribute.AttributeType == AttributeTypeCode.Uniqueidentifier
                        || attribute.AttributeType == AttributeTypeCode.Customer
                        || attribute.AttributeType == AttributeTypeCode.Lookup
                        || attribute.AttributeType == AttributeTypeCode.Owner
                        )
                    {
                        string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(primaryEntityData.EntityLogicalName, attribute);

                        List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForAttribute(attribute);

                        list.Add(CreateCompletion(attributeDescription.ToString(), attribute.LogicalName, CrmIntellisenseCommon.CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
                    }
                }

                if (list.Count > 0)
                {
                    completionSets.Add(new CrmCompletionSet(SourceNameMonikerAll, string.Format("{0} References", primaryEntityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                }
            }

            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, primaryEntityData);
        }

        private HashSet<string> GetRefererenceAttributes(EntityIntellisenseData entityData, HashSet<string> commonEntities)
        {
            HashSet<string> result = new HashSet<string>();

            if (!string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute))
            {
                if (commonEntities.Contains(entityData.EntityLogicalName))
                {
                    result.Add(entityData.EntityPrimaryIdAttribute);
                }
            }

            if (entityData.Attributes != null)
            {
                foreach (var attr in entityData.Attributes.Values)
                {
                    if (!string.Equals(entityData.EntityPrimaryIdAttribute, attr.LogicalName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (attr.AttributeType == AttributeTypeCode.Uniqueidentifier)
                        {
                            result.Add(attr.LogicalName);
                        }
                    }
                }
            }

            if (entityData.ManyToOneRelationships != null)
            {
                foreach (var item in entityData.ManyToOneRelationships.Values)
                {
                    if (commonEntities.Contains(item.TargetEntityName))
                    {
                        result.Add(item.BaseAttributeName);
                    }
                }
            }

            if (entityData.IsIntersectEntity && entityData.ManyToManyRelationships != null)
            {
                foreach (var item in entityData.ManyToManyRelationships.Values)
                {
                    if (commonEntities.Contains(item.Entity1Name))
                    {
                        result.Add(item.Entity1IntersectAttributeName);
                    }

                    if (commonEntities.Contains(item.Entity2Name))
                    {
                        result.Add(item.Entity2IntersectAttributeName);
                    }
                }
            }

            return result;
        }

        private HashSet<string> GetReferencedEntities(EntityIntellisenseData entityData)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (!string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute))
            {
                result.Add(entityData.EntityLogicalName);
            }

            if (entityData.ManyToOneRelationships != null)
            {
                foreach (var item in entityData.ManyToOneRelationships.Values)
                {
                    if (!string.IsNullOrEmpty(item.TargetEntityName))
                    {
                        result.Add(item.TargetEntityName);
                    }
                }
            }

            if (entityData.IsIntersectEntity)
            {
                if (entityData.ManyToManyRelationships != null)
                {
                    foreach (var item in entityData.ManyToManyRelationships.Values)
                    {
                        if (!string.IsNullOrEmpty(item.Entity1Name))
                        {
                            result.Add(item.Entity1Name);
                        }

                        if (!string.IsNullOrEmpty(item.Entity2Name))
                        {
                            result.Add(item.Entity2Name);
                        }
                    }
                }
            }

            return result;
        }

        private void FillLinkedEntityNames(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            var entityName = GetParentEntityName(currentXmlNode);

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var connectionIntellisense = repository.GetEntitiesIntellisenseData();

            if (connectionIntellisense == null || connectionIntellisense.Entities == null)
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityName);

            if (entityData == null)
            {
                return;
            }

            HashSet<string> linkedEntities = entityData.GetLinkedEntities(connectionIntellisense);

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var linkedEntityName in linkedEntities.OrderBy(s => s))
            {
                if (connectionIntellisense.Entities.ContainsKey(linkedEntityName))
                {
                    var linkedEntityData = connectionIntellisense.Entities[linkedEntityName];

                    string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(linkedEntityData);

                    List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForEntity(linkedEntityData);

                    list.Add(CreateCompletion(entityDescription, linkedEntityData.EntityLogicalName, CrmIntellisenseCommon.CreateEntityDescription(linkedEntityData), _defaultGlyph, compareValues));
                }
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Linked Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillAliases(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, Dictionary<string, string> aliases)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var aliasName in aliases.Keys.OrderBy(s => s))
            {
                list.Add(CreateCompletion(aliasName, aliasName, null, _defaultGlyph, null));
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All Aliases", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillEntityNamesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, bool isObjectTypeCode, bool withNone = false)
        {
            var connectionIntellisense = repository.GetEntitiesIntellisenseData();

            if (connectionIntellisense == null || connectionIntellisense.Entities == null)
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            if (withNone)
            {
                list.Add(CreateCompletion("none - 0", "0", "none", _defaultGlyph, new[] { "none", "0" }));
            }

            var keys = connectionIntellisense.Entities.Keys.ToList();

            foreach (var entityName in keys.OrderBy(s => s))
            {
                var entityData = connectionIntellisense.Entities[entityName];

                string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);

                List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForEntity(entityData);

                var insertionText = entityData.EntityLogicalName;

                if (isObjectTypeCode)
                {
                    insertionText = entityData.ObjectTypeCode.ToString();
                }

                list.Add(CreateCompletion(entityDescription, insertionText, CrmIntellisenseCommon.CreateEntityDescription(entityData), _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillEntityAttributesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode, Dictionary<string, string> aliases)
        {
            var entityName = string.Empty;

            if (string.Equals(currentXmlNode.Name.LocalName, "condition", StringComparison.InvariantCultureIgnoreCase))
            {
                var entityNameInCondition = GetAttributeValue(currentXmlNode, "entityname");

                if (!string.IsNullOrEmpty(entityNameInCondition) && aliases.ContainsKey(entityNameInCondition))
                {
                    entityName = aliases[entityNameInCondition];
                }
            }

            if (string.IsNullOrEmpty(entityName))
            {
                entityName = GetParentEntityName(currentXmlNode);
            }

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityName);

            if (entityData == null
                || entityData.Attributes == null
                )
            {
                return;
            }

            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityData);
        }

        private void FillEntityAttributesInListForGrid(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            int? entityTypeCode = GetParentEntityObjectTypeCode(currentXmlNode);

            if (!entityTypeCode.HasValue)
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityTypeCode.Value);

            if (entityData == null
               || entityData.Attributes == null
               )
            {
                return;
            }

            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityData);
        }

        private void FillEntityIntellisenseDataAttributes(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, EntityIntellisenseData entityData)
        {
            if (entityData == null
                || entityData.Attributes == null
                )
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);

            foreach (var attribute in entityData.AttributesOrdered())
            {
                string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(entityData.EntityLogicalName, attribute);

                List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForAttribute(attribute);

                list.Add(CreateCompletion(attributeDescription, attribute.LogicalName, CrmIntellisenseCommon.CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMonikerAttributes, string.Format("{0} Attributes", entityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillEntityPrimaryAttributeForGrid(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode, bool isNameAttribute)
        {
            int? entityTypeCode = GetParentEntityObjectTypeCode(currentXmlNode);

            if (!entityTypeCode.HasValue)
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityTypeCode.Value);

            if (entityData == null
               || entityData.Attributes == null
               )
            {
                return;
            }

            if (isNameAttribute && string.IsNullOrEmpty(entityData.EntityPrimaryNameAttribute))
            {
                return;
            }

            AttributeIntellisenseData attribute = null;

            if (isNameAttribute)
            {
                if (entityData.Attributes.ContainsKey(entityData.EntityPrimaryNameAttribute))
                {
                    attribute = entityData.Attributes[entityData.EntityPrimaryNameAttribute];
                }
            }
            else
            {
                if (entityData.Attributes.ContainsKey(entityData.EntityPrimaryIdAttribute))
                {
                    attribute = entityData.Attributes[entityData.EntityPrimaryIdAttribute];
                }
            }

            if (attribute == null)
            {
                return;
            }

            string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);

            string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(entityData.EntityLogicalName, attribute);

            List<CrmCompletion> list = new List<CrmCompletion>();

            List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForAttribute(attribute);

            list.Add(CreateCompletion(attributeDescription, attribute.LogicalName, CrmIntellisenseCommon.CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));

            var displayName = string.Format("{0} PrimaryIdAttribute", entityData.EntityLogicalName);

            if (isNameAttribute)
            {
                displayName = string.Format("{0} PrimaryNameAttribute", entityData.EntityLogicalName);
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerAll, displayName, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillEntityAttributeValuesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement nodeCondition, Dictionary<string, string> aliases)
        {
            string entityName = string.Empty;

            var attributeName = GetAttributeValue(nodeCondition, "attribute");

            var entityNameInCondition = GetAttributeValue(nodeCondition, "entityname");

            if (!string.IsNullOrEmpty(entityNameInCondition) && aliases.ContainsKey(entityNameInCondition))
            {
                entityName = aliases[entityNameInCondition];
            }
            else
            {
                entityName = GetParentEntityName(nodeCondition);
            }

            if (!string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(attributeName))
            {
                var entityData = repository.GetEntityAttributeIntellisense(entityName);

                if (entityData != null && entityData.Attributes != null)
                {
                    if (entityData.Attributes.ContainsKey(attributeName))
                    {
                        var attributeData = entityData.Attributes[attributeName];

                        if (attributeData.OptionSet != null && attributeData.OptionSet.IsBoolean)
                        {
                            List<CrmCompletion> list = new List<CrmCompletion>
                            {
                                CreateCompletion("0", "0", null, _defaultGlyph, Enumerable.Empty<string>()),
                                CreateCompletion("1", "1", null, _defaultGlyph, Enumerable.Empty<string>()),
                                CreateCompletion("false", "false", null, _defaultGlyph, Enumerable.Empty<string>()),
                                CreateCompletion("true", "true", null, _defaultGlyph, Enumerable.Empty<string>())
                            };

                            if (attributeData.OptionSet != null && attributeData.OptionSet.OptionSetMetadata is BooleanOptionSetMetadata boolOptionSet)
                            {
                                string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);
                                string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(entityName, attributeData);

                                if (boolOptionSet.FalseOption != null)
                                {
                                    string displayText = CrmIntellisenseCommon.GetDisplayTextOptionSetValue(entityData.EntityLogicalName, attributeData.LogicalName, boolOptionSet.FalseOption);

                                    List<string> compareValues = CrmIntellisenseCommon.GetCompareValues(boolOptionSet.FalseOption.Label);

                                    list.Add(CreateCompletion(displayText, boolOptionSet.FalseOption.Value.ToString(), CrmIntellisenseCommon.CreateOptionValueDescription(entityDescription, attributeDescription, boolOptionSet.FalseOption), _defaultGlyph, compareValues));
                                }

                                if (boolOptionSet.TrueOption != null)
                                {
                                    string displayText = CrmIntellisenseCommon.GetDisplayTextOptionSetValue(entityData.EntityLogicalName, attributeData.LogicalName, boolOptionSet.TrueOption);

                                    List<string> compareValues = CrmIntellisenseCommon.GetCompareValues(boolOptionSet.TrueOption.Label);

                                    list.Add(CreateCompletion(displayText, boolOptionSet.TrueOption.Value.ToString(), CrmIntellisenseCommon.CreateOptionValueDescription(entityDescription, attributeDescription, boolOptionSet.TrueOption), _defaultGlyph, compareValues));
                                }
                            }

                            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0}.{1} Values", entityName, attributeName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                        }
                        else if (attributeData.IsEntityNameAttribute)
                        {
                            FillEntityNamesInList(completionSets, applicableTo, repository, true, true);
                        }
                        else if (attributeData.OptionSet != null)
                        {
                            if (attributeData.OptionSet != null && attributeData.OptionSet.OptionSetMetadata is OptionSetMetadata optionSet)
                            {
                                List<CrmCompletion> list = new List<CrmCompletion>();

                                string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);
                                string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(entityName, attributeData);

                                foreach (var item in optionSet.Options.OrderBy(e => e.Value))
                                {
                                    string displayText = CrmIntellisenseCommon.GetDisplayTextOptionSetValue(entityData.EntityLogicalName, attributeData.LogicalName, item);

                                    List<string> compareValues = CrmIntellisenseCommon.GetCompareValues(item.Label);

                                    list.Add(CreateCompletion(displayText, item.Value.ToString(), CrmIntellisenseCommon.CreateOptionValueDescription(entityDescription, attributeDescription, item), _defaultGlyph, compareValues));
                                }

                                if (list.Count > 0)
                                {
                                    completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0}.{1} Values", entityName, attributeName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetAttributeValue(XElement nodeCondition, string attributeName)
        {
            var attrName = nodeCondition.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, attributeName, StringComparison.InvariantCultureIgnoreCase));

            if (attrName != null)
            {
                if (!string.IsNullOrEmpty(attrName.Value))
                {
                    return attrName.Value;
                }
            }

            return null;
        }

        private HashSet<string> GetUsedEntities(XElement doc)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            {
                var entityElements = doc.DescendantsAndSelf().Where(IsEntityOrLinkElement);

                foreach (var entity in entityElements)
                {
                    var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));

                    if (attrName != null)
                    {
                        if (!string.IsNullOrEmpty(attrName.Value))
                        {
                            result.Add(attrName.Value);
                        }
                    }
                }
            }

            {
                var entityElements = doc.DescendantsAndSelf().Where(IsSubAreaOrPrivilege);

                foreach (var entity in entityElements)
                {
                    var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Entity", StringComparison.InvariantCultureIgnoreCase));

                    if (attrName != null)
                    {
                        if (!string.IsNullOrEmpty(attrName.Value))
                        {
                            result.Add(attrName.Value);
                        }
                    }
                }
            }

            return result;
        }

        private HashSet<int> GetUsedEntityObjectTypeCodes(XElement doc)
        {
            HashSet<int> result = new HashSet<int>();

            var entityElements = doc.DescendantsAndSelf().Where(IsGridElement);

            foreach (var entity in entityElements)
            {
                var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "object", StringComparison.InvariantCultureIgnoreCase));

                if (attrName != null
                    && !string.IsNullOrEmpty(attrName.Value)
                    && int.TryParse(attrName.Value, out int tempInt)
                    )
                {
                    result.Add(tempInt);
                }
            }

            return result;
        }

        private Dictionary<string, string> GetEntityAliases(XElement doc)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            var entityElements = doc.DescendantsAndSelf().Where(IsEntityOrLinkElement);

            foreach (var entity in entityElements)
            {
                var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));
                var attrAlias = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "alias", StringComparison.InvariantCultureIgnoreCase));

                if (attrName != null && attrAlias != null)
                {
                    if (!string.IsNullOrEmpty(attrName.Value) && !string.IsNullOrEmpty(attrAlias.Value))
                    {
                        if (!result.ContainsKey(attrName.Value))
                        {
                            result.Add(attrAlias.Value, attrName.Value);
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsEntityOrLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsSubAreaOrPrivilege(XElement element)
        {
            return string.Equals(element.Name.LocalName, "SubArea", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "Privilege", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsGridElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "grid", StringComparison.OrdinalIgnoreCase);
        }

        private ClassificationSpan GetCurrentXmlAttributeName(ITextSnapshot snapshot, ClassificationSpan containingSpan, IList<ClassificationSpan> spans)
        {
            var currentAttr = spans
                    .Where(s => s.ClassificationType.IsOfType("XML Attribute") && s.Span.Start <= containingSpan.Span.Start)
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

            if (currentAttr != null)
            {
                return currentAttr;
            }

            var allSpans = _classifier.GetClassificationSpans(new SnapshotSpan(containingSpan.Span.Snapshot, 0, containingSpan.Span.Snapshot.Length));

            currentAttr = allSpans
                    .Where(s => s.ClassificationType.IsOfType("XML Name"))
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

            if (currentAttr != null)
            {
                return currentAttr;
            }

            //var containgLine = containingSpan.Span.Start.GetContainingLine();

            //for (int i = containgLine.LineNumber - 1; i >= 0; i--)
            //{
            //    var previousLine = snapshot.GetLineFromLineNumber(i);

            //    SnapshotSpan line = previousLine.Extent;

            //    var previousLineSpans = _classifier.GetClassificationSpans(previousLine.Extent);


            //}

            return null;
        }

        private XElement GetCurrentXmlNode(XElement doc, SnapshotSpan extent)
        {
            IList<ClassificationSpan> spans = _classifier.GetClassificationSpans(new SnapshotSpan(extent.Snapshot, 0, extent.Snapshot.Length));

            var elementSpan = spans
                     .Where(s => s.ClassificationType.IsOfType("XML Name") && s.Span.Start <= extent.Start)
                     .OrderByDescending(s => s.Span.Start.Position)
                     .FirstOrDefault();

            if (elementSpan == null)
            {
                return null;
            }

            var line = elementSpan.Span.Start.Subtract(1).GetContainingLine();

            var lineNumber = line.LineNumber + 1;
            var linePosition = elementSpan.Span.Start.Subtract(1).Position - line.Start.Position + 2;

            var result = doc.DescendantsAndSelf().FirstOrDefault(e => (e as IXmlLineInfo)?.LineNumber == lineNumber && (e as IXmlLineInfo)?.LinePosition == linePosition);

            var list = doc.DescendantsAndSelf().Select(e => new { Element = e, (e as IXmlLineInfo)?.LineNumber, (e as IXmlLineInfo)?.LinePosition }).ToList();

            if (result == null)
            {

            }

            return result;
        }

        private string GetParentEntityName(XElement currentXmlNode)
        {
            var entityElement = currentXmlNode.Ancestors().FirstOrDefault(IsEntityOrLinkEntity);

            if (entityElement != null)
            {
                var attrKey = entityElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));

                if (attrKey != null)
                {
                    return attrKey.Value;
                }
            }

            return null;
        }

        private int? GetParentEntityObjectTypeCode(XElement currentXmlNode)
        {
            var entityElement = currentXmlNode.AncestorsAndSelf().FirstOrDefault(IsGridElement);

            if (entityElement != null)
            {
                var attrObject = entityElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "object", StringComparison.InvariantCultureIgnoreCase));

                if (attrObject != null
                    && !string.IsNullOrEmpty(attrObject.Value)
                    && int.TryParse(attrObject.Value, out int tempInt)
                    )
                {
                    return tempInt;
                }
            }

            return null;
        }

        private static bool IsEntityOrLinkEntity(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.InvariantCultureIgnoreCase);
        }

        private ITrackingSpan FindTokenSpanAtPosition(ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = _navigator.GetTextStructureNavigator(_buffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }

        private static XElement ReadXmlDocument(string text)
        {
            try
            {
                //XDocument doc = XDocument.Parse(Regex.Replace(text, " xmlns(:[^\"]+)?=\"([^\"]+)\"", string.Empty));
                XDocument doc = XDocument.Parse(text, LoadOptions.SetLineInfo);

                return doc.Root;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private CrmCompletion CreateCompletion(string displayText, string insertionText, string description, ImageSource glyph, IEnumerable<string> compareValues)
        {
            if (glyph == null)
                glyph = _defaultGlyph;

            return new CrmCompletion(displayText, insertionText, description, glyph, null, compareValues);
        }

        #region IDisposable Support

        private bool _isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {

                }

                _isDisposed = true;
            }
        }

        // ~FetchXmlCompletionSource() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}