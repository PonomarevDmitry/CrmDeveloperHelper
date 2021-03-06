﻿using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class TranslationRepository
    {
        private const string _fileNameDefaultTranslation = "DefaultTranslation.xml";

        private const string _fileNameFieldTranslation = "FieldTranslation.xml";

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

        private static async Task<Translation> GetDefaultTranslationFromCache(Guid connectionId, IOrganizationServiceExtented service)
        {
            var repository = new TranslationRepository(service);

            return await GetTranslationFromCacheOrCreate(connectionId, _cacheDefault, _fileNameDefaultTranslation, repository.GetTranslationsAsync);
        }

        private static async Task<Translation> GetTranslationFromCacheOrCreate(
            Guid connectionId
            , ConcurrentDictionary<Guid, Translation> cacheTranslation
            , string fileNameTranslation
            , Func<Task<Translation>> translationGetter
        )
        {
            if (cacheTranslation.ContainsKey(connectionId))
            {
                return cacheTranslation[connectionId];
            }

            string folderPath = FileOperations.GetTranslationLocalCacheFolder(connectionId);

            string filePath = Path.Combine(folderPath, fileNameTranslation);

            Translation translation = Translation.Get(filePath);

            if (translation != null)
            {
                if (!cacheTranslation.ContainsKey(connectionId))
                {
                    cacheTranslation.TryAdd(connectionId, translation);
                }

                return translation;
            }

            translation = await translationGetter();

            if (translation != null)
            {
                Translation.Save(filePath, translation);

                if (!cacheTranslation.ContainsKey(connectionId))
                {
                    cacheTranslation.TryAdd(connectionId, translation);
                }
            }

            return translation;
        }

        public static Task<Translation> GetFieldTranslationFromCacheAsync(Guid connectionId, IOrganizationServiceExtented service)
        {
            return Task.Run(() => GetFieldTranslationFromCache(connectionId, service));
        }

        private static Task<Translation> GetFieldTranslationFromCache(Guid connectionId, IOrganizationServiceExtented service)
        {
            var sdkMessageRequestRepository = new SdkMessageRequestRepository(service);
            var translationRepository = new TranslationRepository(service);

            return GetTranslationFromCacheOrCreate(connectionId, _cacheField, _fileNameFieldTranslation, () => GetFieldTranslation(sdkMessageRequestRepository, translationRepository));
        }

        private static async Task<Translation> GetFieldTranslation(SdkMessageRequestRepository sdkMessageRequestRepository, TranslationRepository translationRepository)
        {
            var request = await sdkMessageRequestRepository.FindByRequestNameAsync(SdkMessageRequest.Instances.ExportFieldTranslationRequest, ColumnSetInstances.None);

            if (request == null)
            {
                return null;
            }

            var result = await translationRepository.GetFieldTranslationsAsync();

            return result;
        }

        private Task<Translation> GetTranslationsAsync()
        {
            return Task.Run(() => GetTranslations());
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
                xml = ContentComparerHelper.RemoveDiacritics(xml);

                XElement doc = XElement.Parse(xml);

                result = new Translation();

                FillTranslation(result, doc);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }

        private Task<Translation> GetFieldTranslationsAsync()
        {
            return Task.Run(() => GetFieldTranslations());
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
                xml = ContentComparerHelper.RemoveDiacritics(xml);

                result = new Translation();

                XElement doc = XElement.Parse(xml);

                FillTranslation(result, doc);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
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
            if (string.Equals(element.Name.LocalName, "Worksheet", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsRow(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Row", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsCell(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Cell", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsData(XElement element)
        {
            if (string.Equals(element.Name.LocalName, "Data", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static bool IsDisplayStrings(XElement element)
        {
            var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Id", StringComparison.InvariantCultureIgnoreCase));

            if (attr != null)
            {
                return string.Equals(attr.Value, "Display Strings", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        private static bool IsLocalizedLabels(XElement element)
        {
            var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Id", StringComparison.InvariantCultureIgnoreCase));

            if (attr != null)
            {
                return string.Equals(attr.Value, "Localized Labels", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}