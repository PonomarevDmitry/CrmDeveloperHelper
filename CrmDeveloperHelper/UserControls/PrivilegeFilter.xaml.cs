using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for PrivilegeFilter.xaml
    /// </summary>
    public partial class PrivilegeFilter : UserControl
    {
        public static TupleList<string, Func<Privilege, bool>> _filters = new TupleList<string, Func<Privilege, bool>>()
        {
              { nameof(Privilege.CanBeBasic)                   , e => e.CanBeBasic.HasValue                  && e.CanBeBasic.Value                  }
            , { nameof(Privilege.CanBeLocal)                   , e => e.CanBeLocal.HasValue                  && e.CanBeLocal.Value                  }
            , { nameof(Privilege.CanBeDeep)                    , e => e.CanBeDeep.HasValue                   && e.CanBeDeep.Value                   }
            , { nameof(Privilege.CanBeGlobal)                  , e => e.CanBeGlobal.HasValue                 && e.CanBeGlobal.Value                 }

            , { nameof(Privilege.CanBeEntityReference)         , e => e.CanBeEntityReference.HasValue        && e.CanBeEntityReference.Value        }
            , { nameof(Privilege.CanBeParentEntityReference)   , e => e.CanBeParentEntityReference.HasValue  && e.CanBeParentEntityReference.Value  }

            , { nameof(Privilege.IsManaged)                    , e => e.IsManaged.HasValue                   && e.IsManaged.Value                   }

            //, { nameof(Privilege.IsSolutionAware)            , e => e.IsSolutionAware.HasValue             && e.IsSolutionAware.Value             }
        };

        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public bool FilterChanged { get; private set; } = false;

        private const int _countInColumn = 20;

        public PrivilegeFilter()
        {
            InitializeComponent();

            FillRoleEditorLayoutTabs();

            panelFilters.Children.Clear();

            StackPanel panel = null;

            foreach (var item in _filters)
            {
                var checkBox = new CheckBox()
                {
                    Name = item.Item1,
                    Content = item.Item1,
                    Tag = item.Item2,

                    IsThreeState = true,

                    IsChecked = null,
                };

                checkBox.Checked += this.CheckBox_Checked;
                checkBox.Indeterminate += this.CheckBox_Checked;
                checkBox.Unchecked += this.CheckBox_Checked;

                if (_checkBoxes.Count % _countInColumn == 0)
                {
                    panel = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                    };

                    panelFilters.Children.Add(panel);
                }

                _checkBoxes.Add(checkBox);

                panel.Children.Add(checkBox);
            }

            this.FilterChanged = false;
        }

        private void FillRoleEditorLayoutTabs()
        {
            cmBRoleEditorLayoutTabs.Items.Clear();

            cmBRoleEditorLayoutTabs.Items.Add("All");

            var tabs = RoleEditorLayoutTab.GetTabs();

            foreach (var tab in tabs)
            {
                cmBRoleEditorLayoutTabs.Items.Add(tab);
            }

            cmBRoleEditorLayoutTabs.SelectedIndex = 0;
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.FilterChanged = true;
        }

        private void cmBRoleEditorLayoutTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.FilterChanged = true;
        }

        public event EventHandler<EventArgs> CloseClicked;

        private void OnCloseClicked()
        {
            CloseClicked?.Invoke(this, new EventArgs());
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

                    OnCloseClicked();

                    return;
                }
            }

            base.OnKeyDown(e);
        }

        public IEnumerable<Privilege> FilterList(IEnumerable<Privilege> list)
        {
            return FilterList<Privilege>(list, e => e);
        }

        public IEnumerable<T> FilterList<T>(IEnumerable<T> list, Func<T, Privilege> selector)
        {
            this.FilterChanged = false;

            RoleEditorLayoutTab selectedTab = null;

            this.Dispatcher.Invoke(() =>
            {
                selectedTab = cmBRoleEditorLayoutTabs.SelectedItem as RoleEditorLayoutTab;
            });

            var funcs = GetFilters();

            var result = list;

            result = result.Where(e => funcs.All(f => f.Item1(selector(e)) == f.Item2));

            if (selectedTab != null)
            {
                result = result.Where(ent => selector(ent) != null && selector(ent).PrivilegeId.HasValue && selectedTab.PrivilegesHash.Contains(selector(ent).PrivilegeId.Value));
            }

            return result;
        }

        private TupleList<Func<Privilege, bool>, bool> GetFilters()
        {
            var funcs = new TupleList<Func<Privilege, bool>, bool>();

            this.Dispatcher.Invoke(() =>
            {
                foreach (var checkbox in _checkBoxes.Where(c => c.IsChecked.HasValue))
                {
                    if (checkbox.Tag is Func<Privilege, bool> func)
                    {
                        funcs.Add(func, checkbox.IsChecked.Value);
                    }
                }
            });

            return funcs;
        }

        private void hypLinkClear_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            foreach (var item in _checkBoxes)
            {
                item.IsChecked = null;
            }
        }
    }
}