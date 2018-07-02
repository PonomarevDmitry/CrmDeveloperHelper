using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowConfirmPublish : WindowBase
    {
        public WindowConfirmPublish(string message)
        {
            InitializeComponent();

            lblText.Content = message;
        }

        /// <summary>
        /// Не показывать предупреждение перед публикациями
        /// </summary>
        public bool DoNotPromtPublishMessage
        {
            get
            {
                return chBDoNotPromtPublishMessage.IsChecked.GetValueOrDefault();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.Close();
        }
    }
}
