using System;
using System.Globalization;
using Home_FA_Kit.Resources.Strings;
using Microsoft.Maui.Controls;

namespace Home_FA_Kit
{
    public class ExpirationDateRemainingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime expirationDate)
            {
                var remainingDays = (expirationDate - DateTime.Now).Days;

                if (remainingDays <= 0)
                {
                    return "Expired";
                }
                else if (remainingDays <= 7)
                {
                    return $"ExpiresInOneWeek";
                }
                else if (remainingDays <= 14)
                {
                    return $"ExpiresInTwoWeeks";
                }
                else
                {
                    if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                    {
                        return $"Days to the expiration remain: {remainingDays}";
                    } else
                    {
                        return $"До истечения осталось дней: {remainingDays}";
                    }
                }
            }
            return "Expired";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}