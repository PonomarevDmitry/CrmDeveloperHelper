using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CompareController : BaseController<IWriteToOutputAndPublishList>
    {
        public CompareController(IWriteToOutputAndPublishList iWriteToOutputAndPublishList)
            : base(iWriteToOutputAndPublishList)
        {
        }

        #region Сравнение с веб-ресурсами.

        public async Task ExecuteComparingFilesAndWebResources(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            string operation = string.Format(Properties.OperationNames.ComparingFilesAndWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                var compareResult = await ComparingFilesAndWebResourcesAsync(this._iWriteToOutput, selectedFiles, connectionData, withDetails);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private static Task<
            Tuple<IOrganizationServiceExtented
                , List<Tuple<SelectedFile, WebResource>>
                , List<Tuple<SelectedFile, WebResource, ContentCopareResult>>
                >>
            ComparingFilesAndWebResourcesAsync(IWriteToOutput iWriteToOutput, List<SelectedFile> selectedFiles, ConnectionData connectionData, bool withDetails)
        {
            return Task.Run(async () => await ComparingFilesAndWebResources(iWriteToOutput, selectedFiles, connectionData, withDetails));
        }

        private static async Task<Tuple<IOrganizationServiceExtented
                , List<Tuple<SelectedFile, WebResource>>
                , List<Tuple<SelectedFile, WebResource, ContentCopareResult>>
                >> ComparingFilesAndWebResources(
            IWriteToOutput iWriteToOutput
            , List<SelectedFile> selectedFiles
            , ConnectionData connectionData
            , bool withDetails)
        {
            var service = await QuickConnection.ConnectAndWriteToOutputAsync(iWriteToOutput, connectionData);

            if (service == null)
            {
                return null;
            }

            bool isconnectionDataDirty = false;

            List<string> listNotExistsOnDisk = new List<string>();

            List<string> listNotFoundedInCRMNoLink = new List<string>();

            var dictFilesEqualByTextNotContent = new List<Tuple<SelectedFile, WebResource>>();
            var dictFilesNotEqualByText = new List<Tuple<SelectedFile, WebResource, ContentCopareResult>>();

            int countEqualByContent = 0;

            FormatTextTableHandler tableEqualByText = new FormatTextTableHandler();
            tableEqualByText.SetHeader("FriendlyFilePath", "WebResourceName");

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();

            if (withDetails)
            {
                tableDifferent.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");
            }
            else
            {
                tableDifferent.SetHeader("FriendlyFilePath", "WebResourceName");
            }

            FormatTextTableHandler tableDifferentOnlyInserts = new FormatTextTableHandler();
            tableDifferentOnlyInserts.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)");

            FormatTextTableHandler tableDifferentOnlyDeletes = new FormatTextTableHandler();
            tableDifferentOnlyDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentComplexChanges = new FormatTextTableHandler();
            tableDifferentComplexChanges.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentMirror = new FormatTextTableHandler();
            tableDifferentMirror.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentMirrorWithInserts = new FormatTextTableHandler();
            tableDifferentMirrorWithInserts.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentMirrorWithDeletes = new FormatTextTableHandler();
            tableDifferentMirrorWithDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkEqualByContent = new FormatTextTableHandler();
            tableLastLinkEqualByContent.SetHeader("FriendlyFilePath", "WebResourceName");

            FormatTextTableHandler listLastLinkEqualByText = new FormatTextTableHandler();
            listLastLinkEqualByText.SetHeader("FriendlyFilePath", "WebResourceName");

            FormatTextTableHandler tableLastLinkDifferent = new FormatTextTableHandler();
            if (withDetails)
            {
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");
            }
            else
            {
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName");
            }

            FormatTextTableHandler tableLastLinkDifferentOnlyInserts = new FormatTextTableHandler();
            tableLastLinkDifferentOnlyInserts.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)");

            FormatTextTableHandler tableLastLinkDifferentOnlyDeletes = new FormatTextTableHandler();
            tableLastLinkDifferentOnlyDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentComplexChanges = new FormatTextTableHandler();
            tableLastLinkDifferentComplexChanges.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentMirror = new FormatTextTableHandler();
            tableLastLinkDifferentMirror.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentMirrorWithInserts = new FormatTextTableHandler();
            tableLastLinkDifferentMirrorWithInserts.SetHeader("FriendlyFilePath", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentMirrorWithDeletes = new FormatTextTableHandler();
            tableLastLinkDifferentMirrorWithDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names
                    , new ColumnSet(
                        WebResource.Schema.EntityPrimaryIdAttribute
                        , WebResource.Schema.Attributes.name
                        , WebResource.Schema.Attributes.webresourcetype
                        , WebResource.Schema.Attributes.content
                    ));

                foreach (var selectedFile in gr)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        listNotExistsOnDisk.Add(selectedFile.FilePath);
                        continue;
                    }

                    string urlShowDifference = string.Format("{0}:///{1}?ConnectionId={2}", UrlCommandFilter.PrefixShowDifference, selectedFile.FilePath.Replace('\\', '/'), connectionData.ConnectionId.ToString());

                    string name = selectedFile.FriendlyFilePath.ToLower();

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, name, gr.Key);

                    if (webresource != null)
                    {
                        // Запоминается файл
                        isconnectionDataDirty = true;
                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        var contentWebResource = webresource.Content ?? string.Empty;

                        var arrayFile = File.ReadAllBytes(selectedFile.FilePath);

                        var contentFile = Convert.ToBase64String(arrayFile);

                        if (string.Equals(contentFile, contentWebResource))
                        {
                            countEqualByContent++;
                        }
                        else
                        {
                            var arrayWebResource = Convert.FromBase64String(contentWebResource);

                            var nameWebResource = webresource.Name;

                            var compare = ContentComparerHelper.CompareByteArrays(selectedFile.Extension, arrayFile, arrayWebResource, withDetails);

                            if (compare.IsEqual)
                            {
                                tableEqualByText.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource);

                                dictFilesEqualByTextNotContent.Add(Tuple.Create(selectedFile, webresource));
                            }
                            else
                            {
                                dictFilesNotEqualByText.Add(Tuple.Create(selectedFile, webresource, compare));

                                if (withDetails)
                                {
                                    string[] values = new string[]
                                    {
                                            selectedFile.UrlFriendlyFilePath, nameWebResource
                                                , string.Format("+{0}", compare.Inserts)
                                                , string.Format("(+{0})", compare.InsertLength)
                                                , string.Format("-{0}", compare.Deletes)
                                                , string.Format("(-{0})", compare.DeleteLength)
                                                , urlShowDifference
                                    };

                                    tableDifferent.AddLine(values);

                                    if (compare.IsOnlyInserts)
                                    {
                                        tableDifferentOnlyInserts.AddLine(selectedFile.UrlFriendlyFilePath
                                            , string.Format("+{0}", compare.Inserts)
                                            , string.Format("(+{0})", compare.InsertLength)
                                            , urlShowDifference
                                        );
                                    }

                                    if (compare.IsOnlyDeletes)
                                    {
                                        tableDifferentOnlyDeletes.AddLine(selectedFile.UrlFriendlyFilePath
                                            , string.Format("-{0}", compare.Deletes)
                                            , string.Format("(-{0})", compare.DeleteLength)
                                            , urlShowDifference
                                        );
                                    }

                                    if (compare.IsComplexChanges)
                                    {
                                        tableDifferentComplexChanges.AddLine(values);
                                    }

                                    if (compare.IsMirror)
                                    {
                                        tableDifferentMirror.AddLine(values);
                                    }

                                    if (compare.IsMirrorWithInserts)
                                    {
                                        tableDifferentMirrorWithInserts.AddLine(values);
                                    }

                                    if (compare.IsMirrorWithDeletes)
                                    {
                                        tableDifferentMirrorWithDeletes.AddLine(values);
                                    }
                                }
                                else
                                {
                                    tableDifferent.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource, urlShowDifference);
                                }
                            }
                        }
                    }
                    else
                    {
                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.GetByIdAsync(webId.Value);

                            if (webresource != null)
                            {
                                // Запоминается файл
                                isconnectionDataDirty = true;
                                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                                var contentWebResource = webresource.Content ?? string.Empty;
                                var nameWebResource = webresource.Name;

                                var arrayFile = File.ReadAllBytes(selectedFile.FilePath);

                                var contentFile = Convert.ToBase64String(arrayFile);

                                if (string.Equals(contentFile, contentWebResource))
                                {
                                    tableLastLinkEqualByContent.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource);
                                }
                                else
                                {
                                    var arrayWebResource = Convert.FromBase64String(contentWebResource);

                                    var compare = ContentComparerHelper.CompareByteArrays(selectedFile.Extension, arrayFile, arrayWebResource);

                                    if (compare.IsEqual)
                                    {
                                        listLastLinkEqualByText.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource);

                                        dictFilesEqualByTextNotContent.Add(Tuple.Create(selectedFile, webresource));
                                    }
                                    else
                                    {
                                        dictFilesNotEqualByText.Add(Tuple.Create(selectedFile, webresource, compare));

                                        if (withDetails)
                                        {
                                            string[] values = new string[]
                                            {
                                                    selectedFile.UrlFriendlyFilePath, nameWebResource
                                                        , string.Format("+{0}", compare.Inserts)
                                                        , string.Format("(+{0})", compare.InsertLength)
                                                        , string.Format("-{0}", compare.Deletes)
                                                        , string.Format("(-{0})", compare.DeleteLength)
                                                        , urlShowDifference
                                            };

                                            tableLastLinkDifferent.AddLine(values);


                                            if (compare.IsOnlyInserts)
                                            {
                                                tableLastLinkDifferentOnlyInserts.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource
                                                    , string.Format("+{0}", compare.Inserts)
                                                    , string.Format("(+{0})", compare.InsertLength)
                                                    , urlShowDifference
                                                    );
                                            }

                                            if (compare.IsOnlyDeletes)
                                            {
                                                tableLastLinkDifferentOnlyDeletes.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource
                                                    , string.Format("-{0}", compare.Deletes)
                                                    , string.Format("(-{0})", compare.DeleteLength)
                                                    , urlShowDifference
                                                    );
                                            }

                                            if (compare.IsComplexChanges)
                                            {
                                                tableLastLinkDifferentComplexChanges.AddLine(values);
                                            }

                                            if (compare.IsMirror)
                                            {
                                                tableLastLinkDifferentMirror.AddLine(values);
                                            }

                                            if (compare.IsMirrorWithInserts)
                                            {
                                                tableLastLinkDifferentMirrorWithInserts.AddLine(values);
                                            }

                                            if (compare.IsMirrorWithDeletes)
                                            {
                                                tableLastLinkDifferentMirrorWithDeletes.AddLine(values);
                                            }
                                        }
                                        else
                                        {
                                            tableLastLinkDifferent.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource, urlShowDifference);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                                listNotFoundedInCRMNoLink.Add(selectedFile.UrlFriendlyFilePath);
                            }
                        }
                        else
                        {
                            listNotFoundedInCRMNoLink.Add(selectedFile.UrlFriendlyFilePath);
                        }
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            var tabSpacer = "    ";

            if (tableDifferent.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content: {0}", tableDifferent.Count);

                tableDifferent.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentOnlyInserts.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH ONLY INSERTS: {0}", tableDifferentOnlyInserts.Count);

                tableDifferentOnlyInserts.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentOnlyDeletes.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH ONLY DELETES: {0}", tableDifferentOnlyDeletes.Count);

                tableDifferentOnlyDeletes.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentComplexChanges.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH COMPLEX CHANGES: {0}", tableDifferentComplexChanges.Count);

                tableDifferentComplexChanges.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentMirror.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH MIRROR CHANGES: {0}", tableDifferentMirror.Count);

                tableDifferentMirror.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentMirrorWithInserts.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH MIRROR CHANGES AND INSERTS: {0}", tableDifferentMirrorWithInserts.Count);

                tableDifferentMirrorWithInserts.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentMirrorWithDeletes.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH MIRROR CHANGES AND DELETES: {0}", tableDifferentMirrorWithDeletes.Count);

                tableDifferentMirrorWithDeletes.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotFoundedInCRMNoLink.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM: {0}", listNotFoundedInCRMNoLink.Count);

                listNotFoundedInCRMNoLink.Sort();

                listNotFoundedInCRMNoLink.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferent.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT: {0}", tableLastLinkDifferent.Count);

                tableLastLinkDifferent.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }


            if (tableLastLinkDifferentOnlyInserts.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH ONLY INSERTS: {0}", tableLastLinkDifferentOnlyInserts.Count);

                tableLastLinkDifferentOnlyInserts.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentOnlyDeletes.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH ONLY DELETES: {0}", tableLastLinkDifferentOnlyDeletes.Count);

                tableLastLinkDifferentOnlyDeletes.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentComplexChanges.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH COMPLEX CHANGES: {0}", tableLastLinkDifferentComplexChanges.Count);

                tableLastLinkDifferentComplexChanges.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentMirror.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES: {0}", tableLastLinkDifferentMirror.Count);

                tableLastLinkDifferentMirror.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentMirrorWithInserts.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES AND INSERTS: {0}", tableLastLinkDifferentMirrorWithInserts.Count);

                tableLastLinkDifferentMirrorWithInserts.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentMirrorWithDeletes.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES AND DELETES: {0}", tableLastLinkDifferentMirrorWithDeletes.Count);

                tableLastLinkDifferentMirrorWithDeletes.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listLastLinkEqualByText.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files EQUALS BY TEXT: {0}", listLastLinkEqualByText.Count);

                listLastLinkEqualByText.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkEqualByContent.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files EQUALS BY CONTENT: {0}", tableLastLinkEqualByContent.Count);

                tableLastLinkEqualByContent.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableEqualByText.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File and web-resource EQUALS BY TEXT: {0}", tableEqualByText.Count);

                tableEqualByText.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (countEqualByContent > 0)
            {
                if (countEqualByContent == selectedFiles.Count)
                {
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, "All files and web-resources EQUALS BY CONTENT: {0}", countEqualByContent);
                }
                else
                {
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, "File and web-resource EQUALS BY CONTENT: {0}", countEqualByContent);
                }
            }

            return Tuple.Create(service, dictFilesEqualByTextNotContent, dictFilesNotEqualByText);
        }

        #endregion Сравнение с веб-ресурсами.

        public static async Task<Tuple<IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>>> GetWebResourcesWithType(IWriteToOutput iWriteToOutput, List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionData connectionData)
        {
            IOrganizationServiceExtented service = null;

            TupleList<SelectedFile, WebResource> filesToOpen = new TupleList<SelectedFile, WebResource>();

            if (openFilesType == OpenFilesType.All)
            {
                foreach (var item in selectedFiles)
                {
                    filesToOpen.Add(item, null);
                }
            }
            else if (openFilesType == OpenFilesType.NotExistsInCrmWithoutLink
                    || openFilesType == OpenFilesType.NotExistsInCrmWithLink
            )
            {
                var compareResult = await FindFilesNotExistsInCrmAsync(iWriteToOutput, selectedFiles, connectionData);

                if (compareResult != null)
                {
                    service = compareResult.Item1;

                    if (openFilesType == OpenFilesType.NotExistsInCrmWithoutLink)
                    {
                        filesToOpen.AddRange(compareResult.Item2.Select(f => Tuple.Create(f, (WebResource)null)));
                    }
                    else if (openFilesType == OpenFilesType.NotExistsInCrmWithLink)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                }
            }
            else if (openFilesType == OpenFilesType.EqualByText
                    || openFilesType == OpenFilesType.NotEqualByText
            )
            {
                var compareResult = await ComparingFilesAndWebResourcesAsync(iWriteToOutput, selectedFiles, connectionData, false);

                if (compareResult != null)
                {
                    service = compareResult.Item1;

                    if (openFilesType == OpenFilesType.EqualByText)
                    {
                        filesToOpen.AddRange(compareResult.Item2);
                    }
                    else if (openFilesType == OpenFilesType.NotEqualByText)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                }
            }
            else if (openFilesType == OpenFilesType.WithInserts
                    || openFilesType == OpenFilesType.WithDeletes
                    || openFilesType == OpenFilesType.WithComplex
                    || openFilesType == OpenFilesType.WithMirror
                    || openFilesType == OpenFilesType.WithMirrorInserts
                    || openFilesType == OpenFilesType.WithMirrorDeletes
                    || openFilesType == OpenFilesType.WithMirrorComplex
                )
            {
                var compareResult = await CompareController.ComparingFilesAndWebResourcesAsync(iWriteToOutput, selectedFiles, connectionData, true);

                if (compareResult != null)
                {
                    service = compareResult.Item1;

                    if (openFilesType == OpenFilesType.WithInserts)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsOnlyInserts).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithDeletes)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsOnlyDeletes).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithComplex)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsComplexChanges).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirror)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirror).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorInserts)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirrorWithInserts).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorDeletes)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirrorWithDeletes).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorComplex)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirrorWithComplex).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                }
            }

            return Tuple.Create(service, filesToOpen);
        }

        #region Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async Task ExecuteAddingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            string operation = string.Format(Properties.OperationNames.AddingIntoPublishListFilesFormat2, connectionData?.Name, openFilesType.ToString());

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                await AddingIntoPublishListFilesByType(connectionData, commonConfig, selectedFiles, openFilesType);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task AddingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            var compareResult = await GetWebResourcesWithType(this._iWriteToOutput, selectedFiles, openFilesType, connectionData);

            if (compareResult == null || compareResult.Item1 == null)
            {
                return;
            }

            var listFilesToDifference = compareResult.Item2;

            if (listFilesToDifference.Any())
            {
                this._iWriteToOutput.AddToListForPublish(listFilesToDifference.Select(f => f.Item1).OrderBy(f => f.FriendlyFilePath));
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for adding to Publish List.");
            }
        }

        #endregion Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async Task ExecuteComparingFilesWithWrongEncoding(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            string operation = string.Format(Properties.OperationNames.ComparingFilesWithWrongEncodingAndWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncoding(connectionData, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

                await ComparingFilesAndWebResourcesAsync(this._iWriteToOutput, filesWithoutUTF8Encoding, connectionData, withDetails);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        #region Запуск Organization Comparer.

        public void ExecuteOrganizationComparer(ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ShowingOrganizationComparer);

            try
            {
                WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, crmConfig, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ShowingOrganizationComparer);
            }
        }

        #endregion Запуск Organization Comparer.

        private static Task<
            Tuple<IOrganizationServiceExtented
                , List<SelectedFile>
                , List<Tuple<SelectedFile, WebResource>>
                >>
            FindFilesNotExistsInCrmAsync(IWriteToOutput iWriteToOutput, List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            return Task.Run(async () => await FindFilesNotExistsInCrm(iWriteToOutput, selectedFiles, connectionData));
        }

        private static async Task<Tuple<IOrganizationServiceExtented
                , List<SelectedFile>
                , List<Tuple<SelectedFile, WebResource>>
                >> FindFilesNotExistsInCrm(
            IWriteToOutput iWriteToOutput
            , List<SelectedFile> selectedFiles
            , ConnectionData connectionData)
        {
            var service = await QuickConnection.ConnectAndWriteToOutputAsync(iWriteToOutput, connectionData);

            if (service == null)
            {
                return null;
            }

            bool isconnectionDataDirty = false;

            var listNotFoundedInCrmNoLink = new List<SelectedFile>();
            var listNotFoundedInCrmWithLink = new List<Tuple<SelectedFile, WebResource>>();

            List<string> listNotExistsOnDisk = new List<string>();

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names
                    , new ColumnSet(
                        WebResource.Schema.EntityPrimaryIdAttribute
                        , WebResource.Schema.Attributes.name
                        , WebResource.Schema.Attributes.webresourcetype
                    ));

                foreach (var selectedFile in gr)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        listNotExistsOnDisk.Add(selectedFile.FilePath);
                        continue;
                    }

                    string name = selectedFile.FriendlyFilePath.ToLower();

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, name, gr.Key);

                    if (webresource != null)
                    {
                        // Запоминается файл
                        isconnectionDataDirty = true;
                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);
                    }
                    else
                    {
                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.GetByIdAsync(webId.Value, new ColumnSet(true));

                            if (webresource != null)
                            {
                                listNotFoundedInCrmWithLink.Add(Tuple.Create(selectedFile, webresource));

                                isconnectionDataDirty = true;
                                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);
                            }
                            else
                            {
                                connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                                listNotFoundedInCrmNoLink.Add(selectedFile);
                            }
                        }
                        else
                        {
                            listNotFoundedInCrmNoLink.Add(selectedFile);
                        }
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            var tabSpacer = "    ";

            if (listNotFoundedInCrmNoLink.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM: {0}", listNotFoundedInCrmNoLink.Count);

                listNotFoundedInCrmNoLink.Sort();

                listNotFoundedInCrmNoLink.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotFoundedInCrmWithLink.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link: {0}", listNotFoundedInCrmWithLink.Count);

                FormatTextTableHandler tableLastLinkDifferent = new FormatTextTableHandler();
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName");

                listNotFoundedInCrmWithLink.ForEach(i => tableLastLinkDifferent.AddLine(i.Item1.FriendlyFilePath, i.Item2.Name));

                tableLastLinkDifferent.GetFormatedLines(true).ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotFoundedInCrmNoLink.Count + listNotFoundedInCrmWithLink.Count == 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "No files not exists in Crm");
            }

            return Tuple.Create(service, listNotFoundedInCrmNoLink, listNotFoundedInCrmWithLink);
        }
    }
}