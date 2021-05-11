using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public partial class ExportXmlController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public ExportXmlController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Экспортирование списка событий форм.

        public async Task ExecuteExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemFormsEventsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ExportingFormsEvents(connectionData, commonConfig);
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

        private async Task ExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            string filePath = await CreateFormsEventsFile(service, commonConfig.FolderForExport, commonConfig.FormsEventsFileName, connectionData.Name, commonConfig.FormsEventsOnlyWithFormLibraries);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            service.TryDispose();
        }

        private async Task<string> CreateFormsEventsFile(IOrganizationServiceExtented service, string fileFolder, string fileNameFormat, string connectionDataName, bool onlyWithFormLibraries)
        {
            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Start analyzing System Forms.");

            var repository = new SystemFormRepository(service);

            var allForms = (await repository.GetListAsync())
                .OrderBy(ent => ent.ObjectTypeCode)
                .ThenBy(ent => ent.Type.Value)
                .ThenBy(ent => ent.Name)
                ;

            SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service);

            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            StringBuilder content = new StringBuilder();

            foreach (var systemForm in allForms)
            {
                string entityName = systemForm.ObjectTypeCode;
                string name = systemForm.Name;

                string typeName = systemForm.FormattedValues[SystemForm.Schema.Attributes.type];

                string formXml = systemForm.FormXml;

                if (!string.IsNullOrEmpty(formXml))
                {
                    XElement doc = XElement.Parse(formXml);

                    string events = handler.GetEventsDescription(doc);

                    if (!string.IsNullOrEmpty(events))
                    {
                        string desc = handler.GetFormLibrariesDescription(doc);

                        if (onlyWithFormLibraries && string.IsNullOrEmpty(desc))
                        {
                            continue;
                        }

                        if (content.Length > 0)
                        {
                            content
                                .AppendLine()
                                .AppendLine(new string('-', 100))
                                .AppendLine()
                                ;
                        }

                        content.AppendFormat("Entity: {0}", entityName).AppendLine();
                        content.AppendFormat("Form Type: {0}", typeName).AppendLine();
                        content.AppendFormat("Form Name: {0}", name).AppendLine();

                        if (!string.IsNullOrEmpty(desc))
                        {
                            content.AppendLine("FormLibraries:");
                            content.Append(desc);
                        }

                        content.AppendLine("Events:");
                        content.AppendLine(events);
                    }
                }
            }

            string filePath = string.Empty;

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}{1} {2}.txt"
                    , (!string.IsNullOrEmpty(connectionDataName) ? connectionDataName + "." : string.Empty)
                    , fileNameFormat.Trim()
                    , EntityFileNameFormatter.GetDateString()
                );

                filePath = Path.Combine(fileFolder, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(fileFolder))
                {
                    Directory.CreateDirectory(fileFolder);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "System Forms Events were exported to {0}", filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "No Forms Events were founded.");
            }

            return filePath;
        }

        #endregion Экспортирование списка событий форм.
    }
}
