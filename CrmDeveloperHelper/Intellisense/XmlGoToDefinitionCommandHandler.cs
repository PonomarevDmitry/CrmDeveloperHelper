using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public class XmlGoToDefinitionCommandHandler : IOleCommandTarget
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

            return _nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
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

            if (!string.Equals(doc.Name.ToString(), "RibbonDiffXml", StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(doc.Name.ToString(), "RibbonDefinitions", StringComparison.InvariantCultureIgnoreCase)
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

            if (XmlCompletionSource.LabelXmlAttributes.Contains(currentAttributeName))
            {
                bool isTitleElement = string.Equals(currentNodeName, "Title", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent != null
                    && string.Equals(currentXmlNode.Parent.Name.LocalName, "Titles", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent.Parent != null
                    && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "LocLabel", StringComparison.InvariantCultureIgnoreCase);

                if (!isTitleElement)
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        if (currentValue.StartsWith("$LocLabels:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var labelId = currentValue.Substring(11);

                            var elements = doc.XPathSelectElements("./LocLabels/LocLabel").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, labelId, StringComparison.InvariantCultureIgnoreCase)).ToList();

                            if (elements.Count == 1)
                            {
                                var xmlElement = elements[0];

                                if (TryMoveToElement(snapshot, xmlElement))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        var elements = doc.XPathSelectElements("./LocLabels").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            if (XmlCompletionSource.CommandXmlAttributes.Contains(currentAttributeName))
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    {
                        var elements = doc.XPathSelectElements("./CommandDefinitions/CommandDefinition").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }

                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/CommandDefinitions/CommandDefinition").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    {
                        var elements = doc.XPathSelectElements("./CommandDefinitions").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }

                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/CommandDefinitions").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            if (string.Equals(currentNodeName, "EnableRule", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(currentAttributeName, "Id", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent != null
                    && string.Equals(currentXmlNode.Parent.Name.LocalName, "EnableRules", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent.Parent != null
                    && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "CommandDefinition", StringComparison.InvariantCultureIgnoreCase)
                    )
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    {
                        var elements = doc.XPathSelectElements("./RuleDefinitions/EnableRules/EnableRule").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }

                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/RuleDefinitions/EnableRules/EnableRule").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    {
                        var elements = doc.XPathSelectElements("./RuleDefinitions/EnableRules").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }

                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/RuleDefinitions/EnableRules").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            if (string.Equals(currentNodeName, "DisplayRule", StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(currentAttributeName, "Id", StringComparison.InvariantCultureIgnoreCase)
                && currentXmlNode.Parent != null
                && string.Equals(currentXmlNode.Parent.Name.LocalName, "DisplayRules", StringComparison.InvariantCultureIgnoreCase)
                && currentXmlNode.Parent.Parent != null
                && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "CommandDefinition", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    {
                        var elements = doc.XPathSelectElements("./RuleDefinitions/DisplayRules/DisplayRule").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/RuleDefinitions/DisplayRules/DisplayRule").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    {
                        var elements = doc.XPathSelectElements("./RuleDefinitions/DisplayRules").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }

                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/RuleDefinitions/DisplayRules").ToList();

                        if (elements.Count == 1)
                        {
                            var xmlElement = elements[0];

                            if (TryMoveToElement(snapshot, xmlElement))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool TryMoveToElement(ITextSnapshot snapshot, XElement xmlElement)
        {
            var lineNumber = (xmlElement as IXmlLineInfo)?.LineNumber;

            if (lineNumber.HasValue)
            {
                var line = snapshot.GetLineFromLineNumber(lineNumber.Value - 1);

                if (line != null)
                {
                    var point = line.Start;

                    _textView.ViewScroller.EnsureSpanVisible(new SnapshotSpan(point, 0), EnsureSpanVisibleOptions.ShowStart | EnsureSpanVisibleOptions.AlwaysCenter);

                    var lineText = line.GetText();

                    var index = lineText.IndexOf(xmlElement.Name.LocalName);

                    if (index != -1)
                    {
                        point += index;
                    }

                    _textView.Selection.Select(new SnapshotSpan(point, 0), false);
                    _textView.Selection.IsActive = false;

                    _textView.Caret.MoveTo(point);

                    _textView.Caret.EnsureVisible();

                    return true;
                }
            }

            return false;
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
