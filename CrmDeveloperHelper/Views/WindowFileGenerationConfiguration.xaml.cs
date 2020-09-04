using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowFileGenerationConfiguration : WindowBase
    {
        private readonly object sysObjectUtils = new object();

        private readonly FileGenerationConfiguration _config;

        private readonly ObservableCollection<ListViewItem> _sourceOptions = new ObservableCollection<ListViewItem>();

        public WindowFileGenerationConfiguration(FileGenerationConfiguration config)
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this._config = config;

            foreach (var solutionFilePath in _config.FileGenerationOptionsCollection.Keys.OrderBy(s => s))
            {
                _sourceOptions.Add(new ListViewItem(solutionFilePath, _config.FileGenerationOptionsCollection[solutionFilePath]));
            }

            lstVwOptions.ItemsSource = _sourceOptions;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this._config.Save();
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    catch (Exception ex)
                    {
                        Helpers.DTEHelper.WriteExceptionToLog(ex);
                    }
                }
            }
        }

        private class ListViewItem : INotifyPropertyChanging, INotifyPropertyChanged
        {
            public string SolutionFilePath { get; private set; }

            public FileGenerationOptions FileGenerationOptions { get; private set; }

            public string NamespaceClassesCSharp => this.FileGenerationOptions.NamespaceClassesCSharp;

            public string NamespaceClassesJavaScript => this.FileGenerationOptions.NamespaceClassesJavaScript;

            public string NamespaceGlobalOptionSetsCSharp => this.FileGenerationOptions.NamespaceGlobalOptionSetsCSharp;

            public string NamespaceGlobalOptionSetsJavaScript => this.FileGenerationOptions.NamespaceGlobalOptionSetsJavaScript;

            public string NamespaceSdkMessagesCSharp => this.FileGenerationOptions.NamespaceSdkMessagesCSharp;

            public string NamespaceSdkMessagesJavaScript => this.FileGenerationOptions.NamespaceSdkMessagesJavaScript;

            public ListViewItem(string solutionFilePath, FileGenerationOptions fileGenerationOptions)
            {
                this.SolutionFilePath = solutionFilePath;
                this.FileGenerationOptions = fileGenerationOptions;

                this.FileGenerationOptions.PropertyChanged += this.FileGenerationOptions_PropertyChanged;
                this.FileGenerationOptions.PropertyChanging += this.FileGenerationOptions_PropertyChanging;
            }

            private static HashSet<string> _watcheProperties = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                nameof(Model.FileGenerationOptions.NamespaceClassesCSharp)
                , nameof(Model.FileGenerationOptions.NamespaceClassesJavaScript)
                , nameof(Model.FileGenerationOptions.NamespaceGlobalOptionSetsCSharp)
                , nameof(Model.FileGenerationOptions.NamespaceGlobalOptionSetsJavaScript)
                , nameof(Model.FileGenerationOptions.NamespaceSdkMessagesCSharp)
                , nameof(Model.FileGenerationOptions.NamespaceSdkMessagesJavaScript)
            };

            private void FileGenerationOptions_PropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                if (_watcheProperties.Contains(e.PropertyName))
                {
                    PropertyChanging?.Invoke(this, e);
                }
            }

            private void FileGenerationOptions_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (_watcheProperties.Contains(e.PropertyName))
                {
                    PropertyChanged?.Invoke(this, e);
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public event PropertyChangingEventHandler PropertyChanging;

            private void OnPropertyChanged(string propertyName)
            {
                if ((this.PropertyChanged != null))
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private void OnPropertyChanging(string propertyName)
            {
                if ((this.PropertyChanging != null))
                {
                    this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                }
            }
        }

        private void lstVwOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsOptions();
        }

        private void UpdateButtonsOptions()
        {
            this.lstVwOptions.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwOptions.SelectedItems.Count > 0;

                    UIElement[] list = { tSBFileGenerationOptionsEdit, tSBFileGenerationOptionsDelete };

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

        private void tSBFileGenerationOptionsEdit_Click(object sender, RoutedEventArgs e)
        {
            PerformOptionsEdit();
        }

        private void lstVwOptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PerformOptionsEdit();
        }

        private void PerformOptionsEdit()
        {
            if (lstVwOptions.SelectedItems.Count == 1)
            {
                ListViewItem viewItem = lstVwOptions.SelectedItems[0] as ListViewItem;

                WindowHelper.OpenFileGenerationOptions(viewItem.FileGenerationOptions);

                UpdateButtonsOptions();
            }
        }

        private void tSBFileGenerationOptionsEditDefaultOptions_Click(object sender, RoutedEventArgs e)
        {
            WindowHelper.OpenFileGenerationOptions(_config.DefaultFileGenerationOptions);
        }

        private void tSBFileGenerationOptionsDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwOptions.SelectedItems.Count == 1)
            {
                ListViewItem viewItem = lstVwOptions.SelectedItems[0] as ListViewItem;

                string message = string.Format(Properties.MessageBoxStrings.DeleteFileActionFormat1, viewItem.SolutionFilePath);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    _sourceOptions.Remove(viewItem);

                    _config.FileGenerationOptionsCollection.Remove(viewItem.SolutionFilePath);
                }

                UpdateButtonsOptions();
            }
        }

        private void lstVwOptions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwOptions_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            tSBFileGenerationOptionsDelete_Click(sender, e);
        }
    }
}