using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectEntityAndRelationshipName : WindowBase
    {
        public string EntityName { get; private set; }

        public Guid EntityId { get; private set; }

        public string RelationshipName { get; private set; }

        public WindowSelectEntityAndRelationshipName(
            ConnectionData connectionData
            , string entityName
            , string windowTitle
            , string selectionText
        )
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this.Title = windowTitle;

            LoadEntityRelationshipsManyToMany(cmBRelationshipName, cmBEntityName, connectionData, entityName);

            cmBEntityName.Text = selectionText;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                MakeOkClick();
            }

            base.OnKeyDown(e);
        }

        private void MakeOkClick()
        {
            string relationshipName = string.Empty;

            string entityName = string.Empty;
            Guid? entityId = null;

            {
                string textRelation = cmBRelationshipName.Text?.Trim(' ', '<', '>');

                if (!string.IsNullOrEmpty(textRelation))
                {
                    relationshipName = textRelation;
                }
            }

            {
                string textEntityName = cmBEntityName.Text?.Trim(' ', '<', '>');

                if (!string.IsNullOrEmpty(textEntityName))
                {
                    entityName = textEntityName;
                }
            }

            {
                string textEntityId = txtBEntityId.Text?.Trim(' ', '<', '>');

                if (!string.IsNullOrEmpty(textEntityId) && Guid.TryParse(textEntityId, out Guid tempGuid))
                {
                    entityId = tempGuid;
                }
            }

            var message = new StringBuilder();

            if (string.IsNullOrEmpty(relationshipName))
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.RelationshipNameIsEmpty);
            }

            if (string.IsNullOrEmpty(entityName))
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.EntityNameIsEmpty);
            }

            if (!entityId.HasValue)
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.CannotParseGuid);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.EntityName = entityName;
            this.EntityId = entityId.Value;
            this.RelationshipName = relationshipName;

            this.DialogResult = true;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }
    }
}
