using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Data;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class SolutionComponentConverter : IValueConverter
    {
        private readonly SolutionComponentDescriptor _descriptor;

        public SolutionComponentConverter(SolutionComponentDescriptor descriptor)
        {
            this._descriptor = descriptor;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            string propertyName = parameter as string;

            if (String.IsNullOrEmpty(propertyName))
                return new ArgumentNullException(nameof(parameter)).ToString();

            if (!(value is SolutionComponentViewItem viewItem))
                return Binding.DoNothing;

            if (viewItem.SolutionComponent == null || viewItem.SolutionComponent.ComponentType == null || !viewItem.SolutionComponent.ObjectId.HasValue)
            {
                return Binding.DoNothing;
            }

            var entity = _descriptor.GetEntity<Entity>(viewItem.SolutionComponent.ComponentType.Value, viewItem.SolutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return EntityDescriptionHandler.GetAttributeStringShortEntityReferenceAndPicklist(entity, propertyName);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}