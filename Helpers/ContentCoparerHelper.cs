using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Ude;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    internal static class ContentCoparerHelper
    {
        private static readonly List<Encoding> encodings;

        private const float Diff_Timeout = 60;

        static ContentCoparerHelper()
        {
            List<Encoding> list = new List<Encoding>() { Encoding.Unicode, Encoding.UTF32, Encoding.ASCII, Encoding.UTF8, Encoding.UTF7 };

            EncodingInfo[] allEncodings = Encoding.GetEncodings();

            string[] encNames = { "koi8-r", "koi8-u", "dos" };

            foreach (EncodingInfo encInfo in allEncodings)
            {
                if (encInfo.CodePage == 1251)
                {
                    list.Add(encInfo.GetEncoding());
                }
                else if (encNames.Contains(encInfo.Name))
                {
                    list.Add(encInfo.GetEncoding());
                }
            }

            encodings = list;
        }

        private static List<string> GetListString(byte[] arrayByte, bool removeNewLine)
        {
            List<string> list = new List<string>();

            List<Encoding> currentFileEncodings = GetFileEncoding(arrayByte);

            if (currentFileEncodings.Count == 0)
            {
                currentFileEncodings = encodings;
            }

            foreach (Encoding encFrom in currentFileEncodings)
            {
                byte[] arr = RemoveBOM(arrayByte, encFrom.GetPreamble());

                string str = encFrom.GetString(arr).Trim();
                str = RemoveDiacritics(str);

                if (removeNewLine)
                {
                    StringBuilder stringBuilder = new StringBuilder(str);

                    stringBuilder = stringBuilder.Replace("\r", " ");
                    stringBuilder = stringBuilder.Replace("\n", " ");
                    stringBuilder = stringBuilder.Replace("\t", " ");
                    stringBuilder = stringBuilder.Replace(System.Environment.NewLine, " ");
                    RemoveDoubleSpaces(stringBuilder);

                    str = stringBuilder.ToString().Trim();
                }

                list.Add(str);
            }

            return list;
        }

        public static string RemoveDiacritics(string s)
        {
            if (s == null)
            {
                return string.Empty;
            }

            s = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                UnicodeCategory cat = CharUnicodeInfo.GetUnicodeCategory(ch);

                if (cat != UnicodeCategory.NonSpacingMark
                    && cat != UnicodeCategory.Format
                    )
                {
                    sb.Append(s[i]);
                }
            }

            return sb.ToString();
        }

        private static byte[] RemoveBOM(byte[] arrayByte, byte[] bom)
        {
            byte[] result = arrayByte;

            if (bom.Length > 0 && result.Length > bom.Length)
            {
                bool hasBom = true;

                do
                {
                    hasBom = true;

                    for (int index = 0; index < bom.Length; index++)
                    {
                        if (bom[index] != result[index])
                        {
                            hasBom = false;
                            break;
                        }
                    }

                    if (hasBom)
                    {
                        result = result.Skip(bom.Length).ToArray();
                    }

                } while (hasBom);
            }

            return result;
        }

        public static List<Encoding> GetFileEncoding(byte[] arrayByte)
        {
            List<Encoding> result = new List<Encoding>();

            CharsetDetector detector = new CharsetDetector();

            detector.Feed(arrayByte, 0, arrayByte.Length);
            detector.DataEnd();

            if (!string.IsNullOrEmpty(detector.Charset) && detector.Confidence > 0.8f)
            {
                try
                {
                    Encoding enc = Encoding.GetEncoding(detector.Charset);

                    result.Add(enc);
                }
                catch (Exception ex)
                {
                    Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
#endif
                }
            }

            return result;
        }

        private static void RemoveDoubleSpaces(StringBuilder stringBuilder)
        {
            int lastWidth;

            do
            {
                lastWidth = stringBuilder.Length;

                stringBuilder.Replace("  ", " ");

            } while (lastWidth != stringBuilder.Length);
        }

        private static bool ListsHasEquals(List<string> list1, List<string> list2)
        {
            foreach (string str1 in list1)
            {
                foreach (string str2 in list2)
                {
                    if (string.Equals(str1, str2))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static ContentCopareResult CompareByteArrays(string ext, byte[] array1, byte[] array2, bool withDetails = false)
        {
            bool supportExtension = FileOperations.SupportsWebResourceTextType(ext);

            if (!supportExtension)
            {
                return new ContentCopareResult(false, null);
            }

            List<string> list1 = GetListString(array1, true);
            List<string> list2 = GetListString(array2, true);

            bool textEqual = ListsHasEquals(list1, list2);

            if (textEqual)
            {
                return new ContentCopareResult(true, null);
            }

            if (withDetails)
            {
                //list1 = GetListString(array1, false);
                //list2 = GetListString(array2, false);

                List<Diff> minimalDifference = GetMinimalDifferences(list1, list2);

                return new ContentCopareResult(false, minimalDifference);
            }
            else
            {
                return new ContentCopareResult(false, null);
            }
        }

        private static List<Diff> GetMinimalDifferences(List<string> list1, List<string> list2)
        {
            diff_match_patch match = new diff_match_patch
            {
                Diff_Timeout = ContentCoparerHelper.Diff_Timeout
            };
            //match.Patch_Margin

            int minDiff = int.MaxValue;
            List<Diff> selected = null;

            foreach (string str1 in list1)
            {
                foreach (string str2 in list2)
                {
                    List<Diff> diff = match.diff_main(str1, str2, false);

                    IEnumerable<Diff> changes = diff.Where(d => d.operation != Operation.EQUAL);

                    int changesLength = changes.Any() ? changes.Sum(d => d.text.Length) : 0;

                    if (changesLength < minDiff)
                    {
                        minDiff = changesLength;
                        selected = diff;
                    }
                }
            }

            return selected;
        }

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

            if (string.Equals(doc.Name.LocalName, "Activity", StringComparison.OrdinalIgnoreCase))
            {
                XAttribute attr = doc.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Class", StringComparison.OrdinalIgnoreCase));

                if (attr != null && attr.Value.StartsWith("XrmWorkflow", StringComparison.OrdinalIgnoreCase))
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

                xml = ContentCoparerHelper.RemoveDiacritics(xml);

                if (!TryParseXml(xml, out XElement doc))
                {
                    return xml;
                }

                RemoveEmptyXMLText(doc);
                SortAttributes(doc);

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
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

                xml1 = ContentCoparerHelper.RemoveDiacritics(xml1);
                xml2 = ContentCoparerHelper.RemoveDiacritics(xml2);

                diff_match_patch match = new diff_match_patch
                {
                    Diff_Timeout = ContentCoparerHelper.Diff_Timeout
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

                SortAttributes(doc1);
                SortAttributes(doc2);

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
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

                xml1 = ContentCoparerHelper.RemoveDiacritics(xml1);
                xml2 = ContentCoparerHelper.RemoveDiacritics(xml2);

                diff_match_patch match = new diff_match_patch
                {
                    Diff_Timeout = ContentCoparerHelper.Diff_Timeout
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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
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

        public static byte[] FormatXmlWithXmlAttributeOnNewLine(byte[] byteXml)
        {
            byte[] result = new byte[0];

            XElement doc = null;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(byteXml, 0, byteXml.Length);

                memStream.Position = 0;

                doc = XElement.Load(memStream);
            }

            SortAttributes(doc);

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true,
                Encoding = Encoding.UTF8
            };

            using (MemoryStream memStream = new MemoryStream())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(memStream, settings))
                {
                    doc.Save(xmlWriter);
                }

                memStream.Position = 0;

                result = memStream.ToArray();
            }

            return result;
        }

        public static string FormatXml(string xml, bool xmlAttributeOnNewLine)
        {
            if (!TryParseXml(xml, out XElement doc))
            {
                return xml;
            }

            if (!xmlAttributeOnNewLine)
            {
                return doc.ToString();
            }

            SortAttributes(doc);

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
            }

            return result.ToString();
        }

        private static void SortAttributes(XElement doc)
        {
            foreach (XElement element in doc.DescendantsAndSelf())
            {
                List<XAttribute> attributes = element.Attributes().ToList();

                element.RemoveAttributes();

                foreach (XAttribute attr in attributes.OrderBy(a => a.Name, new XNameComparer()))
                {
                    element.Add(attr);
                }
            }
        }

        public static bool IsEntityDifferentInField(Microsoft.Xrm.Sdk.Entity entity1, Microsoft.Xrm.Sdk.Entity entity2, string fieldName)
        {
            if (entity1.Contains(fieldName)
                && entity1.Attributes[fieldName] != null
                && entity1.Attributes[fieldName] is EntityReference
                && (entity1.Attributes[fieldName] as EntityReference).Id == Guid.Empty
                )
            {
                entity1.Attributes.Remove(fieldName);
            }

            if (entity2.Contains(fieldName)
              && entity2.Attributes[fieldName] != null
              && entity2.Attributes[fieldName] is EntityReference
              && (entity2.Attributes[fieldName] as EntityReference).Id == Guid.Empty
              )
            {
                entity2.Attributes.Remove(fieldName);
            }

            if (entity1.Contains(fieldName) && entity1.Attributes[fieldName] != null && !entity2.Contains(fieldName))
            {
                return true;
            }

            if (entity2.Contains(fieldName) && entity2.Attributes[fieldName] != null && !entity1.Contains(fieldName))
            {
                return true;
            }

            if (!entity1.Contains(fieldName) && !entity2.Contains(fieldName))
            {
                return false;
            }

            string str1 = EntityDescriptionHandler.GetAttributeString(entity1, fieldName);
            string str2 = EntityDescriptionHandler.GetAttributeString(entity2, fieldName);

            return str1 != str2;
        }

        public static ContentCopareResult CompareText(string text1, string text2)
        {
            text1 = text1 ?? string.Empty;
            text2 = text2 ?? string.Empty;

            text1 = ContentCoparerHelper.RemoveDiacritics(text1);
            text2 = ContentCoparerHelper.RemoveDiacritics(text2);

            diff_match_patch match = new diff_match_patch
            {
                Diff_Timeout = ContentCoparerHelper.Diff_Timeout
            };

            if (string.IsNullOrEmpty(text1) && string.IsNullOrEmpty(text2))
            {
                return new ContentCopareResult(true, null);
            }

            if (string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
            {
                return new ContentCopareResult(false, match.diff_main(text1, text2, false));
            }

            if (!string.IsNullOrEmpty(text1) && string.IsNullOrEmpty(text2))
            {
                return new ContentCopareResult(false, match.diff_main(text1, text2, false));
            }

            bool isEqual = string.Equals(text1, text2);

            if (isEqual)
            {
                return new ContentCopareResult(true, null);
            }
            else
            {
                return new ContentCopareResult(false, match.diff_main(text1, text2, false));
            }
        }

        public static string FormatToJavaScript(string fieldName, string xmlContent)
        {
            IEnumerable<string> split = xmlContent
                .Split(new[] { "\r\n" }, StringSplitOptions.None)
                .SelectMany(s => s.Split(new[] { "\n\r" }, StringSplitOptions.None))
                .SelectMany(s => s.Split(new[] { "\r", "\n" }, StringSplitOptions.None))
                ;

            StringBuilder str = new StringBuilder();

            str.AppendFormat("var {0} =", fieldName).AppendLine();
            str.AppendLine("[");

            foreach (string item in split)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    str.AppendLine();
                }
                else
                {
                    str.AppendFormat("'{0}',", item).AppendLine();
                }
            }

            str.Append("].join('');");

            return str.ToString();
        }

        private const string patternXsi = " xmlns:xsi=\"([^\"]+)\"";
        private const string patternXsiNamespace = @" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""";
        private const string patternSchemaLocation = " xsi:schemaLocation=\"([^\"]+)\"";
        private const string patternSchemaLocationFormat = " xsi:schemaLocation=\"{0}\"";

        public static bool ClearXsdSchema(string text, out string changeText)
        {
            bool result = false;
            changeText = text;

            if (Regex.IsMatch(changeText, patternXsi))
            {
                result = true;
                changeText = Regex.Replace(changeText, patternXsi, string.Empty, RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(changeText, patternSchemaLocation))
            {
                result = true;
                changeText = Regex.Replace(changeText, patternSchemaLocation, string.Empty, RegexOptions.IgnoreCase);
            }

            return result;
        }

        public static string ReplaceXsdSchema(string text, string schemas)
        {
            var result = text;

            if (Regex.IsMatch(result, patternXsi))
            {
                result = Regex.Replace(result, patternXsi, patternXsiNamespace, RegexOptions.IgnoreCase);
            }
            else
            {
                int? indexInsert = FindIndexToInsert(result);

                if (indexInsert.HasValue)
                {
                    result = result.Insert(indexInsert.Value, patternXsiNamespace);
                }
            }

            if (Regex.IsMatch(result, patternSchemaLocation))
            {
                result = Regex.Replace(result, patternSchemaLocation, string.Format(patternSchemaLocationFormat, schemas), RegexOptions.IgnoreCase);
            }
            else
            {
                int? indexInsert = FindIndexToInsert(result);

                if (indexInsert.HasValue)
                {
                    result = result.Insert(indexInsert.Value, string.Format(patternSchemaLocationFormat, schemas));
                }
            }

            return result;
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
    }
}