using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemini.CommandLine.Tests
{
    [TestClass]
    public class TimeSpanConverterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string input = "7d 12h";
            var output = new MyTimeSpanConverter().ConvertFrom(input);
            var compare = TimeSpan.FromDays(7) + TimeSpan.FromHours(12);

            Assert.AreEqual(output, compare);
        }
    }
}
