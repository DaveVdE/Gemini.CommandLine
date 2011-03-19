using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gemini.CommandLine
{
    internal static class MethodFilters
    {
        public static IEnumerable<MethodInfo> WithName(this IEnumerable<MethodInfo> qry, string name)
        {
            return qry.Where(method => 
                method.Name == name ||
                CommandNameAttribute.ForMethod(method).Where(attribute => attribute.Name == name).Any());
        }
    }
}
