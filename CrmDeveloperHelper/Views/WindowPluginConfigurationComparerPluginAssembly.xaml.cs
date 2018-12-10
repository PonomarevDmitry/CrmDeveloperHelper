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
    public partial class WindowPluginConfigurationComparerPluginAssembly : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        private PluginConfigurationAssemblyDescriptionHandler _handler1;
        private PluginConfigurationAssemblyDescriptionHandler _handler2;

        private CommonConfiguration _commonConfig;

        private PluginDescription _pluginDescription1 = null;
        private PluginDescription _pluginDescription2 = null;

        private bool _controlsEnabled = true;

        private List<LinkedEntities<PluginAssembly>> _allEntities = null;

        private ObservableCollection<EntityViewItem> _itemsSource;

        public WindowPluginConfigurationComparerPluginAssembly(
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

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwPluginAssemblies.ItemsSource = _itemsSource;

            if (!string.IsNullOrEmpty(filePath))
            {
                LoadPluginConfiguration(filePath, SetPluginConfig1, txtBFilePath1);
            }
        }

        private void LoadFromConfig()
        {
            txtBFolder.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
        }

        private void SetPluginConfig1(PluginDescription pluginDescription)
        {
            this._pluginDescription1 = pluginDescription;

            this._handler1 = new PluginConfigurationAssemblyDescriptionHandler(pluginDescription.GetConnectionInfo());
        }

        private void SetPluginConfig2(PluginDescription pluginDescription)
        {
            this._pluginDescription2 = pluginDescription;

            this._handler2 = new PluginConfigurationAssemblyDescriptionHandler(pluginDescription.GetConnectionInfo());
        }

        private async Task LoadPluginConfiguration(string filePath, Action<PluginDescription> setter, TextBox textBox)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            textBox.Text = string.Empty;
            
            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginConfiguration);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            PluginDescription pluginDescription = null;

            try
            {
                pluginDescription = await PluginDescription.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);

                pluginDescription = null;
            }

            setter(pluginDescription);

            this._allEntities = null;

            textBox.Dispatcher.Invoke(() =>
            {
                if (pluginDescription != null)
                {
                    textBox.Text = filePath;
                }
                else
                {
                    textBox.Text = string.Empty;
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingPluginConfigurationCompleted);

            ShowExistingAssemblies();
        }

        private class EntityViewItem
        {
            public string AssemblyName { get; private set; }

            public LinkedEntities<PluginAssembly> Link { get; private set; }

            public EntityViewItem(string assemblyName, LinkedEntities<PluginAssembly> link)
            {
                this.AssemblyName = assemblyName;
                this.Link = link;
            }
        }

        private void ShowExistingAssemblies()
        {
            this._itemsSource.Clear();
            
            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginAssemblies);

            if (this._pluginDescription1 != null && this._pluginDescription2 != null)
            {
                if (this._allEntities == null)
                {
                    List<LinkedEntities<PluginAssembly>> list = new List<LinkedEntities<PluginAssembly>>();

                    var list1 = this._pluginDescription1.PluginAssemblies;
                    var list2 = this._pluginDescription2.PluginAssemblies;

                    foreach (var assembly1 in list1)
                    {
                        var assembly2 = list2.FirstOrDefault(a => a.Name == assembly1.Name);

                        if (assembly2 == null)
                        {
                            continue;
                        }

                        var item = new LinkedEntities<PluginAssembly>(assembly1, assembly2);

                        list.Add(item);
                    }

                    this._allEntities = list;
                }
            }

            var filter = this._allEntities.AsEnumerable();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            if (!string.IsNullOrEmpty(textName))
            {
                filter = filter.Where(ent =>
                ent.Entity1.Name.ToLower().Contains(textName)
                || ent.Entity2.Name.ToLower().Contains(textName)
                );
            }

            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                foreach (var assembly in filter)
                {
                    _itemsSource.Add(new EntityViewItem(assembly.Entity1.Name, assembly));
                }
            });
            
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

            ToggleControl(this.tSBLoadPluginConfiguration1, enabled);
            ToggleControl(this.tSBLoadPluginConfiguration2, enabled);
            ToggleControl(this.menuActions, enabled);

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

                    UIElement[] list = { tSDDBActions };

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

        private LinkedEntities<PluginAssembly> GetSelectedEntity()
        {
            return this.lstVwPluginAssemblies.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwPluginAssemblies.SelectedItems.OfType<EntityViewItem>().Select(e => e.Link).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    ExecuteAction(item.Link, PerformShowingDifference);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(LinkedEntities<PluginAssembly> link, Func<string, LinkedEntities<PluginAssembly>, Task> action)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, link);
        }

        private async Task PerformShowingDifference(string folder, LinkedEntities<PluginAssembly> linked)
        {
            if (!_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferencePluginAssemblyDescriptionFormat1, linked.Entity1.Name);
            
            DateTime now = DateTime.Now;

            string desc1 = await _handler1.CreateDescriptionAsync(linked.Entity1);
            string desc2 = await _handler2.CreateDescriptionAsync(linked.Entity2);

            {
                string filePath1 = await CreateDescriptionFileAsync(folder, _pluginDescription1.FilePath, linked.Entity1.Name, desc1);

                string filePath2 = await CreateDescriptionFileAsync(folder, _pluginDescription2.FilePath, linked.Entity2.Name, desc2);

                if (File.Exists(filePath1) && File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                }
                else
                {
                    this._iWriteToOutput.PerformAction(filePath1);

                    this._iWriteToOutput.PerformAction(filePath2);
                }
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferencePluginAssemblyDescriptionCompletedFormat1, linked.Entity1.Name);
        }

        private Task<string> CreateDescriptionFileAsync(string folder, string filePathConfiguration, string name, string description)
        {
            return Task.Run(() => CreateDescriptionFile(folder, filePathConfiguration, name, description));
        }

        private string CreateDescriptionFile(string folder, string filePathConfiguration, string name, string description)
        {
            string fileName = EntityFileNameFormatter.GetPluginConfigurationFileName(Path.GetFileNameWithoutExtension(filePathConfiguration), name, "txt");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(filePathConfiguration);
                    this._iWriteToOutput.WriteToOutput("Plugin Assembly {0} Description exported to {1}", name, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(filePathConfiguration);
                this._iWriteToOutput.WriteToOutput("PluginAssembly {0} Description is empty.", name);
            }

            return filePath;
        }

        private void tSBLoadPluginConfiguration1_Click(object sender, RoutedEventArgs e)
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
                LoadPluginConfiguration(selectedPath, SetPluginConfig1, txtBFilePath1);
            }
        }

        private void tSBLoadPluginConfiguration2_Click(object sender, RoutedEventArgs e)
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
                LoadPluginConfiguration(selectedPath, SetPluginConfig2, txtBFilePath2);
            }
        }

        private void tSMIExportPluginConfiguration1PluginAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(_handler1, this._pluginDescription1, link.Entity1, PerformExportDescriptionToFile);
        }

        private void tSMIExportPluginConfiguration2PluginAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(_handler2, this._pluginDescription2, link.Entity2, PerformExportDescriptionToFile);
        }

        private async Task ExecuteActionPluginAssemblyDescription(
            PluginConfigurationAssemblyDescriptionHandler handler
            , PluginDescription pluginDescription
            , PluginAssembly pluginAssembly
            , Func<string, PluginConfigurationAssemblyDescriptionHandler, PluginDescription, PluginAssembly, Task> action)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, handler, pluginDescription, pluginAssembly);
        }

        private async Task PerformExportDescriptionToFile(string folder, PluginConfigurationAssemblyDescriptionHandler handler, PluginDescription pluginDescription, PluginAssembly assembly)
        {
            if (!_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.CreatingDescription);

            string description = await handler.CreateDescriptionAsync(assembly);

            string filePath = await CreateDescriptionFileAsync(folder, pluginDescription.FilePath, assembly.Name, description);

            this._iWriteToOutput.PerformAction(filePath);
            
            ToggleControls(true, Properties.WindowStatusStrings.CreatingDescriptionCompleted);
        }

        private void tSMIShowDifferencePluginAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteAction(link, PerformShowingDifference);
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
