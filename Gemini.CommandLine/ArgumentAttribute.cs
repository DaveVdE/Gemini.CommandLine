using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gemini.CommandLine
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsOptional { get; set; }

        public ArgumentAttribute(string name, bool isOptional = false)
        {
            Name = name;
            IsOptional = isOptional;
        }

        public static IEnumerable<ArgumentAttribute> For(ICustomAttributeProvider provider)
        {
            return provider.GetCustomAttributes(true).OfType<ArgumentAttribute>();
        }
    }
}