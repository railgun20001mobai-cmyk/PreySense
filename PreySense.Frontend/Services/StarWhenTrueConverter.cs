using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PreySense.Frontend.Services
{
    /// <summary>
    /// Star-sizes a grid column when the flag is true and collapses it to zero width otherwise,
    /// so hiding a mode tile also gives its space back to the visible tiles.
    /// </summary>
    public sealed class StarWhenTrueConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is true ? new GridLength(1, GridUnitType.Star) : new GridLength(0);

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
