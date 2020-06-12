using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractCommand
    {
        protected static Task ExecuteActionOnPluginTypesAsync(
            DTEHelper helper
            , ConnectionData connectionData
            , string[] pluginTypesNotCompiled
            , VSProject2Info[] projectInfos
            , ActionOnComponent actionOnComponent
        )
        {
            return ExecuteActionOnPluginTypesAsync(helper, connectionData, pluginTypesNotCompiled, projectInfos, actionOnComponent, string.Empty, string.Empty);
        }

        protected static async Task ExecuteActionOnPluginTypesAsync(
            DTEHelper helper
            , ConnectionData connectionData
            , string[] pluginTypesNotCompiled
            , VSProject2Info[] projectInfos
            , ActionOnComponent actionOnComponent
            , string fieldName
            , string fieldTitle
        )
        {
            try
            {
                string[] pluginTypeArray = await CSharpCodeHelper.GetTypeFullNameListAsync(pluginTypesNotCompiled, projectInfos);

                if (pluginTypeArray.Any())
                {
                    helper.HandleActionOnPluginTypesCommand(connectionData, actionOnComponent, fieldName, fieldTitle, pluginTypeArray);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }

        protected static async Task ExecuteAddPluginStepAsync(
            DTEHelper helper
            , ConnectionData connectionData
            , string[] pluginTypesNotCompiled
            , VSProject2Info[] projectInfos
        )
        {
            try
            {
                string pluginType = await CSharpCodeHelper.GetSingleFileTypeFullNameAsync(pluginTypesNotCompiled, projectInfos);

                if (!string.IsNullOrEmpty(pluginType))
                {
                    helper.HandleAddPluginStep(pluginType, connectionData);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }
    }
}
