using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class FileGenerationEntityMetadataCSharpOptionsControl : UserControl
    {
        private FileGenerationOptions _fileGenerationOptions;

        public FileGenerationEntityMetadataCSharpOptionsControl()
        {
            InitializeComponent();
        }

        public void BindFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            this._fileGenerationOptions = fileGenerationOptions;

            System.Windows.FrameworkElement[] dataContextElements =
            {
                chBWithManagedInfo
                , chBAllDescriptions
                , chBWithDependentComponents

                , rBTab
                , rBSpaces
                , txtBSpaceCount

                , rBClasses
                , rBEnums

                , rBReadOnly
                , rBConst

                , chBProxyClassesAttributes
                , chBProxyClassesManyToOne
                , chBProxyClassesManyToMany
                , chBProxyClassesOneToMany

                , chBProxyClassesLocalOptionSet
                , chBProxyClassesGlobalOptionSet
                , chBProxyClassesStatusOptionSet

                , chBProxyClassesWithDebuggerNonUserCode
                , chBProxyClassesAttributesWithNameOf
                , chBProxyClassesUseSchemaConstInCSharpAttributes
                , chBProxyClassesWithoutObsoleteAttribute
                , chBProxyClassesMakeAllPropertiesEditable
                , chBProxyClassesAddConstructorWithAnonymousTypeObject
                , chBProxyClassesOverrideToStringMethod
                , chBProxyClassesAddDescriptionAttribute

                , cmBProxyClassesAttributesEnumsStateStatus
                , cmBProxyClassesAttributesEnumsLocal
                , cmBProxyClassesAttributesEnumsGlobal

                , chBProxyClassesAttributesEnumsUseSchemaStateStatusEnum
                , chBProxyClassesAttributesEnumsUseSchemaLocalEnum
                , cmBProxyClassesAttributesEnumsUseSchemaGlobalEnum

                , chBSchemaAddDescriptionAttribute

                , chBSchemaAddTypeConverterAttributeForEnums

                , chBSchemaAttributes
                , chBSchemaManyToOne
                , chBSchemaManyToMany
                , chBSchemaOneToMany
                , chBSchemaLocalOptionSet
                , chBSchemaGlobalOptionSet
                , chBSchemaStatusOptionSet
                , chBSchemaKeys

                , chBIntoSchemaClass

                , txtBNamespaceClassesCSharp
                , txtBNamespaceGlobalOptionSetsCSharp
                , txtBTypeConverterName
            };

            foreach (var element in dataContextElements)
            {
                element.DataContext = fileGenerationOptions;
            }
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