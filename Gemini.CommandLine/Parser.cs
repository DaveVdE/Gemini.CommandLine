using System;

namespace Gemini.CommandLine
{
    public class Parser
    {
        public string SwitchIndicator { get; set; }
        public string ArgumentSeparator { get; set; }
        public string MemberIndicator { get; set; }
        
        public Parser()
        {
            SwitchIndicator = "/";
            ArgumentSeparator = ":";
            MemberIndicator = ".";
        }

        public Command Parse(params string[] arguments)
        {
            var command = new Command();

            foreach (var argument in arguments)
            {
                if (argument.StartsWith(SwitchIndicator))
                {
                    var argumentIndex = argument.IndexOf(ArgumentSeparator, StringComparison.Ordinal);

                    if (argumentIndex != -1)
                    {
                        command.Options[argument.Substring(SwitchIndicator.Length, argumentIndex - SwitchIndicator.Length)] =
                            argument.Substring(argumentIndex + ArgumentSeparator.Length);
                    }
                    else
                    {
                        command.Options[argument.Substring(SwitchIndicator.Length)] = null;
                    }
                }
                else
                {
                    if (command.Name == null)
                    {
                        var memberIndex = argument.IndexOf(MemberIndicator, StringComparison.Ordinal);
                        if (memberIndex != -1)
                        {
                            command.TypeName = argument.Substring(0, memberIndex);
                            command.Name = argument.Substring(memberIndex + MemberIndicator.Length);
                        }
                        else
                        {
                            command.Name = argument;
                        }
                    }
                    else
                    {
                        command.Arguments.Add(argument);
                    }
                }
            }

            return command;
        }
    }
}
