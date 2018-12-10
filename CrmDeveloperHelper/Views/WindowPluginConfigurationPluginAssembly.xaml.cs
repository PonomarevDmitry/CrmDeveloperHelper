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
    public partial class WindowPluginConfigurationPluginAssembly : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;

        private PluginDescription _pluginDescription = null;

        private bool _controlsEnabled = true;

        private ObservableCollection<PluginAssembly> _itemsSource;

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
            if (!_controlsEnabled)
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

            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginAssemblies);

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

            ShowExistingAssemblies();
        }

        private void ShowExistingAssemblies()
        {
            this._itemsSource.Clear();

            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginAssemblies);

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
                filter = filter.Where(s => s.Name.ToLower().Contains(textName));
            }

            foreach (var assembly in filter)
            {
                _itemsSource.Add(assembly);
            }

            ToggleControls(true, Properties.WindowStatusStrings.LoadingPluginAssembliesCompletedFormat1, filter.Count());
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
            ToggleControl(this.tSBCreateAssemblyDescription, enabled);

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
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwPluginAssemblies.SelectedItems.Count > 0;

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
                var item = ((FrameworkElement)e.OriginalSource).DataContext as PluginAssembly;

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

            this._iWriteToOutput.WriteToOutput("Assembly {0} Description exported to {1}", pluginAssembly.Name, filePath);

            this._iWriteToOutput.PerformAction(filePath);
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

        private void tSBCreateAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformAssemblyDescriptionAsync);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingAssemblies();
            }

            base.OnKeyDown(e);
        }
    }
}
