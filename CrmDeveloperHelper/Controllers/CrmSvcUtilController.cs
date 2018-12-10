using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CrmSvcUtilController
    {
        private IWriteToOutput _iWriteToOutput = null;

        public CrmSvcUtilController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        public async Task ExecuteUpdatingProxyClasses(string filePath, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingProxyClassesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await UpdatingProxyClasses(filePath, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task UpdatingProxyClasses(string filePath, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!connectionData.SelectedCrmSvcUtil.HasValue)
            {
                this._iWriteToOutput.WriteToOutput("No Crm Svc Util is selected.");
                return;
            }

            var crmSvcUtil = commonConfig.Utils.FirstOrDefault(e => e.Id == connectionData.SelectedCrmSvcUtil);

            if (crmSvcUtil == null)
            {
                connectionData.SelectedCrmSvcUtil = null;
                connectionData.ConnectionConfiguration.Save();

                this._iWriteToOutput.WriteToOutput("No Crm Svc Util is selected.");
                return;
            }

            if (!File.Exists(crmSvcUtil.Path))
            {
                commonConfig.Utils.Remove(crmSvcUtil);

                connectionData.SelectedCrmSvcUtil = null;
                connectionData.ConnectionConfiguration.Save();

                this._iWriteToOutput.WriteToOutput("Crm Svc Util not exists.");
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            StringBuilder arguments = new StringBuilder("/language:CS");

            //arguments.Append(" /nologo");

            bool isInteractive = connectionData.InteractiveLogin || connectionData.User == null;

            arguments.AppendFormat(" /url:{0}", connectionData.OrganizationUrl);
            arguments.AppendFormat(" /out:\"{0}\"", filePath);

            if (!string.IsNullOrEmpty(connectionData.NameSpaceClasses))
            {
                arguments.AppendFormat(" /namespace:{0}", connectionData.NameSpaceClasses);
            }

            if (!string.IsNullOrEmpty(connectionData.ServiceContextName))
            {
                arguments.AppendFormat(" /serviceContextName:{0}", connectionData.ServiceContextName);
            }

            if (connectionData.GenerateActions)
            {
                arguments.Append(" /generateActions");
            }

            if (isInteractive)
            {
                arguments.Append(" /interactivelogin:true");
            }
            else
            {
                arguments.AppendFormat(" /username:\"{0}\"", connectionData.User.Username);
            }

            _iWriteToOutput.WriteToOutput(crmSvcUtil.ToString());
            _iWriteToOutput.WriteToOutput(arguments.ToString());

            if (!isInteractive)
            {
                arguments.AppendFormat(" /password:\"{0}\"", connectionData.User.Password);
            }

            ProcessStartInfo info = new ProcessStartInfo();

            info.FileName = string.Format("\"{0}\"", crmSvcUtil.Path);

            info.Arguments = arguments.ToString();

            info.UseShellExecute = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            //_iWriteToOutput.WriteToOutput(info.FileName);
            //_iWriteToOutput.WriteToOutput(info.Arguments);

            try
            {
                var process = new Process();
                process.StartInfo = info;

                process.OutputDataReceived += process_OutputDataReceived;
                process.ErrorDataReceived += process_OutputDataReceived;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e != null)
            {
                _iWriteToOutput.WriteToOutput(e.Data ?? string.Empty);
            }
        }
    }
}
