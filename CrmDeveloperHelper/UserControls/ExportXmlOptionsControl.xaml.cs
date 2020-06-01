using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ExportXmlOptionsControl.xaml
    /// </summary>
    public partial class ExportXmlOptionsControl : UserControl
    {
        private readonly CommonConfiguration _commonConfig;

        public ExportXmlOptionsControl(CommonConfiguration commonConfig, XmlOptionsControls controls)
        {
            InitializeComponent();

            this._commonConfig = commonConfig;

            LoadFromConfig();

            grBRibbon.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.RibbonFilters);

            chBSortRibbonCommandsAndRulesById.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SortRibbonCommandsAndRulesById);
            chBSortFormXmlElements.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SortFormXmlElements);
            chBSortXmlAttributes.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SortXmlAttributes);

            chBXmlAttributeOnNewLine.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.XmlAttributeOnNewLine);
            chBSetXmlSchemas.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SetXmlSchemas);
            chBSetIntellisenseContext.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SetIntellisenseContext);

            chBSolutionComponentWithManagedInfo.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SolutionComponentWithManagedInfo);
            chBSolutionComponentWithSolutionInfo.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SolutionComponentWithSolutionInfo);
            chBSolutionComponentWithUrl.Visibility = GetVisibilityForAttribute(controls, XmlOptionsControls.SolutionComponentWithUrl);
        }

        private static System.Windows.Visibility GetVisibilityForAttribute(XmlOptionsControls controls, XmlOptionsControls attribute)
        {
            return (controls & attribute) != 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void LoadFromConfig()
        {
            System.Windows.FrameworkElement[] dataContextElements =
            {
                chBForm
                , chBHomepageGrid
                , chBSubGrid

                , chBSortRibbonCommandsAndRulesById
                , chBSortFormXmlElements
                , chBSortXmlAttributes

                , chBXmlAttributeOnNewLine
                , chBSetXmlSchemas
                , chBSetIntellisenseContext

                , chBSolutionComponentWithManagedInfo
                , chBSolutionComponentWithSolutionInfo
                , chBSolutionComponentWithUrl
            };

            foreach (var element in dataContextElements)
            {
                element.DataContext = _commonConfig;
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
    }
}