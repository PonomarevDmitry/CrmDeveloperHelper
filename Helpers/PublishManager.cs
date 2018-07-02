using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Crm.Sdk.Messages;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    /// <summary>
    /// Менеджер для обновления веб-ресурсов и публикации
    /// </summary>
    public class PublishManager
    {
        private IWriteToOutput _iWriteToOutput;

        private IOrganizationServiceExtented _Service;

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
            if (_Elements.Count > 0)
            {
                string filesNames = string.Join("; ", this._Elements.Values.Select(element => element.SelectedFile.FileName).OrderBy(name => name));

                this._iWriteToOutput.WriteToOutput("Attempting to update content and publish files: {0}", filesNames);

                this._iWriteToOutput.WriteToOutput("Updating content...");

                UpdateContent();

                this._iWriteToOutput.WriteToOutput("Publishing...");

                PublishResources();

                string webResourcesNames = string.Join("; ", this._Elements.Values.Select(webRes => webRes.WebResource.Name).OrderBy(name => name));

                this._iWriteToOutput.WriteToOutput("Published web-resources: {0}", webResourcesNames);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("Nothing to publish.");
            }
        }

        /// <summary>
        /// Обновление контента веб-ресурсов
        /// </summary>
        /// <returns></returns>
        private void UpdateContent()
        {
            var list = _Elements.Values.OrderBy(element => element.SelectedFile.FriendlyFilePath).ToList();

            foreach (var element in list)
            {
                var contentFile = Convert.ToBase64String(File.ReadAllBytes(element.SelectedFile.FilePath));

                var contentWebResource = element.WebResource.Content ?? string.Empty;

                var name = element.WebResource.Name;

                if (contentFile == contentWebResource)
                {
                    this._iWriteToOutput.WriteToOutput("WebResource and file are equal by content: web {0}:{1}; file: {2}", element.WebResource.Id, name, element.SelectedFile.FileName);
                }
                else
                {
                    WebResource resource = new WebResource();
                    resource.WebResourceId = resource.Id = element.WebResource.Id;
                    resource.Content = contentFile;

                    this._Service.Update(resource);

                    this._iWriteToOutput.WriteToOutput("Updated: web {0}:{1}; file: {2}", element.WebResource.Id, name, element.SelectedFile.FileName);
                }
            }
        }

        private const string _formatPublishAll =
@"<importexportxml>
    <webresources>
        {0}
    </webresources>
</importexportxml>";

        private const string _formatSingleWebResource = "<webresource>{0}</webresource>";

        /// <summary>
        /// Публикация обновленных веб-ресурсов
        /// </summary>
        /// <returns></returns>
        private void PublishResources()
        {
            var content = string.Join("", _Elements.Keys.Select(a => string.Format(_formatSingleWebResource, a)));

            OrganizationRequest request = new OrganizationRequest
            {
                RequestName = "PublishXml",
                Parameters =
                {
                    { "ParameterXml", string.Format(_formatPublishAll, content) },
                }
            };

            _Service.Execute(request);
        }
    }
}
