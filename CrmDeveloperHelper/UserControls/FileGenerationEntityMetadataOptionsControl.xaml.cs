using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class FileGenerationEntityMetadataOptionsControl : UserControl
    {
        private FileGenerationOptions _fileGenerationOptions;

        public FileGenerationEntityMetadataOptionsControl()
        {
            InitializeComponent();
        }

        public void BindFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            this._fileGenerationOptions = fileGenerationOptions;

            txtBSpaceCount.DataContext = fileGenerationOptions;

            rBTab.DataContext = fileGenerationOptions;
            rBSpaces.DataContext = fileGenerationOptions;

            rBClasses.DataContext = fileGenerationOptions;
            rBEnums.DataContext = fileGenerationOptions;

            rBReadOnly.DataContext = fileGenerationOptions;
            rBConst.DataContext = fileGenerationOptions;

            chBAttributesProxyClass.DataContext = fileGenerationOptions;
            chBManyToOneProxyClass.DataContext = fileGenerationOptions;
            chBManyToManyProxyClass.DataContext = fileGenerationOptions;
            chBOneToManyProxyClass.DataContext = fileGenerationOptions;
            chBLocalOptionSetsProxyClass.DataContext = fileGenerationOptions;
            chBGlobalOptionSetsProxyClass.DataContext = fileGenerationOptions;
            chBStatusProxyClass.DataContext = fileGenerationOptions;

            chBProxyClassWithDebuggerNonUserCode.DataContext = fileGenerationOptions;
            chBAttributesProxyClassWithNameOf.DataContext = fileGenerationOptions;
            chBProxyClassUseSchemaConstInCSharpAttributes.DataContext = fileGenerationOptions;
            chBProxyClassesWithoutObsoleteAttribute.DataContext = fileGenerationOptions;
            chBProxyClassesMakeAllPropertiesEditable.DataContext = fileGenerationOptions;
            chBProxyClassesAddConstructorWithAnonymousTypeObject.DataContext = fileGenerationOptions;

            cmBAttributesProxyClassEnumsStateStatus.DataContext = fileGenerationOptions;
            cmBAttributesProxyClassEnumsLocal.DataContext = fileGenerationOptions;
            cmBAttributesProxyClassEnumsGlobal.DataContext = fileGenerationOptions;

            chBAttributesProxyClassEnumsUseSchemaStateStatusEnum.DataContext = fileGenerationOptions;
            chBAttributesProxyClassEnumsUseSchemaLocalEnum.DataContext = fileGenerationOptions;
            cmBAttributesProxyClassEnumsUseSchemaGlobalEnum.DataContext = fileGenerationOptions;

            chBAddDescriptionAttribute.DataContext = fileGenerationOptions;
            chBProxyClassAddDescriptionAttribute.DataContext = fileGenerationOptions;

            chBAddTypeConverterAttributeForEnums.DataContext = fileGenerationOptions;

            chBAttributesSchema.DataContext = fileGenerationOptions;
            chBManyToOneSchema.DataContext = fileGenerationOptions;
            chBManyToManySchema.DataContext = fileGenerationOptions;
            chBOneToManySchema.DataContext = fileGenerationOptions;
            chBLocalOptionSetsSchema.DataContext = fileGenerationOptions;
            chBGlobalOptionSetsSchema.DataContext = fileGenerationOptions;
            chBStatusSchema.DataContext = fileGenerationOptions;
            chBKeysSchema.DataContext = fileGenerationOptions;

            chBIntoSchemaClass.DataContext = fileGenerationOptions;

            chBAllDescriptions.DataContext = fileGenerationOptions;

            chBWithDependentComponents.DataContext = fileGenerationOptions;

            chBWithManagedInfo.DataContext = fileGenerationOptions;



            txtBNamespaceClassesCSharp.DataContext = fileGenerationOptions;
            txtBNamespaceClassesJavaScript.DataContext = fileGenerationOptions;

            txtBNamespaceGlobalOptionSetsCSharp.DataContext = fileGenerationOptions;
            txtBNamespaceGlobalOptionSetsJavaScript.DataContext = fileGenerationOptions;

            txtBTypeConverterName.DataContext = fileGenerationOptions;
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

        private void btnLoadConfiguration_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this._fileGenerationOptions == null)
            {
                return;
            }

            string selectedPath = string.Empty;
            var t = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "FileGenerationOptions (.xml)|*.xml",
                        FilterIndex = 1,
                        RestoreDirectory = true
                    };

                    if (openFileDialog1.ShowDialog().GetValueOrDefault())
                    {
                        selectedPath = openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (string.IsNullOrEmpty(selectedPath))
            {
                return;
            }

            FileGenerationOptions fileOptionFromDisk = null;

            try
            {
                fileOptionFromDisk = FileGenerationOptions.LoadFromPath(selectedPath);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);

                fileOptionFromDisk = null;
            }

            if (fileOptionFromDisk == null)
            {
                return;
            }

            this._fileGenerationOptions.LoadFromDisk(fileOptionFromDisk);

            FileGenerationConfiguration.SaveConfiguration();
        }

        private void btnSaveConfiguration_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this._fileGenerationOptions == null)
            {
                return;
            }

            string fileName = "fileGenerationOptions.xml";

            var dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = ".xml",

                Filter = "FileGenerationOptions (.xml)|*.xml",
                FilterIndex = 1,

                RestoreDirectory = true,
                FileName = fileName,
            };

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    var filePath = dialog.FileName;

                    this._fileGenerationOptions.Save(filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            }
        }
    }
}