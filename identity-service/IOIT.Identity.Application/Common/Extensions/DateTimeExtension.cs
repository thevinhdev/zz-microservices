using System;
using System.Globalization;

namespace IOIT.Identity.Application.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime StringToDateTime(
            string dateTime,
            string format,
            DateTime dtDefault)
        {
            return !DateTime.TryParseExact(dateTime, format, (IFormatProvider)CultureInfo.CurrentCulture, DateTimeStyles.None, out var result) ? dtDefault : result;
        }
    }
}
