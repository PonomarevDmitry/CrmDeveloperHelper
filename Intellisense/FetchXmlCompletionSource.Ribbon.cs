using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public sealed partial class FetchXmlCompletionSource
    {
        private static HashSet<string> _controlsWithImagesXmlElements = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Button"
            , "CheckBox"
            , "ComboBox"
            , "Controls"
            , "DropDown"
            , "FlyoutAnchor"
            , "GalleryButton"
            , "Group"
            , "Label"
            , "MRUSplitButton"
            , "QAT"
            , "Spinner"
            , "SplitButton"
            , "TextBox"
            , "ToggleButton"
            //, "Button"
            //, "Button"
            //, "Button"
        };

        private static HashSet<string> _imagesXmlAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Image16by16"
            , "Image32by32"
            , "ToolTipImage32by32"
            , "Image"
            , "Image32by32Popup"

            , "ImageUpArrow"
            , "ImageSideArrow"
            , "ImageDownArrow"

            , "ImageHover"
            , "ImageDown"
            , "ImageLeftSide"
            , "ImageLeftSideHover"
            , "ImageLeftSideDown"
            , "ImageRightSide"
            , "ImageRightSideHover"
            , "ImageRightSideDown"

            , "Image32by32GroupPopupDefault"
            , "ToolTipFooterImage16by16"
            , "ToolTipDisabledCommandImage16by16"
        };

        private static HashSet<string> _labelXmlAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "LabelText"
            , "Alt"
            , "Description"
            , "ToolTipTitle"
            , "ToolTipDescription"
            , "Title"

            , "ToolTipSelectedItemTitle"

            , "MenuSectionInitialTitle"
            , "MenuSectionTitle"

            , "ToolTipFooterText"
            , "ToolTipDisabledCommandDescription"
            , "ToolTipDisabledCommandTitle"
            , "ToolTipSelectedItemTitlePrefix"
            , "ATContextualTabText"
            , "ATTabPositionText"
            , "NavigationHelpText"

            , "AltArrow"
            , "MenuAlt"
            , "AltDownArrow"
            , "AltUpArrow"
            , "LayoutTitle"
        };

        private static HashSet<string> _commandXmlAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Command"
            , "QueryCommand"
            , "CommandPreview"
            , "CommandRevert"

            , "CommandMenuOpen"
            , "CommandMenuClose"
            , "CommandPreview"
            , "CommandPreviewRevert"
            , "PopulateQueryCommand"

            , "MenuCommand"
            , "RootEventCommand"
            , "TabSwitchCommand"
            , "ScaleCommand"
        };

        private void FillSessionForRibbonDiffXml(
            SnapshotPoint triggerPoint
            , ICompletionSession session
            , IList<CompletionSet> completionSets
            , ITextSnapshot snapshot
            , XElement doc
            , ConnectionIntellisenseDataRepository repositoryEntities
            , WebResourceIntellisenseDataRepository repositoryWebResource
            )
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(snapshot, 0, snapshot.Length));

            var firstSpans = spans
                .Where(s => s.Span.Start <= currentPoint.Position)
                .OrderByDescending(s => s.Span.Start.Position)
                .ToList();

            var firstDelimiter = firstSpans.FirstOrDefault(s => s.ClassificationType.IsOfType("XML Attribute Quotes"));

            var lastSpans = spans
                .Where(s => s.Span.Start > currentPoint.Position)
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
                if (_controlsWithImagesXmlElements.Contains(currentNodeName) && _imagesXmlAttributes.Contains(currentAttributeName))
                {
                    FillWebResourcesIcons(completionSets, applicableTo, repositoryWebResource.GetWebResourceIntellisenseData()?.WebResourcesIcon?.Values?.ToList(), "WebResources");
                }

                if (_labelXmlAttributes.Contains(currentAttributeName))
                {
                    FillLocLables(completionSets, applicableTo, doc, "LocLabels");
                }

                if (_commandXmlAttributes.Contains(currentAttributeName))
                {
                    FillCommandsLocal(completionSets, applicableTo, doc, "Commands in RibbonDiffXml");
                }

                if (string.Equals(currentNodeName, "EnableRule", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(currentAttributeName, "Id", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent != null
                    && string.Equals(currentXmlNode.Parent.Name.LocalName, "EnableRules", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent.Parent != null
                    && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "CommandDefinition", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    FillEnableRulesLocal(completionSets, applicableTo, doc, "EnableRules in RibbonDiffXml");
                }

                if (string.Equals(currentNodeName, "DisplayRule", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(currentAttributeName, "Id", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent != null
                    && string.Equals(currentXmlNode.Parent.Name.LocalName, "DisplayRules", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent.Parent != null
                    && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "CommandDefinition", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    FillDisplayRulesLocal(completionSets, applicableTo, doc, "DisplayRules in RibbonDiffXml");
                }

                if (string.Equals(currentAttributeName, "EntityName", StringComparison.InvariantCultureIgnoreCase))
                {
                    FillEntityNamesInList(completionSets, applicableTo, repositoryEntities, false, false);
                }

                if (string.Equals(currentNodeName, "CustomRule", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillWebResourcesText(completionSets, applicableTo, repositoryWebResource.GetWebResourceIntellisenseData()?.WebResourcesJavaScript?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "FunctionName", StringComparison.InvariantCultureIgnoreCase))
                    {

                    }
                }
                else if (string.Equals(currentNodeName, "JavaScriptFunction", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillWebResourcesText(completionSets, applicableTo, repositoryWebResource.GetWebResourceIntellisenseData()?.WebResourcesJavaScript?.Values?.ToList(), "WebResources");
                    }
                    else if (string.Equals(currentAttributeName, "FunctionName", StringComparison.InvariantCultureIgnoreCase))
                    {

                    }
                }
                else if (string.Equals(currentNodeName, "Title", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "languagecode", StringComparison.InvariantCultureIgnoreCase))
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

        private void FillDisplayRulesLocal(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string name)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./RuleDefinitions/DisplayRules/DisplayRule").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                list.Add(CreateCompletion(id, id, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonDisplayRules, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillEnableRulesLocal(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string name)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./RuleDefinitions/EnableRules/EnableRule").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                list.Add(CreateCompletion(id, id, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonEnableRules, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillCommandsLocal(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string name)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./CommandDefinitions/CommandDefinition").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                list.Add(CreateCompletion(id, id, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonCommands, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private void FillLocLables(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string name)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./LocLabels/LocLabel").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                List<string> compareValues = new List<string>() { id };

                var titlesList = label.XPathSelectElements("./Titles/Title").Where(e =>
                    e.Attribute("languagecode") != null
                    && int.TryParse(e.Attribute("languagecode").Value, out _)
                    && e.Attribute("description") != null
                    && !string.IsNullOrEmpty(e.Attribute("description").Value)
                );

                StringBuilder str = new StringBuilder(id);

                foreach (var title in titlesList.OrderBy(e => int.Parse(e.Attribute("languagecode").Value)))
                {
                    string description = (string)title.Attribute("description");
                    int languageCode = (int)title.Attribute("languagecode");

                    str.AppendLine();

                    if (LanguageLocale.KnownLocales.ContainsKey(languageCode))
                    {
                        str.AppendFormat("{0} - {1}: {2}", languageCode, LanguageLocale.KnownLocales[languageCode], description);
                    }
                    else
                    {
                        str.AppendFormat("{0}: {2}", languageCode, description);
                    }
                }

                string insertText = string.Format("$LocLabels:{0}", id);

                list.Add(CreateCompletion(id, insertText, str.ToString(), _defaultGlyph, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerLocLables, name, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }
    }
}