using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ExportEntityAttributesDependentComponentsOptionsControl.xaml
    /// </summary>
    public partial class ExportEntityAttributesDependentComponentsOptionsControl : UserControl
    {
        private CommonConfiguration _commonConfig;

        public ExportEntityAttributesDependentComponentsOptionsControl(CommonConfiguration commonConfig)
        {
            InitializeComponent();

            this._commonConfig = commonConfig;

            LoadFromConfig();
        }

        private void LoadFromConfig()
        {
            rBAllcomponents.DataContext = _commonConfig;
            rBWorkflowPlugin.DataContext = _commonConfig;
        }
    }
}
