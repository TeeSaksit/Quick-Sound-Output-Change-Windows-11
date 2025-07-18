using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Sound_Output_Change_Windows_11.Helpers
{
    public class ActiveColorConverter : IValueConverter
    {
        public static SolidColorBrush ActiveBrush { get; set; } = ColorConverter.SolidColorBrushFromHex("#3C3C3C");
        public static SolidColorBrush InactiveBrush { get; set; } = new SolidColorBrush(Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isActive = (bool)value;
            return isActive ? ActiveBrush : InactiveBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
