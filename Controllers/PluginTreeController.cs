using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class PluginTreeController
    {
        private IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PluginTreeController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Дерево плагинов.

        public async Task ExecuteShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Showing Plugin Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                await ShowingPluginTree(connectionData, commonConfig, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Showing Plugin Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Дерево плагинов.

        #region Дерево сообщений.

        public async Task ExecuteShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Showing Sdk Message Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                await ShowingSdkMessageTree(connectionData, commonConfig, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Showing Sdk Message Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Дерево сообщений.

        #region Дерево запросов.

        public async Task ExecuteShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Showing Sdk Message Request Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                await ShowingSdkMessageRequestTree(connectionData, commonConfig, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Showing Sdk Message Request Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Дерево запросов.

        #region Дерево плагинов с конфигуации.

        public void ExecuteShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Showing Plugin Configuration Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                ShowingPluginConfigurationTree(connectionData, commonConfig, filePath);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Showing Plugin Configuration Tree at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private void ShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginConfigurationPluginTree(
                        this._iWriteToOutput
                        , connectionData
                        , commonConfig
                        , filePath
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Дерево плагинов с конфигуации.

        #region Форма для описания сборки плагинов по конфигурации.

        public void ExecuteShowingPluginConfigurationAssemblyDescription(CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Plugin Configuration Assembly Description at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                ShowingPluginConfigurationAssemblyDescription(commonConfig, filePath);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Plugin Configuration Assembly Description at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private void ShowingPluginConfigurationAssemblyDescription(CommonConfiguration commonConfig, string filePath)
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginConfigurationPluginAssembly(
                        this._iWriteToOutput
                        , commonConfig
                        , filePath
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Форма для описания сборки плагинов по конфигурации.

        #region Форма для описания типа плагинов по конфигурации.

        public void ExecuteShowingPluginConfigurationTypeDescription(CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Plugin Configuration Type Description at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                ShowingPluginConfigurationTypeDescription(commonConfig, filePath);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Plugin Configuration Type Description at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private void ShowingPluginConfigurationTypeDescription(CommonConfiguration commonConfig, string filePath)
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginConfigurationPluginType(
                        this._iWriteToOutput
                        , commonConfig
                        , filePath
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Форма для описания типа плагинов по конфигурации.

        #region Форма для описания типа плагинов по конфигурации.

        public void ExecuteShowingPluginConfigurationComparer(CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Plugin Configuration Type Description at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                ShowingPluginConfigurationComparer(commonConfig, filePath);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Plugin Configuration Type Description at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private void ShowingPluginConfigurationComparer(CommonConfiguration commonConfig, string filePath)
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginConfigurationComparerPluginAssembly(
                        this._iWriteToOutput
                        , commonConfig
                        , filePath
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Форма для описания типа плагинов по конфигурации.
    }
}
