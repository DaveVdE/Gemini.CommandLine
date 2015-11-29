using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Gemini.CommandLine
{
    public class MyTimeSpanConverter : TypeConverter
    {
        private const string FindGroups = @"([\d.,]+)([smhdwSMHDW])";

        private static TimeSpan InternalConvert(CultureInfo culture, string input)
        {
            var findGroupsExpression = new Regex(FindGroups);
            var matches = findGroupsExpression.Matches(input);
            var result = TimeSpan.Zero;

            foreach (Match match in matches)
            {
                double number = Convert.ToDouble(match.Groups[1].Value, culture);
                switch (match.Groups[2].Value)
                {
                    case "s":
                    case "S":
                        result += TimeSpan.FromSeconds(number);
                        break;
                    case "m":
                    case "M":
                        result += TimeSpan.FromMinutes(number);
                        break;
                    case "h":
                    case "H":
                        result += TimeSpan.FromHours(number);
                        break;
                    case "d":
                    case "D":
                        result += TimeSpan.FromDays(number);
                        break;
                    case "w":
                    case "W":
                        result += TimeSpan.FromDays(number*7);
                        break;
                }
            }

            return result;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return InternalConvert(culture, Convert.ToString(value));
        }
    }
}
