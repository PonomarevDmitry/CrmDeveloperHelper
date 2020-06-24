using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public partial class XmlGoToDefinitionCommandHandler : IOleCommandTarget
    {
        private IOleCommandTarget _nextCommandHandler;
        private ITextView _textView;
        private XmlGoToDefinitionCreationListener _provider;

        private IClassifier _classifier;

        public XmlGoToDefinitionCommandHandler(IVsTextView textViewAdapter, ITextView textView, XmlGoToDefinitionCreationListener provider)
        {
            this._textView = textView;
            this._provider = provider;

            var hresult = textViewAdapter.AddCommandFilter(this, out _nextCommandHandler);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
            {
                if (cCmds == 1)
                {
                    switch (prgCmds[0].cmdID)
                    {
                        case (uint)VSConstants.VSStd97CmdID.GotoDefn: // F12
                            prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED;
                            prgCmds[0].cmdf |= (uint)OLECMDF.OLECMDF_ENABLED;
                            return VSConstants.S_OK;
                    }
                }
            }

            ThreadHelper.ThrowIfNotOnUIThread();

            return _nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (VsShellUtilities.IsInAutomationFunction(_provider.ServiceProvider))
            {
                return _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            if (pguidCmdGroup != VSConstants.GUID_VSStandardCommandSet97 || nCmdID != (uint)VSConstants.VSStd97CmdID.GotoDefn)
            {
                return _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            if (!TryGoToDefinition())
            {
                return _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            return VSConstants.S_OK;
        }

        private bool TryGoToDefinition()
        {
            ITextBuffer buffer = _textView.TextBuffer;

            _classifier = _provider.ClassifierAggregatorService.GetClassifier(buffer);

            ITextSnapshot snapshot = buffer.CurrentSnapshot;

            XElement doc = ReadXmlDocument(snapshot.GetText());

            if (doc == null)
            {
                return false;
            }

            string docRootName = doc.Name.ToString();

            if (!string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootSavedQuery, StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return false;
            }

            SnapshotPoint currentPoint = _textView.Caret.Position.BufferPosition;

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(snapshot, 0, snapshot.Length));

            var firstSpans = spans
                .Where(s => s.Span.Start < currentPoint.Position)
                .OrderByDescending(s => s.Span.Start.Position)
                .ToList();

            var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            var lastSpans = spans
                 .Where(s => s.Span.Start >= currentPoint.Position)
                 .OrderBy(s => s.Span.Start.Position)
                 .ToList();

            var lastDelimiter = lastSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            SnapshotSpan? extentTemp = null;

            if (firstDelimiter != null && firstDelimiter.Span.GetText() == "\"\"" && firstDelimiter.Span.Contains(currentPoint))
            {
                extentTemp = new SnapshotSpan(firstDelimiter.Span.Start.Add(1), firstDelimiter.Span.Start.Add(1));
            }
            else if (firstDelimiter != null && lastDelimiter != null && firstDelimiter.Span.GetText() == "\"" && lastDelimiter.Span.GetText() == "\"")
            {
                extentTemp = new SnapshotSpan(firstDelimiter.Span.End, lastDelimiter.Span.Start);
            }

            if (!extentTemp.HasValue)
            {
                return false;
            }

            var extent = extentTemp.Value;

            var currentValue = extent.GetText();

            var currentXmlNode = GetCurrentXmlNode(doc, extent);

            if (currentXmlNode == null)
            {
                return false;
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
                return false;
            }

            ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

            if (currentAttr == null)
            {
                return false;
            }

            string currentNodeName = currentXmlNode.Name.LocalName;
            string currentAttributeName = currentAttr.Span.GetText();

            if (string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                if (TryGoToDefinitionInRibbon(snapshot, doc, currentXmlNode, currentNodeName, currentAttributeName, currentValue))
                {
                    return true;
                }
            }
            else if (string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(currentNodeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (TryOpenWebResource(currentValue))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(currentNodeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (TryOpenWebResource(currentValue))
                        {
                            return true;
                        }
                    }
                }
                else if (string.Equals(currentNodeName, "Handler", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "libraryName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (TryOpenWebResource(currentValue))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootSavedQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                if (string.Equals(currentNodeName, "cell", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "imageproviderwebresource", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (TryOpenWebResourceWithPrefix(currentValue))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (string.Equals(docRootName, Commands.AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(currentAttributeName, "Url", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentNodeName, "SiteMap", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "Area", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "Group", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "SubArea", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        if (TryOpenWebResourceWithPrefix(currentValue))
                        {
                            return true;
                        }
                    }
                }

                if (string.Equals(currentAttributeName, "Icon", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentNodeName, "Area", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "Group", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "SubArea", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        if (TryOpenWebResourceInWebWithPrefix(currentValue))
                        {
                            return true;
                        }
                    }
                }

                if (string.Equals(currentAttributeName, "OutlookShortcutIcon", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentNodeName, "SubArea", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (TryOpenWebResourceInWebWithPrefix(currentValue))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool TryOpenWebResourceWithPrefix(string currentValue)
        {
            const string webResourcePrefix = "$webresource:";

            if (!string.IsNullOrEmpty(currentValue)
                && currentValue.StartsWith(webResourcePrefix, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                currentValue = currentValue.Substring(webResourcePrefix.Length);

                return TryOpenWebResource(currentValue);
            }

            return false;
        }

        private static bool TryOpenWebResource(string webResourceName)
        {
            var connectionConfig = CrmDeveloperHelper.Model.ConnectionConfiguration.Get();

            if (connectionConfig == null || connectionConfig.CurrentConnectionData == null)
            {
                return false;
            }

            var connectionData = connectionConfig.CurrentConnectionData;

            var repositoryWebResource = Repository.WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

            var webResourcesList = repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList();

            if (webResourcesList == null)
            {
                return false;
            }

            var webResource = webResourcesList.FirstOrDefault(w => string.Equals(w.Name, webResourceName, StringComparison.InvariantCultureIgnoreCase));

            if (webResource == null || !webResource.WebResourceId.HasValue)
            {
                return false;
            }

            if (connectionData.TryGetFriendlyPathByGuid(webResource.WebResourceId.Value, out string friendlyPath))
            {
                Helpers.DTEHelper.Singleton.OpenFileInVisualStudioRelativePath(connectionData, friendlyPath, out bool success);

                if (success)
                {
                    return true;
                }
            }

            connectionData.OpenEntityInstanceInWeb(Entities.WebResource.EntityLogicalName, webResource.WebResourceId.Value);

            return true;
        }

        private static bool TryOpenWebResourceInWebWithPrefix(string currentValue)
        {
            const string webResourcePrefix = "$webresource:";

            if (!string.IsNullOrEmpty(currentValue)
                && currentValue.StartsWith(webResourcePrefix, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                currentValue = currentValue.Substring(webResourcePrefix.Length);

                return TryOpenWebResourceInWeb(currentValue);
            }

            return false;
        }

        private static bool TryOpenWebResourceInWeb(string webResourceName)
        {
            var connectionConfig = CrmDeveloperHelper.Model.ConnectionConfiguration.Get();

            if (connectionConfig == null || connectionConfig.CurrentConnectionData == null)
            {
                return false;
            }

            var connectionData = connectionConfig.CurrentConnectionData;

            var repositoryWebResource = Repository.WebResourceIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

            var webResourcesList = repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesAll?.Values?.ToList();

            if (webResourcesList == null)
            {
                return false;
            }

            var webResource = webResourcesList.FirstOrDefault(w => string.Equals(w.Name, webResourceName, StringComparison.InvariantCultureIgnoreCase));

            if (webResource == null || !webResource.WebResourceId.HasValue)
            {
                return false;
            }

            connectionData.OpenEntityInstanceInWeb(Entities.WebResource.EntityLogicalName, webResource.WebResourceId.Value);

            return true;
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

            var result = doc.DescendantsAndSelf().FirstOrDefault(e => (e as IXmlLineInfo)?.LineNumber == lineNumber);

            return result;
        }

        private static XElement ReadXmlDocument(string text)
        {
            try
            {
                XDocument doc = XDocument.Parse(text, LoadOptions.SetLineInfo);

                return doc.Root;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}