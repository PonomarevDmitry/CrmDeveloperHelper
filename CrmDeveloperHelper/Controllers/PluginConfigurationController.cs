using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class PluginConfigurationController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PluginConfigurationController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Дерево плагинов с конфигуации.

        public void ExecuteShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.ShowingPluginConfigurationTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                ShowingPluginConfigurationTree(connectionData, commonConfig, filePath);
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

        private void ShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            var thread = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginConfigurationPluginTree(
                        this._iWriteToOutput
                        , commonConfig
                        , connectionData
                        , filePath
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);

            thread.Start();
        }

        #endregion Дерево плагинов с конфигуации.

        #region Форма для описания сборки плагинов по конфигурации.

        public void ExecuteShowingPluginConfigurationAssemblyDescriptionWindow(CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.PluginConfigurationAssemblyDescription);

            try
            {
                ShowingPluginConfigurationAssemblyDescriptionWindow(commonConfig, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.PluginConfigurationAssemblyDescription);
            }
        }

        private void ShowingPluginConfigurationAssemblyDescriptionWindow(CommonConfiguration commonConfig, string filePath)
        {
            var thread = new System.Threading.Thread(() =>
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
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);

            thread.Start();
        }

        #endregion Форма для описания сборки плагинов по конфигурации.

        #region Форма для описания типа плагинов по конфигурации.

        public void ExecuteShowingPluginConfigurationTypeDescriptionWindow(CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.PluginConfigurationTypeDescription);

            try
            {
                ShowingPluginConfigurationTypeDescriptionWindow(commonConfig, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.PluginConfigurationTypeDescription);
            }
        }

        private void ShowingPluginConfigurationTypeDescriptionWindow(CommonConfiguration commonConfig, string filePath)
        {
            var thread = new System.Threading.Thread(() =>
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
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);

            thread.Start();
        }

        #endregion Форма для описания типа плагинов по конфигурации.

        #region Форма для описания типа плагинов по конфигурации.

        public void ExecuteShowingPluginConfigurationComparer(CommonConfiguration commonConfig, string filePath)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.PluginConfigurationComparer);

            try
            {
                ShowingPluginConfigurationComparer(commonConfig, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.PluginConfigurationComparer);
            }
        }

        private void ShowingPluginConfigurationComparer(CommonConfiguration commonConfig, string filePath)
        {
            var thread = new System.Threading.Thread(() =>
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
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);

            thread.Start();
        }

        #endregion Форма для описания типа плагинов по конфигурации.
    }
}
