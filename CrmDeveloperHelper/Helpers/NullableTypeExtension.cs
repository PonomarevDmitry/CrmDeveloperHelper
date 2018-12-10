using System;
using System.Windows.Markup;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class NullableTypeExtension : TypeExtension
    {
        public NullableTypeExtension()
        {
        }

        public NullableTypeExtension(string type)
          : base(type)
        {
        }

        public NullableTypeExtension(Type type)
          : base(type)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type basis = (Type)base.ProvideValue(serviceProvider);
            return typeof(Nullable<>).MakeGenericType(basis);
        }
    }
}