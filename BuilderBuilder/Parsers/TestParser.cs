using System;
using System.Linq;

namespace BuilderBuilder
{
    class TestParser : CsParser
    {
        public override BuilderEntity Parse(string[] lines) {
            throw new NotImplementedException();
        }

        public static void RunTests() {
            new TestParser().RunAllTests();
        }

        private void RunAllTests() {
            var testresults = new string[] {
                StartsWithTest(), EndsWithTest(),
                IsAttributeLineTest_Simple(), IsAttributeLineTest_Advanced(),
                    IsAttributeLineTest_MissingStart_False(), IsAttributeLineTest_MissingEnd_False(), IsAttributeLineTest_Comment(),
                IsAttributeTest_Simple(), IsAttributeTest_First(), IsAttributeTest_Second(), IsAttributeTest_None(), IsAttributeTest_Comment(),
                LineHasAttributeTest_False(), LineHasAttributeTest_True(),
                GetPatternMatchTest(), GetPatternMatchesTest()
            };

            var fails = testresults.Where(e => e != null);
            if (fails.Any()) {
                throw new Exception($"There were {fails.Count()} test fails: {string.Join(", ", fails)}");
            }
        }

        // Start and end
        private string StartsWithTest() {
            string line = "    testhahaha";
            string pattern = "test";
            return StartsWith(line, pattern) ? null : "StartsWith";
        }

        private string EndsWithTest() {
            string line = "hahahatest    ";
            string pattern = "test";
            return EndsWith(line, pattern) ? null : "EndsWith";
        }

        // Is attribute line
        private string IsAttributeLineTest_Simple() {
            string line = "[attribute]";
            return IsAttributeLine(line) ? null : "IsAttributeLine_Simple";
        }

        private string IsAttributeLineTest_Advanced() {
            string line = "  [ question(answer = 42), test ]  ";
            return IsAttributeLine(line) ? null : "IsAttributeLine_Advanced";
        }

        private string IsAttributeLineTest_MissingStart_False() {
            string line = "  test]  ";
            return IsAttributeLine(line) ? "IsAttributeLine_MissingStart_False" : null;
        }

        private string IsAttributeLineTest_MissingEnd_False() {
            string line = "  [test  ";
            return IsAttributeLine(line) ? "IsAttributeLine_MissingEnd_False" : null;
        }

        private string IsAttributeLineTest_Comment() {
            string line = "  [ question(answer = 42), test ] // Comment";
            return IsAttributeLine(line) ? null : "IsAttributeLine_Comment";
        }

        // Is attribute
        private string IsAttributeTest_Simple() {
            string line = "  [attribute]  ";
            string pattern = "attribute";
            return IsAttribute(line, pattern) ? null : "IsAttribute_Simple";
        }

        private string IsAttributeTest_First() {
            string line = "  [ question(answer = 42) , test ]  ";
            string pattern = "question";
            return IsAttribute(line, pattern) ? null : "IsAttribute_First";
        }

        private string IsAttributeTest_Second() {
            string line = "  [ question(answer = 42) , test ]  ";
            string pattern = "test";
            return IsAttribute(line, pattern) ? null : "IsAttribute_Second";
        }

        private string IsAttributeTest_None() {
            string line = "  [ question(answer = 42), test ]  ";
            string pattern = "none";
            return IsAttribute(line, pattern) ? "IsAttribute_None" : null;
        }

        private string IsAttributeTest_Comment() {
            string line = "  [ question(answer = 42), test ] // Comment";
            string pattern = "question";
            return IsAttribute(line, pattern) ? null : "IsAttribute_Comment";
        }

        // Line has attribute
        private string LineHasAttributeTest_True() {
            string[] lines = new string[] {
                "",
                "    [test]",
                "    [additional(info = 37)]",
                "    public SoundEffect Boom(int radius) {"
            };
            return LineHasAttribute(lines, 3, "test") ? null : "LineHasAttribute_True";
        }

        private string LineHasAttributeTest_False() {
            string[] lines = new string[] {
                "",
                "    [test]",
                "    [additional(info = 37)]",
                "    public SoundEffect Boom(int radius) {"
            };
            return LineHasAttribute(lines, 3, "none") ? "LineHasAttribute_True" : null;
        }

        // Get pattern match
        private string GetPatternMatchTest() {
            string line = "    public class Thingy";
            string result = GetPatternMatch(line, @"public class (\w+)");
            return result == "Thingy" ? null : "GetPatternMatch";
        }

        private string GetPatternMatchesTest() {
            string line = "    public virtual string Thingy";
            string[] result = GetPatternMatches(line, @"public virtual (\w+) (\w+)");
            return (result.Length == 2 && result[0] == "string" && result[1] == "Thingy") ? null : "GetPatternMatches";
        }
    }
}
