using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gemini.CommandLine
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandNameAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandNameAttribute(string name)
        {
            Name = name;
        }

        public static IEnumerable<CommandNameAttribute> ForMethod(MethodInfo method)
        {
            return method.GetCustomAttributes(true).OfType<CommandNameAttribute>();
        }
    }
}