using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowBase : Window
    {
        private bool loaded = false;

        public WindowBase()
        {
            var winConfig = this.GetWindowsSettings();

            LoadConfiguration(winConfig);
        }

        protected WindowSettings GetWindowsSettings()
        {
            var name = this.Name;

            if (string.IsNullOrEmpty(name))
            {
                name = this.GetType().Name;
            }

            return FileOperations.GetWindowConfiguration(name);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!loaded)
            {
                loaded = true;

                var winConfig = this.GetWindowsSettings();

                LoadConfiguration(winConfig);

                LoadConfigurationInternal(winConfig);
            }
        }

        private void LoadConfiguration(WindowSettings winConfig)
        {
            if (winConfig.Top.HasValue)
            {
                this.Top = winConfig.Top.Value;
                this.WindowStartupLocation = WindowStartupLocation.Manual;
            }

            if (winConfig.Left.HasValue)
            {
                this.Left = winConfig.Left.Value;
                this.WindowStartupLocation = WindowStartupLocation.Manual;
            }

            if (winConfig.WindowState != WindowState.Minimized)
            {
                this.WindowState = winConfig.WindowState;
            }

            if (this.ResizeMode != System.Windows.ResizeMode.CanMinimize
                && this.ResizeMode != System.Windows.ResizeMode.NoResize
            )
            {
                if (winConfig.Width.HasValue)
                {
                    this.Width = winConfig.Width.Value;
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                }

                if (winConfig.Height.HasValue)
                {
                    this.Height = winConfig.Height.Value;
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                }
            }

            foreach (var item in FindChildren<ListView>(this))
            {
                LoadListViewColumnsWidths(item, winConfig);
            }

            foreach (var item in FindChildren<DataGrid>(this))
            {
                LoadDataGridColumnsWidths(item, winConfig);
            }
        }

        protected static IEnumerable<T> FindChildren<T>(DependencyObject source) where T : DependencyObject
        {
            if (source != null)
            {
                var childs = GetChildObjects(source);
                foreach (DependencyObject child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //recurse tree
                    foreach (T descendant in FindChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        protected static IEnumerable<DependencyObject> GetChildObjects(DependencyObject parent)
        {
            if (parent == null) yield break;

            if (parent is ContentElement || parent is FrameworkElement)
            {
                //use the logical tree for content / framework elements
                foreach (object obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null) yield return (DependencyObject)obj;
                }
            }
            else
            {
                //use the visual tree per default
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }

        private void SaveConfiguration()
        {
            var winConfig = this.GetWindowsSettings();

            winConfig.WindowState = this.WindowState;

            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                winConfig.Top = this.Top;
                winConfig.Left = this.Left;

                if (this.ResizeMode != System.Windows.ResizeMode.CanMinimize
                    && this.ResizeMode != System.Windows.ResizeMode.NoResize
                )
                {
                    winConfig.Width = this.ActualWidth;
                    winConfig.Height = this.ActualHeight;
                }
            }

            SaveConfigurationInternal(winConfig);

            foreach (var item in FindChildren<ListView>(this))
            {
                SaveListViewColumnsWidths(item, winConfig);
            }

            foreach (var item in FindChildren<DataGrid>(this))
            {
                SaveDataGridColumnsWidths(item, winConfig);
            }

            winConfig.Save();
        }

        private void LoadListViewColumnsWidths(ListView listView, WindowSettings winConfig)
        {
            if (winConfig.GridViewColumnsWidths == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(listView.Name))
            {
                return;
            }

            if (!(listView.View is GridView))
            {
                return;
            }

            var gridView = listView.View as GridView;

            foreach (var column in gridView.Columns)
            {
                if (column.Header is string)
                {
                    if (!string.IsNullOrEmpty(column.Header.ToString()))
                    {
                        var name = string.Format("{0}.{1}", listView.Name, column.Header.ToString());

                        if (winConfig.GridViewColumnsWidths.ContainsKey(name))
                        {
                            column.Width = winConfig.GridViewColumnsWidths[name];
                        }
                    }
                }
            }
        }

        private void SaveListViewColumnsWidths(ListView listView, WindowSettings winConfig)
        {
            if (string.IsNullOrEmpty(listView.Name))
            {
                return;
            }

            if (!(listView.View is GridView))
            {
                return;
            }

            var gridView = listView.View as GridView;

            foreach (var column in gridView.Columns)
            {
                if (column.Header is string)
                {
                    if (!string.IsNullOrEmpty(column.Header.ToString()))
                    {
                        var name = string.Format("{0}.{1}", listView.Name, column.Header.ToString());

                        winConfig.GridViewColumnsWidths[name] = column.ActualWidth;
                    }
                }
            }
        }

        protected void LoadDataGridColumnsWidths(DataGrid dataGrid, WindowSettings winConfig)
        {
            if (winConfig.GridViewColumnsWidths == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(dataGrid.Name))
            {
                return;
            }

            foreach (var column in dataGrid.Columns)
            {
                if (column.Header is string)
                {
                    if (!string.IsNullOrEmpty(column.Header.ToString()))
                    {
                        var name = string.Format("{0}.{1}", dataGrid.Name, column.Header.ToString());

                        if (winConfig.GridViewColumnsWidths.ContainsKey(name))
                        {
                            column.Width = winConfig.GridViewColumnsWidths[name];
                        }
                    }
                }
            }
        }

        protected void SaveDataGridColumnsWidths(DataGrid dataGrid, WindowSettings winConfig)
        {
            if (string.IsNullOrEmpty(dataGrid.Name))
            {
                return;
            }

            foreach (var column in dataGrid.Columns)
            {
                if (column.Header is string)
                {
                    if (!string.IsNullOrEmpty(column.Header.ToString()))
                    {
                        var name = string.Format("{0}.{1}", dataGrid.Name, column.Header.ToString());

                        winConfig.GridViewColumnsWidths[name] = column.ActualWidth;
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveConfiguration();

            base.OnClosed(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    e.Handled = true;

                    this.Close();

                    return;
                }
            }

            base.OnKeyDown(e);
        }

        protected virtual void LoadConfigurationInternal(WindowSettings winConfig)
        {

        }

        protected virtual void SaveConfigurationInternal(WindowSettings winConfig)
        {

        }
    }
}
