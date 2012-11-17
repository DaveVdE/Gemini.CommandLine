using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemini.CommandLine.Tests
{
    public class ExampleCommandType
    {
        public static bool ExampleCommandRan;
        public static bool ExampleCommandWithOptionsRan;
        public static bool StaticCommandRan;
        public static bool StaticCommandWithOptionsRan;
        public static bool CommandWithNameRan;
        public static bool TestThePropertyRan;
        public static bool TestTheConstructorRan;
        public static bool TestTheConstructor2Ran;
        public static bool ForAnyPublicTypeRan;

        [Argument("TP")]
        public TimeSpan TestProperty { get; set; }
        public bool TestBoolean { get; set; }
        public float Pi { get; set; }
        public Command Command { get; set; }

        public ExampleCommandType()
        {                
        }

        public ExampleCommandType([Argument("tp")] bool tp)
        {
            TestBoolean = tp;
        }

        public ExampleCommandType(float pi, Command command)
        {
            Pi = pi;
            Command = command;
        }

        public void ExampleCommand()
        {
            ExampleCommandRan = true;
        }

        public void ExampleCommand(string options)
        {
            ExampleCommandWithOptionsRan = true;
        }

        public static void StaticCommand()
        {
            StaticCommandRan = true;
        }

        public static void StaticCommand(string options)
        {
            StaticCommandWithOptionsRan = true;
        }

        [CommandName("NamedCommand")]
        public void CommandWithName([Argument("I")] int index)
        {
            if (index == 12)
            {
                CommandWithNameRan = true;
            }
        }

        public void TestTheProperty()
        {
            TestThePropertyRan = this.TestProperty > TimeSpan.FromDays(1);                
        }

        public void TestTheConstructor()
        {
            TestTheConstructorRan = this.TestBoolean;
        }

        public void TestTheConstructor2()
        {
            TestTheConstructor2Ran = Command != null && Pi > 3;
        }

        public void TheImpossibleCommand([Argument("impossible")] object impossible)
        {
            Assert.Fail();
        }

        public void ForAnyPublicType()
        {
            ForAnyPublicTypeRan = true;
        }
    }
}