using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowConfirmPublish : WindowBase
    {
        public WindowConfirmPublish(string message, bool showNotPromt = true)
        {
            InitializeComponent();

            lblText.Content = message;

            if (!showNotPromt)
            {
                chBDoNotPromtPublishMessage.IsEnabled = false;
                chBDoNotPromtPublishMessage.Visibility = Visibility.Collapsed;
            }
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
