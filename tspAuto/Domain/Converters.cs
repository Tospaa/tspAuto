using System;
using tspAuto.Model;

namespace tspAuto.Domain
{
    public class BooleanInverseConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var newVal = System.Convert.ToBoolean(value);
                return !newVal;
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to boolean.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var newVal = System.Convert.ToBoolean(value);
                return !newVal;
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to boolean.");
            }
        }
    }

    public class YeniDosyaEkle_DavaciAlacakliConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                bool newVal = System.Convert.ToBoolean(value);
                if (newVal)
                {
                    return "Alacaklının:";
                }
                else
                {
                    return "Davacının:";
                }
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to boolean.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This feature has not been implemented yet.");
        }
    }

    public class YeniDosyaEkle_DavaliBorcluConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                bool newVal = System.Convert.ToBoolean(value);
                if (newVal)
                {
                    return "Borçlunun:";
                }
                else
                {
                    return "Davalının:";
                }
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to boolean.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This feature has not been implemented yet.");
        }
    }

    public class MainWindow_KullaniciYetkiConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Yetkiler yetki = (Yetkiler)value;
                return yetki == Yetkiler.Yonetici || yetki == Yetkiler.Avukat;
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This feature has not been implemented yet.");
        }
    }
}
