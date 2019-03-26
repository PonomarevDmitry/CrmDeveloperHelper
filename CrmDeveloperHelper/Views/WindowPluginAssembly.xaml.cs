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
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;

        private readonly string _defaultOutputFilePath;
        private readonly EnvDTE.Project _project;

        private readonly ObservableCollection<PluginTreeViewItem> _listLocalAssembly = new ObservableCollection<PluginTreeViewItem>();
        private readonly ObservableCollection<PluginTreeViewItem> _listMissingCrm = new ObservableCollection<PluginTreeViewItem>();

        private readonly BitmapImage _imagePluginAssembly;
        private readonly BitmapImage _imagePluginType;
        private readonly BitmapImage _imageWorkflowActivity;

        public PluginAssembly PluginAssembly { get; private set; }

        private AssemblyReaderResult _assemblyLoad;

        public WindowPluginAssembly(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , PluginAssembly pluginAssembly
            , string defaultOutputFilePath
            , EnvDTE.Project project
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._defaultOutputFilePath = defaultOutputFilePath;
            this._project = project;
            this.PluginAssembly = pluginAssembly;

            this._imagePluginAssembly = this.Resources["ImagePluginAssembly"] as BitmapImage;
            this._imagePluginType = this.Resources["ImagePluginType"] as BitmapImage;
            this._imageWorkflowActivity = this.Resources["ImageWorkflowActivity"] as BitmapImage;

            InitializeComponent();

            LoadFromConfig();

            this.trVPluginTreeNew.ItemsSource = _listLocalAssembly;
            this.trVPluginTreeMissing.ItemsSource = _listMissingCrm;
            this.tSSLblConnectionName.Content = _service.ConnectionData.Name;

            btnBuildProject.IsEnabled = this._project != null;
            btnBuildProject.Visibility = btnBuildProject.IsEnabled ? Visibility.Visible : Visibility.Hidden;

            LoadEntityPluginAssemblyProperties();

            this.DecreaseInit();
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
                string lastAssemblyPath = _service.ConnectionData.GetLastAssemblyPath(this.PluginAssembly.Name);
                List<string> lastPaths = _service.ConnectionData.GetAssemblyPaths(this.PluginAssembly.Name).ToList();

                if (!string.IsNullOrEmpty(_defaultOutputFilePath)
                    && !lastPaths.Contains(_defaultOutputFilePath, StringComparer.InvariantCultureIgnoreCase)
                )
                {
                    lastPaths.Insert(0, _defaultOutputFilePath);
                }

                foreach (var path in lastPaths)
                {
                    cmBAssemblyToLoad.Items.Add(path);
                }

                if (!string.IsNullOrEmpty(lastAssemblyPath))
                {
                    cmBAssemblyToLoad.Text = lastAssemblyPath;
                }
                else if (!string.IsNullOrEmpty(_defaultOutputFilePath))
                {
                    cmBAssemblyToLoad.Text = _defaultOutputFilePath;
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

            var updateAssembly = new PluginAssembly();

            if (PluginAssembly.PluginAssemblyId.HasValue)
            {
                updateAssembly.Id = PluginAssembly.PluginAssemblyId.Value;
                updateAssembly.PluginAssemblyId = PluginAssembly.PluginAssemblyId;
            }
            else
            {
                updateAssembly.Name = _assemblyLoad.Name;
            }

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

            var listToRegister = _listLocalAssembly.Where(p => p.IsChecked).ToList();

            _service.ConnectionData.AddAssemblyMapping(_assemblyLoad.Name, _assemblyLoad.FilePath);
            _service.ConnectionData.Save();

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingPluginAssemblyFormat1, _service.ConnectionData.Name);

            try
            {
                this.PluginAssembly.Id = await _service.UpsertAsync(updateAssembly);

                if (listToRegister.Any())
                {
                    var assemblyRef = this.PluginAssembly.ToEntityReference();

                    ToggleControls(false, Properties.WindowStatusStrings.RegisteringNewPluginTypesFormat2, _service.ConnectionData.Name, listToRegister.Count);

                    foreach (var pluginType in listToRegister)
                    {
                        var pluginTypeEntity = new PluginType()
                        {
                            Name = pluginType.Name,
                            TypeName = pluginType.Name,
                            FriendlyName = pluginType.Name,

                            PluginAssemblyId = assemblyRef,
                        };

                        ToggleControls(false, Properties.WindowStatusStrings.RegisteringPluginTypeFormat2, _service.ConnectionData.Name, pluginType);

                        try
                        {
                            pluginTypeEntity.Id = await _service.CreateAsync(pluginTypeEntity);

                            ToggleControls(true, Properties.WindowStatusStrings.RegisteringPluginTypeCompletedFormat2, _service.ConnectionData.Name, pluginType);
                        }
                        catch (Exception ex)
                        {
                            ToggleControls(true, Properties.WindowStatusStrings.RegisteringPluginTypeFailedFormat2, _service.ConnectionData.Name, pluginType);

                            _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                            _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                        }
                    }

                    ToggleControls(true, Properties.WindowStatusStrings.RegisteringNewPluginTypesCompletedFormat2, _service.ConnectionData.Name, listToRegister.Count);
                }

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingPluginAssemblyCompletedFormat1, _service.ConnectionData.Name);

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
            UpdateTextBoxFileNameOnServerReadOnly();
        }

        private void UpdateTextBoxFileNameOnServerReadOnly()
        {
            var isDisk = this.IsControlsEnabled && rBDisk.IsChecked.GetValueOrDefault();

            txtBFileNameOnServer.IsReadOnly = !isDisk;
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar
                , trVPluginTreeNew

                , trVPluginTreeMissing
                , btnClose

                , btnSelectFile
                , btnLoadAssembly
                , btnBuildProject

                , txtBDescription

                , txtBFileNameOnServer
                , cmBAssemblyToLoad

                , rBDatabase

                , rBDisk
                , rBGAC
                , rBSandBox
                , rBNone
            );

            ToggleControl(btnSave);

            UpdateTextBoxFileNameOnServerReadOnly();
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
                var tempList = _service.ConnectionData.GetAssemblyPaths(this.PluginAssembly.Name).ToList();

                if (!string.IsNullOrEmpty(_defaultOutputFilePath)
                    && !tempList.Contains(_defaultOutputFilePath, StringComparer.InvariantCultureIgnoreCase)
                )
                {
                    tempList.Insert(0, _defaultOutputFilePath);
                }

                lastPaths = tempList;
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
                else if (!string.IsNullOrEmpty(_defaultOutputFilePath))
                {
                    openFileDialog1.InitialDirectory = Path.GetDirectoryName(_defaultOutputFilePath);
                    openFileDialog1.FileName = Path.GetFileName(_defaultOutputFilePath);
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

        private void cmBAssemblyToLoad_KeyDown(object sender, KeyEventArgs e)
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
                UpdateStatus(Properties.WindowStatusStrings.FileNotExists);
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingAssemblyFromPathFormat1, assemblyPath);

            AssemblyReaderResult assemblyCheck = null;

            using (var reader = new AssemblyReader())
            {
                assemblyCheck = reader.ReadAssembly(assemblyPath);
            }

            if (assemblyCheck == null)
            {
                ToggleControls(true, Properties.WindowStatusStrings.LoadingAssemblyFromPathFailedFormat1, assemblyPath);
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

            ToggleControls(true, Properties.WindowStatusStrings.LoadingAssemblyFromPathCompletedFormat1, assemblyPath);
        }

        private async void btnBuildProject_Click(object sender, RoutedEventArgs e)
        {
            if (_project == null)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.BuildingProjectFormat1, _project.Name);

            var buildResult = await _iWriteToOutput.BuildProjectAsync(_project);

            if (buildResult == 0)
            {
                ToggleControls(true, Properties.WindowStatusStrings.BuildingProjectCompletedFormat1, _project.Name);
            }
            else
            {
                ToggleControls(true, Properties.WindowStatusStrings.BuildingProjectFailedFormat1, _project.Name);
            }
        }
    }
}