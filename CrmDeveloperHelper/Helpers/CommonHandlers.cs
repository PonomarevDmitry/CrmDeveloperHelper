using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class CommonHandlers
    {
        private static readonly TimeSpan _cacheItemSpan = TimeSpan.FromSeconds(1);

        private static bool CheckActiveDocumentExtension(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && applicationObject.ActiveWindow.Document != null
            )
            {
                try
                {
                    string file = applicationObject.ActiveWindow.Document.FullName.ToString().ToLower();

                    result = checker(file);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);

                    result = false;
                }
            }

            return result;
        }

        private static bool CheckActiveDocumentIsXmlWithRoot(EnvDTE80.DTE2 applicationObject, out XElement doc, params string[] rootNames)
        {
            doc = null;

            if (rootNames == null || !rootNames.Any())
            {
                return false;
            }

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && applicationObject.ActiveWindow.Document != null
            )
            {
                try
                {
                    string file = applicationObject.ActiveWindow.Document.FullName.ToString().ToLower();

                    if (FileOperations.SupportsXmlType(file))
                    {
                        var objTextDoc = applicationObject.ActiveWindow.Document.Object("TextDocument");
                        if (objTextDoc != null
                            && objTextDoc is EnvDTE.TextDocument textDocument
                            )
                        {
                            string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                            if (!string.IsNullOrEmpty(text))
                            {
                                if (ContentCoparerHelper.TryParseXml(text, out doc))
                                {
                                    string docRootName = doc.Name.ToString();

                                    foreach (var item in rootNames)
                                    {
                                        if (string.Equals(docRootName, item, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }

            return false;
        }

        private static bool CheckActiveDocumentIsXmlWithRootWithAttribute(
            EnvDTE80.DTE2 applicationObject
            , XName attributeName
            , out XAttribute attribute
            , params string[] rootNames
        )
        {
            attribute = null;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && applicationObject.ActiveWindow.Document != null
            )
            {
                try
                {
                    string file = applicationObject.ActiveWindow.Document.FullName.ToString().ToLower();

                    if (FileOperations.SupportsXmlType(file))
                    {
                        var objTextDoc = applicationObject.ActiveWindow.Document.Object("TextDocument");
                        if (objTextDoc != null
                            && objTextDoc is EnvDTE.TextDocument textDocument
                        )
                        {
                            string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                            if (!string.IsNullOrEmpty(text))
                            {
                                if (ContentCoparerHelper.TryParseXml(text, out var doc))
                                {
                                    string docRootName = doc.Name.ToString();

                                    foreach (var rootName in rootNames)
                                    {
                                        if (string.Equals(docRootName, rootName, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            attribute = doc.Attribute(attributeName);

                                            if (attribute != null)
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }

            return false;
        }

        private static bool CheckActiveDocumentIsXmlWithRootWithAttribute(
            EnvDTE80.DTE2 applicationObject
            , XName attributeName
            , out XAttribute attribute
            , params XName[] rootNames
        )
        {
            attribute = null;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && applicationObject.ActiveWindow.Document != null
            )
            {
                try
                {
                    string file = applicationObject.ActiveWindow.Document.FullName.ToString().ToLower();

                    if (FileOperations.SupportsXmlType(file))
                    {
                        var objTextDoc = applicationObject.ActiveWindow.Document.Object("TextDocument");
                        if (objTextDoc != null
                            && objTextDoc is EnvDTE.TextDocument textDocument
                        )
                        {
                            string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                            if (!string.IsNullOrEmpty(text))
                            {
                                if (ContentCoparerHelper.TryParseXml(text, out var doc))
                                {
                                    foreach (var rootName in rootNames)
                                    {
                                        if (rootName == doc.Name)
                                        {
                                            attribute = doc.Attribute(attributeName);

                                            if (attribute != null)
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }

            return false;
        }

        private static bool CheckOpenedDocumentsExtension(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && applicationObject.ActiveWindow.Document != null
            )
            {
                foreach (var document in applicationObject.Documents.OfType<EnvDTE.Document>())
                {
                    if (document == applicationObject.ActiveWindow.Document
                        || document.ActiveWindow == null
                        || document.ActiveWindow.Type != EnvDTE.vsWindowType.vsWindowTypeDocument
                        || document.ActiveWindow.Visible == false
                        )
                    {
                        continue;
                    }

                    if (applicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindTextView)
                        || applicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindCode)
                    )
                    {
                        try
                        {
                            string file = document.FullName;

                            if (checker(file))
                            {
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(null, ex);
                        }
                    }
                }
            }

            return false;
        }

        private static bool CheckInSolutionExplorerSingle(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
            )
            {
                try
                {
                    var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                    var count = items.Where(a =>
                    {
                        try
                        {
                            string file = a.Name.ToLower();

                            return checker(file);
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(null, ex);
                        }

                        return false;
                    }).Take(2).Count();

                    if (count == 1)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }

            return false;
        }

        private static bool CheckInSolutionExplorerAny(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
            )
            {
                try
                {
                    var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                    return items.Any(a =>
                    {
                        try
                        {
                            string file = a.Name.ToLower();

                            return checker(file);
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(null, ex);

                            return false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }

            return false;
        }

        private static bool CheckInSolutionExplorerRecursive(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
                )
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>().ToList();

                foreach (var item in items)
                {
                    if (item.ProjectItem != null)
                    {
                        if (FillListProjectItems(item.ProjectItem.ProjectItems, checker))
                        {
                            return true;
                        }
                    }

                    if (item.Project != null)
                    {
                        if (FillListProjectItems(item.Project.ProjectItems, checker))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool FillListProjectItems(EnvDTE.ProjectItems projectItems, Func<string, bool> checker)
        {
            if (projectItems != null)
            {
                foreach (EnvDTE.ProjectItem projItem in projectItems)
                {
                    string path = projItem.FileNames[1];

                    if (checker(path))
                    {
                        return true;
                    }

                    if (FillListProjectItems(projItem.ProjectItems, checker))
                    {
                        return true;
                    }

                    if (projItem.SubProject != null)
                    {
                        if (FillListProjectItems(projItem.SubProject.ProjectItems, checker))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        internal static void ActionBeforeQueryStatusActiveDocumentWebResourceText(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentWebResourceText), applicationObject, ActionBeforeQueryStatusActiveDocumentWebResourceTextInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentWebResourceTextInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusActiveDocumentWebResource(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentWebResource), applicationObject, ActionBeforeQueryStatusActiveDocumentWebResourceInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentWebResourceInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusActiveDocumentXml(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentXml), applicationObject, ActionBeforeQueryStatusActiveDocumentXmlInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentXmlInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsXmlType);
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand, out XElement doc, params string[] rootNames)
        {
            doc = null;

            bool visible = false;

            visible = CheckActiveDocumentIsXmlWithRoot(applicationObject, out doc, rootNames);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
            EnvDTE80.DTE2 applicationObject
            , OleMenuCommand menuCommand
            , XName attributeName
            , out XAttribute attribute
            , params string[] rootNames
        )
        {
            attribute = null;

            bool visible = false;

            visible = CheckActiveDocumentIsXmlWithRootWithAttribute(applicationObject, attributeName, out attribute, rootNames);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
            EnvDTE80.DTE2 applicationObject
            , OleMenuCommand menuCommand
            , XName attributeName
            , out XAttribute attribute
            , params XName[] rootNames
        )
        {
            attribute = null;

            bool visible = false;

            visible = CheckActiveDocumentIsXmlWithRootWithAttribute(applicationObject, attributeName, out attribute, rootNames);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsFetchOrGrid(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentIsFetchOrGrid), applicationObject, ActionBeforeQueryStatusActiveDocumentIsFetchOrGridInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentIsFetchOrGridInternal(EnvDTE80.DTE2 applicationObject)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && applicationObject.ActiveWindow.Document != null
            )
            {
                try
                {
                    string file = applicationObject.ActiveWindow.Document.FullName.ToString().ToLower();

                    if (FileOperations.SupportsXmlType(file))
                    {
                        var objTextDoc = applicationObject.ActiveWindow.Document.Object("TextDocument");
                        if (objTextDoc != null
                            && objTextDoc is EnvDTE.TextDocument textDocument
                            )
                        {
                            string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                            if (!string.IsNullOrEmpty(text))
                            {
                                if (ContentCoparerHelper.TryParseXml(text, out var doc))
                                {
                                    if (string.Equals(doc.Name.ToString(), Commands.AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                                        || string.Equals(doc.Name.ToString(), Commands.AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                                        || string.Equals(doc.Name.ToString(), Commands.AbstractDynamicCommandXsdSchemas.RootSavedQuery, StringComparison.InvariantCultureIgnoreCase)
                                    )
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }

            return false;
        }

        internal static void ActionBeforeQueryStatusActiveDocumentReport(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentReport), applicationObject, ActionBeforeQueryStatusActiveDocumentReportInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentReportInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsReportType);
        }

        internal static void ActionBeforeQueryStatusActiveDocumentCSharp(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentCSharp), applicationObject, ActionBeforeQueryStatusActiveDocumentCSharpInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentCSharpInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsCSharpType);
        }

        internal static void ActionBeforeQueryStatusActiveDocumentJavaScript(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentJavaScript), applicationObject, ActionBeforeQueryStatusActiveDocumentJavaScriptInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentJavaScriptInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusActiveDocumentContainingProject(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusActiveDocumentContainingProject), applicationObject, ActionBeforeQueryStatusActiveDocumentContainingProjectInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusActiveDocumentContainingProjectInternal(EnvDTE80.DTE2 applicationObject)
        {
            var helper = DTEHelper.Create(applicationObject);

            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            return document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
                ;
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject), applicationObject, ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProjectInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProjectInternal(EnvDTE80.DTE2 applicationObject)
        {
            var helper = DTEHelper.Create(applicationObject);

            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            return projectItem != null && projectItem.ContainingProject != null;
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand, bool recursive)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject), applicationObject, recursive, ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProjectInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProjectInternal(EnvDTE80.DTE2 applicationObject, bool recursive)
        {
            var helper = DTEHelper.Create(applicationObject);

            var list = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, recursive);

            return list.Any(item => item != null && item.ContainingProject != null);
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsWebResourceText(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusOpenedDocumentsWebResourceText), applicationObject, ActionBeforeQueryStatusOpenedDocumentsWebResourceTextInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusOpenedDocumentsWebResourceTextInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsWebResource(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusOpenedDocumentsWebResource), applicationObject, ActionBeforeQueryStatusOpenedDocumentsWebResourceInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusOpenedDocumentsWebResourceInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsReport(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusOpenedDocumentsReport), applicationObject, ActionBeforeQueryStatusOpenedDocumentsReportInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusOpenedDocumentsReportInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsReportType);
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsCSharp(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusOpenedDocumentsCSharp), applicationObject, ActionBeforeQueryStatusOpenedDocumentsCSharpInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusOpenedDocumentsCSharpInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsCSharpType);
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsJavaScript(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusOpenedDocumentsJavaScript), applicationObject, ActionBeforeQueryStatusOpenedDocumentsJavaScriptInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusOpenedDocumentsJavaScriptInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsXml(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusOpenedDocumentsXml), applicationObject, ActionBeforeQueryStatusOpenedDocumentsXmlInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusOpenedDocumentsXmlInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsXmlType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerCSharpSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerCSharpSingle), applicationObject, ActionBeforeQueryStatusSolutionExplorerCSharpSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerCSharpSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsCSharpType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerCSharpAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerCSharpAny), applicationObject, ActionBeforeQueryStatusSolutionExplorerCSharpAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerCSharpAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsCSharpType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerCSharpRecursive), applicationObject, ActionBeforeQueryStatusSolutionExplorerCSharpRecursiveInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerCSharpRecursiveInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsCSharpType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerJavaScriptSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerJavaScriptSingle), applicationObject, ActionBeforeQueryStatusSolutionExplorerJavaScriptSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerJavaScriptSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerJavaScriptAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerJavaScriptAny), applicationObject, ActionBeforeQueryStatusSolutionExplorerJavaScriptAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerJavaScriptAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive), applicationObject, ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursiveInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursiveInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerWebResourceSingle), applicationObject, ActionBeforeQueryStatusSolutionExplorerWebResourceSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerWebResourceSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerWebResourceAny), applicationObject, ActionBeforeQueryStatusSolutionExplorerWebResourceAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerWebResourceAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive), applicationObject, ActionBeforeQueryStatusSolutionExplorerWebResourceRecursiveInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerWebResourceRecursiveInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle), applicationObject, ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny), applicationObject, ActionBeforeQueryStatusSolutionExplorerWebResourceTextAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerWebResourceTextAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerXmlAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerXmlAny), applicationObject, ActionBeforeQueryStatusSolutionExplorerXmlAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerXmlAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsXmlType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive), applicationObject, ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursiveInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursiveInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerXmlRecursive(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerXmlRecursive), applicationObject, ActionBeforeQueryStatusSolutionExplorerXmlRecursiveInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerXmlRecursiveInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsXmlType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerReportSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerReportSingle), applicationObject, ActionBeforeQueryStatusSolutionExplorerReportSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerReportSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsReportType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerReportAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerReportAny), applicationObject, ActionBeforeQueryStatusSolutionExplorerReportAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerReportAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsReportType);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerReportRecursive(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusSolutionExplorerReportRecursive), applicationObject, ActionBeforeQueryStatusSolutionExplorerReportRecursiveInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusSolutionExplorerReportRecursiveInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsReportType);
        }

        internal static void ActiveSolutionExplorerFolderSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (applicationObject.ActiveWindow != null
               && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
               && applicationObject.SelectedItems != null
            )
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>().Take(2);

                if (items.Count() == 1)
                {
                    var item = items.First();

                    visible = true;
                }
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActiveSolutionExplorerProjectSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActiveSolutionExplorerProjectSingle), applicationObject, ActiveSolutionExplorerProjectSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActiveSolutionExplorerProjectSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            bool visible = false;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
            )
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>().Take(2);

                if (items.Count() == 1)
                {
                    var item = items.First();

                    if (item.Project != null)
                    {
                        visible = true;
                    }
                }
            }

            return visible;
        }

        internal static void ActiveSolutionExplorerProjectAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActiveSolutionExplorerProjectAny), applicationObject, ActiveSolutionExplorerProjectAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActiveSolutionExplorerProjectAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
            )
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                return items.All(i => i.Project != null) && items.Any(i => i.Project != null);
            }

            return false;
        }

        private static bool CheckInPublishListSingle(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject != null)
            {
                try
                {
                    var helper = DTEHelper.Create(applicationObject);

                    var selectedFiles = helper.GetSelectedFilesFromListForPublish();

                    var count = selectedFiles.Where(f => checker(f.FilePath)).Take(2).Count();

                    result = count == 1;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);

                    result = false;
                }
            }

            return result;
        }

        private static bool CheckInPublishListAny(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject != null)
            {
                try
                {
                    var helper = DTEHelper.Create(applicationObject);

                    var selectedFiles = helper.GetSelectedFilesFromListForPublish();

                    result = selectedFiles.Any(f => checker(f.FilePath));
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);

                    result = false;
                }
            }

            return result;
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceTextSingle), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceTextSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceTextSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListSingle(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceTextAny), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceTextAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceTextAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListAny(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceSingle), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListSingle(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceAny), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListAny(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusFilesToAdd(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusFilesToAdd), applicationObject, ActionBeforeQueryStatusFilesToAddInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusFilesToAddInternal(EnvDTE80.DTE2 applicationObject)
        {
            var helper = DTEHelper.Create(applicationObject);

            return helper.GetSelectedFilesAll(FileOperations.SupportsWebResourceType, true).Any();
        }

        internal static IEnumerable<SelectedFile> GetOpenedDocuments(DTEHelper helper)
        {
            return helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType);
        }

        internal static IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper)
        {
            return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false);
        }

        internal static IEnumerable<SelectedFile> GetSelectedFilesRecursive(DTEHelper helper)
        {
            return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true);
        }

        internal static IEnumerable<SelectedFile> GetSelectedFilesInListForPublish(DTEHelper helper)
        {
            var selectedFiles = helper.GetSelectedFilesFromListForPublish();

            if (selectedFiles.Any())
            {
                helper.ShowListForPublish(null);
            }
            else
            {
                helper.WriteToOutput(null, Properties.OutputStrings.PublishListIsEmpty);
            }

            return selectedFiles;
        }

        internal static void CorrectCommandNameForConnectionName(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand, string name)
        {
            var helper = DTEHelper.Create(applicationObject);
            var connectionData = helper.GetCurrentConnection();

            CorrectCommandNameForConnectionName(menuCommand, name, connectionData);
        }

        internal static void CorrectCommandNameForConnectionName(OleMenuCommand menuCommand, string name, ConnectionData connectionData)
        {
            if (connectionData != null)
            {
                name = string.Format(Properties.CommandNames.CommandNameWithConnectionFormat2, name, connectionData.Name);
            }

            menuCommand.Text = name;
        }

        internal static void ActionBeforeQueryStatusTextEditorProgramExists(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusTextEditorProgramExists), applicationObject, ActionBeforeQueryStatusTextEditorProgramExistsInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusTextEditorProgramExistsInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CommonConfiguration.Get().TextEditorProgramExists();
        }

        internal static void ActionBeforeQueryStatusConnectionIsNotReadOnly(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            bool visible = CacheValue(nameof(ActionBeforeQueryStatusConnectionIsNotReadOnly), applicationObject, ActionBeforeQueryStatusConnectionIsNotReadOnlyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusConnectionIsNotReadOnlyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return (ConnectionConfiguration.Get().CurrentConnectionData?.IsReadOnly).GetValueOrDefault() == false;
        }

        private static bool CacheValue(string cacheName, Func<bool> valueGetter)
        {
            bool result = false;

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(cacheName))
            {
                result = (bool)cache.Get(cacheName);
            }
            else
            {
                result = valueGetter();

                cache.Set(cacheName, result, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                });
            }

            return result;
        }

        private static bool CacheValue<T1>(string cacheName, T1 arg1, Func<T1, bool> valueGetter)
        {
            bool result = false;

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(cacheName))
            {
                result = (bool)cache.Get(cacheName);
            }
            else
            {
                result = valueGetter(arg1);

                cache.Set(cacheName, result, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                });
            }

            return result;
        }

        private static bool CacheValue<T1, T2>(string cacheName, T1 arg1, T2 arg2, Func<T1, T2, bool> valueGetter)
        {
            bool result = false;

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(cacheName))
            {
                result = (bool)cache.Get(cacheName);
            }
            else
            {
                result = valueGetter(arg1, arg2);

                cache.Set(cacheName, result, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                });
            }

            return result;
        }

        private static bool CacheValue<T1, T2, T3>(string cacheName, T1 arg1, T2 arg2, T3 arg3, Func<T1, T2, T3, bool> valueGetter)
        {
            bool result = false;

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(cacheName))
            {
                result = (bool)cache.Get(cacheName);
            }
            else
            {
                result = valueGetter(arg1, arg2, arg3);

                cache.Set(cacheName, result, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                });
            }

            return result;
        }
    }
}
