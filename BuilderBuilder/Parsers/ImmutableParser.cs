namespace BuilderBuilder.Parsers;

public class ImmutableParser : CsParser
{
    private BuilderEntity _result;
    private List<string[]> _parametersOfConstructors;

    public override BuilderEntity Parse(string[] lines) {
        _result = new BuilderEntity(persistable: false);
        _parametersOfConstructors = new List<string[]>();

        for (var i = 0; i < lines.Length; i++) {
            var line = lines[i];

            parseName(lines, i, line);
            parseConstructor(lines, i, line);
        }

        var parameters = getLastConstructorWithMostParameters();
        addFields(parameters);

        return _result;
    }

    private void parseName(string[] lines, int i, string line) {
        const string classOrStuctPattern = @"^\s*(?:public\s+)?(?:readonly\s+)?(?:class|struct)\s+(\w+)";

        if (MatchesPattern(line, classOrStuctPattern)) {
            _result.Name = GetPatternMatch(line, classOrStuctPattern);
        }
    }

    private void parseConstructor(string[] lines, int i, string line) {
        var constructorParameters = ParseConstructor(lines, i, _result.Name);
        if (constructorParameters != null) {
            var parameters = constructorParameters.Split(',').Select(p => p.Trim()).ToArray();
            _parametersOfConstructors.Add(parameters);
        }
    }

    private string[] getLastConstructorWithMostParameters() {
        var mostParameters = 0;
        var result = new string[0];
        foreach (var parameters in _parametersOfConstructors) {
            if (parameters.Length >= mostParameters) {
                mostParameters = parameters.Length;
                result = parameters;
            }
        }
        return result;
    }

    private void addFields(string[] constructorParameters) {
        foreach (var bothParameters in constructorParameters) {
            var split = bothParameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 2) {
                continue;
            }
            _result.Fields.Add(new Field(split[0], UpperFirst(split[1])));
        }
    }

    private static string UpperFirst(string s) => char.ToUpper(s[0]) + s.Substring(1);
}