using System.Globalization;
using _IValueConverter = Avalonia.Data.Converters.IValueConverter;
using BaseType = System.Application.Converters.Abstractions.IValueConverter;

namespace System.Application.Converters;

/// <inheritdoc cref="BaseType"/>
public interface IValueConverter : BaseType, _IValueConverter, IBinding
{
    object? _IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        BaseType thiz = this;
        return thiz.ConvertBack(value, targetType, parameter, culture);
    }
}
