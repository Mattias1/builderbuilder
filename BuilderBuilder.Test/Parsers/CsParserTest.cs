using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BuilderBuilder.Test
{
    [TestClass]
    public class CsParserTest : CsParser
    {
        public override BuilderEntity Parse(string[] lines) {
            throw new NotImplementedException();
        }

        // Is attribute line
        [TestMethod]
        public void IsAttributeLineTest_Simple() {
            string line = "[attribute]";
            bool result = IsAttributeLine(line);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeLineTest_Advanced() {
            string line = "  [ question(answer = 42), test ]  ";
            bool result = IsAttributeLine(line);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeLineTest_MissingStart_False() {
            string line = "  test]  ";
            bool result = IsAttributeLine(line);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttributeLineTest_MissingEnd_False() {
            string line = "  [test  ";
            bool result = IsAttributeLine(line);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttributeLineTest_Comment() {
            string line = "  [ question(answer = 42), test ] // Comment";
            bool result = IsAttributeLine(line);
            Assert.IsTrue(result);
        }

        // Is attribute
        [TestMethod]
        public void IsAttributeTest_Simple() {
            string line = "  [attribute]  ";
            string pattern = "attribute";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeTest_First() {
            string line = "  [ question(answer = 42) , test ]  ";
            string pattern = "question";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeTest_Second() {
            string line = "  [ question(answer = 42) , test ]  ";
            string pattern = "test";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeTest_None() {
            string line = "  [ question(answer = 42), test ]  ";
            string pattern = "none";
            bool result = IsAttribute(line, pattern);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttributeTest_Comment() {
            string line = "  [ question(answer = 42), test ] // Comment";
            string pattern = "question";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        // Line has attribute
        [TestMethod]
        public void LineHasAttributeTest_True() {
            string[] lines = new string[] {
                "",
                "    [test]",
                "    [additional(info = 37)]",
                "    public SoundEffect Boom(int radius) {"
            };
            bool result = LineHasAttribute(lines, 3, "test");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LineHasAttributeTest_False() {
            string[] lines = new string[] {
                "",
                "    [test]",
                "    [additional(info = 37)]",
                "    public SoundEffect Boom(int radius) {"
            };
            bool result = LineHasAttribute(lines, 3, "none");
            Assert.IsFalse(result);
        }
    }
}
