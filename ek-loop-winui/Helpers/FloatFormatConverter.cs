using Microsoft.UI.Xaml.Data;

namespace ek_loop_winui.Helpers;
public class FloatFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {

        if (value is float floatValue)
        {

            return floatValue.ToString("0.00");
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {

        throw new NotImplementedException();
    }
}
