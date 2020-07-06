using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public partial class XmlGoToDefinitionCommandHandler
    {
        private bool TryGoToDefinitionInRibbon(ITextSnapshot snapshot, XElement doc, XElement currentXmlNode, string currentNodeName, string currentAttributeName, string currentValue)
        {
            #region Labels

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

            #endregion Labels

            #region Command Definitions

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

            #endregion Command Definitions

            #region Enable Rules

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

            #endregion Enable Rules

            #region Display Rules

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

            #endregion Display Rules

            #region Templates

            if (string.Equals(currentAttributeName, "Template", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    {
                        var elements = doc.XPathSelectElements("./Templates/RibbonTemplates/GroupTemplate").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

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
                        var elements = doc.XPathSelectElements("./RibbonDefinition/Templates/RibbonTemplates/GroupTemplate").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, currentValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

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
                        var elements = doc.XPathSelectElements("./Templates/RibbonTemplates").ToList();

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
                        var elements = doc.XPathSelectElements("./RibbonDefinition/Templates/RibbonTemplates").ToList();

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

            #endregion Templates

            #region Template Aliases

            if (string.Equals(currentAttributeName, "TemplateAlias", StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(currentNodeName, "OverflowSection", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                var templateName = currentXmlNode.AncestorsAndSelf().FirstOrDefault(e => e.Attribute("Template") != null)?.Attribute("Template")?.Value;

                if (!string.IsNullOrEmpty(templateName))
                {
                    XElement templateXmlNode = null;

                    {
                        var elements = doc.XPathSelectElements("./Templates/RibbonTemplates/GroupTemplate").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, templateName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            templateXmlNode = elements[0];
                        }
                    }

                    if (templateXmlNode == null)
                    {
                        var elements = doc.XPathSelectElements("./RibbonDefinition/Templates/RibbonTemplates/GroupTemplate").Where(e => e.Attribute("Id") != null && string.Equals(e.Attribute("Id").Value, templateName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (elements.Count == 1)
                        {
                            templateXmlNode = elements[0];
                        }
                    }

                    if (templateXmlNode != null)
                    {
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            var sectionWithAlias = templateXmlNode.XPathSelectElements("./Layout/OverflowSection").FirstOrDefault(e => e.Attribute("TemplateAlias") != null && string.Equals(e.Attribute("TemplateAlias").Value, currentValue, StringComparison.InvariantCultureIgnoreCase));

                            if (sectionWithAlias != null)
                            {
                                if (TryMoveToElement(snapshot, sectionWithAlias))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (TryMoveToElement(snapshot, templateXmlNode))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            #endregion Template Aliases

            if (string.Equals(currentNodeName, "CustomRule", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(currentAttributeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (TryOpenWebResourceWithPrefix(currentValue))
                    {
                        return true;
                    }
                }
                else if (string.Equals(currentAttributeName, "FunctionName", StringComparison.InvariantCultureIgnoreCase))
                {
                    var libraryAttribute = currentXmlNode.Attribute("Library");

                    if (libraryAttribute != null && !string.IsNullOrEmpty(libraryAttribute.Value))
                    {
                        if (TryOpenWebResourceWithPrefix(libraryAttribute.Value))
                        {
                            return true;
                        }
                    }
                }
            }

            if (string.Equals(currentNodeName, "JavaScriptFunction", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(currentAttributeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (TryOpenWebResourceWithPrefix(currentValue))
                    {
                        return true;
                    }
                }
                else if (string.Equals(currentAttributeName, "FunctionName", StringComparison.InvariantCultureIgnoreCase))
                {
                    var libraryAttribute = currentXmlNode.Attribute("Library");

                    if (libraryAttribute != null && !string.IsNullOrEmpty(libraryAttribute.Value))
                    {
                        if (TryOpenWebResourceWithPrefix(libraryAttribute.Value))
                        {
                            return true;
                        }
                    }
                }
            }

            if (XmlCompletionSource.ControlsWithImagesXmlElements.Contains(currentNodeName)
                && XmlCompletionSource.ImagesXmlAttributes.Contains(currentAttributeName)
            )
            {
                if (TryOpenWebResourceInWebWithPrefix(currentValue))
                {
                    return true;
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
    }
}
