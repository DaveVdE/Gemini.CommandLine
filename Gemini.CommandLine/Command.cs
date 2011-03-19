using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gemini.CommandLine
{
    public class Command
    {
        public Dictionary<string, string> Options { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public List<string> Arguments { get; set; }

        public Command()
        {
            Options = new Dictionary<string, string>();
            Arguments = new List<string>();
        }

        public static Command FromArguments(params string[] arguments)
        {
            return (new Parser()).Parse(arguments);
        }

        public IEnumerable<MethodInfo> FindSuitableMethods(Type[] types)
        {
            if (!string.IsNullOrEmpty(TypeName))
            {
                types = types.Where(type => type.Name == TypeName).ToArray();

                if (!types.Any())
                {
                    var message = string.Format("No type named '{0}' could be found.", TypeName);
                    throw new InvalidOperationException(message);
                }
            }

            const BindingFlags bindingFlags = 
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly;

            return types.SelectMany(type => type.GetMethods(bindingFlags).WithName(Name));
        }
    }
}