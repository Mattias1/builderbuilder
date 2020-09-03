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
        public void IsAttributeLine_Simple() {
            string line = "[attribute]";
            bool result = IsAttributeLine(line);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeLine_Advanced() {
            string line = "  [ question(answer = 42), test ]  ";
            bool result = IsAttributeLine(line);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttributeLine_MissingStart_False() {
            string line = "  test]  ";
            bool result = IsAttributeLine(line);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttributeLine_MissingEnd_False() {
            string line = "  [test  ";
            bool result = IsAttributeLine(line);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttributeLine_Comment() {
            string line = "  [ question(answer = 42), test ] // Comment";
            bool result = IsAttributeLine(line);
            Assert.IsTrue(result);
        }

        // Is attribute
        [TestMethod]
        public void IsAttribute_Simple() {
            string line = "  [attribute]  ";
            string pattern = "attribute";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttribute_First() {
            string line = "  [ question(answer = 42) , test ]  ";
            string pattern = "question";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttribute_Second() {
            string line = "  [ question(answer = 42) , test ]  ";
            string pattern = "test";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAttribute_None() {
            string line = "  [ question(answer = 42), test ]  ";
            string pattern = "none";
            bool result = IsAttribute(line, pattern);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttribute_Comment() {
            string line = "  [ question(answer = 42), test ] // Comment";
            string pattern = "question";
            bool result = IsAttribute(line, pattern);
            Assert.IsTrue(result);
        }

        // Line has attribute
        [TestMethod]
        public void LineHasAttribute_True() {
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
        public void LineHasAttribute_False() {
            string[] lines = new string[] {
                "",
                "    [test]",
                "    [additional(info = 37)]",
                "    public SoundEffect Boom(int radius) {"
            };
            bool result = LineHasAttribute(lines, 3, "none");
            Assert.IsFalse(result);
        }

        // Is public field
        [TestMethod]
        public void ParsePublicField_IfVariable_ThenNotNull() {
            string line = "public override int MyVariable";
            var result = ParsePublicField(line);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParsePublicField_IfProperty_ThenNotNull() {
            string line = "public override int MyProperty { get; set; }";
            var result = ParsePublicField(line);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParsePublicField_IfFunction_ThenNull() {
            string line = "public int MyFunction()";
            var result = ParsePublicField(line);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParsePublicField_IfClass_ThenNull() {
            string line = "public class MyClass";
            var result = ParsePublicField(line);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParsePublicVirtualField_IfNotVirtual_ThenNull() {
            string line = "public int MyProperty { get; set; }";
            var result = ParsePublicVirtualField(line);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParsePublicVirtualField_IfVirtual_ThenNotNull() {
            string line = "public virtual int MyProperty { get; set; }";
            var result = ParsePublicVirtualField(line);
            Assert.IsNotNull(result);
        }

        // Is constructor
        [TestMethod]
        public void ParseConstructor_IfParameterlessConstructor_ThenEmptyString() {
            string line = "public MyClass()";
            var result = ParseConstructor(new[] { line }, 0, "MyClass");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ParseConstructor_IfConstructor_ThenParameters() {
            string line = "public MyClass(long? id, IEnumerable<Stuff> stuffs) {";
            var result = ParseConstructor(new[] { line }, 0, "MyClass");
            Assert.AreEqual("long? id, IEnumerable<Stuff> stuffs", result);
        }

        [TestMethod]
        public void ParseConstructor_IfConstructorWithBase_ThenParameters() {
            string line = "public MyClass(int one, int two) : base(one, two, 3) {";
            var result = ParseConstructor(new[] { line }, 0, "MyClass");
            Assert.AreEqual("int one, int two", result);
        }

        [TestMethod]
        public void ParseConstructor_IfConstructorWithParamsOnMultipleLines_ThenParameters() {
            string[] lines = new[] {
                "    public MyClass(",
                "        int one,",
                "        int two) ",
                "    {"
            };
            var result = ParseConstructor(lines, 0, "MyClass");
            AssertHelper.AreEqualModuloWhitespace("int one, int two", result);
        }

        [TestMethod]
        public void ParseConstructor_IfFunction_ThenNull() {
            string line = "public int MyFunction()";
            var result = ParseConstructor(new[] { line }, 0, "MyFunction");
            Assert.IsNull(result);
        }
    }
}
