using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSdkMessageProcessingStepImage : WindowWithSingleConnection
    {
        private EntityMetadata _entityMetadata;

        private readonly string _entityName;
        private readonly string _messageName;

        public SdkMessageProcessingStepImage Image { get; private set; }

        public WindowSdkMessageProcessingStepImage(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SdkMessageProcessingStepImage image
            , string entityName
            , string messageName
        ) : base(iWriteToOutput, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this._entityName = entityName;
            this._messageName = messageName;
            this.Image = image;

            tSSLblConnectionName.Content = _service.ConnectionData.Name;

            LoadEntityImageProperties();

            this.DecreaseInit();

            txtBName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadEntityImageProperties()
        {
            FillMessagePropertyNames();

            txtBName.Text = Image.Name;
            txtBEntityAlias.Text = Image.EntityAlias;
            cmBMessagePropertyName.Text = Image.MessagePropertyName;
            txtBRelatedAttributeName.Text = Image.RelatedAttributeName;
            txtBDescription.Text = Image.Description;

            if (string.IsNullOrEmpty(cmBMessagePropertyName.Text) && cmBMessagePropertyName.Items.Count > 0)
            {
                cmBMessagePropertyName.Text = cmBMessagePropertyName.Items[0].ToString();
            }

            txtBAttributes.Text = Image.Attributes1StringsSorted;
            txtBAttributes.ToolTip = GetImageTooltip(Image.Attributes1);

            if (Image.ImageType?.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.Both_2)
            {
                rBBoth.IsChecked = true;
            }
            else if (Image.ImageType?.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PostImage_1)
            {
                rBPostImage.IsChecked = true;
            }
            else
            {
                rBPreImage.IsChecked = true;
            }
        }

        private static string GetImageTooltip(string attributes)
        {
            if (string.IsNullOrEmpty(attributes))
            {
                return null;
            }

            return string.Join(Environment.NewLine, attributes.Split(',').OrderBy(s => s));
        }

        private void FillMessagePropertyNames()
        {
            string text = cmBMessagePropertyName.Text;

            cmBMessagePropertyName.Items.Clear();

            var properties = GetMessagePropertyNames();

            if (properties.Any())
            {
                foreach (var item in properties.OrderBy(s => s))
                {
                    cmBMessagePropertyName.Items.Add(item);
                }
            }

            cmBMessagePropertyName.Text = text;
        }

        private string[] GetMessagePropertyNames()
        {
            if (string.Equals(_messageName, "Create", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    "Id",
                };
            }
            else if (string.Equals(_messageName, "Update", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "Delete", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "Assign", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "ExecuteWorkflow", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "Route", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return new string[]
                {
                    "Target",
                };
            }
            else if (string.Equals(_messageName, "SetState", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "SetStateDynamicEntity", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return new string[]
                {
                    "EntityMoniker",
                };
            }
            else if (string.Equals(_messageName, "DeliverIncoming", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "DeliverPromote", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(_messageName, "Send", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return new string[]
                {
                    "EmailId",
                };
            }
            else if (string.Equals(_messageName, "Merge", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    "Target",
                    "SubordinateId",
                };
            }

            return new string[0];
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await PerformSaveClick();
        }

        public int GetImageType()
        {
            if (rBBoth.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.Both_2;
            }
            else if (rBPostImage.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PostImage_1;
            }
            else if (rBPreImage.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PreImage_0;
            }

            return (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PreImage_0;
        }

        private async Task PerformSaveClick()
        {
            var imageType = GetImageType();

            this.Image.Name = txtBName.Text.Trim();
            this.Image.EntityAlias = txtBEntityAlias.Text.Trim();
            this.Image.MessagePropertyName = cmBMessagePropertyName.Text?.Trim() ?? string.Empty;
            this.Image.RelatedAttributeName = txtBRelatedAttributeName.Text.Trim();
            this.Image.Description = txtBDescription.Text.Trim();

            this.Image.Attributes1 = txtBAttributes.Text.Trim();

            this.Image.ImageType = new OptionSetValue(imageType);

            ToggleControls(false, Properties.OutputStrings.InConnectionUpdatingSdkMessageProcessingStepImageFormat1, _service.ConnectionData.Name);

            try
            {
                this.Image.Id = await _service.UpsertAsync(this.Image);

                ToggleControls(true, Properties.OutputStrings.InConnectionUpdatingSdkMessageProcessingStepImageCompletedFormat1, _service.ConnectionData.Name);

                this.DialogResult = true;

                this.Close();
            }
            catch (Exception ex)
            {
                ToggleControls(true, Properties.OutputStrings.InConnectionUpdatingSdkMessageProcessingStepImageFailedFormat1, _service.ConnectionData.Name);

                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }
        }

        protected override void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar

                , btnSave
                , btnCancel
                , btnSetAllAttributes
                , btnSelectAttributes

                , txtBName
                , txtBEntityAlias
                , txtBRelatedAttributeName
                , txtBDescription

                , cmBMessagePropertyName

                , rBBoth
                , rBPostImage
                , rBPreImage
            );
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

        private async void btnSelectAttributes_Click(object sender, RoutedEventArgs e)
        {
            if (!_entityName.IsValidEntityName())
            {
                return;
            }

            if (_entityMetadata == null)
            {
                ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, this._entityName);

                var repository = new EntityMetadataRepository(_service);

                _entityMetadata = await repository.GetEntityMetadataAsync(_entityName);

                ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, this._entityName);
            }

            if (_entityMetadata == null)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.UpdatingImageAttributesFormat1, this._entityName);

            var form = new WindowAttributeMultiSelect(_iWriteToOutput
                , _service
                , _entityMetadata.MetadataId.Value
                , _entityMetadata.Attributes
                , txtBAttributes.Text.Trim()
            );

            if (form.ShowDialog().GetValueOrDefault())
            {
                txtBAttributes.Text = form.GetAttributes();
                txtBAttributes.ToolTip = GetImageTooltip(txtBAttributes.Text);
            }

            ToggleControls(true, Properties.OutputStrings.UpdatingImageAttributesCompletedFormat1, this._entityName);
        }

        private void btnSetAllAttributes_Click(object sender, RoutedEventArgs e)
        {
            txtBAttributes.Text = string.Empty;
            txtBAttributes.ToolTip = null;
        }
    }
}