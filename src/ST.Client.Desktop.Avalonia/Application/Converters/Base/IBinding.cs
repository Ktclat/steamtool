using _Binding = Avalonia.Data.BindingOperations;
using _DependencyProperty = Avalonia.AvaloniaProperty;
using BaseType = System.Application.Converters.Abstractions.IBinding;

namespace System.Application.Converters;

/// <inheritdoc cref="BaseType"/>
public interface IBinding : BaseType
{
    object BaseType.DoNothing => _Binding.DoNothing;

    object BaseType.UnsetValue => _DependencyProperty.UnsetValue;
}
