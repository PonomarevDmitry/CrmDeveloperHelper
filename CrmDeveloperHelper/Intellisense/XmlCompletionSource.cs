using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Concurrent;
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
    public sealed partial class XmlCompletionSource : ICompletionSource, IDisposable
    {
        private const string SourceNameMonikerDefaultSingle = "CrmXmlDefaultSingle.{8A22EFE3-4F6D-47A8-A511-82DE08260BA7}";

        private const string SourceNameMonikerAllEntites = "CrmXmlAllEntites.{5EF611DA-AA3C-44B3-A517-04543E92139C}";
        private const string SourceNameMonikerLinkedEntites = "CrmXmlLinkedEntites.{981A8195-06A2-460F-A14F-FFAAC33119EB}";

        private const string SourceNameMonikerWebResourcesText = "CrmXmlWebResourcesText.{E438F5CE-FBBD-4754-A2F7-F1AEB6752499}";
        private const string SourceNameMonikerWebResourcesIcon = "CrmXmlWebResourcesIcon.{AA2AABF2-E32A-40AA-B6EF-A072D45CD8FC}";

        private const string SourceNameMonikerNewGuid = "CrmXmlNewGuid.{A6CE3966-8EC6-4938-B123-80D7F7F83453}";

        private const string SourceNameMonikerAllAttributes = "CrmXmlAllAttributes.{417B4B2F-EC8A-4EBC-A6CD-A45013171817}";
        private const string SourceNameMonikerPrimaryAttributes = "CrmXmlPrimaryAttributes.{A9DA848E-6160-48AE-A41A-6CC15D30DB90}";
        private const string SourceNameMonikerReferenceAttributes = "CrmXmlReferenceAttributes.{9FEBA56B-4CFC-40E4-9C8C-E7699BEEB367}";

        private const string SourceNameMonikerRibbonLocLables = "CrmXmlRibbonLocLables.{4B14E1D7-8F16-4D7C-A8F2-5C11E1F876FB}";

        private const string SourceNameMonikerRibbonCommands = "CrmXmlRibbonCommands.{F13A4D64-86AD-466D-B273-3CD8E9E741F7}";
        private const string SourceNameMonikerRibbonEnableRules = "CrmXmlRibbonEnableRules.{DD111F19-F2D0-4FAC-83F5-15DAD841A5E0}";
        private const string SourceNameMonikerRibbonDisplayRules = "CrmXmlRibbonDisplayRules.{F4EA811F-FE61-4E1D-84D8-1D0156C96D99}";

        private const string SourceNameMonikerRibbonLocations = "CrmXmlRibbonLocations.{8FEA085A-B8DA-4037-A01E-E9CC0F89A943}";
        private const string SourceNameMonikerRibbonSequences = "CrmXmlRibbonSequences.{0CD106F0-27BD-471B-9057-7873CF4E2E7A}";

        private const string xmlClassificationDelimiter = "XML Delimiter";
        private const string xmlClassificationAttributeQuotes = "XML Attribute Quotes";
        private const string xmlClassificationAttributeValue = "XML Attribute Value";

        private const string xmlClassificationXmlName = "XML Name";

        private readonly XmlCompletionSourceProvider _sourceProvider;

        private ITextBuffer _buffer;
        private IClassifier _classifier;
        private ITextStructureNavigatorSelectorService _navigator;

        private readonly ImageSource _defaultGlyph;
        private readonly ImageSource _builtInGlyph;
        private readonly IGlyphService _glyphService;

        private static ConcurrentDictionary<string, ImageSource> _cachedImages = new ConcurrentDictionary<string, ImageSource>();

        public XmlCompletionSource(
            XmlCompletionSourceProvider sourceProvider
            , ITextBuffer buffer
            , IClassifierAggregatorService classifier
            , ITextStructureNavigatorSelectorService navigator
            , IGlyphService glyphService
        )
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

            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(snapshot, 0, snapshot.Length));

            var xmlRootSpan = spans.Where((s, i) =>
                i > 0
                && s.ClassificationType.IsOfType(xmlClassificationXmlName)
                && spans[i - 1].ClassificationType.IsOfType(xmlClassificationDelimiter)
                && spans[i - 1].Span.GetText() == "<"
            ).FirstOrDefault();

            if (xmlRootSpan == null)
            {
                return;
            }

            string xmlRootName = xmlRootSpan.Span.GetText();

            if (!string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootSavedQuery, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase)

            //  && !string.Equals(xmlRootName, Commands.AbstractDynamicCommandXsdSchemas., StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            var firstSpans = spans
                .Where(s => s.Span.Start <= currentPoint.Position)
                .OrderByDescending(s => s.Span.Start.Position)
                .ToList();

            var lastSpans = spans
                .Where(s => s.Span.Start > currentPoint.Position)
                .OrderBy(s => s.Span.Start.Position)
                .ToList();

            SnapshotSpan? extentTemp = null;

            if (!extentTemp.HasValue)
            {
                var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType(xmlClassificationAttributeQuotes));

                if (firstDelimiter != null)
                {
                    string firstDelimiterText = firstDelimiter.Span.GetText();

                    if (firstDelimiterText == "\"\"" || firstDelimiterText == "''")
                    {
                        extentTemp = new SnapshotSpan(firstDelimiter.Span.Start.Add(1), firstDelimiter.Span.Start.Add(1));
                    }
                }
            }

            if (!extentTemp.HasValue)
            {
                var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType(xmlClassificationAttributeQuotes));
                var lastDelimiter = lastSpans.FirstOrDefault(s => s.ClassificationType.IsOfType(xmlClassificationAttributeQuotes));

                if (firstDelimiter != null && lastDelimiter != null)
                {
                    string firstDelimiterText = firstDelimiter.Span.GetText();
                    string lastDelimiterText = lastDelimiter.Span.GetText();

                    if ((firstDelimiterText == "\"" && lastDelimiterText == "\"")
                        || (firstDelimiterText == "'" && lastDelimiterText == "'")
                    )
                    {
                        extentTemp = new SnapshotSpan(firstDelimiter.Span.End, lastDelimiter.Span.Start);
                    }
                }
            }

            if (extentTemp.HasValue)
            {
                var extent = extentTemp.Value;

                {
                    var extentText = extent.GetText();

                    if (extentText == ",\"" || extentText == ",'")
                    {
                        extent = new SnapshotSpan(extent.Snapshot, extent.Start, extent.Length - 1);
                    }
                }

                XElement doc = ReadXmlDocument(snapshot.GetText());

                if (doc == null)
                {
                    return;
                }

                var currentXmlNode = GetCurrentXmlNode(doc, extent);

                if (currentXmlNode != null)
                {
                    var containingAttributeSpans = spans
                        .Where(s => s.Span.Contains(extent.Start)
                            && s.Span.Contains(extent)
                            && s.ClassificationType.IsOfType(xmlClassificationAttributeValue)
                        )
                        .OrderByDescending(s => s.Span.Start.Position)
                        .ToList();

                    var containingAttributeValue = containingAttributeSpans.FirstOrDefault();

                    if (containingAttributeValue == null)
                    {
                        containingAttributeValue = spans
                            .Where(s => s.Span.Contains(extent.Start)
                                && s.Span.Contains(extent)
                                && s.ClassificationType.IsOfType(xmlClassificationAttributeQuotes)
                                && (s.Span.GetText() == "\"\"" || s.Span.GetText() == "''")
                            )
                            .OrderByDescending(s => s.Span.Start.Position)
                            .FirstOrDefault();
                    }

                    if (containingAttributeValue != null)
                    {
                        ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

                        if (currentAttr != null)
                        {
                            string currentNodeName = currentXmlNode.Name.LocalName;

                            string currentAttributeName = currentAttr.Span.GetText();

                            ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

                            if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                HashSet<string> usedEntities = GetUsedEntities(doc);

                                if (usedEntities.Any())
                                {
                                    repository.GetEntityDataForNamesAsync(usedEntities);
                                }

                                FillSessionForFetchXmlCompletionSet(completionSets, doc, connectionConfig.CurrentConnectionData, repository, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);
                            }
                            else if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                HashSet<int> usedEntityCodes = GetUsedEntityObjectTypeCodes(doc);

                                if (usedEntityCodes.Any())
                                {
                                    repositoryEntities.GetEntityDataForObjectTypeCodesAsync(usedEntityCodes);
                                }

                                FillSessionForGridXmlCompletionSet(completionSets, connectionConfig.CurrentConnectionData, repositoryEntities, repositoryWebResource, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);
                            }
                            else if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootSavedQuery, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                HashSet<string> usedEntities = GetUsedEntities(doc);

                                if (usedEntities.Any())
                                {
                                    repositoryEntities.GetEntityDataForNamesAsync(usedEntities);
                                }

                                HashSet<int> usedEntityCodes = GetUsedEntityObjectTypeCodes(doc);

                                if (usedEntityCodes.Any())
                                {
                                    repositoryEntities.GetEntityDataForObjectTypeCodesAsync(usedEntityCodes);
                                }

                                FillSessionForFetchXmlCompletionSet(completionSets, doc, connectionConfig.CurrentConnectionData, repositoryEntities, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);

                                FillSessionForGridXmlCompletionSet(completionSets, connectionConfig.CurrentConnectionData, repositoryEntities, repositoryWebResource, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);
                            }
                            else if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                                var repositorySiteMap = SiteMapIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                HashSet<string> usedEntities = GetUsedEntities(doc);

                                if (usedEntities.Any())
                                {
                                    repositoryEntities.GetEntityDataForNamesAsync(usedEntities);
                                }

                                FillSessionForSiteMapCompletionSet(completionSets, snapshot, connectionConfig.CurrentConnectionData, repositoryEntities, repositorySiteMap, repositoryWebResource, extent, currentNodeName, currentAttributeName, applicableTo);
                            }
                            else if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase)
                                || string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase)
                            )
                            {
                                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                var repositoryRibbon = RibbonIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                FillSessionForRibbonDiffXmlCompletionSet(completionSets, doc, connectionConfig.CurrentConnectionData, repositoryEntities, repositoryWebResource, repositoryRibbon, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);
                            }
                            else if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                FillSessionForWebResourceDependencyXmlCompletionSet(completionSets, doc, connectionConfig.CurrentConnectionData, repositoryEntities, repositoryWebResource, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);
                            }
                            else if (string.Equals(doc.Name.LocalName, Commands.AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                                FillSessionForFormXmlCompletionSet(completionSets, doc, connectionConfig.CurrentConnectionData, repositoryWebResource, currentXmlNode, currentNodeName, currentAttributeName, applicableTo);
                            }
                        }
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

                if (extentTemp.HasValue)
                {
                    var extent = extentTemp.Value;

                    XElement doc = ReadXmlDocument(snapshot.GetText());

                    if (doc == null)
                    {
                        return;
                    }

                    var currentXmlNode = GetCurrentXmlNode(doc, extent);

                    if (currentXmlNode != null)
                    {
                        ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);

                        if (string.Equals(doc.Name.ToString(), Commands.AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(doc.Name.ToString(), Commands.AbstractDynamicCommandXsdSchemas.RootSavedQuery, StringComparison.InvariantCultureIgnoreCase)
                        )
                        {
                            var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                            HashSet<string> usedEntities = GetUsedEntities(doc);

                            if (usedEntities.Any())
                            {
                                repository.GetEntityDataForNamesAsync(usedEntities);
                            }

                            if (string.Equals(currentXmlNode.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var nodeCondition = currentXmlNode.Parent;

                                if (nodeCondition != null && string.Equals(nodeCondition.Name.LocalName, "condition", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Dictionary<string, string> aliases = GetEntityAliases(doc);

                                    FillEntityAttributeValuesInList(completionSets, applicableTo, repository, nodeCondition, aliases);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FillWebResourcesTextWithWebResourcePrefix(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<WebResourceIntellisenseData> webResources, string nameCompletionSet)
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

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerWebResourcesText, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillWebResourcesIcons(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<WebResourceIntellisenseData> webResources, string nameCompletionSet)
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
                    if (_cachedImages.ContainsKey(resource.Content))
                    {
                        _cachedImages.TryGetValue(resource.Content, out image);

                        if (image != null)
                        {
                            str.AppendFormat(" - {0}x{1}", Convert.ToInt32(image.Width), Convert.ToInt32(image.Height));
                        }
                        else
                        {
                            image = _defaultGlyph;
                        }
                    }

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
                                _cachedImages.TryAdd(resource.Content, image);

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

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerWebResourcesIcon, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
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
                completionSets.Add(new CrmCompletionSet(SourceNameMonikerDefaultSingle, "All LCID", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private static HashSet<string> GetUsedEntities(XElement doc)
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

        private void FillWebResourcesNames(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<WebResourceIntellisenseData> webResources, string nameCompletionSet)
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

                list.Add(CreateCompletion(str.ToString(), resource.Name, resource.Description, _defaultGlyph, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerWebResourcesText, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private static readonly string[] _guidFormats = new[] { "B", "D", "N", "P", "X" };

        private void FillNewGuid(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            Guid newGuid = Guid.NewGuid();

            foreach (var format in _guidFormats)
            {
                list.Add(CreateCompletion($"New Guid Format {format}", newGuid.ToString(format), string.Empty, _defaultGlyph, new[] { format }));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerNewGuid, "New Guid", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
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