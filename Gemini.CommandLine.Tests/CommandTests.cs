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
        private class ExampleCommandType
        {
            public void ExampleCommand()
            {                
            }
        }

        [TestMethod]
        public void CommandCanFindNamedMethods()
        {
            var types = new[] {typeof (ExampleCommandType)};
            var command = Command.FromArguments("ExampleCommandType.ExampleCommand");
            var methods = command.FindSuitableMethods(types).ToArray();

            Assert.IsNotNull(methods);
            Assert.AreEqual(1, methods.Length);
            Assert.AreEqual("ExampleCommand", methods[0].Name);
        }
    }
}
