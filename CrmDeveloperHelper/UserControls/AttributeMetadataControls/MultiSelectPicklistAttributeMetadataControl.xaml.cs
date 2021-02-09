using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Text;
using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class MultiSelectPicklistAttributeMetadataControl : UserControl, IAttributeMetadataControl<MultiSelectPicklistAttributeMetadata>
    {
        public MultiSelectPicklistAttributeMetadata AttributeMetadata { get; }

        private readonly HashSet<int> _initialValues;

        private readonly bool _allwaysAddToEntity;

        public MultiSelectPicklistAttributeMetadataControl(MultiSelectPicklistAttributeMetadata attributeMetadata, OptionSetValueCollection initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValues = new HashSet<int>();

            if (initialValue != null)
            {
                foreach (var item in initialValue)
                {
                    this._initialValues.Add(item.Value);
                }
            }

            this._allwaysAddToEntity = allwaysAddToEntity;
            this.AttributeMetadata = attributeMetadata;

            FillListBox();

            Views.WindowBase.SetElementsEnabled(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabled(showRestoreButton, btnRestore, chBChanged);
        }

        private class CheckListBoxItem : INotifyPropertyChanging, INotifyPropertyChanged
        {
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

            private int _Value;
            public int Value
            {
                get
                {
                    return _Value;
                }
                set
                {
                    if (_Value == value)
                    {
                        return;
                    }

                    this.OnPropertyChanging(nameof(Value));
                    this._Value = value;
                    this.OnPropertyChanged(nameof(Value));
                }
            }

            private string _Content;
            public string Content
            {
                get
                {
                    return _Content;
                }
                set
                {
                    if (_Content == value)
                    {
                        return;
                    }

                    this.OnPropertyChanging(nameof(Content));
                    this._Content = value;
                    this.OnPropertyChanged(nameof(Content));
                }
            }

            private bool _IsChecked;
            public bool IsChecked
            {
                get
                {
                    return _IsChecked;
                }
                set
                {
                    if (_IsChecked == value)
                    {
                        return;
                    }

                    this.OnPropertyChanging(nameof(IsChecked));
                    this._IsChecked = value;
                    this.OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        private void FillListBox()
        {
            var optionSetOptions = AttributeMetadata.OptionSet.Options;

            if (_initialValues.Any())
            {
                foreach (var item in _initialValues.OrderBy(v => v))
                {
                    if (optionSetOptions.Any(o => o.Value == item))
                    {
                        continue;
                    }

                    var newItem = new CheckListBoxItem()
                    {
                        Content = string.Format("{0} - UnknownValue", item),
                        Value = item,
                        IsChecked = true,
                    };

                    newItem.PropertyChanged += this.checkListBoxItem_PropertyChanged;

                    lstBValues.Items.Add(newItem);
                }
            }

            foreach (var item in optionSetOptions.OrderBy(o => o.Value))
            {
                StringBuilder name = new StringBuilder();

                name.Append(item.Value);

                var label = CreateFileHandler.GetLocalizedLabel(item.Label);
                var description = CreateFileHandler.GetLocalizedLabel(item.Description);

                if (!string.IsNullOrEmpty(label))
                {
                    name.AppendFormat(" - {0}", label);
                }
                else if (!string.IsNullOrEmpty(description))
                {
                    name.AppendFormat(" - {0}", description);
                }

                var newItem = new CheckListBoxItem()
                {
                    Content = name.ToString(),
                    Value = item.Value.Value,
                    IsChecked = _initialValues.Contains(item.Value.Value),
                };

                newItem.PropertyChanged += this.checkListBoxItem_PropertyChanged;

                lstBValues.Items.Add(newItem);
            }
        }

        private void checkListBoxItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var currentValues = GetCurrentValues();

            chBChanged.IsChecked = !_initialValues.SetEquals(currentValues);
        }

        private HashSet<int> GetCurrentValues()
        {
            HashSet<int> currentValue = new HashSet<int>();

            foreach (var item in lstBValues.Items.OfType<CheckListBoxItem>())
            {
                if (item.IsChecked)
                {
                    currentValue.Add(item.Value);
                }
            }

            return currentValue;
        }

        public void AddAttribute(Entity entity)
        {
            var currentValues = GetCurrentValues();

            if (currentValues.Any())
            {
                entity.Attributes[AttributeMetadata.LogicalName] = new OptionSetValueCollection(currentValues.Select(o => new OptionSetValue(o)).ToList());
            }
            else
            {
                entity.Attributes[AttributeMetadata.LogicalName] = null;
            }
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValues = GetCurrentValues();

            if (this._allwaysAddToEntity
                || !_initialValues.SetEquals(currentValues)
            )
            {
                if (currentValues.Any())
                {
                    entity.Attributes[AttributeMetadata.LogicalName] = new OptionSetValueCollection(currentValues.Select(o => new OptionSetValue(o)).ToList());
                }
                else
                {
                    entity.Attributes[AttributeMetadata.LogicalName] = null;
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            lstBValues.Focus();

            base.OnGotFocus(e);
        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in lstBValues.Items.OfType<CheckListBoxItem>())
            {
                item.IsChecked = _initialValues.Contains(item.Value);
            }
        }

        private void CheckAllOptionSetValues(bool isChecked)
        {
            foreach (var item in lstBValues.Items.OfType<CheckListBoxItem>())
            {
                item.IsChecked = isChecked;
            }
        }

        private void hypLinkSelectAll_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            CheckAllOptionSetValues(true);
        }

        private void hypLinkDeselectAll_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            CheckAllOptionSetValues(false);
        }
    }
}