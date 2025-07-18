using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Quick_Sound_Output_Change_Windows_11.Helpers
{
    public static class ColorConverter
    {
        public static SolidColorBrush SolidColorBrushFromHex(string hex)
        {
            hex = hex.TrimStart('#');

            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }
    }
}
