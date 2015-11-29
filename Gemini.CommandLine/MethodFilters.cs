using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gemini.CommandLine
{
    internal static class MethodFilters
    {
        public static IEnumerable<MethodInfo> WithName(this IEnumerable<MethodInfo> qry, string name)
        {
            return qry.Where(method => 
                method.Name == name ||
                CommandNameAttribute.ForMethod(method).Any(attribute => attribute.Name == name));
        }
    }
}
