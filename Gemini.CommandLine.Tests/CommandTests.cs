using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemini.CommandLine.Tests
{
    [TestClass]
    public class CommandTests
    {
        // ReSharper disable UnusedMember.Local
        // ReSharper disable MemberCanBePrivate.Local

        private class CommandTypeWithoutDefaultConstructor
        {
            public CommandTypeWithoutDefaultConstructor(string example)
            {
                
            }

            public void Test()
            {
                Assert.Fail();
            }
        }

        // ReSharper restore MemberCanBePrivate.Local
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
            Assert.IsTrue(ExampleCommandType.ExampleCommandRan);
        }

        [TestMethod]
        public void CommandWithOptionsCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand", "/options");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.ExampleCommandWithOptionsRan);
        }

        [TestMethod]
        public void StaticCommandCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.StaticCommand");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.StaticCommandRan);
        }

        [TestMethod]
        public void StaticCommandWithOptionsCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("ExampleCommandType.StaticCommand", "/options");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.StaticCommandWithOptionsRan);
        }

        [TestMethod]
        public void CommandWithNameCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("NamedCommand", "/I:12");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.CommandWithNameRan);
        }

        [TestMethod]
        public void TestThePropertyCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TestTheProperty", "/TP:12");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.TestThePropertyRan);
        }

        [TestMethod]
        public void TestTheConstructorCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TestTheConstructor", "/tp");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.TestTheConstructorRan);
        }

        [TestMethod]
        public void TestTheConstructorCanRun2()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TestTheConstructor2", "/pi:3.14");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.TestTheConstructor2Ran);
        }

        [TestMethod]
        public void TheImpossibleCommandCantWork()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TheImpossibleCommand", "/impossible:yes");

            Expect.Throw<InvalidOperationException>(() => command.Run(types));
        }

        [TestMethod]
        public void TheImpossibleCommandTypeCantWork()
        {
            var types = new[] {typeof (CommandTypeWithoutDefaultConstructor)};
            var command = Command.FromArguments("Test");

            Expect.Throw<InvalidOperationException>(() => command.Run(types));
        }

        [TestMethod]
        public void CommandRunsForAnyPublicType()
        {
            Command.FromArguments("ForAnyPublicType").Run();

            Assert.IsTrue(ExampleCommandType.ForAnyPublicTypeRan);
        }

        [TestMethod]
        public void NotSpecifyingAnyArgumentsShowsListOfMethods()
        {
            var messages = new List<string>();
            var command = Command.FromArguments();
            command.HelpWriter = messages.Add;
            command.Run(typeof (ExampleCommandType));

            Assert.IsTrue(messages.Count > 0);
        }
    }
}
