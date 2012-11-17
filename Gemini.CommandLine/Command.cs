using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Action<string> HelpWriter { get; set; }

        public Command()
        {
            Options = new Dictionary<string, string>();
            Arguments = new List<string>();
            HelpWriter = Console.WriteLine;
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

            return types.SelectMany(type => type.GetMethods(bindingFlags)
                .WithName(Name)
                .OrderByDescending(method => method.GetParameters().Length));
        }

        /// <summary>
        /// Find all the constructors for the specified <see cref="Type" /> that have all their parameters
        /// represented as an option.
        /// </summary>
        private IEnumerable<ConstructorInfo> FindSuitableConstructors(Type type)
        {
            return type.GetConstructors().Where(constructor =>
                constructor.GetParameters().All(parameter =>
                {
                    if (parameter.ParameterType == typeof(Command))
                    {
                        return true;
                    }

                    var attributes = ArgumentAttribute.For(parameter);

                    return attributes.Any() 
                        ? attributes.Any(attribute => Options.ContainsKey(attribute.Name)) 
                        : Options.ContainsKey(parameter.Name);
                }));
        }

        private bool BindProvider(Type type, string providerName, ICustomAttributeProvider provider, out object value)
        {
            if (type == typeof(Command))
            {
                value = this;
                return true;
            }

            var converter = TypeDescriptor.GetConverter(type);

            if (converter == null || !converter.CanConvertFrom(typeof(string)))
            {
                var message = string.Format("No conversion possible for '{0}'.", providerName);
                throw new InvalidOperationException(message);
            }

            var attributes = ArgumentAttribute.For(provider).Select(attribute => Tuple.Create(attribute, attribute.Name));

            if (!attributes.Any())
            {
                attributes = new[] {new Tuple<ArgumentAttribute, string>(null, providerName)};
            }

            foreach (var pair in attributes)
            {
                var attribute = pair.Item1;
                var name = pair.Item2;
                string text;

                if (!Options.TryGetValue(name, out text))
                {
                    if (attribute != null && attribute.IsOptional)
                    {
                        value = null;
                        return true;
                    }

                    continue;
                }

                if (type == typeof(bool))
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        value = true;
                        return true;
                    }

                    value = Convert.ToBoolean(text);
                    return true;
                }

                value = converter.ConvertFromInvariantString(text);
                return true;
            }

            value = null;
            return false;
        }

        private bool BindParameter(ParameterInfo parameter, out object output)
        {
            return BindProvider(parameter.ParameterType, parameter.Name, parameter, out output);
        }

        private bool BindParameters(IEnumerable<ParameterInfo> parameters, out object[] output)
        {
            var result = parameters.Select(parameter =>
            {
                object value;
                return BindParameter(parameter, out value)
                           ? new Tuple<bool, object>(true, value)
                           : new Tuple<bool, object>(false, null);
            }).ToArray();

            if (result.All(tuple => tuple.Item1))
            {
                output = result.Select(tuple => tuple.Item2).ToArray();
                return true;
            }

            output = null;
            return false;
        }


        private void BindProperty(PropertyInfo property, object instance)
        {
            foreach (var attribute in ArgumentAttribute.For(property))
            {
                string text;

                if (!Options.TryGetValue(attribute.Name, out text))
                {
                    continue;
                }

                object value;

                if (BindProvider(property.PropertyType, property.Name, property, out value))
                {
                    property.SetValue(instance, value, null);                        
                }
            }
        }

        private void BindProperties(IEnumerable<PropertyInfo> properties, object instance)
        {
            foreach (var property in properties)
            {
                BindProperty(property, instance);
            }            
        }

        public bool Run(MethodInfo methodInfo)
        {
            var type = methodInfo.ReflectedType;
            object instance = null;

            if (!methodInfo.IsStatic)
            {
                // Find all suitable constructors, then order by complexity.
                var constructors = FindSuitableConstructors(type)
                    .OrderByDescending(c => c.GetParameters().Count())
                    .ToArray();

                foreach (var constructor in constructors)
                {
                    object[] constructorParameters;

                    if (BindParameters(constructor.GetParameters(), out constructorParameters))
                    {
                        instance = constructor.Invoke(constructorParameters);
                        BindProperties(type.GetProperties(), instance);

                        break;
                    }
                }

                if (instance == null)
                {
                    var message = string.Format("No suitable constructor found for type '{0}'.", type);
                    throw new InvalidOperationException(message);
                }
            }

            object[] parameters;
            
            if (BindParameters(methodInfo.GetParameters(), out parameters))
            {
                methodInfo.Invoke(instance, parameters);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Run the command on the most suitable method found on the specified types.
        /// </summary>
        /// <param name="types">
        /// The types to examine for suitable methods. 
        /// If this parameter is null, any public type on the calling assembly is considered.
        /// </param>
        /// <returns></returns>
        public bool Run(params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                types = Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsPublic).ToArray();
            }

            var methods = FindSuitableMethods(types);
            return methods.Any(Run);
        }
    }
}