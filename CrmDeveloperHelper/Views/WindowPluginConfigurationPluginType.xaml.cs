using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowPluginConfigurationPluginType : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;

        private PluginDescription _pluginDescription = null;

        private ObservableCollection<PluginTypeFullInfo> _itemsSource;

        private bool _controlsEnabled = true;

        private const string _formatFileTxt = "{0} {1} - Description.txt";

        public WindowPluginConfigurationPluginType(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , string filePath
        )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            

            LoadFromConfig();

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<PluginTypeFullInfo>();

            this.lstVwPluginTypes.ItemsSource = _itemsSource;

            if (!string.IsNullOrEmpty(filePath))
            {
                LoadPluginConfiguration(filePath);
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
        }

        private async Task LoadPluginConfiguration(string filePath)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            this._itemsSource.Clear();

            txtBFilePath.Text = string.Empty;

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginConfiguration);

            try
            {
                this._pluginDescription = await PluginDescription.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

                this._pluginDescription = null;
            }

            txtBFilePath.Dispatcher.Invoke(() =>
            {
                if (this._pluginDescription != null)
                {
                    txtBFilePath.Text = filePath;
                }
                else
                {
                    txtBFilePath.Text = string.Empty;
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingPluginConfigurationCompleted);

            ShowExistingPluginTypes();
        }

        private class PluginTypeFullInfo
        {
            public PluginDescription PluginDescription { get; set; }

            public PluginAssembly PluginAssembly { get; set; }

            public PluginType PluginType { get; set; }
        }

        private void ShowExistingPluginTypes()
        {
            this._itemsSource.Clear();
            
            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginTypes);

            IEnumerable<PluginTypeFullInfo> filter = null;

            if (this._pluginDescription != null)
            {
                filter = from a in this._pluginDescription.PluginAssemblies
                         from t in a.PluginTypes
                         orderby t.TypeName
                         select new PluginTypeFullInfo()
                         {
                             PluginDescription = this._pluginDescription,
                             PluginAssembly = a,
                             PluginType = t,
                         };
            }
            else
            {
                filter = new List<PluginTypeFullInfo>();
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            if (!string.IsNullOrEmpty(textName))
            {
                filter = filter.Where(s => s.PluginType.TypeName.ToLower().Contains(textName));
            }

            foreach (var plugintype in filter)
            {
                _itemsSource.Add(plugintype);
            }
            
            ToggleControls(true, Properties.WindowStatusStrings.LoadingPluginTypesCompletedFormat1, filter.Count());
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSBLoadPluginConfiguration, enabled);
            ToggleControl(this.tSBCreatePluginTypeDescription, enabled);

            ToggleProgressBar(enabled);

            if (enabled)
            {
                UpdateButtonsEnable();
            }
        }

        private void ToggleProgressBar(bool enabled)
        {
            if (tSProgressBar == null)
            {
                return;
            }

            this.tSProgressBar.Dispatcher.Invoke(() =>
            {
                tSProgressBar.IsIndeterminate = !enabled;
            });
        }

        private void ToggleControl(Control c, bool enabled)
        {
            c.Dispatcher.Invoke(() =>
            {
                if (c is TextBox)
                {
                    ((TextBox)c).IsReadOnly = !enabled;
                }
                else
                {
                    c.IsEnabled = enabled;
                }
            });
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginTypes.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwPluginTypes.SelectedItems.Count > 0;

                    UIElement[] list = { tSBCreatePluginTypeDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingPluginTypes();
            }
        }

        private PluginTypeFullInfo GetSelectedEntity()
        {
            return this.lstVwPluginTypes.SelectedItems.OfType<PluginTypeFullInfo>().Count() == 1
                ? this.lstVwPluginTypes.SelectedItems.OfType<PluginTypeFullInfo>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as PluginTypeFullInfo;

                if (item != null)
                {
                    ExecuteAction(item, PerformExportAllXml);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(PluginTypeFullInfo entity, Func<string, PluginTypeFullInfo, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, entity);
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, PluginTypeFullInfo pluginType)
        {
            await PerformPluginTypeDescriptionAsync(folder, pluginType);
        }

        private void mICreatePluginTypeDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformPluginTypeDescriptionAsync);
        }

        private Task PerformPluginTypeDescriptionAsync(string folder, PluginTypeFullInfo pluginType)
        {
            return Task.Run(async () => await PerformPluginTypeDescription(folder, pluginType));
        }

        private async Task PerformPluginTypeDescription(string folder, PluginTypeFullInfo pluginType)
        {
            var handler = new PluginTypeConfigurationDescriptionHandler();

            string description = await handler.CreateDescriptionAsync(pluginType.PluginType);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = string.Format(
                    _formatFileTxt
                  , Path.GetFileNameWithoutExtension(pluginType.PluginDescription.FilePath)
                  , pluginType.PluginType.TypeName);

                StringBuilder content = new StringBuilder();

                content.AppendLine(pluginType.PluginDescription.GetConnectionInfo()).AppendLine();
                content.AppendFormat("Plugin Steps for PluginType '{0}'", pluginType.PluginType.TypeName).AppendLine();

                content.Append(description);

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("PluginType {0} description exported to {1}", pluginType.PluginType.TypeName, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("PluginType {0} description is empty.", pluginType.PluginType.TypeName);
            }
        }

        private void tSBLoadPluginConfiguration_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = string.Empty;
            var t = new Thread((ThreadStart)(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog();

                    openFileDialog1.Filter = "Plugin Configuration (.xml)|*.xml";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.RestoreDirectory = true;

                    if (openFileDialog1.ShowDialog().GetValueOrDefault())
                    {
                        selectedPath = openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (!string.IsNullOrEmpty(selectedPath))
            {
                LoadPluginConfiguration(selectedPath);
            }
        }

        private void tSBCreatePluginTypeDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformPluginTypeDescriptionAsync);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingPluginTypes();
            }

            base.OnKeyDown(e);
        }
    }
}
