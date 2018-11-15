using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    /// <summary>
    /// Менеджер для обновления веб-ресурсов и публикации
    /// </summary>
    public class PublishManager
    {
        private IWriteToOutput _iWriteToOutput;

        private readonly IOrganizationServiceExtented _Service;

        /// <summary>
        /// Конструктор для менеджера
        /// </summary>
        /// <param name="outputWindow"></param>
        /// <param name="service"></param>
        public PublishManager(IWriteToOutput outputWindow, IOrganizationServiceExtented service)
        {
            _iWriteToOutput = outputWindow;
            _Service = service;
        }

        /// <summary>
        /// Коллекция разрешенных веб-ресурсов подготовленных для публикции
        /// </summary>
        private readonly Dictionary<Guid, ElementForPublish> _Elements = new Dictionary<Guid, ElementForPublish>();

        /// <summary>
        /// Добавление веб-ресурса для публикации
        /// </summary>
        /// <param name="webResourceId"></param>
        /// <param name="webResourceContent"></param>
        /// <param name="name"></param>
        public void Add(ElementForPublish element)
        {
            if (!_Elements.ContainsKey(element.WebResource.Id))
            {
                _Elements.Add(element.WebResource.Id, element);
            }
        }

        /// <summary>
        /// Выполнение обновления содержания и публикация.
        /// </summary>
        public void UpdateContentAndPublish()
        {
            if (_Elements.Count == 0)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NothingToPublish);
                return;
            }

            this._iWriteToOutput.WriteToOutput("Updating WebResources content...");

            UpdateContent();

            this._iWriteToOutput.WriteToOutput("Publishing WebResources...");

            PublishActionsRepository repository = new PublishActionsRepository(_Service);
            repository.PublishWebResources(_Elements.Keys);

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("WebResourceName", "WebResourceType");

            foreach (var element in this._Elements.Values.OrderBy(e => e.WebResource.Name))
            {
                element.WebResource.FormattedValues.TryGetValue(WebResource.Schema.Attributes.webresourcetype, out var webresourcetype);
                table.AddLine(element.WebResource.Name, webresourcetype);
            }

            this._iWriteToOutput.WriteToOutput("Published web-resources: {0}", this._Elements.Values.Count);

            var lines = table.GetFormatedLines(false);
            lines.ForEach(item => _iWriteToOutput.WriteToOutput("    {0}", item));
        }

        /// <summary>
        /// Обновление контента веб-ресурсов
        /// </summary>
        /// <returns></returns>
        private void UpdateContent()
        {
            var list = _Elements.Values.OrderBy(element => element.SelectedFile.FriendlyFilePath).ToList();

            FormatTextTableHandler tableUpdated = new FormatTextTableHandler();
            tableUpdated.SetHeader("FileName", "WebResourceName", "WebResourceType");

            FormatTextTableHandler tableNotCustomizable = new FormatTextTableHandler();
            tableNotCustomizable.SetHeader("FileName", "WebResourceName", "WebResourceType");

            FormatTextTableHandler tableEqual = new FormatTextTableHandler();
            tableEqual.SetHeader("FileName", "WebResourceName", "WebResourceType");

            foreach (var element in list)
            {
                var contentFile = Convert.ToBase64String(File.ReadAllBytes(element.SelectedFile.FilePath));

                var contentWebResource = element.WebResource.Content ?? string.Empty;

                element.WebResource.FormattedValues.TryGetValue(WebResource.Schema.Attributes.webresourcetype, out var webresourcetype);

                if (contentFile == contentWebResource)
                {
                    tableEqual.AddLine(element.SelectedFile.FileName, element.WebResource.Name, webresourcetype);
                }
                else
                {
                    var isCustomizable = (element.WebResource.IsCustomizable?.Value).GetValueOrDefault(true);

                    if (isCustomizable)
                    {
                        WebResource resource = new WebResource();
                        resource.WebResourceId = resource.Id = element.WebResource.Id;
                        resource.Content = contentFile;

                        this._Service.Update(resource);

                        tableUpdated.AddLine(element.SelectedFile.FileName, element.WebResource.Name, webresourcetype);
                    }
                    else
                    {
                        tableNotCustomizable.AddLine(element.SelectedFile.FileName, element.WebResource.Name, webresourcetype);
                    }
                }
            }

            if (tableEqual.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput("WebResources equal to file content:");

                var lines = tableEqual.GetFormatedLines(false);

                lines.ForEach(item => _iWriteToOutput.WriteToOutput("    {0}", item));
            }

            if (tableNotCustomizable.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput("WebResources are NOT Customizable, can't change WebResource's content:");

                var lines = tableNotCustomizable.GetFormatedLines(false);

                lines.ForEach(item => _iWriteToOutput.WriteToOutput("    {0}", item));
            }

            if (tableUpdated.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput("Updated WebResources:");

                var lines = tableUpdated.GetFormatedLines(false);

                lines.ForEach(item => _iWriteToOutput.WriteToOutput("    {0}", item));
            }
        }
    }
}