using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowCommonConfiguration : WindowBase
    {
        private readonly object sysObjectUtils = new object();

        private readonly CommonConfiguration _config;

        private readonly ObservableCollection<ListViewItem> _sourceFileActions = new ObservableCollection<ListViewItem>();

        public WindowCommonConfiguration(CommonConfiguration config)
        {
            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._config = config;

            InitializeComponent();

            foreach (var extension in _config.FileActionsByExtensions.Keys.OrderBy(s => s))
            {
                _sourceFileActions.Add(new ListViewItem()
                {
                    Extension = extension,
                    FileAction = _config.FileActionsByExtensions[extension],
                });
            }

            lstVwFileActions.ItemsSource = _sourceFileActions;

            LoadFields();
        }

        private void LoadFields()
        {
            chBClearOutputWindow.IsChecked = _config.ClearOutputWindowBeforeCRMOperation;

            chBDoNotPromtPublishMessage.IsChecked = _config.DoNotPropmPublishMessage;

            txtBFolderForExport.Text = _config.FolderForExport;

            txtBCompareProgram.Text = _config.CompareProgram;

            txtBCompareArgumentsFormat.Text = _config.CompareArgumentsFormat;

            txtBCompareArgumentsThreeWayFormat.Text = _config.CompareArgumentsThreeWayFormat;

            txtBTextEditorProgram.Text = _config.TextEditorProgram;

            txtBFileNameFormsEvents.Text = _config.FormsEventsFileName;

            txtBFileNamePluginConfiguration.Text = _config.PluginConfigurationFileName;
        }

        private void WriteFields()
        {
            _config.ClearOutputWindowBeforeCRMOperation = chBClearOutputWindow.IsChecked.GetValueOrDefault();

            _config.DoNotPropmPublishMessage = chBDoNotPromtPublishMessage.IsChecked.GetValueOrDefault();

            _config.FolderForExport = txtBFolderForExport.Text.Trim();

            _config.CompareProgram = txtBCompareProgram.Text.Trim();

            _config.CompareArgumentsFormat = txtBCompareArgumentsFormat.Text.Trim();

            _config.CompareArgumentsThreeWayFormat = txtBCompareArgumentsThreeWayFormat.Text.Trim();

            _config.TextEditorProgram = txtBTextEditorProgram.Text.Trim();

            _config.FormsEventsFileName = txtBFileNameFormsEvents.Text.Trim();

            _config.PluginConfigurationFileName = txtBFileNamePluginConfiguration.Text.Trim();

            if (string.IsNullOrEmpty(_config.PluginConfigurationFileName))
            {
                _config.PluginConfigurationFileName = "Plugins Configuration";
            }

            if (string.IsNullOrEmpty(_config.FormsEventsFileName))
            {
                _config.FormsEventsFileName = "System Forms Events";
            }

            _config.Save();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ConfigIsValid())
            {
                return;
            }

            WriteFields();

            this.Close();
        }

        private bool ConfigIsValid()
        {
            StringBuilder message = new StringBuilder();

            {
                string pathExport = txtBFolderForExport.Text.Trim();

                if (!string.IsNullOrEmpty(pathExport))
                {
                    if (!Directory.Exists(pathExport))
                    {
                        if (message.Length > 0) { message.AppendLine(); }

                        message.Append(Properties.MessageBoxStrings.FolderForExportDoesNotExists);
                    }
                }
            }

            {
                string pathFile = txtBCompareProgram.Text.Trim();

                if (!string.IsNullOrEmpty(pathFile))
                {
                    if (File.Exists(pathFile))
                    {
                        {
                            string format = txtBCompareArgumentsFormat.Text.Trim();

                            if (string.IsNullOrEmpty(format))
                            {
                                if (message.Length > 0) { message.AppendLine(); }

                                message.Append(Properties.MessageBoxStrings.DifferenceTwoFilesArgumentsIsEmpty);
                            }
                            else
                            {
                                if (!format.Contains("%f1"))
                                {
                                    if (message.Length > 0) { message.AppendLine(); }

                                    message.Append(Properties.MessageBoxStrings.DifferenceTwoFilesArgumentsNotContainsF1);
                                }

                                if (!format.Contains("%f2"))
                                {
                                    if (message.Length > 0) { message.AppendLine(); }

                                    message.Append(Properties.MessageBoxStrings.DifferenceTwoFilesArgumentsNotContainsF2);
                                }
                            }
                        }

                        {
                            string formatThreeWay = txtBCompareArgumentsThreeWayFormat.Text.Trim();

                            if (!string.IsNullOrEmpty(formatThreeWay))
                            {
                                if (!formatThreeWay.Contains("%f1"))
                                {
                                    if (message.Length > 0) { message.AppendLine(); }

                                    message.Append(Properties.MessageBoxStrings.ThreeWayDifferenceArgumentsNotContainsF1);
                                }

                                if (!formatThreeWay.Contains("%f2"))
                                {
                                    if (message.Length > 0) { message.AppendLine(); }

                                    message.Append(Properties.MessageBoxStrings.ThreeWayDifferenceArgumentsNotContainsF2);
                                }

                                if (!formatThreeWay.Contains("%fl"))
                                {
                                    if (message.Length > 0) { message.AppendLine(); }

                                    message.Append(Properties.MessageBoxStrings.ThreeWayDifferenceArgumentsNotContainsFLocal);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (message.Length > 0) { message.AppendLine(); }

                        message.Append(Properties.MessageBoxStrings.DifferenceProgramNotExists);
                    }
                }
            }

            {
                string pathFile = txtBTextEditorProgram.Text.Trim();

                if (!string.IsNullOrEmpty(pathFile))
                {
                    if (!File.Exists(pathFile))
                    {
                        if (message.Length > 0) { message.AppendLine(); }

                        message.Append(Properties.MessageBoxStrings.TextEditorProgramNotExists);
                    }
                }
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        private void btnClearCacheTranslations_Click(object sender, RoutedEventArgs e)
        {
            TranslationRepository.ClearCache();
        }

        private void btnOpenConfigFolder_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this._config.Path))
            {
                string folder = Path.GetDirectoryName(this._config.Path);

                if (Directory.Exists(folder))
                {
                    try
                    {
                        Process.Start(folder);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private void btnSelectFolderForExport_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtBFolderForExport.Text = dialog.SelectedPath;
                }
            }
        }

        private class ListViewItem
        {
            public string Extension { get; set; }

            public FileAction FileAction { get; set; }
        }

        private void lstVwFileActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsFileActions();
        }

        private void UpdateButtonsFileActions()
        {
            this.lstVwFileActions.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwFileActions.SelectedItems.Count > 0;

                    UIElement[] list = { tSBFileActionEdit, tSBFileActionDelete };

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

        private void tSBFileActionCreate_Click(object sender, RoutedEventArgs e)
        {
            var form = new WindowFileActionByExtension(string.Empty, _config.DefaultFileAction);

            if (form.ShowDialog().GetValueOrDefault())
            {
                string extension = form.SelectedExtension;
                var fileAction = form.GetFileAction();

                this.Dispatcher.Invoke(() =>
                {
                    if (!string.IsNullOrEmpty(extension) && !_config.FileActionsByExtensions.ContainsKey(extension))
                    {
                        _config.FileActionsByExtensions.TryAdd(extension, fileAction);
                        _sourceFileActions.Add(new ListViewItem()
                        {
                            Extension = extension,
                            FileAction = fileAction,
                        });
                    }
                });
            }

            UpdateButtonsFileActions();
        }

        private void tSBFileActionEdit_Click(object sender, RoutedEventArgs e)
        {
            PerformFileActionEdit();
        }

        private void lstVwFileActions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PerformFileActionEdit();
        }

        private void PerformFileActionEdit()
        {
            if (lstVwFileActions.SelectedItems.Count == 1)
            {
                ListViewItem itemExtension = lstVwFileActions.SelectedItems[0] as ListViewItem;

                var form = new WindowFileActionByExtension(itemExtension.Extension, itemExtension.FileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    string extension = form.SelectedExtension.ToLower();
                    var fileAction = form.GetFileAction();

                    this.Dispatcher.Invoke(() =>
                    {
                        if (!string.IsNullOrEmpty(extension))
                        {
                            _sourceFileActions.Remove(itemExtension);
                            _config.FileActionsByExtensions.TryRemove(itemExtension.Extension, out _);

                            _config.FileActionsByExtensions.TryAdd(extension, fileAction);
                            _sourceFileActions.Add(new ListViewItem()
                            {
                                Extension = extension,
                                FileAction = fileAction,
                            });
                        }
                    });
                }

                UpdateButtonsFileActions();
            }
        }

        private void tSBFileActionDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwFileActions.SelectedItems.Count == 1)
            {
                ListViewItem itemExtension = lstVwFileActions.SelectedItems[0] as ListViewItem;

                string message = string.Format(Properties.MessageBoxStrings.DeleteFileActionFormat1, itemExtension.Extension);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    _sourceFileActions.Remove(itemExtension);

                    _config.FileActionsByExtensions.TryRemove(itemExtension.Extension, out _);
                }

                UpdateButtonsFileActions();
            }
        }

        private void lstVwFileActions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.ContinueRouting = false;
        }

        private void lstVwFileActions_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            tSBFileActionDelete_Click(sender, e);
        }
    }
}