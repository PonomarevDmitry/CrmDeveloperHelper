using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Backup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowPluginConfigurationPluginAssembly : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private PluginDescription _pluginDescription = null;

        private readonly ObservableCollection<PluginAssembly> _itemsSource;

        private const string _formatFileTxt = "{0} {1} - Description.txt";

        public WindowPluginConfigurationPluginAssembly(
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

            this._itemsSource = new ObservableCollection<PluginAssembly>();

            this.lstVwPluginAssemblies.ItemsSource = _itemsSource;

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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            txtBFilePath.Text = string.Empty;

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ToggleControls(null, false, Properties.OutputStrings.LoadingPluginAssemblies);

            try
            {
                this._pluginDescription = await PluginDescription.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

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

            ShowExistingAssemblies();
        }

        private void ShowExistingAssemblies()
        {
            this._itemsSource.Clear();

            ToggleControls(null, false, Properties.OutputStrings.LoadingPluginAssemblies);

            IEnumerable<PluginAssembly> filter = null;

            if (this._pluginDescription != null)
            {
                filter = this._pluginDescription.PluginAssemblies;
            }
            else
            {
                filter = new List<PluginAssembly>();
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            if (!string.IsNullOrEmpty(textName))
            {
                filter = filter.Where(s => s.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1);
            }

            foreach (var assembly in filter)
            {
                _itemsSource.Add(assembly);
            }

            ToggleControls(null, true, Properties.OutputStrings.LoadingPluginAssembliesCompletedFormat1, filter.Count());
        }

        private void UpdateStatus(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(connectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, this.tSBLoadPluginConfiguration, this.tSBCreateAssemblyDescription);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwPluginAssemblies.SelectedItems.Count > 0;

                    UIElement[] list = { tSBCreateAssemblyDescription };

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
                ShowExistingAssemblies();
            }
        }

        private PluginAssembly GetSelectedEntity()
        {
            return this.lstVwPluginAssemblies.SelectedItems.OfType<PluginAssembly>().Count() == 1
                ? this.lstVwPluginAssemblies.SelectedItems.OfType<PluginAssembly>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                PluginAssembly item = GetItemFromRoutedDataContext<PluginAssembly>(e);

                if (item != null)
                {
                    ExecuteAction(item, PerformAssemblyDescriptionAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(PluginAssembly entity, Func<string, PluginDescription, PluginAssembly, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, this._pluginDescription, entity);
        }

        private Task PerformAssemblyDescriptionAsync(string folder, PluginDescription pluginDescription, PluginAssembly pluginAssembly)
        {
            return Task.Run(async () => await PerformAssemblyDescription(folder, pluginDescription, pluginAssembly));
        }

        private async Task PerformAssemblyDescription(string folder, PluginDescription pluginDescription, PluginAssembly pluginAssembly)
        {
            string fileName = string.Format(
                _formatFileTxt
                , Path.GetFileNameWithoutExtension(pluginDescription.FilePath)
                , pluginAssembly.Name
                );

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var handler = new PluginConfigurationAssemblyDescriptionHandler(pluginDescription.GetConnectionInfo());

            string content = await handler.CreateDescriptionAsync(pluginAssembly);

            File.WriteAllText(filePath, content, new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(null, "Assembly {0} Description exported to {1}", pluginAssembly.Name, filePath);

            this._iWriteToOutput.PerformAction(null, filePath);
        }

        private void tSBLoadPluginConfiguration_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = string.Empty;
            var t = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "Plugin Configuration (.xml)|*.xml",
                        FilterIndex = 1,
                        RestoreDirectory = true
                    };

                    if (openFileDialog1.ShowDialog().GetValueOrDefault())
                    {
                        selectedPath = openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (!string.IsNullOrEmpty(selectedPath))
            {
                LoadPluginConfiguration(selectedPath);
            }
        }

        private void tSBCreateAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformAssemblyDescriptionAsync);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingAssemblies();
        }
    }
}