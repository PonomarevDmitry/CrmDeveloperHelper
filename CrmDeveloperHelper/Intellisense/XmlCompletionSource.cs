using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
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
    public sealed partial class XmlCompletionSource : ICompletionSource, IDisposable
    {
        private const string SourceNameMonikerDefaultSingle = "CrmXmlDefaultSingle.{8A22EFE3-4F6D-47A8-A511-82DE08260BA7}";

        private const string SourceNameMonikerAllEntites = "CrmXmlAllEntites.{5EF611DA-AA3C-44B3-A517-04543E92139C}";
        private const string SourceNameMonikerLinkedEntites = "CrmXmlLinkedEntites.{981A8195-06A2-460F-A14F-FFAAC33119EB}";

        private const string SourceNameMonikerWebResourcesText = "CrmXmlWebResourcesText.{E438F5CE-FBBD-4754-A2F7-F1AEB6752499}";
        private const string SourceNameMonikerWebResourcesIcon = "CrmXmlWebResourcesIcon.{AA2AABF2-E32A-40AA-B6EF-A072D45CD8FC}";

        private const string SourceNameMonikerAllAttributes = "CrmXmlAllAttributes.{417B4B2F-EC8A-4EBC-A6CD-A45013171817}";
        private const string SourceNameMonikerPrimaryAttributes = "CrmXmlPrimaryAttributes.{A9DA848E-6160-48AE-A41A-6CC15D30DB90}";
        private const string SourceNameMonikerReferenceAttributes = "CrmXmlReferenceAttributes.{9FEBA56B-4CFC-40E4-9C8C-E7699BEEB367}";

        private const string SourceNameMonikerRibbonLocLables = "CrmXmlRibbonLocLables.{4B14E1D7-8F16-4D7C-A8F2-5C11E1F876FB}";

        private const string SourceNameMonikerRibbonCommands = "CrmXmlRibbonCommands.{F13A4D64-86AD-466D-B273-3CD8E9E741F7}";
        private const string SourceNameMonikerRibbonEnableRules = "CrmXmlRibbonEnableRules.{DD111F19-F2D0-4FAC-83F5-15DAD841A5E0}";
        private const string SourceNameMonikerRibbonDisplayRules = "CrmXmlRibbonDisplayRules.{F4EA811F-FE61-4E1D-84D8-1D0156C96D99}";

        private const string SourceNameMonikerRibbonLocations = "CrmXmlRibbonLocations.{8FEA085A-B8DA-4037-A01E-E9CC0F89A943}";
        private const string SourceNameMonikerRibbonSequences = "CrmXmlRibbonSequences.{0CD106F0-27BD-471B-9057-7873CF4E2E7A}";

        private readonly XmlCompletionSourceProvider _sourceProvider;

        private ITextBuffer _buffer;
        private IClassifier _classifier;
        private ITextStructureNavigatorSelectorService _navigator;

        private readonly ImageSource _defaultGlyph;
        private readonly ImageSource _builtInGlyph;
        private readonly IGlyphService _glyphService;

        public XmlCompletionSource(XmlCompletionSourceProvider sourceProvider, ITextBuffer buffer, IClassifierAggregatorService classifier, ITextStructureNavigatorSelectorService navigator, IGlyphService glyphService)
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

            if (string.Equals(doc.Name.ToString(), "fetch", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForFetchXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.ToString(), "grid", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForGridXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.ToString(), "savedquery", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForFetchXml(session, completionSets, snapshot, doc, repository);

                FillSessionForGridXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.ToString(), "SiteMap", StringComparison.InvariantCultureIgnoreCase))
            {
                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                var repositorySiteMap = SiteMapIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForSiteMap(triggerPoint.Value, session, completionSets, snapshot, doc, repositoryEntities, repositorySiteMap, repositoryWebResource);
            }
            else if (string.Equals(doc.Name.ToString(), "RibbonDiffXml", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(doc.Name.ToString(), "RibbonDefinitions", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                var repositoryEntities = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);
                var repositoryWebResource = WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                var repositoryRibbon = RibbonIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForRibbonDiffXml(triggerPoint.Value, session, completionSets, snapshot, doc, repositoryEntities, repositoryWebResource, repositoryRibbon);
            }
        }

        private void FillWebResourcesText(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<WebResource> webResources, string name)
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

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerWebResourcesText, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
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

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerWebResourcesIcon, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
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