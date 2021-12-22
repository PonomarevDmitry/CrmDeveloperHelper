namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public abstract class BaseSinglePrivilegeViewItem : BasePrivilegeViewItem
    {
        public bool IsChanged { get; protected set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (!string.Equals(propertyName, nameof(IsChanged)))
            {
                var val = CalculateIsChanged();

                if (val != this.IsChanged)
                {
                    this.OnPropertyChanging(nameof(IsChanged));
                    this.IsChanged = val;
                    this.OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        protected abstract bool CalculateIsChanged();
    }
}
