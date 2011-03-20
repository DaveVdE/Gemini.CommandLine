using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemini.CommandLine.Tests
{
    [TestClass]
    public class CommandTests
    {
        // ReSharper disable UnusedMember.Local
        private class ExampleCommandType
        {
            public static bool ExampleCommandRun;
            public static bool ExampleCommandWithOptionsRun;
            public static bool StaticCommandRun;
            public static bool StaticCommandWithOptionsRun;

            public void ExampleCommand()
            {
                ExampleCommandRun = true;
            }

            public void ExampleCommand(string options)
            {
                ExampleCommandWithOptionsRun = true;
            }

            public static void StaticCommand()
            {
                StaticCommandRun = true;
            }

            public static void StaticCommand(string options)
            {
                StaticCommandWithOptionsRun = true;
            }
        }
        // ReSharper restore UnusedMember.Local


        [TestMethod]
        public void CommandCanFindNamedMethods()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand");
            var methods = command.FindSuitableMethods(types).ToArray();

            Assert.IsNotNull(methods);
            Assert.AreEqual(2, methods.Length);
            Assert.IsTrue(methods.All(method => method.Name == "ExampleCommand"));

            Expect.Throw<InvalidOperationException>(() => command.FindSuitableMethods(new Type[] {}));
        }

        [TestMethod]
        public void CommandCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.ExampleCommandRun);
        }

        [TestMethod]
        public void CommandWithOptionsCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand", "/options");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.ExampleCommandWithOptionsRun);
        }

        [TestMethod]
        public void StaticCommandCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.StaticCommand");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.StaticCommandRun);
        }

        [TestMethod]
        public void StaticCommandWithOptionsCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("ExampleCommandType.StaticCommand", "/options");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.StaticCommandWithOptionsRun);
        }
    }
}
