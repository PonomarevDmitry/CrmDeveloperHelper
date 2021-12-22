namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public abstract class BaseLinkedPrivilegeViewItem : BasePrivilegeViewItem
    {
        public bool IsChanged1 { get; protected set; }

        public bool IsChanged2 { get; protected set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (!string.Equals(propertyName, nameof(IsChanged1))
                && !string.Equals(propertyName, nameof(IsChanged2))
            )
            {
                var val1 = CalculateIsChanged1();

                if (val1 != this.IsChanged1)
                {
                    this.OnPropertyChanging(nameof(IsChanged1));
                    this.IsChanged1 = val1;
                    this.OnPropertyChanged(nameof(IsChanged1));
                }

                var val2 = CalculateIsChanged2();

                if (val2 != this.IsChanged2)
                {
                    this.OnPropertyChanging(nameof(IsChanged2));
                    this.IsChanged2 = val2;
                    this.OnPropertyChanged(nameof(IsChanged2));
                }
            }
        }

        protected abstract bool CalculateIsChanged1();

        protected abstract bool CalculateIsChanged2();
    }
}
