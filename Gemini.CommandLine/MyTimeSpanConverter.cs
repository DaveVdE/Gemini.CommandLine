using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gemini.CommandLine
{
    public class MyTimeSpanConverter : TypeConverter
    {
        private const string FindGroups = @"([\d.,]+)([smhdwSMHDW])";

        private static TimeSpan InternalConvert(CultureInfo culture, string input)
        {
            Regex findGroupsExpression = new Regex(FindGroups);
            MatchCollection matches = findGroupsExpression.Matches(input);
            TimeSpan result = TimeSpan.Zero;

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
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return InternalConvert(culture, Convert.ToString(value));
        }
    }
}
