using ProgramWhichSolveSudoku.ViewModels;
using ProgramWhichSolveSudoku.Views;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ProgramWhichSolveSudoku.Converters
{
    public class MultiValueConverterExtension : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ChosingNumberViewModel.Window = values[0] as Window;
            return values[1].ToString();
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { value, value };
        }
    }
}