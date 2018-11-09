using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class TranslationRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        private TranslationRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        private static ConcurrentDictionary<Guid, Translation> _cacheDefault = new ConcurrentDictionary<Guid, Translation>();

        private static ConcurrentDictionary<Guid, Translation> _cacheField = new ConcurrentDictionary<Guid, Translation>();

        public static Task<Translation> GetDefaultTranslationFromCacheAsync(Guid connectionId, IOrganizationServiceExtented service)
        {
            return Task.Run(() => GetDefaultTranslationFromCache(connectionId, service));
        }

        private static Translation GetDefaultTranslationFromCache(Guid connectionId, IOrganizationServiceExtented service)
        {
            if (_cacheDefault.ContainsKey(connectionId))
            {
                return _cacheDefault[connectionId];
            }

            var fileName = string.Format("DefaultTranslation.{0}.xml", connectionId.ToString());

            var trans = FileOperations.GetTranslationLocalCache(fileName);

            if (trans != null)
            {
                if (!_cacheDefault.ContainsKey(connectionId))
                {
                    _cacheDefault.TryAdd(connectionId, trans);
                }

                return trans;
            }

            var rep = new TranslationRepository(service);
            var result = rep.GetTranslations();

            if (result != null)
            {
                FileOperations.SaveTranslationLocalCache(fileName, result);

                if (!_cacheDefault.ContainsKey(connectionId))
                {
                    _cacheDefault.TryAdd(connectionId, result);
                }
            }

            return result;
        }

        public static Task<Translation> GetFieldTranslationFromCacheAsync(Guid connectionId, IOrganizationServiceExtented service)
        {
            return Task.Run(async () => await GetFieldTranslationFromCache(connectionId, service));
        }

        private static async Task<Translation> GetFieldTranslationFromCache(Guid connectionId, IOrganizationServiceExtented service)
        {
            if (_cacheField.ContainsKey(connectionId))
            {
                return _cacheField[connectionId];
            }

            var fileName = string.Format("FieldTranslation.{0}.xml", connectionId.ToString());

            var trans = FileOperations.GetTranslationLocalCache(fileName);

            if (trans != null)
            {
                if (!_cacheField.ContainsKey(connectionId))
                {
                    _cacheField.TryAdd(connectionId, trans);
                }

                return trans;
            }

            var repository = new SdkMessageRequestRepository(service);

            var request = await repository.FindByRequestNameAsync(SdkMessageRequest.Instances.ExportFieldTranslationRequest, new ColumnSet(false));

            if (request == null)
            {
                return null;
            }

            var rep = new TranslationRepository(service);
            var result = rep.GetFieldTranslations();

            if (result != null)
            {
                if (!_cacheField.ContainsKey(connectionId))
                {
                    _cacheField.TryAdd(connectionId, result);
                }

                FileOperations.SaveTranslationLocalCache(fileName, result);
            }

            return result;
        }

        public static void ClearCache()
        {
            _cacheField.Clear();
            _cacheDefault.Clear();

            FileOperations.ClearTranslationLocalCache();
        }

        private Translation GetTranslations()
        {
            Translation result = null;

            try
            {
                var request = new ExportTranslationRequest()
                {
                    SolutionName = "Default",
                };

                var response = (ExportTranslationResponse)_service.Execute(request);

                var traslationZipFile = response.ExportTranslationFile;

                var traslationFileBytes = FileOperations.UnzipCrmTranslations(traslationZipFile, "/CrmTranslations.xml");

                string xml = Encoding.UTF8.GetString(traslationFileBytes);
                xml = ContentCoparerHelper.RemoveDiacritics(xml);

                XElement doc = XElement.Parse(xml);

                result = new Translation();

                FillTranslation(result, doc);
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        private Translation GetFieldTranslations()
        {
            Translation result = null;

            try
            {
                var request = new ExportFieldTranslationRequest();

                var response = (ExportFieldTranslationResponse)_service.Execute(request);

                var traslationZipFile = response.ExportTranslationFile;

                var traslationFileBytes = FileOperations.UnzipCrmTranslations(traslationZipFile, "/CrmFieldTranslations.xml");

                string xml = Encoding.UTF8.GetString(traslationFileBytes);
                xml = ContentCoparerHelper.RemoveDiacritics(xml);

                result = new Translation();

                XElement doc = XElement.Parse(xml);

                FillTranslation(result, doc);
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        private static void FillTranslation(Translation result, XElement doc)
        {
            var worksheetDisplaString = doc.Descendants().Where(IsWorksheet).FirstOrDefault(IsDisplayStrings);

            if (worksheetDisplaString != null)
            {
                FillDisplayStrings(result, worksheetDisplaString);
            }

            var worksheetLocalizedLabels = doc.Descendants().Where(IsWorksheet).FirstOrDefault(IsLocalizedLabels);

            if (worksheetLocalizedLabels != null)
            {
                FillLocalizedLabels(result, worksheetLocalizedLabels);
            }
        }

        private static void FillDisplayStrings(Translation result, XElement worksheetDisplaString)
        {
            var rows = worksheetDisplaString.Descendants().Where(IsRow);

            if (!rows.Any())
            {
                return;
            }

            var firstRow = rows.FirstOrDefault();

            List<int> languages = new List<int>();

            {
                var cells = firstRow.Descendants().Where(IsCell).Skip(2);

                foreach (var cell in cells)
                {
                    var value = GetCellNumber(cell);

                    languages.Add(value);
                }
            }

            foreach (var row in rows.Skip(1))
            {
                var cells = row.Descendants().Where(IsCell);

                var entityName = GetCellString(cells.FirstOrDefault());

                var stringKey = GetCellString(cells.Skip(1).FirstOrDefault());

                var displayString = new TranslationDisplayString(entityName, stringKey);

                result.DisplayStrings.Add(displayString);

                for (int i = 0; i < languages.Count; i++)
                {
                    var label = GetCellString(cells.Skip(2 + i).FirstOrDefault());

                    displayString.Labels.Add(new LabelString() { LanguageCode = languages[i], Value = label });
                }
            }
        }

        private static void FillLocalizedLabels(Translation result, XElement worksheetLocalizedLabels)
        {
            var rows = worksheetLocalizedLabels.Descendants().Where(IsRow);

            if (!rows.Any())
            {
                return;
            }

            var firstRow = rows.FirstOrDefault();

            List<int> languages = new List<int>();

            {
                var cells = firstRow.Descendants().Where(IsCell).Skip(3);

                foreach (var cell in cells)
                {
                    var value = GetCellNumber(cell);

                    languages.Add(value);
                }
            }

            foreach (var row in rows.Skip(1))
            {
                var cells = row.Descendants().Where(IsCell);

                var entityName = GetCellString(cells.FirstOrDefault());

                var objectId = GetCellGuid(cells.Skip(1).FirstOrDefault());

                var columnName = GetCellString(cells.Skip(2).FirstOrDefault());

                var localizedLabel = new Nav.Common.VSPackages.CrmDeveloperHelper.Model.LocalizedLabel(entityName, objectId, columnName);

                result.LocalizedLabels.Add(localizedLabel);

                for (int i = 0; i < languages.Count; i++)
                {
                    var label = GetCellString(cells.Skip(3 + i).FirstOrDefault());

                    localizedLabel.Labels.Add(new LabelString() { LanguageCode = languages[i], Value = label });
                }
            }
        }

        private static int GetCellNumber(XElement cell)
        {
            if (cell == null)
            {
                return 0;
            }

            var coll = cell.Descendants().Where(IsData);

            var data = coll.Count() == 1 ? coll.SingleOrDefault() : null;

            if (data != null)
            {
                if (int.TryParse(data.Value, out int temp))
                {
                    return temp;
                }
            }

            return 0;
        }

        private static Guid GetCellGuid(XElement cell)
        {
            if (cell == null)
            {
                return Guid.Empty;
            }

            var coll = cell.Descendants().Where(IsData);

            var data = coll.Count() == 1 ? coll.SingleOrDefault() : null;

            if (data != null)
            {
                if (Guid.TryParse(data.Value, out Guid temp))
                {
                    return temp;
                }
            }

            return Guid.Empty;
        }

        private static string GetCellString(XElement cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }



            var coll = cell.Descendants().Where(IsData);

            var data = coll.Count() == 1 ? coll.SingleOrDefault() : null;

            if (data != null)
            {
                return data.Value;
            }

            return string.Empty;
        }

        private static bool IsWorksheet(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Worksheet", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsRow(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Row", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsCell(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Cell", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsData(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Data", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsDisplayStrings(XElement element)
        {
            var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Id", StringComparison.OrdinalIgnoreCase));

            if (attr != null)
            {
                return string.Equals(attr.Value, "Display Strings", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private static bool IsLocalizedLabels(XElement element)
        {
            var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Id", StringComparison.OrdinalIgnoreCase));

            if (attr != null)
            {
                return string.Equals(attr.Value, "Localized Labels", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}