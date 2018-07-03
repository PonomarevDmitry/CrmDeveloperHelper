using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CompareController
    {
        private IWriteToOutputAndPublishList _iWriteToOutputAndPublishList = null;

        public CompareController(IWriteToOutputAndPublishList iWriteToOutputAndPublishList)
        {
            this._iWriteToOutputAndPublishList = iWriteToOutputAndPublishList;
        }

        #region Сравнение с веб-ресурсами.

        public async void ExecuteComparingFilesAndWebResources(List<SelectedFile> selectedFiles, ConnectionConfiguration crmConfig, bool withDetails)
        {
            this._iWriteToOutputAndPublishList.WriteToOutput("*********** Start Comparing Files and WebResources at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                {
                    this._iWriteToOutputAndPublishList.WriteToOutput("Checking Files Encoding");

                    CheckController.CheckingFilesEncoding(this._iWriteToOutputAndPublishList, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);
                    this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);
                    this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);
                }

                var compareResult = await ComparingFilesAndWebResourcesAsync(this._iWriteToOutputAndPublishList, selectedFiles, crmConfig, withDetails);
            }
            catch (Exception xE)
            {
                this._iWriteToOutputAndPublishList.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutputAndPublishList.WriteToOutput("*********** End Comparing Files and WebResources at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private static Task<
            Tuple<IOrganizationServiceExtented
                , List<Tuple<SelectedFile, WebResource>>
                , List<Tuple<SelectedFile, WebResource, ContentCopareResult>>
                >>
            ComparingFilesAndWebResourcesAsync(IWriteToOutput _iWriteToOutput, List<SelectedFile> selectedFiles, ConnectionConfiguration crmConfig, bool withDetails)
        {
            return Task.Run(() => ComparingFilesAndWebResources(_iWriteToOutput, selectedFiles, crmConfig, withDetails));
        }

        private static async Task<Tuple<IOrganizationServiceExtented
                , List<Tuple<SelectedFile, WebResource>>
                , List<Tuple<SelectedFile, WebResource, ContentCopareResult>>
                >> ComparingFilesAndWebResources(
            IWriteToOutput _iWriteToOutput
            , List<SelectedFile> selectedFiles
            , ConnectionConfiguration crmConfig
            , bool withDetails)
        {
            var dictFilesEqualByTextNotContent = new List<Tuple<SelectedFile, WebResource>>();
            var dictFilesNotEqualByText = new List<Tuple<SelectedFile, WebResource, ContentCopareResult>>();

            ConnectionData connectionData = crmConfig.CurrentConnectionData;

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return null;
            }

            _iWriteToOutput.WriteToOutput("Connect to CRM.");

            _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            bool isconnectionDataDirty = false;

            List<string> listNotExistsOnDisk = new List<string>();

            List<string> listNotFoundedInCRMNoLink = new List<string>();

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

                            var compare = ContentCoparerHelper.CompareByteArrays(selectedFile.Extension, arrayFile, arrayWebResource, withDetails);

                            if (compare.IsEqual)
                            {
                                tableEqualByText.AddLine(selectedFile.FriendlyFilePath, nameWebResource);

                                dictFilesEqualByTextNotContent.Add(Tuple.Create(selectedFile, webresource));
                            }
                            else
                            {
                                dictFilesNotEqualByText.Add(Tuple.Create(selectedFile, webresource, compare));

                                if (withDetails)
                                {
                                    string[] values = new string[]
                                    {
                                            selectedFile.FriendlyFilePath, nameWebResource
                                                , string.Format("+{0}", compare.Inserts)
                                                , string.Format("(+{0})", compare.InsertLength)
                                                , string.Format("-{0}", compare.Deletes)
                                                , string.Format("(-{0})", compare.DeleteLength)
                                    };

                                    tableDifferent.AddLine(values);

                                    if (compare.IsOnlyInserts)
                                    {
                                        tableDifferentOnlyInserts.AddLine(selectedFile.FriendlyFilePath
                                            , string.Format("+{0}", compare.Inserts)
                                            , string.Format("(+{0})", compare.InsertLength)
                                            );
                                    }

                                    if (compare.IsOnlyDeletes)
                                    {
                                        tableDifferentOnlyDeletes.AddLine(selectedFile.FriendlyFilePath
                                            , string.Format("-{0}", compare.Deletes)
                                            , string.Format("(-{0})", compare.DeleteLength)
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
                                    tableDifferent.AddLine(selectedFile.FriendlyFilePath, nameWebResource);
                                }
                            }
                        }
                    }
                    else
                    {
                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.FindByIdAsync(webId.Value);

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
                                    tableLastLinkEqualByContent.AddLine(selectedFile.FriendlyFilePath, nameWebResource);
                                }
                                else
                                {
                                    var arrayWebResource = Convert.FromBase64String(contentWebResource);

                                    var compare = ContentCoparerHelper.CompareByteArrays(selectedFile.Extension, arrayFile, arrayWebResource);

                                    if (compare.IsEqual)
                                    {
                                        listLastLinkEqualByText.AddLine(selectedFile.FriendlyFilePath, nameWebResource);

                                        dictFilesEqualByTextNotContent.Add(Tuple.Create(selectedFile, webresource));
                                    }
                                    else
                                    {
                                        dictFilesNotEqualByText.Add(Tuple.Create(selectedFile, webresource, compare));

                                        if (withDetails)
                                        {
                                            string[] values = new string[]
                                            {
                                                    selectedFile.FriendlyFilePath, nameWebResource
                                                        , string.Format("+{0}", compare.Inserts)
                                                        , string.Format("(+{0})", compare.InsertLength)
                                                        , string.Format("-{0}", compare.Deletes)
                                                        , string.Format("(-{0})", compare.DeleteLength)
                                            };

                                            tableLastLinkDifferent.AddLine(values);


                                            if (compare.IsOnlyInserts)
                                            {
                                                tableLastLinkDifferentOnlyInserts.AddLine(selectedFile.FriendlyFilePath, nameWebResource
                                                    , string.Format("+{0}", compare.Inserts)
                                                    , string.Format("(+{0})", compare.InsertLength)
                                                    );
                                            }

                                            if (compare.IsOnlyDeletes)
                                            {
                                                tableLastLinkDifferentOnlyDeletes.AddLine(selectedFile.FriendlyFilePath, nameWebResource
                                                    , string.Format("-{0}", compare.Deletes)
                                                    , string.Format("(-{0})", compare.DeleteLength)
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
                                            tableLastLinkDifferent.AddLine(selectedFile.FriendlyFilePath, nameWebResource);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                                listNotFoundedInCRMNoLink.Add(selectedFile.FriendlyFilePath);
                            }
                        }
                        else
                        {
                            listNotFoundedInCRMNoLink.Add(selectedFile.FriendlyFilePath);
                        }
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                crmConfig.Save();
            }

            var tabSpacer = "    ";

            if (tableDifferent.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content: {0}", tableDifferent.Count);

                tableDifferent.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableDifferentOnlyInserts.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content WITH ONLY INSERTS: {0}", tableDifferentOnlyInserts.Count);

                tableDifferentOnlyInserts.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableDifferentOnlyDeletes.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content WITH ONLY DELETES: {0}", tableDifferentOnlyDeletes.Count);

                tableDifferentOnlyDeletes.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableDifferentComplexChanges.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content WITH COMPLEX CHANGES: {0}", tableDifferentComplexChanges.Count);

                tableDifferentComplexChanges.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableDifferentMirror.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content WITH MIRROR CHANGES: {0}", tableDifferentMirror.Count);

                tableDifferentMirror.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableDifferentMirrorWithInserts.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content WITH MIRROR CHANGES AND INSERTS: {0}", tableDifferentMirrorWithInserts.Count);

                tableDifferentMirrorWithInserts.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableDifferentMirrorWithDeletes.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource are DIFFERENT by content WITH MIRROR CHANGES AND DELETES: {0}", tableDifferentMirrorWithDeletes.Count);

                tableDifferentMirrorWithDeletes.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (listNotFoundedInCRMNoLink.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM: {0}", listNotFoundedInCRMNoLink.Count);

                listNotFoundedInCRMNoLink.Sort();

                listNotFoundedInCRMNoLink.ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkDifferent.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT: {0}", tableLastLinkDifferent.Count);

                tableLastLinkDifferent.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }


            if (tableLastLinkDifferentOnlyInserts.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH ONLY INSERTS: {0}", tableLastLinkDifferentOnlyInserts.Count);

                tableLastLinkDifferentOnlyInserts.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkDifferentOnlyDeletes.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH ONLY DELETES: {0}", tableLastLinkDifferentOnlyDeletes.Count);

                tableLastLinkDifferentOnlyDeletes.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkDifferentComplexChanges.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH COMPLEX CHANGES: {0}", tableLastLinkDifferentComplexChanges.Count);

                tableLastLinkDifferentComplexChanges.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkDifferentMirror.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES: {0}", tableLastLinkDifferentMirror.Count);

                tableLastLinkDifferentMirror.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkDifferentMirrorWithInserts.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES AND INSERTS: {0}", tableLastLinkDifferentMirrorWithInserts.Count);

                tableLastLinkDifferentMirrorWithInserts.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkDifferentMirrorWithDeletes.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES AND DELETES: {0}", tableLastLinkDifferentMirrorWithDeletes.Count);

                tableLastLinkDifferentMirrorWithDeletes.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (listLastLinkEqualByText.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files EQUALS BY TEXT: {0}", listLastLinkEqualByText.Count);

                listLastLinkEqualByText.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableLastLinkEqualByContent.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link, files EQUALS BY CONTENT: {0}", tableLastLinkEqualByContent.Count);

                tableLastLinkEqualByContent.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT EXISTS: {0}", listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (tableEqualByText.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File and web-resource EQUALS BY TEXT: {0}", tableEqualByText.Count);

                tableEqualByText.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (countEqualByContent > 0)
            {
                if (countEqualByContent == selectedFiles.Count)
                {
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput("All files and web-resources EQUALS BY CONTENT: {0}", countEqualByContent);
                }
                else
                {
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput("File and web-resource EQUALS BY CONTENT: {0}", countEqualByContent);
                }
            }

            return Tuple.Create(service, dictFilesEqualByTextNotContent, dictFilesNotEqualByText);
        }

        #endregion Сравнение с веб-ресурсами.

        public static async Task<Tuple<IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>>> GetWebResourcesWithType(IWriteToOutput _iWriteToOutput, List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionConfiguration crmConfig)
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
                var compareResult = await FindFilesNotExistsInCrmAsync(_iWriteToOutput, selectedFiles, crmConfig);

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
            else if (openFilesType == OpenFilesType.EqualByText
                    || openFilesType == OpenFilesType.NotEqualByText
            )
            {
                var compareResult = await ComparingFilesAndWebResourcesAsync(_iWriteToOutput, selectedFiles, crmConfig, false);

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
            else if (openFilesType == OpenFilesType.WithInserts
                    || openFilesType == OpenFilesType.WithDeletes
                    || openFilesType == OpenFilesType.WithComplex
                    || openFilesType == OpenFilesType.WithMirror
                    || openFilesType == OpenFilesType.WithMirrorInserts
                    || openFilesType == OpenFilesType.WithMirrorDeletes
                    || openFilesType == OpenFilesType.WithMirrorComplex
                )
            {
                var compareResult = await CompareController.ComparingFilesAndWebResourcesAsync(_iWriteToOutput, selectedFiles, crmConfig, true);

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

            return Tuple.Create(service, filesToOpen);
        }

        #region Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async void ExecuteAddingIntoPublishListFilesByType(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            this._iWriteToOutputAndPublishList.WriteToOutput("*********** Start Adding into Publish List Files {0} at {1} *******************************************************", openFilesType.ToString(), DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                {
                    this._iWriteToOutputAndPublishList.WriteToOutput("Checking Files Encoding");

                    CheckController.CheckingFilesEncoding(this._iWriteToOutputAndPublishList, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);
                    this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);
                    this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);
                }

                await AddingIntoPublishListFilesByType(selectedFiles, openFilesType, crmConfig, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutputAndPublishList.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutputAndPublishList.WriteToOutput("*********** End Adding into Publish List Files {0} at {1} *******************************************************", openFilesType.ToString(), DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task AddingIntoPublishListFilesByType(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            ConnectionData connectionData = crmConfig.CurrentConnectionData;

            if (connectionData == null)
            {
                this._iWriteToOutputAndPublishList.WriteToOutput("No current CRM Connection.");
                return;
            }

            var compareResult = await GetWebResourcesWithType(this._iWriteToOutputAndPublishList, selectedFiles, openFilesType, crmConfig);

            var listFilesToDifference = compareResult.Item2;

            if (listFilesToDifference.Any())
            {
                this._iWriteToOutputAndPublishList.AddToListForPublish(listFilesToDifference.Select(f => f.Item1).OrderBy(f => f.FriendlyFilePath));
            }
            else
            {
                this._iWriteToOutputAndPublishList.WriteToOutput("No files for adding into Publish List.");
            }
        }

        #endregion Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async void ExecuteComparingFilesWithWrongEncoding(List<SelectedFile> selectedFiles, ConnectionConfiguration crmConfig, bool withDetails)
        {
            this._iWriteToOutputAndPublishList.WriteToOutput("*********** Start Comparing Files with Wrong Encoding and WebResources at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                CheckController.CheckingFilesEncoding(this._iWriteToOutputAndPublishList, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                this._iWriteToOutputAndPublishList.WriteToOutput(string.Empty);

                await ComparingFilesAndWebResourcesAsync(this._iWriteToOutputAndPublishList, filesWithoutUTF8Encoding, crmConfig, withDetails);
            }
            catch (Exception xE)
            {
                this._iWriteToOutputAndPublishList.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutputAndPublishList.WriteToOutput("*********** End Comparing Files with Wrong Encoding and WebResources at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        #region Запуск Organization Comparer.

        public void ExecuteOrganizationComparer(ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            this._iWriteToOutputAndPublishList.WriteToOutput("*********** Start Showing Organization Comparer at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutputAndPublishList, crmConfig, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutputAndPublishList.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutputAndPublishList.WriteToOutput("*********** End Showing Organization Comparer at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        #endregion Запуск Organization Comparer.

        private static Task<
            Tuple<IOrganizationServiceExtented
                , List<SelectedFile>
                , List<Tuple<SelectedFile, WebResource>>
                >>
            FindFilesNotExistsInCrmAsync(IWriteToOutput _iWriteToOutput, List<SelectedFile> selectedFiles, ConnectionConfiguration crmConfig)
        {
            return Task.Run(() => FindFilesNotExistsInCrm(_iWriteToOutput, selectedFiles, crmConfig));
        }

        private static async Task<Tuple<IOrganizationServiceExtented
                , List<SelectedFile>
                , List<Tuple<SelectedFile, WebResource>>
                >> FindFilesNotExistsInCrm(
            IWriteToOutput _iWriteToOutput
            , List<SelectedFile> selectedFiles
            , ConnectionConfiguration crmConfig)
        {
            ConnectionData connectionData = crmConfig.CurrentConnectionData;

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return null;
            }

            _iWriteToOutput.WriteToOutput("Connect to CRM.");

            _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

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
                            webresource = await webResourceRepository.FindByIdAsync(webId.Value, new ColumnSet(true));

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
                crmConfig.Save();
            }

            var tabSpacer = "    ";

            if (listNotFoundedInCrmNoLink.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM: {0}", listNotFoundedInCrmNoLink.Count);

                listNotFoundedInCrmNoLink.Sort();

                listNotFoundedInCrmNoLink.ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (listNotFoundedInCrmWithLink.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT FOUNDED in CRM, but has Last Link: {0}", listNotFoundedInCrmWithLink.Count);

                FormatTextTableHandler tableLastLinkDifferent = new FormatTextTableHandler();
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName");

                listNotFoundedInCrmWithLink.ForEach(i => tableLastLinkDifferent.AddLine(i.Item1.FriendlyFilePath, i.Item2.Name));

                tableLastLinkDifferent.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("File NOT EXISTS: {0}", listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => _iWriteToOutput.WriteToOutput(tabSpacer + item));
            }

            if (listNotFoundedInCrmNoLink.Count + listNotFoundedInCrmWithLink.Count == 0)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
                _iWriteToOutput.WriteToOutput("No files not exists in Crm");
            }

            return Tuple.Create(service, listNotFoundedInCrmNoLink, listNotFoundedInCrmWithLink);
        }
    }
}