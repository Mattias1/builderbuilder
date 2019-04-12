namespace BuilderBuilder
{
    public class PlainCsClassParser : CsParser
    {
        private BuilderEntity _result;

        public override BuilderEntity Parse(string[] lines) {
            _result = new BuilderEntity(persistable: false);

            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i];

                parseName(lines, i, line);
                parseField(lines, i, line);
            }

            return _result;
        }

        private void parseName(string[] lines, int i, string line) {
            const string classPattern = @"^\s*(?:public\s+)?class\s+(\w+)";

            if (MatchesPattern(line, classPattern)) {
                _result.Name = GetPatternMatch(line, classPattern);
            }
        }

        private void parseField(string[] lines, int i, string line) {
            var field = ParsePublicField(line);
            if (field != null) {
                _result.Fields.Add(new Field(field.Value.type, field.Value.name));
            }
        }
    }
}
