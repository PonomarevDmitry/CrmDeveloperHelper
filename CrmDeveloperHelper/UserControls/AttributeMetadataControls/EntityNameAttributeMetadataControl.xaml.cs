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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    /// <summary>
    /// Interaction logic for MemoAttributeMetadataControl.xaml
    /// </summary>
    public partial class EntityNameAttributeMetadataControl : UserControl
    {
        public EntityNameAttributeMetadataControl()
        {
            InitializeComponent();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            //txtBValue.Focus();

            base.OnGotFocus(e);
        }
    }
}
