using Loader.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Loader.Helpers.Converters
{
	public class EnumTranslationConverter : IValueConverter
	{
		public EnumTranslationConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Type type = value.GetType();
			if (!type.IsEnum)
			{
				return value;
			}
			return Loc.GetValue(string.Format("{0}.{1}", type.Name, value));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}