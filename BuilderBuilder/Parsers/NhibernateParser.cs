namespace BuilderBuilder
{
    class NhibernateParser : CsParser
    {
        private BuilderEntity _result;

        public override BuilderEntity Parse(string[] lines) {
            _result = new BuilderEntity();

            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i];

                parseName(lines, i, line);
                parseField(lines, i, line);
            }

            return _result;
        }

        private void parseName(string[] lines, int i, string line) {
            const string classPattern = @"^\s*public\s+class\s+(\w+)";

            if (MatchesPattern(line, classPattern) && LineHasAttribute(lines, i, "Class")) {
                _result.Name = GetPatternMatch(line, classPattern);
            }
        }

        private void parseField(string[] lines, int i, string line) {
            const string fieldPattern = @"public\s+virtual\s+(\w+)\s+(\w+)\s*\{\s*get;\s*set;\s*\}";

            if (MatchesPattern(line, fieldPattern) && LineHasAttribute(lines, i, "Property")) {
                string[] values = GetPatternMatches(line, fieldPattern);
                _result.Fields.Add(new Field(values[0], values[1]));
            }
        }
    }
}
