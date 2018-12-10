using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PluginConfigurationAssemblyDescriptionHandler
    {
        private string _connectionInfo;

        public PluginConfigurationAssemblyDescriptionHandler(string connectionInfo)
        {
            this._connectionInfo = connectionInfo;
        }

        public async Task<string> CreateDescriptionAsync(PluginExtraction.PluginAssembly pluginAssembly)
        {
            var content = new StringBuilder();

            var handler = new PluginTypeConfigurationDescriptionHandler();

            content.AppendLine(_connectionInfo).AppendLine();
            content.AppendFormat("Description for PluginAssembly '{0}'", pluginAssembly.Name).AppendLine();

            content
                .AppendLine()
                .AppendLine("Plugin Types");

            foreach (var pluginType in pluginAssembly.PluginTypes.OrderBy(e => e.TypeName))
            {
                content.AppendLine(pluginType.Name);
            }

            content
                .AppendLine()
                .AppendLine("Plugin Types Descriptions");

            foreach (var pluginType in pluginAssembly.PluginTypes.OrderBy(e => e.TypeName))
            {
                var desc = await handler.CreateDescriptionAsync(pluginType);

                if (!string.IsNullOrEmpty(desc))
                {
                    content.AppendFormat("Plugin Steps for PluginType '{0}'", pluginType.TypeName).AppendLine();

                    content.AppendLine(desc).AppendLine();
                }
            }

            return content.ToString();
        }
    }
}
