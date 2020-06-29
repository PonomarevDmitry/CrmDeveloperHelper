using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public string GetSelectedText()
        {
            string result = string.Empty;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Selection != null
                && ApplicationObject.ActiveWindow.Selection is EnvDTE.TextSelection textSelection
            )
            {
                result = textSelection.Text;
            }
            else if (ApplicationObject.ActiveWindow.Object != null
                && ApplicationObject.ActiveWindow.Object is EnvDTE.OutputWindow outputWindow
                && outputWindow.ActivePane != null
                && outputWindow.ActivePane.TextDocument != null
                && outputWindow.ActivePane.TextDocument.Selection != null
            )
            {
                result = outputWindow.ActivePane.TextDocument.Selection.Text;
            }
            else if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.OfType<EnvDTE.SelectedItem>().Take(2).ToList();

                if (items.Count == 1)
                {
                    var item = items[0];

                    if (item.Project != null)
                    {
                        result = item.Project.Name;
                    }

                    if (string.IsNullOrEmpty(result) && item.ProjectItem != null && item.ProjectItem.FileCount > 0)
                    {
                        result = Path.GetFileName(item.ProjectItem.FileNames[1]).Split('.').FirstOrDefault();
                    }

                    if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(item.Name))
                    {
                        result = item.Name;
                    }
                }
            }

            result = result ?? string.Empty;

            return result;
        }

        public IEnumerable<SelectedFile> GetOpenedFileInCodeWindow(Func<string, bool> checkerFunction)
        {
            return GetOpenedFileInCodeWindow(checkerFunction, true);
        }

        public IEnumerable<SelectedFile> GetOpenedFileInCodeWindow(Func<string, bool> checkerFunction, bool saveIfNeeded)
        {
            string solutionDirectoryPath = GetSolutionDirectory();

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
            )
            {
                string path = ApplicationObject.ActiveWindow.Document.FullName;

                if (checkerFunction(path))
                {
                    if (saveIfNeeded && !ApplicationObject.ActiveWindow.Document.Saved)
                    {
                        ApplicationObject.ActiveWindow.Document.Save();
                    }

                    yield return new SelectedFile(path, solutionDirectoryPath)
                    {
                        Document = ApplicationObject.ActiveWindow.Document,
                    };
                }
            }
        }

        public EnvDTE.Document GetOpenedDocumentInCodeWindow(Func<string, bool> checkerFunction)
        {
            return GetOpenedDocumentInCodeWindow(checkerFunction, true);
        }

        public EnvDTE.Document GetOpenedDocumentInCodeWindow(Func<string, bool> checkerFunction, bool saveIfNeeded)
        {
            EnvDTE.Document result = null;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
            )
            {
                string path = ApplicationObject.ActiveWindow.Document.FullName;

                if (checkerFunction(path))
                {
                    if (saveIfNeeded && !ApplicationObject.ActiveWindow.Document.Saved)
                    {
                        ApplicationObject.ActiveWindow.Document.Save();
                    }

                    result = ApplicationObject.ActiveWindow.Document;
                }
            }

            return result;
        }

        public bool TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType)
        {
            entityName = string.Empty;
            formId = Guid.Empty;
            formType = 0;

            var document = this.GetOpenedDocumentInCodeWindow(FileOperations.SupportsJavaScriptType);

            if (document == null)
            {
                return false;
            }

            var objTextDoc = document.Object(nameof(EnvDTE.TextDocument));
            if (objTextDoc != null
                && objTextDoc is EnvDTE.TextDocument textDocument
            )
            {
                string fileText = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                if (CommonHandlers.GetLinkedSystemForm(fileText, out entityName, out formId, out formType))
                {
                    return true;
                }
            }

            entityName = string.Empty;
            formId = Guid.Empty;
            formType = 0;

            return false;
        }

        public bool TryGetLinkedEntityName(out string entityName)
        {
            entityName = string.Empty;

            var document = this.GetOpenedDocumentInCodeWindow(FileOperations.SupportsJavaScriptType);

            if (document == null)
            {
                return false;
            }

            var objTextDoc = document.Object(nameof(EnvDTE.TextDocument));
            if (objTextDoc != null
                && objTextDoc is EnvDTE.TextDocument textDocument
            )
            {
                string fileText = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                if (CommonHandlers.GetLinkedEntityName(fileText, out entityName))
                {
                    return true;
                }
            }

            entityName = string.Empty;

            return false;
        }

        public IEnumerable<SelectedFile> GetOpenedDocuments(Func<string, bool> checkerFunction)
        {
            return GetOpenedDocuments(checkerFunction, true);
        }

        public IEnumerable<SelectedFile> GetOpenedDocuments(Func<string, bool> checkerFunction, bool saveIfNeeded)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
            )
            {
                string solutionDirectoryPath = GetSolutionDirectory();

                foreach (var document in ApplicationObject.Documents.OfType<EnvDTE.Document>())
                {
                    if (document.ActiveWindow == null
                        || document.ActiveWindow.Type != EnvDTE.vsWindowType.vsWindowTypeDocument
                        || document.ActiveWindow.Visible == false
                    )
                    {
                        continue;
                    }

                    if (ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindTextView)
                        || ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindCode)
                    )
                    {
                        string path = document.FullName;

                        if (checkerFunction(path))
                        {
                            if (saveIfNeeded && !document.Saved)
                            {
                                document.Save();
                            }

                            yield return new SelectedFile(path, solutionDirectoryPath)
                            {
                                Document = document,
                            };
                        }
                    }
                }
            }
        }

        public IEnumerable<EnvDTE.Document> GetOpenedDocumentsAsDocument(Func<string, bool> checkerFunction)
        {
            return GetOpenedDocumentsAsDocument(checkerFunction, true);
        }

        public IEnumerable<EnvDTE.Document> GetOpenedDocumentsAsDocument(Func<string, bool> checkerFunction, bool saveIfNeeded)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
            )
            {
                foreach (var document in ApplicationObject.Documents.OfType<EnvDTE.Document>())
                {
                    if (document.ActiveWindow == null
                        || document.ActiveWindow.Type != EnvDTE.vsWindowType.vsWindowTypeDocument
                        || document.ActiveWindow.Visible == false
                    )
                    {
                        continue;
                    }

                    if (ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindTextView)
                        || ApplicationObject.ItemOperations.IsFileOpen(document.FullName, EnvDTE.Constants.vsViewKindCode)
                    )
                    {
                        string path = document.FullName;

                        if (checkerFunction(path))
                        {
                            if (saveIfNeeded && !document.Saved)
                            {
                                document.Save();
                            }

                            yield return document;
                        }
                    }
                }
            }
        }

        public IEnumerable<SelectedFile> GetSelectedFilesInSolutionExplorer(Func<string, bool> checkerFunction, bool recursive)
        {
            return GetSelectedFilesInSolutionExplorer(checkerFunction, recursive, true);
        }

        public IEnumerable<SelectedFile> GetSelectedFilesInSolutionExplorer(Func<string, bool> checkerFunction, bool recursive, bool saveIfNeeded)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                string solutionDirectoryPath = GetSolutionDirectory();

                var items = ApplicationObject.SelectedItems.OfType<EnvDTE.SelectedItem>().ToList();

                HashSet<string> hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in items)
                {
                    if (item.ProjectItem != null)
                    {
                        string path = item.ProjectItem.FileNames[1];

                        if (!string.IsNullOrEmpty(path)
                            && checkerFunction(path)
                            && hash.Add(path)
                        )
                        {
                            if (saveIfNeeded
                                && item.ProjectItem.Document != null
                                && !item.ProjectItem.Document.Saved
                            )
                            {
                                item.ProjectItem.Document.Save();
                            }

                            yield return new SelectedFile(path, solutionDirectoryPath)
                            {
                                Document = item.ProjectItem.Document,
                            };
                        }

                        if (recursive)
                        {
                            foreach (var subItem in FillHashSubProjectItems(hash, item.ProjectItem.ProjectItems, checkerFunction, saveIfNeeded))
                            {
                                yield return subItem;
                            }
                        }
                    }

                    if (recursive && item.Project != null)
                    {
                        foreach (var subItem in FillHashSubProjectItems(hash, item.Project.ProjectItems, checkerFunction, saveIfNeeded))
                        {
                            yield return subItem;
                        }
                    }
                }
            }
        }

        private IEnumerable<SelectedFile> FillHashSubProjectItems(HashSet<string> hash, EnvDTE.ProjectItems projectItems, Func<string, bool> checkerFunction, bool saveIfNeeded)
        {
            if (projectItems != null)
            {
                string solutionDirectoryPath = GetSolutionDirectory();

                foreach (EnvDTE.ProjectItem projItem in projectItems)
                {
                    string path = projItem.FileNames[1];

                    if (!string.IsNullOrEmpty(path)
                        && checkerFunction(path)
                        && hash.Add(path)
                    )
                    {
                        if (saveIfNeeded && projItem.Document != null && !projItem.Document.Saved)
                        {
                            projItem.Document.Save();
                        }

                        yield return new SelectedFile(path, solutionDirectoryPath)
                        {
                            Document = projItem.Document,
                        };
                    }

                    foreach (var subItem in FillHashSubProjectItems(hash, projItem.ProjectItems, checkerFunction, saveIfNeeded))
                    {
                        yield return subItem;
                    }

                    if (projItem.SubProject != null)
                    {
                        foreach (var subItem in FillHashSubProjectItems(hash, projItem.SubProject.ProjectItems, checkerFunction, saveIfNeeded))
                        {
                            yield return subItem;
                        }
                    }
                }
            }
        }

        public EnvDTE.ProjectItem GetSingleSelectedProjectItemInSolutionExplorer(Func<string, bool> checkerFunction)
        {
            EnvDTE.ProjectItem result = null;

            if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.OfType<EnvDTE.SelectedItem>().ToList();

                var filtered = items.Where(a =>
                {
                    try
                    {
                        return a.ProjectItem != null && checkerFunction(a.Name);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);

                        return false;
                    }
                });

                if (filtered.Count() == 1)
                {
                    result = filtered.FirstOrDefault()?.ProjectItem;
                }
            }

            return result;
        }

        public IEnumerable<EnvDTE.ProjectItem> GetSelectedProjectItemsInSolutionExplorer(Func<string, bool> checkerFunction, bool recursive)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.OfType<EnvDTE.SelectedItem>().ToList();

                HashSet<string> hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in items)
                {
                    if (item.ProjectItem != null
                        && item.ProjectItem.ContainingProject != null
                    )
                    {
                        string path = item.ProjectItem.FileNames[1];

                        if (!string.IsNullOrEmpty(path)
                            && checkerFunction(path)
                            && !hash.Contains(path)
                        )
                        {
                            if (hash.Add(path))
                            {
                                yield return item.ProjectItem;
                            }
                        }

                        if (recursive)
                        {
                            foreach (var subItem in GetSubProjectItems(hash, item.ProjectItem.ProjectItems, checkerFunction))
                            {
                                yield return subItem;
                            }
                        }
                    }

                    if (recursive && item.Project != null)
                    {
                        foreach (var subItem in GetSubProjectItems(hash, item.Project.ProjectItems, checkerFunction))
                        {
                            yield return subItem;
                        }
                    }
                }
            }
        }

        private IEnumerable<EnvDTE.ProjectItem> GetSubProjectItems(HashSet<string> hash, EnvDTE.ProjectItems projectItems, Func<string, bool> checkerFunction)
        {
            if (projectItems == null)
            {
                yield break;
            }

            foreach (EnvDTE.ProjectItem projItem in projectItems)
            {
                string path = projItem.FileNames[1];

                if (checkerFunction(path))
                {
                    if (!hash.Contains(path))
                    {
                        if (hash.Add(path))
                        {
                            yield return projItem;
                        }
                    }
                }

                foreach (var subItem in GetSubProjectItems(hash, projItem.ProjectItems, checkerFunction))
                {
                    yield return subItem;
                }

                if (projItem.SubProject != null)
                {
                    foreach (var subItem in GetSubProjectItems(hash, projItem.SubProject.ProjectItems, checkerFunction))
                    {
                        yield return subItem;
                    }
                }
            }
        }

        public IEnumerable<SelectedFile> GetSelectedFilesAll(Func<string, bool> checkerFunction, bool recursive)
        {
            return GetSelectedFilesAll(checkerFunction, recursive, true);
        }

        /// <summary>
        /// Файл в окне редактирования или выделенные файлы в Solution Explorer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectedFile> GetSelectedFilesAll(Func<string, bool> checkerFunction, bool recursive, bool saveIfNeeded)
        {
            foreach (var item in GetOpenedFileInCodeWindow(checkerFunction, saveIfNeeded))
            {
                yield return item;
            }

            foreach (var item in GetSelectedFilesInSolutionExplorer(checkerFunction, recursive, saveIfNeeded))
            {
                yield return item;
            }
        }

        public EnvDTE.SelectedItem GetSelectedProjectItem()
        {
            EnvDTE.SelectedItem result = null;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.OfType<EnvDTE.SelectedItem>().Take(2).ToList();

                if (items.Count == 1)
                {
                    result = items[0];
                }
            }

            return result;
        }

        public EnvDTE.Project GetSelectedProject()
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.OfType<EnvDTE.SelectedItem>();

                if (items.Count() == 1)
                {
                    var item = items.First();

                    if (item.Project != null)
                    {
                        return item.Project;
                    }
                }
            }

            return null;
        }

        public IEnumerable<EnvDTE.Project> GetSelectedProjects()
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject
                    .SelectedItems
                    .OfType<EnvDTE.SelectedItem>()
                    .Where(e => e.Project != null && !string.IsNullOrEmpty(e.Project.Name))
                    .Select(e => e.Project)
                    .Distinct()
                    ;

                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<SelectedFile> GetSelectedFilesFromListForPublish()
        {
            string solutionDirectoryPath = GetSolutionDirectory();

            foreach (var path in _ListForPublish)
            {
                yield return new SelectedFile(path, solutionDirectoryPath);
            }
        }

        public IEnumerable<SelectedFile> GetSelectedFilesFromListForPublish(Func<string, bool> checkerFunction)
        {
            string solutionDirectoryPath = GetSolutionDirectory();

            foreach (var path in _ListForPublish)
            {
                if (checkerFunction(path))
                {
                    yield return new SelectedFile(path, solutionDirectoryPath);
                }
            }
        }

        public ConnectionData GetOutputWindowConnection()
        {
            if (ApplicationObject.ActiveWindow.Object != null
                && ApplicationObject.ActiveWindow.Object is EnvDTE.OutputWindow outputWindow
                && outputWindow.ActivePane != null
            )
            {
                var outputWindowPane = outputWindow.ActivePane;

                if (!string.IsNullOrEmpty(outputWindowPane.Guid)
                    && Guid.TryParse(outputWindowPane.Guid, out var paneGuid)
                )
                {
                    var connectionConfig = ConnectionConfiguration.Get();

                    var connectionData = connectionConfig.Connections.FirstOrDefault(c => c.ConnectionId == paneGuid);

                    return connectionData;
                }
            }

            return null;
        }
    }
}