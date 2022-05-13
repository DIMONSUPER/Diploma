using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Diploma.Converters
{
    public class FirstLettersConverter : IValueConverter
    {
        #region -- IValueConverter implementation --

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = null;

            if (value is string str)
            {
                if (str.Split(' ').Count() > 0)
                {
                    result = string.Concat(str.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x[0]));
                }
            }

            return result;
        }

        //TODO: implement
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
