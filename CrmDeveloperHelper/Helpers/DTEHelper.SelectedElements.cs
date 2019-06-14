using EnvDTE;
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
                && ApplicationObject.ActiveWindow.Selection is TextSelection
                )
            {
                result = ((TextSelection)ApplicationObject.ActiveWindow.Selection).Text;
            }
            else if (ApplicationObject.ActiveWindow.Object != null
                && ApplicationObject.ActiveWindow.Object is OutputWindow)
            {
                var outputWindow = (OutputWindow)ApplicationObject.ActiveWindow.Object;

                if (outputWindow.ActivePane != null
                    && outputWindow.ActivePane.TextDocument != null
                    && outputWindow.ActivePane.TextDocument.Selection != null)
                {
                    result = outputWindow.ActivePane.TextDocument.Selection.Text;
                }
            }
            else if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().Take(2).ToList();

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
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
                )
            {
                string path = ApplicationObject.ActiveWindow.Document.FullName;

                if (checkerFunction(path))
                {
                    yield return new SelectedFile(path, GetFriendlyPath(path));
                }
            }
        }

        public EnvDTE.Document GetOpenedDocumentInCodeWindow(Func<string, bool> checkerFunction)
        {
            EnvDTE.Document result = null;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
                && ApplicationObject.ActiveWindow.Document != null
                )
            {
                string path = ApplicationObject.ActiveWindow.Document.FullName;

                if (checkerFunction(path))
                {
                    result = ApplicationObject.ActiveWindow.Document;
                }
            }

            return result;
        }

        public IEnumerable<SelectedFile> GetOpenedDocuments(Func<string, bool> checkerFunction)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
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
                            yield return new SelectedFile(path, GetFriendlyPath(path));
                        }
                    }
                }
            }
        }

        public IEnumerable<EnvDTE.Document> GetOpenedDocumentsAsDocument(Func<string, bool> checkerFunction)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeDocument
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
                            yield return document;
                        }
                    }
                }
            }
        }

        public IEnumerable<SelectedFile> GetSelectedFilesInSolutionExplorer(Func<string, bool> checkerFunction, bool recursive)
        {
            if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

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
                            yield return new SelectedFile(path, GetFriendlyPath(path))
                            {
                                Document = item.ProjectItem.Document,
                            };
                        }

                        if (recursive)
                        {
                            foreach (var subItem in FillHashSubProjectItems(hash, item.ProjectItem.ProjectItems, checkerFunction))
                            {
                                yield return subItem;
                            }
                        }
                    }

                    if (recursive && item.Project != null)
                    {
                        foreach (var subItem in FillHashSubProjectItems(hash, item.Project.ProjectItems, checkerFunction))
                        {
                            yield return subItem;
                        }
                    }
                }
            }
        }

        private IEnumerable<SelectedFile> FillHashSubProjectItems(HashSet<string> hash, ProjectItems projectItems, Func<string, bool> checkerFunction)
        {
            if (projectItems != null)
            {
                foreach (ProjectItem projItem in projectItems)
                {
                    string path = projItem.FileNames[1];

                    if (!string.IsNullOrEmpty(path)
                          && checkerFunction(path)
                          && hash.Add(path)
                    )
                    {
                        yield return new SelectedFile(path, GetFriendlyPath(path))
                        {
                            Document = projItem.Document,
                        };
                    }

                    foreach (var subItem in FillHashSubProjectItems(hash, projItem.ProjectItems, checkerFunction))
                    {
                        yield return subItem;
                    }

                    if (projItem.SubProject != null)
                    {
                        foreach (var subItem in FillHashSubProjectItems(hash, projItem.SubProject.ProjectItems, checkerFunction))
                        {
                            yield return subItem;
                        }
                    }
                }
            }
        }

        public ProjectItem GetSingleSelectedProjectItemInSolutionExplorer(Func<string, bool> checkerFunction)
        {
            ProjectItem result = null;

            if (ApplicationObject.ActiveWindow != null
               && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
               && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

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

        public IEnumerable<ProjectItem> GetSelectedProjectItemsInSolutionExplorer(Func<string, bool> checkerFunction, bool recursive)
        {
            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null
            )
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().ToList();

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

        public IEnumerable<ProjectItem> GetSubProjectItems(HashSet<string> hash, ProjectItems projectItems, Func<string, bool> checkerFunction)
        {
            if (projectItems == null)
            {
                yield break;
            }

            foreach (ProjectItem projItem in projectItems)
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

        /// <summary>
        /// Файл в окне редактирования или выделенные файлы в Solution Explorer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectedFile> GetSelectedFilesAll(Func<string, bool> checkerFunction, bool recursive)
        {
            foreach (var item in GetOpenedFileInCodeWindow(checkerFunction))
            {
                yield return item;
            }

            foreach (var item in GetSelectedFilesInSolutionExplorer(checkerFunction, recursive))
            {
                yield return item;
            }
        }

        private string GetFriendlyPath(string filePath)
        {
            string solutionPath = ApplicationObject?.Solution?.FullName;

            if (!string.IsNullOrEmpty(solutionPath))
            {
                return filePath.Replace(Path.GetDirectoryName(solutionPath), string.Empty);
            }

            return filePath;
        }

        public SelectedItem GetSelectedProjectItem()
        {
            SelectedItem result = null;

            if (ApplicationObject.ActiveWindow != null
                && ApplicationObject.ActiveWindow.Type == vsWindowType.vsWindowTypeSolutionExplorer
                && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject.SelectedItems.Cast<SelectedItem>().Take(2).ToList();

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
                   && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject.SelectedItems.Cast<EnvDTE.SelectedItem>();

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
                   && ApplicationObject.SelectedItems != null)
            {
                var items = ApplicationObject
                    .SelectedItems
                    .Cast<EnvDTE.SelectedItem>()
                    .Where(e => e.Project != null)
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
            foreach (var item in _ListForPublish.Select(path => new SelectedFile(path, GetFriendlyPath(path))))
            {
                yield return item;
            }
        }

        public ConnectionData GetOutputWindowConnection()
        {
            if (ApplicationObject.ActiveWindow.Object != null
                && ApplicationObject.ActiveWindow.Object is OutputWindow outputWindow
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