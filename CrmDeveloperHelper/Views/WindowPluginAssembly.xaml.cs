using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowPluginAssembly : WindowBase
    {
        private bool _controlsEnabled = true;

        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;

        private BitmapImage _imagePluginAssembly;
        private BitmapImage _imagePluginType;
        private BitmapImage _imageWorkflowActivity;

        public PluginAssembly PluginAssembly { get; private set; }

        private AssemblyReaderResult _assemblyLoad;

        private string _defaultAssemblyPath;

        private int _init = 0;

        private ObservableCollection<PluginTreeViewItem> _listLocalAssembly = new ObservableCollection<PluginTreeViewItem>();
        private ObservableCollection<PluginTreeViewItem> _listMissingCrm = new ObservableCollection<PluginTreeViewItem>();

        public WindowPluginAssembly(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , PluginAssembly pluginAssembly
            , string defaultAssemblyPath
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._defaultAssemblyPath = defaultAssemblyPath;

            this.PluginAssembly = pluginAssembly;

            InitializeComponent();

            LoadImages();

            LoadFromConfig();

            tSSLblConnectionName.Content = _service.ConnectionData.Name;

            LoadEntityPluginAssemblyProperties();

            this.trVPluginTreeNew.ItemsSource = _listLocalAssembly;

            this.trVPluginTreeMissing.ItemsSource = _listMissingCrm;

            _init--;
        }

        private void LoadFromConfig()
        {
            WindowSettings winConfig = GetWindowsSettings();

            LoadFormSettings(winConfig);
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnGeneralInfoWidth = "ColumnGeneralInfoWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnGeneralInfoWidth))
            {
                columnGeneralInfo.Width = new GridLength(winConfig.DictDouble[paramColumnGeneralInfoWidth]);
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnGeneralInfoWidth] = columnGeneralInfo.Width.Value;
        }

        private void LoadImages()
        {
            this._imagePluginAssembly = this.Resources["ImagePluginAssembly"] as BitmapImage;
            this._imagePluginType = this.Resources["ImagePluginType"] as BitmapImage;
            this._imageWorkflowActivity = this.Resources["ImageWorkflowActivity"] as BitmapImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadEntityPluginAssemblyProperties()
        {
            txtBDescription.Text = PluginAssembly.Description;
            txtBFileNameOnServer.Text = PluginAssembly.Path;

            if (PluginAssembly.IsolationMode?.Value == (int)PluginAssembly.Schema.OptionSets.isolationmode.None_1)
            {
                rBNone.IsChecked = true;
            }
            else if (PluginAssembly.IsolationMode?.Value == (int)PluginAssembly.Schema.OptionSets.isolationmode.Sandbox_2)
            {
                rBSandBox.IsChecked = true;
            }
            else
            {
                rBNone.IsChecked = true;
            }

            if (PluginAssembly.SourceType?.Value == (int)PluginAssembly.Schema.OptionSets.sourcetype.Database_0)
            {
                rBDatabase.IsChecked = true;
            }
            else if (PluginAssembly.SourceType?.Value == (int)PluginAssembly.Schema.OptionSets.sourcetype.Disk_1)
            {
                rBDisk.IsChecked = true;
            }
            else if (PluginAssembly.SourceType?.Value == (int)PluginAssembly.Schema.OptionSets.sourcetype.Normal_2)
            {
                rBGAC.IsChecked = true;
            }
            else
            {
                rBDatabase.IsChecked = true;
            }

            if (!string.IsNullOrEmpty(this.PluginAssembly.Name))
            {
                List<string> lastPaths = _service.ConnectionData.GetAssemblyPaths(this.PluginAssembly.Name).ToList();

                foreach (var path in lastPaths)
                {
                    cmBAssemblyToLoad.Items.Add(path);
                }
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await PerformSaveClick();
        }

        public int GetIsolationMode()
        {
            if (rBSandBox.IsChecked.GetValueOrDefault())
            {
                return (int)PluginAssembly.Schema.OptionSets.isolationmode.Sandbox_2;
            }
            else if (rBNone.IsChecked.GetValueOrDefault())
            {
                return (int)PluginAssembly.Schema.OptionSets.isolationmode.None_1;
            }

            return (int)PluginAssembly.Schema.OptionSets.isolationmode.None_1;
        }

        public int GetSourceType()
        {
            if (rBDatabase.IsChecked.GetValueOrDefault())
            {
                return (int)PluginAssembly.Schema.OptionSets.sourcetype.Database_0;
            }
            else if (rBDisk.IsChecked.GetValueOrDefault())
            {
                return (int)PluginAssembly.Schema.OptionSets.sourcetype.Disk_1;
            }
            else if (rBGAC.IsChecked.GetValueOrDefault())
            {
                return (int)PluginAssembly.Schema.OptionSets.sourcetype.Normal_2;
            }

            return (int)PluginAssembly.Schema.OptionSets.sourcetype.Database_0;
        }

        private async Task PerformSaveClick()
        {
            if (_assemblyLoad == null)
            {
                return;
            }

            if (_listMissingCrm.Count > 0)
            {
                return;
            }

            var updateAssembly = new PluginAssembly()
            {
                Id = PluginAssembly.Id,
            };

            //updateAssembly.Name = txtBName.Text.Trim();
            updateAssembly.Description = txtBDescription.Text.Trim();

            var isolationMode = GetIsolationMode();
            var sourceType = GetSourceType();

            updateAssembly.IsolationMode = new Microsoft.Xrm.Sdk.OptionSetValue(isolationMode);
            updateAssembly.SourceType = new Microsoft.Xrm.Sdk.OptionSetValue(sourceType);
            updateAssembly.Version = _assemblyLoad.Version;
            updateAssembly.Culture = _assemblyLoad.Culture;
            updateAssembly.PublicKeyToken = _assemblyLoad.PublicKeyToken;

            if (sourceType == (int)PluginAssembly.Schema.OptionSets.sourcetype.Database_0
                && _assemblyLoad.Content != null
                && _assemblyLoad.Content.Length > 0
            )
            {
                updateAssembly.Content = Convert.ToBase64String(_assemblyLoad.Content);
            }

            if (sourceType == (int)PluginAssembly.Schema.OptionSets.sourcetype.Disk_1)
            {
                updateAssembly.Path = txtBFileNameOnServer.Text.Trim();
            }
            else
            {
                updateAssembly.Path = string.Empty;
            }

            if (updateAssembly.Id == Guid.Empty)
            {
                updateAssembly.Name = _assemblyLoad.Name;
            }

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingPluginAssemblyFormat1, _service.ConnectionData.Name);

            try
            {
                this.PluginAssembly.Id = await _service.UpsertAsync(updateAssembly);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingPluginAssemblyCompletedFormat1, _service.ConnectionData.Name);

                var assemblyRef = this.PluginAssembly.ToEntityReference();

                foreach (var pluginType in _listLocalAssembly.Where(p => p.IsChecked))
                {
                    var pluginTypeEntity = new PluginType()
                    {
                        Name = pluginType.Name,
                        TypeName = pluginType.Name,
                        FriendlyName = pluginType.Name,

                        PluginAssemblyId = assemblyRef,
                    };

                    ToggleControls(true, Properties.WindowStatusStrings.RegisteringPluginTypeFormat2, _service.ConnectionData.Name, pluginType);

                    try
                    {
                        pluginTypeEntity.Id = await _service.CreateAsync(pluginTypeEntity);
                    }
                    catch (Exception ex)
                    {
                        ToggleControls(true, Properties.WindowStatusStrings.RegisteringPluginTypeFailedFormat2, _service.ConnectionData.Name, pluginType);

                        _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                        _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                    }
                }

                this.DialogResult = true;

                this.Close();
            }
            catch (Exception ex)
            {
                ToggleControls(true, Properties.WindowStatusStrings.UpdatingPluginAssemblyFailedFormat1, _service.ConnectionData.Name);

                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }
        }

        private void rbSourceType_Checked(object sender, RoutedEventArgs e)
        {
            var isDisk = rBDisk.IsChecked.GetValueOrDefault();

            txtBFileNameOnServer.IsReadOnly = !isDisk;
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(enabled, this.tSProgressBar, trVPluginTreeNew, trVPluginTreeMissing, btnSave, btnClose);
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(_service.ConnectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            string lastAssemblyPath = string.Empty;
            IEnumerable<string> lastPaths = Enumerable.Empty<string>();

            if (!string.IsNullOrEmpty(this.PluginAssembly.Name))
            {
                lastAssemblyPath = _service.ConnectionData.GetLastAssemblyPath(this.PluginAssembly.Name);
                lastPaths = _service.ConnectionData.GetAssemblyPaths(this.PluginAssembly.Name).ToList();
            }

            bool isSuccess = false;
            string assemblyPath = string.Empty;

            try
            {
                var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Plugin Assebmly (.dll)|*.dll",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (!string.IsNullOrEmpty(lastAssemblyPath))
                {
                    openFileDialog1.InitialDirectory = Path.GetDirectoryName(lastAssemblyPath);
                    openFileDialog1.FileName = Path.GetFileName(lastAssemblyPath);
                }
                else if (!string.IsNullOrEmpty(_defaultAssemblyPath))
                {
                    openFileDialog1.InitialDirectory = Path.GetDirectoryName(_defaultAssemblyPath);
                    openFileDialog1.FileName = Path.GetFileName(_defaultAssemblyPath);
                }

                if (lastPaths.Any())
                {
                    openFileDialog1.CustomPlaces = new List<Microsoft.Win32.FileDialogCustomPlace>(lastPaths.Select(p => new Microsoft.Win32.FileDialogCustomPlace(Path.GetDirectoryName(p))));
                }

                if (openFileDialog1.ShowDialog().GetValueOrDefault())
                {
                    isSuccess = true;
                    assemblyPath = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }

            if (!isSuccess)
            {
                return;
            }

            LoadLocalAssemblyAsync(assemblyPath);
        }

        private void btnLoadAssembly_Click(object sender, RoutedEventArgs e)
        {
            PerformLoadAssemblyClick();
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLoadAssemblyClick();
            }
        }

        private void PerformLoadAssemblyClick()
        {
            var assemblyPath = cmBAssemblyToLoad.Text?.Trim();

            LoadLocalAssemblyAsync(assemblyPath);
        }

        private async Task LoadLocalAssemblyAsync(string assemblyPath)
        {
            this.Dispatcher.Invoke(() =>
            {
                _listLocalAssembly.Clear();
                _listMissingCrm.Clear();

                cmBAssemblyToLoad.Text = string.Empty;
            });

            if (string.IsNullOrEmpty(assemblyPath)
               || !File.Exists(assemblyPath)
            )
            {
                return;
            }

            AssemblyReaderResult assemblyCheck = null;

            using (var reader = new AssemblyReader())
            {
                assemblyCheck = reader.ReadAssembly(assemblyPath);
            }

            if (assemblyCheck == null)
            {
                return;
            }

            assemblyCheck.Content = File.ReadAllBytes(assemblyPath);

            this._assemblyLoad = assemblyCheck;
            cmBAssemblyToLoad.Text = this._assemblyLoad.FilePath;

            if (PluginAssembly.Id == Guid.Empty)
            {
                txtBFileNameOnServer.Text = this._assemblyLoad.FileName;
            }

            var crmPlugins = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var crmWorkflows = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (this.PluginAssembly != null && this.PluginAssembly.Id != Guid.Empty)
            {
                var repositoryType = new PluginTypeRepository(_service);
                var pluginTypes = await repositoryType.GetPluginTypesAsync(this.PluginAssembly.Id);

                foreach (var item in pluginTypes.Where(e => !e.IsWorkflowActivity.GetValueOrDefault()).Select(e => e.TypeName))
                {
                    crmPlugins.Add(item);
                }

                foreach (var item in pluginTypes.Where(e => e.IsWorkflowActivity.GetValueOrDefault()).Select(e => e.TypeName))
                {
                    crmWorkflows.Add(item);
                }
            }

            HashSet<string> assemblyPlugins = new HashSet<string>(_assemblyLoad.Plugins, StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> assemblyWorkflows = new HashSet<string>(_assemblyLoad.Workflows, StringComparer.InvariantCultureIgnoreCase);

            var pluginsOnlyInCrm = crmPlugins.Except(assemblyPlugins, StringComparer.InvariantCultureIgnoreCase);
            var workflowOnlyInCrm = crmWorkflows.Except(assemblyWorkflows, StringComparer.InvariantCultureIgnoreCase);

            var pluginsOnlyInLocalAssembly = assemblyPlugins.Except(crmPlugins, StringComparer.InvariantCultureIgnoreCase);
            var workflowOnlyInLocalAssembly = assemblyWorkflows.Except(crmWorkflows, StringComparer.InvariantCultureIgnoreCase);

            List<PluginTreeViewItem> listLocalAssembly = new List<PluginTreeViewItem>();
            List<PluginTreeViewItem> listMissingCrm = new List<PluginTreeViewItem>();

            foreach (var pluginTypeName in pluginsOnlyInLocalAssembly.OrderBy(s => s))
            {
                var nodeType = new PluginTreeViewItem(ComponentType.PluginType)
                {
                    Name = pluginTypeName,
                    Image = _imagePluginType,
                };

                listLocalAssembly.Add(nodeType);
            }

            foreach (var pluginTypeName in workflowOnlyInLocalAssembly.OrderBy(s => s))
            {
                var nodeType = new PluginTreeViewItem(ComponentType.PluginType)
                {
                    Name = pluginTypeName,
                    Image = _imageWorkflowActivity,

                    IsWorkflowActivity = true,
                };

                listLocalAssembly.Add(nodeType);
            }

            foreach (var pluginTypeName in pluginsOnlyInCrm.OrderBy(s => s))
            {
                var nodeType = new PluginTreeViewItem(ComponentType.PluginType)
                {
                    Name = pluginTypeName,
                    Image = _imagePluginType,
                };

                listMissingCrm.Add(nodeType);
            }

            foreach (var pluginTypeName in workflowOnlyInCrm.OrderBy(s => s))
            {
                var nodeType = new PluginTreeViewItem(ComponentType.PluginType)
                {
                    Name = pluginTypeName,
                    Image = _imageWorkflowActivity,

                    IsWorkflowActivity = true,
                };

                listMissingCrm.Add(nodeType);
            }

            this.Dispatcher.Invoke(() =>
            {
                foreach (var item in listLocalAssembly)
                {
                    _listLocalAssembly.Add(item);
                }
                this.trVPluginTreeNew.UpdateLayout();

                foreach (var item in listMissingCrm)
                {
                    _listMissingCrm.Add(item);
                }
                this.trVPluginTreeMissing.UpdateLayout();
            });
        }
    }
}