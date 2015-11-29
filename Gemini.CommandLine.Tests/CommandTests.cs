using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Gemini.CommandLine.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private class CommandTypeWithoutDefaultConstructor
        {
            // ReSharper disable once UnusedParameter.Local because it's needed for this test.
            public CommandTypeWithoutDefaultConstructor(string example)
            {
                
            }

            // ReSharper disable once UnusedMember.Local because it's needed for this test.
            public void Test()
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CommandCanFindNamedMethods()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand");
            var methods = command.FindSuitableMethods(types).ToArray();

            Assert.IsNotNull(methods);
            Assert.AreEqual(2, methods.Length);
            Assert.IsTrue(methods.All(method => method.Name == "ExampleCommand"));
            Assert.Throws<InvalidOperationException>(() => command.FindSuitableMethods(new Type[] {}));
        }

        [Test]
        public void CommandCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.ExampleCommandRan);
        }

        [Test]
        public void CommandWithOptionsCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand", "/options");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.ExampleCommandWithOptionsRan);
        }

        [Test]
        public void StaticCommandCanRun()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.StaticCommand");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.StaticCommandRan);
        }

        [Test]
        public void StaticCommandWithOptionsCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("ExampleCommandType.StaticCommand", "/options");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.StaticCommandWithOptionsRan);
        }

        [Test]
        public void CommandWithNameCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("NamedCommand", "/I:12");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.CommandWithNameRan);
        }

        [Test]
        public void TestThePropertyCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TestTheProperty", "/TP:12");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.TestThePropertyRan);
        }

        [Test]
        public void TestTheConstructorCanRun()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TestTheConstructor", "/tp");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.TestTheConstructorRan);
        }

        [Test]
        public void TestTheConstructorCanRun2()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TestTheConstructor2", "/pi:3.14");

            Assert.IsTrue(command.Run(types));
            Assert.IsTrue(ExampleCommandType.TestTheConstructor2Ran);
        }

        [Test]
        public void TheImpossibleCommandCantWork()
        {
            var types = new[] { typeof(ExampleCommandType) };
            var command = Command.FromArguments("TheImpossibleCommand", "/impossible:yes");

            Assert.Throws<InvalidOperationException>(() => command.Run(types));
        }

        [Test]
        public void TheImpossibleCommandTypeCantWork()
        {
            var types = new[] {typeof (CommandTypeWithoutDefaultConstructor)};
            var command = Command.FromArguments("Test");

            Assert.Throws<InvalidOperationException>(() => command.Run(types));
        }

        [Test]
        public void CommandRunsForAnyPublicType()
        {
            Command.FromArguments("ForAnyPublicType").Run();

            Assert.IsTrue(ExampleCommandType.ForAnyPublicTypeRan);
        }

        [Test]
        public void NotSpecifyingAnyArgumentsShowsListOfMethods()
        {
            var messages = new List<string>();
            var command = Command.FromArguments();
            command.HelpWriter = messages.Add;
            command.Run(typeof (ExampleCommandType));
        }
    }
}
