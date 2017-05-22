namespace BuilderBuilder
{
    abstract class CsParser : Parser
    {
        private const string ATTRIBUTE_INSIDE = @"\s*[a-zA-Z_].*";

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
    }
}
