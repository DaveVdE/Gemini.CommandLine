using System;
using NUnit.Framework;

namespace Gemini.CommandLine.Tests
{
    [TestFixture]
    public class TimeSpanConverterTests
    {
        [Test]
        public void TestMethod1()
        {
            const string input = "7d 12h";
            var output = new MyTimeSpanConverter().ConvertFrom(input);
            var compare = TimeSpan.FromDays(7) + TimeSpan.FromHours(12);

            Assert.AreEqual(output, compare);
        }
    }
}
