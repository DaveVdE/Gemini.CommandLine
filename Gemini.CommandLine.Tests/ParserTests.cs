using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemini.CommandLine.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParserSupportsBasicCommands()
        {
            var parser = new Parser();

            var command1 = parser.Parse("Command", "/Option1", "/Option2:Value", "InputFile1", "InputFile2", "InputFile3");
            Assert.AreEqual("Command", command1.Name);
            Assert.AreEqual(2, command1.Options.Count);
            Assert.AreEqual(null, command1.Options["Option1"]);
            Assert.AreEqual("Value", command1.Options["Option2"]);
            Assert.AreEqual(3, command1.Arguments.Count);
            Assert.AreEqual("InputFile1", command1.Arguments[0]);
            Assert.AreEqual("InputFile2", command1.Arguments[1]);
            Assert.AreEqual("InputFile3", command1.Arguments[2]);
        }

        [TestMethod]
        public void ParserSupportsCommandWithTypename()
        {
            var parser = new Parser();

            var command1 = parser.Parse("TypeName.CommandName");
            Assert.AreEqual("TypeName", command1.TypeName);
            Assert.AreEqual("CommandName", command1.Name);
        }
    }
}
