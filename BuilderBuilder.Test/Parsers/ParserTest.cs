using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BuilderBuilder.Test
{
    [TestClass]
    public class ParserTest : Parser
    {
        public override BuilderEntity Parse(string[] lines) {
            throw new NotImplementedException();
        }

        // Start and end
        [TestMethod]
        public void StartsWithTest() {
            string line = "    testhahaha";
            string pattern = "test";
            bool result = StartsWith(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EndsWithTest() {
            string line = "hahahatest    ";
            string pattern = "test";
            bool result = EndsWith(line, pattern);
            Assert.IsTrue(result);
        }

        // Get pattern match
        [TestMethod]
        public void GetPatternMatchTest() {
            string line = "    public class Thingy";
            string result = GetPatternMatch(line, @"public class (\w+)");
            Assert.AreEqual("Thingy", result);
        }

        [TestMethod]
        public void GetPatternMatchesTest() {
            string line = "    public virtual string Thingy";
            string[] result = GetPatternMatches(line, @"public virtual (\w+) (\w+)");

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("string", result[0]);
            Assert.AreEqual("Thingy", result[1]);
        }
    }
}
