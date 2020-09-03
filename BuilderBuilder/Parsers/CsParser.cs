using System.Collections.Generic;

namespace BuilderBuilder
{
    public abstract class CsParser : Parser
    {
        private const string ATTRIBUTE_INSIDE = @"\s*[a-zA-Z_].*";

        private string TYPE => @"[a-zA-Z0-9_<>?]";

        protected bool IsAttributeLine(string line) {
            return MatchesPattern(line, $@"^\s*\[{ATTRIBUTE_INSIDE}\]");
        }

        protected bool IsAttribute(string line, string attribute) {
            return MatchesPattern(line, $@"^\s*\[({ATTRIBUTE_INSIDE},\s*)*\s*{attribute}\s*(\(.*\))?\s*(\s*,{ATTRIBUTE_INSIDE})*\]");
        }

        protected bool LineHasAttribute(string[] lines, int index, string attribute) {
            for (int i = index - 1; i >= 0; i--) {
                string line = lines[i];
                if (!IsAttributeLine(line)) {
                    return false;
                }

                if (IsAttribute(line, attribute)) {
                    return true;
                }
            }
            return false;
        }

        protected (string type, string name)? ParsePublicField(string line) {
            return ParsePublicField(line, false);
        }

        protected (string type, string name)? ParsePublicVirtualField(string line) {
            return ParsePublicField(line, true);
        }

        private (string type, string name)? ParsePublicField(string line, bool forceVirtual) {
            string fieldPattern = @"public\s+" +
                (forceVirtual ? @"virtual\s+" : @"(?:virtual\s+|override\s+)?") +
                $@"({TYPE}+)\s+(\w+)\s*" +
                @"(\()?" +
                @"(?:\{\s*get;\s*set;\s*\})?";

            if (MatchesPattern(line, fieldPattern)) {
                string[] values = GetPatternMatches(line, fieldPattern);
                if (values[0] == "class" || (values.Length >= 3 && values[2] == "(")) {
                    return null;
                }
                return (values[0], values[1]);
            }
            return null;
        }

        public string ParseConstructor(string[] lines, int index, string name) {
            string startPattern = $@"public\s+{name}\s*\(";
            string firstParamsPattern = $@"public\s+{name}\s*\(([^)]*)";
            string paramsPattern = @"([^)]*)";
            string endPattern = @"\)";

            if (MatchesPattern(lines[index], startPattern)) {
                string result = MatchesPattern(lines[index], firstParamsPattern) ? GetPatternMatch(lines[index], firstParamsPattern) : "";
                if (MatchesPattern(lines[index], endPattern)) {
                    return result;
                }

                for (int i = index + 1; i < lines.Length; i++) {
                    string line = lines[i];
                    result += MatchesPattern(line, paramsPattern) ? GetPatternMatch(line, paramsPattern) : "";
                    if (MatchesPattern(line, endPattern)) {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
