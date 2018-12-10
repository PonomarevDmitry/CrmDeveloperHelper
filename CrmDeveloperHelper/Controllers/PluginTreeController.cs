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

        public async Task ExecuteShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            string operation = string.Format(Properties.OperationNames.ShowingPluginTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ShowingPluginTree(connectionData, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
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

        private async Task ShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
        }

        #endregion Дерево плагинов.

        #region Дерево сообщений.

        public async Task ExecuteShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSdkMessageTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ShowingSdkMessageTree(connectionData, commonConfig, entityFilter, messageFilter);
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

        private async Task ShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, commonConfig, entityFilter, messageFilter);
        }

        #endregion Дерево сообщений.

        #region Дерево запросов.

        public async Task ExecuteShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSdkMessageRequestTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ShowingSdkMessageRequestTree(connectionData, commonConfig, entityFilter, messageFilter);
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

        private async Task ShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, commonConfig, entityFilter, messageFilter);
        }

        #endregion Дерево запросов.

        #region Дерево плагинов с конфигуации.

        public void ExecuteShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.ShowingPluginConfigurationTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

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
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private void ShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
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
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PluginConfigurationAssemblyDescription);

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
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PluginConfigurationAssemblyDescription);
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
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PluginConfigurationTypeDescription);

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
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PluginConfigurationTypeDescription);
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
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PluginConfigurationComparer);

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
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PluginConfigurationComparer);
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
