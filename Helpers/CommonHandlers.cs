using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class CommonHandlers
    {
        internal static bool CheckActiveDocumentExtension(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
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
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckActiveDocumentIsXmlWithRoot(EnvDTE80.DTE2 applicationObject, out XElement doc, params string[] rootNames)
        {
            doc = null;

            if (rootNames == null || !rootNames.Any())
            {
                return false;
            }

            bool result = false;

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
                            && objTextDoc is TextDocument textDocument
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
                                            result = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckActiveDocumentIsXmlWithRootWithAttribute(
            EnvDTE80.DTE2 applicationObject
            , XName attributeName
            , out XAttribute attribute
            , params string[] rootNames
        )
        {
            bool result = false;
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
                            && objTextDoc is TextDocument textDocument
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

                                            result = attribute != null;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckActiveDocumentIsFetchOrGrid(EnvDTE80.DTE2 applicationObject)
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

                    if (FileOperations.SupportsXmlType(file))
                    {
                        var objTextDoc = applicationObject.ActiveWindow.Document.Object("TextDocument");
                        if (objTextDoc != null
                            && objTextDoc is TextDocument textDocument
                            )
                        {
                            string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                            if (!string.IsNullOrEmpty(text))
                            {
                                if (ContentCoparerHelper.TryParseXml(text, out var doc))
                                {
                                    result = string.Equals(doc.Name.ToString(), "fetch", StringComparison.InvariantCultureIgnoreCase)
                                        || string.Equals(doc.Name.ToString(), "grid", StringComparison.InvariantCultureIgnoreCase)
                                        || string.Equals(doc.Name.ToString(), "savedquery", StringComparison.InvariantCultureIgnoreCase)
                                        ;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckOpenedDocumentsExtension(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

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
                                result = true;

                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(ex);

                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        private static bool CheckInSolutionExplorerSingle(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
                )
            {
                try
                {
                    var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                    var count = items.Count(a =>
                    {
                        try
                        {
                            string file = a.Name.ToLower();

                            return checker(file);
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(ex);

                            return false;
                        }
                    });

                    result = count == 1;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckInSolutionExplorerAny(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
                )
            {
                try
                {
                    var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                    result = items.Any(a =>
                    {
                        try
                        {
                            string file = a.Name.ToLower();

                            return checker(file);
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(ex);

                            return false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckInSolutionExplorerRecursive(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null
                )
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>().ToList();

                items.ForEach(item =>
                {
                    if (item.ProjectItem != null)
                    {
                        result |= FillListProjectItems(item.ProjectItem.ProjectItems, checker);
                    }

                    if (item.Project != null)
                    {
                        result |= FillListProjectItems(item.Project.ProjectItems, checker);
                    }
                });
            }

            return result;
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

        internal static void ActionBeforeQueryStatusActiveDocumentWebResourceText(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentWebResource(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsWebResourceType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentXml(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsXmlType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(IServiceProviderOwner command, OleMenuCommand menuCommand, out XElement doc, params string[] rootNames)
        {
            doc = null;
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentIsXmlWithRoot(applicationObject, out doc, rootNames);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
            IServiceProviderOwner command
            , OleMenuCommand menuCommand
            , XName attributeName
            , out XAttribute attribute
            , params string[] rootNames
        )
        {
            bool visible = false;
            attribute = null;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentIsXmlWithRootWithAttribute(applicationObject, attributeName, out attribute, rootNames);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentIsFetchOrGrid(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentIsFetchOrGrid(applicationObject);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentReport(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsReportType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentCSharp(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckActiveDocumentExtension(applicationObject, FileOperations.SupportsCSharpType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusActiveDocumentContainingProject(IServiceProviderOwner command, OleMenuCommand menuCommand, Func<string, bool> checker)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                var helper = DTEHelper.Create(applicationObject);

                var document = helper.GetOpenedDocumentInCodeWindow(checker);

                if (document != null
                    && document.ProjectItem != null
                    && document.ProjectItem.ContainingProject != null
                    )
                {
                    visible = true;
                }
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(IServiceProviderOwner command, OleMenuCommand menuCommand, Func<string, bool> checker)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                var helper = DTEHelper.Create(applicationObject);

                EnvDTE.SelectedItem item = helper.GetSingleSelectedItemInSolutionExplorer(checker);

                if (item != null)
                {
                    if (item.ProjectItem != null && item.ProjectItem.ContainingProject != null)
                    {
                        visible = true;
                    }
                }
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(IServiceProviderOwner command, OleMenuCommand menuCommand, Func<string, bool> checker)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                var helper = DTEHelper.Create(applicationObject);

                var list = helper.GetListSelectedItemInSolutionExplorer(checker);

                visible = list.Any(item => item.ProjectItem != null && item.ProjectItem.ContainingProject != null);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsWebResourceText(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsWebResource(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsWebResourceType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsReport(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsReportType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusOpenedDocumentsCSharp(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckOpenedDocumentsExtension(applicationObject, FileOperations.SupportsCSharpType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }




        internal static void ActionBeforeQueryStatusSolutionExplorerCSharpSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsCSharpType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerCSharpAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsCSharpType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsCSharpType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsWebResourceType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsWebResourceType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsWebResourceType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerReportSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerSingle(applicationObject, FileOperations.SupportsReportType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerReportAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerAny(applicationObject, FileOperations.SupportsReportType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerReportRecursive(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerRecursive(applicationObject, FileOperations.SupportsReportType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActiveSolutionExplorerFolderSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                if (applicationObject.ActiveWindow != null
                   && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                   && applicationObject.SelectedItems != null)
                {
                    var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                    if (items.Count() == 1)
                    {
                        var item = items.First();

                        visible = true;
                    }
                }
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActiveSolutionExplorerProjectSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerProjectSingle(applicationObject);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActiveSolutionExplorerProjectAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInSolutionExplorerProjectAny(applicationObject);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static bool CheckInSolutionExplorerProjectSingle(EnvDTE80.DTE2 applicationObject)
        {
            bool visible = false;

            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null)
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

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

        internal static bool CheckInSolutionExplorerProjectAny(EnvDTE80.DTE2 applicationObject)
        {
            if (applicationObject.ActiveWindow != null
                && applicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && applicationObject.SelectedItems != null)
            {
                var items = applicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

                return items.All(i => i.Project != null) && items.Any(i => i.Project != null);
            }

            return false;
        }

        private static bool CheckInPublishListSingle(EnvDTE80.DTE2 applicationObject, IServiceProvider serviceProvider, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject != null)
            {
                try
                {
                    var helper = DTEHelper.Create(applicationObject);

                    var selectedFiles = helper.GetSelectedFilesFromListForPublish();

                    var count = selectedFiles.Count(f => checker(f.FilePath));

                    result = count == 1;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static bool CheckInPublishListAny(EnvDTE80.DTE2 applicationObject, IServiceProvider serviceProvider, Func<string, bool> checker)
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
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = false;
                }
            }

            return result;
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInPublishListSingle(applicationObject, command.ServiceProvider, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInPublishListAny(applicationObject, command.ServiceProvider, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceSingle(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInPublishListSingle(applicationObject, command.ServiceProvider, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = CheckInPublishListAny(applicationObject, command.ServiceProvider, FileOperations.SupportsWebResourceTextType);
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusFilesToAdd(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                var helper = DTEHelper.Create(applicationObject);

                visible = helper.GetSelectedFilesAll(FileOperations.SupportsWebResourceType, true).Any();
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static List<SelectedFile> GetOpenedDocuments(DTEHelper helper)
        {
            return helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType);
        }

        internal static List<SelectedFile> GetSelectedFiles(DTEHelper helper)
        {
            return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false);
        }

        internal static List<SelectedFile> GetSelectedFilesRecursive(DTEHelper helper)
        {
            return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true);
        }

        internal static List<SelectedFile> GetSelectedFilesInListForPublish(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish();
            }
            else
            {
                helper.WriteToOutput(Properties.OutputStrings.PublishListIsEmpty);
            }

            return selectedFiles;
        }

        internal static void CorrectCommandNameForConnectionName(IServiceProviderOwner command, OleMenuCommand menuCommand, string name)
        {
            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                var helper = DTEHelper.Create(applicationObject);
                var connection = helper.GetCurrentConnectionName();

                if (!string.IsNullOrEmpty(connection))
                {
                    name = string.Format(Properties.CommandNames.CommandNameWithConnectionFormat2, name, connection);
                }
            }

            menuCommand.Text = name;
        }

        internal static void ActionBeforeQueryStatusTextEditorProgramExists(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = CommonConfiguration.Get().TextEditorProgramExists();

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        internal static void ActionBeforeQueryStatusConnectionIsNotReadOnly(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            bool visible = false;

            if (command.ServiceProvider.GetService(typeof(EnvDTE.DTE)) is EnvDTE80.DTE2 applicationObject)
            {
                visible = (ConnectionConfiguration.Get().CurrentConnectionData?.IsReadOnly).GetValueOrDefault() == false;
            }

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
