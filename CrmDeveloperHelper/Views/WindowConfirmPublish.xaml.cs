using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowConfirmPublish : WindowBase
    {
        public WindowConfirmPublish(string message, string title, bool hideNotPromt)
        {
            InitializeComponent();

            lblText.Content = message;
            this.Title = title;

            if (hideNotPromt)
            {
                chBDoNotPromtPublishMessage.IsEnabled = false;
                chBDoNotPromtPublishMessage.Visibility = Visibility.Collapsed;
            }

            btnConfirm.Focus();
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
