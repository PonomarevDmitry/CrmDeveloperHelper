using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PluginAssemblyDescriptionHandler
    {
        private IOrganizationServiceExtented _service;

        private PluginTypeRepository _repType;
        private SdkMessageProcessingStepRepository _repStep;
        private SdkMessageProcessingStepImageRepository _repImage;
        private SdkMessageProcessingStepSecureConfigRepository _repSecure;
        private string _connectionInfo;

        public PluginAssemblyDescriptionHandler(IOrganizationServiceExtented service, string connectionInfo)
        {
            this._service = service;
            this._connectionInfo = connectionInfo;

            this._repType = new PluginTypeRepository(service);
            this._repStep = new SdkMessageProcessingStepRepository(service);
            this._repImage = new SdkMessageProcessingStepImageRepository(service);
            this._repSecure = new SdkMessageProcessingStepSecureConfigRepository(service);
        }

        public async Task<string> CreateDescriptionAsync(Guid idPluginAssembly, string name, DateTime now)
        {
            var content = new StringBuilder();

            var allTypes = await _repType.GetPluginTypesAsync(idPluginAssembly);
            var allSteps = await _repStep.GetAllStepsByPluginAssemblyAsync(idPluginAssembly);
            var allImages = await _repImage.GetImagesByPluginAssemblyAsync(idPluginAssembly);
            var allSecure = await _repSecure.GetAllSdkMessageProcessingStepSecureConfigAsync();

            content.AppendLine(_connectionInfo).AppendLine();
            content.AppendFormat("Description for PluginAssembly '{0}' at {1}", name, now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)).AppendLine();

            content
                .AppendLine()
                .AppendLine("Plugin Types");

            foreach (var pluginType in allTypes.OrderBy(e => e.TypeName))
            {
                content.AppendLine(pluginType.TypeName);
            }

            content
                .AppendLine()
                .AppendLine("Plugin Types Descriptions");

            foreach (var pluginType in allTypes.OrderBy(e => e.TypeName))
            {
                var desc = await PluginTypeDescriptionHandler.CreateDescriptionAsync(pluginType.PluginTypeId.Value
                        , allSteps.Where(s => s.EventHandler.Id == pluginType.PluginTypeId.Value)
                        , allImages
                        , allSecure
                        );

                if (!string.IsNullOrEmpty(desc))
                {
                    content.AppendFormat("Plugin Steps for PluginType '{0}'", pluginType.TypeName).AppendLine();

                    content.AppendLine(desc).AppendLine();
                }
            }

            return content.ToString();
        }

        public async Task CreateFileWithDescriptionAsync(string filePath, Guid idPluginAssembly, string name, DateTime now)
        {
            string content = await CreateDescriptionAsync(idPluginAssembly, name, now);

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
        }
    }
}
