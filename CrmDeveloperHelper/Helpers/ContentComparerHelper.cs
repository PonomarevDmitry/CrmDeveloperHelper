using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Ude;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    internal static partial class ContentComparerHelper
    {
        private static readonly List<Encoding> encodings;

        private const float Diff_Timeout = 60;

        static ContentComparerHelper()
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

        public static string RemoveDiacritics(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            str = str.Normalize(NormalizationForm.FormC);
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < str.Length; index++)
            {
                char ch = str[index];
                UnicodeCategory cat = CharUnicodeInfo.GetUnicodeCategory(ch);

                if (cat != UnicodeCategory.NonSpacingMark
                    && cat != UnicodeCategory.Format
                    )
                {
                    sb.Append(str[index]);
                }
                else
                {

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
                    DTEHelper.WriteExceptionToOutput(null, ex);

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
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
                Diff_Timeout = ContentComparerHelper.Diff_Timeout
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

        public static bool IsEntityDifferentInField(Entity entity1, Entity entity2, string fieldName)
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

            text1 = ContentComparerHelper.RemoveDiacritics(text1);
            text2 = ContentComparerHelper.RemoveDiacritics(text2);

            diff_match_patch match = new diff_match_patch
            {
                Diff_Timeout = ContentComparerHelper.Diff_Timeout
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
            xmlContent = RemoveInTextAllCustomXmlAttributesAndNamespaces(xmlContent);

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

        public static string ConvertFetchXmlToQueryExpression(string fetchXml)
        {
            var codeCSharp = new StringBuilder();

            try
            {
                var serializer = new XmlSerializer(typeof(FetchType));

                FetchType fetchXmlProxy;

                using (TextReader reader = new StringReader(fetchXml))
                {
                    fetchXmlProxy = serializer.Deserialize(reader) as FetchType;
                }

                using (var writer = new StringWriter(codeCSharp))
                {
                    var codeGenerator = new QueryExpressionCodeGenerator(writer);

                    codeGenerator.WriteCSharpQueryExpression(fetchXmlProxy);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }

            return codeCSharp.ToString();
        }

        private static string HandleExportXsdSchemaIntoSchamasFolder(string[] fileNamesColl)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                string exportFolder = FileOperations.GetSchemaXsdFolder();

                foreach (var fileName in fileNamesColl)
                {
                    Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                    StreamResourceInfo info = Application.GetResourceStream(uri);

                    var doc = XDocument.Load(info.Stream);
                    info.Stream.Dispose();

                    var filePath = Path.Combine(exportFolder, fileName);

                    doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                    if (result.Length > 0)
                    {
                        result.Append(" ");
                    }

                    result.AppendFormat("{0} {1}", Path.GetFileNameWithoutExtension(fileName), new Uri(filePath).ToString());
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }

            return result.ToString();
        }

        public static async System.Threading.Tasks.Task GetTextViewAndMakeActionAsync(Document document, string operationName, Action<IWpfTextView, int, int> action)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsTextView = GetIVsTextView(document.FullName);

            if (vsTextView == null)
            {
                return;
            }

            var wpfTextView = GetWpfTextView(vsTextView);
            if (wpfTextView == null)
            {
                return;
            }

            if (CrmDeveloperHelperPackage.Singleton == null)
            {
                return;
            }

            var componentModel = (IComponentModel)await CrmDeveloperHelperPackage.Singleton.GetServiceAsync(typeof(SComponentModel));

            if (componentModel == null)
            {
                return;
            }

            var undoHistoryRegistry = componentModel.DefaultExportProvider.GetExportedValue<ITextUndoHistoryRegistry>();

            undoHistoryRegistry.TryGetHistory(wpfTextView.TextBuffer, out var history);

            using (var undo = history?.CreateTransaction(operationName))
            {
                vsTextView.GetCaretPos(out var oldCaretLine, out var oldCaretColumn);
                vsTextView.SetCaretPos(oldCaretLine, 0);

                action(wpfTextView, oldCaretLine, oldCaretColumn);

                vsTextView.GetCaretPos(out var newCaretLine, out var newCaretColumn);
                vsTextView.SetCaretPos(newCaretLine, oldCaretColumn);

                undo?.Complete();
            }
        }

        public static void InsertIntellisenseContextEntityNameInDocument(EnvDTE.Document document, string entityName)
        {
            GetTextViewAndMakeActionAsync(document, Properties.OperationNames.AddXmlSchemaLocation, (wpfTextView, oldCaretLine, oldCaretColumn) => InsertIntellisenseContextEntityNameInTextView(wpfTextView, entityName));
        }

        public static string FormatJson(string xmlContent)
        {
            if (string.IsNullOrEmpty(xmlContent))
            {
                return xmlContent;
            }

            JToken parsedJson = JToken.Parse(xmlContent);

            if (parsedJson == null)
            {
                return xmlContent;
            }

            return parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static void RemoveXsdSchemaInDocument(EnvDTE.Document document)
        {
            GetTextViewAndMakeActionAsync(document, Properties.OperationNames.RemoveXmlSchemaLocation, RemoveXsdSchemaInTextView);
        }

        public static void RemoveAllCustomAttributesInDocument(EnvDTE.Document document)
        {
            GetTextViewAndMakeActionAsync(document, Properties.OperationNames.RemoveXmlSchemaLocation, RemoveAllCustomAttributesInTextView);
        }

        public static void RemoveIntellisenseContextEntityNameInDocument(EnvDTE.Document document)
        {
            GetTextViewAndMakeActionAsync(document, Properties.OperationNames.RemoveRibbonDiffIntellisenseContextEntityName, RemoveIntellisenseContextEntityNameInTextView);
        }

        private static IVsTextView GetIVsTextView(string filePath)
        {
            if (CrmDeveloperHelperPackage.Singleton == null)
            {
                return null;
            }

            IServiceProvider provider = (IServiceProvider)(Package)CrmDeveloperHelperPackage.Singleton;

            return VsShellUtilities.IsDocumentOpen(provider, filePath, Guid.Empty, out var uiHierarchy,
                out var itemId, out var windowFrame)
                ? VsShellUtilities.GetTextView(windowFrame)
                : null;
        }

        private static IWpfTextView GetWpfTextView(IVsTextView vTextView)
        {
            IWpfTextView view = null;
            var userData = (IVsUserData)vTextView;

            if (userData != null)
            {
                var guidViewHost = Microsoft.VisualStudio.Editor.DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out var holder);
                var viewHost = (IWpfTextViewHost)holder;
                view = viewHost.TextView;
            }

            return view;
        }
    }
}