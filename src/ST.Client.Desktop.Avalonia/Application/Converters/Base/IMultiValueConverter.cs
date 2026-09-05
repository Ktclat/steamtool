using _IMultiValueConverter = Avalonia.Data.Converters.IMultiValueConverter;
using BaseType = System.Application.Converters.Abstractions.IMultiValueConverter;

namespace System.Application.Converters;

/// <inheritdoc cref="BaseType"/>
public partial interface IMultiValueConverter : BaseType, _IMultiValueConverter, IBinding
{
}
