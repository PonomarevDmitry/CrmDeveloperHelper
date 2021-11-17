using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ManyToManyRelationshipMetadataFilter.xaml
    /// </summary>
    public partial class ManyToManyRelationshipMetadataFilter : UserControl
    {
        public static TupleList<string, Func<ManyToManyRelationshipMetadata, bool>> _filters = new TupleList<string, Func<ManyToManyRelationshipMetadata, bool>>()
        {
            { nameof(ManyToManyRelationshipMetadata.IsManaged)                  , e => e.IsManaged.HasValue                   && e.IsManaged.Value                 }
            , { nameof(ManyToManyRelationshipMetadata.IsCustomRelationship)       , e => e.IsCustomRelationship.HasValue        && e.IsCustomRelationship.Value      }
            , { nameof(ManyToManyRelationshipMetadata.IsValidForAdvancedFind)     , e => e.IsValidForAdvancedFind.HasValue      && e.IsValidForAdvancedFind.Value    }

            //, { nameof(ManyToManyRelationshipMetadata.IsSolutionAware)          , e => e.IsSolutionAware.HasValue             && e.IsSolutionAware.Value           }

            //IsCustomizable

            , { nameof(ManyToManyRelationshipMetadata.IsCustomizable)                 , e => e.IsCustomizable != null                       && e.IsCustomizable.Value                                                            }

            , { "Entity1AssociatedMenuConfigurationIsCustomizable"                    , e => e.Entity1AssociatedMenuConfiguration != null   && (e.Entity1AssociatedMenuConfiguration?.IsCustomizable).GetValueOrDefault()        }
            , { "Entity1AssociatedMenuConfigurationAvailableOffline"                  , e => e.Entity1AssociatedMenuConfiguration != null   && (e.Entity1AssociatedMenuConfiguration?.AvailableOffline).GetValueOrDefault()      }

            , { "Entity2AssociatedMenuConfigurationIsCustomizable"                    , e => e.Entity2AssociatedMenuConfiguration != null   && (e.Entity2AssociatedMenuConfiguration?.IsCustomizable).GetValueOrDefault()        }
            , { "Entity2AssociatedMenuConfigurationAvailableOffline"                  , e => e.Entity2AssociatedMenuConfiguration != null   && (e.Entity2AssociatedMenuConfiguration?.AvailableOffline).GetValueOrDefault()      }
        };

        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public bool FilterChanged { get; private set; } = false;

        private const int _countInColumn = 20;

        public ManyToManyRelationshipMetadataFilter()
        {
            InitializeComponent();

            panelFilters.Children.Clear();

            StackPanel panel = null;

            foreach (var item in _filters.OrderBy(i => i.Item1))
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

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
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

        public IEnumerable<ManyToManyRelationshipMetadata> FilterList(IEnumerable<ManyToManyRelationshipMetadata> list)
        {
            return FilterList<ManyToManyRelationshipMetadata>(list, e => e);
        }

        public IEnumerable<T> FilterList<T>(IEnumerable<T> list, Func<T, ManyToManyRelationshipMetadata> selector)
        {
            this.FilterChanged = false;

            var funcs = GetFilters();

            var result = list;

            result = result.Where(e => funcs.All(f => f.Item1(selector(e)) == f.Item2));

            return result;
        }

        private TupleList<Func<ManyToManyRelationshipMetadata, bool>, bool> GetFilters()
        {
            var funcs = new TupleList<Func<ManyToManyRelationshipMetadata, bool>, bool>();

            this.Dispatcher.Invoke(() =>
            {
                foreach (var checkbox in _checkBoxes.Where(c => c.IsChecked.HasValue))
                {
                    if (checkbox.Tag is Func<ManyToManyRelationshipMetadata, bool> func)
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