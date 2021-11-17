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
    /// Interaction logic for AttributeMetadataFilter.xaml
    /// </summary>
    public partial class AttributeMetadataFilter : UserControl
    {
        public static TupleList<string, Func<AttributeMetadata, bool>> _filters = new TupleList<string, Func<AttributeMetadata, bool>>()
        {
            //IsSearchable
            //IsManaged
            //IsRequiredForForm
            //IsFilterable
            //IsValidForGrid
            //IsLogical
            //IsDataSourceSecret
            //IsValidForForm
            //IsRetrievable
            //IsSecured
            //CanBeSecuredForUpdate
            //IsCustomAttribute
            //IsPrimaryId
            //IsPrimaryName
            //IsValidForCreate
            //IsValidForRead
            //IsValidForUpdate
            //CanBeSecuredForRead
            //CanBeSecuredForCreate

              { nameof(AttributeMetadata.IsSearchable)               , e => e.IsSearchable.HasValue                && e.IsSearchable.Value              }
            , { nameof(AttributeMetadata.IsManaged)                  , e => e.IsManaged.HasValue                   && e.IsManaged.Value                 }
            , { nameof(AttributeMetadata.IsRequiredForForm)          , e => e.IsRequiredForForm.HasValue           && e.IsRequiredForForm.Value         }
            , { nameof(AttributeMetadata.IsFilterable)               , e => e.IsFilterable.HasValue                && e.IsFilterable.Value              }
            , { nameof(AttributeMetadata.IsValidForGrid)             , e => e.IsValidForGrid.HasValue              && e.IsValidForGrid.Value            }
            , { nameof(AttributeMetadata.IsLogical)                  , e => e.IsLogical.HasValue                   && e.IsLogical.Value                 }
            , { nameof(AttributeMetadata.IsDataSourceSecret)         , e => e.IsDataSourceSecret.HasValue          && e.IsDataSourceSecret.Value        }
            , { nameof(AttributeMetadata.IsValidForForm)             , e => e.IsValidForForm.HasValue              && e.IsValidForForm.Value            }
            , { nameof(AttributeMetadata.IsRetrievable)              , e => e.IsRetrievable.HasValue               && e.IsRetrievable.Value             }
            , { nameof(AttributeMetadata.IsSecured)                  , e => e.IsSecured.HasValue                   && e.IsSecured.Value                 }
            , { nameof(AttributeMetadata.CanBeSecuredForUpdate)      , e => e.CanBeSecuredForUpdate.HasValue       && e.CanBeSecuredForUpdate.Value     }
            , { nameof(AttributeMetadata.IsCustomAttribute)          , e => e.IsCustomAttribute.HasValue           && e.IsCustomAttribute.Value         }
            , { nameof(AttributeMetadata.IsPrimaryId)                , e => e.IsPrimaryId.HasValue                 && e.IsPrimaryId.Value               }
            , { nameof(AttributeMetadata.IsPrimaryName)              , e => e.IsPrimaryName.HasValue               && e.IsPrimaryName.Value             }
            , { nameof(AttributeMetadata.IsValidForCreate)           , e => e.IsValidForCreate.HasValue            && e.IsValidForCreate.Value          }
            , { nameof(AttributeMetadata.IsValidForRead)             , e => e.IsValidForRead.HasValue              && e.IsValidForRead.Value            }
            , { nameof(AttributeMetadata.IsValidForUpdate)           , e => e.IsValidForUpdate.HasValue            && e.IsValidForUpdate.Value          }
            , { nameof(AttributeMetadata.CanBeSecuredForRead)        , e => e.CanBeSecuredForRead.HasValue         && e.CanBeSecuredForRead.Value       }
            , { nameof(AttributeMetadata.CanBeSecuredForCreate)      , e => e.CanBeSecuredForCreate.HasValue       && e.CanBeSecuredForCreate.Value     }

            //, { nameof(AttributeMetadata.IsSolutionAware)          , e => e.IsSolutionAware.HasValue             && e.IsSolutionAware.Value           }

            //IsGlobalFilterEnabled
            //IsSortableEnabled
            //IsCustomizable
            //IsRenameable
            //IsValidForAdvancedFind
            //CanModifyAdditionalSettings
            //IsAuditEnabled

            , { nameof(AttributeMetadata.IsGlobalFilterEnabled)          , e => e.IsGlobalFilterEnabled != null         && e.IsGlobalFilterEnabled.Value          }
            , { nameof(AttributeMetadata.IsSortableEnabled)              , e => e.IsSortableEnabled != null             && e.IsSortableEnabled.Value              }
            , { nameof(AttributeMetadata.IsCustomizable)                 , e => e.IsCustomizable != null                && e.IsCustomizable.Value                 }
            , { nameof(AttributeMetadata.IsRenameable)                   , e => e.IsRenameable != null                  && e.IsRenameable.Value                   }
            , { nameof(AttributeMetadata.IsValidForAdvancedFind)         , e => e.IsValidForAdvancedFind != null        && e.IsValidForAdvancedFind.Value         }
            , { nameof(AttributeMetadata.CanModifyAdditionalSettings)    , e => e.CanModifyAdditionalSettings != null   && e.CanModifyAdditionalSettings.Value    }
            , { nameof(AttributeMetadata.IsAuditEnabled)                 , e => e.IsAuditEnabled != null                && e.IsAuditEnabled.Value                 }
        };

        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public bool FilterChanged { get; private set; } = false;

        private const int _countInColumn = 20;

        public AttributeMetadataFilter()
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

        public IEnumerable<AttributeMetadata> FilterList(IEnumerable<AttributeMetadata> list)
        {
            return FilterList<AttributeMetadata>(list, e => e);
        }

        public IEnumerable<T> FilterList<T>(IEnumerable<T> list, Func<T, AttributeMetadata> selector)
        {
            this.FilterChanged = false;

            var funcs = GetFilters();

            var result = list;

            result = result.Where(e => funcs.All(f => f.Item1(selector(e)) == f.Item2));

            return result;
        }

        private TupleList<Func<AttributeMetadata, bool>, bool> GetFilters()
        {
            var funcs = new TupleList<Func<AttributeMetadata, bool>, bool>();

            this.Dispatcher.Invoke(() =>
            {
                foreach (var checkbox in _checkBoxes.Where(c => c.IsChecked.HasValue))
                {
                    if (checkbox.Tag is Func<AttributeMetadata, bool> func)
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