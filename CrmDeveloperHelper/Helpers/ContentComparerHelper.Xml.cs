using Microsoft.VisualStudio.Text.Editor;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    internal static partial class ContentComparerHelper
    {
        public static void RemoveLayoutObjectCode(XElement doc)
        {
            if (doc.Name == "grid")
            {
                XAttribute attr = doc.Attribute("object");

                if (attr != null)
                {
                    attr.Remove();
                }
            }
        }

        public static void RenameClasses(XElement doc)
        {
            const string className = "XrmWorkflow00000000000000000000000000000000";

            if (string.Equals(doc.Name.LocalName, "Activity", StringComparison.InvariantCultureIgnoreCase))
            {
                XAttribute attr = doc.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Class", StringComparison.InvariantCultureIgnoreCase));

                if (attr != null && attr.Value.StartsWith("XrmWorkflow", StringComparison.InvariantCultureIgnoreCase))
                {
                    string oldClass = attr.Value;

                    attr.Value = className;

                    IEnumerable<XElement> elements = doc.DescendantsAndSelf().Where(e => e.Name.LocalName.Contains(oldClass));

                    foreach (XElement el in elements)
                    {
                        el.Name = XName.Get(el.Name.LocalName.Replace(oldClass, className), el.Name.NamespaceName);
                    }
                }
            }
        }

        private static void RemoveEmptyXMLText(XElement doc)
        {
            List<XElement> list = doc.DescendantsAndSelf().Where(e => !e.IsEmpty && !e.HasElements && !e.HasAttributes && string.IsNullOrEmpty(e.Value)).ToList();

            foreach (XElement item in list)
            {
                item.RemoveAll();
            }
        }

        private static void RemoveEmptyElements(XElement doc)
        {
            {
                List<XElement> list = doc.DescendantsAndSelf().Where(e => !e.IsEmpty && !e.HasElements && !e.HasAttributes && string.IsNullOrEmpty(e.Value)).ToList();

                foreach (XElement item in list)
                {
                    item.RemoveAll();
                }
            }

            {
                List<XElement> list = doc.DescendantsAndSelf().Where(e => e.IsEmpty).ToList();

                foreach (XElement item in list)
                {
                    if (item.Parent != null)
                    {
                        item.Remove();
                    }
                }
            }
        }

        public static string ClearXml(string xml, params Action<XElement>[] actions)
        {
            try
            {
                xml = xml ?? string.Empty;

                xml = ContentComparerHelper.RemoveDiacritics(xml);

                if (!TryParseXml(xml, out XElement doc))
                {
                    return xml;
                }

                RemoveEmptyXMLText(doc);
                SortXmlAttributesInternal(doc);
                SortRibbonCommandsAndRulesByIdInternal(doc);
                SortFormXmlElementsInternal(doc);

                if (actions != null)
                {
                    foreach (Action<XElement> action in actions)
                    {
                        action?.Invoke(doc);
                    }
                }

                return doc.ToString();
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif

                throw;
            }
        }

        public static string GetCorrectedXaml(string xml, LabelReplacer replacer)
        {
            try
            {
                xml = xml ?? string.Empty;

                xml = ContentComparerHelper.RemoveDiacritics(xml);

                if (!TryParseXml(xml, out XElement doc))
                {
                    return xml;
                }

                replacer.FullfillLabelsForWorkflow(doc);

                RemoveEmptyXMLText(doc);

                RenameClasses(doc);

                WorkflowUsedEntitiesHandler.ReplaceGuids(doc);

                return doc.ToString();
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif

                throw;
            }
        }

        public static ContentCopareResult CompareXML(string xml1, string xml2, bool withDetails = false, Action<XElement> action = null)
        {
            try
            {
                xml1 = xml1 ?? string.Empty;
                xml2 = xml2 ?? string.Empty;

                xml1 = ContentComparerHelper.RemoveDiacritics(xml1);
                xml2 = ContentComparerHelper.RemoveDiacritics(xml2);

                diff_match_patch match = new diff_match_patch
                {
                    Diff_Timeout = ContentComparerHelper.Diff_Timeout
                };

                if (string.IsNullOrEmpty(xml1) && string.IsNullOrEmpty(xml2))
                {
                    return new ContentCopareResult(true, null);
                }

                if (string.IsNullOrEmpty(xml1) && !string.IsNullOrEmpty(xml2))
                {
                    return new ContentCopareResult(false, match.diff_main(xml1, xml2, false));
                }

                if (!string.IsNullOrEmpty(xml1) && string.IsNullOrEmpty(xml2))
                {
                    return new ContentCopareResult(false, match.diff_main(xml1, xml2, false));
                }

                if (!TryParseXml(xml1, out XElement doc1) || !TryParseXml(xml2, out XElement doc2))
                {
                    bool isEqual = string.Equals(xml1, xml2);

                    if (isEqual)
                    {
                        return new ContentCopareResult(true, null);
                    }
                    else
                    {
                        if (withDetails)
                        {
                            return new ContentCopareResult(false, match.diff_main(xml1, xml2, false));
                        }
                        else
                        {
                            return new ContentCopareResult(false, null);
                        }
                    }
                }

                RemoveEmptyXMLText(doc1);
                RemoveEmptyXMLText(doc2);

                SortXmlAttributesInternal(doc1);
                SortXmlAttributesInternal(doc2);

                SortRibbonCommandsAndRulesByIdInternal(doc1);
                SortRibbonCommandsAndRulesByIdInternal(doc2);

                SortFormXmlElementsInternal(doc1);
                SortFormXmlElementsInternal(doc2);

                if (action != null)
                {
                    action(doc1);
                    action(doc2);
                }

                {
                    bool isEqual = XNode.DeepEquals(doc1, doc2);

                    if (isEqual)
                    {
                        return new ContentCopareResult(true, null);
                    }
                    else
                    {
                        if (withDetails)
                        {
                            return new ContentCopareResult(false, match.diff_main(doc1.ToString(), doc2.ToString(), false));
                        }
                        else
                        {
                            return new ContentCopareResult(false, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif

                throw;
            }
        }

        public static ContentCopareResult CompareWorkflowXAML(string xml1, string xml2, LabelReplacer rep1, LabelReplacer rep2, bool withDetails = false)
        {
            try
            {
                xml1 = xml1 ?? string.Empty;
                xml2 = xml2 ?? string.Empty;

                xml1 = ContentComparerHelper.RemoveDiacritics(xml1);
                xml2 = ContentComparerHelper.RemoveDiacritics(xml2);

                diff_match_patch match = new diff_match_patch
                {
                    Diff_Timeout = ContentComparerHelper.Diff_Timeout
                };

                if (string.IsNullOrEmpty(xml1) && string.IsNullOrEmpty(xml2))
                {
                    return new ContentCopareResult(true, null);
                }

                if (string.IsNullOrEmpty(xml1) && !string.IsNullOrEmpty(xml2))
                {
                    return new ContentCopareResult(false, match.diff_main(xml1, xml2, false));
                }

                if (!string.IsNullOrEmpty(xml1) && string.IsNullOrEmpty(xml2))
                {
                    return new ContentCopareResult(false, match.diff_main(xml1, xml2, false));
                }

                if (!TryParseXml(xml1, out XElement doc1) || !TryParseXml(xml2, out XElement doc2))
                {
                    bool isEqual = string.Equals(xml1, xml2);

                    if (isEqual)
                    {
                        return new ContentCopareResult(true, null);
                    }
                    else
                    {
                        if (withDetails)
                        {
                            return new ContentCopareResult(false, match.diff_main(xml1, xml2, false));
                        }
                        else
                        {
                            return new ContentCopareResult(false, null);
                        }
                    }
                }

                RemoveEmptyElements(doc1);
                RemoveEmptyElements(doc2);

                RenameClasses(doc1);
                RenameClasses(doc2);

                WorkflowUsedEntitiesHandler.ReplaceGuids(doc1);
                WorkflowUsedEntitiesHandler.ReplaceGuids(doc2);

                if (rep1 != null)
                {
                    rep1.FullfillLabelsForWorkflow(doc1);
                }

                if (rep2 != null)
                {
                    rep2.FullfillLabelsForWorkflow(doc2);
                }

                {
                    bool isEqual = XNode.DeepEquals(doc1, doc2);

                    if (isEqual)
                    {
                        return new ContentCopareResult(true, null);
                    }
                    else
                    {
                        if (withDetails)
                        {
                            return new ContentCopareResult(false, match.diff_main(doc1.ToString(), doc2.ToString(), false));
                        }
                        else
                        {
                            return new ContentCopareResult(false, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif

                throw;
            }
        }

        public static bool TryParseXml(string xml, out XElement doc)
        {
            doc = null;

            try
            {
                xml = RemoveDiacritics(xml);

                doc = XElement.Parse(xml);

                return true;
            }
            catch
            {
                doc = null;

                return false;
            }
        }

        public static bool TryParseXmlDocument(string xml, out XDocument doc)
        {
            doc = null;

            try
            {
                xml = RemoveDiacritics(xml);

                doc = XDocument.Parse(xml);

                return true;
            }
            catch
            {
                doc = null;

                return false;
            }
        }

        public static bool TryParseXml(string xml, out Exception exception, out XElement doc)
        {
            doc = null;
            exception = null;

            try
            {
                xml = RemoveDiacritics(xml);

                doc = XElement.Parse(xml);

                return true;
            }
            catch (Exception ex)
            {
                doc = null;
                exception = ex;

                return false;
            }
        }

        private static readonly List<string> _predefinedAttributeOrder = new List<string>()
        {
            "Id"
            , "GroupId"
            , "Name"
            , "Entity"
            , "Command"
            , "Sequence"
            , "LabelText"
            , "Alt"
            , "ToolTipTitle"
            , "ToolTipDescription"
        };

        private static readonly List<string> _predefinedNamespacesOrder = new List<string>()
        {
            Intellisense.Model.IntellisenseContext.IntellisenseContextNamespace.NamespaceName
        };

        private static void SortXmlAttributesInternal(XElement doc)
        {
            foreach (XElement element in doc.DescendantsAndSelf())
            {
                List<XAttribute> attributes = element.Attributes().ToList();

                element.RemoveAttributes();

                foreach (XAttribute attr in attributes.OrderBy(a => a.Name, new XNameComparer(_predefinedAttributeOrder, _predefinedNamespacesOrder)))
                {
                    element.Add(attr);
                }
            }
        }

        private static readonly string[] _pathsCommandsAndRules = new[]
        {
            "./CommandDefinitions"
            , "./RibbonDefinition/CommandDefinitions"
            , "./RuleDefinitions/EnableRules"
            , "./RibbonDefinition/RuleDefinitions/EnableRules"
            , "./RuleDefinitions/DisplayRules"
            , "./RibbonDefinition/RuleDefinitions/DisplayRules"
        };

        private static void SortRibbonCommandsAndRulesByIdInternal(XElement doc)
        {
            foreach (var path in _pathsCommandsAndRules)
            {
                var elements = doc.XPathSelectElements(path).ToList();

                if (elements.Count == 1)
                {
                    var commandDefinitions = elements.FirstOrDefault();

                    var commandList = commandDefinitions.Elements().ToList();

                    foreach (var item in commandList)
                    {
                        item.Remove();
                    }

                    commandDefinitions.Add(commandList.OrderBy(n => (string)n.Attribute("Id")));
                }
            }
        }

        private static readonly List<string> _predefinedFormXmlElementsOrder = new List<string>()
        {
            "ancestor"
            , "hiddencontrols"
            , "tabs"
            , "header"
            , "footer"
            , "events"
            , "formLibraries"
            , "externaldependencies"
            , "formparameters"
            , "clientresources"
            , "Navigation"
            , "DisplayConditions"
            , "RibbonDiffXml"
        };

        private static void SortFormXmlElementsInternal(XElement doc)
        {
            if (!string.Equals(doc.Name.LocalName, "form", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var elementsList = doc.Elements().ToList();

            foreach (var item in elementsList)
            {
                item.Remove();
            }

            doc.Add(elementsList.OrderBy(e => e.Name, new XNameComparer(_predefinedFormXmlElementsOrder)));
        }

        private static string FormatXmlNewLineOnAttributes(XElement doc)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true,
                Encoding = Encoding.UTF8
            };

            StringBuilder result = new StringBuilder();

            using (XmlWriter xmlWriter = XmlWriter.Create(result, settings))
            {
                doc.Save(xmlWriter);
                xmlWriter.Flush();
            }

            return result.ToString();
        }

        public static string FormatXmlByConfiguration(
            string xml
            , CommonConfiguration commonConfig
            , XmlOptionsControls xmlOptions
            , string schemaName = null
            , string entityName = null
            , string siteMapUniqueName = null
            , Guid? formId = null
            , Guid? savedQueryId = null
            , Guid? customControlId = null
            , Guid? workflowId = null
            , string webResourceName = null
        )
        {
            var result = xml;

            if ((xmlOptions & XmlOptionsControls.SetIntellisenseContext) != 0
                && commonConfig.SetIntellisenseContext
            )
            {
                if (entityName != null)
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextEntityName, replaceIntellisenseContextEntityNameFormat3, entityName);
                }

                if (siteMapUniqueName != null)
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextSiteMapNameUnique, replaceIntellisenseContextSiteMapNameUniqueFormat3, siteMapUniqueName);
                }

                if (savedQueryId.HasValue)
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextSavedQueryId, replaceIntellisenseContextSavedQueryIdFormat3, savedQueryId.ToString());
                }

                if (formId.HasValue)
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextFormId, replaceIntellisenseContextFormIdFormat3, formId.ToString());
                }

                if (customControlId.HasValue)
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextCustomControlId, replaceIntellisenseContextCustomControlIdFormat3, customControlId.ToString());
                }

                if (workflowId.HasValue)
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextWorkflowId, replaceIntellisenseContextWorkflowIdFormat3, workflowId.ToString());
                }

                if (!string.IsNullOrEmpty(webResourceName))
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContextWebResourceName, replaceIntellisenseContextWebResourceNameFormat3, webResourceName);
                }

                if (entityName != null
                    || siteMapUniqueName != null
                    || savedQueryId.HasValue
                    || formId.HasValue
                    || customControlId.HasValue
                    || workflowId.HasValue
                    || !string.IsNullOrEmpty(webResourceName)
                )
                {
                    result = ReplaceOrInsertAttribute(result, patternIntellisenseContext, replaceIntellisenseContextNamespaceFormat3, Intellisense.Model.IntellisenseContext.IntellisenseContextNamespace.NamespaceName);
                }
            }

            if ((xmlOptions & XmlOptionsControls.SetXmlSchemas) != 0
                && commonConfig.SetXmlSchemasDuringExport
                )
            {
                var schemasResources = AbstractDynamicCommandXsdSchemas.GetXsdSchemas(schemaName);

                if (schemasResources != null)
                {
                    result = ContentComparerHelper.SetXsdSchema(result, schemasResources);
                }
            }

            if (TryParseXml(result, out XElement doc))
            {
                var sortRibbonCommandsAndRulesById = (xmlOptions & XmlOptionsControls.SortRibbonCommandsAndRulesById) != 0 && commonConfig.SortRibbonCommandsAndRulesById;
                var sortFormXmlElements = (xmlOptions & XmlOptionsControls.SortFormXmlElements) != 0 && commonConfig.SortFormXmlElements;
                var sortXmlAttributes = (xmlOptions & XmlOptionsControls.SortXmlAttributes) != 0 && commonConfig.SortXmlAttributes;

                if (sortRibbonCommandsAndRulesById)
                {
                    SortRibbonCommandsAndRulesByIdInternal(doc);
                }

                if (sortFormXmlElements)
                {
                    SortFormXmlElementsInternal(doc);
                }

                if (sortXmlAttributes)
                {
                    SortXmlAttributesInternal(doc);
                }

                if ((xmlOptions & XmlOptionsControls.XmlAttributeOnNewLine) != 0 && commonConfig.ExportXmlAttributeOnNewLine)
                {
                    return FormatXmlNewLineOnAttributes(doc);
                }
                else
                {
                    return doc.ToString();
                }
            }
            else
            {
                return result;
            }
        }

        private const string groupNameQuote = "quote";
        private const string groupNameValue = "value";

        private static readonly string patternQuote = $"(?<{groupNameQuote}>'|\")";
        private static readonly string patternValue = $"(?<{groupNameValue}>.*?)";
        private static readonly string patternQuoteBackRefference = $"(?:\\k<{groupNameQuote}>)";

        private const string replaceXsiNamespaceFormat3 = " xmlns:" + Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstancePrefix + "={0}{1}{2}";
        private static readonly string patternXsi = string.Format(replaceXsiNamespaceFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceXsiSchemaLocationFormat3 = " " + Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstancePrefix + ":" + Intellisense.Model.IntellisenseContext.NameXMLSchemaInstanceAttributeSchemaLocation + "={0}{1}{2}";
        private static readonly string patternXsiSchemaLocation = string.Format(replaceXsiSchemaLocationFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private static readonly string patternXsiAnyAttribute = " " + Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstancePrefix + ":([^\"']*)=" + patternQuote + patternValue + patternQuoteBackRefference;

        private const string replaceIntellisenseContextNamespaceFormat3 = " xmlns:" + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + "={0}{1}{2}";
        private static readonly string patternIntellisenseContext = string.Format(replaceIntellisenseContextNamespaceFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private static readonly string patternIntellisenseContextAttributes = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":([^\"']*)=" + patternQuote + patternValue + patternQuoteBackRefference;

        private const string replaceIntellisenseContextEntityNameFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeEntityName + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextEntityName = string.Format(replaceIntellisenseContextEntityNameFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceIntellisenseContextSavedQueryIdFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeSavedQueryId + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextSavedQueryId = string.Format(replaceIntellisenseContextSavedQueryIdFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceIntellisenseContextFormIdFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeFormId + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextFormId = string.Format(replaceIntellisenseContextFormIdFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceIntellisenseContextCustomControlIdFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeCustomControlId + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextCustomControlId = string.Format(replaceIntellisenseContextCustomControlIdFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceIntellisenseContextWebResourceNameFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeWebResourceName + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextWebResourceName = string.Format(replaceIntellisenseContextWebResourceNameFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceIntellisenseContextSiteMapNameUniqueFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeSiteMapNameUnique + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextSiteMapNameUnique = string.Format(replaceIntellisenseContextSiteMapNameUniqueFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private const string replaceIntellisenseContextWorkflowIdFormat3 = " " + Intellisense.Model.IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + Intellisense.Model.IntellisenseContext.NameIntellisenseContextAttributeWorkflowId + "={0}{1}{2}";
        private static readonly string patternIntellisenseContextWorkflowId = string.Format(replaceIntellisenseContextWorkflowIdFormat3, patternQuote, patternValue, patternQuoteBackRefference);

        private static string SetXsdSchema(string text, string[] fileNamesColl)
        {
            if (fileNamesColl == null || !fileNamesColl.Any())
            {
                return text;
            }

            string schemas = HandleExportXsdSchemaIntoSchamasFolder(fileNamesColl);

            if (string.IsNullOrEmpty(schemas))
            {
                return text;
            }

            string result = text;

            result = ReplaceOrInsertAttribute(result, patternXsi, replaceXsiNamespaceFormat3, Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstance.NamespaceName);

            result = ReplaceOrInsertAttribute(result, patternXsiSchemaLocation, replaceXsiSchemaLocationFormat3, schemas);

            return result;
        }

        private static string ReplaceOrInsertAttribute(string result, string patternAttribute, string format3, string newValue)
        {
            try
            {
                var match = Regex.Match(result, patternAttribute);

                if (match.Success)
                {
                    string quoteSymbol = match.Groups[groupNameQuote].Value;
                    string replacement = string.Format(format3, quoteSymbol, newValue, quoteSymbol);

                    result = Regex.Replace(result, patternAttribute, replacement, RegexOptions.IgnoreCase);
                }
                else
                {
                    int? indexInsert = FindIndexToInsert(result);
                    string quoteSymbol = FindQuoteSymbol(result);

                    if (indexInsert.HasValue)
                    {
                        string replacement = string.Format(format3, quoteSymbol, newValue, quoteSymbol);

                        result = result.Insert(indexInsert.Value, replacement);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }

            return result;
        }

        public static void ReplaceXsdSchemaInDocument(EnvDTE.Document document, string[] fileNamesColl)
        {
            if (fileNamesColl == null || !fileNamesColl.Any())
            {
                return;
            }

            string schemas = HandleExportXsdSchemaIntoSchamasFolder(fileNamesColl);

            if (string.IsNullOrEmpty(schemas))
            {
                return;
            }

            GetTextViewAndMakeActionAsync(document, Properties.OperationNames.AddXmlSchemaLocation, (wpfTextView, oldCaretLine, oldCaretColumn) => ReplaceXsdSchemaInTextView(wpfTextView, schemas));
        }

        public static string RemoveAllCustomXmlAttributesAndNamespaces(string text)
        {
            string result = text;

            if (Regex.IsMatch(result, patternIntellisenseContext))
            {
                result = Regex.Replace(result, patternIntellisenseContext, string.Empty, RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(result, patternIntellisenseContextAttributes))
            {
                result = Regex.Replace(result, patternIntellisenseContextAttributes, string.Empty, RegexOptions.IgnoreCase);
            }

            result = RemoveInTextSchemaLocationAndXsiNamespace(result);

            return result;
        }

        private static string RemoveInTextSchemaLocationAndXsiNamespace(string text)
        {
            if (Regex.IsMatch(text, patternXsiSchemaLocation))
            {
                text = Regex.Replace(text, patternXsiSchemaLocation, string.Empty, RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(text, patternXsi))
            {
                if (!Regex.IsMatch(text, patternXsiAnyAttribute))
                {
                    text = Regex.Replace(text, patternXsi, string.Empty, RegexOptions.IgnoreCase);
                }
            }

            return text;
        }

        private static int? FindIndexToInsert(string result)
        {
            int? indexInsert = null;

            var regex = new Regex(@"<[^<>\s]+");

            var matches = regex.Matches(result);

            var firstMatch = matches.OfType<Match>().FirstOrDefault(x => !string.IsNullOrEmpty(x.Value)
                && !string.Equals(x.Value, "<?xml", StringComparison.InvariantCultureIgnoreCase)
                && !x.Value.StartsWith("<!--", StringComparison.InvariantCultureIgnoreCase)
                && !x.Value.StartsWith("<![CDATA[", StringComparison.InvariantCultureIgnoreCase)
            );

            if (firstMatch != null)
            {
                var index = firstMatch.Index + firstMatch.Length;

                var indexEnd = result.IndexOf('>', index);

                if (result[indexEnd] == '/')
                {
                    indexEnd--;
                }

                indexInsert = indexEnd;
            }

            return indexInsert;
        }

        private static string FindQuoteSymbol(string text)
        {
            int countQuote = 0;
            int countDoubleQuote = 0;

            foreach (var ch in text)
            {
                switch (ch)
                {
                    case '"':
                        countDoubleQuote++;
                        break;

                    case '\'':
                        countQuote++;
                        break;

                }
            }

            if (countQuote > countDoubleQuote)
            {
                return "'";
            }

            return "\"";
        }

        public static void ClearRoot(XDocument doc)
        {
            ClearRoot(doc.Root);
        }

        public static void ClearRoot(XElement doc)
        {
            var attributesAll = doc.Attributes().ToList();

            foreach (var attribute in attributesAll)
            {
                if (attribute.IsNamespaceDeclaration)
                {
                    if (string.Equals(attribute.Value, Intellisense.Model.IntellisenseContext.IntellisenseContextNamespace.NamespaceName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        attribute.Remove();
                    }
                }
                else if (attribute.Name.Namespace == Intellisense.Model.IntellisenseContext.IntellisenseContextNamespace)
                {
                    attribute.Remove();
                }
                else if (attribute.Name == Intellisense.Model.IntellisenseContext.XMLSchemaInstanceAttributeSchemaLocation)
                {
                    attribute.Remove();
                }
            }

            attributesAll = doc.Attributes().ToList();

            if (!attributesAll.Any(a => a.Name.Namespace == Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstance))
            {
                foreach (var attribute in attributesAll)
                {
                    if (attribute.IsNamespaceDeclaration)
                    {
                        if (string.Equals(attribute.Value, Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstance.NamespaceName, StringComparison.InvariantCultureIgnoreCase)
                            && attribute.Name.Namespace == XNamespace.Xmlns
                            && attribute.Name.LocalName == Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstancePrefix
                        )
                        {
                            attribute.Remove();
                            break;
                        }
                    }
                }
            }
        }

        public static void RemoveXsdSchemaInFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string text = File.ReadAllText(filePath);

            text = RemoveInTextSchemaLocationAndXsiNamespace(text);

            File.WriteAllText(filePath, text, new UTF8Encoding(false));
        }

        internal static void ReplaceXsdSchemaInFile(string filePath, string[] fileNamesColl)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string text = File.ReadAllText(filePath);

            text = SetXsdSchema(text, fileNamesColl);

            File.WriteAllText(filePath, text, new UTF8Encoding(false));
        }

        private static void RemoveXsdSchemaInTextView(IWpfTextView wpfTextView, int oldCaretLine, int oldCaretColumn)
        {
            var snapshot = wpfTextView.TextSnapshot;

            try
            {
                using (var edit = snapshot.TextBuffer.CreateEdit())
                {
                    var hasModifed = false;

                    string text = snapshot.GetText();

                    {
                        var match = Regex.Match(text, patternXsi);
                        if (match.Success)
                        {
                            hasModifed = true;
                            edit.Delete(match.Index, match.Length);
                        }
                    }

                    {
                        var match = Regex.Match(text, patternXsiSchemaLocation);
                        if (match.Success)
                        {
                            hasModifed = true;
                            edit.Delete(match.Index, match.Length);
                        }
                    }

                    if (hasModifed)
                    {
                        edit.Apply();
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.VisualStudio.Text.ITextEdit.Insert(System.Int32,System.String)")]
        private static void ReplaceXsdSchemaInTextView(IWpfTextView wpfTextView, string schemas)
        {
            var snapshot = wpfTextView.TextSnapshot;

            try
            {
                using (var edit = snapshot.TextBuffer.CreateEdit())
                {
                    var hasModifed = false;

                    string text = snapshot.GetText();

                    {
                        var match = Regex.Match(text, patternXsi);
                        if (match.Success)
                        {
                            string quoteSymbol = match.Groups[groupNameQuote].Value;
                            string replacement = string.Format(replaceXsiNamespaceFormat3, quoteSymbol, Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstance.NamespaceName, quoteSymbol);

                            if (match.Value != replacement)
                            {
                                hasModifed = true;
                                edit.Replace(match.Index, match.Length, replacement);
                            }
                        }
                        else
                        {
                            int? indexInsert = FindIndexToInsert(text);
                            string quoteSymbol = FindQuoteSymbol(text);

                            string replacement = string.Format(replaceXsiNamespaceFormat3, quoteSymbol, Intellisense.Model.IntellisenseContext.NamespaceXMLSchemaInstance.NamespaceName, quoteSymbol);

                            if (indexInsert.HasValue)
                            {
                                hasModifed = true;
                                edit.Insert(indexInsert.Value, replacement);
                            }
                        }
                    }

                    {

                        var match = Regex.Match(text, patternXsiSchemaLocation);
                        if (match.Success)
                        {
                            string quoteSymbol = match.Groups[groupNameQuote].Value;
                            var newLocation = string.Format(replaceXsiSchemaLocationFormat3, quoteSymbol, schemas, quoteSymbol);

                            if (match.Value != newLocation)
                            {
                                hasModifed = true;
                                edit.Replace(match.Index, match.Length, newLocation);
                            }
                        }
                        else
                        {
                            int? indexInsert = FindIndexToInsert(text);
                            string quoteSymbol = FindQuoteSymbol(text);

                            var newLocation = string.Format(replaceXsiSchemaLocationFormat3, quoteSymbol, schemas, quoteSymbol);

                            if (indexInsert.HasValue)
                            {
                                hasModifed = true;
                                edit.Insert(indexInsert.Value, newLocation);
                            }
                        }
                    }

                    if (hasModifed)
                    {
                        edit.Apply();
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.VisualStudio.Text.ITextEdit.Insert(System.Int32,System.String)")]
        private static void InsertIntellisenseContextEntityNameInTextView(IWpfTextView wpfTextView, string entityName)
        {
            var snapshot = wpfTextView.TextSnapshot;

            try
            {
                using (var edit = snapshot.TextBuffer.CreateEdit())
                {
                    var hasModifed = false;

                    string text = snapshot.GetText();

                    {
                        var match = Regex.Match(text, patternIntellisenseContextEntityName);
                        if (!match.Success)
                        {
                            int? indexInsert = FindIndexToInsert(text);
                            string quoteSymbol = FindQuoteSymbol(text);

                            if (indexInsert.HasValue)
                            {
                                hasModifed = true;

                                var newEntityNameAttribute = string.Format(replaceIntellisenseContextEntityNameFormat3, quoteSymbol, entityName, quoteSymbol);

                                edit.Insert(indexInsert.Value, newEntityNameAttribute);
                            }
                        }
                    }

                    {
                        var match = Regex.Match(text, patternIntellisenseContext);
                        if (!match.Success)
                        {
                            int? indexInsert = FindIndexToInsert(text);
                            string quoteSymbol = FindQuoteSymbol(text);

                            if (indexInsert.HasValue)
                            {
                                hasModifed = true;

                                var intellisenseContextNamespace = string.Format(replaceIntellisenseContextNamespaceFormat3, quoteSymbol, Intellisense.Model.IntellisenseContext.IntellisenseContextNamespace.NamespaceName, quoteSymbol);

                                edit.Insert(indexInsert.Value, intellisenseContextNamespace);
                            }
                        }
                    }

                    if (hasModifed)
                    {
                        edit.Apply();
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private static void RemoveIntellisenseContextEntityNameInTextView(IWpfTextView wpfTextView, int oldCaretLine, int oldCaretColumn)
        {
            var snapshot = wpfTextView.TextSnapshot;

            try
            {
                using (var edit = snapshot.TextBuffer.CreateEdit())
                {
                    var hasModifed = false;

                    string text = snapshot.GetText();

                    {
                        var match = Regex.Match(text, patternIntellisenseContext);
                        if (match.Success)
                        {
                            hasModifed = true;
                            edit.Delete(match.Index, match.Length);
                        }
                    }

                    {
                        var match = Regex.Match(text, patternIntellisenseContextEntityName);
                        if (match.Success)
                        {
                            hasModifed = true;
                            edit.Delete(match.Index, match.Length);
                        }
                    }

                    if (hasModifed)
                    {
                        edit.Apply();
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
